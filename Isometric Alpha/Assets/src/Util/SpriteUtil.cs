using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void RegisterBehaviour();

public static class SpriteUtil
{

    private readonly static Dictionary<Sprite, Sprite> outlineCache = new Dictionary<Sprite, Sprite>();
    private static readonly Dictionary<Sprite, float> topCache = new Dictionary<Sprite, float>();

    public static Sprite loadSpriteFromResources(string spriteName)
    {
        if(spriteName == null)
        {
            return null;
        }

        switch(spriteName)
        {
            case HoverMessageList.actionTypePrefix + AbilityList.abilityActionTypeName:
            case HoverMessageList.actionTypePrefix + AbilityList.attackActionTypeName:
            case HoverMessageList.actionTypePrefix + AbilityList.itemActionTypeName:
            case HoverMessageList.actionTypePrefix + AbilityList.passiveActionTypeName:
            case HoverMessageList.actionTypePrefix + AbilityList.equippedPassiveActionTypeName:
            case HoverMessageList.traitTypePrefix + TraitList.boostName:
            case HoverMessageList.traitTypePrefix + TraitList.chargeName:
            case HoverMessageList.traitTypePrefix + AbilityList.equippedPassiveActionTypeName:
            case HoverMessageList.traitTypePrefix + TraitList.foeTypeName:
            case HoverMessageList.traitTypePrefix + TraitList.influenceName:
            case HoverMessageList.traitTypePrefix + TraitList.mentalName:
            case HoverMessageList.traitTypePrefix + TraitList.onDeathName:
            case HoverMessageList.traitTypePrefix + TraitList.protectionName:
            case HoverMessageList.traitTypePrefix + TraitList.sizeName:
            case HoverMessageList.traitTypePrefix + TraitList.targetPriorityName:
            case HoverMessageList.traitTypePrefix + TraitList.woundName:
            case IconList.actionTypeIconName:
            case IconList.traitTypeIconName:
            case IconList.armorTypeIconName:
                spriteName = IconList.typeIconName;
                break;
            case IconList.bonusArmorIconName:
                spriteName = IconList.armorScoreIconName;
                break;
            default:
                break;
        }

        Sprite sprite = Resources.Load<Sprite>(spriteName);

        if (sprite != null)
        {
            return sprite;
        }
        else
        {
            sprite = Resources.Load<Sprite>(spriteName.Replace(" ", ""));

            return sprite;
        }
    }

    public static Vector3 getTopOfBounds(SpriteRenderer renderer, float multiplier = 1f)
    {
        if(renderer == null || renderer.sprite == null)
        {
            return Vector3.zero;
        }

        return renderer.transform.TransformPoint(new Vector3(0f, getOpaqueTopLocal(renderer.sprite)*multiplier, 0f));
    }

    private static float getOpaqueTopLocal(Sprite sprite)
    {
        if (topCache.TryGetValue(sprite, out float top))
        {
            return top;
        }

        List<Vector2> shapePoints = new List<Vector2>();
        top = float.MinValue;

        int shapeCount = sprite.GetPhysicsShapeCount();

        for (int shape = 0; shape < shapeCount; shape++)
        {
            sprite.GetPhysicsShape(shape, shapePoints);

            foreach (Vector2 point in shapePoints)
            {
                top = Mathf.Max(top, point.y);
            }
        }

        // no physics shape (e.g. a fully transparent sprite): fall back to the top of the rect
        if (shapeCount == 0)
        {
            top = sprite.bounds.max.y;
        }

        topCache[sprite] = top;
        return top;
    }

    public static Sprite createBlankSpriteFromTemplate(Sprite template)
    {
        if(outlineCache.ContainsKey(template))
        {
            return outlineCache[template];
        } 

        Sprite outline = Sprite.Create(createBlankTextureFromTemplate(template.texture),
                            template.rect,
                            new Vector2(template.pivot.x / template.rect.width, template.pivot.y / template.rect.height),
                            template.pixelsPerUnit);

        outlineCache[template] = outline;

        return outline;
    }

    private static Texture2D createBlankTextureFromTemplate(Texture2D template)
    {
        Texture2D tex = new Texture2D(template.width, template.height, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;

        Color32[] pixels = new Color32[template.width * template.height];

        tex.SetPixels32(pixels);
        tex.Apply();
        return tex;
    }

}
