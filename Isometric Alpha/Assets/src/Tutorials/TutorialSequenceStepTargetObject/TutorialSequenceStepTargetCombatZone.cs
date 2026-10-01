using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequenceStepTargetCombatZone : TutorialSequenceStepTargetObject 
{

	public RectTransform cutOutMask;

	public override void highlight(bool skip)
	{
        if(cutOutMask == null)
        {
            return;
        }

		cutOutMask.gameObject.SetActive(true);
        
        PlayerStateManager.OnLeavingTutorialSequenceState.AddListener(unhighlight);
	}

	public override void unhighlight(bool skip)
	{
        if(cutOutMask == null)
        {
            return;
        }

		cutOutMask.gameObject.SetActive(false);
        PlayerStateManager.OnLeavingTutorialSequenceState.RemoveListener(unhighlight);
	}
}
