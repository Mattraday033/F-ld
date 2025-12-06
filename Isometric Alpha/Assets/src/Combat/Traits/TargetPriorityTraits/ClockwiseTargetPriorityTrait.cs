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
	private readonly static Color initialIconBackgroundColor = Color.black;
	
	public GridCoords[] positions;
	
	public ClockwiseTargetPriorityTrait(GridCoords[] positions): base(initialName, initialTraitDescription, initialTraitIconName, initialIconBackgroundColor)
	{
		this.positions = positions;
	}
	
	public override Selector findTargetLocation(Selector selector, List<Stats> listOfTargets)
	{		
		for(int positionsIndex = 0; positionsIndex < positions.Length; positionsIndex++)
		{
			if(CombatStateManager.turnNumber % positions.Length == positionsIndex)
			{
				selector.setToLocation(positions[positionsIndex]);
				
				return selector;
			}
		}
		
		throw new IOException("Unexpected number : " + CombatStateManager.turnNumber);	//should never happen
	}
	
}
