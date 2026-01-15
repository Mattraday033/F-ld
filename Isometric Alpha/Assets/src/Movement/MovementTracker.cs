using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementTracker : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        TransitionManager.AfterTransition.AddListener(resetPreviousDirectionMod);
    }

    protected virtual void OnDestroy()
    {
        TransitionManager.AfterTransition.RemoveListener(resetPreviousDirectionMod);
    }

    public void addToList(PlacedPartyMember placedPartyMember)
    {
        placedPartyMember.movementTrackers.Add(this);
    }

    public MovementTracker nextInTrain;

    public abstract string getName();

    protected Vector3 _StartingPosition;
    public virtual Vector3 startingPosition
    {
        get => _StartingPosition;
        set
        {
            _StartingPosition = value;
        }
    }

    protected Vector3 _EndingPosition;
    public virtual Vector3 endingPosition
    {
        get => _EndingPosition;
        set
        {
            _EndingPosition = value;
        }
    }

    protected Vector3 _PreviousCell;
    public virtual Vector3 previousCell
    {
        get => _PreviousCell;
        set
        {
            _PreviousCell = value;
        }
    }

    private void resetPreviousDirectionMod()
    {
        previousDirectionMod = Vector3Int.zero;
    }

    protected Vector3Int _PreviousDirectionMod = Vector3Int.zero;
    public virtual Vector3Int previousDirectionMod
    {
        get => _PreviousDirectionMod;
        set
        {
            _PreviousDirectionMod = value;
        }
    }

    protected Vector3Int _DirectionMod;
    public virtual Vector3Int directionMod
    {
        get => _DirectionMod;
        set
        {   
            _PreviousDirectionMod = _DirectionMod;
            _DirectionMod = value;
            previousCell = getCell();
        }
    }

    public Vector3Int getCell()
    {
        return AreaManager.getMasterGrid().WorldToCell(transform.position);
    }

    public virtual bool isMoving()
    {
        return MovementManager.currentMovements.ContainsKey(this);
    }

    public virtual bool canPlayRunAnimation()
    {
        return isMoving();
    }

    public virtual bool movableObject
    {
        get => false;
        set
        {
        }
    }

    public Transform getTransform()
    {
        return transform;
    }

    public Vector3 getWorldPosition()
    {
        return transform.position;
    }

    public virtual void determineDirection()
    {
        endingPosition = AreaManager.getMasterGrid().GetCellCenterWorld(MovementTracker.getCurrentCell(this) + _DirectionMod);
    }

    public abstract int getMovementIndex();

    public virtual void cancelMovement()
    {
        _DirectionMod = Vector3Int.zero;
        endingPosition = startingPosition;
    }

    public virtual bool isDefeated()
    {
        return false;
    }

    #region Animation

    public abstract AnimationManager getAnimationManager();

    public virtual void setFacing(Facing facing)
    {
        getCharacterFacing().setFacing(facing);
    }

    public abstract CharacterFacing getCharacterFacing();

    public void updateFacing()
    {
        if (directionMod.Equals(Vector3Int.zero))
        {
            return;
        }

        if (directionMod.Equals(MovementManager.distance1TileNorthEastGrid))
        {
            setFacing(Facing.NorthEast);

        }
        else if (directionMod.Equals(MovementManager.distance1TileSouthEastGrid))
        {
            setFacing(Facing.SouthEast);

        }
        else if (directionMod.Equals(MovementManager.distance1TileSouthWestGrid))
        {
            setFacing(Facing.SouthWest);

        }
        else if (directionMod.Equals(MovementManager.distance1TileNorthWestGrid))
        {
            setFacing(Facing.NorthWest);
        }
    }

    public void updateAnimationDirection()
    {
        if(canPlayRunAnimation())
        {
            updateRunDirection();
        } else
        {
            updateIdleDirection();
        }
    }

    public virtual void updateIdleDirection()
    {
        switch (getCharacterFacing().getFacing())
        {
            case Facing.NorthEast:
                if(AreaList.currentAreaIsHostile())
                {
                    getAnimationManager().playNorthEastIdle();
                } else
                {
                    getAnimationManager().playNorthEastOOCIdle();
                }
                break;
            case Facing.NorthWest:
                if(AreaList.currentAreaIsHostile())
                {
                    getAnimationManager().playNorthWestIdle();
                } else
                {
                    getAnimationManager().playNorthWestOOCIdle();
                }
                break;
            case Facing.SouthEast:
                if(AreaList.currentAreaIsHostile())
                {
                    getAnimationManager().playSouthEastIdle();
                } else
                {
                    getAnimationManager().playSouthEastOOCIdle();
                }
                break;
            default:
                if(AreaList.currentAreaIsHostile())
                {
                    getAnimationManager().playSouthWestIdle();
                } else
                {
                    getAnimationManager().playSouthWestOOCIdle();
                }
                break;
        }
    }

    public virtual void updateRunDirection()
    {
        switch (getCharacterFacing().getFacing())
        {
            case Facing.NorthEast:
                getAnimationManager().playNorthEastRun();
                break;
            case Facing.NorthWest:
                getAnimationManager().playNorthWestRun();
                break;
            case Facing.SouthEast:
                getAnimationManager().playSouthEastRun();
                break;
            default:
                getAnimationManager().playSouthWestRun();
                break;
        }
    }

    #endregion

    public static Vector3Int getCurrentCell(MovementTracker movement)
    {
        return AreaManager.getMasterGrid().WorldToCell(movement.getWorldPosition());
    }

    public static Vector3Int getEndingCell(MovementTracker movement)
    {
        return AreaManager.getMasterGrid().WorldToCell(movement.endingPosition);
    }

    public virtual int getPlaceInTrain()
    {
        return 0;
    }

    public virtual bool canMoveInTrain()
    {
        return true;
    }

    public virtual void moveNextInTrain()
    {
        if(nextInTrain == null || !nextInTrain.canMoveInTrain())
        {
            return;
        }
        
        if(Helpers.checkPositionForColliders(PlayerMovement.getColliderWorldPosition(), LayerAndTagManager.blocksMoveableObjectLayerMask))
        {
            nextInTrain.updateAnimationDirection();
            return;
        } 

        nextInTrain.directionMod = previousDirectionMod;
        nextInTrain.startingPosition = nextInTrain.transform.position;
        nextInTrain.endingPosition = transform.position;

        AreaManager.getMovementManager().startMovement(nextInTrain);
        nextInTrain.moveNextInTrain();
    }

    public void hideSprite()
    {
        getAnimationManager().spriteRenderer.enabled = false;
        getAnimationManager().disableExtras();
    }

    public void showSprite()
    {
        getAnimationManager().spriteRenderer.enabled = true;
        getAnimationManager().enableExtras();
    }

    public static MovementTracker determineLowestTrainPriority(MovementTracker movementOne, MovementTracker movementTwo)
    {
        if(movementOne.getPlaceInTrain() < movementTwo.getPlaceInTrain())
        {
            return movementTwo;
        } else
        {
            return movementOne;
        }
    }
}