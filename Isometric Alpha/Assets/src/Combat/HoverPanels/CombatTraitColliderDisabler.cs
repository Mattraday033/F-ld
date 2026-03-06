using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CombatTraitColliderDisabler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public readonly static UnityEvent OnCombatTraitHoverEnter = new UnityEvent();
    public readonly static UnityEvent OnCombatTraitHoverExit = new UnityEvent();

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch(CombatStateManager.currentActivity)
        {
            case CurrentActivity.ChoosingActor:
            case CurrentActivity.ChoosingAbility:
            case CurrentActivity.ChoosingLocation:
            case CurrentActivity.ChoosingTertiary:
            case CurrentActivity.Finished:
                OnCombatTraitHoverEnter.Invoke();
                break;
            default:
                return;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch(CombatStateManager.currentActivity)
        {
            case CurrentActivity.ChoosingActor:
            case CurrentActivity.ChoosingAbility:
            case CurrentActivity.ChoosingLocation:
            case CurrentActivity.ChoosingTertiary:
            case CurrentActivity.Finished:
                OnCombatTraitHoverExit.Invoke();
                break;
            default:
                return;
        }
        
    }
}
