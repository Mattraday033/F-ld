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

    //the key of the cunning object that raises and lowers this spike
    private string cunningKey = "";

    //the second set of spikes on a double cunning object comes up as the first set goes down
    private bool raisedWhenCranked;

    public static bool isRaised(bool cranked, bool raisedWhenCranked)
    {
        return cranked == raisedWhenCranked;
    }

    //Awake has already lowered the spike, so this puts it where the cunning object's saved state says it belongs
    public void linkToCrank(string cunningKey, bool raisedWhenCranked)
    {
        this.cunningKey = cunningKey;
        this.raisedWhenCranked = raisedWhenCranked;

        setCranked(TrapAndButtonStateManager.contains(cunningKey));
    }

    public void setCranked(bool cranked)
    {
        if(isRaised(cranked, raisedWhenCranked))
        {
            setToUp();
        } else
        {
            setToDown();
        }
    }

    public void setStatus(string key, bool status)
    {
        if(!cunningKey.Equals(key))
        {
            return;
        }

        bool wasRaised = movementBlockingCollider.enabled;

        setCranked(status);

        //only a spike that has just come up kills, so nothing dies when the area spawns with its spikes already raised
        if(!wasRaised && movementBlockingCollider.enabled)
        {
            killEnemiesOnSpike();
        }
    }

    //there is no death animation to play yet, so an enemy caught by the spike just disappears
    private void killEnemiesOnSpike()
    {
        Vector3Int cell = AreaManager.getMasterGrid().WorldToCell(transform.position);

        foreach(MovementTracker movementTracker in MovementManager.allMovementTrackers)
        {
            EnemyMovement enemyMovement = movementTracker as EnemyMovement;

            if(enemyMovement == null || enemyMovement.movableObject || enemyMovement.isDefeated())
            {
                continue;
            }

            Vector3Int enemyCell = enemyMovement.getCell();

            if(enemyCell.x == cell.x && enemyCell.y == cell.y)
            {
                enemyMovement.setToDefeated();
                enemyMovement.setToDefeatedMode();
            }
        }
    }

    public override void createListeners()
    {
        base.createListeners();

        TrapAndButtonStateManager.OnSetTraps.AddListener(setStatus);
    }

    public override void destroyListeners()
    {
        base.destroyListeners();

        TrapAndButtonStateManager.OnSetTraps.RemoveListener(setStatus);
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
