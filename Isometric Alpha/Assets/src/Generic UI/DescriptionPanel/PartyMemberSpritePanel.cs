using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberSpritePanel : DescriptionPanel
{

    public GameObject levelUpSymbol;
    public AbilityMenuManagerWithPassives abilityMenuManager;

    public DescriptionPanel zoneOfInfluenceIcon;

    private void OnEnable()
    {
        UpgradePartyMemberDecisionPanel.OnPartyMemberUpgraded.AddListener(levelUpSymbolVisibilityCheck);
    }

    private void OnDisable()
    {
        UpgradePartyMemberDecisionPanel.OnPartyMemberUpgraded.RemoveListener(levelUpSymbolVisibilityCheck);
    }

    private void levelUpSymbolVisibilityCheck()
    {
        if (levelUpSymbol == null || levelUpSymbol is null || getObjectBeingDescribed() == null)
        {
            return;
        }

        AllyStats stats = Stats.convertIDescribableToStats(getObjectBeingDescribed()) as AllyStats;

        if (PartyMember.getNextUpgradeCost(stats.getLevel()) <= AffinityManager.getTotalAffinity() && stats.getLevel() < stats.getLevelMaximum())
        {
            levelUpSymbol.SetActive(true);
        }
        else
        {
            levelUpSymbol.SetActive(false);
        }
    }

    private void zoiTraitCheck()
    {
        if (additionalSlots.Length >= 2 && additionalSlots[1] != null)
        {
            additionalSlots[1].setPrimaryDescribable(Stats.convertIDescribableToStats(getObjectBeingDescribed()).getZoneOfInfluenceTrait());
        }
    }

    public override void setObjectBeingDescribed(IDescribable describable)
    {
        base.setObjectBeingDescribed(describable);

        PartyMember partyMember = (PartyMember)describable;

        // if (iconPanel != null && !(iconPanel is null))
        // {
        //     iconPanel.color = partyMember.getSpriteColor();
        // }

        levelUpSymbolVisibilityCheck();

        zoiTraitCheck();

        if (abilityMenuManager != null)
        {
            abilityMenuManager.actionArraySource = partyMember.stats;

            abilityMenuManager.populateAbilityMenuFromCombatActionArray();
            abilityMenuManager.disableLockedPassiveButtons();
        }

        if (zoneOfInfluenceIcon != null)
        {
            zoneOfInfluenceIcon.setObjectBeingDescribed(partyMember.stats.getZoneOfInfluenceTrait());
            partyMember.stats.getZoneOfInfluenceTrait().describeSelfFull(zoneOfInfluenceIcon);
        }
    }


}
