using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDescriptionPanel : DescriptionPanelWithFormula
{

    public AmountPanel amountPanel;
    public ShopItemGridRow gridRow;

    public GameObject buyButtonParent;
    public Button buyButton;

    public GameObject sellButtonParent;
    public Button sellButton;

    public override void setObjectBeingDescribed(IDescribable describable)
    {
        base.setObjectBeingDescribed(describable);

        if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
        {
            amountPanel.setDirectionMode(DirectionMode.BuySendTo);
            if (amountPanel.getMax() <= 0)
            {
                gridRow.setToUnbuyableDisplay();  
                buyButton.interactable = false;
            } else
            {
                buyButton.interactable = true;
            }
        }
        else
        {
            amountPanel.setDirectionMode(DirectionMode.SellReceiveFrom);
        }

        buyButtonParent.SetActive(ShopPopUpWindow.currentShopMode == ShopMode.Buy);
        sellButtonParent.SetActive(ShopPopUpWindow.currentShopMode == ShopMode.Sell);
    }

    public void buyButtonPress()
    {
        ShopPopUpWindow.buyItem(getItemForTransaction());
    }

    public void sellButtonPress()
    {
        ShopPopUpWindow.sellItem(getItemForTransaction());
    }

    private Item getItemForTransaction()
    {
        ItemListID listID = gridRow.descriptionPanel.getItemBeingDescribed().getItemListID();

        return ItemList.getItem(listID.listIndex, listID.itemIndex, amountPanel.getAmount());
    }
}