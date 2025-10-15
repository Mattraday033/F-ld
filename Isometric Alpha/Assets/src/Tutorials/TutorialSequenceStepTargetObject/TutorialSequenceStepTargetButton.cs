using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequenceStepTargetButton : TutorialSequenceStepTargetUIObject 
{

	public static Button currentButton;

	//[SerializeField]
	public bool advanceSequenceOnButtonPress = false;
	public Button buttonTarget;

	public virtual bool advanceSequenceWithButtonPress()
	{
		return advanceSequenceOnButtonPress;
	}

	private void Awake()
	{
		setRectTransform(gameObject.GetComponent<RectTransform>());
		image = gameObject.GetComponent<Image>();
	}

	public override void createListeners()
	{
		base.createListeners();
		buttonTarget.onClick.AddListener(onButtonPress);
	}

	public override void destroyListeners()
	{
		// Debug.LogError("currentButton == buttonTarget = " + (currentButton == buttonTarget));
		if (currentButton == buttonTarget)
		{
			onButtonPress();
		}

		base.destroyListeners();
		buttonTarget.onClick.RemoveListener(onButtonPress);
	}

	private void onButtonPress()
	{
		if (!TutorialSequence.currentlyInTutorialSequence())
		{
			return;
		}

		if (advanceSequenceWithButtonPress())
		{
			TutorialSequence.advanceCurrentTutorialSequence(fromButton);
		}
	}

	public override void assignToTutorialSequence(TutorialSequenceStep tutorialSequenceStep)
	{
		if (tutorialSequenceStep.isTutorialTarget(getTutorialHash()))
		{
			if (EscapeStack.getEscapableObjectsCount() <= 0)
			{
				PopUpBlocker.destroyPopUpScreenBlocker();
			}

			addToHashDictionary(this);

			tutorialSequenceStep.createMessageWindowAndRunScript(getTutorialHash(), useUltraWideTutorialWindow, disableArrow);
		}
	}

	public override void highlight(bool skip)
	{
		base.highlight(skip);

		advanceSequenceOnButtonPress = true;

		currentButton = buttonTarget;

		currentButton.interactable = true;
	}

	public override void unhighlight(bool skip)
	{
		base.unhighlight(skip);

		advanceSequenceOnButtonPress = false;
		currentButton = null;
	}
}
