using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SpriteOutline
{
    private const string blackBorderSizeXVarName = "_BlackBorderSizeX";
    private const string blackBorderSizeYVarName = "_BlackBorderSizeY";
    private const string colorOutlineSizeXVarName = "_ColorOutlineSizeX";
    private const string colorOutlineSizeYVarName = "_ColorOutlineSizeY";

    private const string blackBorderColorVarName = "_BlackBorderColor";

    private Material outlineMaterial;
    private SpriteRenderer spriteRenderer;
    private Transform spriteTransform;

    private float _NormalZPos;
    public float normalZPos
    {
        get
        {
            return _NormalZPos;
        }    
        set
        {
            _NormalZPos = value;
            setSpriteTransformZPos();
        }
    }

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
        if(!spriteRenderer.gameObject.activeInHierarchy || 
            spriteRenderer.color.Equals(Color.clear))
        {
            removeOutline();
            return;
        }


        outlineMaterial.color = color;

        spriteRenderer.material = outlineMaterial;

        setSpriteTransformZPos();

        try
        {
            
            float width = spriteRenderer.sprite.texture.width;
            float height = spriteRenderer.sprite.texture.height;

            float sizeMod = 4f; //amount of pixels

            setMaterialOutlineSize(spriteRenderer.material, sizeMod/width, sizeMod/height);
        } catch(Exception e)
        {
            Debug.LogError("Caught exception null sprite");
        }
    }

    private void setSpriteTransformZPos()
    {
        spriteTransform = spriteRenderer.transform;

        if(spriteTransform == null)
        {
            return;
        }

        if((CombatStateManager.inCombat && spriteTransform.position.z > normalZPos) || 
            (!CombatStateManager.inCombat && spriteTransform.position.z != normalZPos))
        {
            Vector3 oldPos = spriteTransform.position;
            spriteTransform.position = new Vector3(oldPos.x, oldPos.y, normalZPos);
        } 
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


public class ImageOutline
{
    private const string blackBorderSizeXVarName = "_BlackBorderSizeX";
    private const string blackBorderSizeYVarName = "_BlackBorderSizeY";
    private const string colorOutlineSizeXVarName = "_ColorOutlineSizeX";
    private const string colorOutlineSizeYVarName = "_ColorOutlineSizeY";

    private const string blackBorderColorVarName = "_BlackBorderColor";

    private Material outlineMaterial;
    private Image image;
    private Transform imageTransform;


    public ImageOutline()
    {
        Material outlineMaterialTemplate = Resources.Load<Material>(PrefabNames.outlineMaterial);

        outlineMaterial = new Material(outlineMaterialTemplate);
        outlineMaterial.color = Color.clear;
        outlineMaterial.SetColor(blackBorderColorVarName, Color.clear);
    }

    public void setImage(Image image)
    {
        this.image = image;

        image.material = outlineMaterial;
    }

    public void createOutline(Color color)
    {
        if(!image.gameObject.activeInHierarchy)
        {
            return;
        }

        imageTransform = image.transform;

        if(imageTransform.position.z > 0f)
        {
            Vector3 oldPos = imageTransform.position;
            imageTransform.position = new Vector3(oldPos.x, oldPos.y, -1f);
        }

        outlineMaterial.color = color;

        image.material = outlineMaterial;


        try
        {
            
            float width = image.sprite.texture.width;
            float height = image.sprite.texture.height;

            float sizeMod = 4f; //amount of pixels

            setMaterialOutlineSize(image.material, sizeMod/width, sizeMod/height);
        } catch(Exception e)
        {
            Debug.LogError("Caught exception null sprite");
        }
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
        if(image == null)
        {
            return;
        }

        outlineMaterial.color = Color.clear;
        outlineMaterial.SetColor(blackBorderColorVarName, Color.clear);
        
        image.material = outlineMaterial;
    }

}
