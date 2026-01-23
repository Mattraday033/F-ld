using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonsCombatActionManager : MonoBehaviour
{
	private const bool onAlliedSide = true;
	private const bool onEnemySide = false;
	
	public static List<CombatAction> enemySummonsCombatActionQueue = new List<CombatAction>();
	public static List<CombatAction> alliedSummonsCombatActionQueue = new List<CombatAction>();
	
	public CombatAction volleyCombatAction;

	public void updateSummonedCombatActions()
	{
		List<Stats> listOfSummonedAllies = CombatGrid.getAllAliveSummonedAllies();
		List<Stats> listOfSummonedEnemies = CombatGrid.getAllAliveSummonedEnemies();
		
		if(listOfSummonedAllies.Count > 0)
		{
			decideSummonedCombatActions(listOfSummonedAllies, onAlliedSide);
		}
		
		if(listOfSummonedEnemies.Count > 0)
		{
			decideSummonedCombatActions(listOfSummonedEnemies, onEnemySide);
		}
	}

	private void decideSummonedCombatActions(List<Stats> listOfSummons, bool alliedSide)
	{
		foreach(AlliedSummonStats summon in listOfSummons)
		{	
			if(summon.isPartOfVolley())
			{
				continue; 
			}
			
			CombatAction summonCombatAction = summon.getCombatAction();
			summonCombatAction.setActorCoords(summon.position);
			Selector selector = summonCombatAction.getTargetSelector();
			
			if(selector == null)
			{
				continue;
			}
			
			summonCombatAction.setSelector(selector);
			//summonCombatAction.setTargetCoords(new GridCoords(selector.currentRow, selector.currentCol));
			
			summonCombatAction.queueingAction();
			
			if(alliedSide)
			{
				alliedSummonsCombatActionQueue.Add(summonCombatAction);
			} else
			{
				enemySummonsCombatActionQueue.Add(summonCombatAction);
			}
		}
		
		if(Helpers.hasQuality<AlliedSummonStats>(listOfSummons, s => s.isPartOfVolley()))
		{
			VolleyAbility newVolleyAbility = constructVolleyCombatAction(alliedSide);
		
			if(newVolleyAbility != null)
			{
				//newVolleyAbility.printAllActorCoords();
				
				if(alliedSide)
				{
					alliedSummonsCombatActionQueue.Add(newVolleyAbility);
				} else
				{
					enemySummonsCombatActionQueue.Add(newVolleyAbility);
				}
				
			}
		}
	}
	
	
	public VolleyAbility constructVolleyCombatAction(bool alliedSide)
	{
		VolleyAbility newVolleyAbility = new VolleyAbility(alliedSide);
		Selector selector = newVolleyAbility.getTargetSelector();
		
		if(selector == null)
		{
			return null;
		}
		
		newVolleyAbility.setSelector(selector);
		//newVolleyAbility.setTargetCoords(selector.getCoords());
		
		return newVolleyAbility;
	}
}