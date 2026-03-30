using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PositioningType{Frontline, Backline, Random}

public class PositioningTrait : Trait
{
	private PositioningType positioningType;
	
	public PositioningTrait(string traitName, TraitType traitType, string traitDescription, string iconName, PositioningType positioningType):
	base(traitName, traitType, traitDescription, iconName)
	{
		this.positioningType = positioningType;
	}
	
	public override bool stackInFront()
	{
        return positioningType == PositioningType.Frontline;
	}
	
	public override bool stackInBack()
	{
		return positioningType == PositioningType.Backline;
	}

    public override bool isHiddenTrait()
    {
        return true;
    }

}
