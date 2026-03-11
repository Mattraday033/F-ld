using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonOnDeathTrait : OnDeathEffectTrait
{
	private const string initialTraitName = "Splits";
	private const string initialTraitDescription = "When this creature is killed, it splits into multiple minions.";
	private const string initialTraitIconName = "Splits";
	
	private bool thisTraitPreventsResurrection;
	
	public SummonOnDeathTrait(string abilityKey, GeneratedTargetPriorityTrait targetPriority, bool preventsResurrection = true):
	base(initialTraitName, initialTraitDescription, initialTraitIconName, abilityKey, targetPriority)
	{
		thisTraitPreventsResurrection = preventsResurrection;
		deleteIfIsDead = false;
	}
	
	public SummonOnDeathTrait(string abilityKey, EmptyTargetSpecificPriorityTrait targetPriority, bool preventsResurrection = true, bool deleteIfIsDead = false):
	base(initialTraitName, initialTraitDescription, initialTraitIconName, abilityKey, targetPriority)
	{
		thisTraitPreventsResurrection = preventsResurrection;
		this.deleteIfIsDead = deleteIfIsDead;
	}
	
	public override bool preventsResurrection()
	{
		return thisTraitPreventsResurrection;
	}
}
