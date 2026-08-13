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

    public override SFXType getChestOpenSFX(ChestType type)
    {
        return SFXType.OnTransition;
    }

}
