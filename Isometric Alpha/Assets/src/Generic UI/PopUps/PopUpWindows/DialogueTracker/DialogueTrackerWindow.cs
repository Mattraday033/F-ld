using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrackerWindow : PopUpWindow, IEscapable
{
    public bool doNoBold = false;

    public GameObject speakerPortraitFrame;
    public Image speakerPortrait;
    public GameObject playerPortraitFrame;
    public Image playerPortrait;

	public ScrollableUIElement dialogueGrid;
	public ScrollableUIElement choicesGrid;

	private static DialogueTrackerWindow instance;

	public static DialogueTrackerWindow getInstance()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;

        if(PlayerOOCStateManager.currentActivity == OOCActivity.inDialoguePopUp)
        {
            TutorialSequenceStepTargetUIObject.createCutOutMask(transform);
        }

        setPlayerPortrait();
	}

    public void setPlayerPortrait()
    {
        if(playerPortraitFrame != null && playerPortrait != null)
        {
            playerPortraitFrame.SetActive(true);
            playerPortrait.sprite = PartyMember.getPortrait(State.playerPortraitName);
        }
    }

	public void populateDialogue()
	{
		populateDialogue(SpeechLog.getDialogueList());
	}

	public void populateDialogue(Conversation conversation)
	{
		populateDialogue(conversation.getDialogueList());
	}

	public void populateDialogue(List<DialogueLine> dialogueList)
	{
        updateSpeakerPortrait(dialogueList);

        boldFirstLine(dialogueList);

		dialogueGrid.populatePanels(dialogueList);
	}

	public void appendDialogue(List<DialogueLine> dialogueList)
	{

        updateSpeakerPortrait(dialogueList);

        boldFirstLine(dialogueList);

		dialogueGrid.appendPanels(dialogueList);
	}

	public void populateChoices(List<ChoiceDescription> choicesList)
	{
		choicesGrid.populatePanels(choicesList);
	}

    public void updateSpeakerPortrait(List<DialogueLine> dialogueList)
    {
        if(dialogueList == null || 
            dialogueList.Count <= 0 || 
            Conversation.nameIsUpdate(dialogueList[0].speakerName))
        {
            return;
        }

        if(speakerPortraitFrame != null && speakerPortrait != null)
        {
            Sprite portrait = PartyMember.getPortrait(dialogueList[0].speakerName, allowNull: true);

            if(portrait == null)
            {
                speakerPortraitFrame.SetActive(false);
                return;
            }

            speakerPortraitFrame.SetActive(true);
            speakerPortrait.sprite = portrait;
        }
    }

	public override void closeButtonPress()
	{
		base.closeButtonPress();

		PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
    }

    private void boldFirstLine(List<DialogueLine> dialogueList)
    {
        if(doNoBold)
        {
            return;
        }

        if(dialogueList != null && dialogueList.Count > 0 && 
            dialogueList[0].isBoldable())
        {
            DialogueLine.UnboldAllText.Invoke();

            dialogueList[0].boldText();
        }
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
