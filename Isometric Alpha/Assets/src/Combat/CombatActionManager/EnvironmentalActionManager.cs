using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnvironmentalCombatActionManager : MonoBehaviour
{
	
	public static Dictionary<CombatAction,Trait> environmentalCombatActions = new Dictionary<CombatAction,Trait>();
	
	private static EnvironmentalCombatActionManager instance;
	
	public static EnvironmentalCombatActionManager getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("Duplicate instances of EnvironmentalCombatActionManager exist");
		}
		
		instance = this;
	}
	
	public ArrayList getAllEnvironmentalCombatActions(ArrayList actionList)
	{
		foreach(KeyValuePair<CombatAction,Trait> kvp in environmentalCombatActions)
		{
			CombatAction envCombatAction = kvp.Key.clone();
			
			if(CombatGrid.getCombatantAtCoords(envCombatAction.getActorCoords()) == null || 
				CombatGrid.getCombatantAtCoords(envCombatAction.getActorCoords()).isDead)
			{
				continue;
			}
			
			Trait targetingTrait = kvp.Value.clone();
			ArrayList listOfTargets;
			
			if(envCombatAction.getActorStats().shouldTargetEnemy())
			{
				listOfTargets = CombatGrid.getAllAliveEnemyCombatants();
			} else
			{
				listOfTargets = CombatGrid.getAllAliveAllyCombatants();
			}
			
			Selector envCombatActionSelector = targetingTrait.findTargetLocation(SelectorManager.getInstance().selectors[envCombatAction.getRangeIndex()].clone(), listOfTargets);
			
			if(envCombatActionSelector == null)
			{
				continue;
			}
			
			envCombatAction.setSelector(envCombatActionSelector);
			
			//envCombatAction.setTargetCoords(new GridCoords(envCombatAction.getSelector().currentRow, envCombatAction.getSelector().currentCol));
			
			actionList.Add(envCombatAction);
		}
	
		return actionList;
	}
	
	public void instateEnvironmentalCombatAction(string environmentalCombatActionKey, string environmentalTargetingTraitKey, Stats actorStats)
	{
		if((environmentalCombatActionKey == null || environmentalCombatActionKey.Length <= 0) ||
			(environmentalTargetingTraitKey == null || environmentalTargetingTraitKey.Length <= 0))
		{
			return;
		} else
		{
			CombatAction envCombatAction = AbilityList.getAbility(environmentalCombatActionKey);
			envCombatAction.setActor(actorStats);
			
			Trait targetingTrait = TraitList.getTrait(environmentalTargetingTraitKey);
			
			environmentalCombatActions[envCombatAction] = targetingTrait;
		}
	}
	
	public void updateEnvironmentalCasterPosition(GridCoords oldPosition, GridCoords newPosition)
	{
		foreach(KeyValuePair<CombatAction,Trait> kvp in environmentalCombatActions)
		{
			CombatAction envCombatAction = kvp.Key;
			
			if(envCombatAction.getActorCoords().Equals(oldPosition))
			{
				envCombatAction.setActorCoords(newPosition.clone());
			}
		}
	}
	
	public static void deleteAllEnvironmentalCombatActions()
	{
		environmentalCombatActions = new Dictionary<CombatAction,Trait>();
	}
	
        
}
