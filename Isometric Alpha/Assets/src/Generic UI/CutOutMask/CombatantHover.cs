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
        if(TutorialSequence.blockMouseHovers())
        {
            return;
        }

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null && !useHoverTiles())
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

        if(TutorialSequence.blockMouseHovers())
        {
            return;
        }

        if (CombatStateManager.whoseTurn == WhoseTurn.Player && getTargetStats() != null)
        {
            if (getTargetGameObject() != null && !useHoverTiles())
            {
                onReveal(insideSelectors());
            }

            SelectorManager.displayHoverUIForCurrentSelectorTarget();
        }

        SelectorManager.updateAllDamagePreviews();
    }

    public void OnMouseOver()
    {
        if(TutorialSequence.blockMouseHovers())
        {
            return;
        }

        if(AbilityMenuButton.hoveringOverAbilityMenuButton)
        {
            getTargetStats().removeOutline();
        } else
        {
            onReveal(Constants.reveal);
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

        if(revealPriorityHeld)
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
        return getTargetStats().outline;
    }
    
	public void createListeners()
    {
        SelectorManager.SelectorMoved.AddListener(updateOutlineFromSelectors);
        DamagePreviewManager.UpdateDamagePreviews.AddListener(addDamagePreview);
    }

	public void destroyListeners()
    {
        SelectorManager.SelectorMoved.RemoveListener(updateOutlineFromSelectors);
        DamagePreviewManager.UpdateDamagePreviews.RemoveListener(addDamagePreview);
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
