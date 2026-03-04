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
    public DescriptionPanelSlot upgradeDescriptionPanelSlot;

    public static DescriptionPanelSlot getUpgradeDescriptionPanelSlot()
    {
        if(OverallUIManager.currentScreenManager as CharacterScreen == null)
        {
            return null;
        }

        CharacterScreen screen = OverallUIManager.currentScreenManager as CharacterScreen;

        return screen.upgradeDescriptionPanelSlot;
    }

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
        characterSprite.sprite = PartyMember.getPortrait(currentPartyMember.getName());
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

    public override KeyCode getExitKeyCode()
    {
        return KeyBindingList.characterScreenKey;
    }

}
