using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideAttack : Attack	
{
	public SuicideAttack(Stats actor, Weapon mainHandWeapon): base(actor, mainHandWeapon)
	{

	}
	
	public override void performCombatAction(List<Stats> targets)
	{
		base.performCombatAction(targets);
		
		Stats caster = getActorStats();
		
		caster.modifyCurrentHealth(caster.getTotalHealth()*2);
		
		caster.setToDeadSprite();
	}
	
	public override bool killsCaster()
	{
		return true;
	}

}
