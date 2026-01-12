using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GridRow : MonoBehaviour,
	IPointerEnterHandler,
	IPointerExitHandler,
    IHoverIconSource
{
    public readonly static UnityEvent<IDescribable> OnDescribableToDisplay = new UnityEvent<IDescribable>();

	public bool hoverEnabled;
    public bool useFloatingHover;

	public RowType rowTypeToUse = RowType.Standard;

	public int tabCollection;
	private ScrollableUIElement parentGrid;

	public Button[] buttons;

	public Button nameButton;

	public DescriptionPanel descriptionPanel;

	public bool wipeIfIneligible;
	public TextMeshProUGUI[] buttonTexts;

    public void setParentGrid(ScrollableUIElement grid)
    {
        this.parentGrid = grid;
    }

	public virtual void displayDescribable()
	{
		if (OverallUIManager.currentScreenManager == null)
		{
			OverallUIManager.setCurrentScreenType(SaveHandler.getInstance());
		}

		OnDescribableToDisplay.Invoke(descriptionPanel.getObjectBeingDescribed());
	}

	public virtual void setToIneligible()
	{
		foreach (Button button in buttons)
		{
			ColorBlock colors = button.colors;
			colors.normalColor = ColorList.ineligibleColor;
			button.colors = colors;

			Image buttonGraphic = (Image)button.targetGraphic;
			buttonGraphic.color = ColorList.ineligibleColor;

			if (wipeIfIneligible)
			{
				button.enabled = false;
			}
		}

		if (wipeIfIneligible)
		{
			foreach (TextMeshProUGUI buttonText in buttonTexts)
			{
				buttonText.text = "";
			}
		}
	}

	public virtual void setToAlternate()
	{
		foreach (Button button in buttons)
		{
			ColorBlock colors = button.colors;
			colors.normalColor = ColorList.alternateRowColor;
			button.colors = colors;

			Image buttonGraphic = (Image)button.targetGraphic;
			buttonGraphic.color = ColorList.alternateRowColor;
		}
	}

	public virtual void onDestruction()
	{
		//Empty on purpose
	}

	public void disableButtons()
	{
		parentGrid.enableAllGridRows();

		foreach (Button button in buttons)
		{
			button.interactable = false;
		}

		//if(buttonTexts.Length > 0)
		//{
		//     Debug.Log("Buttons Disabled on " + buttonTexts[0].text);
		//}
	}

	public void enableButtons()
	{
		foreach (Button button in buttons)
		{
			button.interactable = true;
		}
	}

    public virtual bool canSeeHover()
    {
        return hoverEnabled && PlayerOOCStateManager.currentActivity != OOCActivity.inTutorialSequence;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!canSeeHover() || eventData.used)
        {
            return;
        }

        MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldSpawnHoverIcon));
    }

	public virtual void OnPointerExit(PointerEventData eventData)
	{
        // MouseHoverManager.OnHoverPanelCreation.Invoke();

		if (!canSeeHover())
        {
            return;
        }

        MouseHoverManager.startCoroutine(this, MouseHoverManager.waitToHandleDescriptionPanel(this, MouseHoverManager.shouldDestroyHoverIcon));
	}

    public GameObject getDescriptionPanelType()
    {
        return Resources.Load<GameObject>(PrefabNames.hoverIconCombatActionDescriptionPanel);    
    }

    public IDescribable getObjectBeingDescribed()
    {
        return descriptionPanel.getObjectBeingDescribed();
    }

    public void spawnHoverIcon()
    {
        // if (useFloatingHover)
        // {
            MouseHoverManager.spawnHoverIcon(this, transform);
        // }
        // else
        // {
        //     useDedicatedSlot();
        // }
    }

    // private void useDedicatedSlot()
    // {
    //     if (PopUpWindow.currentPopUpDescriptionPanelSlot != null)
    //     {
    //         PopUpWindow.currentPopUpDescriptionPanelSlot.setTempDescribable(descriptionPanel.getObjectBeingDescribed());
    //     }
    //     else if(OverallUIManager.currentScreenManager != null)
    //     {
    //         OverallUIManager.currentScreenManager.revealTemptDescriptionPanelSet(descriptionPanel.getObjectBeingDescribed(), tabCollection);
    //     }
    // }

    public void destroyHoverIcon()
    {
        // if (useFloatingHover)
        // {
            MouseHoverManager.destroyHoverIcon();
        // }
        // else
        // {
            // revertDedicatedSlot();
        // }
    }

    // private void revertDedicatedSlot()
    // {
    //     if (PopUpWindow.currentPopUpDescriptionPanelSlot != null)
    //     {
    //         List<IDescribable> list = PopUpWindow.currentPopUpDescriptionPanelSlot.getCurrentDescribables();

    //         if (list[0] == descriptionPanel.getObjectBeingDescribed())
    //         {
    //             PopUpWindow.currentPopUpDescriptionPanelSlot.revertToPrimaryDescribable();
    //         }
    //     }
    //     else if(OverallUIManager.currentScreenManager != null &&
    //             OverallUIManager.currentScreenManager.descriptionPanelSlots.Count > tabCollection &&
    //             OverallUIManager.currentScreenManager.descriptionPanelSlots[tabCollection] != null)
    //     {
    //         List<IDescribable> list = OverallUIManager.currentScreenManager.descriptionPanelSlots[tabCollection].getCurrentDescribables();

    //         if (list != null && descriptionPanel != null && list[0] == descriptionPanel.getObjectBeingDescribed())
    //         {
    //             OverallUIManager.currentScreenManager.hideTempDescriptionPanelSet(tabCollection);
    //         }
    //     }
    // }

}
