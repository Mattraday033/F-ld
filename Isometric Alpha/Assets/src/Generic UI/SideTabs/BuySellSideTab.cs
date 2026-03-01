using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuySellSideTab : AbilityGridSideTab
{
    public BuySellSideTab otherTab;
    public GameObject depositGameObject;
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
        depositGameObject.SetActive(false);
        closedButton.interactable = true;
    }

    public override void setToOpen()
    {
        otherTab.setToClosed();

        depositGameObject.SetActive(true);
        closedButton.interactable = false;

        ShopPopUpWindow.getInstance().setShopMode(shopMode);
    }

}

