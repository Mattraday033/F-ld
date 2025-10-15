using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropReenterZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool insideGoodZone = false;
    public GridRow handler;

    public void OnPointerEnter(PointerEventData eventData)
    {
        insideGoodZone = true;
        handler.OnPointerEnter(new PointerEventData(null));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        insideGoodZone = false;
        handler.OnPointerExit(new PointerEventData(null));
    }

}
