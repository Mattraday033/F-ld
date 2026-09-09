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

    private Dictionary<SpriteLayer, SpriteRenderer> spriteLayers;

    public SpriteRenderer shieldBackRenderer;
    public SpriteRenderer weaponRenderer;
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer cloakRenderer;
    public SpriteRenderer faceRenderer;
    public SpriteRenderer hairRenderer;
    public SpriteRenderer shieldFrontRenderer;

    private void Awake()
    {
        interpretSchema(ColorSchemaList.getSchema(MonsterNameList.spearman));

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
    }

    public void interpretSchema(ColorReplaceSchema schema)
    {
        colorSchema = schema;

        setRendererMaterial(shieldBackRenderer, SpriteLayer.Shield_Back);
        setRendererMaterial(weaponRenderer, SpriteLayer.Weapon);
        setRendererMaterial(bodyRenderer, SpriteLayer.Body);
        setRendererMaterial(cloakRenderer, SpriteLayer.Cloak);
        setRendererMaterial(faceRenderer, SpriteLayer.Face);
        setRendererMaterial(hairRenderer, SpriteLayer.Hair);
        setRendererMaterial(shieldFrontRenderer, SpriteLayer.Shield_Front);
    }

    private void setRendererMaterial(SpriteRenderer renderer, SpriteLayer section)
    {
        renderer.material.SetColor(ColorReplaceVARNames._R_Replace.ToString(), colorSchema.getColor(section, ColorReplaceSlot.R));
        renderer.material.SetColor(ColorReplaceVARNames._G_Replace.ToString(), colorSchema.getColor(section, ColorReplaceSlot.G));
        renderer.material.SetColor(ColorReplaceVARNames._B_Replace.ToString(), colorSchema.getColor(section, ColorReplaceSlot.B));
        renderer.material.SetColor(ColorReplaceVARNames._C_Replace.ToString(), colorSchema.getColor(section, ColorReplaceSlot.C));
        renderer.material.SetColor(ColorReplaceVARNames._M_Replace.ToString(), colorSchema.getColor(section, ColorReplaceSlot.M));
        renderer.material.SetColor(ColorReplaceVARNames._Y_Replace.ToString(), colorSchema.getColor(section, ColorReplaceSlot.Y));
        renderer.material.SetColor(ColorReplaceVARNames._O_Replace.ToString(), colorSchema.getColor(section, ColorReplaceSlot.O));
        renderer.material.SetColor(ColorReplaceVARNames._V_Replace.ToString(), colorSchema.getColor(section, ColorReplaceSlot.V));
        renderer.material.SetColor(ColorReplaceVARNames._T_Replace.ToString(), colorSchema.getColor(section, ColorReplaceSlot.T));
        renderer.material.SetColor(ColorReplaceVARNames._S_Replace.ToString(), colorSchema.getColor(section, ColorReplaceSlot.S));
        renderer.material.SetColor(ColorReplaceVARNames._P_Replace.ToString(), colorSchema.getColor(section, ColorReplaceSlot.P));
    }

}
