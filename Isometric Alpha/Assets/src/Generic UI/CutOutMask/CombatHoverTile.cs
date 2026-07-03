using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CombatHoverTile : CombatMouseHover
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

    // public override void createListeners()
    // {

    // }

    // public override void destroyListeners()
    // {

    // }

    public void setTargetCoords(int row, int col)
    {
        targetCoords = new GridCoords(row, col);
        onEnemySide = CombatGrid.positionIsOnEnemySide(targetCoords);
    }


    // public void onReveal(bool toggleReveal)
    // {
    //     if(toggleReveal)
    //     {
    //         getSpriteOutline().createOutline(getRevealColor());
    //     } else
    //     {
    //         getSpriteOutline().removeOutline();
    //     }
    // }

    // public Color getRevealColor()
    // {
    //     if (onEnemySide)
    //     {
    //         return ColorList.attacksOnSight;
    //     }
    //     else
    //     {
    //         return ColorList.canBeInteractedWith;
    //     }
    // }

    protected override Stats getTargetStats()
    {
        return CombatGrid.getCombatantAtCoords(targetCoords);
    }

    public void OnMouseEnter()
    {
        // if(CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        // {
        //     return;
        // }

        // preserveHoverCoords();

        // if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null)
        // {
        //     onReveal(Constants.reveal);

        //     createHoverTag();

        //     if (CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation && tileHasTarget() && !DamagePreviewManager.hasPreviewAtCoords(targetCoords))
        //     {
        //         createHoverDamagePreview();
        //     }
        // }
    }

    public void OnMouseExit() 
    {
        // if(CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        // {
        //     return;
        // }

        // purgeHoverCoords();

        // if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null)
        // {
        //     if (getTargetGameObject() != null)
        //     {
        //         getSpriteOutline().removeOutline();
        //     }

        //     SelectorManager.displayCurrentHoverUI();

        //     if (CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation && tileHasTarget())
        //     {
        //         DamagePreviewManager.removeAllHoverPreviews();
        //         DamagePreviewManager.setUpDamagePreviews();
        //     }
        // }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        if(useHoverTiles())
        {
            OnMouseDown();
        }
    }

    protected override GridCoords getTargetCoords()
    {
        return targetCoords;
    }

    private void handleEnemyClick(Stats targetStats)
    {
        if(CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        if (CombatStateManager.currentActivity == CurrentActivity.ChoosingActor || CombatStateManager.currentActivity == CurrentActivity.ChoosingLocation)
        {
            SelectorManager.currentSelector.setToLocation(SelectorManager.findLegalCoordsContainingMandatoryTarget(SelectorManager.getCurrentSelector(), targetCoords));
        }
    }

    private void handleAllyClick(Stats targetStats)
    {
        if(CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        if (CombatStateManager.currentActivity == CurrentActivity.ChoosingAbility)
        {
            SelectorManager.deselectAlly();
        }

        if (targetStats.positions.Count > 0)
        {
            SelectorManager.currentSelector.setToLocation(targetStats.positions[0]);
        }

        if (targetStats.positions.Contains(SelectorManager.currentSelector.getCoords()))
        {
            SelectorManager.handleAllySelection();
        }
    }
}
