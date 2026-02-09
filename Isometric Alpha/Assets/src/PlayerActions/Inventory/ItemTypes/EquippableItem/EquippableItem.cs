using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;

[System.Serializable]
public abstract class EquippableItem : Item, IJSONConvertable
{
    public const string offHandSlotText = "Off Hand";
    public const string headSlotText = "Head";
    public const string bodySlotText = "Body";
    public const string handsSlotText = "Hands";
    public const string feetSlotText = "Feet";
    public const string trinketSlotText = "Trinket";
    public const string mainHandSlotText = "Main Hand";

    public const string offHandSlotIconName = "OffHandSlot";
    public const string headSlotIconName = "HeadSlot";
    public const string bodySlotIconName = "BodySlot";
    public const string handsSlotIconName = "HandsSlot";
    public const string feetSlotIconName = "FeetSlot";
    public const string trinketSlotIconName = "TrinketSlot";
    public const string mainHandSlotIconName = "MainHandSlot";
    public const string twoHandedSlotIconName = "TwoHanded";
    public const string oneHandedSlotIconName = "OneHanded";

    public const string type = "Equip";

    public EquippableItem(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string subtype, int worth = 0) : 
    base(listId, key, loreDescription, damageFormula, critFormula, type, subtype, worth)
    {
        
    }

    public int getArmorRating()
    {
        return DamageCalculator.calculateFormula(getArmorFormula(), getStatSource());
    }

    public string getArmorRatingForDisplay()
    {
        return "" + getArmorRating();
    }

    public override bool isEquippable()
    {
        return true;
    }

    public override bool isUnequippable()
    {
        return true;
    }

    public override bool isEquipped(AllyStats target)
    {
        if (getSlotID() == Weapon.mainHandSlotIndex)
        {
            return base.isEquipped(target);
        }

        if (target.getEquippedItems().getItemInSlot(getSlotID()) != null &&
            String.Equals(getKey(), target.getEquippedItems().getItemInSlot(getSlotID()).getKey(), StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    public override void describeSelfFull(DescriptionPanel panel)
    {
        base.describeSelfFull(panel);

        if (panel.slotIconPanel != null && !(panel.slotIconPanel is null))
        {
            panel.slotIconPanel.sprite = Helpers.loadSpriteFromResources(getSlotIconName());
        }

        if (panel.slotIconBackgroundPanel != null && !(panel.slotIconBackgroundPanel is null))
        {
            panel.slotIconBackgroundPanel.color = getSlotIconBackgroundColor();
        }
    }

    public string getSlotIDForDisplay()
    {
        switch (getSlotID())
        {
            case Armor.offHandSlotIndex:
                return offHandSlotText;

            case Armor.headSlotIndex:
                return headSlotText;

            case Armor.bodySlotIndex:
                return bodySlotText;

            case Armor.handsSlotIndex:
                return handsSlotText;

            case Armor.feetSlotIndex:
                return feetSlotText;

            case Armor.trinketSlotIndex:
                return trinketSlotText;

            case Weapon.mainHandSlotIndex:
                return mainHandSlotText;

            default:
                throw new IOException("Unexpected slotID: " + getSlotID());
        }
    }

    public abstract string getIconName();
    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, getName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getWorthBlock(getWorthForDisplay()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getLoreDescription()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getSlotIconName()));

        return buildingBlocks;
    }

}
