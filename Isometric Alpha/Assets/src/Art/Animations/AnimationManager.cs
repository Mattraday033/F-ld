using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;
using UnityEngine.Events;
using System;
using System.Linq;
// using UnityEditor;

public enum CharacterAnimationType { None, Idle_Front, Idle_Back, OOC_Idle_Front, OOC_Idle_Back, Secondary_Idle, Run_Front, Run_Back, Wounded, Death, Attack_Normal, Attack_Special, Spawn }

public class AnimationManager : MonoBehaviour, IAnimationTracker
{
    public readonly static CharacterAnimationType[] loopedAnimationTypesTypes = new CharacterAnimationType[]
    {   
        CharacterAnimationType.Idle_Front, CharacterAnimationType.Idle_Back, 
        CharacterAnimationType.OOC_Idle_Front, CharacterAnimationType.OOC_Idle_Back, 
        CharacterAnimationType.Secondary_Idle,
        CharacterAnimationType.Run_Front, CharacterAnimationType.Run_Back
    };


    public readonly static CharacterAnimationType[] tempAnimationTypes = new CharacterAnimationType[]
    { 
      CharacterAnimationType.Run_Front, CharacterAnimationType.Run_Back,  CharacterAnimationType.Wounded,
      CharacterAnimationType.Death, CharacterAnimationType.Attack_Normal, CharacterAnimationType.Attack_Special, 
      CharacterAnimationType.Spawn
    };

    public bool changesFacing
    {
        get;
        private set;
    }
    private CharacterFacing facing = new CharacterFacing();
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
        return (!CombatStateManager.inCombat && !PlayerMovement.getInstance().isMoving()) || 
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
            return;
        }

        if(isAlly)
        {
            currentIdle = CharacterAnimationType.Idle_Back;
        } else
        {
            currentIdle = CharacterAnimationType.Idle_Front;
        }
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

        foreach (CharacterAnimationType type in loopedAnimationTypesTypes)
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
        foreach (CharacterAnimationType type in loopedAnimationTypesTypes)
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
        playAnimation(CharacterAnimationType.Run_Back);
    }

    public void playRunFrontAnimation()
    {
        playAnimation(CharacterAnimationType.Run_Front);
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
        CombatAnimationManager.trackAnimation(key, this);
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Normal));
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
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Wounded));
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
        if (!animationDict.ContainsKey(CharacterAnimationType.Death))
        {
            removeAnimation();
            return null;
        }

        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[CharacterAnimationType.Death];
        clipTransition.Events.OnEnd = () => haltAllAnimations();

        return clipTransition;
    }

    private ClipTransition createClipTransitionToDeathThenHide()
    {
        if (!animationDict.ContainsKey(CharacterAnimationType.Death))
        {
            removeAnimation();
            return null;
        }

        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[CharacterAnimationType.Death];
        clipTransition.Events.OnEnd = () => hideObject();

        return clipTransition;
    }

    private void hideObject()
    {
        gameObject.SetActive(false);
    }

    public void startUpAnimations()
    {
        animancer.enabled = true;
    }

    private void haltAllAnimations()
    {
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
        animationType = getFallBackIdleType(characterToAnimate, animationType);

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

    private void disableExtras()
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

    private void enableExtras()
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

/*
    private static Dictionary<CharacterAnimationType, Sprite[]> getIdleSprites(string folderPath)
    {
        Dictionary<CharacterAnimationType, Sprite[]> spriteDict = new Dictionary<CharacterAnimationType, Sprite[]>();

        foreach (CharacterAnimationType type in loopedAnimationTypesTypes)
        {
            int index = 0;
            List<Sprite> sprites = new List<Sprite>();
            Debug.LogError("folderPath = " + folderPath+type.ToString());

            Sprite sprite = Resources.Load<Sprite>(folderPath+type.ToString());

            while(sprite != null)
            {
                sprites.Add(sprite);

                index++;
                sprite = Resources.Load<Sprite>(folderPath+type.ToString()+"_"+index);
            }

            if(sprites.Count <= 0)
            {
                continue;
            }

            spriteDict.Add(type, sprites.ToArray());
        }

        return spriteDict;
    }
*/