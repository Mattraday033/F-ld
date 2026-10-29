using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideAttack : Attack	
{
	public override bool killsCaster => true;

	public SuicideAttack(Stats actor, Weapon mainHandWeapon, List<TargetingLogic> targetingLogics = null): base(actor, mainHandWeapon, targetingLogics)
	{

	}
	
	public override void performCombatAction(List<Stats> targets)
	{
		base.performCombatAction(targets);
		
		Stats caster = actorStats;
		
		caster.modifyCurrentHealth(caster.getTotalHealth()*2);
		
		caster.setToDeadSprite();
	}
}
