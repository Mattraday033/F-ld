using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : EquippedPassive //passives are (currently) mostly used to explain to the player some mechanic that happens naturally,
{                                              //like regeneration
    public PassiveAbility(CombatActionSettings settings) :
    base(settings)
    {

    }

    //        statAbilityDictionary.Add(currentKey, new PassiveAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(TraitList.exitStrategy2Round.getName(), TraitList.exitStrategy2Round.getDescription(), TraitList.exitStrategy2Round.getIconName()), FrequencyParams.build(zeroSlotMax, noCooldown))));
    public PassiveAbility(string key, Trait trait) :
    base(CombatActionSettings.build(key, DescriptionParams.build(trait.getName(), iconName: trait.getIconName(), useDescription: trait.getDescription()), frequencyParams: FrequencyParams.build(AbilityList.zeroSlotMax, AbilityList.noCooldown)))
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
    
    public override int getMaximumCooldown()
	{
		return 0;
	}

    public override bool canBePlacedInActionSlot()
    {
        return false;
    }

    //IDescribable

    public override bool ineligible()
    {
        return true;
    }   

    public override GameObject getDescriptionPanelFull(PanelType panelType)
    {
        string panelTypeName = "";

        switch (panelType)
        {
            case PanelType.GlossaryDescription:
            case PanelType.AbilityEditor:
            case PanelType.Standard:
                panelTypeName = PrefabNames.passivePerkDescriptionPanel;
                break;
            default:
                return base.getDescriptionPanelFull(panelType);
        }

        return DescriptionPanel.getDescriptionPanel(panelTypeName);
    }

    private GameObject getMultiStackDescriptionPanelFull(PanelType panelType)
    {
        string panelTypeName = "";

        switch (panelType)
        {
            case PanelType.GlossaryDescription:
                panelTypeName = PrefabNames.multiStackPassivePerkDescriptionPanel;
                break;
            case PanelType.AbilityEditor:
            case PanelType.Standard:
                panelTypeName = PrefabNames.multiStackableNoDamageActionDescriptionPanels;
                break;
            default:
                return base.getDescriptionPanelFull(panelType);
        }

        return DescriptionPanel.getDescriptionPanel(panelTypeName);
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

    public ZoneOfInfluenceDescriptorAbility(string key, ZoneOfInfluenceTrait trait) :
    base(CombatActionSettings.build(key, DescriptionParams.build(trait.getName(), iconName: trait.getIconName(), useDescription: trait.getDescription()), frequencyParams: FrequencyParams.build(AbilityList.zeroSlotMax, AbilityList.noCooldown)))
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