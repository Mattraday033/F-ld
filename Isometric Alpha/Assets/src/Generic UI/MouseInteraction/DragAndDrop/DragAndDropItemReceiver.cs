using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragAndDropItemReceiver : MonoBehaviour
{
    public DescriptionPanel panel;

    public bool handleItemDrop(UsableItem item)
    {
        if(item == null)
        {
            return false;
        }

        Stats itemTarget = getItemTarget();

        if (!item.fitsUseCriteria(itemTarget))
        {
            return false;
        }

        item.use(itemTarget);

        if (!item.infiniteUses())
        {
            Inventory.removeItem(item, 1);
        }

        return true;
    }

    private Stats getItemTarget()
    {
        if(panel == null)
        {
            return ScreenManager.currentPartyMember;
        } 

        Stats itemTarget = Stats.convertIDescribableToStats(panel.getObjectBeingDescribed());

        if(itemTarget == null)
        {
            return ScreenManager.currentPartyMember;
        } else
        {
            return itemTarget;
        }

    }

}
