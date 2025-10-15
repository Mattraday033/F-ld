using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrackerWindow : PopUpWindow, IEscapable
{
	public ScrollableUIElement dialogueGrid;
	public ScrollableUIElement choicesGrid;

	private static DialogueTrackerWindow instance;

	public static DialogueTrackerWindow getInstance()
	{
		return instance;
	}

	private void Awake()
	{
		if (instance != null)
		{
			throw new IOException("Duplicate instances of DialogueTrackerWindow exist");
		}

		instance = this;
	}

	public void populateDialogue()
	{
		dialogueGrid.populatePanels(SpeechLog.getDialogueList());
	}

	public void populateDialogue(Conversation conversation)
	{
		dialogueGrid.populatePanels(conversation.getDialogueList());
	}

	public void populateDialogue(ArrayList dialogueList)
	{
		dialogueGrid.populatePanels(dialogueList);
	}

	public void appendDialogue(ArrayList dialogueList)
	{
		dialogueGrid.appendPanels(dialogueList);
	}

	public void populateChoices(ArrayList choicesList)
	{
		choicesGrid.populatePanels(choicesList);
	}

	public override void closeButtonPress()
	{
		base.closeButtonPress();

		PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
    }

	/*
	public void setDialogueScrollRectToBottom(ScrollRect scrollRect)
	{
		Canvas.ForceUpdateCanvases();

		scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();

		scrollRect.verticalNormalizedPosition = 0;	
	}
	*/

}
