using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionManager : MonoBehaviour
{
    public const float standardWaitBetweenCombatActions = .35f;

    private static bool _SkipWaitBetweenCombatActions;
    public static bool skipWaitBetweenCombatActions
    {
        get
        {
            return _SkipWaitBetweenCombatActions;
        }
        set
        {
            _SkipWaitBetweenCombatActions = value;
        }
    }

	public static CombatActionManager instance; 
	
	public EnemyCombatActionManager enemyCombatActionManager;
	public PlayerCombatActionManager playerCombatActionManager;	
	public SummonsCombatActionManager summonedCombatActionManager;

    public static List<CombatAction> critCombatActionQueue = new List<CombatAction>();
    public static List<CombatAction> onDeathCombatActionQueue = new List<CombatAction>();
	public static List<CombatAction> lockedInCombatActionQueue = new List<CombatAction>();
	
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
		} else if(CombatStateManager.whoseTurn == WhoseTurn.Resolving)
		{
			List<CombatAction> actions = getCombatActionOrder();
			
			if(actions.Count == 0)
			{
				return null;
			}
			
			nextCombatAction = removeNextCombatActionFromQueue(actions);
		} else
        {
            return null;
        }

        if(nextCombatAction.hasAssignedActor(out currentActor))
        {
            nextCombatAction.activatingAction();

		    return nextCombatAction;
        } else
        {
            return null;
        }
	}

	public void resolveACombatAction()
	{
		CombatAction actionBeingResolved = determineNextCombatAction();
		
		if(actionBeingResolved == null)
		{
			CombatStateManager.getInstance().endResolvingPhase();
			return; //if determineNextCombatAction() returns null then that means both action queues are empty. 
		}
		
		if(actionBeingResolved.hasAssignedActor(out Stats actor) && !actor.isStunned())
		{
			actionBeingResolved.performCombatAction();
			actionBeingResolved.setCooldownToMax();

            if (actor != null && 
				Helpers.hasQuality<Trait>(actor.traitContainer, t => t.deleteIfDead()))
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

        float timeElapsed = 0;

        while(timeElapsed < getWaitBetweenCombatActions())
        {
            yield return null;

            timeElapsed += Time.deltaTime;
        }

		CombatUI.populateCombatActionPanels();
		
		if(CombatStateManager.whoseTurn == WhoseTurn.Resolving || 
            CombatStateManager.whoseTurn == WhoseTurn.TickDown)
		{
			resolveACombatAction();
		}
    }

    public static float getWaitBetweenCombatActions()
    {
        if(skipWaitBetweenCombatActions)
        {
            return 0f;
        }

        return standardWaitBetweenCombatActions;
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
			if(!onDeathCombatActionQueue[actionIndex].hasAssignedActor(out Stats actor) || actor.isAlive())
			{
				onDeathCombatActionQueue.RemoveAt(actionIndex);
			}
		}
	}
	
	public static bool actorAlreadyHasCombatAction(GridCoords coords)
	{
		List<CombatAction> actions = getCombatActionOrder();
		
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
				getNumberOfCurrentPartyCombatActions() >= CombatGrid.getAllAliveNonsummonedAllies().Count;
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
			findNewTarget(EnemyCombatActionManager.enemyCombatActionQueue[index]);
		}

        for (int index = 0; index < EnemyCombatActionManager.slowedEnemyCombatActionQueue.Count; index++)
        {
            findNewTarget(EnemyCombatActionManager.slowedEnemyCombatActionQueue[index]);
        }
    }

	private void findNewTarget(CombatAction action)
	{
		if (action == null || action.targetsOnlyEmptySpace())
		{
			return;
		}

		// EnemyStats enemyActor = (EnemyStats)CombatGrid.combatantExistsAtCoords(action.getActorCoords());
		action.addPreviousTarget(action.getTargetCoords());
		//action.setTargetCoords(action.getTargetSelector().getCoords());
		action.getSelector().setToLocation(action.getTargetSelector().getCoords());
	}
	
	public void promptLaterCombatActionsToReturnToPreviousTarget()
	{
		for (int index = PlayerCombatActionManager.playerCombatActionQueue.Count - 1; index < EnemyCombatActionManager.enemyCombatActionQueue.Count; index++)
		{
			setActionToPreviousTarget(EnemyCombatActionManager.enemyCombatActionQueue[index]);
		}

		for (int index = 0; index < EnemyCombatActionManager.slowedEnemyCombatActionQueue.Count; index++)
		{
			setActionToPreviousTarget(EnemyCombatActionManager.slowedEnemyCombatActionQueue[index]);
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
	
	public static List<CombatAction> getCombatActionOrder()
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
	
	public static List<CombatAction> createCombatActionOrder()
	{
		List<CombatAction> actionOrder = new List<CombatAction>();
		
		DeadCombatantManager.getInstance().removeDeadCombatantCombatActions(PlayerCombatActionManager.playerCombatActionQueue);
        DeadCombatantManager.getInstance().removeDeadCombatantCombatActions(PlayerCombatActionManager.slowedPlayerCombatActionQueue);
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

        foreach (CombatAction action in PlayerCombatActionManager.slowedPlayerCombatActionQueue)
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
		PlayerCombatActionManager.playerCombatActionQueue = new List<CombatAction>();
        PlayerCombatActionManager.slowedPlayerCombatActionQueue = new List<CombatAction>();

        PlayerCombatActionManager.orderOfActorsAddedToQueue = new List<Stats>();

		EnemyCombatActionManager.enemyCombatActionQueue = new List<CombatAction>();
        EnemyCombatActionManager.slowedEnemyCombatActionQueue = new List<CombatAction>();
        
        SummonsCombatActionManager.alliedSummonsCombatActionQueue = new List<CombatAction>();
		SummonsCombatActionManager.enemySummonsCombatActionQueue = new List<CombatAction>();
	}

	public static void wipeLockedInCombatActionQueue()
	{
		lockedInCombatActionQueue = new List<CombatAction>();
	}
	
	private static int getNumberOfCurrentPartyCombatActions()
	{
		int numberOfPartyCombatActions = 0;

		List<CombatAction> actions = getCombatActionOrder();

		foreach (CombatAction action in actions)
		{
			if (action.hasAssignedActor(out Stats actor) &&
				actor.costsPartyCombatActions())
			{
				numberOfPartyCombatActions++;
			}
		}

		return numberOfPartyCombatActions;
	}
	
	private static CombatAction removeNextCombatActionFromQueue(List<CombatAction> actionQueue)
	{
        CombatAction nextCombatAction = actionQueue[0];
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

    private static void useWaitBetweenCombatActions()
    {
        skipWaitBetweenCombatActions = false;
    }

	private void Awake()
	{
		if(instance != null)
		{
			Debug.LogError("Found more than one CombatActionManager in the scene.");
		}
		
		instance = this;
        CombatStateManager.OnNewTurn.AddListener(useWaitBetweenCombatActions);
        CombatStateManager.OnCombatEnd.AddListener(useWaitBetweenCombatActions);
	}
	
}
