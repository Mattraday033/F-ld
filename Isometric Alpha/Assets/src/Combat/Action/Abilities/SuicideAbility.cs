using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideAbility : Ability
{
	public SuicideAbility(CombatActionSettings settings):
	base(settings)
	{

	}
	
	public override void performCombatAction(List<Stats> targets)
	{
		base.performCombatAction(targets);
		
        if(CombatGrid.combatantExistsAtCoords(getActorCoords(), out Stats caster))
        {
            caster.modifyCurrentHealth(caster.getTotalHealth()*2);
		
		    caster.setToDeadSprite();
        }
	}
	
	public override bool killsCaster()
	{
		return true;
	}

}
