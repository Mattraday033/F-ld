using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphicRaycasterShield : MonoBehaviour,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerMoveHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        eventData.Use();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        eventData.Use();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        eventData.Use();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.Use();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.Use();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        eventData.Use();
    }
}
