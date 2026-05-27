using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CostIcon : SlotIconHover
{
    private const string costPrefix = "Cost: ";
    public const string costSuffix = " Cost";

    public static bool forceVerticalLayoutDescriptionPanels;

    public ActionCostType costType;
    public TextMeshProUGUI costText;

    public List<IDescribable> relatedDescribables = new List<IDescribable>();

    public override void Awake()
    {
        base.Awake();
    }

    public void setCostType(ActionCostType costType)
    {
        this.costType = costType;
        string costName = costType.ToFriendlyString();

        if(costTypeUsesDescriptionPanelSlot())
        {
            if(!TraitList.dictionaryOfTraits.ContainsKey(costName))
            {
                Debug.LogError("No Trait associated with ActionCostType: " + costName);
                return;
            }

            Trait costTrait = TraitList.dictionaryOfTraits[costName];
            iconImage.sprite = costTrait.getIconSprite();
            hoverMessageKey = HoverMessageList.traitCostKey;
            setHoverMessage(HoverMessageList.getMessage(hoverMessageKey));

            relatedDescribables.Add(costTrait);

        } else
        {
            iconImage.sprite = Resources.Load<Sprite>(costName);
            hoverMessageKey = costName;
            setHoverMessage(HoverMessageList.getMessage(costName + costSuffix));
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(costTypeUsesDescriptionPanelSlot())
        {
            forceVerticalLayoutDescriptionPanels = true;
        }

        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        forceVerticalLayoutDescriptionPanels = false;
        base.OnPointerExit(eventData);
    }

    private bool costTypeUsesDescriptionPanelSlot()
    {
        switch(costType)
        {
            case ActionCostType.Stance:
            case ActionCostType.RedKnife:
            case ActionCostType.BlueShield:
            case ActionCostType.YellowThorn:
            case ActionCostType.GreenLeaf:
                return false;
            default:
                return true;
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

    public override List<IDescribable> getRelatedDescribables()
    {
        return relatedDescribables;
    }

}