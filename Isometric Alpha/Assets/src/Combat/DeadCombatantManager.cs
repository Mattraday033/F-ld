using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeadCombatantManager : MonoBehaviour
{
	public static DeadCombatantManager instance;
	
	public static void handleDeadCombatants()
	{
		getInstance().cleanUpAllDeadCombatants();
		getInstance().removeDeadCombatantCombatActions(PlayerCombatActionManager.playerCombatActionQueue);
		getInstance().removeDeadCombatantCombatActions(EnemyCombatActionManager.enemyCombatActionQueue);
        getInstance().removeDeadCombatantCombatActions(EnemyCombatActionManager.slowedEnemyCombatActionQueue);
        getInstance().removeDeadCombatantCombatActions(CombatActionManager.lockedInCombatActionQueue);
	}
	
	public void cleanUpAllDeadCombatants()
	{
		ArrayList deadCombatantList = new ArrayList();
		ArrayList listOfCombatants = CombatGrid.getAllCombatants();
		
		for(int i = 0; i < listOfCombatants.Count; i++)
		{
			Stats currentCombatant = (Stats) listOfCombatants[i];
			
			if(currentCombatant.currentHealth <= 0 && !currentCombatant.isDead)
			{
				int row = currentCombatant.position.row;
				int col = currentCombatant.position.col;
				
				deadCombatantList.Add(currentCombatant);
				
				if(currentCombatant.traitNames != null && 
					(currentCombatant.traitNames.Contains("Minion") || 
						currentCombatant.traitNames.Contains("minion")))
				{
					EnemyCombatActionManager.applyLinkDamage();
				}
			}
		}
		
		foreach(Stats deadCombatant in deadCombatantList)
		{
			//Death animation. This will only run once per action so anything in this list should get their death animation
			//at the same time and then be removed before this method runs again
			
			deadCombatant.setToDeadSprite();
		}
		
	}
	
	public void removeDeadCombatantCombatActions(ArrayList actionQueue)
	{
		for(int actionIndex = 0; actionIndex < actionQueue.Count; actionIndex++)
		{
			CombatAction action = (CombatAction) actionQueue[actionIndex];
			if (!action.multiActorAction() &&
				(CombatGrid.getCombatantAtCoords(action.getActorCoords()) == null ||
				!CombatGrid.getCombatantAtCoords(action.getActorCoords()).isAlive()))
			{
				actionQueue.RemoveAt(actionIndex);
				actionIndex--;
			}
        }
	}

	public static DeadCombatantManager getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogError("Found more than one DeadCombatantManager in the scene.");
		}
		
		instance = this;
	}

}
