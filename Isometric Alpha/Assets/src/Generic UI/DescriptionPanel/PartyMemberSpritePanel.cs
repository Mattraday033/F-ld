using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberSpritePanel : DescriptionPanel
{
    public GameObject newPartyMemberText;
    public GameObject levelUpSymbol;
    public AbilityMenuManagerWithPassives abilityMenuManager;

    public DescriptionPanel zoneOfInfluenceIcon;

    private void OnEnable()
    {
        AllyStats.OnPartyMemberUpgraded.AddListener(levelUpSymbolVisibilityCheck);
    }

    private void OnDisable()
    {
        AllyStats.OnPartyMemberUpgraded.RemoveListener(levelUpSymbolVisibilityCheck);
    }

    private void OnDestroy()
    {
        NewPartyMemberManager.PartyMemberNoLongerNew.RemoveListener(determineNewPartyMemberTextVisibility);
    }

    private void levelUpSymbolVisibilityCheck()
    {
        if (levelUpSymbol == null || levelUpSymbol is null || getObjectBeingDescribed() == null)
        {
            return;
        }

        AllyStats stats = Stats.convertIDescribableToStats(getObjectBeingDescribed()) as AllyStats;

        if (stats.xp >= AllyStats.xpNeededToLevelUp)
        {
            levelUpSymbol.SetActive(true);
        }
        else
        {
            levelUpSymbol.SetActive(false);
        }

        setText(levelText, stats.getLevel());
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

        determineNewPartyMemberTextVisibility();

        PartyMember partyMember = (PartyMember) describable;

        // if (iconPanel != null && !(iconPanel is null))
        // {
        //     iconPanel.color = partyMember.getSpriteColor();
        // }

        levelUpSymbolVisibilityCheck();

        // zoiTraitCheck();

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

        iconPanel.sprite = partyMember.stats.getSpriteIcon();
        iconPanel.color = Color.white;
    }

    private void determineNewPartyMemberTextVisibility()
    {
        if(NewPartyMemberManager.partyMemberIsNew(getObjectBeingDescribed() as PartyMember))
        {
            NewPartyMemberManager.PartyMemberNoLongerNew.AddListener(determineNewPartyMemberTextVisibility);
            newPartyMemberText.SetActive(true);
        } else
        {
            NewPartyMemberManager.PartyMemberNoLongerNew.RemoveListener(determineNewPartyMemberTextVisibility);
            newPartyMemberText.SetActive(false);
        }
    }
}
