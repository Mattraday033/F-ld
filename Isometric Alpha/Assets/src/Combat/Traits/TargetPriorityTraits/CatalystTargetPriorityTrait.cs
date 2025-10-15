using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalystTargetPriorityTrait : TargetPriorityTrait
{
	private const string initialName = "Saintly";
	private const string initialTraitDescription = "This creature alternates between building up it's allies and demolishing it's enemies.";
	private const string initialTraitIconName = "Hammer";
	private static Color initialIconBackgroundColor = Color.blue;

	private SpecificTargetPriorityTrait evolutionTargetPriorityTrait;
	private TargetPriorityTrait damagingTargetTrait;

	public CatalystTargetPriorityTrait(SpecificTargetPriorityTrait evolutionTargetPriorityTrait, TargetPriorityTrait damagingTargetTrait): 
	base(initialName, initialTraitDescription + " " + damagingTargetTrait.getDescription(), initialTraitIconName, initialIconBackgroundColor)
	{
		this.evolutionTargetPriorityTrait = evolutionTargetPriorityTrait;
		this.damagingTargetTrait = damagingTargetTrait;
	}
	
	public override Selector findTargetLocation(Selector selector, ArrayList listOfTargets)
	{
		
		if(EvolveAbility.inAttackMode())
		{
			return damagingTargetTrait.findTargetLocation(selector, listOfTargets);
		} else
		{
			return evolutionTargetPriorityTrait.findTargetLocation(selector, listOfTargets);
		}
	}
	
}
