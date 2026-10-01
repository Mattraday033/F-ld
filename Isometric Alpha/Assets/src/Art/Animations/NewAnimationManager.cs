using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public interface IAppearanceSource
{
    public IAppearance appearance
    {
        get;
    }
}

public class NewAnimationManager : MonoBehaviour
{
    public PolygonCollider2D polygonCollider2D
    {
        get
        {
            return rendererList.bodyCollider;
        }
    }

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
            return _AppearanceSource.appearance;
        }
    }

    public bool changesFacing
    {
        get
        {
            return !appearance.large;
        }
    }

    private void Awake()
    {
        rendererList = GetComponent<SpriteLayerRendererList>();
        rendererList.Awake();
    }

    public void playAnimation(CharacterAnimationType animationType = CharacterAnimationType.None)
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

        rendererList.setFlipX(characterFacing.flipSprite());
    }

    #region Combat
    //shells for the combat animations Stats used to drive through the old AnimationManager;
    //each TODO names the commented-out AnimationManager.cs method its body should be ported from

    public void playSpawnAnimation()
    {
        //TODO: port AnimationManager.playSpawnAnimation
    }

    public void playAttackAnimation()
    {
        //TODO: port AnimationManager.playAttackAnimation
    }

    public void playAttackIntoFrontIdleAnimation()
    {
        //TODO: port AnimationManager.playAttackIntoFrontIdleAnimation
    }

    public void playAttackIntoSecondaryIdleAnimation()
    {
        //TODO: port AnimationManager.playAttackIntoSecondaryIdleAnimation
    }

    public void playSpecialAttackAnimation()
    {
        //TODO: port AnimationManager.playSpecialAttackAnimation
    }

    public void playWoundedAnimation()
    {
        //TODO: port AnimationManager.playWoundedAnimation
    }

    public void playDeathAnimation()
    {
        //TODO: port AnimationManager.playDeathAnimation
    }

    public void setToDefaultIdle()
    {
        //TODO: port AnimationManager.setToDefaultIdle
    }

    //front is true for combatants on the enemy side, who face the camera
    public void setToDeadIdle(bool front)
    {
        //TODO: port AnimationManager.setCurrentIdle(Death_Front/Death_Back)
    }

    public void setIdleAnimationOnTraitApplication(Trait trait)
    {
        //TODO: port Trait.setIdleAnimationOnApplication(AnimationManager)
    }

    public void setIdleAnimationOnTraitRemoval(Trait trait)
    {
        //TODO: port Trait.setIdleAnimationOnRemoval(AnimationManager)
    }

    public void setHeartBeatRow(int heartBeatRow)
    {
        //TODO: port AnimationManager.heartBeatRow
    }

    public void playAnimationSFX(CharacterAnimationType animationType)
    {
        //TODO: port the AnimationManager clip events that called Stats.playAnimationSFX
    }

    public float getAnimationLength(CharacterAnimationType animationType)
    {
        //TODO: port AnimationManager.getAnimationLength
        return 0f;
    }

    #endregion

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
