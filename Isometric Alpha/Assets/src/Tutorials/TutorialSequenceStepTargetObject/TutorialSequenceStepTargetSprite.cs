using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSequenceStepTargetSprite : TutorialSequenceStepTargetObject 
{
    public IRevealable revealable;
	public SpriteRenderer spriteRenderer;
    public SpriteOutline spriteOutline;
	public Color previousColor = Color.white;

    public override void highlight(bool skip)
    {
        if (skip)
        {
            return;
        }

        if(spriteOutline == null)
        {
            spriteOutline = new SpriteOutline();
            spriteOutline.setSpriteRenderer(spriteRenderer);
        }

        if(revealable != null)
        {
            spriteOutline.createOutline(revealable.getRevealColor());
        } else
        {
            spriteOutline.createOutline(ColorList.tutorialDefault);
        }
    }
	
    public override void unhighlight(bool skip)
	{
		if(skip || spriteOutline == null)
		{
			return;
		}

        spriteOutline.removeOutline();
	}
}
