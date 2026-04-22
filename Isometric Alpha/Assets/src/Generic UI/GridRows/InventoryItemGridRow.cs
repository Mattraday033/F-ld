using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemGridRow : GridRow, IPointerDownHandler, IDragAndDropSource
{

    public Button inventoryGridRowButton;

    public virtual string getDragAndDropPrefabName()
    {
        return PrefabNames.dragAndDropItemIcon;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Item item = descriptionPanel.getObjectBeingDescribed() as Item;

        StartCoroutine(DragAndDropManager.waitForMouseRelease(this, item));
    }

    public void OnMouseEnter()
    {
        OnPointerEnter(null);
    }

    public void OnMouseExit()
    {
        OnPointerExit(null);
    }

    public void onButtonPress()
    {
        Item item = descriptionPanel.getItemBeingDescribed();

        if(item == null)
        {
            return;
        }

        if(inventoryGridRowButton != null && 
            EventSystem.current != null) 
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if(item is Armor)
        {
            ScreenManager.currentPartyMember.equippedItems.equipItem(item as EquippableItem);
        } else if(item is Weapon)
        {
            Attack attack = new Attack(ScreenManager.currentPartyMember, item as Weapon);
            CombatActionArray actionArray = OverallUIManager.getCurrentActionArray();

            if(attack.hasAvailableSlots(OverallUIManager.getCurrentActionArray()))
            {
                actionArray.equipCombatAction(attack);
                return;
            } 

            int firstAttackSlotIndex = actionArray.findFirstAttackSlot();

            if(firstAttackSlotIndex >= 0)
            {
                actionArray.equipCombatAction(firstAttackSlotIndex, attack);
                return;
            }
        } else if(item is UsableItem &&
                    item.usableOutOfCombat() && 
                    item.fitsUseCriteria(ScreenManager.currentPartyMember))
        {
            (item as UsableItem).use(ScreenManager.currentPartyMember);
        }
    }
}
