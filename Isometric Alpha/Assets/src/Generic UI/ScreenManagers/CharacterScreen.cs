using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CharacterScreen : ScreenManager, ICounter
{
    public TextMeshProUGUI playerNameText;

    public Image characterSprite;

    public static int getCurrentDisplayedStatLevel()
    {
        switch (AbilityGridSideTab.getDescribableListType())
        {
            case DescribableList.Strength:
                return OverallUIManager.getCurrentPartyMember().getStrength();
            case DescribableList.Dexterity:
                return OverallUIManager.getCurrentPartyMember().getDexterity();
            case DescribableList.Wisdom:
                return OverallUIManager.getCurrentPartyMember().getWisdom();
            case DescribableList.Charisma:
                return OverallUIManager.getCurrentPartyMember().getCharisma();
            default:
                return 0;
        }
    }

    private void OnDestroy()
    {
        removeListeners();
    }

    public override void updateCounter()
    {
        playerNameText.text = currentPartyMember.getName().Replace(PartyManager.playerMarker, "");
        characterSprite.sprite = currentPartyMember.getSpriteIcon();
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();
        listOfEvents.Add(CombatActionArray.OnCombatActionArrayChange);
        listOfEvents.Add(Inventory.OnInventoryChange);
        listOfEvents.Add(EquippedItems.OnEquipmentChange);
        listOfEvents.Add(Stats.OnStatsChange);
        listOfEvents.Add(AbilityGridSideTab.OnSideTabChosen);
        listOfEvents.Add(PartySpriteGridRow.OnPartyMemberSelected);
        listOfEvents.Add(OnScreenInteriorUpdate);

        return listOfEvents;
    }

    public static bool levelUpCapable()
    {
        return currentPartyMember.xp >= AllyStats.xpNeededToLevelUp;
    }

    public override bool requiresPartyMemberSelectionGrid()
    {
        return true;
    }

    public override DescribableList getDefaultDescribableList()
    {
        return DescribableList.MainHandWeaponsAsActions;
    }

    // public override void revealDescriptionPanelSet(IDescribable objectToDescribe)
    // {
    //     AllyStats statsToDescribe = Stats.convertIDescribableToStats(objectToDescribe) as AllyStats;

    //     if (statsToDescribe == null)
    //     {
    //         return;
    //     }

    //     currentPartyMember = statsToDescribe;

    //     playerNameText.text = getCurrentPartyMember().getName().Replace(PartyManager.playerMarker, "");
    //     characterSprite.color = currentPartyMember.getSpriteColor();

    //     abilityMenuManager.actionArraySource = getCurrentPartyMember();

    //     abilityMenuManager.populateAbilityMenuFromCombatActionArray();
    //     abilityMenuManager.disableLockedPassiveButtons();

    //     descriptionPanelSlots[sideStatsSlotIndex].setPrimaryDescribable(getCurrentPartyMember());

    //     // setAbilityGridToDefaultTab();
    // }

}
