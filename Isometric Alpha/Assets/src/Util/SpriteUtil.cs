using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void RegisterBehaviour();

public static class SpriteUtil
{

    private readonly static Dictionary<Sprite, Sprite> outlineCache = new Dictionary<Sprite, Sprite>();
    private static readonly Dictionary<Sprite, Vector2> opaqueTopCentreCache = new Dictionary<Sprite, Vector2>();

    //keyed by the name asked for, so a refilled description panel finds its icons without a Resources lookup each time
    private static readonly Dictionary<string, Sprite> loadedSpriteCache = new Dictionary<string, Sprite>();

    [RuntimeInitializeOnLoadMethod]
    private static void clearLoadedSpriteCache()
    {
        loadedSpriteCache.Clear();
    }

    public static Sprite loadSpriteFromResources(string spriteName)
    {
        if(spriteName == null)
        {
            return null;
        }

        //a cached sprite that Unity has since unloaded compares equal to null, so it's loaded again
        if(loadedSpriteCache.TryGetValue(spriteName, out Sprite cachedSprite) && cachedSprite != null)
        {
            return cachedSprite;
        }

        Sprite loadedSprite = loadUncachedSpriteFromResources(spriteName);

        if(loadedSprite != null)
        {
            loadedSpriteCache[spriteName] = loadedSprite;
        }

        return loadedSprite;
    }

    private static Sprite loadUncachedSpriteFromResources(string spriteName)
    {
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

        Vector2 topCentre = getOpaqueTopCentreLocal(renderer.sprite);

        //flipX mirrors the drawn pixels around the pivot without touching the transform
        float centreX = renderer.flipX ? -topCentre.x : topCentre.x;

        return renderer.transform.TransformPoint(new Vector3(centreX, topCentre.y*multiplier, 0f));
    }

    //x is the horizontal middle of the opaque pixels, y is their top, both in sprite-local space
    private static Vector2 getOpaqueTopCentreLocal(Sprite sprite)
    {
        if (opaqueTopCentreCache.TryGetValue(sprite, out Vector2 topCentre))
        {
            return topCentre;
        }

        List<Vector2> shapePoints = new List<Vector2>();
        float top = float.MinValue;
        float minX = float.MaxValue;
        float maxX = float.MinValue;

        int shapeCount = sprite.GetPhysicsShapeCount();

        for (int shape = 0; shape < shapeCount; shape++)
        {
            sprite.GetPhysicsShape(shape, shapePoints);

            foreach (Vector2 point in shapePoints)
            {
                top = Mathf.Max(top, point.y);
                minX = Mathf.Min(minX, point.x);
                maxX = Mathf.Max(maxX, point.x);
            }
        }

        // no physics shape (e.g. a fully transparent sprite): fall back to the top centre of the rect
        if (shapeCount == 0)
        {
            topCentre = new Vector2(sprite.bounds.center.x, sprite.bounds.max.y);
        } else
        {
            topCentre = new Vector2((minX + maxX) / 2f, top);
        }

        opaqueTopCentreCache[sprite] = topCentre;
        return topCentre;
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
