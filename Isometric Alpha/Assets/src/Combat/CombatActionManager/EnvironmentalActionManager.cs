using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			
			Selector envCombatActionSelector = targetingTrait.findTargetLocation(SelectorFactory.buildByTemplate(envCombatAction.getRangeTemplate()), listOfTargets);
			
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
        if(actorStats == null)
        {
            return;
        }

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
            case NPCNameList.chiefTabor:

                if(actorStats.positions.Any(p => CombatGrid.positionIsOnAlliedSide(p)))
                {
                    envCombatAction = AbilityList.getAbility(actorStats, AbilityList.taborsWhipKey);
                    targetingTrait = TraitList.chaotic;

                    environmentalCombatActions.Add(envCombatAction.clone(actorStats), targetingTrait.clone(actorStats));
                } else
                {
                    envCombatAction = AbilityList.getAbility(actorStats, AbilityList.frontHandKey);
                    targetingTrait = TraitList.chaotic;

                    environmentalCombatActions.Add(envCombatAction.clone(actorStats), targetingTrait.clone(actorStats));

                    envCombatAction = AbilityList.getAbility(actorStats, AbilityList.backHandKey);
                    
                    environmentalCombatActions.Add(envCombatAction.clone(actorStats), targetingTrait.clone(actorStats));
                }
                return;
            case NPCNameList.clay:

                envCombatAction = AbilityList.getAbility(actorStats, AbilityList.swapKey);
                targetingTrait = TraitList.singleTargetBuffer;

                environmentalCombatActions.Add(envCombatAction.clone(actorStats), targetingTrait.clone(actorStats));

                envCombatAction = AbilityList.getAbility(actorStats, AbilityList.growMobKey);
                targetingTrait = TraitList.emptyGenerated3;
                
                environmentalCombatActions.Add(envCombatAction.clone(actorStats), targetingTrait.clone(actorStats));

                envCombatAction = AbilityList.getAbility(actorStats, AbilityList.rileKey);
                targetingTrait = TraitList.buffer;
                
                environmentalCombatActions.Add(envCombatAction.clone(actorStats), targetingTrait.clone(actorStats));
                return;
            default:
                return;
        }

        envCombatAction.setActor(actorStats);
        environmentalCombatActions.Add(envCombatAction, targetingTrait);
	}
	
	public static void deleteAllEnvironmentalCombatActions()
	{
		environmentalCombatActions = new Dictionary<CombatAction,Trait>();
	}
}

public static class EnvironmentalCombatActionList
{
    public static void addTakacsPuppetWaveSummon()
    {
        if(CombatActionManager.critCombatActionQueue.Count > 1)
        {
            return;
        }

        WavesWinCondition.incrementWavesDefeated();

        CombatActionManager.skipWaitBetweenCombatActions = true;

        CombatActionManager.addCritCombatAction(prepareSummonWaveAbility(AbilityList.summonAxemanPuppetsKey, 3));

        CombatActionManager.addCritCombatAction(prepareSummonWaveAbility(AbilityList.summonSpearmanPuppetsKey, 2));
        
        CombatActionManager.addCritCombatAction(prepareSummonWaveAbility(AbilityList.summonDisciplinarianPuppetsKey, 1));
        
        CombatActionManager.addCritCombatAction(prepareSummonWaveAbility(AbilityList.summonJavelineerPuppetsKey, 0));
    }

    private static SummonAbility prepareSummonWaveAbility(string key, int row)
    {
        SummonAbility summonAbility = AbilityList.getAbility(EnemyStatsList.getEnemyStats(NPCNameList.takacs), key) as SummonAbility;
        Selector summonSelector = SelectorFactory.buildByTemplate(SelectorTemplate.HorizontalThree).clone();
        summonSelector.setToLocation(new GridCoords(row,0));
        summonAbility.setSelector(summonSelector);

        return summonAbility;
    }
}