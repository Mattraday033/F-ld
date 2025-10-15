using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControlTrait: Trait
{

	public CrowdControlTrait(string traitName, string traitType, string traitDescription, string traitIconName, int roundsLeft, Color traitIconBackgroundColor):
		   base(traitName, traitType, traitDescription, traitIconName, roundsLeft, traitIconBackgroundColor)
	{

	}

    public CrowdControlTrait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor) :
       base(traitName, traitType, traitDescription, traitIconName, traitIconBackgroundColor)
    {

    }

    public override bool preventsCombatAction()
	{
		return true;
	}
}
