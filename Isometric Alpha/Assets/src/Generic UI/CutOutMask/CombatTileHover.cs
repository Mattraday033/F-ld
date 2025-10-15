using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CombatTileHover : AlphaDeterminedRaycastTarget, IRevealable, IPointerDownHandler
{

    public static GridCoords previousGridCoords;

    private bool onEnemySide;
    private GridCoords targetCoords;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public void createListeners()
    {

    }

    public void destroyListeners()
    {

    }

    public override bool useExactCopyOfSprite()
    {
        return false;
    }

    public override bool alphaShouldDetermineRaycastTarget()
    {
        return true;
    }

    public void setTargetCoords(int row, int col)
    {
        targetCoords = new GridCoords(row, col);
        onEnemySide = CombatGrid.positionIsOnEnemySide(targetCoords);
    }

    public void onReveal()
    {
        RevealManager.setRevealForGameObject(getTargetGameObject(), getRevealColor());
    }

    public Color getRevealColor()
    {
        if (onEnemySide)
        {
            return RevealManager.attacksOnSight;
        }
        else
        {
            return RevealManager.canBeInteractedWith;
        }

    }

    public void spawnTargetCanvas()
    {
        //EmptyOnPurpose
    }

    private Stats getTargetStats()
    {
        return CombatGrid.getCombatantAtCoords(targetCoords);
    }

    private bool tileHasTarget()
    {
        return CombatGrid.getCombatantAtCoords(targetCoords) != null;
    }

    private GameObject getTargetGameObject()
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

    public void createHoverTag()
    {
        SelectorManager.displayHoverUI(getTargetStats());
    }

    private void createHoverDamagePreview()
    {
        DamagePreviewManager.setUpHoverDamagePreview(getTargetStats());
    }

    private void preserveHoverCoords()
    {
        previousGridCoords = targetCoords.clone();
    }

    private void purgeHoverCoords()
    {
        previousGridCoords = GridCoords.getDefaultCoords();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        preserveHoverCoords();

        if (CombatStateManager.whoseTurn == WhoseTurn.Player)
        {
            onReveal();

            createHoverTag();

            if (CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation && tileHasTarget() && !DamagePreviewManager.hasPreviewAtCoords(targetCoords))
            {
                createHoverDamagePreview();
            }

            createListeners();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {

        purgeHoverCoords();

        if (CombatStateManager.whoseTurn == WhoseTurn.Player)
        {
            if (getTargetGameObject() != null)
            {
                RevealManager.setOutlineColorToDefault(getTargetGameObject());
            }

            SelectorManager.displayHoverUIForCurrentSelectorTarget();

            if (CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation && tileHasTarget())
            {
                DamagePreviewManager.removeAllHoverPreviews();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
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

                    if (currentSelectorOnTile())
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

                    if (currentSelectorOnTile())
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


            // if (CombatStateManager.currentActivity == CurrentActivity.ChoosingActor || CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation)
            // {
            //     moveSelectorToTarget();
            // }
            // else if (CombatStateManager.currentActivity == CurrentActivity.ChoosingAbility && tileHasTarget() && !onEnemySide)
            // {
            //     SelectorManager.deselectCurrentAlly();
            //     moveSelectorToTarget();

            //     if (SelectorManager.currentSelector.getCoords().Equals(targetCoords))
            //     {
            //         SelectorManager.handleAllySelection();
            //     }
            // }
        }
    }

    private void moveSelectorToTarget()
    {
        SelectorManager.currentSelector.setToLocation(SelectorManager.findLegalCoordsContainingMandatoryTarget(SelectorManager.getCurrentSelector(), targetCoords));

        SelectorManager.createPressEPrompt();

        if (CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation)
        {
            DamagePreviewManager.setUpDamagePreviews();
        }
    }

    private bool currentSelectorOnTile()
    {
        return SelectorManager.currentSelector.getCoords().Equals(targetCoords);
    }

    private void handleEnemyClick(Stats targetStats)
    {
        if (CombatStateManager.currentActivity == CurrentActivity.ChoosingActor || CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation)
        {
            SelectorManager.currentSelector.setToLocation(SelectorManager.findLegalCoordsContainingMandatoryTarget(SelectorManager.getCurrentSelector(), targetCoords));
        }
    }

    private void handleAllyClick(Stats targetStats)
    {
        if (CombatStateManager.currentActivity == CurrentActivity.ChoosingAbility)
        {
            SelectorManager.deselectCurrentAlly();
        }

        SelectorManager.currentSelector.setToLocation(targetStats.position);

        if (SelectorManager.currentSelector.getCoords().Equals(targetStats.position))
        {
            SelectorManager.handleAllySelection();
        }
    }

    public bool canMoveToLocation(CombatAction combatAction)
    {
        if (CombatGrid.positionIsOnAlliedSide(targetCoords) && combatAction.targetsAllySection())
        {
            return true;
        } else if (CombatGrid.positionIsOnEnemySide(targetCoords) && !combatAction.targetsAllySection())
        {
            return true;
        }
        
        return false;
    }
}
