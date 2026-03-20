using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class CharacterScreen : ScreenManager, ICounter
{
    public TextMeshProUGUI playerNameText;

    public TextMeshProUGUI abilityGridTitleHeader;

    public GameObject amountIconColHeader;
    public GameObject lvlIconColHeader;
    public GameObject strIconColHeader;
    public GameObject dexIconColHeader;
    public GameObject wisIconColHeader;
    public GameObject chaIconColHeader;

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
            case DescribableList.CharacterSpecificAbilities:
                return OverallUIManager.getCurrentPartyMember().getLevel();
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

        setAbilityGridHeaders();
    }

    private void setAbilityGridHeaders()
    {
        DescribableList currentList = AbilityGridSideTab.getDescribableListType();

        switch(currentList)
        {
            case DescribableList.CombatUsableItems:
                abilityGridTitleHeader.text = "Combat Items";
                break;
            case DescribableList.CharacterSpecificAbilities:
                abilityGridTitleHeader.text = "Companion Abilities";
                break;
            case DescribableList.Strength:
            case DescribableList.Dexterity:
            case DescribableList.Wisdom:
            case DescribableList.Charisma:
                abilityGridTitleHeader.text = currentList.ToString() + " Abilities";
                break;
            default:
                abilityGridTitleHeader.text = "Weapons";
                break;
        }

        switch(currentList)
        {
            case DescribableList.CombatUsableItems:
                amountIconColHeader.SetActive(true);
                lvlIconColHeader.SetActive(false);
                strIconColHeader.SetActive(false);
                dexIconColHeader.SetActive(false);
                wisIconColHeader.SetActive(false);
                chaIconColHeader.SetActive(false);
                break;
            case DescribableList.CharacterSpecificAbilities:
                amountIconColHeader.SetActive(false);
                lvlIconColHeader.SetActive(true);
                strIconColHeader.SetActive(false);
                dexIconColHeader.SetActive(false);
                wisIconColHeader.SetActive(false);
                chaIconColHeader.SetActive(false);
                break;
            case DescribableList.Strength:
                amountIconColHeader.SetActive(false);
                lvlIconColHeader.SetActive(false);
                strIconColHeader.SetActive(true);
                dexIconColHeader.SetActive(false);
                wisIconColHeader.SetActive(false);
                chaIconColHeader.SetActive(false);
                break;
            case DescribableList.Dexterity:
                amountIconColHeader.SetActive(false);
                lvlIconColHeader.SetActive(false);
                strIconColHeader.SetActive(false);
                dexIconColHeader.SetActive(true);
                wisIconColHeader.SetActive(false);
                chaIconColHeader.SetActive(false);
                break;
            case DescribableList.Wisdom:
                amountIconColHeader.SetActive(false);
                lvlIconColHeader.SetActive(false);
                strIconColHeader.SetActive(false);
                dexIconColHeader.SetActive(false);
                wisIconColHeader.SetActive(true);
                chaIconColHeader.SetActive(false);
                break;
            case DescribableList.Charisma:
                amountIconColHeader.SetActive(false);
                lvlIconColHeader.SetActive(false);
                strIconColHeader.SetActive(false);
                dexIconColHeader.SetActive(false);
                wisIconColHeader.SetActive(false);
                chaIconColHeader.SetActive(true);
                break;
            default:
                amountIconColHeader.SetActive(false);
                lvlIconColHeader.SetActive(false);
                strIconColHeader.SetActive(false);
                dexIconColHeader.SetActive(false);
                wisIconColHeader.SetActive(false);
                chaIconColHeader.SetActive(false);
                break;
        }
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
        return KeyBindingList.characterScreenKey.getCurrentKeyCode();
    }

}
