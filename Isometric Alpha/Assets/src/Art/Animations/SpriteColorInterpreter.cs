using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorReplaceVARNames
{
    _R_Replace,
    _G_Replace,
    _B_Replace,
    _C_Replace,
    _M_Replace,
    _Y_Replace,
    _O_Replace,
    _V_Replace,
    _T_Replace,
    _S_Replace,
    _P_Replace
}

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

        IEnumerable allSpriteLayers = EnumUtil.getValues<SpriteLayer>();
        foreach(SpriteLayer layer in allSpriteLayers)
        {
            setRendererMaterial(layer);
        }
    }

    private void setRendererMaterial(SpriteLayer layer)
    {
        SpriteRenderer renderer = rendererList[layer];

        IEnumerable allColorSlots = EnumUtil.getValues<ColorReplaceSlot>();
        foreach(ColorReplaceSlot slot in allColorSlots)
        {
            renderer.material.SetColor(slot.getMaterialVARName(), colorSchema.getColor(layer, slot));
        }
    }

}
