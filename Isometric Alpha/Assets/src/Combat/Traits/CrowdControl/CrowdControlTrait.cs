using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControlTrait: Trait
{

	public CrowdControlTrait(string traitName, TraitType traitType, string traitDescription, string iconName, int roundsLeft):
		   base(traitName, traitType, traitDescription, iconName, roundsLeft: roundsLeft)
	{

	}

    public CrowdControlTrait(string traitName, TraitType traitType, string traitDescription, string iconName) :
       base(traitName, traitType, traitDescription, iconName)
    {

    }

    public override bool preventsCombatAction()
	{
		return true;
	}
}
