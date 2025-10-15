using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType {Buff = 1, Debuff = 2, Any = 3}

public class DamageOnFutureTraitApplicationTrait : Trait
{
	public string damageOnApplicationFormula;
	public string damagePenaltyFormula;
	public TriggerType triggerType;

	public DamageOnFutureTraitApplicationTrait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor, string damageOnApplicationFormula, TriggerType triggerType):
	base(traitName, traitType, traitDescription, traitIconName, traitIconBackgroundColor)
	{
		this.damageOnApplicationFormula = damageOnApplicationFormula;
		this.damagePenaltyFormula = "";
		this.triggerType = triggerType;
	}

	public DamageOnFutureTraitApplicationTrait(string traitName, string traitType, string traitDescription, string traitIconName, int roundsLeft, Color traitIconBackgroundColor, string damageOnApplicationFormula, TriggerType triggerType):
	base(traitName, traitType, traitDescription, traitIconName, roundsLeft, traitIconBackgroundColor)
	{
		this.damageOnApplicationFormula = damageOnApplicationFormula;
		this.damagePenaltyFormula = "";
		this.triggerType = triggerType;
	}

	public DamageOnFutureTraitApplicationTrait(string traitName, string traitType, string traitDescription, string traitIconName, int roundsLeft, Color traitIconBackgroundColor, string damageOnApplicationFormula, string damagePenaltyFormula, TriggerType triggerType):
	base(traitName, traitType, traitDescription, traitIconName, roundsLeft, traitIconBackgroundColor)
	{
		this.damageOnApplicationFormula = damageOnApplicationFormula;
		this.damagePenaltyFormula = damagePenaltyFormula;
		this.triggerType = triggerType;
	}
	
	public override int getBonusDamageDealt()
	{
		return -1*DamageCalculator.calculateFormula(damagePenaltyFormula, traitApplier);
	}
	
	public override int damageOnDebuffApplication()
	{
		if(triggerType.Equals(TriggerType.Buff))
		{
			return 0;
		} else
		{
			return damageDoneOnTrigger();
		}
	}
	
	public override int damageOnBuffApplication()
	{
		if(triggerType.Equals(TriggerType.Debuff))
		{
			return 0;
		} else
		{
			return damageDoneOnTrigger();
		}
	}
	
	private int damageDoneOnTrigger()
	{
		return DamageCalculator.calculateFormula(damageOnApplicationFormula, traitApplier);
	}

}
