using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum OutlineMode {Normal, Bold}

public class SpriteOutline
{
    private const string blackBorderSizeVarName = "_BlackBorderSize";
    private const string colorOutlineSizeVarName = "_ColorOutlineSize";

    private const float boldOutlineSize = 0.02f;
    private const float normalOutlineSize = 0.0025f;

    private Material outlineMaterial;
    private Material defaultMaterial;
    private SpriteRenderer spriteRenderer;
    private Transform spriteTransform;


    public SpriteOutline()
    {
        Material outlineMaterialTemplate = Resources.Load<Material>(PrefabNames.outlineMaterial);

        outlineMaterial = new Material(outlineMaterialTemplate);
    }

    public void setSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;

        defaultMaterial = spriteRenderer.material;
    }

    public void createOutline(Color color, OutlineMode outlineMode)
    {
        spriteTransform = spriteRenderer.transform;

        if(spriteTransform.position.z > 0f)
        {
            Vector3 oldPos = spriteTransform.position;
            spriteTransform.position = new Vector3(oldPos.x, oldPos.y, -1f);
        }

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
