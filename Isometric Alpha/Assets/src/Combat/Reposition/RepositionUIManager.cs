using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RepositionUIManager : MonoBehaviour, INeedsUpdateOnStateChange
{
	private const bool activate = true;
	private const bool deactivate = false;
	
	private static RepositionUIManager instance;
	
	public GameObject partyMemberSelectionButtonExample;
	public GameObject repositionGrid;
	public Transform repositionGridScrollableArea;
	
	public RepositionManager repositionManager;
	
	public TextMeshProUGUI repositionCounterText;
	
	public Button moveUnitButton;
	
	public Button repositionButton;
	public Button confirmRepositionButton;
	public Image repositionCounterImage;
	
	public ArrayList combatantSelectionButtonQueue = new ArrayList();
	
	void Start()
	{
		updateRepositionCounter();
		
		if(playerHasNotLearnedToReposition())
		{
			greyOutRepositionButtonAndPanel();
		}
	}
	
	public void updateOnStateChange()
	{	
		if(CombatStateManager.currentActivity != CurrentActivity.ChoosingActor || 
		   PlayerCombatActionManager.playerCombatActionQueue.Count > 0 || 
		   repositionManager.getRepositionsRemaining() <= 0)
		{
			repositionButton.interactable = deactivate;
		} else
		{
			repositionButton.interactable = activate;
		}
		
		if(CombatStateManager.currentActivity != CurrentActivity.ChoosingActor)
		{
			moveUnitButton.interactable = deactivate;
		} else
		{
			moveUnitButton.interactable = activate;
		}
	}
	
	public void activateRepositionGrid(bool activateGrid)
	{
		repositionGrid.SetActive(activateGrid);
	}
	
	public void addAllCombatantSelectionButtons(ArrayList combatantsToMove)
	{
		foreach(Stats combatant in combatantsToMove)
		{
			addNewCombatantSelectionButton(combatant);
		}
	}
	
	public void removeAllCombatantSelectionButtons()
	{
		foreach(GameObject statsRepositionGameObject in combatantSelectionButtonQueue)
		{
			GameObject.Destroy(statsRepositionGameObject);
		}
		
		combatantSelectionButtonQueue = new ArrayList();
	}
	
	public void requeueCombatantSelectionButton(Stats stats)
	{		
		GameObject newSelectionButton = createNewCombatantSelectionButton(stats);
		
		newSelectionButton.transform.SetSiblingIndex(0);
		combatantSelectionButtonQueue.Insert(0, newSelectionButton);
		
		deselectAllCombatantSelectionButtons();
		selectNextCombatantSelectionButton();
	}
	
	private void addNewCombatantSelectionButton(Stats stats)
	{
		combatantSelectionButtonQueue.Add(createNewCombatantSelectionButton(stats));
	}
	
	private GameObject createNewCombatantSelectionButton(Stats stats)
	{
		GameObject statsRepositionGameObject = Instantiate(partyMemberSelectionButtonExample, repositionGridScrollableArea);
		statsRepositionGameObject.SetActive(true);
		StatsRepositionObject statsRepositionObject = statsRepositionGameObject.GetComponent<StatsRepositionObject>();
		
		statsRepositionObject.setCombatantToReposition(stats);
		statsRepositionObject.setSprite();
		
		return statsRepositionGameObject;
	}
	
	public void updateRepositionCounter()
	{
		if(playerHasNotLearnedToReposition())
		{
			repositionCounterText.text = "";
		} else
		{
			repositionCounterText.text = repositionManager.getRepositionsRemaining() + "/" +  
										 PartyManager.getPlayerStats().getMaximumRepositionsPerCombat();
		}
	}

	public void greyOutRepositionButtonAndPanel()
	{
		repositionButton.interactable = false;
		repositionCounterImage.color = Color.grey;
	}


	public void selectNextCombatantSelectionButton()
	{
		GameObject buttonToSelect = (GameObject) combatantSelectionButtonQueue[0];
		
		buttonToSelect.GetComponent<Outline>().enabled = activate;
	}
	
	private void deselectAllCombatantSelectionButtons()
	{
		foreach(GameObject button in combatantSelectionButtonQueue)
		{
			button.GetComponent<Outline>().enabled = deactivate;
		}
	}
	
	public void removeSelectedCombatantSelectionButton()
	{
		GameObject buttonToRemove = (GameObject) combatantSelectionButtonQueue[0];
		combatantSelectionButtonQueue.RemoveAt(0);
		
		GameObject.Destroy(buttonToRemove);
	}

	private bool playerHasNotLearnedToReposition()
	{
		return PartyManager.getPlayerStats().getMaximumRepositionsPerCombat() <= 0;
	}
	
	public void revealConfirmRepositionButton()
	{
		confirmRepositionButton.gameObject.SetActive(activate);
		repositionButton.gameObject.SetActive(deactivate);
	}
	
	public void hideConfirmRepositionButton()
	{
		confirmRepositionButton.gameObject.SetActive(deactivate);
		repositionButton.gameObject.SetActive(activate);
	}
	
	public void setConfirmRepositionButtonInteractability(bool newInteractability)
	{
		confirmRepositionButton.interactable = newInteractability;
	}
	
	public static RepositionUIManager getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("there is another instance of RepositionUIManager open");
		}
		
		instance = this;
	}
}
