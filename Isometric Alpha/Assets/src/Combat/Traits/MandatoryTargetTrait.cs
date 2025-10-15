using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MandatoryTargetTrait: Trait
{
    private double percentageDamageReduction = 0.0;

    public MandatoryTargetTrait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor):
	base(traitName, traitType, traitDescription, traitIconName, traitIconBackgroundColor)
	{
		
	}	
	
	public MandatoryTargetTrait(string traitName, string traitType, string traitDescription, string traitIconName, int roundsLeft, Color traitIconBackgroundColor, double percentageDamageReduction) :
	base(traitName, traitType, traitDescription, traitIconName, roundsLeft, traitIconBackgroundColor)
	{
		this.percentageDamageReduction = percentageDamageReduction;
    }
	
	public override bool isMandatoryTarget()
	{
		return true;
	}

    public override double getPercentageDamageReduction()
    {
        return percentageDamageReduction;
    }
}
