using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationSFXDictionaryList
{
    #region Humans

    public readonly static Dictionary<CharacterAnimationType, SFXType> playerHumanAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.WeaponSwing
    };


    public readonly static Dictionary<CharacterAnimationType, SFXType> maleHumanAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.WeaponSwing,
        [CharacterAnimationType.Death] = SFXType.MaleHuman_Death,
        [CharacterAnimationType.Secondary_Death] = SFXType.MaleHuman_Death 
    };

    public readonly static Dictionary<CharacterAnimationType, SFXType> femaleHumanAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.WeaponSwing,
        [CharacterAnimationType.Death] = SFXType.FemaleHuman_Death,
        [CharacterAnimationType.Secondary_Death] = SFXType.FemaleHuman_Death
    };

    public readonly static Dictionary<CharacterAnimationType, SFXType> whipAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.Whip,
        [CharacterAnimationType.Death] = SFXType.MaleHuman_Death,
        [CharacterAnimationType.Secondary_Death] = SFXType.MaleHuman_Death 
    };

    #endregion

    #region Bats

    public readonly static Dictionary<CharacterAnimationType, SFXType> batSwarmAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.BatSwarm_BatSwarm,
        [CharacterAnimationType.Death] = SFXType.BatDeath1,
        [CharacterAnimationType.Secondary_Death] = SFXType.BatDeath1 
    };

    public readonly static Dictionary<CharacterAnimationType, SFXType> largeBatAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.BatAttack,
        [CharacterAnimationType.Death] = SFXType.BatDeath1,
        [CharacterAnimationType.Secondary_Death] = SFXType.BatDeath1 
    };

    public readonly static Dictionary<CharacterAnimationType, SFXType> bipedalBatAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.BatHowl,
        [CharacterAnimationType.Attack_Special] = SFXType.BatHowl,
        [CharacterAnimationType.Death] = SFXType.BatDeath2,
        [CharacterAnimationType.Secondary_Death] = SFXType.BatDeath2 
    };

    #endregion

    #region Worms

    public readonly static Dictionary<CharacterAnimationType, SFXType> biteWormAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.Bite,
        [CharacterAnimationType.Attack_Special] = SFXType.WormExplodeOnDeath
    };

    public readonly static Dictionary<CharacterAnimationType, SFXType> massiveWormAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        // [CharacterAnimationType.Attack_Normal] = AudioClipList.batSwarmAttackSound,
        [CharacterAnimationType.Attack_Special] = SFXType.WormExplodeOnDeath
    };

    public readonly static Dictionary<CharacterAnimationType, SFXType> summonWormAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.WormSummon,
        [CharacterAnimationType.Attack_Special] = SFXType.WormExplodeOnDeath
    };

    public readonly static Dictionary<CharacterAnimationType, SFXType> vomitWormAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.WormAcidVomit,
        [CharacterAnimationType.Attack_Special] = SFXType.WormExplodeOnDeath
    };

    #endregion

    #region Horses

    public readonly static Dictionary<CharacterAnimationType, SFXType> horseAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.HorseWhinny,
        [CharacterAnimationType.Death] = SFXType.Horse_Death,
        [CharacterAnimationType.Secondary_Death] = SFXType.Horse_Death
    };

    #endregion

    #region Saints

    public readonly static Dictionary<CharacterAnimationType, SFXType> stoneSaintAudioDictionary = new Dictionary<CharacterAnimationType, SFXType>
    {
        [CharacterAnimationType.Attack_Normal] = SFXType.RockIntro,
        [CharacterAnimationType.Death] = SFXType.RockIntro,
        [CharacterAnimationType.Secondary_Death] = SFXType.RockIntro,
    };

    #endregion

}
