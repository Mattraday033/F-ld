using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemGridRow : InventoryItemGridRow
{
    public AmountPanel amountPanel;
    public Image interiorImage;

    public override string getDragAndDropPrefabName()
    {
        return PrefabNames.dragAndDropItemShopIcon;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (DragAndDropReenterZone.insideGoodZone)
        {
            base.OnPointerEnter(eventData);
        }
    }

    // public override void OnPointerExit(PointerEventData eventData)
    // {
    // 	if (!canSeeHover())
    // 	{
    // 		return;
    // 	}

    //     if (MouseHoverManager.hoverCoroutine != null)
    //     {
    //         StopCoroutine(MouseHoverManager.hoverCoroutine);
    //     }

    //     MouseHoverManager.hoverCoroutine = MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldDestroyHoverIcon);
    //     StartCoroutine(MouseHoverManager.hoverCoroutine);
    // }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (amountPanel.getMax() <= 0)
        {
            return;
        }

        Item item = descriptionPanel.getObjectBeingDescribed() as Item;

        if (item != null)
        {
            ItemListID listID = item.getItemListID();
            item = ItemList.getItem(listID.listIndex, listID.itemIndex, amountPanel.getAmount());

            StartCoroutine(DragAndDropManager.waitForMouseRelease(this, item));
        }
    }

    public void setToUnbuyableDisplay()
    {
        interiorImage.color = new Color32(125, 125, 125, 255);
    }
}
