using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerRendererList : MonoBehaviour
{
    private readonly static Dictionary<Sprite, Sprite> outlineCache = new Dictionary<Sprite, Sprite>();

    private const string replaceVarName = "_Replace";    

    private const string blackBorderSizeXVarName = "_BlackBorderSizeX";
    private const string blackBorderSizeYVarName = "_BlackBorderSizeY";
    private const string colorOutlineSizeXVarName = "_ColorOutlineSizeX";
    private const string colorOutlineSizeYVarName = "_ColorOutlineSizeY";

    private const string blackBorderColorVarName = "_BlackBorderColor";
    private const string outlineColorVarName = "_OutlineColor";


    #region SpriteRenderers
    [SerializeField]
    private SpriteRenderer outlineRenderer;
    [SerializeField]
    private SpriteRenderer shieldBackRenderer;
    [SerializeField]
    private SpriteRenderer weaponRenderer;
    [SerializeField]
    private SpriteRenderer bodyRenderer;
    [SerializeField]
    private SpriteRenderer cloakRenderer;
    [SerializeField]
    private SpriteRenderer faceRenderer;
    [SerializeField]
    private SpriteRenderer hairRenderer;
    [SerializeField]
    private SpriteRenderer shieldFrontRenderer;
    #endregion

    private bool instantiated = false;
    private ColorReplaceSchema colorSchema;
    private Dictionary<SpriteLayer, SpriteRenderer> spriteLayers;

    public void Awake()
    {
        if(instantiated)
        {
            return;
        }

        spriteLayers = new Dictionary<SpriteLayer, SpriteRenderer>()
        {
            [SpriteLayer.Shield_Back] = shieldBackRenderer,  
            [SpriteLayer.Weapon] = weaponRenderer,  
            [SpriteLayer.Body] = bodyRenderer,  
            [SpriteLayer.Cloak] = cloakRenderer,  
            [SpriteLayer.Face] = faceRenderer,  
            [SpriteLayer.Hair] = hairRenderer,  
            [SpriteLayer.Shield_Front] = shieldFrontRenderer  
        };

        instantiated = true;
    }

    public SpriteRenderer this[SpriteLayer layer]
    {
        get { return spriteLayers[layer]; }
    }

    public void setFlipX(bool flip)
    {
        foreach(SpriteRenderer renderer in spriteLayers.Values)
        {
            renderer.flipX = flip;
        }
    }

    public void setToSingleLayer(SpriteLayer spriteLayer)
    {
        foreach(KeyValuePair<SpriteLayer, SpriteRenderer> kvp in spriteLayers)
        {
            kvp.Value.enabled = kvp.Key == spriteLayer;
        }
    }

    public void enableAllLayers()
    {
        foreach(SpriteRenderer renderer in spriteLayers.Values)
        {
            renderer.enabled = true;
        }
    }

    public void ignoreColorReplace()
    {
        foreach(SpriteRenderer renderer in spriteLayers.Values)
        {
            renderer.material.SetFloat(replaceVarName, Constants.falseFloatToBool);
        }
    }

    public void interpretSchema(ColorReplaceSchema schema)
    {
        colorSchema = schema;

        foreach(SpriteLayer layer in EnumUtil.SpriteLayers)
        {
            applySchemaToLayer(layer);
        }
    }

    private void applySchemaToLayer(SpriteLayer layer)
    {
        foreach(ColorReplacementSlot slot in EnumUtil.ColorReplacementSlots)
        {
            spriteLayers[layer].material.SetColor(slot.getMaterialVARName(), colorSchema.getColor(layer, slot));
        }
    }


    #region Outline
    public void createOutline(Color color, float sizeMod = 4f)
    {
        outlineRenderer.enabled = true;

        foreach(SpriteLayer layer in EnumUtil.SpriteLayers)
        {
            outlineRenderer.material.SetTexture("_" + layer.ToString(), spriteLayers[layer].sprite.texture);
        }

        outlineRenderer.material.SetColor(outlineColorVarName, color);

        outlineRenderer.sprite = createBlankSpriteFromTemplate(spriteLayers[SpriteLayer.Body].sprite);
        outlineRenderer.flipX = spriteLayers[SpriteLayer.Body].flipX;

        float sizeX = sizeMod/spriteLayers[SpriteLayer.Body].sprite.texture.width;
        float sizeY = sizeMod/spriteLayers[SpriteLayer.Body].sprite.texture.height;

        outlineRenderer.material.SetFloat(blackBorderSizeXVarName, sizeX/4f);
        outlineRenderer.material.SetFloat(blackBorderSizeYVarName, sizeY/4f);
        outlineRenderer.material.SetFloat(colorOutlineSizeXVarName, sizeX);
        outlineRenderer.material.SetFloat(colorOutlineSizeYVarName, sizeY);

        outlineRenderer.material.SetColor(blackBorderColorVarName, Color.black);
    }

    public void removeOutline()
    {
        outlineRenderer.enabled = false;
    }

    #endregion

    #region Textures

    private static Sprite createBlankSpriteFromTemplate(Sprite template)
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

    #endregion

}
