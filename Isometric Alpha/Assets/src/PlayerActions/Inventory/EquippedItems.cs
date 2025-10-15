using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class EquippedItems : IEnumerable
{
    public const int totalEquipmentSlots = 6;
    public static UnityEvent OnEquipmentChange = new UnityEvent();

    public Stats owner;
    public EquippableItem[] equippedItems = new EquippableItem[totalEquipmentSlots];

    public EquippedItems(Stats owner)
    {
        this.owner = owner;
    }

    public EquippedItems(Stats owner, EquippableItem[] equippedItems)
    {
        this.owner = owner;
        this.equippedItems = equippedItems;

        foreach (EquippableItem equippedItem in equippedItems)
        {
            if (equippedItem != null)
            {
                equippedItem.equipTarget = owner;
            }
        }
    }

    public EquippableItem getItemInSlot(int index)
    {
        return equippedItems[index];
    }

    public EquippableItem getOffHand()
    {
        if (equippedItems[Weapon.offHandSlotIndex] != null &&
            !(equippedItems[Weapon.offHandSlotIndex] is null))
        {
            return equippedItems[Weapon.offHandSlotIndex];
        }

        return ItemList.getOffHandFist();
    }

    public void equipItem(EquippableItem item)
    {

        Dictionary<string, Item> currentPocket;

        if (item.isJunk())
        {
            currentPocket = State.junkPocket;
        }
        else
        {
            currentPocket = State.inventory;
        }

        if (equippedItems[item.getSlotID()] != null && equippedItems[item.getSlotID()].isJunk())
        {
            Inventory.addItem(equippedItems[item.getSlotID()], State.junkPocket);
        }
        else if (equippedItems[item.getSlotID()] != null)
        {
            Inventory.addItem(equippedItems[item.getSlotID()], State.inventory);
        }

        EquippableItem itemToEquip = (EquippableItem) Inventory.removeItem(item, 1, currentPocket);

        itemToEquip.equipTarget = owner;

        equippedItems[item.getSlotID()] = itemToEquip;

        OnEquipmentChange.Invoke();
    }

    public void unequipItem(int index)
    {
        if (index >= 0 && index < equippedItems.Length)
        {
            EquippableItem item = equippedItems[index];

            if (item != null)
            {
                unequipItem(item);
            }
        }
    }

    public void unequipItem(EquippableItem item)
    {
        if (!item.isUnequippable())
        {
            return;
        }
        
        if (item.getSlotID() == Weapon.mainHandSlotIndex ||
                item.getSlotID() < Weapon.offHandSlotIndex)
            {
                Debug.LogError("item = " + item.getName());
                Helpers.debugNullCheck("owner", owner);
                Helpers.debugNullCheck("owner.getActionArray()", owner.getActionArray());

                owner.getActionArray().unequipCombatAction(item.getKey());
            }
            else
            {
                Dictionary<string, Item> currentPocket;

                if (item.isJunk())
                {
                    currentPocket = State.junkPocket;
                }
                else
                {
                    currentPocket = State.inventory;
                }

                if (equippedItems[item.getSlotID()] != null)
                {
                    Inventory.addItem(equippedItems[item.getSlotID()], currentPocket);
                }


                equippedItems[item.getSlotID()].equipTarget = null;
                equippedItems[item.getSlotID()] = null;
            }

        OnEquipmentChange.Invoke();
    }

    public ArrayList createEquippedItemList()
    {
        ArrayList equippedItems = new ArrayList();
        ArrayList equippedMainHandWeapons = new ArrayList();
        ArrayList equippedArmorPlusOffHand = new ArrayList();

        foreach (CombatAction action in owner.getActionArray().getActions())
        {
            if (action != null && action.getSourceItem() != null && action.getSourceItem().isEquippable())
            {
                equippedMainHandWeapons.Add(action.getSourceItem());
            }
        }

        while (equippedMainHandWeapons.Count < Wisdom.maxNumberOfWeaponSlots)
        {
            equippedMainHandWeapons.Add(null);
        }

        foreach (EquippableItem item in equippedItems)
        {
            equippedArmorPlusOffHand.Add(item);
        }

        equippedArmorPlusOffHand.Insert(0, getOffHand());

        equippedItems.AddRange(equippedMainHandWeapons);
        equippedItems.AddRange(equippedArmorPlusOffHand);

        return equippedItems;
    }

    //IEnumerable methods

    public IEnumerator GetEnumerator()
    {
        return equippedItems.GetEnumerator();
    }

    /*
        private static bool isDualWielding()
        {
            if(equippedItems[Weapon.offHandSlotIndex] == null || 
                !equippedItems[Weapon.offHandSlotIndex].getSubtype().Equals(Weapon.subtype))
            {
                return false;
            } else
            {
                return true;
            }
        }
    */
}
