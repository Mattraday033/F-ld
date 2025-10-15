using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

//[System.Serializable]
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

	public Armor(ItemListID listID, string key, string loreDescription, int armorRating, int slotID) : base(listID, key, loreDescription, subtype, 0, slotID)
	{
		this.armorRating = armorRating;
		setWorth(calculateWorth(armorRating, slotID));
	}

    public override int getDamageFormulaTotal()
	{
		return DamageCalculator.calculateFormula(getDamageFormula(), getStatSource());
	}

    public override string getDamageTotalForDisplay()
	{
		return DamageCalculator.calculateFormula(getDamageFormula(), getStatSource()) + "";
	}

    public override int getCritFormulaTotal()
	{
		return DamageCalculator.calculateFormula(getCritFormula(), getStatSource());
	}

    public override int getArmorRating()
    {
        return DamageCalculator.calculateFormula(getBonusArmorFormula(), getStatSource());
    }

    public override string getBonusArmorFormula()
    {
        if (base.getBonusArmorFormula().Equals(IStatBoostSource.zeroRating)) 
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

        if (!getDamageFormula().Equals(IStatBoostSource.zeroRating))
        {
            buildingBlocks.Insert(1, DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getBonusDamageBlock(getDamageFormulaTotal().ToString()), getDamageFormula()));
        }

        return buildingBlocks;
    }
}

