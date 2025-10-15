using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : ActivatedPassive //passives are (currently) mostly used to explain to the player some mechanic that happens naturally,
{                                              //like regeneration
    public const string passiveAbilityType = "Passive";

    private MultiStackTrait multiStackTrait;

    public PassiveAbility(CombatActionSettings settings) :
    base(settings)
    {

    }
    public PassiveAbility(CombatActionSettings settings, MultiStackTrait multiStackTrait) :
    base(settings)
    {
        this.multiStackTrait = multiStackTrait;
    }

    //        statAbilityDictionary.Add(currentKey, new PassiveAbility(CombatActionSettings.build(currentKey, DescriptionParams.build(TraitList.exitStrategy2Round.getName(), TraitList.exitStrategy2Round.getDescription(), TraitList.exitStrategy2Round.getIconName()), FrequencyParams.build(zeroSlotMax, noCooldown))));
    public PassiveAbility(string key, Trait trait) :
    base(CombatActionSettings.build(key, DescriptionParams.build(trait.getName(), trait.getDescription(), trait.getIconName()), FrequencyParams.build(AbilityList.zeroSlotMax, AbilityList.noCooldown)))
    {

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

    public override GameObject getDescriptionPanelFull(PanelType panelType)
    {
        if (multiStackTrait != null)
        {
            return getMultiStackDescriptionPanelFull(panelType);
        }

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

    public override GameObject getRowType(RowType rowType)
    {
        if (multiStackTrait == null)
        {
            return base.getRowType(rowType);
        }

        string rowTypeName = "";

        switch (rowType)
        {
            case RowType.JournalCategory:
                rowTypeName = PrefabNames.multiStackPerkEntryRow;
                break;
            case RowType.StatRequirements:
                rowTypeName = PrefabNames.multiStackableAbilityRow;
                break;
            case RowType.LevelUp:
                rowTypeName = PrefabNames.multiStackableNoDamageActionLevelUpRow;
                break;
            default:
                return base.getRowType(rowType);
        }

        return DescriptionPanel.getDescriptionPanel(rowTypeName);
    }

    public override void describeSelfFull(DescriptionPanel panel)
    {
        base.describeSelfFull(panel);

        if (multiStackTrait == null)
        {
            return;
        }

        MultiStackableTraitDescriptionPanel multiPanel = (MultiStackableTraitDescriptionPanel)panel;

        for (int index = 0; index < multiPanel.iconPanels.Length; index++)
        {
            DescriptionPanel.setImage(multiPanel.iconPanels[index], Helpers.loadSpriteFromResources(multiStackTrait.stackableTraits[index].getIconName()));
        }
    }

    public override void describeSelfRow(DescriptionPanel panel)
    {
        base.describeSelfRow(panel);

        if(multiStackTrait == null)
        {
            return;
        }

        MultiStackableTraitDescriptionPanel multiPanel = (MultiStackableTraitDescriptionPanel) panel; 

        for (int index = 0; index < multiPanel.iconPanels.Length && index < multiPanel.stackCounters.Length; index++)
        {
            DescriptionPanel.setImage(multiPanel.iconPanels[index], Helpers.loadSpriteFromResources(multiStackTrait.stackableTraits[index].getIconName()));
        }
    }

    //ISortable Methods
    public override string getDisplayType()
    {
        return passiveAbilityType;
    }

    public override string getType()
    {
        return getDisplayType();
    }
}
