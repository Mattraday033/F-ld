using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

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
    Glossary = 11,
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
    ShopKeeperEssentialItems = 25,
    CharacterSpecificAbilities = 26
}

[System.Serializable]
public struct Tab
{
    public readonly static UnityEvent<DescribableList> OnListRetrieved = new UnityEvent<DescribableList>();

    public static IEnumerable<IDescribable> getList(DescribableList describableList)
    {
		return getList(describableList, null);
    }

    public static IEnumerable<IDescribable> getList(DescribableList describableList, string[] filterParameters)
    {
        OnListRetrieved.Invoke(describableList);
        
        switch (describableList)
        {
            case DescribableList.Unnecessary:

                return new List<IDescribable>();

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

                return new List<IDescribable>(PartyManager.getAllJoinablePartyMembers());

            case DescribableList.Quests:

                return QuestList.getActiveQuests();

            case DescribableList.Glossary:

                return GlossaryCategoryList.getAllGlossaryCategories();

            case DescribableList.MainHandWeaponsAsActions:

                return Inventory.getAllMainHandWeaponsInPocketAsCombatActions(State.inventory);
            case DescribableList.Armor:

                return Inventory.getAllArmorInPocket(State.inventory);
            case DescribableList.Usable:
                
                return Inventory.getPocketForDisplayGenericUI(State.inventory, new string[]{UsableItem.type}, new NameComparer());
            case DescribableList.QuestItems:
                
                return Inventory.getPocketForDisplayGenericUI(State.inventory, new string[]{QuestItem.subtype, Key.subtype}, new NameComparer());
            case DescribableList.OffHandWeapons:
                
                return Inventory.getAllOffHandWeaponsInPocket(State.inventory);
            case DescribableList.PartyMembersWithPlayer:

                return new List<IDescribable>(PartyManager.getAllJoinablePartyMembers());
            case DescribableList.CombatUsableItems:
                
                return Inventory.getAllItemsUsableInCombat();
            case DescribableList.MainHandWeaponsAsItems:
                
                return Inventory.getAllMainHandWeaponsInPocket(State.inventory);

            case DescribableList.ShopKeeperMainHandWeapons:

                if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
                {
                    return Inventory.getAllMainHandWeaponsInPocket(ShopPopUpWindow.getCurrentShopkeeper().getInventory());
                }
                else
                {
                    return getList(DescribableList.MainHandWeaponsAsItems);
                }
            case DescribableList.ShopKeeperUseItems:

                if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
                {
                    return Inventory.getAllItemsOfTypeInPocket(ShopPopUpWindow.getCurrentShopkeeper().getInventory(), UsableItem.type);
                }
                else
                {
                    return getList(DescribableList.Inventory, new string[] { UsableItem.type });
                }
            case DescribableList.ShopKeeperOffHandWeapons:

                if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
                {
                    return Inventory.getAllOffHandWeaponsInPocket(ShopPopUpWindow.getCurrentShopkeeper().getInventory());
                }
                else
                {
                    return getList(DescribableList.OffHandWeapons);
                }
            case DescribableList.ShopKeeperArmor:

                if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
                {
                    return Inventory.getAllArmorInPocket(ShopPopUpWindow.getCurrentShopkeeper().getInventory());
                }
                else
                {
                    return getList(DescribableList.Armor);
                }
            case DescribableList.ShopKeeperEssentialItems:

                if (ShopPopUpWindow.currentShopMode == ShopMode.Buy)
                {
                    return Inventory.getPocketForDisplayGenericUI(ShopPopUpWindow.getCurrentShopkeeper().getInventory(), new string[]{EssentialItem.type}, new NameComparer());
                }
                else
                {
                    return new List<IDescribable>(); //can't sell essential items
                }
            case DescribableList.CharacterSpecificAbilities:

                if(OverallUIManager.getCurrentPartyMember() == null)
                {
                    return new List<IDescribable>();
                } else
                {
                    return AbilityList.getCompanionAbilities(OverallUIManager.getCurrentPartyMember().getName());
                }
            default:

                throw new IOException("Unknown DescribableList = " + describableList.ToString());
        }
    }
}

public abstract class ScreenManager : MonoBehaviour, ITabParent
{
    #region Events
    public readonly static UnityEvent<ScreenManager> OnScreenDeclaration = new UnityEvent<ScreenManager>();
    public readonly static UnityEvent OnScreenInteriorUpdate = new UnityEvent();
    #endregion

    private static AllyStats _CurrentPartyMember;
    public static AllyStats currentPartyMember
    {
        get
        {
            if (_CurrentPartyMember == null)
            {
                _CurrentPartyMember = PartyManager.getPlayerStats();
            }

            return _CurrentPartyMember;
        }
        set
        {
            _CurrentPartyMember = value;
        }
    }

    public virtual void Awake()
    {
        OverallUIManager.currentScreenManager = this; 
        OnScreenDeclaration.Invoke(this);
        addListeners();
    }

    protected virtual void Start()
    {        
        OnScreenInteriorUpdate.Invoke();
    }

    public virtual bool enableSpriteRowDragAndDrop()
    {
        return false;
    }

    public abstract bool requiresPartyMemberSelectionGrid();

    public abstract List<UnityEvent> getUpdateEvents();

    public abstract DescribableList getDefaultDescribableList();

    public abstract void updateCounter();

    public virtual void addListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.AddListener(updateCounter);
        }
    }
    public virtual void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
    }

    public abstract KeyCode getExitKeyCode();

}