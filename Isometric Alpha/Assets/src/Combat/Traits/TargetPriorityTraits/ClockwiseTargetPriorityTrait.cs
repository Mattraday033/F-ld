using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ClockwiseTargetPriorityTrait : TargetPriorityTrait
{
	private const string initialName = "Clockwise";
	private const string initialTraitDescription = "This creature chooses it's targets in a cycle in the clockwise direction.";
	private const string initialTraitIconName = "Clockwise";
	
	public GridCoords[] positions;
	
	public ClockwiseTargetPriorityTrait(GridCoords[] positions): base(initialName, initialTraitDescription, initialTraitIconName)
	{
		this.positions = positions;
	}
	
	public override Selector findTargetLocation(Selector selector, List<Stats> listOfTargets)
	{		
        selector = selector.clone();

		for(int positionsIndex = 0; positionsIndex < positions.Length; positionsIndex++)
		{
			if(CombatStateManager.turnNumber % positions.Length == positionsIndex)
			{
				selector.setToLocation(positions[positionsIndex]);
				
				return selector;
			}
		}
		
        Debug.LogError("Unexpected number : " + CombatStateManager.turnNumber);

        return selector;

	}
	
}
