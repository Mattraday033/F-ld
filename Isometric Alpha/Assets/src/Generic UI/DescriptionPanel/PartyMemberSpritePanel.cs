using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PartyMemberSpritePanel : DescriptionPanel, ICounter
{
    public GameObject newPartyMemberText;
    public GameObject levelUpSymbol;
    public AbilityMenuManagerWithPassives abilityMenuManager;

    public Button partyMemberSpriteButton;

    public DescriptionPanel zoneOfInfluenceIcon;

    public TextMeshProUGUI healthText;
    public HealthBarManager healthBar;

    private void OnEnable()
    {
        addListeners();
    }

    private void OnDisable()
    {   
        removeListeners();
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

        if(ScreenManager.currentPartyMember == null)
        {
            ScreenManager.currentPartyMember = partyMember.stats;
        } 
        
        if(ScreenManager.currentPartyMember.Equals(partyMember.stats))
        {
            partyMemberSpriteButton.onClick.Invoke();
            partyMemberSpriteButton.interactable = false;
        } else
        {
            partyMemberSpriteButton.interactable = true;
        }

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

        updateCounter();
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

    #region ICounter

    public void addListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.AddListener(updateCounter);
        }

        AllyStats.OnPartyMemberUpgraded.AddListener(levelUpSymbolVisibilityCheck);
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
        
        AllyStats.OnPartyMemberUpgraded.RemoveListener(levelUpSymbolVisibilityCheck);
    }

    public void updateCounter()
    {
        Stats stats = Stats.convertIDescribableToStats(getObjectBeingDescribed());

        healthText.text = stats.currentHealth + "/" + stats.getTotalHealth();

        healthBar.setTotalHealth(stats.getTotalHealth());
        healthBar.setMissingHealth(stats.getTotalHealth() - stats.currentHealth);
    }

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(Stats.OnHealthChange);
        listOfEvents.Add(ScreenManager.OnScreenInteriorUpdate);

        return listOfEvents;
    }

    private IEnumerator updateHealthBarAfterDescribableIsSet()
    {
        yield return new WaitForEndOfFrame();

        updateCounter();
    }

    #endregion
}
