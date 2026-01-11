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
        IEnumerable<IDescribable> equippedItems = OverallUIManager.getCurrentEquippedItems().createEquippedItemList();

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
        Dictionary<int, Attack> weaponsAndSlots = findAllWeaponActionSlots();

        int weaponSlotIndex = 0;
        for(int index = 0; index < OverallUIManager.getCurrentActionArray().getActions().Length; index++)
        {
            if(weaponsAndSlots.ContainsKey(index))
            {
                if(index >= CombatActionArray.numberOfActivatablePlayerCombatActions)
                {
                    slotIconList[Weapon.mainHandSlotIndex+weaponSlotIndex].setToFilledAndUsable();
                } else
                {
                    slotIconList[Weapon.mainHandSlotIndex+weaponSlotIndex].setToFilledAndUnusable();
                }

                weaponSlotIndex++;
            }
        }

        for (int index = Weapon.mainHandSlotIndex+weaponSlotIndex; index < slotIconList.Count; index++)
        {
            if(OverallUIManager.getCurrentActionArray().allActionSlotsFull())
            {
                slotIconList[index].setToUnavailableAndUnusable();
            } else
            {
                slotIconList[index].setToAvailableAndUsable();
            }
        }

        for (int index = Weapon.mainHandSlotIndex; index < slotIconList.Count; index++)
        {
            if(index >= Weapon.mainHandSlotIndex + OverallUIManager.getCurrentPartyMember().getWeaponSlots())
            {
                slotIconList[index].gameObject.SetActive(false);
            } else
            {
                slotIconList[index].gameObject.SetActive(true);
            }
        }
    }

    private static Dictionary<int, Attack> findAllWeaponActionSlots()
    {
        Dictionary<int, Attack> output = new Dictionary<int, Attack>();

        for (int index = 0; index < OverallUIManager.getCurrentActionArray().getActions().Length; index++)
        {
            if (OverallUIManager.getCurrentActionArray().getActions()[index] != null &&
                OverallUIManager.getCurrentActionArray().getActions()[index].getSourceItem() != null &&
                OverallUIManager.getCurrentActionArray().getActions()[index].getSourceItem().isEquippable())
            {
                output[index] = OverallUIManager.getCurrentActionArray().getActions()[index] as Attack;
            }
        }

        return output;
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
        listOfEvents.Add(ScreenManager.OnScreenInteriorUpdate);

        return listOfEvents;
    }
}
