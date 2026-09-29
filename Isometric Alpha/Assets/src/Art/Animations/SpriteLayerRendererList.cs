using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteLayerRendererList : MonoBehaviour
{
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
    //optional, and deliberately never falls back to the body: only prefabs that sit in the terrain have one
    [SerializeField]
    private SpriteRenderer terrainRenderer;
    #endregion

    private bool instantiated = false;
    private ColorReplaceSchema colorSchema;
    private Dictionary<SpriteLayer, SpriteRenderer> spriteLayers;

    public PolygonCollider2D bodyCollider;

    public void Awake()
    {
        if(instantiated)
        {
            return;
        }

        spriteLayers = new Dictionary<SpriteLayer, SpriteRenderer>()
        {
            [SpriteLayer.Body] = bodyRenderer,

            [SpriteLayer.Shield_Back] = orBody(shieldBackRenderer),
            [SpriteLayer.Weapon] = orBody(weaponRenderer),
            [SpriteLayer.Cloak] = orBody(cloakRenderer),
            [SpriteLayer.Face] = orBody(faceRenderer),
            [SpriteLayer.Hair] = orBody(hairRenderer),
            [SpriteLayer.Shield_Front] = orBody(shieldFrontRenderer),

            [SpriteLayer.Terrain] = terrainRenderer
        };

        foreach(SpriteRenderer renderer in allRenderers)
        {
            renderer.RegisterSpriteChangeCallback(onSpriteChange);
        }


        foreach(SpriteRenderer renderer in allRenderers)
        {
            registeredBehaviours[renderer] = new Dictionary<Component, List<RegisterBehaviour>>();
        }

        if(bodyCollider != null)
        {
            registerBehaviour(SpriteLayer.Body, this, () => RendererUtil.updatePolygonCollider(bodyRenderer, bodyCollider));
        }

        instantiated = true;
    }

    private SpriteRenderer orBody(SpriteRenderer renderer)
    {
        return renderer != null ? renderer : bodyRenderer;
    }

    //null when the prefab has no terrain renderer
    public SpriteRenderer this[SpriteLayer layer]
    {
        get {
                return spriteLayers[layer];
            }
    }

    public bool hasTerrainRenderer { get { return terrainRenderer != null; } }

    //the renderers that make up the character's appearance, which leaves out the terrain renderer
    private IEnumerable<SpriteRenderer> characterRenderers
    {
        get
        {
            foreach(SpriteLayer layer in EnumUtil.CharacterSpriteLayers)
            {
                yield return spriteLayers[layer];
            }
        }
    }

    private IEnumerable<SpriteRenderer> allRenderers
    {
        get
        {
            foreach(SpriteRenderer renderer in characterRenderers)
            {
                yield return renderer;
            }

            if(terrainRenderer != null)
            {
                yield return terrainRenderer;
            }
        }
    }

    public void setFlipX(bool flip)
    {
        //flipping doesn't fire the sprite change callback, so the collider has to be rebuilt here
        bool bodyFlipChanged = bodyRenderer.flipX != flip;

        //the terrain sprite belongs to the map rather than the character, so it keeps its own facing
        foreach(SpriteRenderer renderer in characterRenderers)
        {
            renderer.flipX = flip;
        }

        if(bodyFlipChanged && bodyCollider != null)
        {
            RendererUtil.updatePolygonCollider(bodyRenderer, bodyCollider);
        }
    }

    //the terrain renderer's visibility is owned by the terrain state, so these leave it alone
    public void setToSingleLayer(SpriteLayer spriteLayer)
    {
        foreach(SpriteLayer layer in EnumUtil.CharacterSpriteLayers)
        {
            spriteLayers[layer].enabled = layer == spriteLayer;
        }
    }

    public void enableAllLayers()
    {
        foreach(SpriteRenderer renderer in characterRenderers)
        {
            renderer.enabled = true;
        }
    }

    public void ignoreColorReplace()
    {
        foreach(SpriteRenderer renderer in characterRenderers)
        {
            renderer.material.SetFloat(replaceVarName, Constants.falseFloatToBool);
        }
    }

    public void interpretSchema(ColorReplaceSchema schema)
    {
        colorSchema = schema;

        foreach(SpriteLayer layer in EnumUtil.CharacterSpriteLayers)
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

        foreach(SpriteLayer layer in EnumUtil.CharacterSpriteLayers)
        {
            outlineRenderer.material.SetTexture("_" + layer.ToString(), spriteLayers[layer].sprite.texture);
        }

        outlineRenderer.material.SetColor(outlineColorVarName, color);

        outlineRenderer.sprite = SpriteUtil.createBlankSpriteFromTemplate(spriteLayers[SpriteLayer.Body].sprite);
        outlineRenderer.flipX = spriteLayers[SpriteLayer.Body].flipX;
        outlineRenderer.sortingLayerID = spriteLayers[SpriteLayer.Body].sortingLayerID;
        outlineRenderer.sortingOrder = spriteLayers[SpriteLayer.Body].sortingOrder;

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

    public Color getOutlineColor()
    {
        return outlineRenderer.material.GetColor(outlineColorVarName);
    }

    #endregion

    #region Terrain
    //only prefabs with a terrain renderer take part, so the player's own renderer list is never touched by this
    private void OnEnable()
    {
        if(terrainRenderer == null)
        {
            return;
        }

        TerrainVisibilityManager.OnTerrainVisibilityChange.AddListener(applyTerrainState);
        applyTerrainState(TerrainVisibilityManager.currentTerrainHiddenState);
    }

    private void OnDisable()
    {
        TerrainVisibilityManager.OnTerrainVisibilityChange.RemoveListener(applyTerrainState);
    }

    public void setTerrainSprite(Sprite sprite)
    {
        if(terrainRenderer == null)
        {
            return;
        }

        terrainRenderer.sprite = sprite;
        applyTerrainState(TerrainVisibilityManager.currentTerrainHiddenState);
    }

    //the character layers act like terrain tilemaps and the terrain layer like a shown-while-terrain-hidden tilemap,
    //mirroring what TerrainVisibilityManager does to the map itself.
    //forceRenderingOff hides the character layers without disturbing which of them setToSingleLayer enabled
    private void applyTerrainState(TerrainHiddenState terrainState)
    {
        if(terrainRenderer == null)
        {
            return;
        }

        if(terrainRenderer.sprite == null)
        {
            setCharacterTerrainVisibility(true, SpriteMaskInteraction.None);
            terrainRenderer.enabled = false;
            return;
        }

        switch(terrainState)
        {
            case TerrainHiddenState.BehindTerrain:
                setCharacterTerrainVisibility(true, SpriteMaskInteraction.VisibleOutsideMask);

                terrainRenderer.enabled = true;
                terrainRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                break;
            case TerrainHiddenState.TerrainHidden:
                setCharacterTerrainVisibility(false, SpriteMaskInteraction.None);

                terrainRenderer.enabled = true;
                terrainRenderer.maskInteraction = SpriteMaskInteraction.None;
                break;
            default:
                setCharacterTerrainVisibility(true, SpriteMaskInteraction.None);

                terrainRenderer.enabled = false;
                break;
        }
    }

    private void setCharacterTerrainVisibility(bool visible, SpriteMaskInteraction maskInteraction)
    {
        foreach(SpriteRenderer renderer in characterRenderers)
        {
            renderer.forceRenderingOff = !visible;
            renderer.maskInteraction = maskInteraction;
        }
    }
    #endregion

    private Dictionary<SpriteRenderer, Dictionary<Component, List<RegisterBehaviour>>> registeredBehaviours = new();

    public void registerBehaviour(SpriteLayer layer, Component component, RegisterBehaviour behaviour)
    {
        if(!registeredBehaviours[spriteLayers[layer]].ContainsKey(component))
        {
            registeredBehaviours[spriteLayers[layer]][component] = new List<RegisterBehaviour>(); 
        } 

        registeredBehaviours[spriteLayers[layer]][component].Add(behaviour);
    }

    // public void UnregisterBehaviour(SpriteLayer layer, Component component, RegisterBehaviour behaviour)
    // {
    //     if(!registeredBehaviours[spriteLayers[layer]].ContainsKey(component))
    //     {
    //         registeredBehaviours[spriteLayers[layer]][component] = new List<RegisterBehaviour>(); 
    //     } 
    // }

    public void unregisterComponent(SpriteLayer layer, Component component)
    {
        if(!registeredBehaviours[spriteLayers[layer]].ContainsKey(component))
        {
            return;
        } 

        registeredBehaviours[spriteLayers[layer]].Remove(component);
    }

    private void onSpriteChange(SpriteRenderer renderer)
    {
        foreach(List<RegisterBehaviour> behaviours in registeredBehaviours[renderer].Values)
        {
            foreach(RegisterBehaviour behaviour in behaviours)
            {
                behaviour();
            }
        }
    }

}
