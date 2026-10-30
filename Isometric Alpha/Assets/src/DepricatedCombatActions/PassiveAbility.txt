using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : EquippedPassive //passives are (currently) mostly used to explain to the player some mechanic that happens naturally,
{                                              //like regeneration

    public override bool canBePlacedInActionSlot => false;

    public PassiveAbility(CombatActionSettings settings, List<TargetingLogic> targetingLogics = null) :
    base(settings, targetingLogics)
    {

    }

    //        statAbilityDictionary.Add(currentKey, new PassiveAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(TraitList.exitStrategy2Round.displayName, TraitList.exitStrategy2Round.getDescription(), TraitList.exitStrategy2Round.getIconName()), FrequencyParams.build(zeroSlotMax, noCooldown))));
    public PassiveAbility(string key, Trait trait, List<TargetingLogic> targetingLogics = null) :
    base(CombatActionSettings.build(key, DescriptionParams.build(trait.displayName, iconName: trait.getIconName(), useDescription: trait.getDescription()), frequencyParams: FrequencyParams.build(AbilityList.zeroSlotMax, AbilityList.noCooldown)), targetingLogics)
    {

    }

	public override string getUseDescription()
    {
        return useDescription;
    }

    public override int getMaximumSlots()
	{
		return 0;
	}

    //IDescribable

    public override bool ineligible()
    {
        return true;
    }   

    public override void addSlotsTextToRow(DescriptionPanel panel)
    {
        DescriptionPanel.setText(panel.slotsUsedText, "Passive");
    }

    //ISortable Methods
    public override string getDisplayType()
    {
        return AbilityList.passiveActionTypeName;
    }

    public override string getType()
    {
        return getDisplayType();
    }
}


public class ZoneOfInfluenceDescriptorAbility : PassiveAbility 
{                                             
    private ZoneOfInfluenceTrait zoiTrait;

    public ZoneOfInfluenceDescriptorAbility(string key, ZoneOfInfluenceTrait trait, List<TargetingLogic> targetingLogics = null) :
    base(CombatActionSettings.build(key, DescriptionParams.build(trait.displayName, iconName: trait.getIconName(), useDescription: trait.getDescription()), frequencyParams: FrequencyParams.build(AbilityList.zeroSlotMax, AbilityList.noCooldown)), targetingLogics)
    {
        zoiTrait = trait;
    }

    public override int getRequiredStatLevel()
    {
        return 1;
    }

	public override void describeSelfRow(DescriptionPanel panel)
	{
		base.describeSelfRow(panel);
		
		if(panel.statText != null && !(panel.statText is null) && getRequiredStatLevel() >= 0)
		{
			panel.statText.text = "" + getRequiredStatLevelForDisplay();
		}
	}

    public override List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        return zoiTrait.getDescriptionBuildingBlocks();
    }
}