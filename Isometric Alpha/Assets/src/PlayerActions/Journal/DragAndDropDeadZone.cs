using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropDeadZone : MonoBehaviour,	IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MouseHoverManager.hoverCoroutine != null)
        {
            MouseHoverManager.stopCoroutine(); 
        }

        MouseHoverManager.OnHoverPanelCreation.Invoke();
	}


}
