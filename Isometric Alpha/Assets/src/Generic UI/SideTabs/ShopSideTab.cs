using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ShopSideTab : AbilityGridSideTab
{
    public override void setToDefaultState()
    {
        int currentListCount = Tab.getList(listToChoose).Count();

        setVisibility(currentListCount > 0);

        base.setToDefaultState();
    }

    private void setVisibility(bool visible)
    {
        if(listToChoose == DescribableList.Junk && 
            PlayerOOCStateManager.currentActivity == OOCActivity.inShopUI && 
            ShopPopUpWindow.currentShopMode != ShopMode.Sell)
        {
            gameObject.SetActive(false); 
        } else
        {
            gameObject.SetActive(visible);
        }
    }

    public override void setToOpen()
    {
        if(getCurrentDictKey() == null)
        {
            return;
        }

        ShopPopUpWindow.currentDescribableList = listToChoose;

        base.setToOpen();
    }

    // public override void setToOpen()
    // {
    //     OnSideTabChosen.Invoke();
    //     openTabPanel.SetActive(true);

    //     ShopPopUpWindow.populateGrid();
    // }

    // public override void setToClosed()
    // {
    //     closedButton.interactable = true;

    //     if (openTabPanel == null || openTabPanel is null)
    //     {
    //         return;
    //     }

    //     openTabPanel.SetActive(false);
    // }

}

