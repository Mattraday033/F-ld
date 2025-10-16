using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;

[System.Serializable]
public class EquippableItem : Item, IJSONConvertable, IStatBoostSource 
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

    public Stats equipTarget;

    //[SerializeField] 
    private int slotID;

    public EquippableItem(ItemListID listId, string key, string loreDescription, string subtype, int worth, int slotID) : base(listId, key, loreDescription, type, subtype, worth)
    {
        this.slotID = slotID;
    }

    [JsonConstructor]
    public EquippableItem(ItemListID listId, string key, string loreDescription, string subtype, int worth, int slotID, int quantity) : base(listId, key, loreDescription, type, subtype, worth, quantity)
    {
        this.slotID = slotID;
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

    public override GameObject getDecisionPanel()
    {
        return Resources.Load<GameObject>(PrefabNames.equippableDecisionButtons);
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

    public override int getSlotID()
    {
        return slotID;
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
                throw new IOException("Unexpected slotID: " + slotID);
        }
    }

    public virtual string getIconName()
    {
        throw new IOException("getIconName() was called in the base class extraneously");
    }

    public virtual int getArmorRating()
    {
        throw new IOException("getArmorRating() was called in the base class extraneously");
    }

    public override string getDamageFormula()
    {
        return "0";
    }

    public override string getCritFormula()
    {
        return getBonusCritFormula();
    }

	public override string getCritFormulaTotalForDisplay()
	{
		return DamageCalculator.calculateFormula(getCritFormula(), getStatSource()) + "%";
	}

    public override Item clone()
	{
        EquippableItem clone = base.clone() as EquippableItem;

        clone.equipTarget = equipTarget;

        return clone as Item;
	}

    #region IStatBoostSource Methods
    #region Generic Stats

    public string getBonusCritFormula()
    {
        return StatBoostManager.getBonusCritFormula(this);
    }

    public string getBonusDamageFormula()
    {
        return StatBoostManager.getBonusDamageFormula(this);
    }

    #endregion

    #region PrimaryStats

    public string getBonusStrengthFormula()
    {
        return StatBoostManager.getBonusStrengthFormula(this);
    }
    
    public string getBonusDexterityFormula()
    {
        return StatBoostManager.getBonusDexterityFormula(this);
    }
 
    public string getBonusWisdomFormula()
    {
        return StatBoostManager.getBonusWisdomFormula(this);
    }
 
    public string getBonusCharismaFormula()
    {
        return StatBoostManager.getBonusCharismaFormula(this);
    }
 

    #endregion

    #region Secondary Stats

    //Strength Stats
    public string getBonusPhysicalResistanceFormula()
    {
        return StatBoostManager.getBonusPhysicalResistanceFormula(this);
    }
 
    public string getBonusCriticalDamageMultiplierFormula()
    {
        return StatBoostManager.getBonusCriticalDamageMultiplierFormula(this);
    }
 
    public string getBonusHealthFormula()
    {
        return StatBoostManager.getBonusHealthFormula(this);
    }
 

    //Dexterity Stats
    public string getBonusSurpriseRoundDamageFormula()
    {
        return StatBoostManager.getBonusSurpriseRoundDamageFormula(this);
    }
 
    public virtual string getBonusArmorFormula()
    {
        return StatBoostManager.getBonusArmorFormula(this);
    }
 
    public string getBonusArmorPenetrationFormula()
    {
        return StatBoostManager.getBonusArmorPenetrationFormula(this);
    }
 

    //Wisdom Stats
    public string getBonusPassiveSlotsFormula()
    {
        return StatBoostManager.getBonusPassiveSlotsFormula(this);
    }
 
    public string getBonusWeaponSlotsFormula()
    {
        return StatBoostManager.getBonusWeaponSlotsFormula(this);
    }
 
    public string getBonusMentalResistanceFormula()
    {
        return StatBoostManager.getBonusMentalResistanceFormula(this);
    }
 

    //Charisma Stats
    public string getBonusSynergyFormula()
    {
        return StatBoostManager.getBonusSynergyFormula(this);
    }
 
    public string getBonusExuberancesFormula()
    {
        return StatBoostManager.getBonusExuberancesFormula(this);
    }
 
    public string getBonusZOIPotencyFormula()
    {
        return StatBoostManager.getBonusZOIPotencyFormula(this);
    }
 

    #endregion

    #region Party Stats

    public string getBonusRegenFormula()
    {
        return StatBoostManager.getBonusRegenFormula(this);
    }
 

    public string getBonusSurpriseRoundsFormula()
    {
        return StatBoostManager.getBonusSurpriseRoundsFormula(this);
    }
 
    public string getBonusRetreatChanceFormula()
    {
        return StatBoostManager.getBonusRetreatChanceFormula(this);
    }
 

    public string getBonusPartyActionsFormula()
    {
        return StatBoostManager.getBonusPartyActionsFormula(this);
    }
 
    public string getBonusPartySlotsFormula()
    {
        return StatBoostManager.getBonusPartySlotsFormula(this);
    }
 

    public string getBonusGoldMultiplierFormula()
    {
        return StatBoostManager.getBonusGoldMultiplierFormula(this);
    }
 
    public string getBonusDiscountFormula()
    {
        return StatBoostManager.getBonusDiscountFormula(this);
    }
 

    public string getBonusVolleyAccuracyFormula()
    {
        return StatBoostManager.getBonusVolleyAccuracyFormula(this);
    }
 

    #endregion

    #region Skills
    public string getBonusIntimidateChargesFormula()
    {
        return StatBoostManager.getBonusIntimidateChargesFormula(this);
    }
 
    public string getBonusCunningChargesFormula()
    {
        return StatBoostManager.getBonusCunningChargesFormula(this);
    }
 
    public string getBonusObservationLevelFormula()
    {
        return StatBoostManager.getBonusObservationLevelFormula(this);
    }
 
    public string getBonusLeadershipUsesFormula()
    {
        return StatBoostManager.getBonusLeadershipUsesFormula(this);
    }

    #endregion

    public Stats getStatSource()
    {
        Helpers.debugNullCheck("equipTarget", equipTarget);

        if (equipTarget == null && !CombatStateManager.inCombat && OverallUIManager.getCurrentPartyMember() != null)
        {
            return OverallUIManager.getCurrentPartyMember();
        }

        return equipTarget;
    }

    #endregion

    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, getName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getAmountBlock(getQuantityForDisplay()));
        buildingBlocks.Add(DescriptionPanelBuildingBlock.getWorthBlock(getWorthForDisplay()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getLoreDescription()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getSlotIconName()));

        return buildingBlocks;
    }

}
