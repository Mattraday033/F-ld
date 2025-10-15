using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneOfInfluenceTrait : Trait
{
	private const string zoneOfInfluenceTraitType = "Influence";
	
	public ZoneOfInfluenceTrait(string traitName, string traitDescription, string iconBackgroundName, string[] statBoostKeys): base(traitName, zoneOfInfluenceTraitType, traitDescription, iconBackgroundName, Color.black)
	{
		this.statBoostKeys = statBoostKeys;
	}
	
	public override bool fromZoneOfInfluence()
	{
		return true;
	}
}
