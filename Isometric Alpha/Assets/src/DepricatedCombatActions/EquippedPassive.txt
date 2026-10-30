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

	public override bool unactivatable => true;
	public override bool autoApply => true;
	public override bool canBePlacedInPassiveSlot => true;

	public EquippedPassive(CombatActionSettings settings, List<TargetingLogic> targetingLogics = null) :
	base(settings, targetingLogics)
	{

	}

	public override void applySettings(CombatActionSettings settings)
	{
		if (settings.appliedTrait != null)
		{
			settings.descriptionParams = DescriptionParams.build(settings.appliedTrait.displayName, iconName: settings.appliedTrait.getIconName(), useDescription: settings.appliedTrait.getDescription());
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

	public override List<Trait> getNonAppliedRelatedTraits()
	{
		return relatedTraits;
	}

	public override Trait getAppliedTraitForDescription()
	{
		return null;
	}

	public override string getUseDescription()
    {
        if(appliedTrait == null)
        {
            Debug.LogError("Null applied Trait in Equipped Passive: " + uniqueName);
        }

        return "Applies the " + appliedTrait.displayName + " Trait at the start of Combat to whoever equips this Ability to their Action Wheel.";
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

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(displayName));

        buildingBlocks.Add(DescriptionPanelBuildingBlock.getActionTypeBlock(getType(), HoverMessageList.actionTypePrefix + getType()));

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getDescriptionBlock(getUseDescription()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Icon, iconName: iconName));

        buildingBlocks.AddRange(getStatBoostDescriptionBuildingBlocks(getStatSource(), this));

		return buildingBlocks;
	}

    public override List<IDescribable> getRelatedDescribables()
    {
        List<IDescribable> relatedDescribables = base.getRelatedDescribables();

        if(appliedTrait != null)
        {
            relatedDescribables.Add(appliedTrait);
        }

        return relatedDescribables;
    }
}
