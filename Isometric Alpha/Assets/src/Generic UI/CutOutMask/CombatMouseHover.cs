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

    protected GridCoords getLegalSelectorDestination()
    {
        return SelectorManager.findLegalCoordsContainingMandatoryTarget(SelectorManager.getCurrentSelector(), getTargetCoords());
    }

    protected void moveSelectorToTarget()
    {
        SelectorManager.currentSelector.setToLocation(getLegalSelectorDestination());

        SelectorManager.createPressEPrompt();

        SelectorManager.updateAllDamagePreviews();
    }

    // Returns whether the click was consumed by the current tutorial step. Only combat hover
    // tiles handle this; everything else falls through to the normal click behaviour.
    protected virtual bool handleTutorialClick()
    {
        return false;
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

                    if (canMoveToLocation(AbilityMenuManager.getInstance().getCurrentlySelectedAction()))
                    {
                        moveSelectorToTarget();
                        SelectorManager.handleChoosingTertiary();
                    }
                    break;
                case CurrentActivity.Tutorial:

                    if (handleTutorialClick())
                    {
                        return;
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
