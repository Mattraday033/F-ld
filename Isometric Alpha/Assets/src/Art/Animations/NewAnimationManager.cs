using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public interface IAppearanceSource
{
    public IAppearance getAppearance();
}

public class NewAnimationManager : MonoBehaviour
{
    public PolygonCollider2D polygonCollider2D;
    public SpriteLayerRendererList rendererList;

    private MovementTracker _MovementTracker;
    public MovementTracker movementTracker
    {
        set
        {
            if(value == null)
            {
                return;
            }

            _MovementTracker = value;
            MovementManager.AfterMoveStarted.RemoveListener(handleMovementAnimation);
            MovementManager.OnMoveFinished.RemoveListener(handleMovementAnimation);
            MovementManager.AfterMoveStarted.AddListener(handleMovementAnimation);
            MovementManager.OnMoveFinished.AddListener(handleMovementAnimation);
        }
        get
        {
            return _MovementTracker;
        }
    }

    public CharacterFacing characterFacing = new CharacterFacing();

    private CharacterAnimationType _CurrentIdle;
    public CharacterAnimationType currentIdle
    {
        set
        {
            switch(value)
            {
                case CharacterAnimationType.Idle_Back:
                case CharacterAnimationType.Idle_Front:
                case CharacterAnimationType.OOC_Idle_Back:
                case CharacterAnimationType.OOC_Idle_Front:
                case CharacterAnimationType.Death_Back:
                case CharacterAnimationType.Death_Front:
                case CharacterAnimationType.Secondary_Idle_Back:
                case CharacterAnimationType.Secondary_Idle_Front:
                case CharacterAnimationType.Secondary_Death:
                    _CurrentIdle = value;
                    return;
            }   
        }
        private get
        {
            return _CurrentIdle;
        }
    }

    public Coroutine currentAnimation;

    private IAppearanceSource _AppearanceSource;
    public IAppearanceSource appearanceSource
    {
        set
        {
            _AppearanceSource = value;
        } 
    }
    public void setAppearanceSource(IAppearanceSource appearanceSource,
                                    CharacterAnimationType newIdle = CharacterAnimationType.None)
    {
        _AppearanceSource = appearanceSource;

        if(newIdle != CharacterAnimationType.None)
        {
            playAnimation(newIdle);
        } else
        {
            handleMovementAnimation();
        }
    }

    public IAppearance appearance
    {
        get
        {
            return _AppearanceSource.getAppearance();
        }
    }

    public bool changesFacing
    {
        get
        {
            return !appearance.large;
        }
    }

    public void playAnimation(CharacterAnimationType animationType)
    {
        currentIdle = animationType;

        appearance.applyAppearance(rendererList, animationType);

        // switch(animationType)
        // {
        //     case CharacterAnimationType.OOC_Idle_Front:



        //         return;
        //     default:
        //         return;
        // }
    }

    public void handleMovementAnimation(int i)
    {
        if(i != movementTracker.getMovementIndex())
        {
            return;
        }

        handleMovementAnimation();
    }

    public void handleMovementAnimation()
    {
        if(movementTracker != null && movementTracker.isMoving())
        {
            switch(characterFacing.getFacing())
            {
                case Facing.NorthEast:
                case Facing.NorthWest:
                    if(State.onLeftFoot)
                    {
                        playAnimation(CharacterAnimationType.Run_Back_Left);
                    } else
                    {
                        playAnimation(CharacterAnimationType.Run_Back_Right);
                    }
                    break;
                case Facing.SouthEast:
                case Facing.SouthWest:
                    if(State.onLeftFoot)
                    {
                        playAnimation(CharacterAnimationType.Run_Front_Left);
                    } else
                    {
                        playAnimation(CharacterAnimationType.Run_Front_Right);
                    }
                    break;
            }
        } else
        {
            switch(characterFacing.getFacing())
            {
                case Facing.NorthEast:
                case Facing.NorthWest:
                    if(AreaList.currentAreaIsHostile())
                    {
                        playAnimation(CharacterAnimationType.Idle_Back);
                    } else
                    {
                        playAnimation(CharacterAnimationType.OOC_Idle_Back);
                    }
                    break;
                case Facing.SouthEast:
                case Facing.SouthWest:
                    if(AreaList.currentAreaIsHostile())
                    {
                        playAnimation(CharacterAnimationType.Idle_Front);
                    } else
                    {
                        playAnimation(CharacterAnimationType.OOC_Idle_Front);
                    }
                    break;
            }
        }

        rendererList.setFlipX(characterFacing.getFacing() == Facing.NorthWest || 
                                characterFacing.getFacing() == Facing.SouthEast);
    }

    private void OnEnable()
    {
        if(movementTracker == null)
        {
            movementTracker = GetComponent<MovementTracker>();
        }

        characterFacing.OnFacingChange.AddListener(handleMovementAnimation);
    }

    private void OnDestroy()
    {
        MovementManager.AfterMoveStarted.RemoveListener(handleMovementAnimation);
        MovementManager.OnMoveFinished.RemoveListener(handleMovementAnimation);

        characterFacing.OnFacingChange.RemoveListener(handleMovementAnimation);
    }

}
