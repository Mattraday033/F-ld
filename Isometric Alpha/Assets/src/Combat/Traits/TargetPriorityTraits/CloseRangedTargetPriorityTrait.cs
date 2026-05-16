using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CloseRangedTargetPriorityTrait : TargetPriorityTrait
{
	private const string initialName = "Close Ranged";
	private const string initialTraitDescription = "This creature always attacks the targets that are closest to it.";
	private const string initialTraitIconName = "Close Ranged";
	
	public CloseRangedTargetPriorityTrait(): base(initialName, initialTraitDescription, initialTraitIconName)
	{
		
	}
	
	public override Stats getMandatoryTarget(List<Stats> listOfTargets)
	{
		Stats mandatoryTarget = base.getMandatoryTarget(listOfTargets);
		
		if(mandatoryTarget == null)
		{
			Stats mostForwardTarget = null;
			List<Stats> mostForwardTargets = new List<Stats>();
			
			foreach(Stats potentialTarget in listOfTargets)
			{
				if(mostForwardTarget == null || frontmostRow(potentialTarget) < frontmostRow(mostForwardTarget))
				{
					mostForwardTarget = potentialTarget;
				}
			}

			foreach(Stats potentialTarget in listOfTargets)
			{
				if(frontmostRow(potentialTarget) == frontmostRow(mostForwardTarget))
				{
					mostForwardTargets.Add(potentialTarget);
				}
			}
			
			int randomIndex = UnityEngine.Random.Range(0,mostForwardTargets.Count);
			
			mandatoryTarget = (Stats) mostForwardTargets[randomIndex];
		} 
		
		return mandatoryTarget;
	}

	private static int frontmostRow(Stats stats)
	{
		return stats.positions.Count > 0 ? stats.positions.Min(p => p.row) : int.MaxValue;
	}
}
