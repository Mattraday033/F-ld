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
        Debug.LogError("1 cleanUpAllDeadCombatants()");

		ArrayList deadCombatantList = new ArrayList();
		ArrayList listOfCombatants = CombatGrid.getAllCombatants();

        for (int i = 0; i < listOfCombatants.Count; i++)
        {
            Stats currentCombatant = (Stats)listOfCombatants[i];

            if (currentCombatant.isDead())
            {
                int row = currentCombatant.position.row;
                int col = currentCombatant.position.col;

                deadCombatantList.Add(currentCombatant);

                if (currentCombatant.traits != null &&
                    currentCombatant.traits.Contains(TraitList.minion))
                {
                    EnemyCombatActionManager.applyLinkDamage();
                }
            }
        }
		
        Debug.LogError("2 cleanUpAllDeadCombatants()");
        
		foreach(Stats deadCombatant in deadCombatantList)
		{			
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
