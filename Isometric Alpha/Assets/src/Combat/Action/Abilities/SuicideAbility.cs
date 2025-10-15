using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideAbility : Ability
{
	public SuicideAbility(CombatActionSettings settings):
	base(settings)
	{

	}
	
	public override void performCombatAction(ArrayList targets)
	{
		base.performCombatAction(targets);
		
		Stats caster = CombatGrid.getCombatantAtCoords(getActorCoords());
		
		caster.modifyCurrentHealth(caster.getTotalHealth()*2);
		
		caster.setToDeadSprite();
	}
	
	public override bool killsCaster()
	{
		return true;
	}

}
