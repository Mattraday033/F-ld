using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialSequenceStepTargetTraitRow : TutorialSequenceStepTargetUIObject 
{

	public DescriptionPanel descriptionPanel;

	public override string getTutorialHash()
	{
		Trait traitBeingDescribed = descriptionPanel.getObjectBeingDescribed() as Trait;

		return traitBeingDescribed.getName() + " Trait Icon";
	}

}
