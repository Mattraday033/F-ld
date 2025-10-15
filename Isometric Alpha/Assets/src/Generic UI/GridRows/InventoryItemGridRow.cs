using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemGridRow : GridRow, IPointerDownHandler, IDragAndDropSource
{


    public virtual string getDragAndDropPrefabName()
    {
        return PrefabNames.dragAndDropItemIcon;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Item item = descriptionPanel.getObjectBeingDescribed() as Item;

        StartCoroutine(DragAndDropManager.waitForMouseRelease(this, item));
    }
}
