using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public enum DescribableList
{
    Unnecessary = 0,
    Inventory = 1,
    Junk = 2,
    Equipment = 3,
    Strength = 4,
    Dexterity = 5,
    Wisdom = 6,
    Charisma = 7,
    Saves = 8,
    PartyMembers = 9,
    Quests = 10,
    GlossaryCategories = 11,
    PerkCategories = 12,
    MainHandWeaponsAsActions = 13,
    Armor = 14,
    Usable = 15,
    QuestItems = 16,
    OffHandWeapons = 17,
    PartyMembersWithPlayer = 18,
    CombatUsableItems = 19,
    MainHandWeaponsAsItems = 20,
    ShopKeeperMainHandWeapons = 21,
    ShopKeeperUseItems = 22,
    ShopKeeperOffHandWeapons = 23,
    ShopKeeperArmor = 24,
    ShopKeeperEssentialItems = 25
}

// [System.Serializable]
public struct TabCollection
{
	public bool statTabs;
	public Tab[] collection;
	
	public TabCollection(Tab[] collection)
	{
		this.collection = collection;
		this.statTabs = false;
	}
	
	public void setTabCollectionIndex(int index)
	{
		foreach(Tab tab in collection)
		{
			tab.setTabCollectionIndex(index);
		}
	}
	
	public void selectTab(int tabIndex)
	{
		foreach (Tab tab in collection)
		{
			tab.button.interactable = true;
		}
		
		collection[tabIndex].button.interactable = false;
	}
	
	public void selectAndClickTab(int tabIndex)
	{
		foreach (Tab tab in collection)
		{
			tab.button.interactable = true;
		}
		
		if(collection[tabIndex].button.enabled)
		{
			collection[tabIndex].button.onClick.Invoke();			
		}
		
		collection[tabIndex].button.interactable = false;
	}
	
	public void selectDefaultTab()
	{
		if(!statTabs)
		{
			selectTab(0);
		} else
		{
			selectTab((int) PartyManager.getPlayerStats().getHighestStat());
		}
	}

	public Tab getCurrentTab()
	{
		for (int tabIndex = 0; tabIndex < collection.Length; tabIndex++)
		{
			if (!collection[tabIndex].button.interactable && collection[tabIndex].button.enabled)
			{
				return collection[tabIndex];
			}
		}

		return collection[0];

		// throw new IOException("All tabs are interactable");
	}

	public int getCurrentTabIndex()
	{
		for (int tabIndex = 0; tabIndex < collection.Length; tabIndex++)
		{
			if (!collection[tabIndex].button.interactable)
			{
				return tabIndex;
			}
		}

		return 0;
	}
}

// [System.Serializable]
public struct Tab
{
	public Button button;
	public string[] filterParameters;
	public ScrollableUIElement grid;
	public DescribableList list;
	public int tabCollectionIndex;
	public bool usePartyMemberName;
	public string partyMemberName;
	
	public Tab(GridRow gridRow, int collectionIndex, string partyMemberName)
	{
		this.button = gridRow.buttons[0];
		
		this.grid = OverallUIManager.currentScreenManager.grids[collectionIndex];
		
		this.filterParameters = null;
		
		this.list = DescribableList.Unnecessary;
		
		this.tabCollectionIndex = collectionIndex;
		
		this.usePartyMemberName= true;
		this.partyMemberName = partyMemberName;

    }
	
	public Tab(Button button, ScrollableUIElement grid, string[] filterParameters, DescribableList list)
	{
		this.button = button;
		
		this.grid = grid;
		
		this.filterParameters = filterParameters;
		
		this.list = list;
		
		this.tabCollectionIndex = -1;
		this.usePartyMemberName = false;
		this.partyMemberName = "";
	}
	
	public bool isCurrentTab()
	{
		return !button.interactable;
	}
	
	public ArrayList getList()
	{
		if(usePartyMemberName)
		{
            return new ArrayList();
		}

		return getList(list, filterParameters);
	}

    public static ArrayList getList(DescribableList describableList)
    {
		return getList(describableList, null);
    }

    public static ArrayList getList(DescribableList describableList, string[] filterParameters)
    {
        switch (describableList)
        {
            case DescribableList.Unnecessary:

                return new ArrayList();

            case DescribableList.Inventory:

                return Inventory.getPocketForDisplayGenericUI(State.inventory, filterParameters, new NameComparer());

            case DescribableList.Junk:

                return Inventory.getPocketForDisplayGenericUI(State.junkPocket, filterParameters, new NameComparer());

            case DescribableList.Equipment:

                return OverallUIManager.getCurrentEquippedItems().createEquippedItemList();

            case DescribableList.Strength:

                return AbilityList.getAllStrengthAbilities();

            case DescribableList.Dexterity:

                return AbilityList.getAllDexterityAbilities();

            case DescribableList.Wisdom:

                return AbilityList.getAllWisdomAbilities();

            case DescribableList.Charisma:

                return AbilityList.getAllCharismaAbilities();

            case DescribableList.Saves:

                return SaveHandler.getSaveGameList();

            case DescribableList.PartyMembers:

                return new ArrayList(PartyManager.getAllJoinablePartyMembers());

            case DescribableList.Quests:

                return QuestList.getActiveQuests();

            case DescribableList.GlossaryCategories:

                return GlossaryCategoryList.getAllGlossaryCategories();

            case DescribableList.PerkCategories:

                return PerkCategoryList.getAllPerkCategories();

            case DescribableList.MainHandWeaponsAsActions:

                return Inventory.getAllMainHandWeaponsInPocketAsCombatActions(State.inventory);
            case DescribableList.Armor:

                return Inventory.getPocketForDisplayGenericUI(State.inventory, new string[]{Armor.subtype}, new NameComparer());
            case DescribableList.Usable:
                
                return Inventory.getPocketForDisplayGenericUI(State.inventory, new string[]{UsableItem.type}, new NameComparer());
            case DescribableList.QuestItems:
                
                return Inventory.getPocketForDisplayGenericUI(State.inventory, new string[]{QuestItem.subtype, Key.subtype}, new NameComparer());
            case DescribableList.OffHandWeapons:
                
                return Inventory.getAllOffHandWeaponsInPocket(State.inventory);
            case DescribableList.PartyMembersWithPlayer:

                return new ArrayList(PartyManager.getAllJoinablePartyMembers());
            case DescribableList.CombatUsableItems:
                
                return Inventory.getAllItemsUsableInCombat();
            case DescribableList.MainHandWeaponsAsItems:
                
                return Inventory.getAllMainHandWeaponsInPocket(State.inventory);

            case DescribableList.ShopKeeperMainHandWeapons:

                if (ShopPopUpWindow.getCurrentMode() == ShopMode.Buy)
                {
                    return Inventory.getAllMainHandWeaponsInPocket(ShopPopUpWindow.getCurrentShopkeeper().getInventory());
                }
                else
                {
                    return getList(DescribableList.MainHandWeaponsAsItems);
                }
            case DescribableList.ShopKeeperUseItems:

                if (ShopPopUpWindow.getCurrentMode() == ShopMode.Buy)
                {
                    return Inventory.getAllItemsOfTypeInPocket(ShopPopUpWindow.getCurrentShopkeeper().getInventory(), UsableItem.type);
                }
                else
                {
                    return getList(DescribableList.Inventory, new string[] { UsableItem.type });
                }
            case DescribableList.ShopKeeperOffHandWeapons:

                if (ShopPopUpWindow.getCurrentMode() == ShopMode.Buy)
                {
                    return Inventory.getAllOffHandWeaponsInPocket(ShopPopUpWindow.getCurrentShopkeeper().getInventory());
                }
                else
                {
                    return getList(DescribableList.OffHandWeapons);
                }
            case DescribableList.ShopKeeperArmor:

                if (ShopPopUpWindow.getCurrentMode() == ShopMode.Buy)
                {
                    return Inventory.getPocketForDisplayGenericUI(ShopPopUpWindow.getCurrentShopkeeper().getInventory(), new string[]{Armor.subtype}, new NameComparer());
                }
                else
                {
                    return getList(DescribableList.Armor);
                }
            case DescribableList.ShopKeeperEssentialItems:

                if (ShopPopUpWindow.getCurrentMode() == ShopMode.Buy)
                {
                    return Inventory.getPocketForDisplayGenericUI(ShopPopUpWindow.getCurrentShopkeeper().getInventory(), new string[]{EssentialItem.type}, new NameComparer());
                }
                else
                {
                    return new ArrayList(); //can't sell essential items
                }
            default:

                throw new IOException("Unknown DescribableList = " + describableList.ToString());
        }
    }

    public void setTabCollectionIndex(int index)
	{
		tabCollectionIndex = index;
	}
}

public class ScreenManager : MonoBehaviour
{
    private const int defaultAbilityGridIndex = 0;

    //allStatsPanels does not use currentTabCollection and can be in any order
    //[SerializeField]
    public List<InterfaceReference<IStatsPanel>> allStatsPanels;


    public bool alternateWayOfSettingDefaultState;

    public TabCollection[] tabCollections;

    public ScrollableUIElement[] grids;

    public int currentTabCollection = 0;

    public List<DescriptionPanelSlot> descriptionPanelSlots;

    //[SerializeField]
    private ScreenType screenType;

    public virtual void Awake()
    {
        OverallUIManager.currentScreenManager = this; 

        setAllTabCollectionsAndDescriptionSlotIndexes();

        updateAllStatsPanels();
    }

    public virtual ScreenType getScreenType()
    {
        return screenType;
    }

    public virtual bool hasGeneratedTabs()
    {
        return false;
    }

    public virtual void updateAllStatsPanels()
    {
        foreach (InterfaceReference<IStatsPanel> reference in allStatsPanels)
        {
            reference.Target.updateStatsPanel();
        }
    }

    public virtual void revealDescriptionPanelSet(IDescribable objectToDescribe)
    {
        if (descriptionPanelSlots.Count <= currentTabCollection)
        {
            return;
        }

        descriptionPanelSlots[currentTabCollection].setPrimaryDescribable(objectToDescribe);
    }

    public virtual void revealTemptDescriptionPanelSet(IDescribable objectToDescribe, int slotIndex)
    {
        if (descriptionPanelSlots.Count <= slotIndex)
        {
            return;
        }

        descriptionPanelSlots[slotIndex].setTempDescribable(objectToDescribe);
    }

    public virtual void hideTempDescriptionPanelSet(int slotIndex)
    {
        if (descriptionPanelSlots.Count <= slotIndex)
        {
            return;
        }

        descriptionPanelSlots[slotIndex].revertToPrimaryDescribable();
    }

    public virtual void populateObjectAttachedToSpriteRowButton(PartyMember partyMember)
    {
        //Empty on purpose
    }

    public virtual int getAbilityGridIndex()
    {
        return defaultAbilityGridIndex;
    }

    public virtual bool enableSpriteRowDragAndDrop()
    {
        return false;
    }

    public virtual void setGridRowType(int gridIndex, RowType rowType)
    {
        grids[gridIndex].setRowType(rowType);
    }

    public void populateGrid()
    {
        populateGrid(currentTabCollection);
    }

    public virtual void populateGrid(int tabCollectionIndex)
    {
        ScrollableUIElement currentScrollableUIElement = grids[tabCollectionIndex];

        if (currentScrollableUIElement == null || currentScrollableUIElement is null)
        {
            return;
        }

        string disabledRowName = currentScrollableUIElement.getDisabledRowName();

        currentScrollableUIElement.populatePanels(tabCollections[tabCollectionIndex].getCurrentTab().getList());

        if (disabledRowName != null)
        {
            currentScrollableUIElement.disableGridRowAndClick(disabledRowName);
        }
    }

    public virtual void populateAllGrids()
    {
        for (int tabCollectionIndex = 0; tabCollectionIndex < grids.Length; tabCollectionIndex++)
        {
            populateGrid(tabCollectionIndex);
        }
    }

    public void populateAllGridsEnableAllRows()
    {
        for (int tabCollectionIndex = 0; tabCollectionIndex < grids.Length; tabCollectionIndex++)
        {
            populateGrid(tabCollectionIndex);

            grids[tabCollectionIndex].enableAllGridRows();
        }
    }

    public int getCurrentTabCollection()
    {
        return currentTabCollection;
    }

    public void setCurrentTabCollection(int tabCollectionIndex)
    {
        this.currentTabCollection = tabCollectionIndex;
    }

    public virtual void setCurrentTab(int tabIndex)
    {
        tabCollections[currentTabCollection].selectTab(tabIndex);
    }

    private void setAllTabCollectionsAndDescriptionSlotIndexes()
    {
        int collectionIndex = 0;

        foreach (TabCollection collection in tabCollections)
        {
            collection.setTabCollectionIndex(collectionIndex);

            collectionIndex++;
        }

        collectionIndex = 0;

        foreach (DescriptionPanelSlot slot in descriptionPanelSlots)
        {
            slot.collectionTabIndex = collectionIndex;

            collectionIndex++;
        }
    }

    public virtual void setToScreenState(ScreenState screenState)
    {
        if (screenState == null)
        {
            // if (!alternateWayOfSettingDefaultState)
            // {
            setToDefaultScreenState();
            // }

            return;
        }
        else if (hasGeneratedTabs() && alternateWayOfSettingDefaultState)
        {
            //do nothing here
        }
        else if (hasGeneratedTabs())
        {
            setToDefaultScreenState();
        }

        for (int index = 0; index < tabCollections.Length; index++)
        {
            tabCollections[index].selectAndClickTab(screenState.currentTabIndexes[index]);
        }

        populateAllGrids();

        if (hasGeneratedTabs())
        {
            setPartyMemberTabGridToPreviousPartyMember();
        }

        for (int index = 0; index < grids.Length; index++)
        {
            if (grids[index] != null && !(grids[index] is null))
            {
                grids[index].disableGridRowAndClick(screenState.rowNames[index]);
            }
        }


    }

    public void setPartyMemberTabGridToPreviousPartyMember()
    {
        if (OverallUIManager.previousPartyMember == null)
        {
            return;
        }

        grids[0].disableGridRowAndClick(OverallUIManager.previousPartyMember.getName());
    }

    public virtual void setToDefaultScreenState()
    {
        int collectionIndex = 0;
        foreach (TabCollection collection in tabCollections)
        {
            collection.selectDefaultTab();

            populateGrid(collectionIndex);

            if (alternateWayOfSettingDefaultState)
            {
                break;
            }

            collectionIndex++;
        }

        if (hasGeneratedTabs() && !alternateWayOfSettingDefaultState)
        {
            tabCollections[1].selectAndClickTab(0);
        }
    }

    public void hideCurrentDescriptionPanel()
    {
        hideDescriptionPanel(currentTabCollection);
    }

    public void hideDescriptionPanel(int tabCollectionIndex)
    {
        descriptionPanelSlots[currentTabCollection].removePrimaryDescribable();
    }

    public void updateAllDecisionPanels()
    {
        foreach (DescriptionPanelSlot slot in descriptionPanelSlots)
        {
            slot.updateDecisionPanel();
        }
    }

    public virtual bool levelUpCapable()
    {
        return false;
    }

    public virtual float getNumberOfUpgradeTilesPerRow()
    {
        return 4f;
    }

    public virtual AllyStats getCurrentPartyMember()
    {
        return null;
    }
}