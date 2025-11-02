using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;
public enum CharacterAnimationType { Idle_Front, Idle_Back, Run_Front, Run_Back, Wounded, Death, Attack }

public class AnimationManager : MonoBehaviour
{
    public readonly static CharacterAnimationType[] idleAnimationsTypes = new CharacterAnimationType[] 
                                { CharacterAnimationType.Idle_Front, CharacterAnimationType.Idle_Back};


    public readonly static CharacterAnimationType[] tempAnimationTypes = new CharacterAnimationType[] 
    { CharacterAnimationType.Run_Front, CharacterAnimationType.Run_Back,  CharacterAnimationType.Wounded, CharacterAnimationType.Death};

    public Dictionary<CharacterAnimationType, AnimationClip> animationDict;

    private const int maxAttackAnimationNumber = 10;

    public NamedAnimancerComponent animancer;

    //MonsterNameList.armoredBat
    public virtual void setAnimations(string monsterName)
    {
        string folderPath = EnemyTypeFolderPathList.getEnemyTypeFolderPath(monsterName);
        animationDict = getTempAnimations(folderPath);

        animancer.Animations = getIdleAnimations(folderPath);
    }

    private static AnimationClip[] getIdleAnimations(string folderPath)
    {
        List<AnimationClip> animationDict = new List<AnimationClip>();

        foreach (CharacterAnimationType type in idleAnimationsTypes)
        {
            AnimationClip animClip = Resources.Load<AnimationClip>(folderPath + type.ToString());

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
        playAnimation(CharacterAnimationType.Idle_Back);
    }

    public void playIdleFrontAnimation()
    {
        playAnimation(CharacterAnimationType.Idle_Front);
    }

    public void playDeathAnimation()
    {
        playAnimation(createClipTransitionToDeath());
    }

    // public void playAttackAnimation(int attackIndex)
    // {
    //    playAnimation(CharacterAnimationType.Attack.ToString() + "_" + attackIndex);
    // }

    // public void playAttackAnimation()
    // {
    //    playAnimation(CharacterAnimationType.Attack.ToString() + "_" + Constants.indexOne);
    // }


    public void playWoundedAnimation()
    {
        playAnimation(createClipTransitionToIdle(CharacterAnimationType.Wounded));
    }

    private ClipTransition createClipTransitionToIdle(CharacterAnimationType type)
    {
        ClipTransition clipTransition = new ClipTransition();
        clipTransition.Clip = animationDict[type];
        clipTransition.Events.OnEnd = () => playIdleFrontAnimation();

        return clipTransition;
    }

    private ClipTransition createClipTransitionToDeath()
    {
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
        animancer.Stop();
        AnimancerState state = animancer.Play(clipTransition);

        if (state == null)
        {
            Debug.LogError("No such animation for type: " + clipTransition);
        }
    }

    private void playAnimation(CharacterAnimationType animationType)
    {
        animancer.Stop();
        AnimancerState state = animancer.TryPlay(animationType.ToString());

        if (state == null)
        {
            Debug.LogError("No such animation for type: " + animationType.ToString());
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