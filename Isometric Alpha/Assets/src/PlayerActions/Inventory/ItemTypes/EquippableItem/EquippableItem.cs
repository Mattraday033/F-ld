using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;

[System.Serializable]
public abstract class EquippableItem : Item, IJSONConvertable, IStatBoostSource, IFormulaSource
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

    private int slotID;
    protected string armorFormula;

    public EquippableItem(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string armorFormula, string subtype, int worth, int slotID) : 
    base(listId, key, loreDescription, damageFormula, critFormula, type, subtype, worth)
    {
        this.slotID = slotID;
        this.armorFormula = armorFormula;
    }

    [JsonConstructor]
    public EquippableItem(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string armorFormula, string subtype, int worth, int slotID, int quantity) : 
    base(listId, key, loreDescription, damageFormula, critFormula, type, subtype, worth, quantity)
    {
        this.slotID = slotID;
        this.armorFormula = armorFormula;
    }

    public int getArmorRating()
    {
        return DamageCalculator.calculateFormula(armorFormula, getStatSource());
    }

    public virtual string getArmorFormula()
    {
        return armorFormula;
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

    public abstract string getIconName();

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
        return Constants.zeroRating;
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

    #endregion

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

/*
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class Armor : EquippableItem, IJSONConvertable
{
	public const string typeIconName = "Armor";
	public const string subtype = "Armor";

	public const int trinketBaseWorthWithoutArmor = 75;

	public const int offHandSlotIndex = 0;  //Both the Armor and Weapon classes have an offHandSlotIndex because both types of item can go in
	public const int headSlotIndex = 1;     //this slot. They're the same index, it doesn't matter which one is called but to prevent confusion the 
	public const int bodySlotIndex = 2;     //Armor index should be called for Armors and the Weapon index for Weapons.
	public const int handsSlotIndex = 3;
	public const int feetSlotIndex = 4;
	public const int trinketSlotIndex = 5;

	private const double maximumArmorDamageReduction = 99.0;

	private int armorRating;

	public Armor(ItemListID listID, string key, string loreDescription, int armorRating, int slotID) : 
    base(listID, key, loreDescription, Constants.zeroRating, Constants.zeroRating, subtype, 0, slotID)
	{
		this.armorRating = armorRating;
		setWorth(calculateWorth(armorRating, slotID));
	}

	public Armor(ItemListID listID, string key, string loreDescription, string damageFormula, int armorRating, int slotID) : 
    base(listID, key, loreDescription, damageFormula, Constants.zeroRating, subtype, 0, slotID)
	{
		this.armorRating = armorRating;
		setWorth(calculateWorth(armorRating, slotID));
	}

	public Armor(ItemListID listID, string key, string loreDescription, string damageFormula, string critFormula, int armorRating, int slotID) : 
    base(listID, key, loreDescription, damageFormula, critFormula, subtype, 0, slotID)
	{
		this.armorRating = armorRating;
		setWorth(calculateWorth(armorRating, slotID));
	}

    public override string getBonusArmorFormula()
    {
        if (base.getBonusArmorFormula().Equals(Constants.zeroRating)) 
        {
            return armorRating.ToString();
        }
        else
        {
            return base.getBonusArmorFormula() + "+" + armorRating;
        }
    }

    public string getArmorRatingForDisplay()
    {
        return "" + getArmorRating();
    }

	private static int calculateWorth(int armorRating, int slotID)
	{
		if (slotID == trinketSlotIndex && armorRating * 2 <= trinketBaseWorthWithoutArmor)
		{
			return trinketBaseWorthWithoutArmor;
		}

		return (armorRating * 2);
	}

	public static double getDamageReduction(int armorRating)
	{
		double damageReduction = (double)(armorRating / 2);

		if (damageReduction > maximumArmorDamageReduction)
		{
			damageReduction = maximumArmorDamageReduction;
		}

		return (damageReduction / 100.0);
	}

	public override GameObject getDescriptionPanelFull(PanelType panelType)
	{
		string panelTypeName = "";

		switch (panelType)
		{
			case PanelType.Standard:
				panelTypeName = PrefabNames.armorDescPanelFull;
				break;
			default:
				throw new IOException("Unknown PanelType: " + panelType);
		}

		return DescriptionPanel.getDescriptionPanel(panelTypeName);
	}

	public override GameObject getRowType(RowType rowType)
	{
		string rowTypeName = "";

		switch (rowType)
		{
			case RowType.AbilityEditor:
				rowTypeName = PrefabNames.actionEditorRow;
				break;
			default:
				return base.getRowType(rowType);
		}

		return Resources.Load<GameObject>(rowTypeName);
	}

	public override void describeSelfFull(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);

		if (panel.armorRatingText != null && !(panel.armorRatingText is null))
		{
			panel.armorRatingText.text = "" + getArmorRating();
		}

		if (panel.slotText != null && !(panel.slotText is null))
		{
			panel.slotText.text = "" + getSlotIDForDisplay();
		}

		if (panel.typeIconPanel != null && !(panel.typeIconPanel is null))
		{
			panel.typeIconPanel.sprite = Helpers.loadSpriteFromResources(getTypeIconName());
		}

		if (panel.typeIconBackgroundPanel != null && !(panel.typeIconBackgroundPanel is null))
		{
			panel.typeIconBackgroundPanel.color = getTypeIconBackgroundColor();
		}
	}

	public override string getTypeIconName()
	{
		return getSlotIconName();
	}

	public override string getSlotIconName()
	{
        return Armor.getSlotIconName(getSlotID());
	}
    
    public static string getSlotIconName(int slotIndex)
	{
		switch (slotIndex)
		{
			case Armor.offHandSlotIndex:
				return EquippableItem.offHandSlotIconName;

			case Armor.headSlotIndex:
				return EquippableItem.headSlotIconName;

			case Armor.bodySlotIndex:
				return EquippableItem.bodySlotIconName;

			case Armor.handsSlotIndex:
				return EquippableItem.handsSlotIconName;

			case Armor.feetSlotIndex:
				return EquippableItem.feetSlotIconName;

			case Armor.trinketSlotIndex:
				return EquippableItem.trinketSlotIconName;

			default:
				throw new IOException("Unexpected slotID: " + slotIndex);
		}
	}

	//IBuildableWithBlocks methods
    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = base.getDescriptionBuildingBlocks();

        if (!getDamageFormula().Equals(Constants.zeroRating))
        {
            buildingBlocks.Insert(1, DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getBonusDamageBlock(getDamageFormulaTotal().ToString()), getDamageFormula()));
        }

        return buildingBlocks;
    }
}


*/