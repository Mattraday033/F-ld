using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonOnDeathTrait : OnDeathEffectTrait
{
	private const string initialTraitName = "Splits";
	private const string initialTraitType = "On Death";
	private const string initialTraitDescription = "When this creature is killed, it splits into multiple minions.";
	private const string initialTraitIconName = "Splits";
	
	private bool thisTraitPreventsResurrection;
	
	public SummonOnDeathTrait(string abilityKey, GeneratedTargetPriorityTrait targetPriority):
	base(initialTraitName, initialTraitType, initialTraitDescription, initialTraitIconName, Color.black, abilityKey, (TargetPriorityTrait) targetPriority)
	{
		thisTraitPreventsResurrection = false;
		deleteIfIsDead = false;
	}
	
	public SummonOnDeathTrait(string abilityKey, EmptyTargetSpecificPriorityTrait targetPriority, bool preventsResurrection):
	base(initialTraitName, initialTraitType, initialTraitDescription, initialTraitIconName, Color.black, abilityKey, (TargetPriorityTrait) targetPriority)
	{
		thisTraitPreventsResurrection = preventsResurrection;
		deleteIfIsDead = true;
	}
	
	public override bool preventsResurrection()
	{
		return thisTraitPreventsResurrection;
	}
}
