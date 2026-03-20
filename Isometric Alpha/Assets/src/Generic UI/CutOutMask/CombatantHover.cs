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

    public bool revealPriorityHeld = false;
    public Stats linkedStats;
    private List<Selector> selectors = new List<Selector>();

    private Coroutine highlightAndFadeCoroutine;

    private void stopHighlightAndFade(Stats stats)
    {
        if(stats == linkedStats)
        {
            StopCoroutine(highlightAndFadeCoroutine);
            highlightAndFadeCoroutine = null;
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

    private IEnumerator highlightAndFade()
    {
        getTargetStats().setOutline();

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

            getTargetStats().setOutline((byte) Mathf.Lerp(254f, 0f, timeWaited/timeToWaitFade));
        }

        getTargetStats().removeOutline();

    }

    public void OnMouseEnter()
    {
        if(TutorialSequence.blockMouseHovers() || 
            AbilityMenuButton.hoveringOverAbilityMenuButton || 
            CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null && !getTargetStats().isDead() && !useHoverTiles())
        {
            revealPriorityHeld = true;

            onReveal(Constants.reveal);

            CombatActionOrderRow.HighlightRow.Invoke(getTargetStats(), true);

            createHoverTag();

            SelectorManager.updateAllDamagePreviews();
        }
    }

    public void OnMouseExit()
    {
        revealPriorityHeld = false;

        if(TutorialSequence.blockMouseHovers() || 
            CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null && !getTargetStats().isDead())
        {
            if (getTargetGameObject() != null && !useHoverTiles())
            {
                onReveal(insideSelectors());
            }

            SelectorManager.displayCurrentHoverUI();

            CombatActionOrderRow.HighlightRow.Invoke(getTargetStats(), false);
        }

        SelectorManager.updateAllDamagePreviews();
    }

    public static bool expectedHoveringOverAbilityMenuButtonFlagState = false;

    public static void setExpectedFlagState()
    {
        expectedHoveringOverAbilityMenuButtonFlagState = AbilityMenuButton.hoveringOverAbilityMenuButton;
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
        return linkedStats.position;
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
        if (CombatGrid.positionIsOnAlliedSide(getTargetStats().position))
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

            if(linkedStats.isInsideCoordinates(gridCoords))
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

    private void answerCurrentCombatantPriorityRequest()
    {
        if(revealPriorityHeld)
        {
            HoverPanelPopUpButton.currentCombatantWithPriority = linkedStats;
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
