using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableCrowdControlTrait: CrowdControlTrait
{
	public readonly static UnityEvent<string> OnApplyingBreakableCrowdControl = new UnityEvent<string>();

    public BreakableCrowdControlTrait(string traitName, TraitType traitType, string traitDescription, string iconName):
	base(traitName, traitType, traitDescription, iconName)
	{
		OnApplyingBreakableCrowdControl.AddListener(breakCrowdControl);
    }

    public override void onApplication()
    {
		OnApplyingBreakableCrowdControl.Invoke(getName());
    }

    private void breakCrowdControl(string appliedCrowdControlTraitName)
	{
		if(appliedCrowdControlTraitName.Equals(getName()) && getTraitHolder() != null)
		{
			getTraitHolder().removeTrait(this);
			OnApplyingBreakableCrowdControl.RemoveListener(breakCrowdControl);
        }
	}
    public override bool isRemovedOnDamage()
    {
        return true;
    }
}
