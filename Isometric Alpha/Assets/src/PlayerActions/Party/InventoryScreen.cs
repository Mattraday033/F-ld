using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InventoryScreen : ScreenManager, ICounter
{
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
        // populateGrid(inventoryGridIndex);
        // statsDescriptionSlot.setPrimaryDescribable(getCurrentPartyMember());
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
}
