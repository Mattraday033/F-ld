using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathEffectTrait : Trait
{
	private string abilityKey;
	private TargetPriorityTrait targetPriority;
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
		Selector actionSelector = SelectorManager.getInstance().selectors[actionOnDeath.getRangeIndex()].clone();
		
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
			actionOnDeath.getSelector().setToLocation(actor.position);
			//actionOnDeath.setTargetCoords(actor.position);
		} else
		{
			actionOnDeath.setSelector(targetPriority.findTargetLocation(actionSelector, listOfTargets).clone());
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
