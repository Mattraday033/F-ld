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

	public NestedDescriptionPanelMouseListener nestedDescriptionPanelMouseListener;

	private GameObject targetDisplaySelector;
	private GameObject tertiaryDisplaySelector;
	
	public GameObject arrowIndicator;

	public Image rowBackground;
    public Image[] panelSections;

    private void Awake()
    {
        HighlightRow.AddListener(setRowHighlight);
        MouseHoverManager.OnHoverPanelCreation.AddListener(removeHoverDataFromScreen);
    }

    private void OnDestroy()
    {
        HighlightRow.RemoveListener(setRowHighlight);
        MouseHoverManager.OnHoverPanelCreation.RemoveListener(removeHoverDataFromScreen);
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
		return (CombatAction) descriptionPanel.getObjectBeingDescribed();
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
		if (CombatStateManager.currentActivity == CurrentActivity.Waiting ||
			CombatStateManager.currentActivity == CurrentActivity.Retreating)
		{
			return;
		}

		CombatAction actionBeingDescribed = getCombatActionBeingDescribed();

        if(actionBeingDescribed != null &&
            actionBeingDescribed.getActorStats() != null &&
            actionBeingDescribed.getActorStats().positions.Any(p => CombatGrid.positionIsOnAlliedSide(p)))
        {
		    rowBackground.color = Color.green;
        } else
        {
		    rowBackground.color = Color.red;
        }

        CombatHoverTileManager.GetHoverSelector.AddListener(getHoverSelector);
        SelectorManager.declareSelectors();
        
		actionBeingDescribed.highlightActorSprites();
	}
 
 
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (CombatStateManager.currentActivity == CurrentActivity.Waiting ||
			CombatStateManager.currentActivity == CurrentActivity.Retreating ||
            InspectNode.inspecting)
        {
            return;
        }

		removeHoverDataFromScreen();
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
		
		actionBeingDescribed.removeHighlightFromActorSprites();

        CombatHoverTileManager.GetHoverSelector.RemoveListener(getHoverSelector);
        SelectorManager.declareSelectors();
	}
}
