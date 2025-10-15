using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDescriptionPanel : DescriptionPanel
{

	public RectTransform parentRectTransform;
	public RectTransform speakerNameRect;
	public RectTransform dialogueContentsRect;

	//LayoutRebuilder.ForceRebuildLayoutImmediate(containerRectTransform)

	public void updateSize()
	{
		parentRectTransform.sizeDelta = new Vector2 (parentRectTransform.sizeDelta.x,
													 dialogueContentsRect.sizeDelta.y + 10);
		
		/*
		parentRectTransform.rect = new Rect(parentRectTransform.rect.x,
											parentRectTransform.rect.y,
											parentRectTransform.rect.width,
											dialogueContentsRect.y+10f);
		*/
		LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);
	}


}
