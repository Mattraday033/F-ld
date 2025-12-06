using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatActionManager : MonoBehaviour
{	
	public static List<CombatAction> playerCombatActionQueue = new List<CombatAction>();
	public static List<CombatAction> slowedPlayerCombatActionQueue = new List<CombatAction>();
	public static List<Stats> orderOfActionsAddedToQueue = new List<Stats>();

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
	    orderOfActionsAddedToQueue = new List<Stats>();

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

    void Update() //here for Key Input
	{
        switch (CombatStateManager.currentActivity)
		{
			case CurrentActivity.ChoosingActor:
                KeyPressManager.updateKeyBools();

                if (KeyPressManager.handlingPrimaryKeyPress)
                {
                    return;
                }

                if (KeyBindingList.eitherBackoutKeyIsPressed())
                {
                    removeLastCombatActionFromPlayerCombatActionQueue();
                    KeyPressManager.handlingPrimaryKeyPress = true;
                }
                break;
            case CurrentActivity.Waiting:
            case CurrentActivity.ChoosingAbility:
			case CurrentActivity.ChoosingLocation:
			case CurrentActivity.ChoosingTertiary:
            case CurrentActivity.Retreating:
            case CurrentActivity.Repositioning:
			case CurrentActivity.Finished:
				return;
		}
	}

	public void queueCombatAction(Selector actorSelector, Selector targetSelector, CombatAction action)
	{
		action.setActor(CombatGrid.getCombatantAtCoords(actorSelector.currentRow, actorSelector.currentCol));
		
		action.setSelector(targetSelector.clone());
		
        if(action.actorIsSlowed())
        {
            slowedPlayerCombatActionQueue.Add(action);
        } else
        {
            playerCombatActionQueue.Add(action);
        }
		
        orderOfActionsAddedToQueue.Add(action.actorStats);

		CombatUI.populateCombatActionPanels();
		
		action.queueingAction();
		
		CombatUI.checkAndSetResolveTurnButtonInteractability();
		
		if(combatStateManager.shouldMoveToFinished())
		{
			CombatStateManager.setCurrentActivity(CurrentActivity.Finished);
		}
	}

	public void removeLastCombatActionFromPlayerCombatActionQueue()
	{
		if ((playerCombatActionQueue.Count == 0 && slowedPlayerCombatActionQueue.Count == 0) || 
            orderOfActionsAddedToQueue.Count == 0)
		{
			return;
		}

        Stats actorToRemoveFromQueue = orderOfActionsAddedToQueue[orderOfActionsAddedToQueue.Count-1];
        orderOfActionsAddedToQueue.RemoveAt(orderOfActionsAddedToQueue.Count-1);

        List<CombatAction> currentActionQueue;

        if(actorToRemoveFromQueue.isSlowed())
        {
            currentActionQueue = slowedPlayerCombatActionQueue;
        } else
        {
            currentActionQueue = playerCombatActionQueue;
        }

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
	}

	public void queueCombatActionWithTertiary(Selector actorSelector, Selector tertiarySelector, CombatAction action)
	{
		action.setActor(CombatGrid.getCombatantAtCoords(actorSelector.currentRow, actorSelector.currentCol));
		
		Selector targetSelector = action.getSelector().clone();
		
		targetSelector.setToLocation(action.getTargetCoords());
		
		action.setSelector((Selector) targetSelector.Clone());
		
        if(action.actorIsSlowed())
        {
            slowedPlayerCombatActionQueue.Add(action);
        } else
        {
            playerCombatActionQueue.Add(action);
        }
		
        orderOfActionsAddedToQueue.Add(action.actorStats);
		
		CombatUI.populateCombatActionPanels();
		
		action.queueingAction();
		
		CombatUI.checkAndSetResolveTurnButtonInteractability();
	}

	public static void removeAllPlayerActions()
	{
		for(int index = orderOfActionsAddedToQueue.Count-1; index >= 0 && orderOfActionsAddedToQueue.Count > 0; index--)
		{
			getInstance().removeLastCombatActionFromPlayerCombatActionQueue();
		}

        playerCombatActionQueue = new List<CombatAction>();
	    slowedPlayerCombatActionQueue = new List<CombatAction>();
	    orderOfActionsAddedToQueue = new List<Stats>();
	}
}
