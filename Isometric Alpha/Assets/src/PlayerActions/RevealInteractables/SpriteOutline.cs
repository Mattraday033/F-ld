using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteOutline
{
    private const string blackBorderSizeXVarName = "_BlackBorderSizeX";
    private const string blackBorderSizeYVarName = "_BlackBorderSizeY";
    private const string colorOutlineSizeXVarName = "_ColorOutlineSizeX";
    private const string colorOutlineSizeYVarName = "_ColorOutlineSizeY";

    private const string blackBorderColorVarName = "_BlackBorderColor";

    private const float maxSize = 0.015f;

    private Material outlineMaterial;
    private SpriteRenderer spriteRenderer;
    private Transform spriteTransform;


    public SpriteOutline()
    {
        Material outlineMaterialTemplate = Resources.Load<Material>(PrefabNames.outlineMaterial);

        outlineMaterial = new Material(outlineMaterialTemplate);
        outlineMaterial.color = Color.clear;
        outlineMaterial.SetColor(blackBorderColorVarName, Color.clear);
    }

    public void setSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;

        spriteRenderer.material = outlineMaterial;
    }

    public void createOutline(Color color)
    {
        if(spriteRenderer.material.color.Equals(color))
        {
            return;
        }

        spriteTransform = spriteRenderer.transform;

        if(spriteTransform.position.z > 0f)
        {
            Vector3 oldPos = spriteTransform.position;
            spriteTransform.position = new Vector3(oldPos.x, oldPos.y, -1f);
        }

        outlineMaterial.color = color;

        spriteRenderer.material = outlineMaterial;


        float width = spriteRenderer.sprite.texture.width;
        float height = spriteRenderer.sprite.texture.height;

        float sizeMod = 4f; //amount of pixels

        setMaterialOutlineSize(spriteRenderer.material, sizeMod/width, sizeMod/height);
    }

    private static void setMaterialOutlineSize(Material material, float sizeX, float sizeY)
    {
        material.SetFloat(blackBorderSizeXVarName, sizeX/4f);
        material.SetFloat(blackBorderSizeYVarName, sizeY/4f);
        material.SetFloat(colorOutlineSizeXVarName, sizeX);
        material.SetFloat(colorOutlineSizeYVarName, sizeY);

        material.SetColor(blackBorderColorVarName, Color.black);
    }

    public void removeOutline()
    {
        if(spriteRenderer == null)
        {
            return;
        }

        outlineMaterial.color = Color.clear;
        outlineMaterial.SetColor(blackBorderColorVarName, Color.clear);
        
        spriteRenderer.material = outlineMaterial;
    }

}
