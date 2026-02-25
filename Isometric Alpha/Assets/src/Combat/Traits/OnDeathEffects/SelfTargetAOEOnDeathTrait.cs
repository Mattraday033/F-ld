using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfTargetAOEOnDeathTrait : OnDeathEffectTrait
{
	private bool thisTraitPreventsResurrection;
	
	public SelfTargetAOEOnDeathTrait(string traitName, string traitDescription, string iconName, string abilityKey, bool thisTraitPreventsResurrection = true):
	base(traitName, traitDescription, iconName, abilityKey, null)
	{
        this.thisTraitPreventsResurrection = thisTraitPreventsResurrection;

        if(thisTraitPreventsResurrection)
        {
            this.deleteIfIsDead = true;
        }
	}
	
	public override bool preventsResurrection()
	{
		return thisTraitPreventsResurrection;
	}
}
