using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotIconImage : Image
{
    public SlotIconHover parentIcon;

    public void setSprite(Sprite newSprite)
    {
        sprite = newSprite;

        if(sprite != null && parentIcon != null && spriteNameShouldBeInBubble(sprite.name))
        {
            parentIcon.revealBubble();
        } else if(parentIcon != null && parentIcon.bubble != null)
        {
            parentIcon.bubble.gameObject.SetActive(false);
        }
    }

    public static bool spriteNameShouldBeInBubble(string spriteName)
    {
        switch(spriteName)
        {
            case StatSourceNameList.chewKey:


            case ItemSpriteList.bandagesSprite:
            case ItemSpriteList.bronzeBarSprite:
            case ItemSpriteList.bronzeSpearSprite:
            case ItemSpriteList.capeSprite:
            case ItemSpriteList.cudgelSprite:
            case ItemSpriteList.curvedDaggerSprite:
            case ItemSpriteList.malletSprite:
            case ItemSpriteList.meatSprite:
            case ItemSpriteList.oneHandedPickSprite:
            case ItemSpriteList.plankSprite:
            case ItemSpriteList.properFoodSprite:
            case ItemSpriteList.rationsSprite:
            case ItemSpriteList.rockCakeSprite:
            case ItemSpriteList.sharpRockSprite:
            case ItemSpriteList.shivSprite:
            case ItemSpriteList.smallCoinPurseSprite:
            case ItemSpriteList.smokeBombSprite:
            case ItemSpriteList.staffSprite:
            case ItemSpriteList.teaSprite:
            case ItemSpriteList.thinbladeSprite:
            case ItemSpriteList.twoHandedPickSprite:
            case ItemSpriteList.wickedKnifeSprite:
            case ItemSpriteList.wornBowSprite:
                return true;
            default:
                return false;
        }
    }

}
