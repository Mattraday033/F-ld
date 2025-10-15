using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuySellSideTab : AbilityGridSideTab
{
    public BuySellSideTab otherTab;
    public ShopMode shopMode;

    private void Awake()
    {
        if (shopMode == ShopMode.Sell)
        {
            setToClosed();
        }
    }

    private void OnDestroy()
    {

    }

    public override void setToOpen()
    {
        openTabPanel.SetActive(true);

        otherTab.setToClosed();

        ShopPopUpWindow.getInstance().setShopMode(shopMode);
    }

}

