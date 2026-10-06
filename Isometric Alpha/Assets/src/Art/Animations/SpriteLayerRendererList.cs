using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteLayerRendererList : MonoBehaviour
{
    private const string replaceVarName = "_Replace";    

    private static readonly int blackBorderSizeXID = Shader.PropertyToID("_BlackBorderSizeX");
    private static readonly int blackBorderSizeYID = Shader.PropertyToID("_BlackBorderSizeY");
    private static readonly int colorOutlineSizeXID = Shader.PropertyToID("_ColorOutlineSizeX");
    private static readonly int colorOutlineSizeYID = Shader.PropertyToID("_ColorOutlineSizeY");

    private static readonly int blackBorderColorID = Shader.PropertyToID("_BlackBorderColor");
    private static readonly int outlineColorID = Shader.PropertyToID("_OutlineColor");

    //the outline shader has one texture slot per character layer, named after the layer
    private static readonly Dictionary<SpriteLayer, int> layerTextureIDs = getLayerTextureIDs();

    private static Dictionary<SpriteLayer, int> getLayerTextureIDs()
    {
        Dictionary<SpriteLayer, int> textureIDs = new Dictionary<SpriteLayer, int>();

        foreach(SpriteLayer layer in EnumUtil.CharacterSpriteLayers)
        {
            textureIDs[layer] = Shader.PropertyToID("_" + layer.ToString());
        }

        return textureIDs;
    }

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

        if(outlineRenderer != null)
        {
            //the prefab's outline renderer starts enabled, which would run the outline shader for nothing until the first removeOutline
            outlineRenderer.enabled = false;

            foreach(SpriteLayer layer in EnumUtil.CharacterSpriteLayers)
            {
                SpriteRenderer renderer = spriteLayers[layer];

                if(!outlineTextureIDs.ContainsKey(renderer))
                {
                    outlineTextureIDs[renderer] = new List<int>();
                }

                outlineTextureIDs[renderer].Add(layerTextureIDs[layer]);
            }
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

        if(outlineRenderer != null)
        {
            outlineRenderer.flipX = flip;
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

    //pushing the body back on z lets the colliders of anything overlapping it win the mouse hover
    public void decreaseBodyColliderPriority()
    {
        Vector3 localPosition = bodyRenderer.transform.localPosition;

        localPosition.z = lowBodyColliderPriorityZ;

        bodyRenderer.transform.localPosition = localPosition;
    }

    private const float lowBodyColliderPriorityZ = 1f;

    public void enableAllLayers()
    {
        setAllLayersEnabled(true);
    }

    public void disableAllLayers()
    {
        setAllLayersEnabled(false);
    }

    private void setAllLayersEnabled(bool enabled)
    {
        foreach(SpriteRenderer renderer in characterRenderers)
        {
            renderer.enabled = enabled;
        }
    }

    public void setBodyColliderEnabled(bool enabled)
    {
        if(bodyCollider != null)
        {
            bodyCollider.enabled = enabled;
        }
    }

    public void setAlpha(float alpha)
    {
        foreach(SpriteRenderer renderer in characterRenderers)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
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
    //which of the outline shader's texture slots each renderer fills. A renderer standing in for missing layers fills several
    private readonly Dictionary<SpriteRenderer, List<int>> outlineTextureIDs = new();

    //fetched on the first createOutline, so a character that's never outlined doesn't get its own material instance
    private Material outlineMaterial;
    private Color outlineColor;
    private float outlineSizeMod;

    //true while the outline is showing, which is when each sprite change has to be copied onto it
    private bool outlineActive = false;
    //false once a sprite changes while the outline is hidden, so the next createOutline knows to copy every layer again
    private bool outlineSynced = false;

    public void createOutline(Color color, float sizeMod = 4f)
    {
        if(outlineRenderer == null || bodyRenderer.sprite == null)
        {
            return;
        }

        if(outlineMaterial == null)
        {
            outlineMaterial = outlineRenderer.material;
        }

        if(!outlineSynced || sizeMod != outlineSizeMod)
        {
            outlineSizeMod = sizeMod;

            matchOutlineToBodySprite();

            foreach(KeyValuePair<SpriteRenderer, List<int>> kvp in outlineTextureIDs)
            {
                setOutlineTextures(kvp.Key, kvp.Value);
            }

            outlineMaterial.SetColor(blackBorderColorID, Color.black);

            outlineSynced = true;
        }

        outlineColor = color;
        outlineMaterial.SetColor(outlineColorID, color);

        //neither of these fires the sprite change callback when changed directly on the body, so they're copied on every call
        outlineRenderer.flipX = bodyRenderer.flipX;
        outlineRenderer.sortingLayerID = bodyRenderer.sortingLayerID;
        outlineRenderer.sortingOrder = bodyRenderer.sortingOrder;

        outlineRenderer.enabled = true;
        outlineActive = true;
    }

    public void removeOutline()
    {
        if(outlineRenderer == null)
        {
            return;
        }

        outlineRenderer.enabled = false;
        outlineActive = false;
    }

    public Color getOutlineColor()
    {
        return outlineColor;
    }

    //copies one renderer's new sprite onto the outline. Runs inside the sprite change callback, ahead of the
    //registered behaviours, so it must never throw
    private void syncOutlineLayer(SpriteRenderer renderer)
    {
        //the terrain renderer isn't part of the outline
        if(!outlineTextureIDs.TryGetValue(renderer, out List<int> textureIDs))
        {
            return;
        }

        if(renderer == bodyRenderer)
        {
            matchOutlineToBodySprite();
        }

        setOutlineTextures(renderer, textureIDs);
    }

    //the outline's own sprite is blank, and only lends the body frame's rect for every layer's texture to be sampled at
    private void matchOutlineToBodySprite()
    {
        Sprite bodySprite = bodyRenderer.sprite;

        if(bodySprite == null)
        {
            return;
        }

        outlineRenderer.sprite = SpriteUtil.createBlankSpriteFromTemplate(bodySprite);

        //the sizes are in UV space, so they change whenever the body moves to a texture of a different size
        float sizeX = outlineSizeMod/bodySprite.texture.width;
        float sizeY = outlineSizeMod/bodySprite.texture.height;

        outlineMaterial.SetFloat(blackBorderSizeXID, sizeX/4f);
        outlineMaterial.SetFloat(blackBorderSizeYID, sizeY/4f);
        outlineMaterial.SetFloat(colorOutlineSizeXID, sizeX);
        outlineMaterial.SetFloat(colorOutlineSizeYID, sizeY);
    }

    private void setOutlineTextures(SpriteRenderer renderer, List<int> textureIDs)
    {
        Sprite sprite = renderer.sprite;

        //a layer left without a sprite adds nothing to the outline, so the outline's own blank texture stands in for it
        Texture texture = sprite != null ? sprite.texture : outlineRenderer.sprite.texture;

        foreach(int textureID in textureIDs)
        {
            outlineMaterial.SetTexture(textureID, texture);
        }
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
        if(outlineActive)
        {
            syncOutlineLayer(renderer);
        } else
        {
            outlineSynced = false;
        }

        foreach(List<RegisterBehaviour> behaviours in registeredBehaviours[renderer].Values)
        {
            foreach(RegisterBehaviour behaviour in behaviours)
            {
                behaviour();
            }
        }
    }

}
