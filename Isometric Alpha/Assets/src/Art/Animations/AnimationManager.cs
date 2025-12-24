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

    private bool changesFacing;
    public CharacterFacing facing = new CharacterFacing();
    public SpriteRenderer spriteRenderer;

    public CharacterAnimationType currentIdle;
    [SerializeField]
    private SpriteRenderer shadowSprite;
    public PolygonCollider2D polygonCollider2D;
    public HealthBarManager healthBarManager;

    public Dictionary<CharacterAnimationType, AnimationClip> animationDict;

    public NamedAnimancerComponent animancer;

    #region HeartBeatListener

    public void updateIdleAnimation(int rowToUpdate, bool beatIsEven)
    {

        if(!spriteSetByHeartBeat() || (!updatesIdleEveryBeat() && !beatIsEven) || healthBarManager.linkedStats.position.row != rowToUpdate)
        {
            return;
        }

        setSpriteToCurrentIdle();
    }

    private void setSpriteToCurrentIdle()
    {
        spriteRenderer.sprite = IdleDictionary.getCurrentIdleSprite(healthBarManager.linkedStats.position.row,
                                                                    healthBarManager.linkedStats.getName(), 
                                                                    currentIdle);

        updateCollider();
    }

    public bool updatesIdleEveryBeat()
    {
        return true;
    }

    public bool spriteSetByHeartBeat()
    {
        return !CombatAnimationManager.trackerBeingTracked(this) && !healthBarManager.linkedStats.isDead();
    }

    private void updateCollider()
    {
        List<Vector2> pointsList = new List<Vector2>();

        spriteRenderer.sprite.GetPhysicsShape(0, pointsList); 

        polygonCollider2D.points = pointsList.ToArray();
    }

    private void OnEnable()
    {
        if(CombatStateManager.inCombat)
        {
            HeartBeatManager.HeartBeat.AddListener(updateIdleAnimation);
            CombatStateManager.OnCombatEnd.AddListener(OnDisable);
        }
    }

    private void OnDisable()
    {
        HeartBeatManager.HeartBeat.RemoveListener(updateIdleAnimation);
        CombatStateManager.OnCombatEnd.RemoveListener(OnDisable);
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
        string folderPath = EnemyTypeFolderPathList.getEnemyTypeFolderPath(monsterName);

        if (folderPath == null)
        {
            animationDict = new Dictionary<CharacterAnimationType, AnimationClip>();
            changesFacing = false;
            return;
        }

        changesFacing = true;

        animationDict = getTempAnimations(folderPath);

        animancer.Animations = getIdleAnimations(folderPath);

        addIdleSprites(monsterName, folderPath);

        setToDefaultIdle();
    }

    private void setToDefaultIdle()
    {
        if(CombatStateManager.inCombat)
        {
            currentIdle = CharacterAnimationType.Idle_Front;
            haltAllAnimations();

            setSpriteToCurrentIdle();
        } else
        {
            currentIdle = CharacterAnimationType.OOC_Idle_Front;

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

    private static void addIdleSprites(string monsterName, string folderPath)
    {
        foreach (CharacterAnimationType type in loopedAnimationTypesTypes)
        {
            if(IdleDictionary.idleDictContainsSprites(monsterName, type))
            {
                continue;
            }

            Sprite[] sprites = Resources.LoadAll<Sprite>(folderPath+type.ToString());

            if(sprites == null || sprites.Length <= 0)
            {
                continue;
            }

            IdleDictionary.addSpritesToIdleDict(monsterName, type, sprites);
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

    public void playIdleBackAnimation()
    {
        enableExtras();
        removeAnimation();
        currentIdle = CharacterAnimationType.Idle_Back;
        playAnimation(CharacterAnimationType.Idle_Back);
    }

    public void playIdleFrontAnimation()
    {
        enableExtras();
        removeAnimation();
        currentIdle = CharacterAnimationType.Idle_Front;
        playAnimation(CharacterAnimationType.Idle_Front);
    }

    public void playOOCIdleBackAnimation()
    {
        enableExtras();
        removeAnimation();
        currentIdle = CharacterAnimationType.OOC_Idle_Back;
        playAnimation(CharacterAnimationType.OOC_Idle_Back);
    }

    public void playOOCIdleFrontAnimation()
    {
        enableExtras();
        removeAnimation();
        currentIdle = CharacterAnimationType.OOC_Idle_Front;
        playAnimation(CharacterAnimationType.OOC_Idle_Front);
    }

    public void playSecondaryIdleAnimation()
    {
        enableExtras();
        removeAnimation();
        currentIdle = CharacterAnimationType.Secondary_Idle;
        playAnimation(CharacterAnimationType.Secondary_Idle);
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
        currentIdle = CharacterAnimationType.Idle_Front;
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Normal));
    }

    public void playAttackIntoSecondaryIdleAnimation()
    {
        CombatAnimationManager.trackAnimation(key, this);
        currentIdle = CharacterAnimationType.Secondary_Idle;
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
        facing.setFacing(Facing.NorthEast);
        playRunBackAnimation();
    }

    public void playNorthWestRun()
    {
        facing.setFacing(Facing.NorthWest);
        playRunBackAnimation();
    }

    public void playSouthEastRun()
    {
        facing.setFacing(Facing.SouthEast);
        playRunFrontAnimation();
    }

    public void playSouthWestRun()
    {
        facing.setFacing(Facing.SouthWest);
        playRunFrontAnimation();
    }


    public void playNorthEastOOCIdle()
    {
        facing.setFacing(Facing.NorthEast);
        playOOCIdleBackAnimation();
    }

    public void playNorthWestOOCIdle()
    {
        facing.setFacing(Facing.NorthWest);
        playOOCIdleBackAnimation();
    }

    public void playSouthEastOOCIdle()
    {
        facing.setFacing(Facing.SouthEast);
        playOOCIdleFrontAnimation();
    }

    public void playSouthWestOOCIdle()
    {
        facing.setFacing(Facing.SouthWest);
        playOOCIdleFrontAnimation();
    }

    public void playNorthEastIdle()
    {
        facing.setFacing(Facing.NorthEast);
        playIdleBackAnimation();
    }

    public void playNorthWestIdle()
    {
        facing.setFacing(Facing.NorthWest);
        playIdleBackAnimation();
    }

    public void playSouthEastIdle()
    {
        facing.setFacing(Facing.SouthEast);
        playIdleFrontAnimation();
    }

    public void playSouthWestIdle()
    {
        facing.setFacing(Facing.SouthWest);
        playIdleFrontAnimation();
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

        // switch (currentIdle)
        // {
        //     case CharacterAnimationType.Idle_Front:
        //         clipTransition.Events.OnEnd = () => playIdleFrontAnimation();
        //         break;
        //     case CharacterAnimationType.Idle_Back:
        //         clipTransition.Events.OnEnd = () => playIdleBackAnimation();
        //         break;
        //     case CharacterAnimationType.Secondary_Idle:
        //         clipTransition.Events.OnEnd = () => playSecondaryIdleAnimation();
        //         break;
        // }

        return clipTransition;
    }

    private ClipTransition createClipTransitionToSecondaryIdle(CharacterAnimationType type)
    {
        if(!animationDict.ContainsKey(type))
        {
            removeAnimation();
            return null;
        }

        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[type];
        clipTransition.Events.OnEnd = () => playSecondaryIdleAnimation();

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
        if(CombatStateManager.inCombat)
        {
            switch(animationType)
            {
                case CharacterAnimationType.Idle_Front:
                case CharacterAnimationType.Idle_Back:
                case CharacterAnimationType.OOC_Idle_Front:
                case CharacterAnimationType.OOC_Idle_Back: 
                case CharacterAnimationType.Secondary_Idle:
                case CharacterAnimationType.Run_Front: 
                case CharacterAnimationType.Run_Back:
                    haltAllAnimations();
                    setSpriteToCurrentIdle();
                    return;
                default:
                    startUpAnimations();
                    break;              
            }
        }

        if(animationType == CharacterAnimationType.None)
        {
            removeAnimation();
            return;
        }

        // Debug.Log("Play Animation: " + animationType.ToString());

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
                    playIdleFrontAnimation();
                    break;
                case CharacterAnimationType.OOC_Idle_Back:
                case CharacterAnimationType.Run_Back:
                    playIdleBackAnimation();
                    break;
                default:
                    removeAnimation();
                    break;                
            }
        }
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

}
