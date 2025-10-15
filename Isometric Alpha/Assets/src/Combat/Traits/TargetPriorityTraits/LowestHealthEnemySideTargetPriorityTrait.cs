using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct TargetHealthRatio
{
	public float healthRatio;
	public Stats stats;
	
	public TargetHealthRatio(float healthRatio, Stats stats)
	{
		this.healthRatio = healthRatio;
		this.stats = stats;
	}
}

public class LowestHealthEnemySideTargetPriorityTrait : TargetPriorityTrait
{

	public LowestHealthEnemySideTargetPriorityTrait() :
	base("", "", "", Color.black)
	{

	}

	public override Stats getMandatoryTarget(ArrayList listOfTargets)
	{
		listOfTargets = CombatGrid.getAllAliveEnemyCombatants();

		List<TargetHealthRatio> sortedTargets = orderTargetsByHealthMissing(listOfTargets);

		sortedTargets = getLowestHealthTargets(sortedTargets);

		int randomIndex = UnityEngine.Random.Range(0, sortedTargets.Count);

		if (sortedTargets[randomIndex].stats != getTraitHolder())
		{
			return sortedTargets[randomIndex].stats;
		}
		else
		{
			foreach (TargetHealthRatio targetHealthRatio in sortedTargets)
			{
				if (targetHealthRatio.stats != getTraitHolder())
				{
					return targetHealthRatio.stats;
				}
			}
		}

		return getTraitHolder();
		
	}

	private List<TargetHealthRatio> getLowestHealthTargets(List<TargetHealthRatio> sortedTargets)
	{
		return sortedTargets.Where(x => x.healthRatio == sortedTargets.Min(y => y.healthRatio)).ToList();
	}

	private List<TargetHealthRatio> orderTargetsByHealthMissing(ArrayList listOfTargets)
	{
		List<TargetHealthRatio> targetHealthDictionary = new List<TargetHealthRatio>();

		foreach (Stats target in listOfTargets)
		{
			if (target == null)
			{
				continue;
			}

			float currentHealth = target.currentHealth;
			float totalHealth = target.getTotalHealth();

			float missingHealthRatio = currentHealth / totalHealth;

			targetHealthDictionary.Add(new TargetHealthRatio(missingHealthRatio, target));
		}

		return targetHealthDictionary.OrderBy(x => x.healthRatio).ToList();
	}

}
