using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfTargetAOEOnDeathTrait : OnDeathEffectTrait
{
	private bool thisTraitPreventsResurrection;
	
	public SelfTargetAOEOnDeathTrait(string traitName, string traitType, string traitDescription, string traitIconName, string abilityKey):
	base(traitName, traitType, traitDescription, traitIconName, Color.black, abilityKey, null)
	{
		this.thisTraitPreventsResurrection = false;
	}
	
	public SelfTargetAOEOnDeathTrait(string traitName, string traitType, string traitDescription, string traitIconName, string abilityKey, bool thisTraitPreventsResurrection):
	base(traitName, traitType, traitDescription, traitIconName, Color.black, abilityKey, null)
	{
		this.thisTraitPreventsResurrection = thisTraitPreventsResurrection;
		this.deleteIfIsDead = true;
	}
	
	public override bool preventsResurrection()
	{
		return thisTraitPreventsResurrection;
	}
}
