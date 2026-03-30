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

        if(traitBeingDescribed.preventsCombatAction())
        {
            stunnedIcon.SetActive(true);
        }

        if(traitBeingDescribed.isMandatoryTarget())
        {
            mandatoryTargetIcon.SetActive(true);
        }
	}
}
