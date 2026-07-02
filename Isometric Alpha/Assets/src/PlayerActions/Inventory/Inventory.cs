using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class Inventory
{

    public readonly static UnityEvent OnInventoryChange = new UnityEvent();

    public static void addItem(Item item, bool ignoreEvent = false)
    {
        addItem(item, State.inventory, ignoreEvent);
    }

    //Sprite square = Helpers.loadSpriteFromResources("Square"); 
    //Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Square.png
    public static void addItem(Item item, Dictionary<string, Item> pocket, bool ignoreEvent = false)
    {

        if (item == null || pocket == null || !ItemList.addableToInventory(item))
        {
            return;
        }

        if (pocket == State.junkPocket || pocket == State.inventory)
        {
            //treasure items always go to the junk pocket and should not be able to be added to any other pocket
            if (item.mustBeJunk() || item.isJunk())
            {
                pocket = State.junkPocket;
            }
            else if (!item.canBeJunk())
            {
                pocket = State.inventory;
            }
        }

        if (pocket.ContainsKey(item.getKey()))
        { 
            pocket[item.getKey()].addQuantity(item.getQuantity());
        }
        else
        {
            pocket.Add(item.getKey(), item);
        }

        if(!ignoreEvent)
        {
            OnInventoryChange.Invoke();
        }
    }


    public static Item removeItem(Item item)
    {
        if (item == null || !ItemList.addableToInventory(item))
        {
            return null;
        }

        if (item.isJunk())
        {
            return removeItem(item.getKey(), State.junkPocket);
        }
        else
        {
            return removeItem(item.getKey(), State.inventory);
        }
    }

    public static Item removeItem(string key)
    {
        if (key == null)
        {
            return null;
        }

        return removeItem(key, State.inventory);
    }

    //if no quantity specified, remove all of that item
    public static Item removeItem(Item item, Dictionary<string, Item> pocket)
    {
        if (item == null || pocket == null)
        {
            return null;
        }

        return removeItem(item.getKey(), pocket);
    }

    //if no quantity specified, remove all of that item
    public static Item removeItem(string key, Dictionary<string, Item> pocket)
    {
        if (key == null || pocket == null)
        {
            return null;
        }

        if(!pocket.ContainsKey(key))
        {
            return null;
        }

        Item oldItem = pocket[key];

        oldItem = oldItem.clone();

        pocket.Remove(key);

        OnInventoryChange.Invoke();

        return oldItem.clone();
    }

    public static Item removeItem(Item item, int amount, bool ignoreEvent = false)
    {
        if (item == null)
        {
            return null;
        }
        if (item.isJunk())
        {
            return removeItem(item.getKey(), amount, State.junkPocket, ignoreEvent);
        }
        else
        {
            return removeItem(item.getKey(), amount, State.inventory, ignoreEvent);
        }
    }

    public static Item removeItem(string key, int amount, bool ignoreEvent = false)
    {
        if (key == null)
        {
            return null;
        }

        return removeItem(key, amount, State.inventory, ignoreEvent);
    }

    //if amount specified, remove that amount
    public static Item removeItem(Item item, int amount, Dictionary<string, Item> pocket, bool ignoreEvent = false)
    {
        if (item == null || pocket == null)
        {
            return null;
        }

        return removeItem(item.getKey(), amount, pocket, ignoreEvent);
    }

    //if amount specified, remove that amount
    public static Item removeItem(string key, int amount, Dictionary<string, Item> pocket, bool ignoreEvent = false)
    {
        if (key == null || pocket == null)
        {
            return null;
        }

        if (pocket.ContainsKey(key))
        {
            Item removedItems = pocket[key].clone();

            removedItems.setQuantity(amount);

            pocket[key].removeQuantity(amount);

            if (pocket[key].getQuantity() <= 0)
            {
                pocket.Remove(key);
            }

            if(!ignoreEvent)
            {
                OnInventoryChange.Invoke();
            }

            return removedItems;
        }
        else
        {
            return null;
        }

    }

    public static bool equipmentContainsItem(string key)
    {
        List<PartyMember> partyMembers = PartyManager.getAllPartyMembers();

        foreach (PartyMember partyMember in partyMembers)
        {
            foreach (EquippableItem equippableItem in partyMember.stats.getEquippedItems())
            {
                if (equippableItem != null && equippableItem.getKey().Equals(key))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool inventoryContainsItem(string key)
    {
        return pocketContainsItem(key, State.inventory);
    }

    public static bool junkContainsItem(string key)
    {
        return pocketContainsItem(key, State.junkPocket);
    }

    public static bool pocketContainsItem(string key, Dictionary<string, Item> pocket)
    {
        if (key == null)
        {
            return false;
        }

        return pocket.ContainsKey(key);
    }

    public static bool inventoryContainsItem(string subtype, int ID)
    {
        return pocketContainsItem(subtype, ID, State.inventory);
    }

    public static bool pocketContainsItem(string subtype, int ID, Dictionary<string, Item> pocket)
    {
        if (!subtype.Equals(QuestItem.subtype) && !subtype.Equals(Key.subtype))
        {
            Debug.LogError("this version of inventoryContainsItem() should be used for key/quest items only. Current subtype: " + subtype);
        }

        foreach (KeyValuePair<string, Item> kvp in pocket)
        {
            Item item = kvp.Value;

            if (String.Equals(item.getSubtype(), subtype, StringComparison.OrdinalIgnoreCase))
            {
                if (item.getSubtype().Equals(QuestItem.subtype))
                {
                    if (((QuestItem)item).getQuestID() == ID)
                    {
                        return true;
                    }
                }
                else if (item.getSubtype().Equals(Key.subtype))
                {
                    if (((Key)item).getID() == ID)
                    {
                        return true;
                    }
                }

            }
        }

        return false;
    }

    public static Item getItem(Item item)
    {
        if (item.isJunk())
        {
            return getItem(item.getKey(), State.junkPocket);
        }
        else
        {
            return getItem(item.getKey(), State.inventory);
        }

    }

    public static Item getItem(string key)
    {
        if (pocketContainsItem(key, State.inventory))
        {
            return getItem(key, State.inventory);
        }
        else
        {
            return getItem(key, State.junkPocket);
        }
    }

    public static Item getItem(string key, Dictionary<string, Item> pocket)
    {
        return (Item)pocket[key].Clone();
    }

    public static Dictionary<string, Item> getAllItemsOfCurrentType(string type)
    {
        return getAllItemsOfCurrentType(type, State.inventory);
    }

    public static Dictionary<string, Item> getAllItemsOfCurrentType(string type, Dictionary<string, Item> pocket)
    {
        Dictionary<string, Item> smallerPocket = new Dictionary<string, Item>();

        foreach (KeyValuePair<string, Item> kvp in pocket)
        {

            if (String.Equals(kvp.Value.getType(), type, StringComparison.OrdinalIgnoreCase))
            {
                smallerPocket.Add(kvp.Key, kvp.Value);
            }
        }

        return smallerPocket;
    }

    public static List<IDescribable> getPocketForDisplayGenericUI(Dictionary<string, Item> pocket, string[] filterParameters, IComparer<ISortable> comparer)
    {
        List<Item> output = new List<Item>();

        foreach (KeyValuePair<string, Item> kvp in pocket)
        {
            if (kvp.Value.withinFilter(filterParameters))
            {
                output.Add(kvp.Value);
            }
        }

        output.Sort(comparer);

        return output.Cast<IDescribable>().ToList();
    }

    public static Dictionary<string, Item> getPocketForDisplay(Dictionary<string, Item> pocket, string[] subtypes)
    {
        Dictionary<string, Item>[] pockets = new Dictionary<string, Item>[subtypes.Length];

        for (int pocketIndex = 0; pocketIndex < pockets.Length; pocketIndex++)
        {
            pockets[pocketIndex] = new Dictionary<string, Item>();
        }

        foreach (KeyValuePair<string, Item> kvp in pocket)
        {

            for (int subtypeIndex = 0; subtypeIndex < subtypes.Length; subtypeIndex++)
            {

                if (String.Equals(subtypes[subtypeIndex], kvp.Value.getSubtype(), StringComparison.OrdinalIgnoreCase))
                {
                    pockets[subtypeIndex].Add(kvp.Key, kvp.Value);
                    break;
                }
            }
        }

        //if you want to sort each individual pocket by some metric: alphabetical, worth, etc.
        // do it here		
        for (int pocketIndex = 1; pocketIndex < pockets.Length; pocketIndex++)
        {
            foreach (KeyValuePair<string, Item> kvp in pockets[pocketIndex])
            {
                pockets[0].Add(kvp.Key, kvp.Value);
            }
        }


        return pockets[0];
    }

    public static List<Item> getAllItemsOfTypeInPocket(Dictionary<string, Item> pocket, string type)
    {
        List<Item> allItemsOfType = new List<Item>();

        foreach (KeyValuePair<string, Item> kvp in pocket)
        {
            Item item = kvp.Value;

            if (String.Equals(item.getType(), type, StringComparison.OrdinalIgnoreCase))
            {
                allItemsOfType.Add(item);
            }
        }

        return allItemsOfType;
    }

    public static List<Item> getAllItemsExcludingOfTypesInPocket(Dictionary<string, Item> pocket, string[] types)
    {
        List<Item> allItemsExceptOfTypes = new List<Item>();

        foreach (KeyValuePair<string, Item> kvp in pocket)
        {
            Item item = kvp.Value;

            if (!types.Contains(item.getType()))
            {
                allItemsExceptOfTypes.Add(item);
            }
        }

        return allItemsExceptOfTypes;
    }

    public static List<CombatAction> getAllUsableItemCombatActionsInPocket(Dictionary<string, Item> pocket)
    {
        List<CombatAction> allUsableItemCombatActions = new List<CombatAction>();

        foreach (KeyValuePair<string, Item> kvp in pocket)
        {
            Item item = kvp.Value;

            if (item.getType().Equals(UsableItem.type) && item.usableInCombat())
            {
                allUsableItemCombatActions.Add(new ItemCombatAction(OverallUIManager.getCurrentPartyMember(), (UsableItem)item));
            }
        }

        return allUsableItemCombatActions;
    }

    public static List<Item> convertPocketToList<Item>(Dictionary<string, Item> pocket)
    {
        return new List<Item>(pocket.Select(x => x.Value).ToList());
    }

    public static List<Item> getAllItemsOfSubtypeInPocket(Dictionary<string, Item> pocket, string subtype)
    {
        List<Item> allItemsOfSubtype = new List<Item>();

        foreach (KeyValuePair<string, Item> kvp in pocket)
        {
            Item item = kvp.Value;

            if (String.Equals(item.getSubtype(), subtype, StringComparison.OrdinalIgnoreCase))
            {
                allItemsOfSubtype.Add(item);
            }
        }

        return allItemsOfSubtype;
    }

    public static List<Item> getAllMainHandWeaponsInPocket(Dictionary<string, Item> pocket)
    {
        List<Item> allWeapons = getAllItemsOfSubtypeInPocket(pocket, Weapon.subtype);
        List<Item> allMainHandWeapons = new List<Item>();

        foreach (Item item in allWeapons)
        {
            Weapon weapon = (Weapon)item;

            if (weapon.getSlotID() == Weapon.mainHandSlotIndex)
            {
                allMainHandWeapons.Add(weapon);
            }
        }

        return allMainHandWeapons;
    }

    public static List<CombatAction> getAllMainHandWeaponsInPocketAsCombatActions(Dictionary<string, Item> pocket)
    {
        List<Item> allWeapons = getAllItemsOfSubtypeInPocket(pocket, Weapon.subtype);
        List<CombatAction> allMainHandWeaponsAsCombatActions = new List<CombatAction>();

        allMainHandWeaponsAsCombatActions.Add(new FistAttack(OverallUIManager.getCurrentPartyMember()));

        foreach (Item item in allWeapons)
        {
            Weapon weapon = (Weapon)item;

            if (weapon.getSlotID() == Weapon.mainHandSlotIndex)
            {
                allMainHandWeaponsAsCombatActions.Add(new Attack(OverallUIManager.getCurrentPartyMember(), weapon));
            }
        }

        return allMainHandWeaponsAsCombatActions;
    }

    public static List<Item> getAllOffHandWeaponsInPocket(Dictionary<string, Item> pocket)
    {
        List<Item> allArmor = getAllItemsOfSubtypeInPocket(pocket, Armor.subtype);

        for (int index = allArmor.Count - 1; index >= 0; index--)
        {
            if (allArmor[index] as OffHandWeapon == null || allArmor[index] as Shield != null)
            {
                allArmor.RemoveAt(index);
            }
        }

        return allArmor;
    }

    public static List<Item> getAllArmorInPocket(Dictionary<string, Item> pocket)
    {
        List<IDescribable> allArmor = getPocketForDisplayGenericUI(pocket, new string[]{Armor.subtype}, new NameComparer());

        for (int index = allArmor.Count - 1; index >= 0; index--)
        {
            if (allArmor[index] as OffHandWeapon != null && allArmor[index] as Shield == null)
            {
                allArmor.RemoveAt(index);
            }
        }

        return allArmor.Cast<Item>().ToList();
    }

    public static List<CombatAction> getAllItemsUsableInCombat()
    {
        List<Item> allUsableItems = getAllItemsOfTypeInPocket(State.inventory, UsableItem.type );

        List<CombatAction> allCombatUsableItems = new List<CombatAction>();

        foreach (UsableItem item in allUsableItems)
        {
            if (item.usableInCombat())
            {
                allCombatUsableItems.Add(new ItemCombatAction(OverallUIManager.getCurrentPartyMember(), item));
            }
        }

        allCombatUsableItems.Sort(new NameComparer());

        return allCombatUsableItems;
    }
}
