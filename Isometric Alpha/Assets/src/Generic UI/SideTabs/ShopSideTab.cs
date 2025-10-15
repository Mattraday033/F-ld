using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopSideTab : AbilityGridSideTab
{
    private void Awake()
    {
        setToClosed();

        OnSideTabChosen.AddListener(setToClosed);
    }

    private void OnDestroy()
    {
        OnSideTabChosen.RemoveListener(setToClosed);
    }

    public override void setToOpen()
    {
        OnSideTabChosen.Invoke();
        openTabPanel.SetActive(true);

        TabCollection typeTabs = ShopPopUpWindow.getInstance().itemTypeTabs;

        typeTabs.selectTab(tabIndex);

        ShopPopUpWindow.populateGrid();
    }

    public override void setToClosed()
    {
        closedButton.interactable = true;

        if (openTabPanel == null || openTabPanel is null)
        {
            return;
        }

        openTabPanel.SetActive(false);
    }

}

