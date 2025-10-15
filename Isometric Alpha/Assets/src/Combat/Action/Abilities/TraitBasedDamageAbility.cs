using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TraitBasedDamageAbility's are Abilities that deal more damage, or whose entire damage is, based on
//how many traits the target has on them. Should deal low amounts of starting damage. Mandatory Traits,
//such as minion/master, targeting parameters, and sometimes ability specific traits like spawner, do not
//cause this extra damage. Very powerful when paired with traits like vulnerable/bristled

public class TraitBasedDamageAbility: Ability
{
	private double extraDamagePercentagePerTrait = -1.0;

	public TraitBasedDamageAbility(CombatActionSettings settings, double extraDamagePercentagePerTrait):
	base(settings)
	{
		this.extraDamagePercentagePerTrait = extraDamagePercentagePerTrait;
	}

    public override int[] findFinalDamage(Stats targetCombatant, bool isCrit)
	{
		int initialFinalDamage = base.findFinalDamage(targetCombatant, isCrit)[0];
		int damageAfterAddingBonusDamage = 0;
		int numberOfTraitsOnTarget = getNumberOfEligibleTraits(targetCombatant);
		
		if(initialFinalDamage < 0 || targetCombatant == null)
		{
			return new int[]{-1};
		}		
		
		if(extraDamagePercentagePerTrait > 0.0)
		{
			double baseDamage = (double) initialFinalDamage;
			
			damageAfterAddingBonusDamage = (int) (baseDamage* (1.0 + (extraDamagePercentagePerTrait * (double) numberOfTraitsOnTarget)));
		} else
		{
			damageAfterAddingBonusDamage = initialFinalDamage*numberOfTraitsOnTarget;
		}
		
		return new int[]{damageAfterAddingBonusDamage};
	}
	
	private int getNumberOfEligibleTraits(Stats targetCombatant)
	{
		int numberOfEligibleTraits = 0;
		
		foreach(Trait trait in targetCombatant.traits)
		{
			if(!trait.isMandatoryTrait())
			{
				numberOfEligibleTraits++;
			}
		}
		
		return numberOfEligibleTraits;
	}
	
}