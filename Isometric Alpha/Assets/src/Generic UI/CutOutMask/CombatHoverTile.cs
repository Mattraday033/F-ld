using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CombatHoverTile : CombatMouseHover, IPointerDownHandler, IPointerUpHandler
{

    public readonly static UnityEvent ReleaseAllMouseUpWaits = new UnityEvent();
    private bool onEnemySide;
    private GridCoords targetCoords;

    private Color currentColor;

    public SpriteRenderer spriteRenderer;

    private bool inVisibleSelector = false;

    private bool _WaitingOnMouseUp = false;
    private bool waitingOnMouseUp
    {
        get
        {
            return _WaitingOnMouseUp;
        }
        set
        {
            _WaitingOnMouseUp = value;
            if(_WaitingOnMouseUp)
            {
                ReleaseAllMouseUpWaits.AddListener(releaseMouseUpWait);
            }
            else
            {
                ReleaseAllMouseUpWaits.RemoveListener(releaseMouseUpWait);
            }
        }
    }

    private void OnEnable()
    {
        SelectorManager.SelectorMoved.AddListener(determineVisbility);
    }

    private void OnDisable()
    {
        SelectorManager.SelectorMoved.RemoveListener(determineVisbility);
    }

    #region
    
    public void setColor(Color color)
    {
        currentColor = color;
    }

    private void setToPressedColor()
    {
        spriteRenderer.color = currentColor;
        spriteRenderer.color = new Color(spriteRenderer.color.r - .25f, 
                                         spriteRenderer.color.g - .25f, 
                                         spriteRenderer.color.b - .25f);
    }

    public void hideTile()
    {
        spriteRenderer.color = Color.clear;
        inVisibleSelector = false;
    }

    private void determineVisbility(List<Selector> visibleSelectors)
    {
        bool updated = false;

        foreach(Selector selector in visibleSelectors)
        {
            if(selector.containsTarget(targetCoords))
            {
                selector.setToColor();
                spriteRenderer.color = currentColor;
                inVisibleSelector = true;
                updated = true;
            }
        }

        if(!updated)
        {
            hideTile();
        }
    }

    #endregion

    public override void getHoverSelector(SelectorContainer container)
    {
        Selector hoverSelector = SelectorManager.currentSelector.clone();

        hoverSelector.hoverSelector = true;

        hoverSelector.setToLocation(SelectorManager.findLegalCoordsContainingMandatoryTarget(hoverSelector, targetCoords), declareSelectors: false);

        if(hoverSelector.getCoords().Equals(SelectorManager.getCurrentSelectorCoords()))
        {
            return;
        }

        container.selector = hoverSelector;
    }

    private void releaseMouseUpWait()
    {
        waitingOnMouseUp = false;
    }

    public void OnMouseEnter()
    {
        if(AbilityMenuButton.hoveringOverAbilityMenuButton || 
            CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        CombatHoverTileManager.GetHoverSelector.AddListener(getHoverSelector);

        SelectorManager.declareSelectors();
    }

    public void OnMouseExit() 
    {
        if(AbilityMenuButton.hoveringOverAbilityMenuButton || 
            CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        CombatHoverTileManager.GetHoverSelector.RemoveListener(getHoverSelector);

        SelectorManager.declareSelectors();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch(CombatStateManager.currentActivity)
        {
            case CurrentActivity.ChoosingActor:
            case CurrentActivity.ChoosingLocation:
            case CurrentActivity.ChoosingTertiary:
            case CurrentActivity.Tutorial:
                waitingOnMouseUp = true;
                return;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch(CombatStateManager.currentActivity)
        {
            case CurrentActivity.ChoosingActor:
            case CurrentActivity.ChoosingLocation:
            case CurrentActivity.ChoosingTertiary:
            case CurrentActivity.Tutorial:
                ReleaseAllMouseUpWaits.Invoke();
                return;
        }
    }

    public void setTargetCoords(int row, int col)
    {
        targetCoords = new GridCoords(row, col);
        onEnemySide = CombatGrid.positionIsOnEnemySide(targetCoords);
    }

    protected override Stats getTargetStats()
    {
        return CombatGrid.getCombatantAtCoords(targetCoords);
    }

    protected override GridCoords getTargetCoords()
    {
        return targetCoords;
    }
}
/*
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
    */