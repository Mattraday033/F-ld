using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

public class EquippedItems : StatBoostSourceCombiner, ICloneable
{
    public const int totalEquipmentSlots = 6;
    public readonly static UnityEvent OnEquipmentChange = new UnityEvent();

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

        checkForEmptyOffHandSlot();

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

    public void checkForEmptyOffHandSlot()
    {
        if(equippedItems[Weapon.offHandSlotIndex] == null)
        {
            equippedItems[Weapon.offHandSlotIndex] = ItemList.getOffHandFist();
        }

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

        checkForEmptyOffHandSlot();

        AllyStats equipmentOwner = getStatSource() as AllyStats;

        if(equipmentOwner != null)
        {
            equipmentOwner.checkStatsAfterEquipmentRemoval();
        }

        OnEquipmentChange.Invoke();
    }

    public IEnumerable<IDescribable> createEquippedItemList()
    {
        List<EquippableItem> allEquippedItems = new List<EquippableItem>();
        List<EquippableItem> equippedMainHandWeapons = new List<EquippableItem>();
        List<EquippableItem> equippedArmorPlusOffHand = new List<EquippableItem>();

        foreach (CombatAction action in owner.getActionArray().getActions())
        {
            if (action != null && action.getSourceItem() != null && action.getSourceItem().isEquippable())
            {
                equippedMainHandWeapons.Add(action.getSourceItem() as EquippableItem);
            }
        }

        while (equippedMainHandWeapons.Count < Wisdom.maxNumberOfWeaponSlots)
        {
            equippedMainHandWeapons.Add(null);
        }

        equippedArmorPlusOffHand.Add(getOffHand());

        foreach (EquippableItem item in equippedItems)
        {
            equippedArmorPlusOffHand.Add(item);
        }

        allEquippedItems.AddRange(equippedMainHandWeapons);
        allEquippedItems.AddRange(equippedArmorPlusOffHand);

        return allEquippedItems;
    }

    //IEnumerable methods

    public override IEnumerator GetEnumerator()
    {
        return equippedItems.GetEnumerator();
    }

    #region ICloneable

    public object Clone()
    {
        return MemberwiseClone();
    }

    public EquippedItems clone()
    {
        EquippedItems clonedObject = Clone() as EquippedItems;
        
        clonedObject.equippedItems = equippedItems;
        clonedObject.owner = owner;

        if(clonedObject.equippedItems[Weapon.offHandSlotIndex] == null)
        {
            clonedObject.equippedItems[Weapon.offHandSlotIndex] = ItemList.getOffHandFist();
        }

        return clonedObject;
    }

    #endregion

    public override Stats getStatSource()
    {
        return owner;
    }

    #region IDescribable (Unimplemented)

    public override string getName()
    {
        return owner.getName() + "'s Equipped Items";
    }

	public bool ineligible()
    {
        return false;
    }

	public GameObject getRowType(RowType rowType)
    {
        return null;
    }
	public GameObject getDescriptionPanelFull()
    {
        return null;
    }
	public GameObject getDescriptionPanelFull(PanelType type)
    {
        return null;
    }
    public GameObject getDecisionPanel()
    {
        return null;
    }

	public bool withinFilter(string[] filterParameters)
    {
        return false;
    }

	public void describeSelfFull(DescriptionPanel panel)
    {
        
    }

	public void describeSelfRow(DescriptionPanel panel)
    {
        
    }

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {
        
    }

	public List<IDescribable> getRelatedDescribables()
    {
        return new List<IDescribable>();
    }

	public bool buildableWithBlocks()
    {
        return false;
    }
	public bool buildableWithBlocksRows()
    {
        return false;
    }

    #endregion

}
