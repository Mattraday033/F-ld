using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatActionManager : MonoBehaviour
{	
	public static List<CombatAction> playerCombatActionQueue = new List<CombatAction>();
	public static List<CombatAction> slowedPlayerCombatActionQueue = new List<CombatAction>();
	public static List<Stats> orderOfActorsAddedToQueue = new List<Stats>();

	public CombatStateManager combatStateManager;
	
	public SelectorManager selectorManager;

	public bool activated = false;

	private static PlayerCombatActionManager instance;

	public static PlayerCombatActionManager getInstance()
	{
		return instance;
	}
    [RuntimeInitializeOnLoadMethod]
    private static void instantiatePlayerCombatActionManager()
    {
        playerCombatActionQueue = new List<CombatAction>();
	    slowedPlayerCombatActionQueue = new List<CombatAction>();
	    orderOfActorsAddedToQueue = new List<Stats>();

        instance = null;
    }

    private void Awake()
    {
        if(instance != null)
		{
			Debug.LogError("Duplicate instances of PlayerCombatActionManager exist erroneously.");
		}

		instance = this;
    }

	public void queueCombatAction(Selector actorSelector, Selector targetSelector, CombatAction action)
	{
		action.setActor(CombatGrid.getCombatantAtCoords(actorSelector.getCoords()));
		
		action.setSelector(targetSelector.clone());
		
        if(action.actorIsSlowed())
        {
            slowedPlayerCombatActionQueue.Add(action);
        } else
        {
            playerCombatActionQueue.Add(action);
        }
		
        orderOfActorsAddedToQueue.Add(action.actorStats);

		CombatUI.populateCombatActionPanels();
		
		action.queueingAction();
		
		CombatUI.checkAndSetResolveTurnButtonInteractability();
		
		if(combatStateManager.shouldMoveToFinished())
		{
			CombatStateManager.setCurrentActivity(CurrentActivity.Finished);
		}
	}

    public static bool playerHasActionsInQueue()
    {
        return playerCombatActionQueue.Count > 0 || slowedPlayerCombatActionQueue.Count > 0 || 
            orderOfActorsAddedToQueue.Count > 0;
    }

	public static void removeLastCombatActionFromPlayerCombatActionQueue()
	{
		if (!playerHasActionsInQueue())
		{
			return;
		}

        Stats actorToRemoveFromQueue = orderOfActorsAddedToQueue[orderOfActorsAddedToQueue.Count-1];
        orderOfActorsAddedToQueue.RemoveAt(orderOfActorsAddedToQueue.Count-1);

        List<CombatAction> currentActionQueue;

        if(actorToRemoveFromQueue.isSlowed())
        {
            currentActionQueue = slowedPlayerCombatActionQueue;
        } else
        {
            currentActionQueue = playerCombatActionQueue;
        }

        try
        {
            CombatAction actionToBeRemoved = currentActionQueue[currentActionQueue.Count - 1];

            actionToBeRemoved.unqueueingAction();

            currentActionQueue.RemoveAt(currentActionQueue.Count - 1);

            if (CombatStateManager.currentActivity == CurrentActivity.Finished)
            {
                CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);
            }

            CombatUI.populateCombatActionPanels();

            CombatUI.checkAndSetResolveTurnButtonInteractability();
            
            SelectorManager.createPressEPrompt();
        } catch(Exception e)
        {
            Debug.LogError("Exception found");
        }
	}

	public void queueCombatActionWithTertiary(Selector actorSelector, Selector tertiarySelector, CombatAction action)
	{
		action.setActor(CombatGrid.getCombatantAtCoords(actorSelector.getCoords()));
		
		Selector targetSelector = action.getSelector().clone();
		
		targetSelector.setToLocation(action.getTargetCoords(), declareSelectors: false);
		
		action.setSelector((Selector) targetSelector.Clone());
		
        if(action.actorIsSlowed())
        {
            slowedPlayerCombatActionQueue.Add(action);
        } else
        {
            playerCombatActionQueue.Add(action);
        }
		
        orderOfActorsAddedToQueue.Add(action.actorStats);
		
		CombatUI.populateCombatActionPanels();
		
		action.queueingAction();
		
		CombatUI.checkAndSetResolveTurnButtonInteractability();
	}

	public static void removeAllPlayerActions()
	{
		for(int index = orderOfActorsAddedToQueue.Count-1; index >= 0 && orderOfActorsAddedToQueue.Count > 0; index--)
		{
			removeLastCombatActionFromPlayerCombatActionQueue();
		}

        playerCombatActionQueue = new List<CombatAction>();
	    slowedPlayerCombatActionQueue = new List<CombatAction>();
	    orderOfActorsAddedToQueue = new List<Stats>();
	}

    public static bool actorHasActionsInQueue(Stats actor)
    {
        if(actor == null)
        {
            return false;
        }

        List<CombatAction> actions = new List<CombatAction>();
        actions.AddRange(playerCombatActionQueue);
        actions.AddRange(slowedPlayerCombatActionQueue);

        foreach(CombatAction action in actions)
        {
            if(action != null && actor.Equals(action.getActorStats()))
            {
                return true;
            }
        }


        return false;
    }
}
