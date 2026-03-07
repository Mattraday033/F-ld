using System.IO;
using System.Collections;
using UnityEngine;
using Ink.Runtime;

public class PlayerMovement : MovementTracker
{
    #region MovementTracker Overrides
    protected override void OnEnable()
    {
        base.OnEnable();
        PlacedPartyMember.PartyMemberLocationRequest.AddListener(addToList);
        MovementManager.OnMoveFinished.AddListener(preventAnimationStall);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        PlacedPartyMember.PartyMemberLocationRequest.RemoveListener(addToList);
        MovementManager.OnMoveFinished.RemoveListener(preventAnimationStall);
    }

	public override string getName()
	{
        return PartyManager.getPlayerStats().getName();
	}

    public override void cancelMovement()
    {
        directionMod = Vector3Int.zero;
        endingPosition = startingPosition;
    }

    public static int getPlayerMovementIndex()
    {
        return getInstance().getMovementIndex();
    }

    public override int getMovementIndex()
    {
        return MovementManager.playerSpriteIndex;
    }
    public override AnimationManager getAnimationManager()
    {
        return PlayerObject.getAnimationManager();
	}

    public static void updatePlayerFacing()
    {
        instance.updateFacing();
    }

	public static void setPlayerFacing(Facing newFacing)
	{
        instance.setFacing(newFacing);
	}

	public override void setFacing(Facing newFacing)
	{
		getCharacterFacing().setFacing(newFacing);

        updateAnimationDirection();
	}

	public override CharacterFacing getCharacterFacing()
	{
        return State.playerFacing;
	}

    public override bool canPlayRunAnimation()
    {
        return PlayerInput.canMove() && (isMoving() || KeyBindingList.movementKeyPressed()) && !KeyPressManager.handlingPrimaryKeyPress;
    }

    public override void playFootstepSFX()
    {
        // AudioManager.playFootStep();
    }

    #endregion

    private static PlayerMovement instance;

    public SpriteRenderer playerSpriteRenderer;

    [RuntimeInitializeOnLoadMethod]
    private static void initializePlayerMovement()
    {
        instance = null;
    }

    public static PlayerMovement getInstance()
    {
        return instance;
    }

    public void Awake()
    {
        instance = this;
    }

    public void preventAnimationStall(int index)
    {
        if(index != getMovementIndex() || getAnimationManager() == null)
        {
            return;
        }

        StartCoroutine(preventAnimationStallCoroutine());
    }

    private IEnumerator preventAnimationStallCoroutine()
    {
        yield return null;

        if(getAnimationManager().animancer.enabled && !canPlayRunAnimation())
        {
            getAnimationManager().haltAllAnimations();
        }
    }

    public static void updateStartEndPosition()
    {
        if (instance == null)
        {
            return;
        }

        instance.startingPosition = PlayerObject.getInstanceTransform().position;
        instance.endingPosition = PlayerObject.getInstanceTransform().position;
    }

    void Start()
    {
        MovementManager.addMovementTracker(this);
    }

    public static void adjustPlayerDirectionalMod(Vector3Int directionMod)
    {
        getInstance().directionMod = directionMod;
    }

    public static void cancelPlayerMovement()
    {
        getInstance().cancelMovement();
    }

    public static bool playerIsMoving()
    {
        if(instance == null)
        {
            return false;
        }

        return instance.isMoving();
    }

    public static bool canInteract()
    {
        PlayerMovement player = getInstance();

        if (player == null)
        {
            return false;
        }

        Collider2D npcCollider = PositionQuery.npcAtPosition(getColliderWorldPosition());
        Collider2D chestCollider = PositionQuery.chestAtPosition(getColliderWorldPosition());

        if (npcCollider != null)
        {
            GameObject currentGameObject = Physics2D.OverlapCircle(player.colliderWorldPosition(), Constants.detectionSize, LayerAndTagManager.npcLayerMask).gameObject;

            if (currentGameObject.tag.Equals(LayerAndTagManager.npcTag) ||
                currentGameObject.tag.Equals(LayerAndTagManager.observableTag) ||
                currentGameObject.tag.Equals(LayerAndTagManager.transitionTag)) //added transition tag for Ladders, normal transitions shouldn't be interactable
            {                                                                   //If a transition is interactable (it would throw an error when interacted with)
                                                                                //then it has it's layer set to NPC erroneously
                return true;
            }
            else if (currentGameObject.tag.Equals(LayerAndTagManager.bookTag))
            {
                return true;
            }

        }
        else if (chestCollider != null)
        {
            Chest currentChest = chestCollider.gameObject.GetComponent<Chest>();

            if (!currentChest.hasBeenOpened())
            {
                return true;
            }
        }

        return false;
    }

    public static Vector3 getColliderWorldPosition()
    {
        return getInstance().colliderWorldPosition();
    }

    private Vector3 colliderWorldPosition() //world used for checking for colliders and drawing gizmos
    {
        return AreaManager.getMovementManager().grid.GetCellCenterWorld(AreaManager.getMovementManager().grid.WorldToCell(transform.position) + getDirectionModFromFacing()) - new Vector3(0f, .2f, 0);
    }

    public static Vector3 getColliderWorldPosition(int multiplier)
    {
        return getInstance().colliderWorldPosition(multiplier);
    }

    private Vector3 colliderWorldPosition(int multiplier) //world used for checking for colliders and drawing gizmos, with multiplier
    {
        return AreaManager.getMovementManager().grid.GetCellCenterWorld(AreaManager.getMovementManager().grid.WorldToCell(transform.position) + (getDirectionModFromFacing() * multiplier)) - new Vector3(0f, .2f, 0);
    }

    public static Vector3Int getMovementGridCoords()
    {
        return AreaManager.getMovementManager().grid.WorldToCell(PlayerObject.getInstanceTransform().position);
    }

    public Vector3 convertGridCoordsToWorldPos(Vector3Int gridSquareCoords)
    {
        return AreaManager.getMovementManager().grid.GetCellCenterWorld(gridSquareCoords);
    }

    public static GameObject getCurrentInteractableBeforePlayer()
    {
        Collider2D collider = PositionQuery.npcAtPosition(getColliderWorldPosition());

        if(collider != null)
        {
            return collider.gameObject;
        }

        return null;
    }

    private Vector3Int getDirectionModFromFacing()
    {
        switch(State.playerFacing.getFacing())
        {
            case Facing.NorthEast:
                return MovementManager.distance1TileNorthEastGrid;
            case Facing.NorthWest:
                return MovementManager.distance1TileNorthWestGrid;
            case Facing.SouthEast:
                return MovementManager.distance1TileSouthEastGrid;
            default:
                return MovementManager.distance1TileSouthWestGrid;
        }
    }

    public static Story addAllVariables(Story currentStory)
    {
        if (currentStory.variablesState[InkVariableNameList.facingNE] != null)
        {
            currentStory.variablesState[InkVariableNameList.facingNE] = State.playerFacing.getFacing().Equals(Facing.NorthEast);
        }

        if (currentStory.variablesState[InkVariableNameList.facingNW] != null)
        {
            currentStory.variablesState[InkVariableNameList.facingNW] = State.playerFacing.getFacing().Equals(Facing.NorthWest);
        }

        if (currentStory.variablesState[InkVariableNameList.facingSW] != null)
        {
            currentStory.variablesState[InkVariableNameList.facingSW] = State.playerFacing.getFacing().Equals(Facing.SouthWest);
        }

        if (currentStory.variablesState[InkVariableNameList.facingSE] != null)
        {
            currentStory.variablesState[InkVariableNameList.facingSE] = State.playerFacing.getFacing().Equals(Facing.SouthEast);
        }

        return currentStory;
    }

    public static void setNextInTrain(PartyMemberMovement nextInTrain)
    {
        instance.nextInTrain = nextInTrain;
    }
}
