using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum OutlineMode {Normal, Bold}

public class SpriteOutline
{
    private const string blackBorderSizeVarName = "_BlackBorderSize";
    private const string colorOutlineSizeVarName = "_ColorOutlineSize";

    private const float maxSize = 0.015f;

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
        Debug.LogError("outlineMode = " + outlineMode.ToString());

        spriteTransform = spriteRenderer.transform;

        if(spriteTransform.position.z > 0f)
        {
            Vector3 oldPos = spriteTransform.position;
            spriteTransform.position = new Vector3(oldPos.x, oldPos.y, -1f);
        }

        outlineMaterial.color = color;

        spriteRenderer.material = outlineMaterial;


        Debug.LogError("("+spriteRenderer.sprite.name+").pixelsPerUnit = " + spriteRenderer.sprite.pixelsPerUnit);

        Debug.LogError("spriteRenderer.sprite.texture.height = " + spriteRenderer.sprite.texture.height);
        Debug.LogError("spriteRenderer.sprite.texture.width = " + spriteRenderer.sprite.texture.width);


        // float size = spriteRenderer.sprite.pixelsPerUnit/10000f;

        float width = spriteRenderer.sprite.texture.width;
        float height = spriteRenderer.sprite.texture.height;

        // Debug.LogError("width/height = " + width/height);

        // if(width > height*2)
        // {
        //     size *= (height/width)*3;
        // }

        // if(size > maxSize)
        // {
        //     size = maxSize;
        // }

        setMaterialOutlineSize(spriteRenderer.material, outlineMode, (1/width) * (float) (Math.Log(width)) * .9f);
    }

    private static void setMaterialOutlineSize(Material material, OutlineMode outlineMode, float size)
    {
        Debug.LogError("size = " + size);

        material.SetFloat(blackBorderSizeVarName, size/4);
        material.SetFloat(colorOutlineSizeVarName, size);

        // switch(outlineMode)
        // {
        //     case OutlineMode.Bold:
        //         material.SetFloat(blackBorderSizeVarName, boldOutlineSize/4);
        //         material.SetFloat(colorOutlineSizeVarName, boldOutlineSize);
        //         break;
        //     default:
        //         material.SetFloat(blackBorderSizeVarName, normalOutlineSize/4);
        //         material.SetFloat(colorOutlineSizeVarName, normalOutlineSize);
        //         break;
        // }
    }

    public void removeOutline()
    {
        spriteRenderer.material = defaultMaterial;
    }

}
