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

        //the effect managers are on child objects, so they're readied here in case they haven't woken yet
        backEffectManager.Awake();
        backEffectManager.loops = true;
        backEffectManager.animationData = AnimationDataList.backSelector2;
        backEffectManager.startAnimation();

        frontEffectManager.Awake();
        frontEffectManager.loops = true;
        frontEffectManager.animationData = AnimationDataList.frontSelector2;
        frontEffectManager.startAnimation();

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
        SelectorManager.SelectorMoved.AddListener(updateOutlineFromSelectors);
        CombatActionOrderRow.HoldActorOutlines.AddListener(holdOutline);
        CombatActionOrderRow.ReleaseActorOutlines.AddListener(releaseOutline);
        HoverPanelPopUpButton.HoverPriorityRequest.AddListener(answerCurrentCombatantPriorityRequest);
    }

    private void OnDisable()
    {
        SelectorManager.SelectorMoved.RemoveListener(determineVisbility);
        SelectorManager.SelectorMoved.RemoveListener(updateOutlineFromSelectors);
        CombatActionOrderRow.HoldActorOutlines.RemoveListener(holdOutline);
        CombatActionOrderRow.ReleaseActorOutlines.RemoveListener(releaseOutline);
        HoverPanelPopUpButton.HoverPriorityRequest.RemoveListener(answerCurrentCombatantPriorityRequest);
    }

    #region Combatant Outline

    //the action an action order row is keeping outlined, whose actors' outlines the selectors must leave alone.
    //Every tile keeps it, since an action like a volley has an actor on several of them
    private CombatAction outlineHeldFor;

    private void holdOutline(CombatAction action)
    {
        outlineHeldFor = action;
    }

    private void releaseOutline(CombatAction action)
    {
        if(ReferenceEquals(action, outlineHeldFor))
        {
            outlineHeldFor = null;
        }
    }

    //outlines the creature standing on this tile while it's inside a visible selector, a job its CombatantHover used to do.
    //Every tile under a creature reaches the same answer, since the whole creature is checked rather than this one tile
    private void updateOutlineFromSelectors(List<Selector> visibleSelectors)
    {
        if(!hasTargetStats(out Stats target) || target.isRepositionClone() || outlineIsHeld(target))
        {
            return;
        }

        //a mandatory target's fading highlight would otherwise go on to overwrite the outline set here
        CombatantHover.StopHighlightFadeMandatoryTarget.Invoke(target);

        if(!target.isDead() && insideSelectors(target, visibleSelectors))
        {
            target.setOutline();
        } else
        {
            target.removeOutline();
        }
    }

    //held by an action order row, or by the mouse sitting on one of the creature's sprites
    private bool outlineIsHeld(Stats target)
    {
        if(outlineHeldFor != null && outlineHeldFor.actorIsPartOfAction(target))
        {
            return true;
        }

        foreach(Combatant combatant in target.combatants)
        {
            if(combatant != null && combatant.hover != null && combatant.hover.revealPriorityHeld)
            {
                return true;
            }
        }

        return false;
    }

    private static bool insideSelectors(Stats target, List<Selector> visibleSelectors)
    {
        foreach(Selector selector in visibleSelectors)
        {
            GridCoords[] gridCoords = selector.getAllSelectorCoords();

            if(target.isInsideCoordinates(gridCoords) ||
                (target.queuedToMove() && target.repositionClone.isInsideCoordinates(gridCoords)))
            {
                return true;
            }
        }

        return false;
    }

    #endregion

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
                    PlayerStateManager.currentActivity == CurrentActivity.ChoosingTertiary && 
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
            PlayerStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        CombatHoverTileManager.GetHoverSelector.AddListener(getHoverSelector);

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && hasTargetStats(out Stats target) && target.isAlive())
        {
            revealPriorityHeld = true;

            setHealthBarHovered(target, true);

            CombatActionOrderRow.HighlightRow.Invoke(target, true);
        }

        SelectorManager.declareSelectors();
    }

    public void OnMouseExit() 
    {
        if(AbilityMenuButton.hoveringOverAbilityMenuButton || 
            PlayerStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        CombatHoverTileManager.GetHoverSelector.RemoveListener(getHoverSelector);

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && hasTargetStats(out Stats target) && target.isAlive())
        {
            revealPriorityHeld = false;

            setHealthBarHovered(target, false);

            CombatActionOrderRow.HighlightRow.Invoke(target, false);
        }

        SelectorManager.declareSelectors();
    }

    private void setHealthBarHovered(Stats target, bool isHovered)
    {
        HealthBarManager healthBarManager = target.getHealthBarManager();

        if(healthBarManager != null)
        {
            healthBarManager.setHovered(isHovered);
        }
    }

    public void OnMouseOver() 
    {
        if(PlayerStateManager.currentActivity == CurrentActivity.InEscapeMenu)
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

        switch(PlayerStateManager.currentActivity)
        {
            case CurrentActivity.ChoosingActor:
            case CurrentActivity.ChoosingLocation:
            case CurrentActivity.ChoosingTertiary:
            case CurrentActivity.InTutorialSequence:
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

        switch(PlayerStateManager.currentActivity)
        {
            case CurrentActivity.ChoosingActor:
            case CurrentActivity.ChoosingLocation:
            case CurrentActivity.ChoosingTertiary:
            case CurrentActivity.InTutorialSequence:
                ReleaseAllMouseUpWaits.Invoke();
                return;
        }
    }

    public void setTargetCoords(int row, int col)
    {
        targetCoords = new GridCoords(row, col);
        onEnemySide = CombatGrid.positionIsOnEnemySide(targetCoords);
    }

    protected override bool hasTargetStats(out Stats target)
    {
        return CombatGrid.combatantExistsAtCoords(targetCoords, out target);
    }

    protected override GridCoords getTargetCoords()
    {
        return targetCoords;
    }
}