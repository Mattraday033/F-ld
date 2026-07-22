using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatMouseHover : MonoBehaviour
{

    public bool revealPriorityHeld = false;

    public void createHoverTag()
    {
        // SelectorManager.displayHoverUI(getTargetStats());
        SelectorManager.displayCurrentHoverUI();
    }

    protected void moveSelectorToTarget()
    {
        SelectorManager.currentSelector.setToLocation(SelectorManager.findLegalCoordsContainingMandatoryTarget(SelectorManager.getCurrentSelector(), getTargetCoords()));

        SelectorManager.createPressEPrompt();

        SelectorManager.updateAllDamagePreviews();
    }

    public void OnMouseDown()
    {
        if(AbilityMenuButton.hoveringOverAbilityMenuButton || 
            CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        if (CombatStateManager.whoseTurn == WhoseTurn.Player)
        {
            switch (CombatStateManager.currentActivity)
            {
                case CurrentActivity.ChoosingActor:

                    moveSelectorToTarget();

                    SelectorManager.handleAllySelection();

                    break;
                case CurrentActivity.ChoosingAbility:

                    SelectorManager.deselectAlly();

                    moveSelectorToTarget();

                    SelectorManager.handleAllySelection();

                    break;
                case CurrentActivity.ChoosingLocation:
                    if (canMoveToLocation(AbilityMenuManager.getInstance().getCurrentlySelectedAction()))
                    {
                        moveSelectorToTarget();
                        SelectorManager.handleChoosingLocation();
                    }
                    break;
                case CurrentActivity.ChoosingTertiary:

                    // if (currentSelectorContainsTarget())
                    // {
                    //     SelectorManager.handleChoosingTertiary();
                    // }
                    // else
                    // {
                    //     if (canMoveToLocation(AbilityMenuManager.getInstance().getCurrentlySelectedAction()))
                    //     {
                    //         moveSelectorToTarget();
                    //     }
                    // }

                    if (canMoveToLocation(AbilityMenuManager.getInstance().getCurrentlySelectedAction()))
                    {
                        moveSelectorToTarget();
                        SelectorManager.handleChoosingTertiary();
                    }
                    break;
                default:
                    break;
            }

            createHoverTag();
        }
    }

    protected abstract Stats getTargetStats();
    protected abstract GridCoords getTargetCoords();

    public abstract void getHoverSelector(SelectorContainer container);

    protected bool canMoveToLocation(CombatAction combatAction)
    {
        if (CombatGrid.positionIsOnAlliedSide(getTargetCoords()) && combatAction.targetsAllySection())
        {
            return true;
        } else if (CombatGrid.positionIsOnEnemySide(getTargetCoords()) && !combatAction.targetsAllySection())
        {
            return true;
        }
        
        return false;
    }

    protected bool currentSelectorOnTile()
    {
        return SelectorManager.currentSelector.getCoords().Equals(getTargetCoords());
    }

    protected bool currentSelectorContainsTarget()
    {
        return SelectorManager.currentSelector.containsTarget(getTargetCoords());
    }

    protected GameObject getTargetGameObject()
    {
        Stats targetStats = getTargetStats();

        if (targetStats != null)
        {
            return targetStats.combatSprite;
        }
        else
        {
            return null;
        }
    }

    protected void answerCurrentCombatantPriorityRequest()
    {
        if(revealPriorityHeld)
        {
            HoverPanelPopUpButton.currentCombatantWithPriority = getTargetStats();
        }
    }

}
