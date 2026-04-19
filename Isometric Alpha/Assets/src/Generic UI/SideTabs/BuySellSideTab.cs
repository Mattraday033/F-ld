using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuySellSideTab : AbilityGridSideTab
{
    public BuySellSideTab otherTab;
    // public GameObject depositGameObject;
    public ShopMode shopMode;

    private void Awake()
    {
        if (shopMode == ShopMode.Sell)
        {
            setToClosed();
        }
    }

    public override void setToClosed()
    {
        closedButton.interactable = true;
    }

    public override void setToOpen()
    {
        otherTab.setToClosed();

        closedButton.interactable = false;

        AudioManager.playChangeScreenSFX();

        ShopPopUpWindow.getInstance().setShopMode(shopMode);
    }

}

