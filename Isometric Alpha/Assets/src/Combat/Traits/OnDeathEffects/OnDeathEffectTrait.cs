using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathEffectTrait : Trait
{
	private string abilityKey;
	protected TargetPriorityTrait targetPriority;
	public bool deleteIfIsDead {private get; set;}
    private bool usedEffect = false;
	
	public OnDeathEffectTrait(string traitName, string traitDescription, string iconName, string abilityKey, TargetPriorityTrait targetPriority, TraitType traitType = TraitType.OnDeath):
	base(traitName, traitType, traitDescription, iconName)
	{
		this.abilityKey = abilityKey;
		this.targetPriority = targetPriority;
	}
	
	public override void onDeathEffect(Stats actor)
	{
        if(usedEffect)
        {
            return;
        }

		CombatAction actionOnDeath = AbilityList.enemyAbilityDictionary[abilityKey].clone();
		actionOnDeath.setActor(actor);
        actor.inOnDeathEffect = true;
		Selector actionSelector = SelectorList.getByName(actionOnDeath.getRangeName());
		
		List<Stats> listOfTargets;
		
		if(actor.shouldTargetEnemy())
		{
			listOfTargets = CombatGrid.getAllAliveEnemyCombatants();
		} else
		{
			listOfTargets = CombatGrid.getAllAliveAllyCombatants();
		}
		
		if(actionOnDeath.isSelfTargeting())
		{
			actionOnDeath.setSelector(actionOnDeath.getTargetSelector());
			if (actor.positions.Count > 0)
			{
				actionOnDeath.getSelector().setToLocation(actor.positions[0]);
			}
			//actionOnDeath.setTargetCoords(actor.position);
		} else
		{
            Selector targetSelector = targetPriority.findTargetLocation(actionSelector, listOfTargets);

            if(targetSelector == null)
            {
                return;
            }

			actionOnDeath.setSelector(targetSelector.clone());
		}
		
		CombatActionManager.addOnDeathCombatAction(actionOnDeath);
        usedEffect = true;
	}
	
    public override bool hasUnusedOnDeathEffect()
    {
        return !usedEffect;
    }

	public override bool deleteIfDead()
	{
		return deleteIfIsDead;
	}

}
