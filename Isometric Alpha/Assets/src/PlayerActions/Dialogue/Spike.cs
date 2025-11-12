using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Obstacle
{

    public SpriteRenderer spriteRenderer;
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
        downSprite = Resources.Load<Sprite>(PrefabNames.spikesDown);
        upSprite = Resources.Load<Sprite>(PrefabNames.spikesUp);
    }

    public override void setToDown()
    {
        movementBlockingCollider.enabled = false;
        spriteRenderer.sprite = downSprite;
    }
    
    public override void setToUp()
    {
        movementBlockingCollider.enabled = true;
        spriteRenderer.sprite = upSprite;
    }

}
