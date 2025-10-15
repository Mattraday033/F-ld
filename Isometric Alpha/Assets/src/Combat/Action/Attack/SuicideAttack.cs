using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideAttack : Attack	
{
	public SuicideAttack(Weapon mainHandWeapon): base(mainHandWeapon)
	{

	}
	
	public override void performCombatAction(ArrayList targets)
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
