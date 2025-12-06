using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeadCombatantManager : MonoBehaviour
{
    public static DeadCombatantManager instance;
	
    [RuntimeInitializeOnLoadMethod]
    private static void initializeDeadCombatManager()
    {
        instance = null;
    }

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
		List<Stats> deadCombatantList = new List<Stats>();
		List<Stats> listOfCombatants = CombatGrid.getAllCombatants();

        for (int i = 0; i < listOfCombatants.Count; i++)
        {
            Stats currentCombatant = listOfCombatants[i];

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
		
		foreach(Stats deadCombatant in deadCombatantList)
		{			
			deadCombatant.setToDeadSprite();
		}
	}
	
	public void removeDeadCombatantCombatActions(List<CombatAction> actionQueue)
	{
		for(int actionIndex = 0; actionIndex < actionQueue.Count; actionIndex++)
		{
			CombatAction action = actionQueue[actionIndex];
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
