using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableCrowdControlTrait: CrowdControlTrait
{
	public static UnityEvent<String> OnApplyingBreakableCrowdControl = new UnityEvent<string>();

    public BreakableCrowdControlTrait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor):
	base(traitName, traitType, traitDescription, traitIconName, traitIconBackgroundColor)
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
