using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TargetPriorityTrait : Trait 
{
	public const string initialTraitType = "Target Priority";
	
	public TargetPriorityTrait(string traitName, string traitDescription, string traitIconName, Color traitIconBackgroundColor): 
	base(traitName, initialTraitType, traitDescription, traitIconName, traitIconBackgroundColor)
	{
		
	}
	
	public override bool isMandatoryTrait()
	{
		return true;
	}
	
	public virtual bool targetsOnlyEmptySpace()
	{
		return false;
	}
	
	public virtual bool targetsGround()
	{
		return false;
	}
	
	public virtual bool deterministic()
	{
		return true;
	}
	
	public override Selector findTargetLocation(Selector selector, ArrayList listOfTargets)
	{
		Selector selectorOnTarget = selector.clone();
		Stats mandatoryTarget = getMandatoryTarget(listOfTargets);
		
		selectorOnTarget.setToLocation(mandatoryTarget.position);
		
		if(!selectorOnTarget.allTilesAreLegal() || !selectorOnTarget.containsTarget(mandatoryTarget))
		{
			GridCoords legalCoordsContainingTarget = SelectorManager.findLegalCoordsContainingMandatoryTarget(selectorOnTarget, mandatoryTarget);

			selectorOnTarget.setToLocation(new GridCoords(legalCoordsContainingTarget.row, legalCoordsContainingTarget.col));
		}
		
		if(selectorOnTarget.currentRow < 0 || selectorOnTarget.currentCol < 0)
		{
			return null;
		} else
		{	
			return selectorOnTarget;
		}
	}
	
	public virtual Stats getMandatoryTarget(ArrayList listOfTargets)
	{
		foreach(Stats target in listOfTargets)
		{
			if(Helpers.hasQuality<Trait>(target.traits, t => t.isMandatoryTarget()))
			{
				return target;
			}
		}

		return null;
	}
	
}
