using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequenceStepTargetCombatZone : TutorialSequenceStepTargetObject 
{

	public RectTransform cutOutMask;

	public override void highlight(bool skip)
	{
		cutOutMask.gameObject.SetActive(true);
	}

	public override void unhighlight(bool skip)
	{
		cutOutMask.gameObject.SetActive(false);
	}
}
