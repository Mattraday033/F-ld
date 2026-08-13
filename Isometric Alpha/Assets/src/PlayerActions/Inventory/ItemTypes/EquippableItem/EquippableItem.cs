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

    public const string offHandSlotIconName = "Off Hand Slot";
    public const string headSlotIconName = "Head Slot";
    public const string bodySlotIconName = "Body Slot";
    public const string handsSlotIconName = "Hands Slot";
    public const string feetSlotIconName = "Feet Slot";
    public const string trinketSlotIconName = "Trinket Slot";
    public const string mainHandSlotIconName = "Main Hand Slot";
    public const string twoHandedSlotIconName = "Two Handed";
    public const string oneHandedSlotIconName = "One Handed";

    public const string type = "Equip";

    public EquippableItem(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string subtype, int worth = 0) : 
    base(listId, key, loreDescription, damageFormula, critFormula, type, subtype, worth)
    {
        
    }

    public int getArmorRating()
    {
        return DamageCalculator.calculateFormula(getArmorFormula(), getStatSource());
    }


    public string getInvulnerabilityForDisplay()
    {
        return DamageCalculator.calculateFormula(getInvulnerableFormula(), getStatSource()) + "";
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

        // if (panel.slotIconPanel != null && !(panel.slotIconPanel is null))
        // {
        //     panel.slotIconPanel.sprite = Helpers.loadSpriteFromResources(getSlotIconName());
        // }

        // if (panel.slotIconBackgroundPanel != null && !(panel.slotIconBackgroundPanel is null))
        // {
        //     panel.slotIconBackgroundPanel.color = getSlotIconBackgroundColor();
        // }
    }

    public void playEquipSFX()
    {
        switch (getSlotID())
        {
            case Armor.headSlotIndex:
                AudioManager.playAudioClipAsSingleton(SFXType.Head);
                break;

            case Armor.bodySlotIndex:
                AudioManager.playAudioClipAsSingleton(SFXType.Body);
                break;

            case Armor.handsSlotIndex:
                AudioManager.playAudioClipAsSingleton(SFXType.Hands);
                break;

            case Armor.feetSlotIndex:
                AudioManager.playAudioClipAsSingleton(SFXType.Feet);
                break;

            case Armor.trinketSlotIndex:
                AudioManager.playAudioClipAsSingleton(SFXType.Trinket);
                break;

            default:
                AudioManager.playWeaponChangeSFX();
                break;
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

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getWorthBlock(getWorthForDisplay()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getLoreDescription()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: getSlotIconName()));

        return buildingBlocks;
    }

}
