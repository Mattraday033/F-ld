using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaoticTargetPriorityTrait : TargetPriorityTrait
{
	private static string initialName = "Chaotic";
	private static string initialTraitDescription = "This creature chooses it's targets at random. Ignores summons.";
	private static string initialTraitIconName = "Dice";
	private static Color initialIconBackgroundColor = Color.blue;
	
	public ChaoticTargetPriorityTrait(): base(initialName, initialTraitDescription, initialTraitIconName, initialIconBackgroundColor)
	{
		
	}
	
	public override bool deterministic()
	{
		return false;
	}
	
	public override Stats getMandatoryTarget(ArrayList listOfTargets)
	{
		listOfTargets = scrubSummonsFromTargetList(listOfTargets);
		
		Stats mandatoryTarget = base.getMandatoryTarget(listOfTargets);
		
		if(mandatoryTarget == null)
		{
			int index = UnityEngine.Random.Range(0, listOfTargets.Count);
			
			mandatoryTarget = (Stats) listOfTargets[index];
		} 
		
		return mandatoryTarget;
	}
	
	private ArrayList scrubSummonsFromTargetList(ArrayList listOfTargets)
	{
		ArrayList newListOfTargets = new ArrayList();

		foreach(Stats target in listOfTargets)
		{
			if(!target.wasSummoned())
			{
				newListOfTargets.Add(target);
			}
		}
		
		return newListOfTargets;
	}
}
