using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatoryTargetPriorityTrait : TargetPriorityTrait
{
	private static string initialName = "Predatory";
	private static string initialTraitDescription = "This creature always attacks the target with the least health left.";
	private static string initialTraitIconName = "Predatory";
	private static Color initialIconBackgroundColor = Color.red;
	
	public PredatoryTargetPriorityTrait(): base(initialName, initialTraitDescription, initialTraitIconName, initialIconBackgroundColor)
	{
		
	}
	
	public override Stats getMandatoryTarget(ArrayList listOfTargets)
	{
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
