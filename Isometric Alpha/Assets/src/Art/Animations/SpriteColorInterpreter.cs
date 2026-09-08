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

    public SpriteRenderer weaponRenderer;
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer faceRenderer;
    public SpriteRenderer hairRenderer;
    public SpriteRenderer cloakRenderer;
    public SpriteRenderer shieldRenderer;

    void Start()
    {
        interpretSchema(ColorSchemaList.getSchema(MonsterNameList.spearman));
    }

    public void interpretSchema(ColorReplaceSchema schema)
    {
        colorSchema = schema;

        setRendererMaterial(weaponRenderer, SpriteSection.Weapon);
        setRendererMaterial(bodyRenderer, SpriteSection.Body);
        setRendererMaterial(faceRenderer, SpriteSection.Face);
        setRendererMaterial(hairRenderer, SpriteSection.Hair);
        setRendererMaterial(cloakRenderer, SpriteSection.Cloak);
        setRendererMaterial(shieldRenderer, SpriteSection.Shield);
    }

    private void setRendererMaterial(SpriteRenderer renderer, SpriteSection section)
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
