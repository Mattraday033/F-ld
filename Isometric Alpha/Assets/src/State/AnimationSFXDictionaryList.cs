using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationSFXDictionaryList
{
    #region Humans

    public readonly static Dictionary<CharacterAnimationType, string> humanAudioDictionary = new Dictionary<CharacterAnimationType, string>
    {
        [CharacterAnimationType.Attack_Normal] = AudioClipList.weaponSwingAttackSound
    };

    #endregion

    #region Bats

    public readonly static Dictionary<CharacterAnimationType, string> batSwarmAudioDicionary = new Dictionary<CharacterAnimationType, string>
    {
        [CharacterAnimationType.Attack_Normal] = AudioClipList.batSwarmAttackSound,
        [CharacterAnimationType.Death] = AudioClipList.batDeathSFXOne
    };

    public readonly static Dictionary<CharacterAnimationType, string> largeBatAudioDictionary = new Dictionary<CharacterAnimationType, string>
    {
        [CharacterAnimationType.Attack_Normal] = AudioClipList.batAttackSound,
        [CharacterAnimationType.Death] = AudioClipList.batDeathSFXOne
    };

    public readonly static Dictionary<CharacterAnimationType, string> bipedalBatAudioDictionary = new Dictionary<CharacterAnimationType, string>
    {
        [CharacterAnimationType.Attack_Special] = AudioClipList.batHowelAttackSound,
        [CharacterAnimationType.Death] = AudioClipList.batDeathSFXTwo
    };

    #endregion

    #region Worms

    public readonly static Dictionary<CharacterAnimationType, string> biteWormAudioDictionary = new Dictionary<CharacterAnimationType, string>
    {
        [CharacterAnimationType.Attack_Normal] = AudioClipList.biteAttackSound,
        [CharacterAnimationType.Attack_Special] = AudioClipList.wormExplodeOnDeathSound
    };

    public readonly static Dictionary<CharacterAnimationType, string> massiveWormAudioDictionary = new Dictionary<CharacterAnimationType, string>
    {
        // [CharacterAnimationType.Attack_Normal] = AudioClipList.batSwarmAttackSound,
        [CharacterAnimationType.Attack_Special] = AudioClipList.wormExplodeOnDeathSound
    };

    public readonly static Dictionary<CharacterAnimationType, string> summonWormAudioDictionary = new Dictionary<CharacterAnimationType, string>
    {
        [CharacterAnimationType.Attack_Normal] = AudioClipList.wormSummonSound,
        [CharacterAnimationType.Attack_Special] = AudioClipList.wormExplodeOnDeathSound
    };

    public readonly static Dictionary<CharacterAnimationType, string> vomitWormAudioDictionary = new Dictionary<CharacterAnimationType, string>
    {
        [CharacterAnimationType.Attack_Normal] = AudioClipList.wormVomitAttackSound,
        [CharacterAnimationType.Attack_Special] = AudioClipList.wormExplodeOnDeathSound
    };

    #endregion

    #region Horses

    public readonly static Dictionary<CharacterAnimationType, string> horseAudioDictionary = new Dictionary<CharacterAnimationType, string>
    {
        [CharacterAnimationType.Attack_Normal] = AudioClipList.batSwarmAttackSound
    };

    #endregion

}
