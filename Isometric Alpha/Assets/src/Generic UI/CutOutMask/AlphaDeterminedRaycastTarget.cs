using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlphaDeterminedRaycastTarget : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler
{
    public bool exactCopyOfSprite = true;
    public bool letAlphaDetermineRaycastTarget = true;

    public Image image;

    private void Awake()
    {
        Sprite targetSprite;

        if (!useExactCopyOfSprite())
        {
            targetSprite = image.sprite;
        }
        else
        {
            targetSprite = transform.parent.parent.GetComponent<SpriteRenderer>().sprite;
        }

        if (targetSprite != null)
        {
            image.sprite = convertTextureToSprite(duplicateTexture(targetSprite.texture), targetSprite.pivot);
        }

        if (alphaShouldDetermineRaycastTarget())
        {
           image.alphaHitTestMinimumThreshold = .1f; 
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //Empty on purpose
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //Empty on purpose
    }

    public virtual bool useExactCopyOfSprite()
    {
        return exactCopyOfSprite;
    }

    public virtual bool alphaShouldDetermineRaycastTarget()
    {
        return letAlphaDetermineRaycastTarget;
    }

    private Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    private Sprite convertTextureToSprite(Texture2D source, Vector2 pivot)
    {

        return Sprite.Create(source, new Rect(0, 0, source.width, source.height), pivot);
    }

}
