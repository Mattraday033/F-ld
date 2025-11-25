using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;
public enum CharacterAnimationType { None, Idle_Front, Idle_Back, Secondary_Idle, Run_Front, Run_Back, Wounded, Death, Attack_Normal, Attack_Special, Spawn }

public class AnimationManager : MonoBehaviour, IAnimationTracker
{
    public readonly static CharacterAnimationType[] loopedAnimationTypesTypes = new CharacterAnimationType[]
    { CharacterAnimationType.Idle_Front, CharacterAnimationType.Idle_Back, CharacterAnimationType.Secondary_Idle,
        CharacterAnimationType.Run_Front, CharacterAnimationType.Run_Back};


    public readonly static CharacterAnimationType[] tempAnimationTypes = new CharacterAnimationType[]
    { 
      CharacterAnimationType.Run_Front, CharacterAnimationType.Run_Back,  CharacterAnimationType.Wounded,
      CharacterAnimationType.Death, CharacterAnimationType.Attack_Normal, CharacterAnimationType.Attack_Special, 
      CharacterAnimationType.Spawn
    };

    public SpriteRenderer spriteRenderer;

    public CharacterAnimationType currentIdle;
    [SerializeField]
    private SpriteRenderer shadowSprite;
    public HealthBarManager healthBarManager;

    public Dictionary<CharacterAnimationType, AnimationClip> animationDict;

    public NamedAnimancerComponent animancer;

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
            return;
        }

        animationDict = getTempAnimations(folderPath);

        animancer.Animations = getIdleAnimations(folderPath);
        currentIdle = CharacterAnimationType.Idle_Front;
    }

    private static AnimationClip[] getIdleAnimations(string folderPath)
    {
        List<AnimationClip> animationDict = new List<AnimationClip>();

        foreach (CharacterAnimationType type in loopedAnimationTypesTypes)
        {
            AnimationClip animClip = Resources.Load<AnimationClip>(folderPath + type.ToString());

            if(animClip == null)
            {
                continue;
            }

            animationDict.Add(animClip);
        }

        return animationDict.ToArray();
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
        spriteRenderer.flipX = false;
        playRunBackAnimation();
    }

    public void playNorthWestRun()
    {
        spriteRenderer.flipX = true;
        playRunBackAnimation();
    }

    public void playSouthEastRun()
    {
        spriteRenderer.flipX = true;
        playRunFrontAnimation();
    }

    public void playSouthWestRun()
    {
        spriteRenderer.flipX = false;
        playRunFrontAnimation();
    }

    public void playNorthEastIdle()
    {
        spriteRenderer.flipX = false;
        playIdleBackAnimation();
    }

    public void playNorthWestIdle()
    {
        spriteRenderer.flipX = true;
        playIdleBackAnimation();
    }

    public void playSouthEastIdle()
    {
        spriteRenderer.flipX = true;
        playIdleFrontAnimation();
    }

    public void playSouthWestIdle()
    {
        spriteRenderer.flipX = false;
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

        animancer.Stop();
        AnimancerState state = animancer.Play(clipTransition);

        if (state == null)
        {
            removeAnimation();
            Debug.LogError("No such animation for type: " + clipTransition.Clip.name);
        } else
        {
            // Debug.LogError("Play Animation: " + clipTransition.Clip.name);
        }
    }

    public void playAnimation(CharacterAnimationType animationType)
    {
        if(animationType == CharacterAnimationType.None)
        {
            removeAnimation();
            return;
        }

        // Debug.LogError("Play Animation: " + animationType.ToString());

        animancer.Stop();
        AnimancerState state = animancer.TryPlay(animationType.ToString());

        if (state == null)
        {
            switch(animationType)
            {
                case CharacterAnimationType.Run_Front:
                case CharacterAnimationType.Secondary_Idle:
                case CharacterAnimationType.Spawn:
                    playIdleFrontAnimation();
                    break;
                case CharacterAnimationType.Run_Back:
                    playIdleBackAnimation();
                    break;
                default:
                    removeAnimation();
                    Debug.LogError("No such animation for type: " + animationType.ToString());
                    break;                
            }
        }
    }

    private void disableExtras()
    {
        if(CombatStateManager.inCombat)
        {
            healthBarManager.hide();
        }

        shadowSprite.enabled = false;
    }

    private void enableExtras()
    {
        if(CombatStateManager.inCombat)
        {
            healthBarManager.show();
        }
        
        shadowSprite.enabled = true;
    }

}
