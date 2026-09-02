using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class CombatantHover : CombatMouseHover, IRevealable
{
    public readonly static UnityEvent HighlightAllMandatoryTargets = new UnityEvent();
    public readonly static UnityEvent<Stats> StopHighlightFadeMandatoryTarget = new UnityEvent<Stats>();

    private const float timeToWaitFull = .5f;
    private const float timeToWaitFade = 1f;

    public Stats linkedStats;
    private List<Selector> selectors = new List<Selector>();

    private Coroutine highlightAndFadeCoroutine;

    private void stopHighlightAndFade(Stats stats)
    {
        if(stats == linkedStats && linkedStats != null)
        {
            if(highlightAndFadeCoroutine != null)
            {
                StopCoroutine(highlightAndFadeCoroutine);
                highlightAndFadeCoroutine = null;
            }

            linkedStats.removeOutline();
        }
    }

    private void highlightMandatoryTarget()
    {
        if(linkedStats != null && linkedStats as AllyStats == null && linkedStats.isMandatoryTarget())
        {
            if(highlightAndFadeCoroutine != null)
            {
                StopCoroutine(highlightAndFadeCoroutine);
            }

            highlightAndFadeCoroutine = StartCoroutine(highlightAndFade());
        } 
    }

    public void createOutlineAndStartFade()
    {
        if(highlightAndFadeCoroutine != null)
        {
            StopCoroutine(highlightAndFadeCoroutine);
        }

        highlightAndFadeCoroutine = StartCoroutine(highlightAndFade());
    }

    private Stats getHighlightTarget()
    {
        if(hasTargetStats(out Stats target) && target.queuedToMove())
        {
            return target.repositionClone;
        }

        return target;
    }

    private IEnumerator highlightAndFade()
    {
        Stats highlightTarget = getHighlightTarget();
        highlightTarget.setOutline();

        float timeWaited = 0f;

        while(timeWaited < timeToWaitFull)
        {
            yield return null;
            timeWaited += Time.deltaTime;
        }

        timeWaited = 0f;

        while(timeWaited < timeToWaitFade)
        {
            yield return null;
            timeWaited += Time.deltaTime;

            highlightTarget.setOutline((byte) Mathf.Lerp(254f, 0f, timeWaited/timeToWaitFade));
        }

        highlightTarget.removeOutline();

    }

    private void setHealthBarHovered(bool isHovered)
    {
        if(linkedStats != null && linkedStats.healthBarManager != null)
        {
            linkedStats.healthBarManager.setHovered(isHovered);
        }
    }

    public virtual void OnMouseEnter()
    {
        setHealthBarHovered(true);

        if(TutorialSequence.blockMouseHovers() ||
            AbilityMenuButton.hoveringOverAbilityMenuButton || 
            CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && hasTargetStats(out Stats target) && target.isAlive())
        {
            revealPriorityHeld = true;

            onReveal(Constants.reveal);

            CombatActionOrderRow.HighlightRow.Invoke(target, true);

            createHoverTag();

            SelectorManager.updateAllDamagePreviews();
            
            CombatUIModule.OnHideCombatUI.RemoveListener(target.removeOutline);
            CombatUIModule.OnHideCombatUI.AddListener(target.removeOutline);
        }

        CombatHoverTileManager.GetHoverSelector.AddListener(getHoverSelector);

        SelectorManager.declareSelectors();
    }

    public virtual void OnMouseExit()
    {
        setHealthBarHovered(false);

        revealPriorityHeld = false;

        if(TutorialSequence.blockMouseHovers() || 
            CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && hasTargetStats(out Stats target) && target.isAlive())
        {
            if (getTargetGameObject() != null)
            {
                onReveal(insideSelectors());
            }

            SelectorManager.displayCurrentHoverUI();

            CombatActionOrderRow.HighlightRow.Invoke(target, false);
        }

        SelectorManager.updateAllDamagePreviews();
        CombatHoverTileManager.GetHoverSelector.RemoveListener(getHoverSelector);

        SelectorManager.declareSelectors();
    }

    public static bool expectedHoveringOverAbilityMenuButtonFlagState = false;

    public static void setExpectedFlagState()
    {
        expectedHoveringOverAbilityMenuButtonFlagState = AbilityMenuButton.hoveringOverAbilityMenuButton;
    }

    public override void getHoverSelector(SelectorContainer container)
    {
        Selector hoverSelector = SelectorManager.currentSelector.clone();

        hoverSelector.hoverSelector = true;

        hoverSelector.setToLocation(CombatGrid.getCoordsWithHighestCoverageOfTargetPositions(linkedStats), declareSelectors: false);

        if(hoverSelector.getCoords().Equals(SelectorManager.getCurrentSelectorCoords()))
        {
            return;
        }

        container.selector = hoverSelector;
    }
    
    public void OnMouseOver()
    {
        if(TutorialSequence.blockMouseHovers() || 
            CombatStateManager.whoseTurn != WhoseTurn.Player || 
            CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu || 
            !hasTargetStats(out Stats target))
        {
            return;
        }

        if(AbilityMenuButton.hoveringOverAbilityMenuButton == expectedHoveringOverAbilityMenuButtonFlagState)
        {
            return;
        } else
        {
            if(AbilityMenuButton.hoveringOverAbilityMenuButton)
            {
                target.removeOutline();
                SelectorManager.displayCurrentHoverUI();
            } else
            {
                onReveal(Constants.reveal);
                createHoverTag();
            }

            setExpectedFlagState();
        }
    }

    protected override bool hasTargetStats(out Stats target)
    {
        Stats originalCombatant = CombatGrid.findOriginalCombatant(linkedStats);

        if(originalCombatant == null)
        {
            target = linkedStats;
        } else
        {
            target = originalCombatant;
        }

        return target != null;
    }

    protected override GridCoords getTargetCoords()
    {
        return linkedStats.positions.Count > 0 ? linkedStats.positions[0] : GridCoords.getDefaultCoords();
    }

    public void onReveal(bool toggleReveal)
    {
        if(!hasTargetStats(out Stats target))
        {
            return;
        }

        if(highlightAndFadeCoroutine != null)
        {
            StopHighlightFadeMandatoryTarget.Invoke(target);
        }

        if(toggleReveal && (!target.isDead() || revealPriorityHeld))
        {
            target.setOutline();
            target.healthBarManager.show();
        } else
        {
            target.removeOutline();
            target.healthBarManager.hide();
        }
    }

    public Color getRevealColor()
    {
        if(!hasTargetStats(out Stats target))
        {
            return Color.clear;
        }

        if (target.positions.Any(p => CombatGrid.positionIsOnAlliedSide(p)))
        {
            return ColorList.canBeInteractedWith;
        }
        else
        {
            return ColorList.attacksOnSight;
        }
    }

    private void updateOutlineFromSelectors(List<Selector> selectors)
    {
        this.selectors = selectors;

        if(revealPriorityHeld || linkedStats.isRepositionClone())
        {
            return;
        }

        onReveal(insideSelectors());
    }

    private bool insideSelectors()
    {
        foreach(Selector selector in selectors)
        {
            GridCoords[] gridCoords = selector.getAllSelectorCoords();

            if(linkedStats.isInsideCoordinates(gridCoords) || 
                (linkedStats.queuedToMove() && linkedStats.repositionClone.isInsideCoordinates(gridCoords)))
            {
                return true;
            }
        }
        
        return false;
    }

    private void addDamagePreview(CombatAction combatAction)
    {
        if(combatAction == null || combatAction.getSelector() == null)
        {
            return;
        }

        if(revealPriorityHeld || linkedStats.isInsideCoordinates(combatAction.getSelector().getAllSelectorCoords()))
        {
            DamagePreviewManager.addDamagePreview(linkedStats, linkedStats.healthBarManager, combatAction);
        }
    }

    private void OnEnable()
    {
        createListeners();
    }

    private void OnDisable()
    {
        destroyListeners();
    }

    #region IRevealable

    public string getName()
    {
        if(linkedStats != null)
        {
            return linkedStats.getName();
        } else
        {
            return "";
        }
    }

    public SpriteOutline getSpriteOutline()
    {
        if(!hasTargetStats(out Stats target))
        {
            return new SpriteOutline();
        }

        return target.getOutlines()[0];
    }

    private void holdRevealPriority(Stats stats)
    {
        if(stats != null && stats.Equals(linkedStats))
        {
            revealPriorityHeld = true;
        }
    }

    private void releaseRevealPriority(Stats stats)
    {
        if(stats != null && stats.Equals(linkedStats))
        {
            revealPriorityHeld = false;
        }
    }
    /*
        public readonly static UnityEvent<Stats> HoldRevealPriority = new UnityEvent<Stats>();
    public readonly static UnityEvent<Stats> ReleaseRevealPriority = new UnityEvent<Stats>();
    */
    
	public void createListeners()
    {
        CombatActionOrderRow.HoldRevealPriority.AddListener(holdRevealPriority);
        CombatActionOrderRow.ReleaseRevealPriority.AddListener(releaseRevealPriority);

        SelectorManager.SelectorMoved.AddListener(updateOutlineFromSelectors);
        DamagePreviewManager.UpdateDamagePreviews.AddListener(addDamagePreview);
        HoverPanelPopUpButton.HoverPriorityRequest.AddListener(answerCurrentCombatantPriorityRequest);
        CombatResultsUI.OnCombatResultsUICreation.AddListener(disableCollider);
        HighlightAllMandatoryTargets.AddListener(highlightMandatoryTarget);
        StopHighlightFadeMandatoryTarget.AddListener(stopHighlightAndFade);
    }

	public void destroyListeners()
    {
        CombatActionOrderRow.HoldRevealPriority.RemoveListener(holdRevealPriority);
        CombatActionOrderRow.ReleaseRevealPriority.RemoveListener(releaseRevealPriority);

        SelectorManager.SelectorMoved.RemoveListener(updateOutlineFromSelectors);
        DamagePreviewManager.UpdateDamagePreviews.RemoveListener(addDamagePreview);
        HoverPanelPopUpButton.HoverPriorityRequest.RemoveListener(answerCurrentCombatantPriorityRequest);
        CombatResultsUI.OnCombatResultsUICreation.RemoveListener(disableCollider);
        HighlightAllMandatoryTargets.RemoveListener(highlightMandatoryTarget);
        StopHighlightFadeMandatoryTarget.RemoveListener(stopHighlightAndFade);

        if(hasTargetStats(out Stats target))
        {
            CombatUIModule.OnHideCombatUI.RemoveListener(target.removeOutline);
        }
    }

    public void disableCollider()
    {
        if(linkedStats != null)
        {
            linkedStats.disablePolygonCollider();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Empty on purpose
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Empty on purpose
    }

    #endregion

}
