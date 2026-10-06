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
		//a button inside the prebuilt map window is still listening while the map is hidden
		if (!onScreen())
		{
			return;
		}

		if (tutorialSequenceStep.isTutorialTarget(getTutorialHash()))
		{
			if (EscapeStack.getEscapableObjectsCount() <= 0)
			{
				PopUpScreenBlockerManager.destroyPopUpScreenBlocker();
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

        if(skip)
        {
            PlayerStateManager.OnLeavingTutorialSequenceState.AddListener(unhighlight);
        }
	}

	public override void unhighlight(bool skip)
	{
		base.unhighlight(skip);

		advanceSequenceOnButtonPress = false;
		currentButton = null;
        
        PlayerStateManager.OnLeavingTutorialSequenceState.RemoveListener(unhighlight);
	}
}
