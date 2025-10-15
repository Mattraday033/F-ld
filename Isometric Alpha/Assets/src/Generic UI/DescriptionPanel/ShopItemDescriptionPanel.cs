using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDescriptionPanel : DescriptionPanelWithFormula
{

    public AmountPanel amountPanel;
    public ShopItemGridRow gridRow;

    public override void setObjectBeingDescribed(IDescribable describable)
    {
        base.setObjectBeingDescribed(describable);

        if (ShopPopUpWindow.getCurrentMode() == ShopMode.Buy)
        {
            amountPanel.setDirectionMode(DirectionMode.BuySendTo);
        }
        else
        {
            amountPanel.setDirectionMode(DirectionMode.SellReceiveFrom);
        }

        if (amountPanel.getMax() <= 0)
        {
            gridRow.setToUnbuyableDisplay();  
        }
    }


}
