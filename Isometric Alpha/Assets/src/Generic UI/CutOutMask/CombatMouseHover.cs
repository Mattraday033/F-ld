using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatMouseHover : MonoBehaviour
{
    protected void createHoverDamagePreview()
    {
        DamagePreviewManager.setUpHoverDamagePreview(getTargetStats());
    }

    public void createHoverTag()
    {
        SelectorManager.displayHoverUI(getTargetStats());
    }

    protected void moveSelectorToTarget()
    {
        SelectorManager.currentSelector.setToLocation(SelectorManager.findLegalCoordsContainingMandatoryTarget(SelectorManager.getCurrentSelector(), getTargetCoords()));

        SelectorManager.createPressEPrompt();

        if (CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation)
        {
            DamagePreviewManager.setUpDamagePreviews();
        }
    }

    public void OnMouseDown()
    {
        if(AbilityMenuButton.hoveringOverAbilityMenuButton)
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

                    SelectorManager.deselectCurrentAlly();

                    moveSelectorToTarget();

                    SelectorManager.handleAllySelection();

                    break;
                case CurrentActivity.ChoosingLocation:

                    if (currentSelectorContainsTarget())
                    {
                        SelectorManager.handleChoosingLocation();
                    }
                    else
                    {
                        if (canMoveToLocation(AbilityMenuManager.getInstance().getCurrentlySelectedAbility()))
                        {
                            moveSelectorToTarget();
                        }
                    }
                    break;
                case CurrentActivity.ChoosingTertiary:

                    if (currentSelectorContainsTarget())
                    {
                        SelectorManager.handleChoosingTertiary();
                    }
                    else
                    {
                        if (canMoveToLocation(AbilityMenuManager.getInstance().getCurrentlySelectedAbility()))
                        {
                            moveSelectorToTarget();
                        }
                    }
                    break;
                default:
                    break;
            }

            createHoverTag();
        }
    }

    protected bool useHoverTiles()
    {
        return CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation && 
            AbilityMenuManager.getInstance().getCurrentlySelectedLoadedCombatAction().targetsOnlyEmptySpace();
    }

    protected abstract Stats getTargetStats();
    protected abstract GridCoords getTargetCoords();

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

}
