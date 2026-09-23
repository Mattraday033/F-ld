using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spike : Obstacle
{
    private const bool up = false;
    private const bool down = true;

    public Collider2D movementBlockingCollider;

    private static Sprite downSprite;
    private static Sprite upSprite;

    protected override void Awake()
    {
        base.Awake();

        //spikes spawned from the creature prefab block movement with the tilemap collider sitting on their own root
        if(movementBlockingCollider == null)
        {
            movementBlockingCollider = GetComponent<TilemapCollider2D>();
        }

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
        setSprite(downSprite, SortingLayerManager.getSpikeSortingLayerInfo(down));
    }

    public override void setToUp()
    {
        movementBlockingCollider.enabled = true;
        setSprite(upSprite, SortingLayerManager.getSpikeSortingLayerInfo(up));
    }

    //the spikes ride on the body layer, the way every other single sprite obstacle does
    private void setSprite(Sprite sprite, SortingLayerInfo sortingLayerInfo)
    {
        SpriteRenderer bodyRenderer = rendererList[SpriteLayer.Body];

        bodyRenderer.sprite = sprite;

        sortingLayerInfo.setRendererSortingLayer(bodyRenderer);
    }

}
