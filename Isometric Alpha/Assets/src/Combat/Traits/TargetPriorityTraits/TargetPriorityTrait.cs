using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TargetPriorityTrait : Trait 
{
	public TargetPriorityTrait(string traitName, string traitDescription, string iconName): 
	base(traitName, TraitType.TargetPriority, traitDescription, iconName)
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
	
	public override Selector findTargetLocation(Selector selector, List<Stats> listOfTargets)
	{
		Selector selectorOnTarget = selector.clone();
		Stats mandatoryTarget = getMandatoryTarget(listOfTargets);
		
        if(mandatoryTarget == null)
        {
            return null;
        }

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
	
	public virtual Stats getMandatoryTarget(List<Stats> listOfTargets)
	{
		foreach(Stats target in listOfTargets)
		{
			if(Helpers.hasQuality<Trait>(target.traitContainer, t => t.isMandatoryTarget()))
			{
				return target;
			}
		}

		return null;
	}
	
}
