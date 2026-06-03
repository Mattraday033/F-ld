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

	public const int offHandSlotIndex = 0;  
	public const int headSlotIndex = 1;     
	public const int bodySlotIndex = 2;     
	public const int handsSlotIndex = 3;
	public const int feetSlotIndex = 4;
	public const int trinketSlotIndex = 5;

	private const double maximumArmorDamageReduction = 99.0;
    protected readonly static Stats worthStatsSource = new AllyStats("Worth Stats Calc", Constants.sizeFour, Constants.sizeFour, Constants.sizeFour, Constants.sizeFour);

    private int armorTier;
    private int slotID;

	public Armor(ItemListID listID, string key, string loreDescription, int slotID, int armorTier, 
                                                                                                         string damageFormula = Constants.zeroRating, 
                                                                                                         string critFormula = Constants.zeroRating) : 
    base(listID, key, loreDescription, damageFormula, critFormula, subtype) 
    {
        this.slotID = slotID;
        this.armorTier = armorTier;
        this.worth = calculateWorth(DamageCalculator.calculateFormula(getArmorFormula(), worthStatsSource), slotID);
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
        if(armorRating < 0)
        {
            armorRating = 0;
        }

		double damageReduction = (double) armorRating;

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
    
    public override string getBonusWoundResistanceFormula()
    {
        string resistanceFormula = base.getBonusWoundResistanceFormula();

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

        int slotMod;

        switch(getSlotID())
        {
            case offHandSlotIndex:
                slotMod = 2;
                break;
            case trinketSlotIndex:
                slotMod = 0;
                break; 
            default:
                slotMod = 1;
                break;
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

        DescriptionPanel.setText(panel.armorRatingText, getArmorRating() + "%");
        DescriptionPanel.setText(panel.invulnerabilityText, getInvulnerabilityForDisplay());
        DescriptionPanel.setText(panel.slotText, getSlotIDForDisplay());
        DescriptionPanel.setImage(panel.typeIconPanel, Helpers.loadSpriteFromResources(getTypeIconName()));
        DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(getIconName()));
        DescriptionPanel.setImageColor(panel.typeIconBackgroundPanel, getTypeIconBackgroundColor());
	}

	public override string getTypeIconName()
	{
		return getSlotIconName();
	}

	public override string getSlotIconName()
	{
        return getSlotIconName(getSlotID());
	}

    public override int getSlotID()
    {
        return slotID;
    }
    
    public static string getSlotIconName(int slotIndex)
	{
		switch (slotIndex)
		{
			case offHandSlotIndex:
				return offHandSlotIconName;

			case headSlotIndex:
				return headSlotIconName;

			case bodySlotIndex:
				return bodySlotIconName;

			case handsSlotIndex:
				return handsSlotIconName;

			case feetSlotIndex:
				return feetSlotIconName;

			case trinketSlotIndex:
				return trinketSlotIconName;

			default:
				throw new IOException("Unexpected slotID: " + slotIndex);
		}
	}

	//IBuildableWithBlocks methods
    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.AddRange(getStatBoostDescriptionBuildingBlocks(getStatSource(), this));
        
        buildingBlocks.AddRange(base.getDescriptionBuildingBlocks());

        return buildingBlocks;
    }
}
