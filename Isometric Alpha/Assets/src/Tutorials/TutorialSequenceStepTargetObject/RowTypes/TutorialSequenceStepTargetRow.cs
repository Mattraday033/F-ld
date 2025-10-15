using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialSequenceStepTargetRow : TutorialSequenceStepTargetButton 
{
	public DescriptionPanel descriptionPanel;

	public static UnityEvent<bool> ChangeButtonInteractivity = new UnityEvent<bool>();
	public Button buttonToDisable;

	public static bool disableButtons = false;

	private void Awake()
	{
		// if (!TutorialSequence.currentlyInTutorialSequence())
		// {
		// 	enabled = false;
		// 	return;
		// }

		if (image == null)
		{
			image = gameObject.GetComponent<Image>();
		}

		if (rectTransform == null)
		{
			setRectTransform(gameObject.GetComponent<RectTransform>());
		}

		if (buttonToDisable != null)
		{
			buttonToDisable.enabled = !advanceSequenceWithButtonPress();
		}
	}

	public override bool advanceSequenceWithButtonPress()
	{
		if(!TutorialSequence.currentlyInTutorialSequence())
		{
			return false;
		}

		TutorialSequence currentTutorialSequence = TutorialSequence.currentTutorialSequence;

		return !currentTutorialSequence.inFinalStep();
	}

	public override void createListeners()
	{
		base.createListeners();
		ChangeButtonInteractivity.AddListener(onChangeButtonInteractivity);
	}

	public override void destroyListeners()
	{
		base.destroyListeners();
		ChangeButtonInteractivity.RemoveListener(onChangeButtonInteractivity);
		removeFromHashDictionary(this);
	}

	private void onChangeButtonInteractivity(bool enabled)
	{
		if (buttonToDisable != null)
		{
			buttonToDisable.enabled = enabled;
		}
	}
}
