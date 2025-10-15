using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialSequenceStepTargetMultiButton : TutorialSequenceStepTargetUIObject 
{
	//[SerializeField]
	private bool advanceSequenceOnButtonPress = false;
	public Button[] buttonTargets;

	public override void createListeners()
	{
		base.createListeners();

		foreach (Button buttonTarget in buttonTargets)
		{
			buttonTarget.onClick.AddListener(onButtonPress);
		}
	}

	public override void destroyListeners()
	{
		base.destroyListeners();

		//if ever this TutorialSequenceStepTarget isn't firing onButtonPress(), it may be because the buttons are destroyed before they
		//onButtonPress() can be called.
		foreach (Button buttonTarget in buttonTargets)
		{
			buttonTarget.onClick.RemoveListener(onButtonPress);
		}
	}

	private void onButtonPress()
	{

		if (!TutorialSequence.currentlyInTutorialSequence())
		{
			return;
		}

		if (advanceSequenceOnButtonPress)
			{
				TutorialSequence.advanceCurrentTutorialSequence(fromButton);
			}
	}

	public override void assignToTutorialSequence(TutorialSequenceStep tutorialSequenceStep)
	{
		if (tutorialSequenceStep.isTutorialTarget(getTutorialHash()))
		{
			PopUpBlocker.destroyPopUpScreenBlocker();
			addToHashDictionary(this);
			tutorialSequenceStep.createMessageWindowAndRunScript(getTutorialHash(), useUltraWideTutorialWindow, disableArrow);
		}
	}

	public override void highlight(bool skip)
	{
		base.highlight(skip);

		advanceSequenceOnButtonPress = true;
		// Debug.LogError("advanceSequenceOnButtonPress = " + advanceSequenceOnButtonPress	);
		
		Button filledButton = getFilledButton();

		foreach (Button button in buttonTargets)
		{
			if (button != filledButton)
			{
				button.interactable = true;
			}
			else
			{
				button.interactable = false;
			}
		}
	}

	public override void unhighlight(bool skip)
	{
		base.unhighlight(skip);

		advanceSequenceOnButtonPress = false;

		foreach (Button button in buttonTargets)
		{
			button.interactable = true;
		}
	}

	public Button getFilledButton()
	{
		foreach (Button button in buttonTargets)
		{
			Transform buttonTextTransform = button.transform.GetChild(0);

			string buttonText = buttonTextTransform.GetComponent<TextMeshProUGUI>().text;

			if (buttonText.Length > 0)
			{
				return button;
			}
		}

		return null;
	}
}
