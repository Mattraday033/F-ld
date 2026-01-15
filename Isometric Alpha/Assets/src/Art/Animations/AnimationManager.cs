using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using UnityEngine.Events;
using System;
using System.Linq;

public enum CharacterAnimationType { 
                                    None, 
                                    Idle_Front, Idle_Back, 
                                    OOC_Idle_Front, OOC_Idle_Back, 
                                    Secondary_Idle, 
                                    Run_Front, Run_Front_Left, Run_Front_Right, 
                                    Run_Back, Run_Back_Left, Run_Back_Right, 
                                    Wounded, Wounded_Front, Wounded_Back, 
                                    Death, Death_Front, Death_Back, 
                                    Attack_Normal, Attack_Normal_Front, Attack_Normal_Back, 
                                    Attack_Special, 
                                    Spawn }

public class AnimationManager : MonoBehaviour, IAnimationTracker
{
    public readonly static CharacterAnimationType[] loopedAnimationTypes = new CharacterAnimationType[]
    {   
        CharacterAnimationType.Idle_Front, CharacterAnimationType.Idle_Back, 
        CharacterAnimationType.OOC_Idle_Front, CharacterAnimationType.OOC_Idle_Back, 
        CharacterAnimationType.Secondary_Idle
    };


    public readonly static CharacterAnimationType[] tempAnimationTypes = new CharacterAnimationType[]
    { 
      CharacterAnimationType.Run_Front,     CharacterAnimationType.Run_Front_Left,      CharacterAnimationType.Run_Front_Right, 
      CharacterAnimationType.Run_Back,      CharacterAnimationType.Run_Back_Left,       CharacterAnimationType.Run_Back_Right,  
      CharacterAnimationType.Wounded,       CharacterAnimationType.Wounded_Front,       CharacterAnimationType.Wounded_Back,
      CharacterAnimationType.Death,         CharacterAnimationType.Death_Front,         CharacterAnimationType.Death_Back,   
      CharacterAnimationType.Attack_Normal, CharacterAnimationType.Attack_Normal_Front, CharacterAnimationType.Attack_Normal_Back, 
      CharacterAnimationType.Attack_Special,
      CharacterAnimationType.Spawn
    };

    public bool changesFacing
    {
        get;
        private set;
    }
    public CharacterFacing _Facing = new CharacterFacing();

    public CharacterFacing facing
    {
        get
        {
            return _Facing;
        }
        private set
        {
            _Facing = value;
        }
    }
    public SpriteRenderer spriteRenderer;

    private CharacterAnimationType currentIdle;
    [SerializeField]
    private SpriteRenderer shadowSprite;
    public PolygonCollider2D polygonCollider2D;
    public HealthBarManager healthBarManager;
    public string characterToAnimate = "";
    public int heartBeatRow = 0;

    public Dictionary<CharacterAnimationType, AnimationClip> animationDict;

    public NamedAnimancerComponent animancer;

    #region HeartBeatListener

    private void updateIdleAnimation(int rowToUpdate)
    {
        if(spriteSetByHeartBeat() && animancer.enabled)
        {
            haltAllAnimations();
        }

        if(!spriteSetByHeartBeat() || heartBeatRow != rowToUpdate)
        {
            return;
        }

        setSpriteToCurrentIdle();
    }

    private void setSpriteToCurrentIdle()
    {
        spriteRenderer.sprite = IdleDictionary.getCurrentIdleSprite(heartBeatRow,
                                                                    characterToAnimate, 
                                                                    currentIdle);

        Helpers.updatePolygonCollider(spriteRenderer, polygonCollider2D);
    }

    public bool spriteSetByHeartBeat()
    {
        return (!CombatStateManager.inCombat && !PlayerMovement.getInstance().canPlayRunAnimation()) || 
                (CombatStateManager.inCombat && !CombatAnimationManager.trackerBeingTracked(this) && !healthBarManager.linkedStats.isDead());
    }

    public void setCurrentIdle(CharacterAnimationType newIdle)
    {
        newIdle = getFallBackIdleType(characterToAnimate, newIdle);

        if(!CombatStateManager.inCombat)
        {
            currentIdle = newIdle;
            return;
        } 

        bool isAlly = CombatGrid.positionIsOnAlliedSide(healthBarManager.linkedStats.position);
        bool containsSprites = IdleDictionary.idleDictContainsSprites(characterToAnimate, newIdle);

        if((newIdle == CharacterAnimationType.Secondary_Idle || newIdle == CharacterAnimationType.Death) &&
            containsSprites)
        {
            currentIdle = newIdle;
            setSpriteToCurrentIdle();
            return;
        }

        if(isAlly)
        {
            currentIdle = CharacterAnimationType.Idle_Back;
        } else
        {
            currentIdle = CharacterAnimationType.Idle_Front;
        }

        setSpriteToCurrentIdle();
    }

    private void OnDestroy()
    {
        HeartBeatManager.getHeartBeat(characterToAnimate).RemoveListener(updateIdleAnimation);
    }

    #endregion

    #region IAnimationTracker

    public int key;

    private void Awake()
    {
        if(CombatStateManager.inCombat)
        {
            key = CombatAnimationManager.getCurrentKey();
        } 
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }

    public virtual void removeAnimation()
    {
        if(!CombatStateManager.inCombat)
        {
            return;
        }

        CombatAnimationManager.removeAnimation(key);

        CombatAnimationManager.checkAllAnimationsFinished();
    }

    #endregion

    public virtual void setAnimations(string monsterName)
    {
        characterToAnimate = monsterName;
        string folderPath = EnemyTypeFolderPathList.getEnemyTypeFolderPath(characterToAnimate);

        if (folderPath == null)
        {
            animationDict = new Dictionary<CharacterAnimationType, AnimationClip>();
            changesFacing = false;
            return;
        }

        changesFacing = true;

        animationDict = getTempAnimations(folderPath);

        animancer.Animations = getIdleAnimations(folderPath);

        addIdleSprites(characterToAnimate, folderPath);

        setHeartBeatRow();

        HeartBeatManager.getHeartBeat(characterToAnimate).AddListener(updateIdleAnimation);

        setToDefaultIdle();
    }

    private void setHeartBeatRow()
    {
        if(CombatStateManager.inCombat)
        {
            heartBeatRow = healthBarManager.linkedStats.position.row;
        } else
        {
            heartBeatRow = UnityEngine.Random.Range(CombatGrid.enemyRowUpperBounds,CombatGrid.allyRowLowerBounds);
        }
    }

    private void setToDefaultIdle()
    {
        if(CombatStateManager.inCombat)
        {
            setCurrentIdle(getFallBackIdleType(characterToAnimate, CharacterAnimationType.Idle_Front));
            haltAllAnimations();

            setSpriteToCurrentIdle();
        } else
        {
            setCurrentIdle(getFallBackIdleType(characterToAnimate, CharacterAnimationType.OOC_Idle_Front));

            playCurrentIdleAnimation();
        }
    }

    private static AnimationClip[] getIdleAnimations(string folderPath)
    {
        List<AnimationClip> animationList = new List<AnimationClip>();
        List<CharacterAnimationType> animationTypes = new List<CharacterAnimationType>();

        animationTypes.AddRange(loopedAnimationTypes);
        animationTypes.AddRange(tempAnimationTypes);

        foreach (CharacterAnimationType type in animationTypes)
        {
            AnimationClip animClip = Resources.Load<AnimationClip>(folderPath + type.ToString());

            if(animClip == null)
            {
                continue;
            }

            animationList.Add(animClip);
        }

        return animationList.ToArray();
    }

    private static void addIdleSprites(string characterToAnimate, string folderPath)
    {
        foreach (CharacterAnimationType type in loopedAnimationTypes)
        {
            addIdleSpritesOfType(characterToAnimate, folderPath, type);
        }

        addIdleSpritesOfType(characterToAnimate, folderPath, CharacterAnimationType.Death);
    }

    private static void addIdleSpritesOfType(string characterToAnimate, string folderPath, CharacterAnimationType type)
    {
        if(IdleDictionary.idleDictContainsSprites(characterToAnimate, type))
        {
            return;
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>(folderPath+type.ToString());

        if(sprites == null || sprites.Length <= 0)
        {
            return;
        }

        if(type == CharacterAnimationType.Death)
        {
            sprites = new Sprite[1]{sprites[sprites.Length-1]};    
        } 

        IdleDictionary.addSpritesToIdleDict(characterToAnimate, type, sprites);
    }

    private static Dictionary<CharacterAnimationType, AnimationClip> getTempAnimations(string folderPath)
    {
        Dictionary<CharacterAnimationType, AnimationClip> animationDict = new Dictionary<CharacterAnimationType, AnimationClip>();

        foreach (CharacterAnimationType type in tempAnimationTypes)
        {
            AnimationClip animClip = Resources.Load<AnimationClip>(folderPath + type.ToString());

            if(animClip == null)
            {
                continue;
            }

            animationDict.Add(type, animClip);
        }

        return animationDict;
    }

    public void playRunBackAnimation()
    {
        playAnimation(getRunAnimationFooting(CharacterAnimationType.Run_Back));
    }

    public void playRunFrontAnimation()
    {
        playAnimation(getRunAnimationFooting(CharacterAnimationType.Run_Front));
    }

    private static CharacterAnimationType getRunAnimationFooting(CharacterAnimationType animationType)
    {
        if(animationType == CharacterAnimationType.Run_Front)
        {
            if(State.onLeftFoot)
            {
                return CharacterAnimationType.Run_Front_Left;
            } else
            {
                return CharacterAnimationType.Run_Front_Right;
            }
        } else
        {
            if(State.onLeftFoot)
            {
                return CharacterAnimationType.Run_Back_Left;
            } else
            {
                return CharacterAnimationType.Run_Back_Right;
            }
        }
    }

    private void playIdleAnimation(CharacterAnimationType newIdle)
    {
        newIdle = getFallBackIdleType(characterToAnimate, newIdle);

        enableExtras();
        removeAnimation();
        setCurrentIdle(newIdle);
        playAnimation(newIdle);
    }

    public void playCurrentIdleAnimation()
    {
        enableExtras();
        removeAnimation();
        playAnimation(currentIdle);
    }

    public void playSpawnAnimation()
    {
        disableExtras();
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Spawn));
    }

    public void playDeathAnimation()
    {
        disableExtras();
        playAnimation(createClipTransitionToDeath());
        removeAnimation();
    }

    public void playDeathAnimationThenHide()
    {
        playAnimation(createClipTransitionToDeathThenHide());
        removeAnimation();
    }

    public void playAttackAnimation()
    {
        CharacterAnimationType attackAnimationType = CharacterAnimationType.Attack_Normal;

        if(CombatStateManager.inCombat)
        {
            if(CombatGrid.positionIsOnAlliedSide(healthBarManager.linkedStats.position))
            {
                attackAnimationType = CharacterAnimationType.Attack_Normal_Back;
            } else
            {
                attackAnimationType = CharacterAnimationType.Attack_Normal_Front;
            }
        }

        CombatAnimationManager.trackAnimation(key, this);
        playAnimation(createClipTransitionToIdle(attackAnimationType));
    }

    public void playAttackIntoFrontIdleAnimation()
    {
        CombatAnimationManager.trackAnimation(key, this);
        setCurrentIdle(CharacterAnimationType.Idle_Front);
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Normal));
    }

    public void playAttackIntoSecondaryIdleAnimation()
    {
        CombatAnimationManager.trackAnimation(key, this);
        setCurrentIdle(CharacterAnimationType.Secondary_Idle);
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Special));
    }

    public void playSpecialAttackAnimation()
    {
        CombatAnimationManager.trackAnimation(key, this);
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Special));
    }

    public void playWoundedAnimation()
    {
        CharacterAnimationType woundedAnimationType = CharacterAnimationType.Wounded;

        if(CombatStateManager.inCombat)
        {
            if(CombatGrid.positionIsOnAlliedSide(healthBarManager.linkedStats.position))
            {
                woundedAnimationType = CharacterAnimationType.Wounded_Back;
            } else
            {
                woundedAnimationType = CharacterAnimationType.Wounded_Front;
            }
        }

        CombatAnimationManager.trackAnimation(key, this);
        playAnimation(createClipTransitionToIdle(woundedAnimationType));
    }

    public void playNorthEastRun()
    {
        setFacing(Facing.NorthEast);
        playRunBackAnimation();
    }

    public void playNorthWestRun()
    {
        setFacing(Facing.NorthWest);
        playRunBackAnimation();
    }

    public void playSouthEastRun()
    {
        setFacing(Facing.SouthEast);
        playRunFrontAnimation();
    }

    public void playSouthWestRun()
    {
        setFacing(Facing.SouthWest);
        playRunFrontAnimation();
    }


    public void playNorthEastOOCIdle()
    {
        setFacing(Facing.NorthEast);
        playIdleAnimation(CharacterAnimationType.OOC_Idle_Back);
    }

    public void playNorthWestOOCIdle()
    {
        setFacing(Facing.NorthWest);
        playIdleAnimation(CharacterAnimationType.OOC_Idle_Back);
    }

    public void playSouthEastOOCIdle()
    {
        setFacing(Facing.SouthEast);
        playIdleAnimation(CharacterAnimationType.OOC_Idle_Front);
    }

    public void playSouthWestOOCIdle()
    {
        setFacing(Facing.SouthWest);
        playIdleAnimation(CharacterAnimationType.OOC_Idle_Front);
    }

    public void playNorthEastIdle()
    {
        setFacing(Facing.NorthEast);
        playIdleAnimation(CharacterAnimationType.Idle_Back);
    }

    public void playNorthWestIdle()
    {
        setFacing(Facing.NorthWest);
        playIdleAnimation(CharacterAnimationType.Idle_Back);
    }

    public void playSouthEastIdle()
    {
        setFacing(Facing.SouthEast);
        playIdleAnimation(CharacterAnimationType.Idle_Front);
    }

    public void playSouthWestIdle()
    {
        setFacing(Facing.SouthWest);
        playIdleAnimation(CharacterAnimationType.Idle_Front);
    }

    private ClipTransition createClipTransitionToIdle(CharacterAnimationType type)
    {
        type = getFallBackAnimationType(type);

        if (!animationDict.ContainsKey(type))
        {
            if(type == CharacterAnimationType.Attack_Special)
            {
                return createClipTransitionToIdle(CharacterAnimationType.Attack_Normal);
            }
            removeAnimation();
            return null;
        }

        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[type];
        clipTransition.Events.OnEnd = () => playCurrentIdleAnimation();

        return clipTransition;
    }

    private ClipTransition createClipTransitionToDeath()
    {        
        CharacterAnimationType deathAnimationType = getDeathAnimationType();

        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[deathAnimationType];
        clipTransition.Events.OnEnd = () => haltAllAnimations();

        return clipTransition;
    }

    private ClipTransition createClipTransitionToDeathThenHide()
    {
        CharacterAnimationType deathAnimationType = getDeathAnimationType();

        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[deathAnimationType];
        clipTransition.Events.OnEnd = () => hideObject();

        return clipTransition;
    }

    private CharacterAnimationType getDeathAnimationType()
    {
        CharacterAnimationType deathAnimationType = CharacterAnimationType.Death;

        if(CombatStateManager.inCombat)
        {
            if(CombatGrid.positionIsOnAlliedSide(healthBarManager.linkedStats.position))
            {
                deathAnimationType = CharacterAnimationType.Death_Back;
            } else
            {
                deathAnimationType = CharacterAnimationType.Death_Front;
            }
        }

        return getFallBackAnimationType(deathAnimationType);
    }

    private void hideObject()
    {
        gameObject.SetActive(false);
    }

    public void startUpAnimations()
    {
        animancer.enabled = true;
    }

    public void haltAllAnimations()
    {
        animancer.Stop();
        animancer.enabled = false;
        removeAnimation();
    }

    private void playAnimation(ClipTransition clipTransition)
    {
        if(clipTransition == null)
        {
            return;
        }

        startUpAnimations();

        animancer.Stop();
        AnimancerState state = animancer.Play(clipTransition);

        if (state == null)
        {
            removeAnimation();
            Debug.Log("No such animation for type: " + clipTransition.Clip.name);
        } else
        {
            // Debug.Log("Play Animation: " + clipTransition.Clip.name);
        }
    }

    public void playAnimation(CharacterAnimationType animationType)
    {
        if(spriteSetByHeartBeat())
        {
            animationType = getFallBackIdleType(characterToAnimate, animationType);
        }

        switch(animationType)
        {
            case CharacterAnimationType.Secondary_Idle:
                haltAllAnimations();
                setSpriteToCurrentIdle();
                return;

            case CharacterAnimationType.Idle_Front:
            case CharacterAnimationType.Idle_Back:
            case CharacterAnimationType.OOC_Idle_Front:
            case CharacterAnimationType.OOC_Idle_Back: 
                haltAllAnimations();
                setSpriteToCurrentIdle();
                return;
            case CharacterAnimationType.None:
                removeAnimation();
                return;
            default:
                startUpAnimations();
                break;              
        }

        animancer.Stop();

        handleFacingChange();

        AnimancerState state = animancer.TryPlay(animationType.ToString());

        if (state == null)
        {
            switch(animationType)
            {
                case CharacterAnimationType.Wounded_Front:
                case CharacterAnimationType.Wounded_Back: 
                    playAnimation(CharacterAnimationType.Wounded);
                    break;
                case CharacterAnimationType.Death_Front:
                case CharacterAnimationType.Death_Back: 
                    playAnimation(CharacterAnimationType.Death);
                    break;
                case CharacterAnimationType.Run_Front_Left:
                case CharacterAnimationType.Run_Front_Right: 
                    playAnimation(CharacterAnimationType.Run_Front);
                    break;
                case CharacterAnimationType.Run_Back_Left:
                case CharacterAnimationType.Run_Back_Right: 
                    playAnimation(CharacterAnimationType.Run_Back);
                    break;
                case CharacterAnimationType.OOC_Idle_Front:
                case CharacterAnimationType.Run_Front:
                case CharacterAnimationType.Secondary_Idle:
                case CharacterAnimationType.Spawn:
                    playIdleAnimation(CharacterAnimationType.Idle_Front);
                    break;
                case CharacterAnimationType.OOC_Idle_Back:
                case CharacterAnimationType.Run_Back:
                    playIdleAnimation(CharacterAnimationType.Idle_Back);
                    break;
                default:
                    removeAnimation();
                    break;                
            }
        }
    }

    public void setFacing(Facing newFacing)
    {
        if(!changesFacing || CombatStateManager.inCombat)
        {
            return;
        }

        facing.setFacing(newFacing);

        switch(facing.getFacing())
        {
            case Facing.NorthEast:
            case Facing.NorthWest:
                playIdleAnimation(CharacterAnimationType.OOC_Idle_Back);
                break;
            case Facing.SouthWest:
            case Facing.SouthEast:
                playIdleAnimation(CharacterAnimationType.OOC_Idle_Front);
                break;
        }

        handleFacingChange();
    }

    private void handleFacingChange()
    {
        if(!changesFacing || CombatStateManager.inCombat)
        {
            return;
        }

        switch(facing.getFacing())
        {
            case Facing.NorthEast:
            case Facing.SouthWest:
                spriteRenderer.flipX = false;
                break;
            case Facing.NorthWest:
            case Facing.SouthEast:
                spriteRenderer.flipX = true;
                break;
        }
    }

    public void disableExtras()
    {
        if(CombatStateManager.inCombat)
        {
            healthBarManager.hide();
        }

        if(shadowSprite != null)
        {
            shadowSprite.enabled = false;
        }
    }

    public void enableExtras()
    {
        if(CombatStateManager.inCombat)
        {
            healthBarManager.show();
        }
        
        if(shadowSprite != null)
        {
            shadowSprite.enabled = true;
        }

    }

    public CharacterAnimationType getFallBackAnimationType(CharacterAnimationType animationType)
    {
        if(animationDict.ContainsKey(animationType))
        {
            return animationType;
        }

        switch(animationType)
        {
            case CharacterAnimationType.Wounded_Front:
            case CharacterAnimationType.Wounded_Back:
                return CharacterAnimationType.Wounded;
            case CharacterAnimationType.Death_Front:
            case CharacterAnimationType.Death_Back:
                return CharacterAnimationType.Death;
            case CharacterAnimationType.Attack_Normal_Front:
            case CharacterAnimationType.Attack_Normal_Back:
                return CharacterAnimationType.Attack_Normal;
            default:
                Debug.LogError("No Animation Type in animationDict: " + animationType.ToString());
                return animationType;
        }
    }

    public static CharacterAnimationType getFallBackIdleType(string characterToAnimate, CharacterAnimationType animationType)
    {
        return getFallBackIdleType(characterToAnimate, animationType, false);
    }

    private static CharacterAnimationType getFallBackIdleType(string characterToAnimate, CharacterAnimationType animationType, bool retry)
    {
        if(IdleDictionary.idleDictContainsSprites(characterToAnimate, animationType))
        {
            return animationType;
        } else if(retry)
        {
            return animationType;
        }

        retry = true;

        switch(animationType)
        {
            case CharacterAnimationType.OOC_Idle_Front:
                return getFallBackIdleType(characterToAnimate, CharacterAnimationType.Idle_Front, retry);
            case CharacterAnimationType.OOC_Idle_Back:
                return getFallBackIdleType(characterToAnimate, CharacterAnimationType.Idle_Back, retry);
            case CharacterAnimationType.Idle_Front:
                return getFallBackIdleType(characterToAnimate, CharacterAnimationType.OOC_Idle_Front, retry);
            case CharacterAnimationType.Idle_Back:
                return getFallBackIdleType(characterToAnimate, CharacterAnimationType.OOC_Idle_Back, retry);
            case CharacterAnimationType.Run_Front:
                return getFallBackIdleType(characterToAnimate, CharacterAnimationType.Idle_Front, retry);
            case CharacterAnimationType.Run_Back:
                return getFallBackIdleType(characterToAnimate, CharacterAnimationType.Idle_Back, retry);
            case CharacterAnimationType.Secondary_Idle:
                return getFallBackIdleType(characterToAnimate, CharacterAnimationType.Idle_Front, false);
            default:
                return animationType;
        }
    }

}
