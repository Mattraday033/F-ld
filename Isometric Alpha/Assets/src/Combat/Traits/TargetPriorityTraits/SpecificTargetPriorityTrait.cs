using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpecificTargetPriorityTrait : TargetPriorityTrait
{
	private GridCoords[] specificTargets;
	
	public SpecificTargetPriorityTrait(string traitName, string traitDescription, string traitIconName, Color traitIconBackgroundColor, GridCoords specificTarget): 
	base(traitName, traitDescription, traitIconName, traitIconBackgroundColor)
	{
		this.specificTargets = new GridCoords[1]{specificTarget};
	}
	
	public SpecificTargetPriorityTrait(string traitName, string traitDescription, string traitIconName, Color traitIconBackgroundColor, GridCoords[] specificTargets): 
	base(traitName, traitDescription, traitIconName, traitIconBackgroundColor)
	{
		this.specificTargets = specificTargets;
	}
	
	public override Selector findTargetLocation(Selector selector, ArrayList listOfTargets)
	{
		Selector selectorOnTarget = selector.clone();

		if(selectorOnTarget.singleTile && specificTargets.Length > 1)
		{
			selectorOnTarget = SelectorGenerator.generate(specificTargets);
		} else
		{
			selectorOnTarget.setToLocation(specificTargets[0].clone());
		}

		return selectorOnTarget;
	}
}
