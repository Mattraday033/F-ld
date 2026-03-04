using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class InventoryScreen : ScreenManager, ICounter
{
    public TextMeshProUGUI playerNameText;

    public Image characterSprite;

    //ICounter methods
    private void OnEnable()
    {
        // updateCounter();
        addListeners();
    }

    private void OnDisable()
    {
        removeListeners();
    }

    private void OnDestroy()
    {
        removeListeners();
    }

    public override void updateCounter()
    {
        playerNameText.text = currentPartyMember.getName().Replace(PartyManager.playerMarker, "");
        characterSprite.sprite = PartyMember.getPortrait(currentPartyMember.getName());
        characterSprite.gameObject.SetActive(true);
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
        return DescribableList.MainHandWeaponsAsItems;
    }

    public override KeyCode getExitKeyCode()
    {
        return KeyBindingList.inventoryScreenKey;
    }
}
