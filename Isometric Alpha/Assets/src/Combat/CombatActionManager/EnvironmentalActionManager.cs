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
		instance = this;
	}
	
	public List<CombatAction> getAllEnvironmentalCombatActions(List<CombatAction> actionList)
	{
		foreach(KeyValuePair<CombatAction,Trait> kvp in environmentalCombatActions)
		{
			CombatAction envCombatAction = kvp.Key.clone();
			
			if(CombatGrid.getCombatantAtCoords(envCombatAction.getActorCoords()) == null || 
				CombatGrid.getCombatantAtCoords(envCombatAction.getActorCoords()).isDead())
			{
				continue;
			}
			
			Trait targetingTrait = kvp.Value.clone();
			List<Stats> listOfTargets;
			
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
			
			actionList.Add(envCombatAction);
		}
	
		return actionList;
	}
	
	public static void instateEnvironmentalCombatAction(Stats actorStats)
	{
        CombatAction envCombatAction;
        Trait targetingTrait;

        switch(actorStats.getName())
        {
            case NPCNameList.kende:
                envCombatAction = AbilityList.getAbility(actorStats, AbilityList.turnUpTheHeatKey);
                targetingTrait = TraitList.specificCheckeredLeftAlliedSide.clone();
                break;
            case MonsterNameList.stoneSaint:

                if(actorStats.hasTrait(TraitList.cannotSummon))
                {
                    return;
                }

                envCombatAction = AbilityList.getAbility(actorStats, AbilityList.stoneSaintMaterialsSummonKey);
                targetingTrait = TraitList.emptyGenerated2;
                break;
            default:
                return;
        }

        envCombatAction.setActor(actorStats);
        environmentalCombatActions.Add(envCombatAction, targetingTrait);
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
