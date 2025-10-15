using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class EquipmentDisplay : MonoBehaviour, ICounter
{
    public List<EquipmentDisplayEditorSlot> slotIconList;

    public void setIconList()
    {
        ArrayList equippedItems = OverallUIManager.getCurrentEquippedItems().createEquippedItemList();

        foreach (EquippableItem item in equippedItems)
        {
            if (item == null)
            {
                continue;
            }

            int index = item.getSlotID();

            if (index >= 0 && index < Weapon.mainHandSlotIndex)
            {
                addItemToSlot(item, index);
            }
            else if (index == Weapon.mainHandSlotIndex)
            {
                setWeaponSlot(item as Weapon);
            }
        }
    }

    public void resetIconList()
    {
        foreach (EquipmentDisplayEditorSlot slotIcon in slotIconList)
        {
            slotIcon.resetUI();
        }
    }

    public void setWeaponSlotEligibility()
    {
        int weaponSearchStartingIndex = -1;

        for (int index = Weapon.mainHandSlotIndex; index < Weapon.mainHandSlotIndex + OverallUIManager.getCurrentPartyMember().getWeaponSlots(); index++)
        {
            weaponSearchStartingIndex = findNextWeaponActionSlot(weaponSearchStartingIndex);

            if (weaponSearchStartingIndex >= 0 && weaponSearchStartingIndex < CombatActionArray.numberOfActivatablePlayerCombatActions)
            {
                slotIconList[index].setToFilledAndUsable(-1);
            }
            else if (weaponSearchStartingIndex >= CombatActionArray.numberOfActivatablePlayerCombatActions)
            {
                slotIconList[index].setToFilledAndUnusable(-1);
            }
            else if (weaponSearchStartingIndex < 0 && OverallUIManager.getCurrentPartyMember().getActionArray().everyAvailableCombatActionSlotFilled())
            {
                slotIconList[index].setToUnavailableAndUnusable();
            }
            else if (weaponSearchStartingIndex < 0)
            {
                slotIconList[index].setToAvailableAndUsable();
            }
        }

        for (int index = Weapon.mainHandSlotIndex + OverallUIManager.getCurrentPartyMember().getWeaponSlots(); index < slotIconList.Count; index++)
        {
            slotIconList[index].gameObject.SetActive(false);
        }
    }

    private int findNextWeaponActionSlot(int startingIndex)
    {
        for (int index = startingIndex + 1; index < OverallUIManager.getCurrentActionArray().getActions().Length; index++)
        {
            if (OverallUIManager.getCurrentActionArray().getActions()[index] != null &&
                OverallUIManager.getCurrentActionArray().getActions()[index].getSourceItem() != null &&
                OverallUIManager.getCurrentActionArray().getActions()[index].getSourceItem().isEquippable())
            {
                return index;
            }
        }

        return -1;
    }

    private void setWeaponSlot(Weapon mainHand)
    {
        for (int index = Weapon.mainHandSlotIndex; index < Weapon.mainHandSlotIndex + Wisdom.maxNumberOfWeaponSlots; index++)
        {
            if (!slotIsSet(index))
            {
                addItemToSlot(mainHand, index);
                return;
            }
        }
    }

    private void addItemToSlot(EquippableItem item, int index)
    {
        slotIconList[index].addItemToSlot(item);
    }

    private bool slotIsSet(int index)
    {
        return slotIconList[index].isFilled();
    }

    //ICounter Methods
    private void OnEnable()
    {
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

    public void addListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach (UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.AddListener(updateCounter);
        }
    }
    public void removeListeners()
    {
        List<UnityEvent> listOfEvents = getUpdateEvents();

        foreach(UnityEvent unityEvent in listOfEvents)
        {
            unityEvent.RemoveListener(updateCounter);
        }
    }

    public void updateCounter()
    {
        if (OverallUIManager.getCurrentPartyMember() == null)
        {
            return;
        }

        resetIconList();
        setWeaponSlotEligibility();
        setIconList();
    }

    public List<UnityEvent> getUpdateEvents()
    {
        List<UnityEvent> listOfEvents = new List<UnityEvent>();

        listOfEvents.Add(EquippedItems.OnEquipmentChange);
        listOfEvents.Add(CombatActionArray.OnCombatActionArrayChange);
        listOfEvents.Add(PartySpriteGridRow.OnPartyMemberSelected);

        return listOfEvents;
    }
}
