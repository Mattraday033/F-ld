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
    Spear_Simple,
    Spear_Great,
    Staff,
    TwoHandedAxe,
    TwoHandedHammer,
    Whip
}
public enum WeaponPose
{
    No_Weapon,
    Javelin,
    OneHandedWeapon,
    OneHandedStab,
    OneHandedSwing,
    Polearm,
    Shield,
    Shield_Axe,
    Shield_Spear,
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
    Lovashi
}

public enum FacialHairType
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

public class Costume
{

    public readonly BodyType bodyType;
    private readonly Dictionary<CharacterAnimationType, WeaponPose> weaponAnimationInfo;
    public readonly FacialHairType facialHairType;
    public readonly HairType hairType;
    public readonly CloakType cloakType;

    public Costume  (
                        BodyType bodyType, 
                        WeaponAppearanceType weaponAppearanceType = WeaponAppearanceType.Unarmed,
                        FacialHairType facialHairType = FacialHairType.None,
                        HairType hairType = HairType.Bald,
                        CloakType cloakType = CloakType.None
                    )
    {
        this.bodyType = bodyType;
        this.weaponAnimationInfo = WeaponAnimationInfoFactory.getWeaponAppearanceInfo(weaponAppearanceType);
        this.facialHairType = facialHairType;
        this.hairType = hairType;
        this.cloakType = cloakType;
    }

    public WeaponPose getWeaponPose(CharacterAnimationType animationType)
    {
        if(weaponAnimationInfo.ContainsKey(animationType))
        {
            return weaponAnimationInfo[animationType];
        } else
        {
            return WeaponPose.No_Weapon;
        }
    }

}

public static class WeaponAnimationInfoFactory
{
    public static Dictionary<CharacterAnimationType, WeaponPose> getWeaponAppearanceInfo(WeaponAppearanceType type)
    {
        switch(type)
        {
            case WeaponAppearanceType.Spear_Simple:
            case WeaponAppearanceType.Spear_Great:
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


public static class AppearanceTypeToSpritePathConverter
{
    public static SpritePath convertWeaponPose(CharacterAnimationType animationType, WeaponPose pose)
    {
        return SpritePath.NoSprite;
    }
}