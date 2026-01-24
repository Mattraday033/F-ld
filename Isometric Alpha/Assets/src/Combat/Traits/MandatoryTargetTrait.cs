using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MandatoryTargetTrait: Trait
{
    private double percentageDamageReduction = 0.0;

    public MandatoryTargetTrait(string traitName, TraitType traitType, string traitDescription, string iconName):
	base(traitName, traitType, traitDescription, iconName)
	{
		
	}	
	
	public MandatoryTargetTrait(string traitName, TraitType traitType, string traitDescription, string iconName, int roundsLeft, double percentageDamageReduction) :
	base(traitName, traitType, traitDescription, iconName, roundsLeft: roundsLeft)
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
