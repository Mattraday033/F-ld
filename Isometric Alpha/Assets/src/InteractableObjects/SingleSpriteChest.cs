using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SingleSpriteChest : Chest
{
    public Sprite sprite;
    public string chestName;

    public override string getName()
    {
        return chestName;
    }

    protected override void setToCurrentSprite()
    {
        spriteRenderer.sprite = sprite;

        setMouseHoverPosition();
    }

    public override string getChestOpenSFX(ChestType type)
    {
        return AudioClipList.onTransitionSFX;
    }

}
