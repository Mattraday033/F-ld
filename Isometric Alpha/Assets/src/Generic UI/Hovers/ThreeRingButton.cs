using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThreeRingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public Transform topRing;
    public Transform middleRing;

    private void OnDisable()
    {
        middleRing.localPosition = new Vector2(0f, 1f);
        topRing.localPosition = new Vector2(0f, 1.5f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        topRing.localPosition = Vector2.zero;
        middleRing.localPosition = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        middleRing.localPosition = new Vector2(0f, 1f);
        topRing.localPosition = new Vector2(0f, 1.5f);
    }
}
