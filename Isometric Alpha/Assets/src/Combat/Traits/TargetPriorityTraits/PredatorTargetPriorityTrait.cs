using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatoryTargetPriorityTrait : TargetPriorityTrait
{
	public const string initialName = "Predatory";
	private const string initialTraitDescription = "This creature always attacks the target with the least health left.";
	public const string initialTraitIconName = "Predatory";
	
	public PredatoryTargetPriorityTrait(): base(initialName, initialTraitDescription, initialTraitIconName)
	{
		
	}
	
	public override Stats getMandatoryTarget(List<Stats> listOfTargets)
	{
        foreach(Stats stats in listOfTargets)
        {
            if(stats != null && stats.isAlive() && stats.mandatoryTargetForTargetingType(this))
            {
                return stats;
            }
        }

		Stats mandatoryTarget = base.getMandatoryTarget(listOfTargets);
		
		if(mandatoryTarget == null)
		{
			Stats lowestCurrentHealthTarget = null;
			
			foreach(Stats target in listOfTargets)
			{
				if(lowestCurrentHealthTarget == null || target.currentHealth < lowestCurrentHealthTarget.currentHealth)
				{
					lowestCurrentHealthTarget = target;
				}
			}
			
			mandatoryTarget = lowestCurrentHealthTarget;
		} 
		
		return mandatoryTarget;
	}
}
