using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealingAbility : Ability
{
	private const string healingCritFormula = "0";

	public HealingAbility(CombatActionSettings settings):
	base(settings)
	{
		
	}

    public override void applySettings(CombatActionSettings settings)
    {
		settings.damageParams.critFormula = healingCritFormula;

        base.applySettings(settings);
    }

    public override int[] findFinalDamage(Stats targetCombatant, bool isCrit)
	{
		if(targetCombatant == null)
		{
			return new int[]{-1};
		}
		
		return new int[]{DamageCalculator.calculateFormula(getDamageFormula(), getActorStats())};
	}
	
	public override bool healsTarget()
	{
		return true;
	}
	
	public override bool targetsAllySection()
	{
		return true;
	}
}