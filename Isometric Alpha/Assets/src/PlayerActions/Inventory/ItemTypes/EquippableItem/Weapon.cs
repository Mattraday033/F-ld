using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class Weapon : EquippableItem, IJSONConvertable
{
	public const string typeIconName = "Weapon";
	public const string subtype = "Weapon";
	public const int mainHandSlotIndex = 6;

    private string rangeName;
    private string iconName;
    private bool isTwoHanded;
    private EffectAnimationType effectAnimationType;

    public Trait traitToApply;

	public Weapon(ItemListID listId, string key, string loreDescription, string damageFormula, string critFormula, string iconName, string rangeName, int worth, bool isTwoHanded, EffectAnimationType effectAnimationType = EffectAnimationType.Slash, Trait traitToApply = null) :
    base(listId, key, loreDescription, damageFormula, critFormula, subtype, worth)
	{
		this.isTwoHanded = isTwoHanded;
		this.iconName = iconName;
		this.rangeName = rangeName;
        this.effectAnimationType = effectAnimationType;
        this.traitToApply = traitToApply;
	}

	public override string convertToJson()
	{
        int quantity = getQuantity();

        if(quantity <= 0)
        {
            Debug.LogError("Quantiy = " + quantity);
        }

		return "{\"listIndex\":\"" + listID.listIndex + "\"," +
				"\"itemIndex\":\"" + listID.itemIndex + "\"," +
				"\"quantity\":\"" + quantity + "\"" +
				"}";

	}

    public override int getSlotID()
    {
        return mainHandSlotIndex;
    }

	public bool getIsTwoHanded()
	{
		return isTwoHanded;
	}

	public override string getIconName()
	{
		return iconName;
	}

	public string getRange()
	{
		return getRangeName();
	}

	public string getRangeName()
	{
		return rangeName;
	}

	public override bool removeFromInventoryWhenCreatingCombatAction()
	{
		return true;
	}

	public override bool isUnequippable()
	{
		return true;
	}

    public override Trait getAppliedTrait()
    {
        if(traitToApply == null)
        {
            return null;
        }

        Trait traitClone = traitToApply.clone();

        return traitClone;
    }

	public override CombatAction getCombatAction(AllyStats stats)
	{
		return new Attack(stats, this);
	}

	public override string getEffectAnimationType()
	{
		return effectAnimationType.ToString();
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
		base.describeSelfRow(panel);

		DescriptionPanel.setText(panel.slotText, getSlotIDForDisplay());
		DescriptionPanel.setText(panel.damageText, getDamageFormulaTotal());
		DescriptionPanel.setText(panel.critRatingText, getCritTotalForDisplay());

		DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(getIconName()));

	}

	public override string getTypeIconName()
	{
        if (isTwoHanded)
        {
            return twoHandedSlotIconName;
        }
        else
        {
            return oneHandedSlotIconName;
        }
	}

	public override string getSlotIconName()
	{
        return mainHandSlotIconName;
	}

    public override string getBonusDamageFormula()
    {
        return DamageCalculator.calculateBonusDamage(getDamageFormula()).ToString();
    }

	//IBuildableWithBlocks methods

    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

        buildingBlocks.AddRange(getStatBoostDescriptionBuildingBlocks(getStatSource(), this));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getRangeBlock(getRangeName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getWorthBlock(getWorthForDisplay()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getLoreDescription()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: getIconName()));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: getSlotIconName()));
        
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: getTypeIconName()));

        if (appliesStanceStacks())
        {
            buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: IconList.stanceWeaponIconName));
        }

        return buildingBlocks;
    }
}
