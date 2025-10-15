using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacklashAbility : Ability
{
	private double backlashMultiplier;

	public BacklashAbility(CombatActionSettings settings, double backlashMultiplier):
	base(settings)
	{
		this.backlashMultiplier = backlashMultiplier;
	}

    public override void performCombatAction(ArrayList targets)
	{
		dealBacklashDamage();
		
		base.performCombatAction(targets);
	}
	
	private void dealBacklashDamage()
	{
		getActorStats().modifyCurrentHealth((int) ((double) DamageCalculator.calculateFormula(getDamageFormula(), getActorStats()) * backlashMultiplier));
	}

}
