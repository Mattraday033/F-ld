using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class PathSegment
{
	public static int counter = 0;
	public int ID;
	public Vector3Int coords;
	public PathSegment nextSegment;
	
	public PathSegment(Vector3Int coords, PathSegment nextSegment)
	{
		ID = counter;
		counter++;
		this.coords = coords;
		this.nextSegment = nextSegment;
	}
	
	public PathSegment(PathSegment segmentToCopy)
	{
		ID = counter;
		counter++;
		
		this.coords = new Vector3Int(segmentToCopy.coords.x, segmentToCopy.coords.y, segmentToCopy.coords.z);
		
		if(segmentToCopy.nextSegment != null)
		{
			this.nextSegment = new PathSegment(segmentToCopy.nextSegment);
		}
	}
	
	public PathSegment(Vector3Int coords)
	{
		ID = counter;
		counter++;
		this.coords = coords;
		this.nextSegment = null;
	}
}

public class PathToPlayer
{
	public const int maxLength = 1000;	
	
	public PathSegment firstSegment;
	public PathSegment lastSegment;
	public int length;
	
	public PathToPlayer(PathSegment firstSegment)
	{
		this.firstSegment = firstSegment;
		this.lastSegment = firstSegment;
		this.length = 1;
	}
	
	public PathToPlayer(PathSegment firstSegment, PathSegment lastSegment, int length)
	{
		this.firstSegment = firstSegment;
		this.lastSegment = lastSegment;
		this.length = length;
	}
	
	public void moveAlongPath()
	{
		firstSegment = firstSegment.nextSegment;
		length--;
	}
	
	public void printAll()
	{
		PathSegment segment = firstSegment;
		
		for(int index = 0; index <= length; index++)
		{
			Debug.Log("Current Segment = " + segment.coords.ToString());
			Debug.Log("Current Segment ID = " + segment.ID);
			if(segment.nextSegment == null || segment.nextSegment is null)
			{
				break;
			} else
			{
				segment = segment.nextSegment;
			}
		}
		
		Debug.Log("End of path");
	}
	
	public Vector3Int getDirection(Vector3Int currentCell)
	{
		if(Mathf.Abs(currentCell.x - firstSegment.coords.x) >  Mathf.Abs(currentCell.y - firstSegment.coords.y))
		{
			if(currentCell.x > firstSegment.coords.x)
			{
				return MovementManager.distance1TileSouthWestGrid;
			} else
			{
				return MovementManager.distance1TileNorthEastGrid;
			}
		} else
		{
			if(currentCell.y > firstSegment.coords.y)
			{
				return MovementManager.distance1TileSouthEastGrid;
			} else
			{
				return MovementManager.distance1TileNorthWestGrid;
			}
		}
	}
	
	public void setLastSegment(PathSegment segment)
	{
		PathSegment cloneBuffer = new PathSegment(segment);
		
		lastSegment.nextSegment = cloneBuffer;
		lastSegment = cloneBuffer;
		length++;
		//printAll();
	}
	
	public PathToPlayer clone()
	{
		PathSegment newFirstSegment = new PathSegment(firstSegment);
		PathSegment newLastSegment = newFirstSegment;
		
		while(newLastSegment.nextSegment != null)
		{
			newLastSegment = newLastSegment.nextSegment;
		}
		
		PathToPlayer clonePath = new PathToPlayer(newFirstSegment, newLastSegment, length);
		
		return clonePath;
	}
}

public class EnemyMovement : MonoBehaviour, ISkillTarget, IRevealable, ITutorialSequenceTarget, IDescribableInBlocks
{
	public enum playerDirection
	{
		NorthEast = 1, NorthWest = 2, SouthWest = 3, SouthEast = 4
	}
	public const string neDebugText = "NORTHEAST";
	public const string nwDebugText = "NORTHWEST";
	public const string seDebugText = "SOUTHEAST";
	public const string swDebugText = "SOUTHWEST";

	private static Color cunningStunnedColor = Color.red;
	private static Color intimidatedColor = Color.magenta;
	private static Color retreatStunnedColor = Color.cyan;

	public const int pathIndexHardCutoff = 1000;

    //[SerializeField]
    private int monsterPackIndex; //private so it can only set by using setMonsterPackIndex()

	public string tutorialHash;
	public string packName = "???"; //Worms

	public bool moveableObject;
	public bool followsPlayer = true;
	public bool movesEveryTurn = false;
	public bool neverMoves = false;

	public EnemyDirectionIndicator enemyDirectionIndicator;

	public int cunningStunCounter = 0;
	public int intimidateCounter = 0;
	public int retreatStunnedCounter = 0;

	public Facing startingFacing; //if monster is starting with the wrong startingFacing, check it's AllMonsterPackLists entry first
	public CharacterFacing enemyFacing = new CharacterFacing();

	private const int moveThreshold = 7;

	private float detectionSize = .05f;

	public Dictionary<Vector3Int, bool> dictionaryOfSegments = new Dictionary<Vector3Int, bool>();
	public ArrayList gizmosToDraw = new ArrayList();

	public GameObject hoverTarget;

	private void Awake()
	{
		spawnTargetCanvas();
	}

    public MovementManager getMovementManager()
    {
        return MovementManager.getInstance();
    }

    public EnemyPackInfo getEnemyPackInfo()
    {
        return EnemyPackInfoList.getEnemyPackInfo(AreaManager.locationName, monsterPackIndex);
    }

    private void OnDrawGizmos()
    {

        if (followsPlayer)
        {

            int coordsIndex = 0;
            foreach (Vector3Int coords in gizmosToDraw)
            {
                if (coordsIndex == 0)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawWireSphere(AreaManager.getMasterGrid().CellToWorld(coords), detectionSize);
                coordsIndex++;
            }
        }
        else
        {
            Gizmos.color = Color.red;

            foreach (Vector3Int direction in MovementManager.allDirectionVectors)
            {
                Gizmos.DrawWireSphere(MovementManager.getColliderInCellPosition(getCurrentCell() + direction), detectionSize);
            }
        }

    }

	public bool checkForPlayer(int monsterTransformIndex)
	{
		Vector3Int[] distanceModifiers = new Vector3Int[]{  MovementManager.distance1TileNorthEastGrid,
															MovementManager.distance1TileNorthWestGrid,
															MovementManager.distance1TileSouthWestGrid,
															MovementManager.distance1TileSouthEastGrid};

		foreach (Vector3Int distanceModifier in distanceModifiers)
		{
			if (MovementManager.colliderInCell(getCurrentCell() + distanceModifier, LayerAndTagManager.playerLayerMask))
			{
				if (!moveableObject && !stunnedFromRetreating())
				{
					prepCombat(monsterTransformIndex);
				}

				return true;
			}
		}

		return false;
	}

	public void prepCombat(int monsterTransformIndex)
	{
		State.enemyPackInfo = getEnemyPackInfo();
		CombatStateManager.currentMonsterPack = getMonsterPack();
        CombatStateManager.locationBeforeCombat = AreaManager.locationName;

		if (intimidated())
		{
			CombatStateManager.whoIsSurprised = SurpriseState.NoOneSurprised;
		}
		else
		{
			// CombatStateManager.whoIsSurprised = SurpriseState.PlayerSurprised;
			CombatStateManager.whoIsSurprised = MovementManager.determineSurprisedParty(PlayerMovement.getInstance().transform.position, transform.position, Facing.SouthEast);
		}

        SceneChange.changeSceneToCombat();
	}


	public Vector3Int findDirection()
	{
		decrementSkillCounters();

		if (stunnedFromCunning() || stunnedFromRetreating())
		{
			return Vector3Int.zero;
		}
		else if (neverMoves || (!movesEveryTurn && !MovementManager.onLeftFoot()))
		{
			return Vector3Int.zero;
		}

		if (!followsPlayer)
		{
			return findRandomDirection();
		}
		else
		{
			PathToPlayer pathToPlayer = findPathToPlayer();

			if (pathToPlayer == null || pathToPlayer is null)
			{
				//Debug.LogError("Couldn't Find Player");
				return Vector3Int.zero;
			}
			else
			{
				return pathToPlayer.getDirection(AreaManager.getMasterGrid().WorldToCell(transform.position));
			}
		}
	}

	public PathToPlayer findPathToPlayer()
	{
		dictionaryOfSegments = new Dictionary<Vector3Int, bool>();
		ArrayList listOfPaths = new ArrayList();
		Vector3Int playerCoords = PlayerMovement.getMovementGridCoords();
		PathSegment firstPathSegment = new PathSegment(AreaManager.getMasterGrid().WorldToCell(transform.position));
		dictionaryOfSegments.Add(firstPathSegment.coords, true);
		PathToPlayer firstPathToPlayer = new PathToPlayer(firstPathSegment);
		gizmosToDraw = new ArrayList();

		gizmosToDraw.Add(firstPathToPlayer.firstSegment.coords);

		if (firstPathToPlayer.firstSegment.coords.x == playerCoords.x && firstPathToPlayer.firstSegment.coords.y == playerCoords.y)
		{
			return null;
		}

		listOfPaths.Add(firstPathToPlayer);

		PathToPlayer outputPathToPlayer = null;

		for (int pathIndex = 0; pathIndex < listOfPaths.Count && pathIndex < pathIndexHardCutoff; pathIndex++)
		{
			PathToPlayer currentPath = (PathToPlayer)listOfPaths[pathIndex];

			if (currentPath.lastSegment.coords.x == playerCoords.x && currentPath.lastSegment.coords.y == playerCoords.y)
			{
				outputPathToPlayer = currentPath;
				break;
			}
			else if (thisMoveIsLegal(currentPath.lastSegment.coords))
			{
				Vector3Int[] directions = new Vector3Int[4];
				directions[0] = findClosestVectorToPlayer(currentPath.firstSegment.coords, playerCoords);

				directions = fillOtherDirections(directions);

				foreach (Vector3Int direction in directions)
				{
					PathSegment newSegment = new PathSegment(currentPath.lastSegment.coords + direction);

					if (!dictionaryOfSegments.ContainsKey(newSegment.coords))
					{
						gizmosToDraw.Add(newSegment.coords);
						dictionaryOfSegments.Add(newSegment.coords, true);
					}
					else
					{
						continue;
					}

					PathToPlayer newPath = currentPath.clone();

					newPath.setLastSegment(newSegment);

					if (newPath.length <= PathToPlayer.maxLength)
					{
						listOfPaths.Add(newPath);
					}
				}
			}
		}

		if (outputPathToPlayer != null)
		{
			outputPathToPlayer.moveAlongPath();
			return outputPathToPlayer;
		}
		else
		{
			return null;
		}
	}

	private Vector3Int[] fillOtherDirections(Vector3Int[] directions)
	{
		int directionIndex = 1;
		foreach (Vector3Int direction in MovementManager.allDirectionVectors)
		{
			if (direction.Equals(directions[0]))
			{
				continue;
			}
			else
			{
				directions[directionIndex] = direction;
				directionIndex++;
			}
		}

		return directions;
	}

	private Vector3Int findClosestVectorToPlayer(Vector3Int enemyCoords, Vector3Int playerCoords)
	{
		if (Mathf.Abs(enemyCoords.x - playerCoords.x) > Mathf.Abs(enemyCoords.y - playerCoords.y))
		{
			if (enemyCoords.x > playerCoords.x)
			{
				return MovementManager.distance1TileSouthWestGrid;
			}
			else
			{
				return MovementManager.distance1TileNorthEastGrid;
			}
		}
		else
		{
			if (enemyCoords.y > playerCoords.y)
			{
				return MovementManager.distance1TileSouthEastGrid;
			}
			else
			{
				return MovementManager.distance1TileNorthWestGrid;
			}
		}
	}

	public ArrayList getAllLegalMoves()
	{
		ArrayList legalMoves = new ArrayList();

		foreach (Vector3Int dirMod in MovementManager.allDirectionVectors)
		{
			if (thisMoveIsLegal(dirMod))
			{
				legalMoves.Add(dirMod);
			}
		}

		return legalMoves;
	}

	private Vector3Int findRandomDirection()
	{
		int moveRoll = UnityEngine.Random.Range(1, 10);

		if (moveRoll > moveThreshold)
		{
			return Vector3Int.zero;
		}
		else
		{
			ArrayList legalMoves = getAllLegalMoves();

			if (legalMoves.Count == 0 || legalMoves.Count > 4)
			{
				return Vector3Int.zero;
			}
			else if (legalMoves.Count == 1)
			{
				return (Vector3Int)legalMoves[0];
			}
			else
			{
				int moveIndex = UnityEngine.Random.Range(0, legalMoves.Count);

				return (Vector3Int)legalMoves[moveIndex];
			}
		}
	}

	public bool thisMoveIsLegal(Vector3Int directionalModifier)
	{
        return !MovementManager.colliderInCell(getCurrentCell() + directionalModifier, LayerAndTagManager.blocksEnemyMovementLayerMask);
	}

	public void setMonsterPackIndex(int i)
	{
		monsterPackIndex = i;
	}

	public int getMonsterPackIndex()
	{
		return monsterPackIndex;
	}

    private Vector3Int getCurrentCell() //world used for checking for colliders and drawing gizmos
    {
        return AreaManager.getMasterGrid().WorldToCell(transform.position);
    }
    
	public void putBackToStartingPosition()
	{
		if (!canBePutBackToStartingPosition())
		{
			return;
		}

		Vector3 startPosition = getMonsterPack().startPosition;
		transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(AreaManager.getMasterGrid().WorldToCell(startPosition));

		State.currentMonsterPackList.monsterPacks[monsterPackIndex].currentPosition = transform.position;

		Helpers.updateGameObjectPosition(gameObject);

		getMovementManager().updateArrays();
	}

	public bool canBePutBackToStartingPosition()
	{
		Vector3 startPosition = getMonsterPack().startPosition;

		startPosition.z = 0f;

		Vector3Int startPositionGridSquare = AreaManager.getMasterGrid().WorldToCell(startPosition);
		List<Vector3Int> allCurrentCells = MovementManager.getAllCurrentSpriteCells();

		foreach (Vector3Int cellPos in allCurrentCells)
		{
			if (startPositionGridSquare.x == cellPos.x && startPositionGridSquare.y == cellPos.y)
			{
				return false;
			}
		}

		return true;
	}

	public void setEnemyFacing(Facing newFacing)
	{
		enemyFacing.setFacing(newFacing);

		if (enemyDirectionIndicator != null)
		{
			enemyDirectionIndicator.setArrowDirection(enemyFacing);
		}
	}

	public void cunning()
	{
		setCunningCounter(CunningManager.cunningRange / 2);

		setEnemyFacing(CharacterFacing.getOpposingFacing(enemyFacing.getFacing()));

		enemyDirectionIndicator.setColors(cunningStunnedColor);
	}

	public void setCunningCounter(int newCunningCounter)
	{
		cunningStunCounter = newCunningCounter;

		if (enemyDirectionIndicator != null && stunnedFromCunning())
		{
			enemyDirectionIndicator.setColors(cunningStunnedColor);
		}

		State.currentMonsterPackList.monsterPacks[monsterPackIndex].cunningCounter = cunningStunCounter;
	}

	public void intimidate()
	{
		intimidateCounter = IntimidateManager.intimidateRange / 2;

		enemyDirectionIndicator.setColors(intimidatedColor);
	}

	public void setIntimidateCounter(int newIntimidateCounter)
	{
		intimidateCounter = newIntimidateCounter;

		if (enemyDirectionIndicator != null && intimidated())
		{
			enemyDirectionIndicator.setColors(intimidatedColor);
		}

		State.currentMonsterPackList.monsterPacks[monsterPackIndex].intimidateCounter = intimidateCounter;
	}

	public void retreatStun()
	{
		retreatStunnedCounter = 1;

		enemyDirectionIndicator.setColors(retreatStunnedColor);
	}

	public void setRetreatStunCounter(int newRetreatStunnedCounter)
	{
		retreatStunnedCounter = newRetreatStunnedCounter;

		if (enemyDirectionIndicator != null && stunnedFromRetreating())
		{
			enemyDirectionIndicator.setColors(retreatStunnedColor);
		}

		State.currentMonsterPackList.monsterPacks[monsterPackIndex].retreatCounter = retreatStunnedCounter;
	}

	public bool stunnedFromCunning()
	{
		return cunningStunCounter > 0;
	}

	public bool intimidated()
	{
		return intimidateCounter > 0;
	}

	public bool stunnedFromRetreating()
	{
		return retreatStunnedCounter > 0;
	}

	private void decrementSkillCounters()
	{
		if (cunningStunCounter > 0)
		{
			cunningStunCounter--;
		}

		if (intimidateCounter > 0)
		{
			intimidateCounter--;
		}

		if (retreatStunnedCounter > 0)
		{
			retreatStunnedCounter--;
		}



		setIndicatorColor();
	}

	private void setIndicatorColor()
	{
		switch (cunningStunCounter, intimidateCounter, retreatStunnedCounter)
		{
			case ( > 0, <= 0, <= 0):
				enemyDirectionIndicator.setColors(cunningStunnedColor);
				return;

			case ( <= 0, > 0, <= 0):
				enemyDirectionIndicator.setColors(intimidatedColor);
				return;

			case ( <= 0, <= 0, > 0):
				enemyDirectionIndicator.setColors(retreatStunnedColor);
				return;

			case ( <= 0, <= 0, <= 0):
				enemyDirectionIndicator.setColors(Color.white);
				return;
		}
	}

	private MonsterPack getMonsterPack()
	{
		return State.currentMonsterPackList.monsterPacks[monsterPackIndex];
	}

	private void OnEnable()
	{
		createListeners();
	}

	private void OnDisable()
	{
		destroyListeners();
	}

	//IRevealable interface methods

	public void createListeners()
	{
		RevealManager.OnReveal.AddListener(onReveal);
		TutorialSequence.TutorialSequenceTargetFinder.AddListener(assignToTutorialSequence);
	}

	public void destroyListeners()
	{
		RevealManager.OnReveal.RemoveListener(onReveal);
		TutorialSequence.TutorialSequenceTargetFinder.RemoveListener(assignToTutorialSequence);
	}

	public void onReveal()
	{
		RevealManager.setRevealForGameObject(gameObject, getRevealColor());
	}

	public Color getRevealColor()
	{
		if (moveableObject)
		{
			return RevealManager.canBePushed;
		}
		else
		{
			return RevealManager.attacksOnSight;
		}
	}
	
	public void spawnTargetCanvas()
	{
		// if (hoverTarget == null)
		// {
		// 	gameObject.AddComponent<RectTransform>();

		// 	if (moveableObject)
		// 	{
		// 		hoverTarget = Instantiate(Resources.Load<GameObject>(PrefabNames.targetBox), transform);
		// 	}
		// 	else
		// 	{
		// 		hoverTarget = Instantiate(Resources.Load<GameObject>(PrefabNames.targetBox), transform);
		// 	}
		// }
	}

	public void createHoverTag()
	{
		MouseHoverManager.getMouseHoverBase();
		//MouseHoverManager.createHoverTag(getName());
		MouseHoverManager.createHoverDescBlockPanel(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!RevealManager.currentlyRevealed)
		{
			RevealManager.setOutlineColor(gameObject, getRevealColor());
			createHoverTag();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!RevealManager.currentlyRevealed)
		{
			RevealManager.setOutlineColorToDefault(gameObject);
			MouseHoverManager.destroyMouseHoverBase();
		}
	}

	//IDescribableInBlocks

	public string getName()
	{
		if (moveableObject)
		{
			return "Pushable " + packName;
		}
		else
		{
			return packName;
		}
	}

	public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> blocks = new List<DescriptionPanelBuildingBlock>();

		blocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

        blocks.AddRange(getEnemyPackInfo().getDescriptionBuildingBlocks());

		return blocks;
	}

	//ITutorialSequenceTarget interface methods
	public bool isUI()
	{
		return false;
	}

	public Transform getTransform() 
	{
		return transform;
	}

	public string getTutorialHash()
	{
		return tutorialHash;
	}

	public void assignToTutorialSequence(TutorialSequenceStep tutorialSequenceStep)
	{
		if (tutorialSequenceStep.isTutorialTarget(getTutorialHash()))
		{
			TutorialSequenceStepTargetObject.addToHashDictionary(this);

			tutorialSequenceStep.createMessageWindowAndRunScript(getTutorialHash());
		}
	}

	public GameObject getGameObject()
	{
		return gameObject;
	}
	public RectTransform getRectTransform()
	{

		return GetComponent<RectTransform>();
	}

	public void highlight(bool skip)
	{
		if(skip)
		{
			return;
		}

		RevealManager.manuallyRevealGameObject(gameObject, RevealManager.tutorialDefault);
	}
	public void unhighlight(bool skip)
	{
		if(skip)
		{
			return;
		}
		
		RevealManager.manuallyUnrevealGameObject(gameObject);
	}

	public Vector2 getDimensions()
	{
		return (new Vector2(getRectTransform().rect.width / 4f, getRectTransform().rect.height / 4f) * PlayerMovement.getTransform().localScale);
	}
}
