using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;
public enum CharacterAnimationType { None, Idle_Front, Idle_Back, Secondary_Idle, Run_Front, Run_Back, Wounded, Death, Attack_Normal, Attack_Special }

public class AnimationManager : MonoBehaviour
{
    public readonly static CharacterAnimationType[] loopedAnimationTypesTypes = new CharacterAnimationType[]
    { CharacterAnimationType.Idle_Front, CharacterAnimationType.Idle_Back, CharacterAnimationType.Secondary_Idle,
        CharacterAnimationType.Run_Front, CharacterAnimationType.Run_Back};


    public readonly static CharacterAnimationType[] tempAnimationTypes = new CharacterAnimationType[]
    { CharacterAnimationType.Run_Front, CharacterAnimationType.Run_Back,  CharacterAnimationType.Wounded,
      CharacterAnimationType.Death, CharacterAnimationType.Attack_Normal, CharacterAnimationType.Attack_Special};

    public SpriteRenderer spriteRenderer;

    private CharacterAnimationType lastIdle;

    public Dictionary<CharacterAnimationType, AnimationClip> animationDict;

    private const int maxAttackAnimationNumber = 10;

    public NamedAnimancerComponent animancer;

    //MonsterNameList.armoredBat
    public virtual void setAnimations(string monsterName)
    {
        Debug.LogError("monsterName = " + monsterName);

        string folderPath = EnemyTypeFolderPathList.getEnemyTypeFolderPath(monsterName);

        if(folderPath == null)
        {
            animationDict = new Dictionary<CharacterAnimationType, AnimationClip>();
            return;
        }

        animationDict = getTempAnimations(folderPath);

        animancer.Animations = getIdleAnimations(folderPath);
        lastIdle = CharacterAnimationType.Idle_Front;
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
        lastIdle = CharacterAnimationType.Idle_Back;
        playAnimation(CharacterAnimationType.Idle_Back);
    }

    public void playIdleFrontAnimation()
    {
        lastIdle = CharacterAnimationType.Idle_Front;
        playAnimation(CharacterAnimationType.Idle_Front);
    }

    public void playSecondaryIdleAnimation()
    {
        lastIdle = CharacterAnimationType.Secondary_Idle;
        playAnimation(CharacterAnimationType.Secondary_Idle);
    }

    public void playDeathAnimation()
    {
        playAnimation(createClipTransitionToDeath());
    }

    public void playAttackAnimation()
    {
       playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Normal));
    }

    public void playAttackIntoFrontIdleAnimation()
    {
        lastIdle = CharacterAnimationType.Idle_Front;
       playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Normal));
    }

    public void playAttackIntoSecondaryIdleAnimation()
    {
        lastIdle = CharacterAnimationType.Secondary_Idle;
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Attack_Normal));
    }

    public void playSpecialAttackAnimation()
    {
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
            return null;
        }

        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[type];

        switch (lastIdle)
        {
            case CharacterAnimationType.Idle_Front:
                clipTransition.Events.OnEnd = () => playIdleFrontAnimation();
                break;
            case CharacterAnimationType.Idle_Back:
                clipTransition.Events.OnEnd = () => playIdleBackAnimation();
                break;
            case CharacterAnimationType.Secondary_Idle:
                clipTransition.Events.OnEnd = () => playSecondaryIdleAnimation();
                break;
        }

        return clipTransition;
    }

    private ClipTransition createClipTransitionToSecondaryIdle(CharacterAnimationType type)
    {
        if(!animationDict.ContainsKey(type))
        {
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
            return null;
        }

        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[CharacterAnimationType.Death];
        clipTransition.Events.OnEnd = () => haltAllAnimations();

        return clipTransition;
    }

    public void startUpAnimations()
    {
        animancer.enabled = true;
    }

    private void haltAllAnimations()
    {
        animancer.enabled = false;
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
            return;
        }

        // Debug.LogError("Play Animation: " + animationType.ToString());

        animancer.Stop();
        AnimancerState state = animancer.TryPlay(animationType.ToString());

        if (state == null)
        {
            if(animationType == CharacterAnimationType.Secondary_Idle)
            {
                playAnimation(CharacterAnimationType.Idle_Front);
            }else
            {
                Debug.LogError("No such animation for type: " + animationType.ToString());
            }
        }
    }

}


    // private static List<AnimationClip> getAttackAnimations(string folderPath)
    // {
    //     List<AnimationClip> animationList = new List<AnimationClip>();

    //     for (int index = 1; index <= maxAttackAnimationNumber; index++)
    //     {
    //         AnimationClip animationClip = Resources.Load<AnimationClip>(folderPath + CharacterAnimationType.Attack.ToString() + "_" + index);

    //         if (animationClip == null)
    //         {
    //             break;
    //         }
    //         else
    //         {
    //             animationList.Add(animationClip);
    //         }
    //     }

    //     return animationList;
    // }

    // private static AnimationClip[] getAllAnimations(string folderPath)
    // {
    //     List<AnimationClip> animationList = new List<AnimationClip>();

    //     animationList.AddRange(getTempAnimations(folderPath));
    //     animationList.AddRange(getAttackAnimations(folderPath));

    //     return animationList.ToArray();
    // }

    // private static List<AnimationClip> getTempAnimations(string folderPath)
    // {
    //     List<AnimationClip> animationList = new List<AnimationClip>();

    //     foreach (CharacterAnimationType type in animationTypes)
    //     {
    //         AnimationClip animClip = Resources.Load<AnimationClip>(folderPath + type.ToString());

    //         animationList.Add(animClip);
    //     }

    //     return animationList;
    // }

    // private static List<AnimationClip> getAttackAnimations(string folderPath)
    // {
    //     List<AnimationClip> animationList = new List<AnimationClip>();

    //     for (int index = 1; index <= maxAttackAnimationNumber; index++)
    //     {
    //         AnimationClip animationClip = Resources.Load<AnimationClip>(folderPath + CharacterAnimationType.Attack.ToString() + "_" + index);

    //         if (animationClip == null)
    //         {
    //             break;
    //         }
    //         else
    //         {
    //             animationList.Add(animationClip);
    //         }
    //     }

    //     return animationList;
    // }