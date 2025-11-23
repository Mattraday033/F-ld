using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum OutlineMode {Normal, Bold}

public class SpriteOutline
{
    private const string blackBorderSizeVarName = "_BlackBorderSize";
    private const string colorOutlineSizeVarName = "_ColorOutlineSize";

    private const float boldOutlineSize = 0.016f;
    private const float normalOutlineSize = 0.002f;

    private Material outlineMaterial;
    private Material defaultMaterial;
    private SpriteRenderer spriteRenderer;


    public SpriteOutline()
    {
        Material outlineMaterialTemplate = Resources.Load<Material>(PrefabNames.outlineMaterial);

        outlineMaterial = new Material(outlineMaterialTemplate);
    }

    public void setSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;

        Helpers.debugNullCheck("spriteRenderer.material", spriteRenderer.material);

        defaultMaterial = spriteRenderer.material;
    }

    public void createOutline(Color color, OutlineMode outlineMode)
    {
        outlineMaterial.color = color;

        spriteRenderer.material = outlineMaterial;
        setMaterialOutlineSize(spriteRenderer.material, outlineMode);
    }

    private static void setMaterialOutlineSize(Material material, OutlineMode outlineMode)
    {
        switch(outlineMode)
        {
            case OutlineMode.Bold:
                material.SetFloat(blackBorderSizeVarName, boldOutlineSize/4);
                material.SetFloat(colorOutlineSizeVarName, boldOutlineSize);
                break;
            default:
                material.SetFloat(blackBorderSizeVarName, normalOutlineSize/4);
                material.SetFloat(colorOutlineSizeVarName, normalOutlineSize);
                break;
        }
    }

    public void removeOutline()
    {
        spriteRenderer.material = defaultMaterial;
    }

}
