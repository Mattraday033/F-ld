using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragAndDropItem : DragAndDropUIObject
{
    public override bool handleTargetObject(Collider2D collision, string tag)
    {
        GameObject target = collision.gameObject;

        switch (tag)
        {
            case LayerAndTagManager.junkSlotTargetTag:
            case LayerAndTagManager.equipmentDisplayTag:
                return handleEquipmentDrop(target);
            case LayerAndTagManager.itemUseTargetTag:
                return handleUsableItemDrop(target);
            default:
                return false;
        }
    }

    private bool handleEquipmentDrop(GameObject target)
    {
        EquipmentDisplayEditorSlot equipmentSlot = target.gameObject.GetComponent<EquipmentDisplayEditorSlot>();

        Item item = getObjectBeingDragged() as Item;

        if (equipmentSlot.sendToPocketSlot())
        {
            switch (equipmentSlot.slotType)
            {
                case DragDrogItemSlotType.Junk:
                    equipmentSlot.moveAllItemToJunk(item);
                    return true;
                case DragDrogItemSlotType.Inventory:
                    equipmentSlot.moveAllItemOutOfJunk(item);
                    return true;
                case DragDrogItemSlotType.Buy:
                    equipmentSlot.buyItem(item);
                    return true;
                case DragDrogItemSlotType.Sell:
                    equipmentSlot.sellItem(item);
                    return true;
                default:
                    return false;
            }
        }

        EquippableItem equippableItem = item as EquippableItem;

        if (equippableItem == null)
        {
            return false;
        }

        if (equipmentSlot.slotIndex >= Weapon.mainHandSlotIndex &&
                equippableItem.getSlotID() == Weapon.mainHandSlotIndex)
        {
            equipmentSlot.unequipInCurrentSlot();
            OverallUIManager.getCurrentActionArray().equipCombatAction(equippableItem.getCombatAction(OverallUIManager.getCurrentPartyMember()));
            return true;
        }
        else if (equippableItem.getSlotID() == equipmentSlot.slotIndex)
        {
            OverallUIManager.getCurrentEquippedItems().equipItem(equippableItem);
            return true;
        }

        return false;
    }

    public override string[] getTargetTags()
    {
        Item item = descriptionPanel.getObjectBeingDescribed() as Item;

        if (item.isEquippable())
        {
            return new string[] { LayerAndTagManager.equipmentDisplayTag };
        }
        else
        {
            return new string[] { LayerAndTagManager.itemUseTargetTag };
        }
    }

    public override bool handlesJunkSlot()
    {
        return true;
    }

}
