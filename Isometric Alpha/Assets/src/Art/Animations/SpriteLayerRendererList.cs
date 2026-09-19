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

        foreach(SpriteRenderer renderer in spriteLayers.Values)
        {
            renderer.RegisterSpriteChangeCallback(onSpriteChange);
        }


        foreach(SpriteRenderer renderer in spriteLayers.Values)
        {
            registeredBehaviours[renderer] = new Dictionary<Component, List<RegisterBehaviour>>();
        }

        instantiated = true;
    }

    public SpriteRenderer this[SpriteLayer layer]
    {
        get { 
                return spriteLayers[layer];
            }
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

        outlineRenderer.sprite = SpriteUtil.createBlankSpriteFromTemplate(spriteLayers[SpriteLayer.Body].sprite);
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
