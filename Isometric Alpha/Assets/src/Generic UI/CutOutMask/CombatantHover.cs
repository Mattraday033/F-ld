using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatantHover : CombatMouseHover, IRevealable
{
    public bool revealPriorityHeld = false;
    public Stats linkedStats;
    private List<Selector> selectors = new List<Selector>();

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
        return getTargetStats().outline;
    }
    
	public void createListeners()
    {
        SelectorManager.SelectorMoved.AddListener(updateOutlineFromSelectors);
        DamagePreviewManager.UpdateDamagePreviews.AddListener(addDamagePreview);
        HoverPanelPopUpButton.HoverPriorityRequest.AddListener(answerCurrentCombatantPriorityRequest);
    }

	public void destroyListeners()
    {
        SelectorManager.SelectorMoved.RemoveListener(updateOutlineFromSelectors);
        DamagePreviewManager.UpdateDamagePreviews.RemoveListener(addDamagePreview);
        HoverPanelPopUpButton.HoverPriorityRequest.RemoveListener(answerCurrentCombatantPriorityRequest);
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
