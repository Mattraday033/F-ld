using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAppearanceType
{
    Unarmed,
    Club,
    Javelin,
    Knife,
    OneHandedSword,
    Pickaxe,
    Shield_Axe,
    Shield_Spear,
    Shovel,
    SignalTorch,
    SpearSimple,
    SpearGreat,
    Staff,
    TwoHandedAxe,
    TwoHandedHammer,
    Whip
}
public enum WeaponPose
{
    PoseAgnostic,
    NoWeapon,
    Javelin,
    OneHandedWeapon,
    OneHandedStab,
    OneHandedSwing,
    Polearm,
    Shield,
    ShieldAxe,
    ShieldSpear,
    TwoHandedStab,
    TwoHandedSwing,
    Whip
}

public enum BodyType
{
    Naked_M,
    Naked_F,
    Rags_M,
    RagsTorn_M,
    Rags_F,
    PlainRobe,
    LovashiArmor
}

public enum FacialFeatureType
{
    None,
    Short_Goatee
}

public enum HairType
{
    Bald,
    Short_Ruffled
}

public enum CloakType
{
    None
}

#nullable enable
public class Costume
{

    public readonly BodyType bodyType;
    private readonly Dictionary<CharacterAnimationType, WeaponPose> weaponAnimationInfo;
    public readonly WeaponAppearanceType weaponAppearanceType;
    public readonly FacialFeatureType facialFeatureType;
    public readonly HairType hairType;
    public readonly CloakType cloakType;

    public Costume  (
                        BodyType bodyType, 
                        WeaponAppearanceType weaponAppearanceType = WeaponAppearanceType.Unarmed,
                        FacialFeatureType facialFeatureType = FacialFeatureType.None,
                        HairType hairType = HairType.Bald,
                        CloakType cloakType = CloakType.None
                    )
    {
        this.bodyType = bodyType;
        this.weaponAnimationInfo = WeaponAnimationInfoFactory.getWeaponAppearanceInfo(weaponAppearanceType);
        this.weaponAppearanceType = weaponAppearanceType;
        this.facialFeatureType = facialFeatureType;
        this.hairType = hairType;
        this.cloakType = cloakType;
    }

    private WeaponPose getWeaponPose(CharacterAnimationType animationType)
    {
        if(weaponAnimationInfo.ContainsKey(animationType))
        {
            return weaponAnimationInfo[animationType];
        } else
        {
            return WeaponPose.NoWeapon;
        }
    }

    public Sprite[] getSprite(SpriteLayer layer, CharacterAnimationType animationType)
    {
        SpritePath spritePath = SpritePath.NoSprite;

        switch(layer)
        {
            case SpriteLayer.Body:
                spritePath = getSpritePath(layer, bodyType, animationType);
                break;
            case SpriteLayer.Weapon:
                spritePath = getSpritePath(layer, weaponAppearanceType, animationType, getWeaponPose(animationType));
                break;
            case SpriteLayer.Face:
                spritePath = getSpritePath(layer, facialFeatureType, animationType);
                break;
            case SpriteLayer.Hair:
                spritePath = getSpritePath(layer, hairType, animationType);
                break;
        }

        return SpriteList.getSprites(spritePath);
    }

    public static Costume getDefaultCostume()
    {
        return new Costume(
                            bodyType: BodyType.LovashiArmor,
                            weaponAppearanceType: WeaponAppearanceType.SpearSimple,
                            facialFeatureType: FacialFeatureType.Short_Goatee,
                            hairType: HairType.Short_Ruffled,
                            cloakType: CloakType.None
                            );
    }

    // private static SpritePath getWeaponAppearanceTypeSpritePath(CharacterAnimationType animationType, WeaponAppearanceType appearanceType)
    // {
    //     if(Enum.TryParse(SpriteLayer.Weapon.ToString() + "_" + 
    //                         appearanceType.ToString() + "_" + 
    //                         animationType.ToString(),
    //                         out SpritePath spritePath))
    //     {
    //         return spritePath;
    //     }

    //     return SpritePath.NoSprite;
    // }

    private static SpritePath getSpritePath(SpriteLayer layer, 
                                            Enum type, 
                                            CharacterAnimationType animationType,
                                            WeaponPose weaponPose = WeaponPose.PoseAgnostic)
    {
        string spritePathName = layer.ToString() + "_" + type.ToString() + "_";

        if(weaponPose != WeaponPose.PoseAgnostic)
        {
            spritePathName += weaponPose.ToString() + "_";
        }

        spritePathName += animationType.ToString();

        if(Enum.TryParse( spritePathName,
                            out SpritePath spritePath))
        {
            return spritePath;
        }

        return SpritePath.NoSprite;
    }

}

public static class WeaponAnimationInfoFactory
{
    public static Dictionary<CharacterAnimationType, WeaponPose> getWeaponAppearanceInfo(WeaponAppearanceType type)
    {
        switch(type)
        {
            case WeaponAppearanceType.SpearSimple:
            case WeaponAppearanceType.SpearGreat:
                return new Dictionary<CharacterAnimationType, WeaponPose>()
                {
                    [CharacterAnimationType.Idle_Back] = WeaponPose.Polearm,
                    [CharacterAnimationType.Idle_Front] = WeaponPose.Polearm,
                    [CharacterAnimationType.OOC_Idle_Back] = WeaponPose.Polearm,
                    [CharacterAnimationType.OOC_Idle_Front] = WeaponPose.Polearm,
                    [CharacterAnimationType.Attack_Normal_Back] = WeaponPose.TwoHandedStab,
                    [CharacterAnimationType.Attack_Normal_Front] = WeaponPose.TwoHandedStab
                };
            default:
                return new Dictionary<CharacterAnimationType, WeaponPose>();
        }
    }
}