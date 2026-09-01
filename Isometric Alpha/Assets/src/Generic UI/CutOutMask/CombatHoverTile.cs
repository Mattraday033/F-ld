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

    public SpriteRenderer frontSpriteRenderer;
    public SpriteRenderer backSpriteRenderer;

    public SpriteRenderer arrowSpriteRenderer;

    public EffectAnimationManager frontEffectManager;
    public EffectAnimationManager backEffectManager;

    private bool inVisibleSelector = false;

    private bool queueMouseOver = false;

    private PolygonCollider2D hoverCollider;

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

    private void Awake()
    {
        hoverCollider = GetComponent<PolygonCollider2D>();

        backEffectManager.loops = true;
        backEffectManager.setAnimations(EffectAnimationType.BackSelector2);

        frontEffectManager.loops = true;
        frontEffectManager.setAnimations(EffectAnimationType.FrontSelector2);

        CombatStateManager.OnActivityChangeToInEscapeMenu.AddListener(disableHoverCollider);
        CombatStateManager.OnActivityChangeFromInEscapeMenu.AddListener(enableHoverCollider);
        
        CombatStateManager.OnActivityChangeToResolveTurnWarning.AddListener(disableHoverCollider);
        CombatStateManager.OnActivityChangeFromResolveTurnWarning.AddListener(enableHoverCollider);
    }

    private void OnDestroy()
    {
        CombatStateManager.OnActivityChangeToInEscapeMenu.RemoveListener(disableHoverCollider);
        CombatStateManager.OnActivityChangeFromInEscapeMenu.RemoveListener(enableHoverCollider);

        CombatStateManager.OnActivityChangeToResolveTurnWarning.RemoveListener(disableHoverCollider);
        CombatStateManager.OnActivityChangeFromResolveTurnWarning.RemoveListener(enableHoverCollider);
    }

    private void disableHoverCollider()
    {
        hoverCollider.enabled = false;
    }

    private void enableHoverCollider()
    {
        hoverCollider.enabled = true;
    }

    protected override bool handleTutorialClick()
    {
        if(!TutorialSequence.currentStepAllowsCombatTileClicks())
        {
            return false;
        }

        GridCoords currentCoords = SelectorManager.getCurrentSelectorCoords();
        GridCoords destination = getLegalSelectorDestination();

        // The keyboard cannot cross the battlefield divide during a tutorial step, so neither can a click.
        if(!CombatGrid.positionsAreOnSameSide(currentCoords, destination))
        {
            return true;
        }

        moveSelectorToTarget();

        if(!destination.Equals(currentCoords))
        {
            AudioManager.playSelectorMovedSFX();
        }

        SpawnHoverPanel.runInstanceOfScript();

        SelectorManager.updateAllDamagePreviews();

        if(TutorialSequence.conditionFulfilled())
        {
            TutorialSequence.advanceCurrentTutorialSequence();
        }

        return true;
    }

    private void OnEnable()
    {
        SelectorManager.SelectorMoved.AddListener(determineVisbility);
        HoverPanelPopUpButton.HoverPriorityRequest.AddListener(answerCurrentCombatantPriorityRequest);
    }

    private void OnDisable()
    {
        SelectorManager.SelectorMoved.RemoveListener(determineVisbility);
        HoverPanelPopUpButton.HoverPriorityRequest.RemoveListener(answerCurrentCombatantPriorityRequest);
    }

    #region
    
    public void setColor(Color color)
    {
        currentColor = color;
    }

    public void hideTile()
    {
        backSpriteRenderer.color = Color.clear;
        frontSpriteRenderer.color = Color.clear;
        arrowSpriteRenderer.color = Color.clear;
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
                backSpriteRenderer.color = currentColor;
                frontSpriteRenderer.color = currentColor;

                if(CombatStateManager.inCombat && 
                    CombatStateManager.currentActivity == CurrentActivity.ChoosingTertiary && 
                    (currentColor.Equals(Color.yellow) || currentColor.a < 1f))
                {
                    arrowSpriteRenderer.color = currentColor;
                } else
                {
                    arrowSpriteRenderer.color = Color.clear;
                }

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

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null && !getTargetStats().isDead())
        {
            revealPriorityHeld = true;

            getTargetStats().healthBarManager.setHovered(true);

            CombatActionOrderRow.HighlightRow.Invoke(getTargetStats(), true);
        }

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

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null && !getTargetStats().isDead())
        {
            revealPriorityHeld = false;

            getTargetStats().healthBarManager.setHovered(false);

            CombatActionOrderRow.HighlightRow.Invoke(getTargetStats(), false);
        }

        SelectorManager.declareSelectors();
    }

    public void OnMouseOver() 
    {
        if(CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        } else if(AbilityMenuButton.hoveringOverAbilityMenuButton)
        {
            queueMouseOver = true;
        }

        if(queueMouseOver && !AbilityMenuButton.hoveringOverAbilityMenuButton)
        {
            queueMouseOver = false;
            OnMouseEnter();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(CutOutMaskInternalBlockerManager.isBlocking())
        {
            return;
        }

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
        if(CutOutMaskInternalBlockerManager.isBlocking())
        {
            return;
        }

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