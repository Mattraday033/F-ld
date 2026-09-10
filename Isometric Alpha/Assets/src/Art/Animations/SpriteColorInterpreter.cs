using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteColorInterpreter : MonoBehaviour
{

    private ColorReplaceSchema colorSchema;
    public SpriteLayerRendererList rendererList;

    private void Awake()
    {
        rendererList.Awake();
        
        interpretSchema(ColorSchemaList.getSchema(MonsterNameList.spearman));
    }

    public void interpretSchema(ColorReplaceSchema schema)
    {
        colorSchema = schema;

        foreach(SpriteLayer layer in EnumUtil.SpriteLayers)
        {
            setRendererMaterial(layer);
        }
    }

    private void setRendererMaterial(SpriteLayer layer)
    {
        SpriteRenderer renderer = rendererList[layer];

        foreach(ColorReplacementSlot slot in EnumUtil.ColorReplacementSlots)
        {
            renderer.material.SetColor(slot.getMaterialVARName(), colorSchema.getColor(layer, slot));
        }
    }

}
