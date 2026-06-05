using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThreeRingButton : Button, IPointerDownHandler, IPointerUpHandler
{

    public Transform topRing;
    public Transform middleRing;

    public Vector3 midRingResting;
    public Vector3 topRingResting;


    protected override void Awake()
    {
        base.Awake();

        if(midRingResting.Equals(Vector3.zero) &&
            topRingResting.Equals(Vector3.zero))
        {
            lockInRestingPosition();
        }
    }

    public void lockInRestingPosition()
    {
        midRingResting = middleRing.localPosition;
        topRingResting = topRing.localPosition;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        setPositionByInteractability();
    }

    // public override void OnPointerClick(PointerEventData eventData)
    // {
    //     base.OnPointerClick(eventData);
    // }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        setToPressed();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if(interactable)
        {
            setToResting();
        }
    }

    public void setInteractable(bool newInteractable)
    {
        interactable = newInteractable;

        setPositionByInteractability();
    }

    private void setPositionByInteractability()
    {
        if(interactable)
        {
            setToResting();
        } else
        {
            setToPressed();
        }
    }

    private void setToResting()
    {
        middleRing.localPosition = midRingResting;
        topRing.localPosition = topRingResting;
    }

    private void setToPressed()
    {
        topRing.localPosition = Vector2.zero;
        middleRing.localPosition = Vector2.zero;
    }
}
