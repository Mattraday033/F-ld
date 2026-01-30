using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EnemyCombatActionManager : MonoBehaviour
{
	public static List<CombatAction> enemyCombatActionQueue = new List<CombatAction>();
    public static List<CombatAction> slowedEnemyCombatActionQueue = new List<CombatAction>();

	
    [RuntimeInitializeOnLoadMethod]
    private static void instantiatePlayerCombatActionManager()
    {
        enemyCombatActionQueue = new List<CombatAction>();
	    slowedEnemyCombatActionQueue = new List<CombatAction>();
    }

	public void decideEnemyCombatActions()
	{
		List<Stats> listOfEnemies = CombatGrid.getAllAliveEnemyCombatants();
		List<CombatAction> lowPriorityAttacks = new List<CombatAction>();
        List<CombatAction> slowedAttacks = new List<CombatAction>();

        foreach (EnemyStats enemy in listOfEnemies)
		{			
			if(Helpers.hasQuality<Trait>(enemy.traitContainer, t => t.isPacifist()) || 
				containsCombatActionFromPosition(enemy.position) || enemy.isPartOfVolley())
			{
				continue; //if enemy failed to find a target, it shouldn't do anything
			}
			CombatAction enemyCombatAction = enemy.getCombatAction();
			enemyCombatAction.setActor(enemy);
			Selector selector = enemyCombatAction.getTargetSelector(); 
			
			if(selector == null)
			{
				continue; //if enemy failed to find a target, it shouldn't do anything
			}
			
			enemyCombatAction.setSelector(selector);
			
			enemyCombatAction.queueingAction();

			if (enemyCombatAction.actorIsSlowed())
			{
				slowedAttacks.Add(enemyCombatAction);

            } else if(enemyCombatAction.actorIsPriorityAttacker())
			{
				enemyCombatActionQueue.Insert(0, enemyCombatAction);

			} else if(enemyCombatAction.actorIsLowPriorityAttacker())
			{
				lowPriorityAttacks.Add(enemyCombatAction);
			} else
			{
				enemyCombatActionQueue.Add(enemyCombatAction);
			}
		}
		
		foreach(CombatAction action in lowPriorityAttacks)
		{
			enemyCombatActionQueue.Add(action);
		}
		
		enemyCombatActionQueue = EnvironmentalCombatActionManager.getInstance().getAllEnvironmentalCombatActions(enemyCombatActionQueue);
		slowedEnemyCombatActionQueue = slowedAttacks;
	}
	
	private bool containsCombatActionFromPosition(GridCoords actorCoords)
	{
		foreach(CombatAction action in enemyCombatActionQueue)
		{
			if(action.getActorCoords().Equals(actorCoords))
			{
				return true;
			}
		}

        foreach (CombatAction action in slowedEnemyCombatActionQueue)
        {
            if (action.getActorCoords().Equals(actorCoords))
            {
                return true;
            }
        }

        return false;
	}
	
	public static void applyLinkDamage()
	{
		List<Stats> listOfEnemies = CombatGrid.getAllAliveEnemyCombatants();
		
		foreach(EnemyStats enemy in listOfEnemies)
		{
			double linkedPercentage = enemy.getLinkedPercentage();
			
			if(linkedPercentage > 0.0)
			{
				enemy.modifyCurrentHealth((int) ((double) enemy.getTotalHealth() * linkedPercentage));
			}
		}
	}
}
