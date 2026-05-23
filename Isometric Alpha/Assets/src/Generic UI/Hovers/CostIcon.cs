using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//class SlotHoverIcon
public class CostIcon : SlotIconHover
{

    private const string costPrefix = "Cost: ";
    public const string costSuffix = " Cost";

    public ActionCostType costType;
    public TextMeshProUGUI costText;

    public override void Awake()
    {
        base.Awake();
    }

    public void setCostType(ActionCostType costType)
    {
        this.costType = costType;

        switch(costType)
        {
            default:
                string costName = costType.ToFriendlyString();

                iconImage.sprite = Resources.Load<Sprite>(costName);
                hoverMessageKey = costName;
                setHoverMessage(HoverMessageList.getMessage(costName + costSuffix));
                return;
        }
    }

    public void setCostText(string cost)
    {
        costText.text = cost;
    }

    protected override string getHoverMessageKeyForDisplay()
    {
        return costPrefix + base.getHoverMessageKeyForDisplay();
    }

    protected override float getInWorldSpaceScale()
    {
        return .025f;
    }

}