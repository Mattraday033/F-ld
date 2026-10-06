using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class InventoryScreen : ScreenManager, ICounter
{
    public TextMeshProUGUI playerNameText;    
    public TextMeshProUGUI partyGoldText;

    public Image characterSprite;

    //ICounter methods
    //the listeners are added once, by ScreenManager.Awake

    private void OnDestroy()
    {
        removeListeners();
    }

    public override void updateCounter()
    {
        playerNameText.text = currentPartyMember.displayName;
        characterSprite.sprite = PartyMember.getPortrait(currentPartyMember.uniqueName);
        characterSprite.gameObject.SetActive(true);
        partyGoldText.text = Purse.getCoinsInPurseForDisplay();
    }

    public override List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(Inventory.OnInventoryChange);
        listOfEvents.Add(EquippedItems.OnEquipmentChange);
        listOfEvents.Add(CombatActionArray.OnCombatActionArrayChange);
        listOfEvents.Add(PartySpriteGridRow.OnPartyMemberSelected);
        listOfEvents.Add(AbilityGridSideTab.OnSideTabChosen);
        listOfEvents.Add(OnScreenInteriorUpdate);
        
        return listOfEvents;
    }

    public override bool requiresPartyMemberSelectionGrid()
    {
        return true;
    }

    public override DescribableList getDefaultDescribableList()
    {
        return DescribableList.AllItems;
    }

    public override KeyCode getExitKeyCode()
    {
        return KeyBindingList.inventoryScreenKey.getCurrentKeyCode();
    }
}
