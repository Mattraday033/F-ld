using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTargetPriorityTrait : TargetPriorityTrait
{
	private const string initialName = "Empty";
	private const string initialTraitDescription = "This creature targets an empty space on it's side of the field.";
	private const string initialTraitIconName = "Empty";
	private static Color initialIconBackgroundColor = Color.blue;
	
	public EmptyTargetPriorityTrait(): base(initialName, initialTraitDescription, initialTraitIconName, initialIconBackgroundColor)
	{
		
	}
	
	public override bool deterministic()
	{
		return false;
	}
	
	public override bool targetsOnlyEmptySpace()
	{
		return true;
	}
	
	public override Selector findTargetLocation(Selector selector, ArrayList listOfTargets)
	{
		GridCoords coords = CombatGrid.findRandomOpenSpace(CombatGrid.getAllEmptySpacesInEnemyZone());
		
		if(coords.Equals(GridCoords.getDefaultCoords()))
		{
			return null;
		}
		
		Selector selectorOnTarget = selector.clone();
		selectorOnTarget.setToLocation(coords);
		
		return selectorOnTarget;
	}
}
