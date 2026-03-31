using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerTrait : TargetPriorityTrait
{
	private const string initialName = "Blocker";
	private const string initialTraitDescription = "This enemy prevents you or your allies from attacking anything else.";
	private const string initialTraitIconName = "Blocker";
	
	public BlockerTrait(): base(initialName, initialTraitDescription, initialTraitIconName)
	{
		
	}
	
	public override bool isPacifist()
	{
		return true;
	}
	
	public override bool isMandatoryTarget()
	{
		return true;
	}
	
	public override Selector findTargetLocation(Selector selector, List<Stats> listOfTargets)
	{
		Selector selectorOnTarget = selector.clone();
		selectorOnTarget.setToLocation(GridCoords.getDefaultCoords());
		
		return selectorOnTarget;
	}
}
