using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CheckboxButton : Button, IPointerUpHandler
{
    private bool ignoreSFX = true;
    public Image secondaryGraphic;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        SelectionState previousState = currentSelectionState;

        base.DoStateTransition(state,instant);

        switch(state)
        {
            case SelectionState.Pressed:
            
                ignoreSFX = false;
                targetGraphic.color = ColorList.grey125;
                secondaryGraphic.color = ColorList.grey125;
                return;
            case SelectionState.Highlighted:
                targetGraphic.color = ColorList.grey75;
                secondaryGraphic.color = ColorList.grey75;
                return;
            case SelectionState.Normal:
                targetGraphic.color = ColorList.grey25;
                secondaryGraphic.color = ColorList.grey25;
                if(EventSystem.current != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }

                if(!ignoreSFX)
                {
                    AudioManager.playButtonOffSFX();
                }

                ignoreSFX = true;
                return;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        DoStateTransition(SelectionState.Normal, instant: true);
    }

}
