using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadshotDisplay : MonoBehaviour, IColorSchemaTarget
{
	public Image hairImage;
    public Image faceImage;
    public Image bodyImage;

    //the material instances this display gave its images, which Image doesn't destroy on its own
    private readonly List<Material> ownMaterials = new List<Material>();

    //head sprites live in SpriteLayers/Head and are named after their HeadType, e.g. Head_Human
    public static Sprite getHeadSprite(BodyType bodyType)
    {
        HeadType headType = bodyType.getHeadType();

        if(!Enum.TryParse("Head_" + headType.ToString(), out SpritePath spritePath))
        {
            Debug.LogWarning("[HeadshotDisplay] No head sprite for HeadType." + headType + " (BodyType." + bodyType + "), so the headshot uses a blank texture.");
            spritePath = SpritePath.NoSprite;
        }

        return SpriteList.getSprite(spritePath);
    }

    private Image getImage(SpriteLayer layer)
    {
        switch(layer)
        {
            case SpriteLayer.Body:
                return bodyImage;
            case SpriteLayer.Face:
                return faceImage;
            case SpriteLayer.Hair:
                return hairImage;
            default:
                return null;
        }
    }

    #region IColorSchemaTarget
    //Image.material hands back the shared material, so each image is given its own before its colors are changed
    public Material getColorSchemaMaterial(SpriteLayer layer)
    {
        Image image = getImage(layer);

        if(image == null)
        {
            return null;
        }

        if(!ownMaterials.Contains(image.material))
        {
            Material instance = new Material(image.material);

            ownMaterials.Add(instance);
            image.material = instance;
        }

        return image.material;
    }

    //changing a material's values doesn't tell the canvas to redraw
    public void onColorSchemaMaterialsChanged()
    {
        foreach(SpriteLayer layer in EnumUtil.CharacterSpriteLayers)
        {
            Image image = getImage(layer);

            if(image != null)
            {
                image.SetMaterialDirty();
            }
        }
    }
    #endregion

    private void OnDestroy()
    {
        foreach(Material material in ownMaterials)
        {
            Destroy(material);
        }

        ownMaterials.Clear();
    }
}
