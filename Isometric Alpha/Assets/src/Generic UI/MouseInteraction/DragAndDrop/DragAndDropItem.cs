using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragAndDropItem : DragAndDropUIObject
{
    public override void handleTargetObject(Collider2D collision)
    {
        GameObject target = collision.gameObject;

        switch (target.tag)
        {
            case LayerAndTagManager.junkSlotTargetTag:
            case LayerAndTagManager.equipmentDisplayTag:
                handleEquipmentDrop(target);
                return;
            case LayerAndTagManager.itemUseTargetTag:
                handleUsableItemDrop(target);
                return;
        }
    }

    private void handleUsableItemDrop(GameObject target)
    {
        DescriptionPanel partyMemberGridRow = target.GetComponent<DescriptionPanel>();

        UsableItem item = getObjectBeingDragged() as UsableItem;

        if (item == null)
        {
            return;
        }

        Stats targetStats = Stats.convertIDescribableToStats(partyMemberGridRow.getObjectBeingDescribed());

        if (!item.fitsUseCriteria(targetStats))
        {
            return;
        }

        item.use(targetStats);

        if (!item.infiniteUses())
        {
            Inventory.removeItem(item, 1);
        }
    }

    private void handleEquipmentDrop(GameObject target)
    {
        EquipmentDisplayEditorSlot equipmentSlot = target.gameObject.GetComponent<EquipmentDisplayEditorSlot>();

        Item item = getObjectBeingDragged() as Item;

        if (equipmentSlot.sendToPocketSlot())
        {
            switch (equipmentSlot.slotType)
            {
                case DragDrogItemSlotType.Junk:
                    equipmentSlot.moveAllItemToJunk(item);
                    return;
                case DragDrogItemSlotType.Inventory:
                    equipmentSlot.moveAllItemOutOfJunk(item);
                    return;
                case DragDrogItemSlotType.Buy:
                    equipmentSlot.buyItem(item);
                    return;
                case DragDrogItemSlotType.Sell:
                    equipmentSlot.sellItem(item);
                    return;
            }


            return;
        }

        EquippableItem equippableItem = item as EquippableItem;

        if (equippableItem == null)
        {
            return;
        }

        if (equipmentSlot.slotIndex >= Weapon.mainHandSlotIndex &&
                equippableItem.getSlotID() == Weapon.mainHandSlotIndex)
        {
            equipmentSlot.unequipInCurrentSlot();
            OverallUIManager.getCurrentActionArray().equipCombatAction(equippableItem.getCombatAction());
        }
        else if (equippableItem.getSlotID() == equipmentSlot.slotIndex)
        {
            OverallUIManager.getCurrentEquippedItems().equipItem(equippableItem);
        }
    }

    public override string getTargetTag()
    {
        Item item = descriptionPanel.getObjectBeingDescribed() as Item;

        if (item.isEquippable())
        {
            return LayerAndTagManager.equipmentDisplayTag;
        }
        else
        {
            return LayerAndTagManager.itemUseTargetTag;
        }
    }

    public override bool handlesJunkSlot()
    {
        return true;
    }

}
