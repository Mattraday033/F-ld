using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TutorialSequenceStepTargetItemRow : TutorialSequenceStepTargetRow
{

	public override string getTutorialHash()
	{
		Item itemBeingDescribed = descriptionPanel.getItemBeingDescribed();

		if (itemBeingDescribed.isEquippable())
		{
			return itemBeingDescribed.getSubtype() + itemBeingDescribed.getSlotIconName();
		}

		return itemBeingDescribed.getSubtype();
	}

}
