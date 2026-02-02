using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public abstract class Armor : EquippableItem, IJSONConvertable
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

    private int armorTier;
    private int slotID;

	public Armor(ItemListID listID, string key, string loreDescription, string armorFormula, int slotID, int armorTier, 
                                                                                                         string damageFormula = Constants.zeroRating, 
                                                                                                         string critFormula = Constants.zeroRating) : 
    base(listID, key, loreDescription, damageFormula, critFormula, armorFormula, subtype, calculateWorth(DamageCalculator.calculateFormula(armorFormula, worthStatsSource), slotID))
    {
        this.slotID = slotID;
        this.armorTier = armorTier;
    }

    public override string getIconName()
    {
        return getSlotIconName();
    }

	protected static int calculateWorth(int armorRating, int slotID)
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

    public int getTier()
    {
        return armorTier;
    }
    
    public override string getBonusPhysicalResistanceFormula()
    {
        string resistanceFormula = base.getBonusPhysicalResistanceFormula();

        if(resistanceFormula.Equals(Constants.zeroRating) && (getSlotID() != headSlotIndex || getSlotID() == offHandSlotIndex))
        {
            return (Constants.resistanceBonusPerTier*getTier()) + "";
        } else
        {
            return resistanceFormula;
        }
    }
     public override string getBonusMentalResistanceFormula()
    {
        string resistanceFormula = base.getBonusMentalResistanceFormula();

        if(resistanceFormula.Equals(Constants.zeroRating) && (getSlotID() == headSlotIndex || getSlotID() == offHandSlotIndex))
        {
            return (Constants.resistanceBonusPerTier*getTier()) + "";
        } else
        {
            return resistanceFormula;
        }
    }

    public override string getInvulnerableFormula()
    {
        if(!base.getInvulnerableFormula().Equals(Constants.zeroRating))
        {
            return base.getInvulnerableFormula();
        }

        int slotMod = 1;

        if(getSlotID() == offHandSlotIndex)
        {
            slotMod = 2;
        }

        return ((getTier() + 1)*slotMod).ToString();
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

    public override int getSlotID()
    {
        return slotID;
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

        if (!getInvulnerableFormula().Equals(Constants.zeroRating))
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getInvulnerableBlock(DamageCalculator.calculateFormula(getInvulnerableFormula(), getStatSource()).ToString()), getInvulnerableFormula()));
        }

        if (!getBonusPhysicalResistanceFormula().Equals(Constants.zeroRating))
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getPhysicalResistBlock(DamageCalculator.calculateFormula(getBonusPhysicalResistanceFormula(), getStatSource()).ToString()+"%"), getBonusPhysicalResistanceFormula()));
        }

        if (!getBonusMentalResistanceFormula().Equals(Constants.zeroRating))
        {
            buildingBlocks.Add(DescriptionPanelBuildingBlock.getBlockWithFormula(DescriptionPanelBuildingBlock.getMentalResistBlock(DamageCalculator.calculateFormula(getBonusMentalResistanceFormula(), getStatSource()).ToString()+"%"), getBonusMentalResistanceFormula()));
        }

        return buildingBlocks;
    }
}

