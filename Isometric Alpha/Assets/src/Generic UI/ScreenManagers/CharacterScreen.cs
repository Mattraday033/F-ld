using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CharacterScreen : ScreenWithGeneratedPartyTabs, ICounter
{
    public AbilityMenuManagerWithPassives abilityMenuManager;

    private const int nonPrimaryStatTabOffset = 2;
    private const int abilityGridIndex = 1;
    private const int sideStatsSlotIndex = 0;
    private const int upgradeDescriptionPanelSlotIndex = 1;

    public TextMeshProUGUI playerNameText;

    public static AllyStats currentPartyMember;

    public Image characterSprite;

    public DescriptionPanelSlot local;
    public static DescriptionPanelSlot upgradeDescriptionPanelSlot;

    public override void Awake()
    {
        base.Awake();

        upgradeDescriptionPanelSlot = descriptionPanelSlots[upgradeDescriptionPanelSlotIndex];

        revealDescriptionPanelSet(getCurrentPartyMember());

        addListeners();
    }

    public override int getAbilityGridIndex()
    {
        return abilityGridIndex;
    }

    public override void setUpTabs()
    {
        TabCollection abilityGridTabCollection = tabCollections[getAbilityGridIndex()];

        base.setUpTabs();

        tabCollections = Helpers.appendArray<TabCollection>(tabCollections, abilityGridTabCollection);
    }

    public override void setToDefaultScreenState()
    {
        base.setToDefaultScreenState();

        setAbilityGridToDefaultTab();
    }

    private void setAbilityGridToDefaultTab()
    {
        currentTabCollection = getAbilityGridIndex();

        int buttonIndex = ((int)getCurrentPartyMember().getHighestPrimaryStats()[0]) + nonPrimaryStatTabOffset;


        if (tabCollections[getAbilityGridIndex()].getCurrentTabIndex() != buttonIndex)
        {
            tabCollections[getAbilityGridIndex()].collection[buttonIndex].button.onClick.Invoke();
        }
        else
        {
            populateGrid(getAbilityGridIndex());
        }
    }

    public static int getCurrentDisplayedStatLevel()
    {
        int currentTabCollection = OverallUIManager.currentScreenManager.currentTabCollection;
        TabCollection collection = OverallUIManager.currentScreenManager.tabCollections[currentTabCollection];

        switch (collection.getCurrentTabIndex())
        {
            case 2:
                return OverallUIManager.getCurrentPartyMember().getStrength();
            case 3:
                return OverallUIManager.getCurrentPartyMember().getDexterity();
            case 4:
                return OverallUIManager.getCurrentPartyMember().getWisdom();
            case 5:
                return OverallUIManager.getCurrentPartyMember().getCharisma();
            default:
                return 0;
        }
    }

    private void OnDestroy()
    {
        removeListeners();
    }

    public void addListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.AddListener(updateCounter);
        }
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
    }

    public void updateCounter()
    {
        descriptionPanelSlots[sideStatsSlotIndex].resetPrimary();
        populateGrid(0);
    }

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(CombatActionArray.OnCombatActionArrayChange);
        listOfEvents.Add(EquippedItems.OnEquipmentChange);
        listOfEvents.Add(Stats.OnStatsChange);

        return listOfEvents;
    }

    public override bool levelUpCapable()
    {
        return getCurrentPartyMember().xp >= AllyStats.xpNeededToLevelUp;
    }

    public override float getNumberOfUpgradeTilesPerRow()
    {
        return 6f;
    }

    public override void revealDescriptionPanelSet(IDescribable objectToDescribe)
    {
        AllyStats statsToDescribe = Stats.convertIDescribableToStats(objectToDescribe) as AllyStats;

        if (statsToDescribe == null)
        {
            return;
        }

        currentPartyMember = statsToDescribe;

        playerNameText.text = getCurrentPartyMember().getName();
        characterSprite.color = currentPartyMember.getSpriteColor();

        abilityMenuManager.actionArraySource = getCurrentPartyMember();

        abilityMenuManager.populateAbilityMenuFromCombatActionArray();
        abilityMenuManager.disableLockedPassiveButtons();

        descriptionPanelSlots[sideStatsSlotIndex].setPrimaryDescribable(getCurrentPartyMember());

        // setAbilityGridToDefaultTab();
    }

    public override AllyStats getCurrentPartyMember()
    {
        if (currentPartyMember == null)
        {
            currentPartyMember = PartyManager.getPlayerStats();
        }

        return currentPartyMember;
    }
}
