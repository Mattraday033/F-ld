using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum CurrentRepositionType {NotRepositioning = 1, SingleAlly = 2, AllyFormation = 3, EnemyFormation = 4}
public enum CurrentRepositionActivity {NotRepositioning = 1, ChoosingRepositionTarget = 2, ChoosingNewLocation = 3}

public struct RepositionDetails
{
	public Stats combatantToReposition;
	public GridCoords repositionDestination;
	public GameObject placeHolderObject;
	
	public RepositionDetails(Stats combatantToReposition, GridCoords repositionDestination)
	{
		this.combatantToReposition = combatantToReposition;
		this.repositionDestination = repositionDestination;
		this.placeHolderObject = RepositionPlaceholderGenerator.generatePlaceholderObject(combatantToReposition, repositionDestination);
	}
	
	public void destroyPlaceHolder()
	{
		GameObject.Destroy(placeHolderObject);
	}
}

public class RepositionManager : MonoBehaviour, INeedsUpdateOnStateChange
{
	private static RepositionManager instance;
	
	public RepositionUIManager repositionUIManager;

	public static CurrentRepositionType currentRepositionType;
	public static CurrentRepositionActivity currentRepositionActivity;

	public static CombatAction currentSingleTargetRepositionCombatAction;

	private int repositionsRemaining;
	private const bool activate = true;
	private const bool deactivate = false;
	private const bool dontPopulateCombatActionPanels = false;
	
	private ArrayList repositionDetailsList = new ArrayList();
	private ArrayList currentSetOfCombatantsToReposition = new ArrayList();
	
	void Start()
	{
		resetRepositioningTypeAndActivity();
		
		repositionsRemaining = PartyManager.getPlayerStats().getMaximumRepositionsPerCombat();
	}
	
	void Update() //here for Key Input
	{
		if(CombatStateManager.currentActivity == CurrentActivity.Repositioning)
		{
			KeyPressManager.updateKeyBools();
			
			if(KeyPressManager.handlingPrimaryKeyPress)
			{
				return;
			}
			
			if(currentRepositionType == CurrentRepositionType.SingleAlly)
			{
				if(currentRepositionActivity == CurrentRepositionActivity.ChoosingRepositionTarget)
				{
					if(KeyBindingList.eitherBackoutKeyIsPressed())
					{
						resetCurrentActivity();
						resetRepositioningTypeAndActivity();
						
						KeyPressManager.handlingPrimaryKeyPress = true;
					}
				} else if(currentRepositionActivity == CurrentRepositionActivity.ChoosingNewLocation)
				{
					if(KeyBindingList.eitherBackoutKeyIsPressed()) 
					{
						deselectSingleAllyToMove();
						
						SelectorManager.resetCurrentSelector();
						
						KeyPressManager.handlingPrimaryKeyPress = true;
					}
				}
				
			} else if(currentRepositionType == CurrentRepositionType.AllyFormation)
			{
				if(currentRepositionActivity == CurrentRepositionActivity.ChoosingNewLocation && 
					combatantsRemainingToReposition() > 0 &&
					Input.GetKey(KeyBindingList.combatAcceptChoiceKey))
				{
					placeNextRepositionDetailsInList();
					KeyPressManager.handlingPrimaryKeyPress = true;
				} else if(KeyBindingList.eitherBackoutKeyIsPressed())
				{
					if(repositionDetailsList.Count == 0)
					{
						exitRepositioning();
					} else
					{
						requeueLatestRepositionDetails();
						repositionUIManager.setConfirmRepositionButtonInteractability(deactivate);
					}
					
					KeyPressManager.handlingPrimaryKeyPress = true;
				}
			}
		}
	}
	
	public void startSingleAllyReposition()
	{
		CombatStateManager.setCurrentActivity(CurrentActivity.Repositioning);
		currentRepositionType = CurrentRepositionType.SingleAlly;
		currentRepositionActivity = CurrentRepositionActivity.ChoosingRepositionTarget;
		
		repositionUIManager.updateOnStateChange();
		
		currentSingleTargetRepositionCombatAction = AbilityList.getAbility(AbilityList.moveAllyAbilityKey);
	}
	
	public void startAllyFormationReposition()
	{
		CombatStateManager.setCurrentActivity(CurrentActivity.Repositioning);
		currentRepositionType = CurrentRepositionType.AllyFormation;
		currentRepositionActivity = CurrentRepositionActivity.ChoosingNewLocation;
		
		repositionUIManager.activateRepositionGrid(activate);
		currentSetOfCombatantsToReposition = CombatGrid.getAllAliveAllyCombatants();
		
		activateAllSprites(currentSetOfCombatantsToReposition, deactivate);
		
		repositionUIManager.addAllCombatantSelectionButtons(currentSetOfCombatantsToReposition);
		
		repositionUIManager.selectNextCombatantSelectionButton();
	}
	
	public void startEnemyFormationReposition()
	{
		CombatStateManager.setCurrentActivity(CurrentActivity.Repositioning);
		currentRepositionType = CurrentRepositionType.EnemyFormation;
		currentRepositionActivity = CurrentRepositionActivity.ChoosingNewLocation;
		
	}
	
	public void updateOnStateChange()
	{
		if(CombatStateManager.currentActivity != CurrentActivity.Repositioning)
		{
			currentRepositionType = CurrentRepositionType.NotRepositioning;
			currentRepositionActivity = CurrentRepositionActivity.NotRepositioning;
		}
	}
	
	private void placeNextRepositionDetailsInList()
	{
		GridCoords destinationCoords = SelectorManager.currentSelector.getCoords();
		
		if(noRepositionDetailsSharePosition(destinationCoords))
		{
			Stats nextCombatant = getNextCombatant();
			repositionUIManager.removeSelectedCombatantSelectionButton();
			
			repositionDetailsList.Add(new RepositionDetails(nextCombatant, destinationCoords));
			
			if(combatantsRemainingToReposition() > 0)
			{
				repositionUIManager.selectNextCombatantSelectionButton();
			} else
			{
				repositionUIManager.setConfirmRepositionButtonInteractability(activate);
			}
		}
	}
	
	private bool noRepositionDetailsSharePosition(GridCoords newDetails)
	{
		foreach(RepositionDetails details in repositionDetailsList)
		{
			if(newDetails.Equals(details.repositionDestination))
			{
				return false;
			}
		}
		
		return true;
	}
	
	public int getRepositionsRemaining()
	{
		return repositionsRemaining;
	}
	
	public void decrementRepositionsRemaining()
	{
		if(repositionsRemaining > 0)
		{
			repositionsRemaining--;
		}
		
		repositionUIManager.updateRepositionCounter();
		repositionUIManager.updateOnStateChange();
	}
	
	public void resetRepositionsRemaining()
	{
		repositionsRemaining = PartyManager.getPlayerStats().getMaximumRepositionsPerCombat();
	}
	
	private void requeueLatestRepositionDetails()
	{
		RepositionDetails repositionDetails = (RepositionDetails) repositionDetailsList[repositionDetailsList.Count-1];
		repositionDetailsList.RemoveAt(repositionDetailsList.Count-1);
		
		
		repositionDetails.destroyPlaceHolder();
		repositionUIManager.requeueCombatantSelectionButton(repositionDetails.combatantToReposition);
	}
	
	private Stats getNextCombatant()
	{
		GameObject nextCombatantButton = (GameObject) repositionUIManager.combatantSelectionButtonQueue[0];
		
		return nextCombatantButton.GetComponent<StatsRepositionObject>().combatantToReposition;
	}
	
	public int combatantsRemainingToReposition()
	{
		return repositionUIManager.combatantSelectionButtonQueue.Count;
	}
	
	private void activateAllSprites(ArrayList combatants, bool activateSprites)
	{
		foreach(Stats combatant in combatants)
		{
			combatant.combatSprite.SetActive(activateSprites);
			combatant.healthBar.SetActive(activateSprites);
		}
	}
	
	private void resetCurrentActivity()
	{
		CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);
	}
	
	private void resetRepositioningTypeAndActivity()
	{
		currentRepositionActivity = CurrentRepositionActivity.NotRepositioning;
		currentRepositionType = CurrentRepositionType.NotRepositioning;
	}
	
	private void exitRepositioning()
	{
		repositionUIManager.activateRepositionGrid(deactivate);
		resetCurrentActivity();
		resetRepositioningTypeAndActivity();
		
		activateAllSprites(currentSetOfCombatantsToReposition, activate);
		repositionUIManager.removeAllCombatantSelectionButtons();
		currentSetOfCombatantsToReposition = new ArrayList();
		
		repositionUIManager.hideConfirmRepositionButton();
		CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);
	}
	
	public static void selectSingleAllyToMove(GridCoords coordsOfAlly)
	{
		RepositionManager.currentSingleTargetRepositionCombatAction.setActorCoords(coordsOfAlly);
		RepositionManager.currentRepositionActivity = CurrentRepositionActivity.ChoosingNewLocation;
	}
	
	private static void deselectSingleAllyToMove()
	{
		RepositionManager.currentSingleTargetRepositionCombatAction.setActorCoords(GridCoords.getDefaultCoords());
		RepositionManager.currentRepositionActivity = CurrentRepositionActivity.ChoosingRepositionTarget;
	}
	
	public void performFormationReposition()
	{		
		foreach(RepositionDetails repositionDetails in repositionDetailsList)
		{
			repositionDetails.combatantToReposition.moveTo(repositionDetails.repositionDestination);
			GameObject.Destroy(repositionDetails.placeHolderObject);
		}
		
		repositionDetailsList = new ArrayList();
		currentSetOfCombatantsToReposition = new ArrayList();
		
		ZoneOfInfluenceManager.getInstance().applyAllZOITraits();
		
		CombatActionManager.getInstance().decideAndShowEnemyCombatActions(dontPopulateCombatActionPanels);
		CombatActionManager.getInstance().decideAndShowSummonedCombatActions();
	
		decrementRepositionsRemaining();
	
		CombatStateManager.setCurrentActivity(CurrentActivity.ChoosingActor);
	}
	
	public static RepositionManager getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("there is another instance of RepositionManager open");
		}
		
		instance = this;
	}
	
}
