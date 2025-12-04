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
    protected readonly static Stats worthStatsSource = new AllyStats("Worth Stats Calc", Constants.sizeFour, Constants.sizeFour, Constants.sizeFour, Constants.sizeFour);

	public Armor(ItemListID listID, string key, string loreDescription, int armorRating, int slotID) : 
    base(listID, key, loreDescription, Constants.zeroRating, Constants.zeroRating, armorRating.ToString(), subtype, 0, slotID)
	{
		setWorth(calculateWorth(armorRating, slotID));
	}

	public Armor(ItemListID listID, string key, string loreDescription, string armorFormula, int slotID) : 
    base(listID, key, loreDescription, Constants.zeroRating, Constants.zeroRating, armorFormula, subtype, 0, slotID)
	{
		setWorth(calculateWorth(DamageCalculator.calculateFormula(armorFormula, worthStatsSource), slotID));
	}

	public Armor(ItemListID listID, string key, string loreDescription, string damageFormula, int armorRating, int slotID) : 
    base(listID, key, loreDescription, damageFormula, Constants.zeroRating, armorRating.ToString(), subtype, 0, slotID)
	{
		setWorth(calculateWorth(armorRating, slotID));
	}

	public Armor(ItemListID listID, string key, string loreDescription, string damageFormula, string critFormula, int armorRating, int slotID) : 
    base(listID, key, loreDescription, damageFormula, critFormula, armorRating.ToString(), subtype, 0, slotID)
	{
		setWorth(calculateWorth(armorRating, slotID));
	}

	public Armor(ItemListID listID, string key, string loreDescription, string damageFormula, string critFormula, string armorFormula, int slotID) : 
    base(listID, key, loreDescription, damageFormula, critFormula, armorFormula, subtype, 0, slotID)
	{
		setWorth(calculateWorth(DamageCalculator.calculateFormula(armorFormula, worthStatsSource), slotID));
	}

    public override string getIconName()
    {
        return getSlotIconName();
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

        DescriptionPanel.setText(panel.armorRatingText, getArmorRating());
        DescriptionPanel.setText(panel.slotText, getSlotIDForDisplay());
        DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(getIconName()));
        DescriptionPanel.setImage(panel.typeIconPanel, Helpers.loadSpriteFromResources(getTypeIconName()));
        DescriptionPanel.setImageColor(panel.typeIconBackgroundPanel, getTypeIconBackgroundColor());

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

        if (getArmorRating() > Constants.sizeZero)
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getArmorBlock(getArmorRatingForDisplay(), armorFormula));
        }

        if (!getCritFormula().Equals(Constants.zeroRating))
        {
            buildingBlocks.Insert(1, DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getCritBlock(getCritFormulaTotal().ToString(), getCritFormula()), getCritFormula()));
        }

        if (!getDamageFormula().Equals(Constants.zeroRating))
        {
            buildingBlocks.Insert(1, DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getDamageBlock(getDamageFormulaTotal().ToString(), getDamageFormula()), getDamageFormula()));
        }

        return buildingBlocks;
    }
}

