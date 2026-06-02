using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastTravelIcon : MonoBehaviour
{

    public Image interiorImage;
    public Image arrowIconImage;

    public void disableFastTravelIcon()
    {
        gameObject.SetActive(false);
    }

    public void setToFastTravelAllowed()
    {
        interiorImage.color = ColorList.surpriseIconGreen;
        arrowIconImage.sprite = Helpers.loadSpriteFromResources(MapTileSpriteList.fastTravelIndicatorSprite);
    }

    public void setToFastTravelNotAllowed()
    {
        interiorImage.color = ColorList.surpriseIconRed;
        arrowIconImage.sprite = Helpers.loadSpriteFromResources(MapTileSpriteList.fastTravelBlockedSprite);
    }

}