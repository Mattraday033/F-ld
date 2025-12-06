using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangedTargetPriorityTrait : TargetPriorityTrait
{
	private const string initialName = "Close Ranged";
	private const string initialTraitDescription = "This creature always attacks the targets that are closest to it.";
	private const string initialTraitIconName = "Close Ranged";
	private readonly static Color initialIconBackgroundColor = Color.red;
	
	public CloseRangedTargetPriorityTrait(): base(initialName, initialTraitDescription, initialTraitIconName, initialIconBackgroundColor)
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
				if(mostForwardTarget == null || potentialTarget.position.row < mostForwardTarget.position.row)
				{
					mostForwardTarget = potentialTarget;
				}
			}
			
			foreach(Stats potentialTarget in listOfTargets)
			{
				if(potentialTarget.position.row == mostForwardTarget.position.row)
				{
					mostForwardTargets.Add(potentialTarget);
				}
			}
			
			int randomIndex = UnityEngine.Random.Range(0,mostForwardTargets.Count);
			
			mandatoryTarget = (Stats) mostForwardTargets[randomIndex];
		} 
		
		return mandatoryTarget;
	}
}
