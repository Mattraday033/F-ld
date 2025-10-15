using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MovementManager : MonoBehaviour
{
	public static Vector3Int distance1TileNorthEastGrid = new Vector3Int(1, 0, 0);
	public static Vector3Int distance1TileNorthWestGrid = new Vector3Int(0, 1, 0);
	public static Vector3Int distance1TileSouthWestGrid = new Vector3Int(-1, 0, 0);
	public static Vector3Int distance1TileSouthEastGrid = new Vector3Int(0, -1, 0);

	public static Vector3Int[] allDirectionVectors = new Vector3Int[]{distance1TileNorthEastGrid,
																	  distance1TileNorthWestGrid,
																	  distance1TileSouthWestGrid,
																	  distance1TileSouthEastGrid};

	public static Vector3Int[] directionMod;

	public static MovementManager instance;

	private ArrayList floorButtons = new ArrayList();
	public Dictionary<IButtonEvaluationScript, FloorButtonTrueFalse[]> buttonEvaluators = new Dictionary<IButtonEvaluationScript, FloorButtonTrueFalse[]>();

	public Grid grid;

	//public bool isMoving = false;
	public bool smallWaitAfterMoving = false;

	public static int firstEnemyIndex = 1;
	public static int framesWaited = 0;
	public static float framesToWait = 15;

	public static Transform[] allSpritesToMove = new Transform[1];
	public static bool[] isSpriteMoveableObject = new bool[1];
	//An array may be faster, which might be important later when worried about
	//performance and animation/sprites. Call also adjust timeToMove

	public static Vector3[] startingPositions = new Vector3[1];
	public static Vector3[] endingPositions = new Vector3[1];
	public static bool[] isMoving = new bool[1];

	public int adjacentMonsterIndex = -1;

	private bool neverMoved = true;

	private float timeToMove = .2f;

	public const int playerSpriteIndex = 0;

	public static MovementManager getInstance()
	{
		return instance;
	}

    public static Grid getGrid()
    {
        return instance.grid;
    }

	private void Awake()
	{
		if (instance != null)
		{
			throw new IOException("There is more than 1 instance of MovementManager");
		}

		instance = this;
	}

	void Start()
	{
		updateArrays();

		PartyMemberMovement.instantiatePartyMemberTrain();

		StartCoroutine(checkButtonsAfterStartMethods());
	}

	public void addFloorButton(IFloorButton button)
	{
		floorButtons.Add(button);
	}

	public bool isBetweenTiles(int spriteID)
	{
		if (neverMoved)
		{
			return false;
		}

		return (allSpritesToMove[spriteID].localPosition.x < startingPositions[spriteID].x && allSpritesToMove[spriteID].localPosition.x > endingPositions[spriteID].x) ||
				(allSpritesToMove[spriteID].localPosition.x > startingPositions[spriteID].x && allSpritesToMove[spriteID].localPosition.x < endingPositions[spriteID].x) ||
				(allSpritesToMove[spriteID].localPosition.y < startingPositions[spriteID].y && allSpritesToMove[spriteID].localPosition.y > endingPositions[spriteID].y) ||
				(allSpritesToMove[spriteID].localPosition.y > startingPositions[spriteID].y && allSpritesToMove[spriteID].localPosition.y < endingPositions[spriteID].y) ||
				 isMoving.Contains(true);

	}

	private void updateAllMonsterPackCurrentPositions()
	{
		if (State.currentMonsterPackList == null)
		{
			return;
		}

		MonsterPack[] monsterPacks = State.currentMonsterPackList.monsterPacks;

		int endingPositionIndex = firstEnemyIndex;
		for (int monsterPackIndex = (endingPositionIndex - 1);
			monsterPackIndex < monsterPacks.Length && endingPositionIndex < endingPositions.Length;
			monsterPackIndex++)
		{
			monsterPacks[monsterPackIndex].currentPosition = endingPositions[endingPositionIndex];
			endingPositionIndex++;
		}
	}

	//something keeps setting sprites' Z position to 25.5 and this messes with positioning. 
	//this sets them all back to 0 before doing movement stuff
	private void setAllZPositionsToZero()
	{
		foreach (Transform sprite in allSpritesToMove)
		{
			if (sprite == null || sprite is null)
			{
				continue;
			}

			Vector3 newLocalPosition = sprite.localPosition;
			newLocalPosition.z = 0;
			sprite.localPosition = newLocalPosition;
		}
	}

	public void moveAllSprites(Vector3Int playerDirection)
	{
		setAllZPositionsToZero();

		directionMod[playerSpriteIndex] = playerDirection;

		if (isBetweenTiles(playerSpriteIndex))
		{
			return;
		}

		for (int i = 0; i < allSpritesToMove.Length; i++)
		{
			if (allSpritesToMove[i] == null)
			{
				continue;
			}

			Vector3Int coords = grid.LocalToCell(allSpritesToMove[i].localPosition);

			if (!isMoving[i])
			{
				directionMod[i] = determineDirection(i, coords);

				startingPositions[i] = allSpritesToMove[i].localPosition;
				endingPositions[i] = grid.GetCellCenterLocal(coords + directionMod[i]);
			}
		}

		preventCollidingEndingPositions();

		adjacentMonsterIndex = checkIfPlayerEndsAdjacentToAnyMonsterStart();

		if (adjacentMonsterIndex >= 0)
		{
			endingPositions[adjacentMonsterIndex] = startingPositions[adjacentMonsterIndex];
		}
		else
		{
			adjacentMonsterIndex = checkIfPlayerAndMonstersEndAdjacent();
		}

		//movement loop
		for (int i = 0; i < allSpritesToMove.Length; i++)
		{
			if (allSpritesToMove[i] == null)
			{
				continue;
			}

			if (!isMoving[i])
			{
				isMoving[i] = true;
				neverMoved = false;

				if (i > playerSpriteIndex && endingPositions[i] != startingPositions[i])
				{
					setEnemyFacing(directionMod[i], allSpritesToMove[i].GetComponent<EnemyMovement>());
				}

				StartCoroutine(moveSprite(i));

				if (!playerDirection.Equals(Vector3Int.zero))
				{
					PartyMemberMovement.showAllPartyMembers();
					movePartyMembers(0, grid.CellToLocal(getFirstPartyMemberEndingPosition()));
				}
			}
		}

		if (adjacentMonsterIndex >= 0)
		{
			StartCoroutine(prepCombatAfterMovesFinish());
		}

		updateAllMonsterPackCurrentPositions();
		smallWaitAfterMoving = true;

		changeFooting();
	}

	public static bool onLeftFoot()
	{
		return State.onLeftFoot;
	}

	public static void changeFooting()
	{
		StepCountScriptManager.incrementStepCount();

		State.onLeftFoot = !State.onLeftFoot;
		OOCUIManager.getInstance().updateFooting();
	}

	public static void setFooting(bool onLeftFoot)
	{
		State.onLeftFoot = onLeftFoot;

		if (OOCUIManager.getInstance() != null)
		{
			OOCUIManager.getInstance().updateFooting();
		}
	}

	private IEnumerator checkButtonsAfterStartMethods()
	{
		yield return null;

		evaluateAllButtonScripts();

		yield break;
	}

	public void evaluateAllButtonScripts()
	{
		foreach (IFloorButton button in floorButtons)
		{
			button.evaluate();
		}

		foreach (KeyValuePair<IButtonEvaluationScript, FloorButtonTrueFalse[]> kvp in buttonEvaluators)
		{
			kvp.Key.evaluate(kvp.Value);
		}
	}

	private Vector3Int determineDirection(int spriteIndex, Vector3Int coords)
	{
		if (spriteIndex > 0)
		{
			if (allSpritesToMove[spriteIndex].GetComponent<EnemyMovement>().moveableObject &&
				  (grid.LocalToCell(endingPositions[0]) == coords))
			{
				return directionMod[playerSpriteIndex];
			}
			else if (allSpritesToMove[spriteIndex].GetComponent<EnemyMovement>().moveableObject)
			{
				return Vector3Int.zero;
			}
			else
			{
				EnemyMovement enemyMovement = allSpritesToMove[spriteIndex].GetComponent<EnemyMovement>();
				Vector3Int enemyDirection = enemyMovement.findDirection();
				return enemyDirection;
			}
		}
		else
		{
			return directionMod[playerSpriteIndex];
		}
	}

	private void setEnemyFacing(Vector3Int directionMod, EnemyMovement enemyMovement)
	{
		if (directionMod.Equals(Vector3Int.zero))
		{
			return;
		}

		if (directionMod.Equals(distance1TileNorthEastGrid))
		{
			enemyMovement.setEnemyFacing(Facing.NorthEast);

		}
		else if (directionMod.Equals(distance1TileSouthEastGrid))
		{
			enemyMovement.setEnemyFacing(Facing.SouthEast);

		}
		else if (directionMod.Equals(distance1TileSouthWestGrid))
		{
			enemyMovement.setEnemyFacing(Facing.SouthWest);

		}
		else if (directionMod.Equals(distance1TileNorthWestGrid))
		{
			enemyMovement.setEnemyFacing(Facing.NorthWest);
		}
		else
		{
			throw new IOException("Illegal directionMod: " + directionMod.ToString());
		}
	}

    public void addPlayerSprite(Transform player)
    {
        allSpritesToMove[0] = player;
        updateArrays();
	}

	public void addEnemySprite(Transform enemy, int index)
	{
		if (index >= allSpritesToMove.Length)
		{
			Transform[] newAllSpritesToMove = new Transform[index + 1];

			for (int i = 0; i < allSpritesToMove.Length; i++)
			{
				newAllSpritesToMove[i] = allSpritesToMove[i];
			}

			newAllSpritesToMove[index] = enemy;

			allSpritesToMove = newAllSpritesToMove;
		}
		else
		{
			allSpritesToMove[index] = enemy;
		}

		updateArrays();
	}

	public static void removeEnemySprite(int indexToRemove)
	{

		allSpritesToMove[indexToRemove] = null;

	}

	private IEnumerator prepCombatAfterMovesFinish()
	{
		while (isMoving[playerSpriteIndex])
		{
			yield return null;
		}

		if (allSpritesToMove[adjacentMonsterIndex] != null)
		{
			allSpritesToMove[adjacentMonsterIndex].GetComponent<EnemyMovement>().prepCombat(adjacentMonsterIndex);
		}


		yield break;
	}

	private IEnumerator moveSprite(int spriteID)
	{
		float elapsedTime = 0;

		while (elapsedTime <= timeToMove)
		{
			allSpritesToMove[spriteID].localPosition = Vector3.Lerp(startingPositions[spriteID], endingPositions[spriteID], (elapsedTime / timeToMove));
			elapsedTime += Time.deltaTime;
			//Helpers.updateGameObjectPosition(allSpritesToMove[spriteID]);
			yield return null;
		}

		if (allSpritesToMove[spriteID] != null)
		{
			allSpritesToMove[spriteID].localPosition = endingPositions[spriteID];
			startingPositions[spriteID] = allSpritesToMove[spriteID].localPosition;
		}

		//Helpers.updateColliderPosition(allSpritesToMove[spriteID]);

		PartyMemberMovement.hideOverlappingPartyMembers();

		isMoving[spriteID] = false;
		/*
		for(int j = 1; j < allSpritesToMove.Length; j++)
		{
			if(allSpritesToMove[j] != null && !allSpritesToMove[j].GetComponent<EnemyMovement>().moveableObject && allSpritesToMove[j].GetComponent<EnemyMovement>().checkForPlayer(j) )
			{
				yield break;
			}
		}*/
		evaluateAllButtonScripts();
		yield break;
	}

	private IEnumerator moveSprite(Transform spriteToMove, Vector3 startingPosition, Vector3 endingPosition)
	{
		float elapsedTime = 0;

		while (elapsedTime <= timeToMove)
		{
			spriteToMove.localPosition = Vector3.Lerp(startingPosition, endingPosition, (elapsedTime / timeToMove));
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		PartyMemberMovement.hideOverlappingPartyMembers();

		if (spriteToMove != null)
		{
			spriteToMove.localPosition = endingPosition;
		}

		evaluateAllButtonScripts();

		yield break;
		//Helpers.updateColliderPosition(allSpritesToMove[spriteID]);

	}

	public void updateArrays()
	{
		directionMod = new Vector3Int[allSpritesToMove.Length];
		startingPositions = new Vector3[allSpritesToMove.Length];
		endingPositions = new Vector3[allSpritesToMove.Length];
		isSpriteMoveableObject = new bool[allSpritesToMove.Length];
		isMoving = new bool[allSpritesToMove.Length];

		for (int i = 0; i < allSpritesToMove.Length; i++)
		{
			directionMod[i] = new Vector3Int(0, 0, 0);

			if (allSpritesToMove[i] != null)
			{
				startingPositions[i] = allSpritesToMove[i].localPosition;
				endingPositions[i] = allSpritesToMove[i].localPosition;
			}

			isMoving[i] = false;

			if (i == playerSpriteIndex || allSpritesToMove[i] == null || allSpritesToMove[i] is null)
			{
				continue;
			}
			else
			{
				isSpriteMoveableObject[i] = allSpritesToMove[i].GetComponent<EnemyMovement>().moveableObject;
			}
		}
	}

	private void preventCollidingEndingPositions()
	{

		for (int i = playerSpriteIndex + 1; i < endingPositions.Length; i++)
		{

			if (allSpritesToMove[i] == null)
			{
				continue;
			}

			Vector3Int currentMonstersCellCoords = grid.LocalToCell(new Vector3(endingPositions[i].x, endingPositions[i].y, 0f));

			for (int j = (i - 1); j >= 0; j--)
			{
				if (allSpritesToMove[j] == null)
				{
					continue;
				}

				Vector3Int previousMonstersCellCoords = grid.LocalToCell(new Vector3(endingPositions[j].x, endingPositions[j].y, 0f));

				if (currentMonstersCellCoords.Equals(previousMonstersCellCoords) &&
					(endingPositions[i].x != startingPositions[i].x &&
						endingPositions[i].y != startingPositions[i].y))
				{
					endingPositions[i] = startingPositions[i];
					i = playerSpriteIndex;  //restart loop
				}
			}
		}
	}

	private Vector3Int getFirstPartyMemberEndingPosition()
	{
		return grid.LocalToCell(startingPositions[playerSpriteIndex]);
	}

	public void movePartyMembers(int partyMemberIndex, Vector3 endingPosition)
	{

		if (PartyMemberMovement.partyMemberTrain == null)
		{
			return;
		}

		if (partyMemberIndex < PartyMemberMovement.partyMemberTrain.Length &&
			partyMemberIndex < (PartyMemberMovement.stepCounter))
		{
			Vector3 startingPosition = PartyMemberMovement.partyMemberTrain[partyMemberIndex].localPosition;

			StartCoroutine(moveSprite(PartyMemberMovement.partyMemberTrain[partyMemberIndex], startingPosition, endingPosition));

			endingPosition = startingPosition;

			movePartyMembers(partyMemberIndex + 1, endingPosition);
		}
		else
		{
			PartyMemberMovement.stepCounter++;
			return;
		}
	}

	private int checkIfPlayerEndsAdjacentToAnyMonsterStart()
	{
		return checkForPlayerAdjacentPositions(startingPositions);
	}

	private int checkIfPlayerAndMonstersEndAdjacent()
	{
		return checkForPlayerAdjacentPositions(endingPositions);
	}

	private int checkForPlayerAdjacentPositions(Vector3[] positions)
	{
		for (int positionIndex = 1; positionIndex < positions.Length; positionIndex++)
		{
			if (isSpriteMoveableObject[positionIndex])
			{
				continue;
			}

			if (cellsAreAdjacent(endingPositions[playerSpriteIndex], positions[positionIndex]))
			{
				return positionIndex;
			}
		}

		return -1;
	}

	public bool cellsAreAdjacent(Vector3 first, Vector3 second)
	{
		first.z = 0f;
		second.z = 0f;

		Vector3Int firstCellPosition = getCellLocal(first);
		Vector3Int secondCellPosition = getCellLocal(second);

		int xDistance = firstCellPosition.x - secondCellPosition.x;
		int yDistance = firstCellPosition.y - secondCellPosition.y;

		if (xDistance <= 1 && xDistance >= -1 && yDistance == 0)
		{
			return true;
		}

		if (yDistance <= 1 && yDistance >= -1 && xDistance == 0)
		{
			return true;
		}

		return false;
	}

	public Vector3Int getPlayerPosition()
	{
		return PlayerMovement.getInstance().getMovementGridCoordsLocal();
	}

	public static SurpriseState determineSurprisedParty()
	{
		MovementManager movementManager = MovementManager.getInstance();
		Grid grid = MovementManager.getInstance().grid;

		Vector3Int playerCell = grid.WorldToCell(new Vector3(State.playerPosition.x, State.playerPosition.y, 0f));
		Vector3Int enemyCell = grid.LocalToCell(new Vector3(State.enemyPosition.x, State.enemyPosition.y, 0f));

		PlayerDirectionFromEnemy playerDirectionFromEnemy;
		SurpriseState surpriseState;

		//Debug.LogError("playerCell = " + playerCell);
		//Debug.LogError("enemyCell = " + enemyCell);

		if (playerCell.x > enemyCell.x && playerCell.y == enemyCell.y)
		{
			playerDirectionFromEnemy = PlayerDirectionFromEnemy.NorthEast;

		}
		else if (playerCell.x == enemyCell.x && playerCell.y > enemyCell.y)
		{
			playerDirectionFromEnemy = PlayerDirectionFromEnemy.NorthWest;

		}
		else if (playerCell.x == enemyCell.x && playerCell.y < enemyCell.y)
		{
			playerDirectionFromEnemy = PlayerDirectionFromEnemy.SouthEast;
		}
		else if (playerCell.x < enemyCell.x && playerCell.y == enemyCell.y)
		{
			playerDirectionFromEnemy = PlayerDirectionFromEnemy.SouthWest;
		}
		else
		{
			Debug.LogError("Could not determine PlayerDirectionFromEnemy:");
			Debug.LogError("playerCell = " + playerCell);
			Debug.LogError("enemyCell = " + enemyCell);

			return SurpriseState.NoOneSurprised;
		}

		/*
		Debug.LogError("playerDirectionFromEnemy = " + Helpers.enumToString(playerDirectionFromEnemy));

		Debug.LogError("Player is facing " + Helpers.enumToString(State.playerFacing.getFacing()));
		Debug.LogError("Enemy is facing " + Helpers.enumToString(State.enemyFacing.getFacing()));

		Debug.LogError("State.playerFacing.getFacing() == Facing.NorthWest = " + State.playerFacing.getFacing() == Facing.NorthWest);
		Debug.LogError("State.playerFacing.getFacing() == Facing.SouthWest = " + State.playerFacing.getFacing() == Facing.SouthWest);
		Debug.LogError("State.playerFacing.getFacing() == Facing.NorthEast = " + State.playerFacing.getFacing() == Facing.NorthEast);
		Debug.LogError("State.playerFacing.getFacing() == Facing.SouthEast = " + State.playerFacing.getFacing() == Facing.SouthEast);

		Debug.LogError("State.enemyFacing.getFacing() == Facing.NorthWest = " + State.enemyFacing.getFacing() == Facing.NorthWest);
		Debug.LogError("State.enemyFacing.getFacing() == Facing.SouthWest = " + State.enemyFacing.getFacing() == Facing.SouthWest);
		Debug.LogError("State.enemyFacing.getFacing() == Facing.NorthEast = " + State.enemyFacing.getFacing() == Facing.NorthEast);
		Debug.LogError("State.enemyFacing.getFacing() == Facing.SouthEast = " + State.enemyFacing.getFacing() == Facing.SouthEast);
		*/
		if (playerDirectionFromEnemy == PlayerDirectionFromEnemy.NorthEast)
		{
			if ((State.playerFacing.getFacing() == Facing.SouthWest && State.enemyFacing.getFacing() == Facing.SouthWest) ||
			   (State.playerFacing.getFacing() == Facing.SouthWest && State.enemyFacing.getFacing() == Facing.SouthEast) ||
			   (State.playerFacing.getFacing() == Facing.SouthWest && State.enemyFacing.getFacing() == Facing.NorthWest) ||
			   (State.playerFacing.getFacing() == Facing.NorthWest && State.enemyFacing.getFacing() == Facing.SouthWest) ||
			   (State.playerFacing.getFacing() == Facing.SouthEast && State.enemyFacing.getFacing() == Facing.SouthWest))
			{
				surpriseState = SurpriseState.EnemySurprised;

			}
			else if ((State.playerFacing.getFacing() == Facing.NorthEast && State.enemyFacing.getFacing() == Facing.NorthEast) ||
					  (State.playerFacing.getFacing() == Facing.NorthEast && State.enemyFacing.getFacing() == Facing.NorthWest) ||
					  (State.playerFacing.getFacing() == Facing.NorthEast && State.enemyFacing.getFacing() == Facing.SouthWest) ||
					  (State.playerFacing.getFacing() == Facing.NorthWest && State.enemyFacing.getFacing() == Facing.NorthEast) ||
					  (State.playerFacing.getFacing() == Facing.SouthEast && State.enemyFacing.getFacing() == Facing.NorthEast))
			{
				surpriseState = SurpriseState.PlayerSurprised;

			}
			else
			{
				surpriseState = SurpriseState.NoOneSurprised;
			}
		}
		else if (playerDirectionFromEnemy == PlayerDirectionFromEnemy.NorthWest)
		{
			if ((State.playerFacing.getFacing() == Facing.SouthEast && State.enemyFacing.getFacing() == Facing.SouthEast) ||
				(State.playerFacing.getFacing() == Facing.SouthEast && State.enemyFacing.getFacing() == Facing.NorthEast) ||
				(State.playerFacing.getFacing() == Facing.SouthEast && State.enemyFacing.getFacing() == Facing.SouthWest) ||
				(State.playerFacing.getFacing() == Facing.NorthEast && State.enemyFacing.getFacing() == Facing.SouthEast) ||
				(State.playerFacing.getFacing() == Facing.SouthWest && State.enemyFacing.getFacing() == Facing.SouthEast))
			{
				surpriseState = SurpriseState.EnemySurprised;

			}
			else if ((State.playerFacing.getFacing() == Facing.NorthWest && State.enemyFacing.getFacing() == Facing.NorthWest) ||
					 (State.playerFacing.getFacing() == Facing.NorthWest && State.enemyFacing.getFacing() == Facing.NorthEast) ||
					 (State.playerFacing.getFacing() == Facing.NorthWest && State.enemyFacing.getFacing() == Facing.SouthWest) ||
					 (State.playerFacing.getFacing() == Facing.NorthEast && State.enemyFacing.getFacing() == Facing.NorthWest) ||
					 (State.playerFacing.getFacing() == Facing.SouthWest && State.enemyFacing.getFacing() == Facing.NorthWest))
			{
				surpriseState = SurpriseState.PlayerSurprised;
			}
			else
			{
				surpriseState = SurpriseState.NoOneSurprised;
			}
		}
		else if (playerDirectionFromEnemy == PlayerDirectionFromEnemy.SouthWest)
		{
			if ((State.playerFacing.getFacing() == Facing.NorthEast && State.enemyFacing.getFacing() == Facing.NorthEast) ||
				(State.playerFacing.getFacing() == Facing.NorthEast && State.enemyFacing.getFacing() == Facing.NorthWest) ||
				(State.playerFacing.getFacing() == Facing.NorthEast && State.enemyFacing.getFacing() == Facing.SouthEast) ||
				(State.playerFacing.getFacing() == Facing.NorthWest && State.enemyFacing.getFacing() == Facing.NorthEast) ||
				(State.playerFacing.getFacing() == Facing.SouthEast && State.enemyFacing.getFacing() == Facing.NorthEast))
			{
				surpriseState = SurpriseState.EnemySurprised;

			}
			else if ((State.playerFacing.getFacing() == Facing.SouthWest && State.enemyFacing.getFacing() == Facing.SouthWest) ||
					 (State.playerFacing.getFacing() == Facing.SouthWest && State.enemyFacing.getFacing() == Facing.NorthWest) ||
					 (State.playerFacing.getFacing() == Facing.SouthWest && State.enemyFacing.getFacing() == Facing.SouthEast) ||
					 (State.playerFacing.getFacing() == Facing.NorthWest && State.enemyFacing.getFacing() == Facing.SouthWest) ||
					 (State.playerFacing.getFacing() == Facing.SouthEast && State.enemyFacing.getFacing() == Facing.SouthWest))
			{
				surpriseState = SurpriseState.PlayerSurprised;

			}
			else
			{
				surpriseState = SurpriseState.NoOneSurprised;
			}
		}
		else if (playerDirectionFromEnemy == PlayerDirectionFromEnemy.SouthEast)
		{
			if ((State.playerFacing.getFacing() == Facing.NorthWest && State.enemyFacing.getFacing() == Facing.NorthWest) ||
				(State.playerFacing.getFacing() == Facing.NorthWest && State.enemyFacing.getFacing() == Facing.NorthEast) ||
				(State.playerFacing.getFacing() == Facing.NorthWest && State.enemyFacing.getFacing() == Facing.SouthWest) ||
				(State.playerFacing.getFacing() == Facing.NorthEast && State.enemyFacing.getFacing() == Facing.NorthWest) ||
				(State.playerFacing.getFacing() == Facing.SouthWest && State.enemyFacing.getFacing() == Facing.NorthWest))
			{
				surpriseState = SurpriseState.EnemySurprised;

			}
			else if ((State.playerFacing.getFacing() == Facing.SouthEast && State.enemyFacing.getFacing() == Facing.SouthEast) ||
					 (State.playerFacing.getFacing() == Facing.SouthEast && State.enemyFacing.getFacing() == Facing.NorthEast) ||
					 (State.playerFacing.getFacing() == Facing.SouthEast && State.enemyFacing.getFacing() == Facing.SouthWest) ||
					 (State.playerFacing.getFacing() == Facing.NorthEast && State.enemyFacing.getFacing() == Facing.SouthEast) ||
					 (State.playerFacing.getFacing() == Facing.SouthWest && State.enemyFacing.getFacing() == Facing.SouthEast))
			{
				surpriseState = SurpriseState.PlayerSurprised;

			}
			else
			{
				surpriseState = SurpriseState.NoOneSurprised;
			}
		}
		else
		{
			throw new IOException("Unable to determine who is surprised.");
		}

		//Debug.LogError("surpriseState = " + Helpers.enumToString(surpriseState));

		return surpriseState;
	}

	public static Vector3Int getCellWorld(Vector3 position)
	{
		return getInstance().grid.WorldToCell(position);
	}

	public static Vector3Int getCellLocal(Vector3 localPosition)
	{
		return getInstance().grid.LocalToCell(localPosition);
	}

	public static List<Vector3Int> getAllCurrentSpriteCells()
	{
		List<Vector3Int> currentSpriteCells = new List<Vector3Int>();		
		
		for(int i = 0; i < allSpritesToMove.Length; i++)
		{
			if(allSpritesToMove[i] != null)
			{
				currentSpriteCells.Add(instance.grid.LocalToCell(endingPositions[i]));
			}
		}
		
		return currentSpriteCells;
	}
}
