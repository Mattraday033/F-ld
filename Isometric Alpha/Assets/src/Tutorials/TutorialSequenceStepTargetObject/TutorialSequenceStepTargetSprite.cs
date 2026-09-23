using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSequenceStepTargetSprite : TutorialSequenceStepTargetObject 
{
    public IRevealable revealable;
	public SpriteLayerRendererList rendererList;
    private Color previousColor = Color.clear;

    public override void highlight(bool skip)
    {
        if (skip || rendererList == null)
        {
            return;
        }

        previousColor = rendererList.getOutlineColor();

        rendererList.createOutline(ColorList.tutorialDefault);
    }
	
    public override void unhighlight(bool skip)
	{
		if(skip || rendererList == null)
		{
			return;
		}

        rendererList.createOutline(previousColor);
	}
}
