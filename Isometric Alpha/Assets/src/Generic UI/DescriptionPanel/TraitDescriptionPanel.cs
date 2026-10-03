using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class TraitDescriptionPanel : DescriptionPanel
{
    public GameObject mandatoryTargetIcon;
    public GameObject stunnedIcon;

	public override void setObjectBeingDescribed(IDescribable describable)
	{
        base.setObjectBeingDescribed(describable);

        Trait traitBeingDescribed = describable as Trait;

        if(traitBeingDescribed == null)
        {
            return;
        }

        //set both ways, since a reused row may have shown a trait that had these
        if(stunnedIcon != null)
        {
            stunnedIcon.SetActive(traitBeingDescribed.preventsCombatAction());
        }

        if(mandatoryTargetIcon != null)
        {
            mandatoryTargetIcon.SetActive(traitBeingDescribed.isMandatoryTarget());
        }
	}
}
