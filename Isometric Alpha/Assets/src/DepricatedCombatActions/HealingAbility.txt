using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealingAbility : Ability
{
	private const string healingCritFormula = "0";

	public override bool healsTarget => true;
	public override bool targetsAllySection => true;

	public HealingAbility(CombatActionSettings settings, List<TargetingLogic> targetingLogics = null):
	base(settings, targetingLogics)
	{
		
	}

    public override void applySettings(CombatActionSettings settings)
    {
		settings.damageParams.critFormula = healingCritFormula;

        base.applySettings(settings);
    }
}