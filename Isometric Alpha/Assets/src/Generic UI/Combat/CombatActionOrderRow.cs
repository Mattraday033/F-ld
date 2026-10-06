using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CombatActionOrderRow : GridRow, IPointerEnterHandler, IPointerExitHandler
{
    public readonly static UnityEvent<Stats, bool> HighlightRow = new UnityEvent<Stats, bool>();

    public readonly static UnityEvent<Stats> HoldRevealPriority = new UnityEvent<Stats>();
    public readonly static UnityEvent<Stats> ReleaseRevealPriority = new UnityEvent<Stats>();

    //covers every actor of the action, where the reveal priority only goes to the assigned one
    public readonly static UnityEvent<CombatAction> HoldActorOutlines = new UnityEvent<CombatAction>();
    public readonly static UnityEvent<CombatAction> ReleaseActorOutlines = new UnityEvent<CombatAction>();

    public readonly static UnityEvent OnPointerEnterCombatActionOrderRow = new UnityEvent();
    public readonly static UnityEvent OnPointerExitCombatActionOrderRow = new UnityEvent();

	public NestedDescriptionPanelMouseListener nestedDescriptionPanelMouseListener;

	public GameObject arrowIndicator;

	public Image rowBackground;
    public Image[] panelSections;

    private void Awake()
    {
        HighlightRow.AddListener(setRowHighlight);
        // MouseHoverManager.OnHoverPanelCreation.AddListener(removeHoverDataFromScreen);
    }

    private void OnDestroy()
    {
        HighlightRow.RemoveListener(setRowHighlight);
        // MouseHoverManager.OnHoverPanelCreation.RemoveListener(removeHoverDataFromScreen);
    }

    public void setRowHighlight(Stats actor, bool highlightRow)
    {
        if(actor == null)
        {
            return;
        }

        CombatAction actionBeingDescribed = getCombatActionBeingDescribed();

        if(highlightRow && actionBeingDescribed != null && actionBeingDescribed.actorIsPartOfAction(actor))
        {
            if(actor.positions.Any(p => CombatGrid.positionIsOnAlliedSide(p)))
            {
                rowBackground.color = Color.green;
            } else
            {
                rowBackground.color = Color.red;
            }

            arrowIndicator.SetActive(true);
        } else
        {
            rowBackground.color = Color.white;
            arrowIndicator.SetActive(false);
        }
    }

    private CombatAction getCombatActionBeingDescribed()
	{
		return descriptionPanel.getObjectBeingDescribed() as CombatAction;
	}

    public override void setToIneligible()
    {
		foreach(Image panel in panelSections)
		{
			panel.color = ColorList.ineligibleColor;
        }
    }

	public override void onDestruction()
	{
		removeHoverDataFromScreen();
		nestedDescriptionPanelMouseListener.destroyAllDescriptionPanels();
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{

		CombatAction actionBeingDescribed = getCombatActionBeingDescribed();

		if (PlayerStateManager.currentActivity == CurrentActivity.Waiting ||
			PlayerStateManager.currentActivity == CurrentActivity.Retreating ||
            actionBeingDescribed == null)
		{
			return;
		}

        OnPointerEnterCombatActionOrderRow.Invoke();

        if(actionBeingDescribed.hasAssignedActor(out Stats actor) &&
            actor.positions.Any(p => CombatGrid.positionIsOnAlliedSide(p)))
        {
		    rowBackground.color = Color.green;
        } else
        {
		    rowBackground.color = Color.red;
        }

        HoldRevealPriority.Invoke(actor);
        HoldActorOutlines.Invoke(actionBeingDescribed);

        CombatHoverTileManager.GetHoverSelector.AddListener(getHoverSelector);
        SelectorManager.declareSelectors();
        
		actionBeingDescribed.highlightActorSprites();
	}
 
 
    public override void OnPointerExit(PointerEventData eventData)
    {
		CombatAction actionBeingDescribed = getCombatActionBeingDescribed();

        if (PlayerStateManager.currentActivity == CurrentActivity.Waiting ||
			PlayerStateManager.currentActivity == CurrentActivity.Retreating ||
            actionBeingDescribed == null || 
            !actionBeingDescribed.hasAssignedActor(out Stats actor))
        {
            return;
        } else if(InspectNode.inspecting)
        {
            StartCoroutine(waitForInspectingToEnd());
            return;
        }

        ReleaseRevealPriority.Invoke(actor);

        OnPointerExitCombatActionOrderRow.Invoke();

		removeHoverDataFromScreen();
	}

    private IEnumerator waitForInspectingToEnd()
    {
        while(InspectNode.inspecting)
        {
            yield return null;
        }

        OnPointerExit(null);
    }

    public void getHoverSelector(SelectorContainer container)
    {
        CombatAction combatAction = getCombatActionBeingDescribed();

        if(combatAction == null)
        {
            return;
        }

        Selector hoverSelector = combatAction.getSelector().clone();

        hoverSelector.alwaysRed = true;

        container.selector = hoverSelector;
    }

	public void removeHoverDataFromScreen()
	{
        if (InspectNode.inspecting)
        {
            return;
        }

		rowBackground.color = Color.white;
		
		CombatAction actionBeingDescribed = getCombatActionBeingDescribed();

        ReleaseActorOutlines.Invoke(actionBeingDescribed);

		actionBeingDescribed.removeHighlightFromActorSprites();

        CombatHoverTileManager.GetHoverSelector.RemoveListener(getHoverSelector);
        SelectorManager.declareSelectors();
	}
}
