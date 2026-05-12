using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaoticTargetPriorityTrait : TargetPriorityTrait
{
	public const string initialName = "Chaotic";
	public const string initialTraitDescription = "This creature chooses it's targets at random. Ignores summons.";
	public const string initialTraitIconName = "Dice";
	
	public ChaoticTargetPriorityTrait(): base(initialName, initialTraitDescription, initialTraitIconName)
	{
		
	}
	
	public override bool deterministic()
	{
		return false;
	}
	
	public override Stats getMandatoryTarget(List<Stats> listOfTargets)
	{
		listOfTargets = scrubSummonsFromTargetList(listOfTargets);
		
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
			int index = UnityEngine.Random.Range(0, listOfTargets.Count);
			
			mandatoryTarget = listOfTargets[index];
		} 
		
		return mandatoryTarget;
	}
	
	private List<Stats> scrubSummonsFromTargetList(List<Stats> listOfTargets)
	{
		List<Stats> newListOfTargets = new List<Stats>();

		foreach(Stats target in listOfTargets)
		{
			if(!target.isSummon())
			{
				newListOfTargets.Add(target);
			}
		}
		
		return newListOfTargets;
	}
}

public class NonMasterChaoticTargetPriorityTrait : TargetPriorityTrait
{
	public NonMasterChaoticTargetPriorityTrait(): 
    base( ChaoticTargetPriorityTrait.initialName,
          ChaoticTargetPriorityTrait.initialTraitDescription,
          ChaoticTargetPriorityTrait.initialTraitIconName)
	{
		
	}
	
	public override Stats getMandatoryTarget(List<Stats> listOfTargets)
	{
		listOfTargets = scrubMastersFromTargetList(listOfTargets);
		
		Stats mandatoryTarget = base.getMandatoryTarget(listOfTargets);
		
		if(mandatoryTarget == null && listOfTargets.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, listOfTargets.Count);
			
			mandatoryTarget = listOfTargets[index];
		} 

		return mandatoryTarget;
	}
	
	private List<Stats> scrubMastersFromTargetList(List<Stats> listOfTargets)
	{
		List<Stats> newListOfTargets = new List<Stats>();

		foreach(Stats target in listOfTargets)
		{
			if(target != null && target.isSummon() || target.isMinion())
			{
				newListOfTargets.Add(target);
			}
		}
		
		return newListOfTargets;
	}
}