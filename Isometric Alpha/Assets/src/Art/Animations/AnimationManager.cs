using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

public enum CharacterAnimationType { Idle_Front, Idle_Back, Run_Front, Run_Back, Wound, Death, Attack }

public class AnimationManager : MonoBehaviour
{

    public readonly static CharacterAnimationType[] animationTypes = new CharacterAnimationType[] { CharacterAnimationType.Idle_Front,
    CharacterAnimationType.Idle_Back, CharacterAnimationType.Run_Front, CharacterAnimationType.Run_Back,  CharacterAnimationType.Wound, CharacterAnimationType.Death};

    private const int maxAttackAnimationNumber = 10;

    public NamedAnimancerComponent animancer;

    //MonsterNameList.armoredBat
    public void setAnimations(string monsterName)
    {
        animancer.Animations = getAllAnimations(EnemyTypeFolderPathList.getEnemyTypeFolderPath(monsterName));

        playAnimation(CharacterAnimationType.Idle_Front);
    }

    private static AnimationClip[] getAllAnimations(string folderPath)
    {
        List<AnimationClip> animationList = new List<AnimationClip>();

        animationList.AddRange(getNonAttackAnimations(folderPath));
        animationList.AddRange(getAttackAnimations(folderPath));

        return animationList.ToArray();
    }

    private static List<AnimationClip> getNonAttackAnimations(string folderPath)
    {
        List<AnimationClip> animationList = new List<AnimationClip>();

        foreach (CharacterAnimationType type in animationTypes)
        {
            AnimationClip animClip = Resources.Load<AnimationClip>(folderPath + type.ToString());

            animationList.Add(animClip);
        }

        return animationList;
    }

    private static List<AnimationClip> getAttackAnimations(string folderPath)
    {
        List<AnimationClip> animationList = new List<AnimationClip>();

        for (int index = 1; index <= maxAttackAnimationNumber; index++)
        {
            AnimationClip animationClip = Resources.Load<AnimationClip>(folderPath + CharacterAnimationType.Attack.ToString() + "_" + index);

            if (animationClip == null)
            {
                break;
            }
            else
            {
                animationList.Add(animationClip);
            }
        }

        return animationList;
    }


    public void playAnimation(CharacterAnimationType animationType)
    {
        AnimancerState state = animancer.TryPlay(animationType.ToString());

        if (state == null)
        {
            Debug.LogError("No such animation for type: " + animationType.ToString());
        }
    }

    public void playAttackAnimation(CharacterAnimationType animationType, int attackIndex)
    {
        AnimancerState state = animancer.TryPlay(animationType.ToString() + "_" + attackIndex);

        if(state == null)
        {
            Debug.LogError("No such animation for type: " + animationType.ToString() + "_" + attackIndex);
        }
    }

}
