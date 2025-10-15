using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

//[System.Serializable]
public class Weapon : EquippableItem, IJSONConvertable, IFormulaSource
{
	public const string typeIconName = "Weapon";
	public const string subtype = "Weapon";
	public const int mainHandSlotIndex = 6;
    public const int offHandSlotIndex = 0;  //Both the Armor and Weapon classes have an offHandSlotIndex because both types of item can go in
											//this slot. They're the same index, it doesn't matter which one is called but to prevent confusion the 
											//Armor index should be called for Armors and the Weapon index for Weapons.
	//[SerializeField] 
    private int rangeIndex;
    //[SerializeField] 
    private string damageFormula;
    //[SerializeField] 
    private string critFormula;
    //[SerializeField] 
    private string iconName;
	//[SerializeField] 
    private bool isTwoHanded;

	public Weapon(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, int worth, int slotID) : base(listId, key, loreDescription, subtype, worth, slotID)
	{
		this.isTwoHanded = false;
		this.damageFormula = damageFormula;
		this.critFormula = critFormula;
		this.iconName = iconName;
		this.rangeIndex = 1;
	}

	public Weapon(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, int rangeIndex, int worth, int slotID, bool isTwoHanded) : base(listId, key, loreDescription, subtype, worth, slotID)
	{
		this.isTwoHanded = isTwoHanded;
		this.damageFormula = damageFormula;
		this.critFormula = critFormula;
		this.iconName = iconName;
		this.rangeIndex = rangeIndex;
	}

	[JsonConstructor]
	public Weapon(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, int rangeIndex, int worth, int slotID, int quantity, bool isTwoHanded) : base(listId, key, loreDescription, subtype, worth, slotID, quantity)
	{
		this.isTwoHanded = isTwoHanded;
		this.damageFormula = damageFormula;
		this.critFormula = critFormula;
		this.iconName = iconName;
		this.rangeIndex = rangeIndex;
	}

	public bool getIsTwoHanded()
	{
		return isTwoHanded;
	}

	public override string getDamageFormula()
	{
		return damageFormula;
	}

	public override int getDamageFormulaTotal()
	{
		return DamageCalculator.calculateFormula(getDamageFormula(), getStatSource());
	}

	public override string getDamageTotalForDisplay()
	{
		if (CombatStateManager.inCombat)
		{
			return "" + getDamageFormulaTotal();
		}

		return "" + getDamageFormulaTotal();
	}

	public string getDamageFormulaForDisplayAlternate()
	{
		return getDamageFormula();
	}

	public override string getCritFormula()
	{
		return critFormula;
	}

	public override int getCritFormulaTotal()
	{
		return DamageCalculator.calculateFormula(getCritFormula(), getStatSource());
	}

	public string getCritFormulaForDisplayAlternate()
	{
		return "(" + getCritFormula() + ")%";
	}

	public override string getIconName()
	{
		return iconName;
	}

	public string getRange()
	{
		return Range.getRangeTitle(getRangeIndex());
	}

	public int getRangeIndex()
	{
		return rangeIndex;
	}

	public override bool removeFromInventoryWhenCreatingCombatAction()
	{
		return true;
	}

	public override int getArmorRating()
	{
		return 0;
	}

	public override bool isUnequippable()
	{
		if (getSlotID() == offHandSlotIndex && getKey().Equals(ItemList.getOffHandFist().getKey()))
		{
			return false;
		}

		return true;
	}

	public override CombatAction getCombatAction()
	{
		return new Attack(this);
	}

	public override GameObject getRowType(RowType rowType)
	{
		string rowTypeName = "";

		switch (rowType)
		{
			case RowType.CompanionAbilities:
				rowTypeName = PrefabNames.companionAbilityRow;
				break;
			case RowType.AbilityEditor:
				rowTypeName = PrefabNames.actionEditorRow;
				break;
			default:
				return base.getRowType(rowType);

		}

        return Resources.Load<GameObject>(rowTypeName);
	}

	public override GameObject getDescriptionPanelFull(PanelType panelType)
	{
		string panelTypeName = "";

		switch (panelType)
		{
			case PanelType.Combat:
			case PanelType.CombatHover:

				panelTypeName = PrefabNames.offhandHoverDescriptionPanel;
				break;

			case PanelType.Standard:

				if (getSlotID() == mainHandSlotIndex)
				{
					panelTypeName = PrefabNames.weaponDescPanelFull;
				}
				else
				{
					panelTypeName = PrefabNames.offHandWeaponDescPanelFull;
				}

				break;
			default:
				throw new IOException("Unknown PanelType: " + panelType);
		}

		return DescriptionPanel.getDescriptionPanel(panelTypeName);
	}

	public override void describeSelfFull(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);

		DescriptionPanel.setText(panel.slotText, getSlotIDForDisplay());

		if (!CombatStateManager.inCombat) //will use the Attack class damage/crit display methods in combat
		{
			DescriptionPanel.setText(panel.damageText, getDamageTotalForDisplay());
			DescriptionPanel.setText(panel.critRatingText, getCritTotalForDisplay());
		}

		DescriptionPanel.setText(panel.rangeText, getRange());
		DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(getIconName()));
	}

	public override void describeSelfRow(DescriptionPanel panel)
	{
		base.describeSelfFull(panel);

		DescriptionPanel.setText(panel.slotText, getSlotIDForDisplay());
		DescriptionPanel.setText(panel.damageText, getDamageFormulaTotal());
		DescriptionPanel.setText(panel.critRatingText, getCritFormulaTotalForDisplay());

		DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(getIconName()));
	}

	public override string getTypeIconName()
	{
		switch (getSlotID())
		{
			case Weapon.mainHandSlotIndex:

				if (isTwoHanded)
				{
					return EquippableItem.twoHandedSlotIconName;
				}
				else
				{
					return EquippableItem.oneHandedSlotIconName;
				}

			case Weapon.offHandSlotIndex:
				return EquippableItem.offHandSlotIconName;

			default:
				throw new IOException("Unexpected slotID: " + getSlotID());
		}
	}

	public override string getSlotIconName()
	{
        return getSlotIconName(getSlotID());
	}
    
    public static string getSlotIconName(int slotIndex)
	{
		switch (slotIndex)
		{
			case Weapon.mainHandSlotIndex:
                return mainHandSlotIconName;
			case Weapon.offHandSlotIndex:
				return EquippableItem.offHandSlotIconName;

			default:
				throw new IOException("Unexpected slotID: " + slotIndex);
		}
	}

	//IBuildableWithBlocks methods

    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = base.getDescriptionBuildingBlocks();

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, getName()));

        buildingBlocks.Insert(1, DescriptionPanelBuildingBlock.getDamageBlock(getDamageTotalForDisplay(), getDamageFormulaForDisplayAlternate()));
        buildingBlocks.Insert(2, DescriptionPanelBuildingBlock.getCritBlock(getCritTotalForDisplay(), getCritFormulaForDisplayAlternate()));

        buildingBlocks.RemoveAt(buildingBlocks.Count - 1); //remove base range version of slot icon

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getIconName()));

        if (getSlotID() == mainHandSlotIndex)
        {
            buildingBlocks.Insert(3, DescriptionPanelBuildingBlock.getRangeBlock(getRangeIndex()));
            buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getTypeIconName()));
        }
        else
        {
            buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getSlotIconName()));
        }

        if (appliesStanceStacks())
        {
            buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, IconList.stanceWeaponIconName));
        }

        return buildingBlocks;
    }
}
