using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InventoryScreen : ScreenManager, ICounter
{
    private const int partyMemberGridIndex = 0;
    private const int inventoryGridIndex = 1;
    private const int sideTabCollectionIndex = 1;


    public TextMeshProUGUI nameText;

    public DescriptionPanelSlot statsDescriptionSlot;

    public override void populateAllGrids()
    {
        // Debug.LogError("populating All Grids");
        base.populateAllGrids();
    }

    public override void populateObjectAttachedToSpriteRowButton(PartyMember partyMember)
    {

    }

    public override ScreenType getScreenType()
    {
        return ScreenType.Inventory;
    }

    public override int getAbilityGridIndex()
    {
        return inventoryGridIndex;
    }

    // public override void setUpTabs()
    // {
    //     TabCollection inventorySideTabs = tabCollections[sideTabCollectionIndex];

    //     base.setUpTabs();

    //     tabCollections = Helpers.appendArray<TabCollection>(tabCollections, inventorySideTabs);
    // }

    public override void setToDefaultScreenState()
    {
        base.setToDefaultScreenState();

        tabCollections[inventoryGridIndex].selectAndClickTab(0);
    }

    public override void setToScreenState(ScreenState screenState)
    {
        if (screenState == null)
        {
            setToDefaultScreenState();
        }
        else
        {
            base.setToDefaultScreenState();

            setPartyMemberTabGridToPreviousPartyMember();
            tabCollections[inventoryGridIndex].selectAndClickTab(screenState.currentTabIndexes[1]);
        }
    }

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
