using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfTargetAOEOnDeathTrait : OnDeathEffectTrait
{
	private bool thisTraitPreventsResurrection;
	
	public SelfTargetAOEOnDeathTrait(string traitName, string traitDescription, string iconName, string abilityKey, bool thisTraitPreventsResurrection = true, TargetPriorityTrait targetPriority = null):
	base(traitName, traitDescription, iconName, abilityKey, null)
	{
        this.thisTraitPreventsResurrection = thisTraitPreventsResurrection;

        if(thisTraitPreventsResurrection)
        {
            this.deleteIfIsDead = true;
        }

        if(targetPriority != null)
        {
            this.targetPriority = targetPriority;
        }
	}
	
	public override bool preventsResurrection()
	{
		return thisTraitPreventsResurrection;
	}
}
