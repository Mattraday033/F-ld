using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequenceStepTargetUIObject : TutorialSequenceStepTargetObject 
{

	public const bool fromButton = true;

	public Image image;
	public Color previousColor = Color.clear;

	private RectTransform cutOutMask;

	private void Awake()
	{
		if (image == null)
		{
			setImage(gameObject.GetComponent<Image>());
		}

		if (rectTransform == null)
		{
			setRectTransform(gameObject.GetComponent<RectTransform>());
		}
	}

    private void OnDisable()
    {
		destroyListeners();
    }

	public void setImage(Image image)
	{
		this.image = image;
	}

	public override Vector2 getDimensions()
	{
		return new Vector2(getRectTransform().rect.width / 2f, getRectTransform().rect.height / 2f) * PlayerMovement.getTransform().localScale * new Vector2(.05f, .05f);
	}

	public override Vector2 getPosition()
	{
		Vector2 screenCenter = RectTransformUtility.WorldToScreenPoint(Camera.main, getRectTransform().position);
		Vector2 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, Camera.main.nearClipPlane + 1f));

		return targetPosition;
	}

	private void createCutOutMask()
	{
		if (cutOutMask != null)
		{
			return;
		}

		cutOutMask = Instantiate(Resources.Load<GameObject>(PrefabNames.cutOutMask), transform).GetComponent<RectTransform>();

		if (TutorialSequence.currentTutorialSequence.getCurrentTutorialSequenceStep().blockInternalRaycastsOnCutOutMask)
		{
			cutOutMask.gameObject.GetComponent<CutOutMaskInternalBlockerManager>().turnOnInternalBlocker();
		}
	}

	public override void highlight(bool skip)
	{
		createCutOutMask();

		if(skip)
		{
			return;
		}

		if (image != null)
		{
			previousColor = image.color;
			image.color = RevealManager.tutorialDefault;
		}
	}

	public override void unhighlight(bool skip)
	{
		if (cutOutMask != null)
		{
			GameObject.DestroyImmediate(cutOutMask.gameObject);
		}

		if (skip)
		{
			return;
		}

		if (image != null)
		{
			image.color = previousColor;
		}
	}

	public override bool isUI()
	{
		return true;
	}

}
