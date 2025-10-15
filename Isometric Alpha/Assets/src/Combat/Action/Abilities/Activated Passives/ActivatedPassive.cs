using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ActivatedPassive : Ability
{
	private const int mustBeUnique = 1;
	private const int noCooldown = 1;
	private const string personalRangeTitle = "Personal";

	public const string displayType = "Equipped Passive";

	private ArrayList relatedTraits = new ArrayList();

	public ActivatedPassive(CombatActionSettings settings) :
	base(settings)
	{

	}

	public override void applySettings(CombatActionSettings settings)
	{
		if (settings.appliedTrait != null)
		{
			settings.descriptionParams = DescriptionParams.build(settings.appliedTrait.getName(), settings.appliedTrait.getDescription(), settings.appliedTrait.getIconName());
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
		return displayType;
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

	public override ArrayList getNonAppliedRelatedTraits()
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

	//ISortable Methods

	public override string getType()
	{
		return "Equipped Passive";
	}

	//IDescribableInBlocks methods
	public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, getName()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getActionTypeBlock(getType()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getUseDescription()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, getIconName()));

		return buildingBlocks;
	}
}
