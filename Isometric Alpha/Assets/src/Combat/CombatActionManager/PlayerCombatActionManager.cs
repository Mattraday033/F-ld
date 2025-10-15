using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatActionManager : MonoBehaviour
{	
	public static ArrayList playerCombatActionQueue = new ArrayList();

	public CombatStateManager combatStateManager;
	
	public SelectorManager selectorManager;

	public bool activated = false;

	private static PlayerCombatActionManager instance;

	public static PlayerCombatActionManager getInstance()
	{
		return instance;
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
		
		playerCombatActionQueue.Add(action);
		
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
		if (playerCombatActionQueue.Count == 0)
		{
			return;
		}

		CombatAction actionToBeRemoved = (CombatAction)playerCombatActionQueue[playerCombatActionQueue.Count - 1];

		actionToBeRemoved.unqueueingAction();

		playerCombatActionQueue.RemoveAt((playerCombatActionQueue.Count - 1));

		if (CombatStateManager.currentActivity == CurrentActivity.Finished)
		{
			CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);
		}

		CombatUI.populateCombatActionPanels();

		CombatUI.checkAndSetResolveTurnButtonInteractability();

		if (RepositionUIManager.getInstance() != null)
		{
			RepositionUIManager.getInstance().updateOnStateChange();
		}
		else
		{
			//Debug.Log("RepositionUIManager.getInstance() == null");
		}
		
		SelectorManager.createPressEPrompt();
	}

	public void queueCombatActionWithTertiary(Selector actorSelector, Selector tertiarySelector, CombatAction action)
	{
		action.setActor(CombatGrid.getCombatantAtCoords(actorSelector.currentRow, actorSelector.currentCol));
		
		Selector targetSelector = action.getSelector().clone();
		
		targetSelector.setToLocation(action.getTargetCoords());
		
		action.setSelector((Selector) targetSelector.Clone());
		
		playerCombatActionQueue.Add(action);
		
		CombatUI.populateCombatActionPanels();
		
		action.queueingAction();
		
		CombatUI.checkAndSetResolveTurnButtonInteractability();
	}

	public static void removeAllPlayerActions()
	{
		for(int index = playerCombatActionQueue.Count-1; index >= 0 && playerCombatActionQueue.Count > 0; index--)
		{
			getInstance().removeLastCombatActionFromPlayerCombatActionQueue();
		}
	}
}
