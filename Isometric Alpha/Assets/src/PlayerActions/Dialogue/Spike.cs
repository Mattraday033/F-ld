using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Obstacle
{
    private const bool up = false;
    private const bool down = true;

    public Collider2D movementBlockingCollider;

    private static Sprite downSprite;
    private static Sprite upSprite;

    private void Awake()
    {
        setToDown();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeSpike()
    {
        downSprite = SpriteUtil.loadSpriteFromResources(PrefabNames.spikesDown);
        upSprite = SpriteUtil.loadSpriteFromResources(PrefabNames.spikesUp);
    }

    public override void setToDown()
    {
        movementBlockingCollider.enabled = false;
        spriteRenderer.sprite = downSprite;
        SortingLayerManager.getSpikeSortingLayerInfo(down).setRendererSortingLayer(spriteRenderer);
    }
    
    public override void setToUp()
    {
        movementBlockingCollider.enabled = true;
        spriteRenderer.sprite = upSprite;
        SortingLayerManager.getSpikeSortingLayerInfo(up).setRendererSortingLayer(spriteRenderer);
    }

}
