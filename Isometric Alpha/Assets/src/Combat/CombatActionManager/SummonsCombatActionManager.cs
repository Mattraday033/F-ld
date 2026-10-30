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
		foreach(VolleyParticipantStats summon in listOfSummons)
		{	
			if(summon.isPartOfVolley)
			{
				continue; 
			}
			
			CombatAction summonCombatAction = summon.combatAction;

			if (summon.positions.Count > 0)
			{
				summonCombatAction.actorStats = summon;
			}

			Selector selector = summonCombatAction.selector;
			
			if(selector == null)
			{
				continue;
			}
			
			summonCombatAction.selector = selector;
			//summonCombatAction.setTargetCoords(new GridCoords(selector.currentRow, selector.currentCol));
			
			summonCombatAction.performAllQueuingUnits();
			
			if(alliedSide)
			{
				alliedSummonsCombatActionQueue.Add(summonCombatAction);
			} else
			{
				enemySummonsCombatActionQueue.Add(summonCombatAction);
			}
		}
		
		//uses deprecated combat actions (VolleyAbility), see Assets/src/DepricatedCombatActions
		// if(Helpers.hasQuality<VolleyParticipantStats>(listOfSummons, s => s.isPartOfVolley))
		// {
		// 	VolleyAbility newVolleyAbility = constructVolleyCombatAction(alliedSide);

		// 	if(newVolleyAbility != null)
		// 	{
		// 		//newVolleyAbility.printAllActorCoords();

		// 		if(alliedSide)
		// 		{
		// 			alliedSummonsCombatActionQueue.Add(newVolleyAbility);
		// 		} else
		// 		{
		// 			enemySummonsCombatActionQueue.Add(newVolleyAbility);
		// 		}

		// 	}
		// }
	}


	//uses deprecated combat actions (VolleyAbility), see Assets/src/DepricatedCombatActions
	// public VolleyAbility constructVolleyCombatAction(bool alliedSide)
	// {
	// 	VolleyAbility newVolleyAbility = new VolleyAbility(alliedSide);
	// 	Selector selector = newVolleyAbility.selector;

	// 	if(selector == null)
	// 	{
	// 		return null;
	// 	}

	// 	newVolleyAbility.selector = selector;
	// 	//newVolleyAbility.setTargetCoords(selector.getCoords());

	// 	return newVolleyAbility;
	// }
}