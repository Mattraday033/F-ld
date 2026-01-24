using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonOnDeathTrait : OnDeathEffectTrait
{
	private const string initialTraitName = "Splits";
	private const string initialTraitDescription = "When this creature is killed, it splits into multiple minions.";
	private const string initialTraitIconName = "Splits";
	
	private bool thisTraitPreventsResurrection;
	
	public SummonOnDeathTrait(string abilityKey, GeneratedTargetPriorityTrait targetPriority):
	base(initialTraitName, initialTraitDescription, initialTraitIconName, abilityKey, targetPriority)
	{
		thisTraitPreventsResurrection = false;
		deleteIfIsDead = false;
	}
	
	public SummonOnDeathTrait(string abilityKey, EmptyTargetSpecificPriorityTrait targetPriority, bool preventsResurrection):
	base(initialTraitName, initialTraitDescription, initialTraitIconName, abilityKey, targetPriority)
	{
		thisTraitPreventsResurrection = preventsResurrection;
		deleteIfIsDead = true;
	}
	
	public override bool preventsResurrection()
	{
		return thisTraitPreventsResurrection;
	}
}
