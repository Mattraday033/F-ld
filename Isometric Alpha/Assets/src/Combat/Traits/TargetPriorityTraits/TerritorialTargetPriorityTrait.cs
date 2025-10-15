using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritorialTargetPriorityTrait : TargetPriorityTrait
{
	private static string initialName = "Territorial";
	private static string initialTraitDescription = "This creature always attacks the target with the most health left.";
	private static string initialTraitIconName = "Territorial";
	private static Color initialIconBackgroundColor = Color.red;
	
	public TerritorialTargetPriorityTrait(): base(initialName, initialTraitDescription, initialTraitIconName, initialIconBackgroundColor)
	{
		
	}
	
	public override Stats getMandatoryTarget(ArrayList listOfTargets)
	{
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
