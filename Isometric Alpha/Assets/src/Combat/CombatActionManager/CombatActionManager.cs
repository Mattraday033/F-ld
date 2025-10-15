using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionManager : MonoBehaviour
{
	public static CombatActionManager instance;
	
	public EnemyCombatActionManager enemyCombatActionManager;
	public PlayerCombatActionManager playerCombatActionManager;	
	public SummonsCombatActionManager summonedCombatActionManager;

    public static ArrayList critCombatActionQueue = new ArrayList();
    public static ArrayList onDeathCombatActionQueue = new ArrayList();
	public static ArrayList lockedInCombatActionQueue = new ArrayList();
	
	public static Coroutine currentWait;

	public static Stats currentActor;
	
	public static CombatActionManager getInstance()
	{
		return instance;
	}
	
	public CombatAction determineNextCombatAction()
	{		
		CombatAction nextCombatAction = null;
		
		checkOnDeathCombatActionsForAliveActors();
		
		if(critCombatActionQueue.Count > 0)
		{
			nextCombatAction = removeNextCombatActionFromQueue(critCombatActionQueue);
        } else if(onDeathCombatActionQueue.Count > 0)
		{
			nextCombatAction = removeNextCombatActionFromQueue(onDeathCombatActionQueue);
		} else
		{
			ArrayList actions = getCombatActionOrder();
			
			if(actions.Count == 0)
			{
				return null;
			}
			
			nextCombatAction = removeNextCombatActionFromQueue(actions);
		}

		nextCombatAction.activatingAction();

		currentActor = nextCombatAction.getActorStats();

		return nextCombatAction;
	}

	public void resolveACombatAction()
	{
		CombatAction actionBeingResolved = determineNextCombatAction();
		
		if(actionBeingResolved == null)
		{
			CombatStateManager.getInstance().endResolvingPhase();
			return; //if determineNextCombatAction() returns null then that means both action queues are empty. 
		}
		
		if(!actionBeingResolved.getActorStats().isStunned())
		{
			actionBeingResolved.performCombatAction();
			actionBeingResolved.setCooldownToMax();
			actionBeingResolved.chargeActorActionCost();

            if (actionBeingResolved.getActorStats() != null && 
				Helpers.hasQuality<Trait>(actionBeingResolved.getActorStats().traits, t => t.deleteIfDead()))
			{
				CombatGrid.deleteDeadOnDeathEffectActors();
			}
		}
		
		currentWait = StartCoroutine(waitOnCombatAction());
	}
	
	public IEnumerator waitOnCombatAction()
	{
        do{
            yield return null;
        }while(CombatAnimationManager.getInstance().hasOngoingAnimations());
		
		currentWait = null;
		
		CombatUI.populateCombatActionPanels();
		
		if(CombatStateManager.whoseTurn == WhoseTurn.Resolving)
		{
			resolveACombatAction();
		}
		
		yield break;
    }
	
	public void decideAndShowEnemyCombatActions()
	{
		decideAndShowEnemyCombatActions(true);
	}
	
	public void decideAndShowEnemyCombatActions(bool populateCombatActionPanels)
	{
		if(!CombatStateManager.isPlayerSurpriseRound()) 
		{
			enemyCombatActionManager.decideEnemyCombatActions();
		}
		
		if(populateCombatActionPanels)
		{
			CombatUI.populateCombatActionPanels();
		}
	}
	
	public void decideAndShowSummonedCombatActions()
	{
		decideAndShowSummonedCombatActions(true);
	}
	
	public void decideAndShowSummonedCombatActions(bool populateCombatActionPanels)
	{
		summonedCombatActionManager.updateSummonedCombatActions();
		
		if(populateCombatActionPanels)
		{
			CombatUI.populateCombatActionPanels();
		}
	}
	
	public void checkOnDeathCombatActionsForAliveActors()
	{
		for(int actionIndex = onDeathCombatActionQueue.Count-1; actionIndex >= 0; actionIndex--)
		{
			Stats currentActor = ((CombatAction) onDeathCombatActionQueue[actionIndex]).getActorStats();
			
			if(currentActor == null || currentActor is null || !currentActor.isDead)
			{
				onDeathCombatActionQueue.RemoveAt(actionIndex);
			}
		}

	}
	
	public static bool actorAlreadyHasCombatAction(GridCoords coords)
	{
		ArrayList actions = getCombatActionOrder();
		
		foreach(CombatAction action in actions)
		{
			if(action.getActorCoords().Equals(coords))
			{
				return true;
			}
		}
		
		return false;
	}
	
	// eventually will need to track alive party members as well probably
	public static bool finishedChoosingPartyMemberCombatActions()
	{
		return getNumberOfCurrentPartyCombatActions() >= PartyStats.getPartyMemberCombatActionSlots() || 
				getNumberOfCurrentPartyCombatActions() >= (CombatGrid.getAllAliveNonsummonedAllies().Count-1);
	}
	
	public void promptLaterCombatActionsToFindNewTarget()
	{
		Stats mandatoryTargetAlly = CombatGrid.allyHasMandatoryTarget();

		if (mandatoryTargetAlly == null)
		{
			return;
		}

		for (int index = PlayerCombatActionManager.playerCombatActionQueue.Count - 1; index < EnemyCombatActionManager.enemyCombatActionQueue.Count; index++)
		{
			findNewTarget((CombatAction)EnemyCombatActionManager.enemyCombatActionQueue[index]);
		}

        for (int index = 0; index < EnemyCombatActionManager.slowedEnemyCombatActionQueue.Count; index++)
        {
            findNewTarget((CombatAction)EnemyCombatActionManager.slowedEnemyCombatActionQueue[index]);
        }
    }

	private void findNewTarget(CombatAction action)
	{
		if (action == null || action.targetsOnlyEmptySpace())
		{
			return;
		}

		EnemyStats enemyActor = (EnemyStats)CombatGrid.getCombatantAtCoords(action.getActorCoords());
		action.addPreviousTarget(action.getTargetCoords());
		//action.setTargetCoords(action.getTargetSelector().getCoords());
		action.getSelector().setToLocation(action.getTargetSelector().getCoords());
	}
	
	public void promptLaterCombatActionsToReturnToPreviousTarget()
	{
		for (int index = PlayerCombatActionManager.playerCombatActionQueue.Count - 1; index < EnemyCombatActionManager.enemyCombatActionQueue.Count; index++)
		{
			setActionToPreviousTarget((CombatAction)EnemyCombatActionManager.enemyCombatActionQueue[index]);
		}

		for (int index = 0; index < EnemyCombatActionManager.slowedEnemyCombatActionQueue.Count; index++)
		{
			setActionToPreviousTarget((CombatAction)EnemyCombatActionManager.slowedEnemyCombatActionQueue[index]);
		}
	}

	private void setActionToPreviousTarget(CombatAction action)
	{
		if (action == null || action.previousTargets.Count <= 0)
		{
			return;
		}

		//action.setTargetCoords(action.getPreviousTarget());
		action.getSelector().setToLocation(action.getPreviousTarget());
	}
	
	public static ArrayList getCombatActionOrder()
	{
		if (lockedInCombatActionQueue.Count > 0)
		{
			return lockedInCombatActionQueue;
		}
		else
		{
			return createCombatActionOrder();
		}
	}	
	
	public static ArrayList createCombatActionOrder()
	{
		ArrayList actionOrder = new ArrayList();
		
		DeadCombatantManager.getInstance().removeDeadCombatantCombatActions(PlayerCombatActionManager.playerCombatActionQueue);
		DeadCombatantManager.getInstance().removeDeadCombatantCombatActions(EnemyCombatActionManager.enemyCombatActionQueue);
        DeadCombatantManager.getInstance().removeDeadCombatantCombatActions(EnemyCombatActionManager.slowedEnemyCombatActionQueue);

        for (int actionIndex = 0; actionIndex < PlayerCombatActionManager.playerCombatActionQueue.Count || actionIndex < EnemyCombatActionManager.enemyCombatActionQueue.Count; actionIndex++)  
		{  
			if(actionIndex < PlayerCombatActionManager.playerCombatActionQueue.Count)
			{
				actionOrder.Add(PlayerCombatActionManager.playerCombatActionQueue[actionIndex]);
			}
			if(actionIndex < EnemyCombatActionManager.enemyCombatActionQueue.Count)
			{
				actionOrder.Add(EnemyCombatActionManager.enemyCombatActionQueue[actionIndex]);
			}
		}
		
		foreach(CombatAction action in SummonsCombatActionManager.alliedSummonsCombatActionQueue)
		{
			actionOrder.Add(action);
		}
		
		foreach(CombatAction action in SummonsCombatActionManager.enemySummonsCombatActionQueue)
		{
			actionOrder.Add(action);
		}

        foreach (CombatAction action in EnemyCombatActionManager.slowedEnemyCombatActionQueue)
        {
            actionOrder.Add(action);
        }

        return actionOrder;
	}
	
	public static void addOnDeathCombatAction(CombatAction action)
	{
		if(onDeathCombatActionQueueHasCombatActionAtCoords(action.getActorCoords()))
		{
			return;
		}
		
		onDeathCombatActionQueue.Add(action);
	}

    public static void addCritCombatAction(CombatAction action)
    {
        critCombatActionQueue.Add(action);
    }

    public static void lockInCombatActionOrder()
	{
		lockedInCombatActionQueue = createCombatActionOrder();
		wipeAllCombatActions();
	}

	public static void wipeAllCombatActions()
	{
		PlayerCombatActionManager.playerCombatActionQueue = new ArrayList();
		EnemyCombatActionManager.enemyCombatActionQueue = new ArrayList();
        EnemyCombatActionManager.slowedEnemyCombatActionQueue = new ArrayList();
        SummonsCombatActionManager.alliedSummonsCombatActionQueue = new ArrayList();
		SummonsCombatActionManager.enemySummonsCombatActionQueue = new ArrayList();
	}

	public static void wipeLockedInCombatActionQueue()
	{
		lockedInCombatActionQueue = new ArrayList();
	}
	
	private static int getNumberOfCurrentPartyCombatActions()
	{
		int numberOfPartyCombatActions = 0;

		ArrayList actions = getCombatActionOrder();

		foreach (CombatAction action in actions)
		{
			if (CombatGrid.getCombatantAtCoords(action.getActorCoords()) != null &&
				CombatGrid.getCombatantAtCoords(action.getActorCoords()).costsPartyCombatActions())
			{
				numberOfPartyCombatActions++;
			}
		}

		return numberOfPartyCombatActions;
	}
	
	private static CombatAction removeNextCombatActionFromQueue(ArrayList actionQueue)
	{
        CombatAction nextCombatAction = (CombatAction)actionQueue[0];
        actionQueue.RemoveAt(0);

		return nextCombatAction;
    }

	private static bool onDeathCombatActionQueueHasCombatActionAtCoords(GridCoords coords)
	{
		foreach(CombatAction action in onDeathCombatActionQueue)
		{
			if(action.getActorCoords().Equals(coords))
			{
				return true;
			}
		}
		
		return false;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogError("Found more than one CombatActionManager in the scene.");
		}
		
		instance = this;
	}
	
}
