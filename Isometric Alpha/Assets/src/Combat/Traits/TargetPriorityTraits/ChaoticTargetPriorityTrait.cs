using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaoticTargetPriorityTrait : TargetPriorityTrait
{
	private const string initialName = "Chaotic";
	private const string initialTraitDescription = "This creature chooses it's targets at random. Ignores summons.";
	private const string initialTraitIconName = "Dice";
	private readonly static Color initialIconBackgroundColor = Color.blue;
	
	public ChaoticTargetPriorityTrait(): base(initialName, initialTraitDescription, initialTraitIconName, initialIconBackgroundColor)
	{
		
	}
	
	public override bool deterministic()
	{
		return false;
	}
	
	public override Stats getMandatoryTarget(List<Stats> listOfTargets)
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
	
	private List<Stats> scrubSummonsFromTargetList(List<Stats> listOfTargets)
	{
		List<Stats> newListOfTargets = new List<Stats>();

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
