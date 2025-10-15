using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{

	//public GameObject stateInfoUIBackgroundPanel;

	public PlayerCombatActionCounterManager playerCombatActionCounterManager;
	public ScrollableUIElement actionOrderGrid;

	public Button resolveTurnButton;
	
	public Transform descriptionPanelParent;
	public ArrayList descriptionPanels;

	public static Transform selectorParent;
	
	public static CombatResultsPopUpButton combatResultsPopUpButton;
	
	private static CombatUI instance;

	public static Transform getDescriptionPanelParent()
	{
		return getInstance().descriptionPanelParent;
	}
	
	public static void setCombatActionCounterPanelsToDefault()
	{
		getInstance().playerCombatActionCounterManager.setCombatActionCounterPanelsToDefault();
	}
	
	public static void checkAndSetResolveTurnButtonInteractability()
	{
		getInstance().resolveTurnButton.interactable = CombatStateManager.canResolveTurn();
	}

    public static void setCurrentActivityText(CurrentActivity currentActivity)
    {
        
	}
	
	public static void setTurnInfoText(WhoseTurn whoseTurn)
	{
        
	}
	
	public static void populateCombatActionPanels()
	{
		ArrayList actionOrder = CombatActionManager.getCombatActionOrder();
		
		getInstance().actionOrderGrid.populatePanels(actionOrder);
		
		if(CombatStateManager.whoseTurn == WhoseTurn.Player)
		{	
			getInstance().playerCombatActionCounterManager.updateCombatActionCounterPanels(actionOrder);
		}
	}
	
	public static CombatUI getInstance()
	{
		return instance;
	}

	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("Another instance of CombatUI was found");
		}
		
		instance = this;
		combatResultsPopUpButton = new CombatResultsPopUpButton();
		descriptionPanels = new ArrayList();
	}
}
