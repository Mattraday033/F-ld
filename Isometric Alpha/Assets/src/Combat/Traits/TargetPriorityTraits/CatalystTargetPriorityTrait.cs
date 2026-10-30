using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalystTargetPriorityTrait : TargetPriorityTrait
{
	private const string initialName = "Saintly";
	private const string initialTraitDescription = "This creature alternates between building up it's allies and demolishing it's enemies.";
	private const string initialTraitIconName = "Hammer";

	private SpecificTargetPriorityTrait evolutionTargetPriorityTrait;
	private TargetPriorityTrait damagingTargetTrait;

	public CatalystTargetPriorityTrait(SpecificTargetPriorityTrait evolutionTargetPriorityTrait, TargetPriorityTrait damagingTargetTrait): 
	base(initialName, initialTraitDescription + " " + damagingTargetTrait.getDescription(), initialTraitIconName)
	{
		this.evolutionTargetPriorityTrait = evolutionTargetPriorityTrait;
		this.damagingTargetTrait = damagingTargetTrait;
	}
	
	public override Selector findTargetLocation(Selector selector, List<Stats> listOfTargets)
	{
		
		//uses deprecated combat actions (EvolveAbility), see Assets/src/DepricatedCombatActions; this is what its inAttackMode() did
		// if(EvolveAbility.inAttackMode())
		if(CombatStateManager.turnNumber % 2 == 1)
		{
			return damagingTargetTrait.findTargetLocation(selector, listOfTargets);
		} else
		{
			return evolutionTargetPriorityTrait.findTargetLocation(selector, listOfTargets);
		}
	}
	
}
