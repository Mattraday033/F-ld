using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpWindow : MonoBehaviour, IEscapable
{
	public static DescriptionPanelSlot currentPopUpDescriptionPanelSlot;

	public DescriptionPanelSlot descriptionPanelSlot;

	private void OnEnable()
	{
		currentPopUpDescriptionPanelSlot = descriptionPanelSlot;
	}

    private void OnDisable()
    {
		currentPopUpDescriptionPanelSlot = null;
    }

    public Button acceptButton;
	public Button closeButton;
	
	public PopUpButton popupProgenitor;
	
	public virtual void setProgenitor(PopUpButton popupProgenitor)
	{
		this.popupProgenitor = popupProgenitor;
	}
	
	public virtual void destroyWindow()
	{
        popupProgenitor.destroyPopUp();
	}

	public virtual void acceptButtonPress()
	{
		EscapeStack.handleEscapePress();
	}

	public virtual void closeButtonPress()
	{
		EscapeStack.handleEscapePress();
	}

	public virtual void handleEscapePress()
	{
        destroyWindow();		
	}
}
