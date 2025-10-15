using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSequenceStepTargetSprite : TutorialSequenceStepTargetObject 
{
	public SpriteRenderer spriteRenderer;
	public Color previousColor = Color.white;

	public override void highlight(bool skip)
	{
		if(skip)
		{
			return;
		}
		
		previousColor = spriteRenderer.color;
		spriteRenderer.color = RevealManager.tutorialDefault;
	}
	
    public override void unhighlight(bool skip)
	{
		if(skip)
		{
			return;
		}

		spriteRenderer.color = previousColor;
	}
}
