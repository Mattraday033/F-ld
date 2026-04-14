using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritorialTargetPriorityTrait : TargetPriorityTrait
{
	public const string initialName = "Territorial";
	private const string initialTraitDescription = "This creature always attacks the target with the most health left.";
	public const string initialTraitIconName = "Territorial";
	
	public TerritorialTargetPriorityTrait(): base(initialName, initialTraitDescription, initialTraitIconName)
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
			Stats highestCurrentHealthTarget = null;
			
			foreach(Stats target in listOfTargets)
			{
				if(highestCurrentHealthTarget == null || target.currentHealth > highestCurrentHealthTarget.currentHealth)
				{
					highestCurrentHealthTarget = target;
				}
			}
			
			mandatoryTarget = highestCurrentHealthTarget;
		} 
		
		return mandatoryTarget;
	}
}
