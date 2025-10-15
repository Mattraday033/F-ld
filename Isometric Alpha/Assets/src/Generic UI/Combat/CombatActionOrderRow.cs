using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CombatActionOrderRow : GridRow, IPointerEnterHandler, IPointerExitHandler
{
	public NestedDescriptionPanelMouseListener nestedDescriptionPanelMouseListener;

	private GameObject targetDisplaySelector;
	private GameObject tertiaryDisplaySelector;
	
	public Image rowBackground;
    public Image[] panelSections;

    private CombatAction getCombatActionBeingDescribed()
	{
		return (CombatAction) descriptionPanel.getObjectBeingDescribed();
	}

    public override void setToIneligible()
    {
		foreach(Image panel in panelSections)
		{
			panel.color = ineligibleColor;
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

		rowBackground.color = Color.red;

		actionBeingDescribed.highlightActorSprites();

		if (actionBeingDescribed.getRangeIndex() >= 0)
		{
			targetDisplaySelector = (GameObject)Instantiate(actionBeingDescribed.getSelector().getSelectorObject(), CombatUI.selectorParent);

			targetDisplaySelector.transform.position = actionBeingDescribed.getTargetPosition();

			targetDisplaySelector.SetActive(true);
		}

		if (actionBeingDescribed.requiresTertiaryCoords())
		{
			tertiaryDisplaySelector = (GameObject)Instantiate(SelectorManager.getInstance().selectors[actionBeingDescribed.getRangeIndex()].getSelectorObject(), CombatUI.selectorParent);

			tertiaryDisplaySelector.transform.position = actionBeingDescribed.getTertiaryPosition();
			tertiaryDisplaySelector.GetComponent<SpriteRenderer>().color = Selector.secondaryColor;

			tertiaryDisplaySelector.SetActive(true);
		}
	}
 
 
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (CombatStateManager.currentActivity == CurrentActivity.Waiting ||
			CombatStateManager.currentActivity == CurrentActivity.Retreating)
        {
            return;
        }

		removeHoverDataFromScreen();
	}

	public void removeHoverDataFromScreen()
	{
		rowBackground.color = Color.black;
		
		CombatAction actionBeingDescribed = getCombatActionBeingDescribed();
		
		actionBeingDescribed.removeHighlightFromActorSprites();

		if(actionBeingDescribed.getRangeIndex() >= 0)
		{
			destroySelectors();
		}
	}

	public void destroySelectors()
	{
		if (targetDisplaySelector != null)
		{
			GameObject.Destroy(targetDisplaySelector);
			targetDisplaySelector = null;
		}

		if (tertiaryDisplaySelector != null)
		{
			GameObject.Destroy(tertiaryDisplaySelector);
			tertiaryDisplaySelector = null;
		}
	}
}
