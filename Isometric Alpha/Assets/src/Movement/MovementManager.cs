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
        return AreaManager.getMasterGrid();
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

		return (allSpritesToMove[spriteID].position.x < startingPositions[spriteID].x && allSpritesToMove[spriteID].position.x > endingPositions[spriteID].x) ||
				(allSpritesToMove[spriteID].position.x > startingPositions[spriteID].x && allSpritesToMove[spriteID].position.x < endingPositions[spriteID].x) ||
				(allSpritesToMove[spriteID].position.y < startingPositions[spriteID].y && allSpritesToMove[spriteID].position.y > endingPositions[spriteID].y) ||
				(allSpritesToMove[spriteID].position.y > startingPositions[spriteID].y && allSpritesToMove[spriteID].position.y < endingPositions[spriteID].y) ||
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

			Vector3 newposition = sprite.position;
			newposition.z = 0;
			sprite.position = newposition;
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

			Vector3Int coords = grid.WorldToCell(allSpritesToMove[i].position);

			if (!isMoving[i])
			{
				directionMod[i] = determineDirection(i, coords);

				startingPositions[i] = allSpritesToMove[i].position;
				endingPositions[i] = grid.GetCellCenterWorld(coords + directionMod[i]);
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
					movePartyMembers(0, grid.CellToWorld(getFirstPartyMemberEndingPosition()));
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
				  (grid.WorldToCell(endingPositions[0]) == coords))
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

    public static bool colliderInCell(Vector3Int cellCoords, LayerMask layerMask)
    {
        Vector3 worldPosition = AreaManager.getMasterGrid().CellToWorld(cellCoords);

        return Physics2D.OverlapCircle(worldPosition, .05f, layerMask);
    }

    public static Vector3 getColliderInCellPosition(Vector3Int cellCoords)
    {
        return AreaManager.getMasterGrid().CellToWorld(cellCoords);
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
			allSpritesToMove[spriteID].position = Vector3.Lerp(startingPositions[spriteID], endingPositions[spriteID], (elapsedTime / timeToMove));
			elapsedTime += Time.deltaTime;
			//Helpers.updateGameObjectPosition(allSpritesToMove[spriteID]);
			yield return null;
		}

		if (allSpritesToMove[spriteID] != null)
		{
			allSpritesToMove[spriteID].position = endingPositions[spriteID];
			startingPositions[spriteID] = allSpritesToMove[spriteID].position;
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
			spriteToMove.position = Vector3.Lerp(startingPosition, endingPosition, (elapsedTime / timeToMove));
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		PartyMemberMovement.hideOverlappingPartyMembers();

		if (spriteToMove != null)
		{
			spriteToMove.position = endingPosition;
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
				startingPositions[i] = allSpritesToMove[i].position;
				endingPositions[i] = allSpritesToMove[i].position;
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

			Vector3Int currentMonstersCellCoords = grid.WorldToCell(new Vector3(endingPositions[i].x, endingPositions[i].y, 0f));

			for (int j = (i - 1); j >= 0; j--)
			{
				if (allSpritesToMove[j] == null)
				{
					continue;
				}

				Vector3Int previousMonstersCellCoords = grid.WorldToCell(new Vector3(endingPositions[j].x, endingPositions[j].y, 0f));

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
		return grid.WorldToCell(startingPositions[playerSpriteIndex]);
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
			Vector3 startingPosition = PartyMemberMovement.partyMemberTrain[partyMemberIndex].position;

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

		Vector3Int firstCellPosition = getCellWorld(first);
		Vector3Int secondCellPosition = getCellWorld(second);

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

    public static PositionWrapper[] getAllMonsterLocations()
    {
        PositionWrapper[] positions = new PositionWrapper[startingPositions.Length - 1];
        
        for(int positionIndex = 0; positionIndex < positions.Length; positionIndex++)
        {
            positions[positionIndex] = new PositionWrapper(startingPositions[positionIndex + 1]);
        }

        return positions;
    }



    public static SurpriseState determineSurprisedParty(Vector3 playerPosition, Vector3 enemyPosition, Facing enemyFacing)
    {
        Grid grid = AreaManager.getMasterGrid();

        Vector3Int playerCell = grid.WorldToCell(new Vector3(playerPosition.x, playerPosition.y, 0f));
        Vector3Int enemyCell = grid.WorldToCell(new Vector3(enemyPosition.x, enemyPosition.y, 0f));

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

        if (playerDirectionFromEnemy == PlayerDirectionFromEnemy.NorthEast)
        {
            if ((State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthWest) ||
               (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthEast) ||
               (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.NorthWest) ||
               (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.SouthWest) ||
               (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthWest))
            {
                surpriseState = SurpriseState.EnemySurprised;

            }
            else if ((State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthEast) ||
                      (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthWest) ||
                      (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.SouthWest) ||
                      (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthEast) ||
                      (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.NorthEast))
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
            if ((State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthEast) ||
                (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.NorthEast) ||
                (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthWest) ||
                (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.SouthEast) ||
                (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthEast))
            {
                surpriseState = SurpriseState.EnemySurprised;

            }
            else if ((State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthWest) ||
                     (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthEast) ||
                     (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.SouthWest) ||
                     (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthWest) ||
                     (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.NorthWest))
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
            if ((State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthEast) ||
                (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthWest) ||
                (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.SouthEast) ||
                (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthEast) ||
                (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.NorthEast))
            {
                surpriseState = SurpriseState.EnemySurprised;

            }
            else if ((State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthWest) ||
                     (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.NorthWest) ||
                     (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthEast) ||
                     (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.SouthWest) ||
                     (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthWest))
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
            if ((State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthWest) ||
                (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.NorthEast) ||
                (State.playerFacing.getFacing() == Facing.NorthWest && enemyFacing == Facing.SouthWest) ||
                (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.NorthWest) ||
                (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.NorthWest))
            {
                surpriseState = SurpriseState.EnemySurprised;

            }
            else if ((State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthEast) ||
                     (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.NorthEast) ||
                     (State.playerFacing.getFacing() == Facing.SouthEast && enemyFacing == Facing.SouthWest) ||
                     (State.playerFacing.getFacing() == Facing.NorthEast && enemyFacing == Facing.SouthEast) ||
                     (State.playerFacing.getFacing() == Facing.SouthWest && enemyFacing == Facing.SouthEast))
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

	public static List<Vector3Int> getAllCurrentSpriteCells()
	{
		List<Vector3Int> currentSpriteCells = new List<Vector3Int>();		
		
		for(int i = 0; i < allSpritesToMove.Length; i++)
		{
			if(allSpritesToMove[i] != null)
			{
				currentSpriteCells.Add(instance.grid.WorldToCell(endingPositions[i]));
			}
		}
		
		return currentSpriteCells;
	}
}
