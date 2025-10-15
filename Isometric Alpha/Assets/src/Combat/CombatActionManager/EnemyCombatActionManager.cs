using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EnemyCombatActionManager : MonoBehaviour
{
	/*
		public int totalEnemyCount; //number of currently alive enemies. Should always be equal to mainEnemyCount + minionCount
		public int mainEnemyCount;  //number of full enemies whose turn is determined on enemy main phase
		public int minionCount;		//number of minions whose turn is determined on enemy minion phase
	*/
	
	public static ArrayList enemyCombatActionQueue = new ArrayList();
    public static ArrayList slowedEnemyCombatActionQueue = new ArrayList();

    public SelectorManager selectorManager;
	
	public static int actualEnemyMasterCombatActionCount()
	{
		int masterCombatActionCount = 0;
		
		foreach(CombatAction action in enemyCombatActionQueue)
		{
			EnemyStats enemyStats = (EnemyStats) action.getActorStats();
			
			if(enemyStats == null)
			{
				continue;
			}
			string traitName = enemyStats.traitNames[0];
	
	
			if(traitName.Equals("Master"))
			{
				masterCombatActionCount++;
			}
		}

        foreach (CombatAction action in slowedEnemyCombatActionQueue)
        {
            EnemyStats enemyStats = (EnemyStats) action.getActorStats();

            if (enemyStats == null)
            {
                continue;
            }
            string traitName = enemyStats.traitNames[0];


            if (traitName.Equals("Master"))
            {
                masterCombatActionCount++;
            }
        }

        return masterCombatActionCount;
	}
	
	public static int getTotalAliveEnemyCount()
	{
		return CombatGrid.getAllAliveEnemyCombatants().Count;
	}
	
	//method to populate CombatStateManager.EnemyCombatActionQueue
	//runs through all enemies in listOfEnemies and assigns them actions
	//eventually will need to only provide actions for the amount of enemies based on
	//some enemies leadership stat. For now all enemies attack the player
	public void decideEnemyCombatActions()
	{
		ArrayList listOfEnemies = CombatGrid.getAllAliveEnemyCombatants();
		ArrayList lowPriorityAttacks = new ArrayList();
        ArrayList slowedAttacks = new ArrayList();

        foreach (EnemyStats enemy in listOfEnemies)
		{			
			if(Helpers.hasQuality<Trait>(enemy.traits, t => t.isPacifist()) || 
				containsCombatActionFromPosition(enemy.position) || enemy.isPartOfVolley())
			{
				//Debug.LogError("Pacifistic enemy found");
				continue; //if enemy failed to find a target, it shouldn't do anything
			}
			CombatAction enemyCombatAction = enemy.getCombatAction();
			enemyCombatAction.setActor(enemy);
			Selector selector = enemyCombatAction.getTargetSelector(); 
			
			if(selector == null)
			{
				//Debug.LogError("selector == null, skipping enemy");
				continue; //if enemy failed to find a target, it shouldn't do anything
			}
			
			enemyCombatAction.setSelector(selector);
			//enemyCombatAction.setTargetCoords(new GridCoords(selector.currentRow, selector.currentCol));
			
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
		ArrayList listOfEnemies = CombatGrid.getAllAliveEnemyCombatants();
		
		foreach(EnemyStats enemy in listOfEnemies)
		{
			double linkedPercentage = 0.0;
			
			if(enemy.hasTrait(TraitList.mobLinked) >= 0)
			{
				linkedPercentage = enemy.getTraits()[enemy.hasTrait(TraitList.mobLinked)].getLinkedPercentage();
				
			} else if(enemy.hasTrait(TraitList.bossLinked) >= 0)
			{
				linkedPercentage = enemy.getTraits()[enemy.hasTrait(TraitList.bossLinked)].getLinkedPercentage();
			}
			
			if(linkedPercentage > 0.0)
			{
				enemy.modifyCurrentHealth((int) ((double) enemy.getTotalHealth() * linkedPercentage));
			}
		}
	}
}
