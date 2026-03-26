using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PathSegment
{
    public static int counter;
	public int ID;
	public Vector3Int coords;
    public PathSegment nextSegment;
	
    [RuntimeInitializeOnLoadMethod]
    private static void initializePathSegment()
    {
        counter = 0;
    }

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

public class EnemyMovement : MovementTracker, ISkillTarget, IRevealable, ITutorialSequenceTarget, IDescribableInBlocks
{
    private const int cunningChargeCost = 2;
	public const int pathIndexHardCutoff = 1000;
    private const bool skipFileCreation = true;

    private int monsterPackIndex; //private so it can only set by using setMonsterPackIndex()

	public string tutorialHash;

    private MonsterMovementType _MovementType = MonsterMovementType.Random;
    public virtual MonsterMovementType movementType
    {
        get => _MovementType;
        set => _MovementType = value;
    }

	public bool movesEveryTurn = false;
	public bool neverMoves = false;

    public AnimationManager animationManager;
    public SpriteRenderer spriteRenderer;
    public SpriteOutline outline;

    public int cunningStunCounter = 0;
	public int intimidateCounter = 0;
	public int retreatStunnedCounter = 0;

	public CharacterFacing enemyFacing = new CharacterFacing();

    public Collider2D attachedCollider2D;

	private const int moveThreshold = 7;

	public Dictionary<Vector3Int, bool> dictionaryOfSegments = new Dictionary<Vector3Int, bool>();
	public List<Vector3Int> gizmosToDraw = new List<Vector3Int>();

    private void Awake()
    {
        outline = new SpriteOutline();
        outline.setSpriteRenderer(spriteRenderer);

        if(getMonsterPackIndex() == CombatStateManager.retreatedFromIndex)
        {
            retreatStun();
            SkillManager.OnSkillUse.Invoke();
        }
    }

    public virtual void Start()
    {
        animationManager.linkedStats = EnemyPackInfoList.getEnemyPackInfo(AreaManager.locationName, getMonsterPackIndex()).FoeTypes[0].enemyStats.clone();
    }

    #region MovementTracker Overrides

    public override bool isDefeated()
    {
        return MonsterDefeatKeysList.monsterIsDefeated(getMonsterPackIndex());
    }

    public override AnimationManager getAnimationManager()
    {
        return animationManager;
    }

    public override int getMovementIndex()
    {
        return getMonsterPackIndex() + 1;
    }

    public override void determineDirection()
    {
        _DirectionMod = findDirection();

        _StartingPosition = getWorldPosition();
        _EndingPosition = AreaManager.getMasterGrid().GetCellCenterWorld(getCurrentCell(this) + _DirectionMod);
    }

    #endregion

    public MovementManager getMovementManager()
    {
        return AreaManager.getMovementManager();
    }

    public EnemyPackInfo getEnemyPackInfo()
    {
        return EnemyPackInfoList.getEnemyPackInfo(AreaManager.locationName, monsterPackIndex);
    }

    public AllyPackInfo getAllyPackInfo()
    {
        return AllyPackInfoList.getAllyPackInfo(AreaManager.locationName, monsterPackIndex);
    }

    public virtual SpriteRenderer getSpriteRenderer()
    {
        return animationManager.spriteRenderer;
    }

    private void OnDrawGizmos()
    {

        if (movementType == MonsterMovementType.Chases)
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

                Gizmos.DrawWireSphere(AreaManager.getMasterGrid().CellToWorld(coords), Constants.detectionSize);
                coordsIndex++;
            }
        }
        else
        {
            Gizmos.color = Color.red;

            foreach (Vector3Int direction in MovementManager.allDirectionVectors)
            {
                Gizmos.DrawWireSphere(MovementManager.getColliderInCellPosition(getCurrentCell() + direction), Constants.detectionSize);
            }
        }

    }

    public void setToDefeated()
    {
        MonsterDefeatKeysList.setDefeatKey(MonsterDefeatKeysList.generateMonsterDefeatKey(getMonsterPackIndex()), true);
    }

    public virtual void prepCombat()
    {
        if (isDefeated())
        {
            return;
        }

        SpawnInfoManager.lastSaveBlueprint = SaveHandler.save("Before Combat", skipFileCreation);
        AudioManager.playBattleMusic();
        State.enemyPackInfo = getEnemyPackInfo();
        State.allyPackInfo = getAllyPackInfo();
        CombatStateManager.currentDefeatKey = AreaManager.locationName + "-" + monsterPackIndex;
        CombatStateManager.locationBeforeCombat = AreaManager.locationName;
        CombatStateManager.retreatedFromIndex = monsterPackIndex;

        if (intimidated())
        {
            CombatStateManager.whoIsSurprised = SurpriseState.NoOneSurprised;
        }
        else
        {
            // CombatStateManager.whoIsSurprised = SurpriseState.PlayerSurprised;
            CombatStateManager.whoIsSurprised = MovementManager.determineSurprisedParty(PlayerMovement.getInstance().transform.position, transform.position, enemyFacing.getFacing());
        }

        SceneChange.changeSceneToCombat(this);
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

        switch(_MovementType)
        {
            case MonsterMovementType.Random:
                return findRandomDirection();
            case MonsterMovementType.Chases:
                PathToPlayer pathToPlayer = findPathToPlayer();

                if (pathToPlayer == null || pathToPlayer is null)
                {
                    return Vector3Int.zero;
                }
                else
                {
                    return pathToPlayer.getDirection(AreaManager.getMasterGrid().WorldToCell(transform.position));
                }
            default:
                return Vector3Int.zero;
        }
	}

	public PathToPlayer findPathToPlayer()
	{
		dictionaryOfSegments = new Dictionary<Vector3Int, bool>();
		List<PathToPlayer> listOfPaths = new List<PathToPlayer>();
		Vector3Int playerCoords = PlayerMovement.getMovementGridCoords();
		PathSegment firstPathSegment = new PathSegment(AreaManager.getMasterGrid().WorldToCell(transform.position));
		dictionaryOfSegments.Add(firstPathSegment.coords, true);
		PathToPlayer firstPathToPlayer = new PathToPlayer(firstPathSegment);
		gizmosToDraw = new List<Vector3Int>();

		gizmosToDraw.Add(firstPathToPlayer.firstSegment.coords);

		if (firstPathToPlayer.firstSegment.coords.x == playerCoords.x && firstPathToPlayer.firstSegment.coords.y == playerCoords.y)
		{
			return null;
		}

		listOfPaths.Add(firstPathToPlayer);

		PathToPlayer outputPathToPlayer = null;

		for (int pathIndex = 0; pathIndex < listOfPaths.Count && pathIndex < pathIndexHardCutoff; pathIndex++)
		{
			PathToPlayer currentPath = listOfPaths[pathIndex];

            if (currentPath.lastSegment.coords.x == playerCoords.x && currentPath.lastSegment.coords.y == playerCoords.y)
            {
                outputPathToPlayer = currentPath;
                break;
            }
            else if (canMoveIntoThisSpace(currentPath.lastSegment.coords))
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

	public List<Vector3Int> getAllLegalMoves()
	{
		List<Vector3Int> legalMoves = new List<Vector3Int>();

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
			List<Vector3Int> legalMoves = getAllLegalMoves();

			if (legalMoves.Count == 0 || legalMoves.Count > 4)
			{
				return Vector3Int.zero;
			}
			else if (legalMoves.Count == 1)
			{
				return legalMoves[0];
			}
			else
			{
				int moveIndex = UnityEngine.Random.Range(0, legalMoves.Count);

				return legalMoves[moveIndex];
			}
		}
	}

	public bool thisMoveIsLegal(Vector3Int directionalModifier)
	{
        return !MovementManager.colliderInCell(getCurrentCell() + directionalModifier, LayerAndTagManager.blocksEnemyMovementLayerMask);
	}

	public bool canMoveIntoThisSpace(Vector3Int spaceCoords)
	{
        return spaceCoords.Equals(AreaManager.getMasterGrid().WorldToCell(transform.position)) || !MovementManager.colliderInCell(spaceCoords, LayerAndTagManager.blocksEnemyMovementLayerMask);
	}

    public void setMonsterPackIndex(int i)
    {
        monsterPackIndex = i;
    }

	public int getMonsterPackIndex()
	{
		return monsterPackIndex;
	}

    public void setToDefeatedMode()
    {
        attachedCollider2D.enabled = false;
        gameObject.SetActive(false);
    }

    private Vector3Int getCurrentCell() //world used for checking for colliders and drawing gizmos
    {
        return AreaManager.getMasterGrid().WorldToCell(transform.position);
    }
    
    private Vector3Int getStartingCell()
    {
        List<MonsterSpawnDetails> list = MonsterSpawnDetailsList.getMonsterSpawnDetails();

        return list[monsterPackIndex].cellCoords;
    }

	public Vector3 getTargetPosition()
	{
		return transform.position;
	}

	public void putBackToStartingPosition()
	{
		if (!canBePutBackToStartingPosition())
		{
			return;
		}

        List<MonsterSpawnDetails> list = MonsterSpawnDetailsList.getMonsterSpawnDetails();

        Transform newMonster = SpawnInfoManager.spawnMonster(list[monsterPackIndex], monsterPackIndex);

        MovementManager.replaceMovementTracker(newMonster.GetComponent<MovementTracker>());
        
        DestroyImmediate(gameObject);
	}

    public bool canBePutBackToStartingPosition()
    {
        if(PlayerMovement.getInstance().canPlayRunAnimation())
        {
            return false;
        }

        Vector3Int startPositionGridSquare = getStartingCell();

        foreach (MovementTracker movement in MovementManager.allMovementTrackers)
        {
            Vector3Int currentCell = MovementManager.getCellWorld(movement.startingPosition);

            if (currentCell.x == startPositionGridSquare.x &&
                currentCell.y == startPositionGridSquare.y)
            {
                return false;
            }
        }

        return true;
    }

    public int getChargeCost(SkillType skillType)
    {
        switch(skillType)
        {
            case SkillType.Cunning:
                return cunningChargeCost;
            default:
                return Constants.sizeOne;
        }
    }

    public void cunning()
    {
        setCunningCounter(CunningManager.cunningRange / 2);

        setFacing(CharacterFacing.getOpposingFacing(enemyFacing.getFacing()));
    }
    
	public virtual bool validTarget(SkillType skillType)
	{
        return true;
	}

	public void setCunningCounter(int newCunningCounter)
	{
		cunningStunCounter = newCunningCounter;
	}

	public void intimidate()
	{
		intimidateCounter = IntimidateManager.intimidateRange / 2;
	}

	public void setIntimidateCounter(int newIntimidateCounter)
	{
		intimidateCounter = newIntimidateCounter;
	}

	public void retreatStun()
	{
		retreatStunnedCounter = 1;
	}

	public void setRetreatStunCounter(int newRetreatStunnedCounter)
	{
		retreatStunnedCounter = newRetreatStunnedCounter;
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
	}

	protected override void OnEnable()
	{
		createListeners();
	}

	private void OnDisable()
	{
		destroyListeners();
	}

    public void setFromWrapper(EnemyStatWrapper statsWrapper)
    {
        transform.position = statsWrapper.getPosition();

        if (statsWrapper.intimidateCounter > 0)
        {
            intimidate();
            intimidateCounter = statsWrapper.intimidateCounter;
        }

        if (statsWrapper.cunningCounter > 0)
        {
            cunning();
            cunningStunCounter = statsWrapper.cunningCounter;
        }

        if (statsWrapper.retreatCounter > 0)
        {
            retreatStun();
            retreatStunnedCounter = statsWrapper.retreatCounter;
        }

        setFacing(statsWrapper.facing);
    }

    public void initializeAnimationManager()
    {
        EnemyPackInfo enemyPackInfo = getEnemyPackInfo();

        animationManager.setAnimations(enemyPackInfo.FoeTypes[0].enemyStats.getName());

        updateIdleDirection();
    }

	public override void setFacing(Facing newFacing)
	{
		getCharacterFacing().setFacing(newFacing);

        updateAnimationDirection();
	}

	public override CharacterFacing getCharacterFacing()
	{
        return enemyFacing;
	}

    //IRevealable interface methods

    public SpriteOutline getSpriteOutline()
    {
        return outline;
    }

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

	public void onReveal(bool toggleReveal)
	{
        if(toggleReveal)
        {
            outline.createOutline(getRevealColor());
        } else
        {
            outline.removeOutline();
        }
	}

	public Color getRevealColor()
	{
		if (movableObject)
		{
			return ColorList.canBePushed;
		}
		else
		{
			return ColorList.attacksOnSight;
		}
	}

	public void createHoverTag()
	{
		MouseHoverManager.getMouseHoverBase();
		//MouseHoverManager.createHoverTag(getName());
		MouseHoverManager.createHoverDescBlockPanel(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
        if(eventData != null && eventData.used)
        {
            return;
        }

		if (!RevealManager.currentlyRevealed)
		{
            outline.createOutline(getRevealColor());
		}

        createHoverTag();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
        if(eventData != null && eventData.used)
        {
            return;
        }

		if (!RevealManager.currentlyRevealed)
		{
			outline.removeOutline();
		}

        MouseHoverManager.destroyMouseHoverBase();
	}

	//IDescribableInBlocks

	public override string getName()
	{
        return getEnemyPackInfo().getPackName();
	}

	public virtual List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> blocks = new List<DescriptionPanelBuildingBlock>();

		blocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName() + "\n"));

        blocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(" Movement: " + movementType.ToString()+"  \n\n"));

        blocks.AddRange(getEnemyPackInfo().getDescriptionBuildingBlocks());

		return blocks;
	}

    public bool requiresInspectNode()
    {
        return false;
    }

	//ITutorialSequenceTarget interface methods
	public bool isUI()
	{
		return false;
	}

	public string getTutorialHash()
	{
		return tutorialHash;
	}

	public void setTutorialHash(string tutorialHash)
	{
		this.tutorialHash = tutorialHash;
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

        outline.createOutline(ColorList.tutorialDefault);
	}
	public void unhighlight(bool skip)
	{
		if(skip)
		{
			return;
		}
		
        outline.removeOutline();
	}

	public Vector2 getDimensions()
	{
		return new Vector2(getRectTransform().rect.width / 4f, getRectTransform().rect.height / 4f) * PlayerObject.getInstanceTransform().localScale;
	}
}
