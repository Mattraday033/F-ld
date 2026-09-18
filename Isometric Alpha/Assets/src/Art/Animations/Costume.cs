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

public interface IAppearance
{
    public void applyAppearance(SpriteLayerRendererList rendererList, 
                                CharacterAnimationType type = CharacterAnimationType.OOC_Idle_Front, 
                                bool updateColors = false);

    public bool large
    {
        get;
    }

    public bool withScale
    {
        get;
    }
}

public class SpriteDescription: IAppearance
{
    private string spriteName;
    private SortingLayerInfo sortingLayerInfo;
    private bool flipX;

    private Color _Tint = Color.white;
    private Color tint
    {
        get
        {
            if(useRubbleColor)
            {
                return ColorList.getRubbleColorFromLocationName();
            }

            return _Tint;
        }
        set
        {
            _Tint = value;
        }
    }
    private bool useRubbleColor;

    private bool _Large;
    public bool large
    {
        get
        {
            return _Large;
        }
    }

    private bool _WithScale;
    public bool withScale
    {
        get
        {
            return _WithScale;
        }
        private set
        {
            _WithScale = value;
        }
    }
    private float offset;

    public SpriteDescription(string spriteName = PrefabNames.blankTexture,
                                bool flipX = false,
                                bool large = false,
                                SortingLayerInfo sortingLayerInfo = null,
                                Color tint = default,
                                bool useRubbleColor = false,
                                bool withScale = true,
                                float offset = 0f
                                )
    {
        this.spriteName = spriteName;
        this.flipX = flipX;
        this._Large = large;
        
        this.sortingLayerInfo = sortingLayerInfo ?? SortingLayerManager.firstSortingLayerInfo;

        if(tint == default)
        {
            this.tint = Color.white;
        } else
        {
            this.tint = tint;
        }

        this.useRubbleColor = useRubbleColor;

        this._WithScale = withScale;
        this.offset = offset;
    }

    public void applyAppearance(SpriteLayerRendererList rendererList, CharacterAnimationType type = CharacterAnimationType.OOC_Idle_Front, bool updateColors = false)
    {
        if(rendererList == null)
        {
            return;
        }

        rendererList.ignoreColorReplace();
        rendererList.setToSingleLayer(SpriteLayer.Body);

        rendererList[SpriteLayer.Body].sprite = Helpers.loadSpriteFromResources(spriteName);
        rendererList[SpriteLayer.Body].color = tint;

        rendererList.setFlipX(flipX);

        sortingLayerInfo.setRendererSortingLayer(rendererList[SpriteLayer.Body]);

        if(offset == 0f)
        {
            return;
        }

        Transform transform = rendererList.GetComponent<RectTransform>();

        if(transform == null)
        {
            transform = rendererList.transform;
        }

        Vector3 currentPosition = transform.position;

        currentPosition.y -= offset;
        // Collider2D collider2D = interactable.GetComponent<Collider2D>();

        // if(collider2D != null)
        // {
        //     collider2D.offset += new Vector2(0f, offset);
        // }

        transform.position = currentPosition;

        // transform.
    }

}

public class Costume: IAppearance
{

    public readonly BodyType bodyType;
    private readonly Dictionary<CharacterAnimationType, WeaponPose> weaponAnimationInfo;
    public readonly WeaponAppearanceType weaponAppearanceType;
    public readonly FacialFeatureType facialFeatureType;
    public readonly HairType hairType;
    public readonly CloakType cloakType;

    private bool _Large;
    public bool large
    {
        get
        {
            return _Large;
        }
    }

    public bool withScale
    {
        get
        {
            return false;
        }
    }

    public Costume  (
                        BodyType bodyType, 
                        WeaponAppearanceType weaponAppearanceType = WeaponAppearanceType.Unarmed,
                        FacialFeatureType facialFeatureType = FacialFeatureType.None,
                        HairType hairType = HairType.Bald,
                        CloakType cloakType = CloakType.None,
                        bool large = false
                    )
    {
        this.bodyType = bodyType;
        this.weaponAnimationInfo = WeaponAnimationInfoFactory.getWeaponAppearanceInfo(weaponAppearanceType);
        this.weaponAppearanceType = weaponAppearanceType;
        this.facialFeatureType = facialFeatureType;
        this.hairType = hairType;
        this.cloakType = cloakType;

        this._Large = large;
    }

    private ColorReplaceSchema getColorSchema()
    {
        return ColorSchemaList.getSchema(MonsterNameList.spearman);
    }

    private WeaponPose getWeaponPose(CharacterAnimationType animationType)
    {
        // if(weaponAnimationInfo.ContainsKey(animationType))
        // {
        //     return weaponAnimationInfo[animationType];
        // } else
        // {
            return WeaponPose.NoWeapon;
        // }
    }

    public Sprite[] getSprites(SpriteLayer layer, CharacterAnimationType animationType)
    {
        SpritePath spritePath = SpritePath.NoSprite;

        switch(layer)
        {
            case SpriteLayer.Body:
                spritePath = getSpritePath(layer, bodyType, animationType, getWeaponPose(animationType));
                break;
            case SpriteLayer.Weapon:
                spritePath = getSpritePath(layer, weaponAppearanceType, animationType);
                break;
            case SpriteLayer.Face:
                spritePath = getSpritePath(layer, facialFeatureType, animationType, getWeaponPose(animationType));
                break;
            case SpriteLayer.Hair:
                spritePath = getSpritePath(layer, hairType, animationType, getWeaponPose(animationType));
                break;
        }

        return SpriteList.getSprites(spritePath);
    }

    public Sprite getSprite(SpriteLayer layer, CharacterAnimationType animationType)
    {
        return getSprites(layer, animationType)[0];
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

    private static SpritePath getSpritePath(SpriteLayer layer, 
                                            Enum type, 
                                            CharacterAnimationType animationType,
                                            WeaponPose weaponPose = WeaponPose.PoseAgnostic)
    {
        string poseName = "";

        if(weaponPose != WeaponPose.PoseAgnostic)
        {
            poseName = weaponPose.ToString() + "_";
        }

        do
        {
            if(Enum.TryParse(layer.ToString() + "_" + 
                                type.ToString() + "_" + 
                                poseName + 
                                animationType.ToString(),
                                out SpritePath spritePath))
            {
                return spritePath;
            } else if(Enum.TryParse(layer.ToString() + "_" + 
                                    type.ToString() + "_" + 
                                    animationType.ToString(),
                                    out spritePath))
            {
                return spritePath;
            } else
            {
                animationType = animationType.nextAnimationType();
            }
        } while(animationType != CharacterAnimationType.None);

        return SpritePath.NoSprite;
    }

    public void applyAppearance(SpriteLayerRendererList rendererList, CharacterAnimationType type = CharacterAnimationType.OOC_Idle_Front, bool updateColors = false)
    {
        if(rendererList == null)
        {
            return;
        }

        if(updateColors)
        {
            rendererList.interpretSchema(getColorSchema());
        }

        foreach(SpriteLayer layer in EnumUtil.SpriteLayers)
        {
            rendererList[layer].sprite = getSprite(layer, type);
        }
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