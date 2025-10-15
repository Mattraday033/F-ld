using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class Inventory
{

    public static void addItem(Item item)
    {
        addItem(item, State.inventory);
    }

    //Sprite square = Helpers.loadSpriteFromResources("Square"); 
    //Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Square.png
    public static void addItem(Item item, Dictionary<string, Item> pocket)
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

        Item oldItem;

        if (pocket.TryGetValue(item.getKey(), out oldItem))
        { // This section checks for an amount of the item already existing in the pocket. 
          //if it exists, it stores the amount of it, removes it from the State.inventory, then add's it in again along with new amount you're adding
          //might be slower than adjusting.

            pocket.Remove(item.getKey());

            oldItem.addQuantity(item.getQuantity());

            pocket.Add(item.getKey(), oldItem);

        }
        else
        {

            pocket.Add(item.getKey(), item);
        }

        EquippedItems.OnEquipmentChange.Invoke();
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

        Item oldItem;

        if (pocket.TryGetValue(key, out oldItem))
        {

            oldItem = oldItem.clone();

            pocket.Remove(key);

            EquippedItems.OnEquipmentChange.Invoke();

            return oldItem.clone();
        }
        else
        {
            return null;
        }
    }

    public static Item removeItem(Item item, int amount)
    {
        if (item == null)
        {
            return null;
        }
        if (item.isJunk())
        {
            return removeItem(item.getKey(), amount, State.junkPocket);
        }
        else
        {
            return removeItem(item.getKey(), amount, State.inventory);
        }
    }

    public static Item removeItem(string key, int amount)
    {
        if (key == null)
        {
            return null;
        }

        return removeItem(key, amount, State.inventory);
    }

    //if amount specified, remove that amount
    public static Item removeItem(Item item, int amount, Dictionary<string, Item> pocket)
    {
        if (item == null || pocket == null)
        {
            return null;
        }

        return removeItem(item.getKey(), amount, pocket);
    }

    //if amount specified, remove that amount
    public static Item removeItem(string key, int amount, Dictionary<string, Item> pocket)
    {
        if (key == null || pocket == null)
        {
            return null;
        }

        Item oldItem;

        if (pocket.TryGetValue(key, out oldItem))
        {
            if (oldItem.getQuantity() < amount)
            {
                throw new IOException("Quantity of " + key + " is already 0. Cannot remove anymore of this item");
            }

            oldItem = oldItem.clone();

            oldItem.setQuantity(amount);

            pocket[key].removeQuantity(amount);

            if (pocket[key].getQuantity() == 0)
            {
                pocket.Remove(key);
            }

            EquippedItems.OnEquipmentChange.Invoke();

            return oldItem;

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

        try
        {
            if (pocket[key].getQuantity() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        catch (KeyNotFoundException e)
        {
            return false;
        }
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

    public static ArrayList getPocketForDisplayGenericUI(Dictionary<string, Item> pocket, string[] filterParameters, IComparer comparer)
    {
        ArrayList output = new ArrayList();

        foreach (KeyValuePair<string, Item> kvp in pocket)
        {
            if (kvp.Value.withinFilter(filterParameters))
            {
                output.Add(kvp.Value);
            }
        }

        output.Sort(comparer);

        return output;
    }

    public static ArrayList getAllMainHandWeapons()
    {
        ArrayList allWeapons = getPocketForDisplayGenericUI(State.inventory, new string[] { Weapon.subtype }, new NameComparer());
        ArrayList allMainHandWeapons = new ArrayList();

        foreach (Weapon weapon in allWeapons)
        {
            if (weapon.getSlotID().Equals(Weapon.mainHandSlotIndex))
            {
                allMainHandWeapons.Add(weapon);
            }
        }

        allMainHandWeapons.Sort(new NameComparer());

        return allMainHandWeapons;
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

    public static ArrayList getAllItemsOfTypeInPocket(Dictionary<string, Item> pocket, string type)
    {
        ArrayList allItemsOfType = new ArrayList();

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

    public static ArrayList getAllItemsExcludingOfTypesInPocket(Dictionary<string, Item> pocket, string[] types)
    {
        ArrayList allItemsExceptOfTypes = new ArrayList();

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

    public static ArrayList getAllUsableItemCombatActionsInPocket(Dictionary<string, Item> pocket)
    {
        ArrayList allUsableItemCombatActions = new ArrayList();

        foreach (KeyValuePair<string, Item> kvp in pocket)
        {
            Item item = kvp.Value;

            if (item.getType().Equals(UsableItem.type) && item.usableInCombat())
            {
                allUsableItemCombatActions.Add(new ItemCombatAction((UsableItem)item));
            }
        }

        return allUsableItemCombatActions;
    }

    public static ArrayList convertPocketToArrayList(Dictionary<string, Item> pocket)
    {
        return new ArrayList(pocket.Select(x => x.Value).ToList());
    }

    public static ArrayList getAllItemsOfSubtypeInPocket(Dictionary<string, Item> pocket, string subtype)
    {
        ArrayList allItemsOfSubtype = new ArrayList();

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

    public static ArrayList getAllMainHandWeaponsInPocket(Dictionary<string, Item> pocket)
    {
        ArrayList allWeapons = getAllItemsOfSubtypeInPocket(pocket, Weapon.subtype);
        ArrayList allMainHandWeapons = new ArrayList();

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

    public static ArrayList getAllMainHandWeaponsInPocketAsCombatActions(Dictionary<string, Item> pocket)
    {
        ArrayList allWeapons = getAllItemsOfSubtypeInPocket(pocket, Weapon.subtype);
        ArrayList allMainHandWeaponsAsCombatActions = new ArrayList();

        allMainHandWeaponsAsCombatActions.Add(new FistAttack());

        foreach (Item item in allWeapons)
        {
            Weapon weapon = (Weapon)item;

            if (weapon.getSlotID() == Weapon.mainHandSlotIndex)
            {
                allMainHandWeaponsAsCombatActions.Add(new Attack(weapon));
            }
        }

        return allMainHandWeaponsAsCombatActions;
    }

    public static ArrayList getAllOffHandWeaponsInPocket(Dictionary<string, Item> pocket)
    {
        ArrayList allWeapons = getAllItemsOfSubtypeInPocket(pocket, Weapon.subtype);

        for (int index = allWeapons.Count - 1; index >= 0; index--)
        {
            EquippableItem item = (EquippableItem)allWeapons[index]; ;

            if (item.getSlotID() == Weapon.mainHandSlotIndex)
            {
                allWeapons.RemoveAt(index);
            }
        }

        return allWeapons;
    }

    public static ArrayList getAllItemsUsableInCombat()
    {
        ArrayList allUsableItems = getPocketForDisplayGenericUI(State.inventory, new string[] { UsableItem.type }, new NameComparer());

        ArrayList allCombatUsableItems = new ArrayList();

        foreach (UsableItem item in allUsableItems)
        {
            if (item.usableInCombat())
            {
                allCombatUsableItems.Add(new ItemCombatAction(item));
            }
        }

        return allCombatUsableItems;
    }
}
