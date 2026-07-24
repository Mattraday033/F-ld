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
        Stats target = getTargetStats();

        if(target.queuedToMove())
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

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null && !getTargetStats().isDead())
        {
            revealPriorityHeld = true;

            onReveal(Constants.reveal);

            CombatActionOrderRow.HighlightRow.Invoke(getTargetStats(), true);

            createHoverTag();

            SelectorManager.updateAllDamagePreviews();
            
            CombatUIModule.OnHideCombatUI.RemoveListener(getTargetStats().removeOutline);
            CombatUIModule.OnHideCombatUI.AddListener(getTargetStats().removeOutline);
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

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null && !getTargetStats().isDead())
        {
            if (getTargetGameObject() != null)
            {
                onReveal(insideSelectors());
            }

            SelectorManager.displayCurrentHoverUI();

            CombatActionOrderRow.HighlightRow.Invoke(getTargetStats(), false);
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
            CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
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
                getTargetStats().removeOutline();
                SelectorManager.displayCurrentHoverUI();
            } else
            {
                onReveal(Constants.reveal);
                createHoverTag();
            }

            setExpectedFlagState();
        }
    }

    protected override Stats getTargetStats()
    {
        Stats originalCombatant = CombatGrid.findOriginalCombatant(linkedStats);

        if(originalCombatant == null)
        {
            return linkedStats;
        }

        return originalCombatant;
    }

    protected override GridCoords getTargetCoords()
    {
        return linkedStats.positions.Count > 0 ? linkedStats.positions[0] : GridCoords.getDefaultCoords();
    }

    public void onReveal(bool toggleReveal)
    {
        if(highlightAndFadeCoroutine != null)
        {
            StopHighlightFadeMandatoryTarget.Invoke(linkedStats);
        }

        if(toggleReveal && (!linkedStats.isDead() || revealPriorityHeld))
        {
            getTargetStats().setOutline();
        } else
        {
            getTargetStats().removeOutline();
        }
    }

    public Color getRevealColor()
    {
        if (getTargetStats().positions.Any(p => CombatGrid.positionIsOnAlliedSide(p)))
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

    public SpriteOutline getSpriteOutline()
    {
        return getTargetStats().getOutlines()[0];
    }
    
	public void createListeners()
    {
        SelectorManager.SelectorMoved.AddListener(updateOutlineFromSelectors);
        DamagePreviewManager.UpdateDamagePreviews.AddListener(addDamagePreview);
        HoverPanelPopUpButton.HoverPriorityRequest.AddListener(answerCurrentCombatantPriorityRequest);
        CombatResultsUI.OnCombatResultsUICreation.AddListener(disableCollider);
        HighlightAllMandatoryTargets.AddListener(highlightMandatoryTarget);
        StopHighlightFadeMandatoryTarget.AddListener(stopHighlightAndFade);
    }

	public void destroyListeners()
    {
        SelectorManager.SelectorMoved.RemoveListener(updateOutlineFromSelectors);
        DamagePreviewManager.UpdateDamagePreviews.RemoveListener(addDamagePreview);
        HoverPanelPopUpButton.HoverPriorityRequest.RemoveListener(answerCurrentCombatantPriorityRequest);
        CombatResultsUI.OnCombatResultsUICreation.RemoveListener(disableCollider);
        HighlightAllMandatoryTargets.RemoveListener(highlightMandatoryTarget);
        StopHighlightFadeMandatoryTarget.RemoveListener(stopHighlightAndFade);
        CombatUIModule.OnHideCombatUI.RemoveListener(getTargetStats().removeOutline);
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
