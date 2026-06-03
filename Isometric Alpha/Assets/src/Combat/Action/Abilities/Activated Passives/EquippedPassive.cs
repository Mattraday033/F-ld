using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EquippedPassive : Ability
{
	private const int mustBeUnique = 1;
	private const int noCooldown = 1;
	private const string personalRangeTitle = "Personal";

	private List<Trait> relatedTraits = new List<Trait>();

	public EquippedPassive(CombatActionSettings settings) :
	base(settings)
	{

	}

	public override void applySettings(CombatActionSettings settings)
	{
		if (settings.appliedTrait != null)
		{
			settings.descriptionParams = DescriptionParams.build(settings.appliedTrait.getName(), iconName: settings.appliedTrait.getIconName(), useDescription: settings.appliedTrait.getDescription());
			settings.frequencyParams = FrequencyParams.build(mustBeUnique, noCooldown);
		}

		if (settings.relatedTraits != null)
		{
			relatedTraits.AddRange(settings.relatedTraits);
		}

		base.applySettings(settings);
	}

	public override string getDisplayType()
	{
		return AbilityList.equippedPassiveActionTypeName;
	}

	public override string getRangeTitle()
	{
		return personalRangeTitle;
	}

	public override bool unactivatable()
	{
		return true;
	}

	public override bool autoApply()
	{
		return true;
	}

	public override List<Trait> getNonAppliedRelatedTraits()
	{
		return relatedTraits;
	}

	public override Trait getAppliedTraitForDescription()
	{
		return null;
	}

    public override bool canBePlacedInPassiveSlot()
    {
        return true;
    }

    public override GameObject getDescriptionPanelFull(PanelType panelType)
    {
        string panelTypeName = "";

        switch (panelType)
        {
            case PanelType.CombatHover:
                panelTypeName = PrefabNames.harmlessCombatActionHoverDescriptionPanel;
                break;
            case PanelType.AbilityEditor:
            case PanelType.Standard:
                panelTypeName = PrefabNames.noDamageCombatActionDescPanelFull;
                break;
            case PanelType.GlossaryDescription:
                panelTypeName = PrefabNames.perkDescriptionPanelFull;
                break;
            default:
                return base.getDescriptionPanelFull(panelType);
        }

        return DescriptionPanel.getDescriptionPanel(panelTypeName);
    }

	public override GameObject getRowType(RowType rowType)
	{
		string rowTypeName = "";

		switch (rowType)
		{
			case RowType.LevelUp:
				rowTypeName = PrefabNames.noDamageCombatActionDescPanelRow;
				break;
			default:
				return base.getRowType(rowType);
		}

		return DescriptionPanel.getDescriptionPanel(rowTypeName);
	}

	public override string getUseDescription()
    {
        if(getAppliedTrait() == null)
        {
            Debug.LogError("Null applied Trait in Equipped Passive: " + getName());
        }

        return "Applies the " + getAppliedTrait().getName() + " Trait at the start of Combat to whoever equips this Ability to their Action Wheel.";
    }

	//ISortable Methods

	public override string getType()
	{
		return "Equipped Passive";
	}

	//IDescribableInBlocks methods
	public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getActionTypeBlock(getType(), HoverMessageList.actionTypePrefix + getType()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getUseDescription()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: getIconName()));

        buildingBlocks.AddRange(getStatBoostDescriptionBuildingBlocks(getStatSource(), this));

		return buildingBlocks;
	}

    public override List<IDescribable> getRelatedDescribables()
    {
        List<IDescribable> relatedDescribables = base.getRelatedDescribables();

        if(getAppliedTrait() != null)
        {
            relatedDescribables.Add(getAppliedTrait());
        }

        return relatedDescribables;
    }
}
