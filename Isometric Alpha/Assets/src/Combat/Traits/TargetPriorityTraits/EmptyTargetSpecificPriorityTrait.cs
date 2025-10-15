using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTargetSpecificPriorityTrait : TargetPriorityTrait
{
	private const string initialName = "Empty";
	private const string initialTraitDescription = "This creature targets an empty space on it's side of the field.";
	private const string initialTraitIconName = "Empty";
	private static Color initialIconBackgroundColor = Color.blue;
	
	private GridCoords targetCoords;
	
	public EmptyTargetSpecificPriorityTrait(GridCoords targetCoords): base(initialName, initialTraitDescription, initialTraitIconName, initialIconBackgroundColor)
	{
		this.targetCoords = targetCoords;
	}
	
	public override bool deterministic()
	{
		return true;
	}
	
	public override bool targetsOnlyEmptySpace()
	{
		return true;
	}
	
	public override Selector findTargetLocation(Selector selector, ArrayList listOfTargets)
	{
		Selector selectorOnTarget = selector.clone();
		selectorOnTarget.setToLocation(targetCoords);
		
		return selectorOnTarget;
	}
}
