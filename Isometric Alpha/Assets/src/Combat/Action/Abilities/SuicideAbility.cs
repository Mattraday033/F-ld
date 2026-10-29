using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideAbility : Ability
{
	public override bool killsCaster => true;

	public SuicideAbility(CombatActionSettings settings, List<TargetingLogic> targetingLogics = null):
	base(settings, targetingLogics)
	{

	}
	
	public override void performCombatAction(List<Stats> targets)
	{
		base.performCombatAction(targets);
		
        if(CombatGrid.combatantExistsAtCoords(actorCoords, out Stats caster))
        {
            caster.modifyCurrentHealth(caster.getTotalHealth()*2);
		
		    caster.setToDeadSprite();
        }
	}
}
