using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathEffectTrait : Trait
{
	private string abilityKey;
	private TargetPriorityTrait targetPriority;
	public bool deleteIfIsDead {private get; set;}
	
	public OnDeathEffectTrait(string traitName, string traitType, string traitDescription, string traitIconName, Color traitIconBackgroundColor, string abilityKey, TargetPriorityTrait targetPriority):
	base(traitName, traitType, traitDescription, traitIconName, traitIconBackgroundColor)
	{
		this.abilityKey = abilityKey;
		this.targetPriority = targetPriority;
	}
	
	public override void onDeathEffect(Stats actor)
	{
		CombatAction actionOnDeath = ((CombatAction) AbilityList.enemyAbilityDictionary[abilityKey].clone());
		actionOnDeath.setActorCoords(actor.position);
		Selector actionSelector = SelectorManager.getInstance().selectors[actionOnDeath.getRangeIndex()].clone();
		
		ArrayList listOfTargets;
		
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
	}
	
	public override bool deleteIfDead()
	{
		return deleteIfIsDead;
	}

}
