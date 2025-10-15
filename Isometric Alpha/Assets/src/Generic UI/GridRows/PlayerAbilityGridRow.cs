using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAbilityGridRow : GridRow, IPointerDownHandler, IDragAndDropSource
{

    // private void Awake()
    // {
    //     if (PlayerOOCStateManager.currentActivity == OOCActivity.inTutorialSequence)
    //     {
    //         buttons[0].enabled = false;
    //         TutorialSequence.OnEnableButtons.AddListener(enableButton);
    //     }
    // }

    // private void enableButton()
    // {
    //     buttons[0].enabled = true;
    //     TutorialSequence.OnEnableButtons.RemoveListener(enableButton);
    // }

    public string getDragAndDropPrefabName()
    {
        return PrefabNames.dragAndDropActionIcon;
    }

    private bool canAutoEquipAction()
    {
        CombatAction action = descriptionPanel.getObjectBeingDescribed() as CombatAction;

        if (action == null)
        {
            return false;
        }

        if (action.alternateActionWhenPlacedInActionSlot() != null)
        {
            action = action.alternateActionWhenPlacedInActionSlot();
        }

        int statRequirement = action.getRequiredStatLevel();

        return statRequirement <= CharacterScreen.getCurrentDisplayedStatLevel() &&
                action.hasAvailableSlots(OverallUIManager.getCurrentActionArray());
    }

    private bool canCreateActionDragAndDropIcon()
    {
        CombatAction action = descriptionPanel.getObjectBeingDescribed() as CombatAction;

        if (action == null || !action.canBePlacedInActionSlot() || action.getMaximumSlots() <= 0)
        {
            return false;
        }

        if (action.alternateActionWhenPlacedInActionSlot() != null)
        {
            action = action.alternateActionWhenPlacedInActionSlot();
        }

        int statRequirement = action.getRequiredStatLevel();

        return statRequirement <= CharacterScreen.getCurrentDisplayedStatLevel();
    }

    private CombatAction checkForAlternateAction(CombatAction action)
    {
        if (action != null && action.alternateActionWhenPlacedInActionSlot() != null)
        {
            return action.alternateActionWhenPlacedInActionSlot();
        }

        return action;
    }

    public override bool canSeeHover()
    {
        return hoverEnabled;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (!canCreateActionDragAndDropIcon())
        {
            return;
        }

        CombatAction action = checkForAlternateAction(descriptionPanel.getObjectBeingDescribed() as CombatAction);

        StartCoroutine(DragAndDropManager.waitForMouseRelease(this, action));
    }

    public void putActionInFirstAvailableSlot()
    {
        if (canAutoEquipAction())
        {
            CombatAction action = checkForAlternateAction(descriptionPanel.getObjectBeingDescribed() as CombatAction);

            OverallUIManager.getCurrentActionArray().equipCombatAction(action);
        }
    }
}
