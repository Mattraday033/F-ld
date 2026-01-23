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

    public SpriteOutline getSpriteOutline()
    {
        return getTargetStats().outline;
    }

    public void onReveal(bool toggleReveal)
    {
        if(toggleReveal)
        {
            getSpriteOutline().createOutline(getRevealColor());
        } else
        {
            getSpriteOutline().removeOutline();
        }
    }

    public Color getRevealColor()
    {
        if (onEnemySide)
        {
            return ColorList.attacksOnSight;
        }
        else
        {
            return ColorList.canBeInteractedWith;
        }
    }

    protected override Stats getTargetStats()
    {
        return CombatGrid.getCombatantAtCoords(targetCoords);
    }

    private bool tileHasTarget()
    {
        return CombatGrid.getCombatantAtCoords(targetCoords) != null;
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

    public override void OnPointerExit(PointerEventData eventData) 
    {
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
}
