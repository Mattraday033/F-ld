using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustPartyRosterManager : MonoBehaviour
{
	
	public FormationDisplayUI formationDisplayUI;
	public static Formation interimFormation = new Formation();
	public static bool isInPartySelection = false;
	
	public PartyPositionAcceptButton partyPositionAcceptButton;
	
	public PartySlotTracker partySlotTracker;
	
	public GameObject addPartyMemberScreen;
	
	public UnassignedPartyMemberGridManager unassignedPartyMemberGridManager;
	
	public static AdjustPartyRosterManager instance;
	
	private void Awake(){
		if(instance != null){
			Debug.LogError("Found more than one Selector Manager in the scene.");
		}
		
		instance = this;
	}
	
	public static AdjustPartyRosterManager getInstance()
	{
		return instance;
	}
	
	public void readInCurrentFormation()
	{
		interimFormation.setGrid(Formation.getEmptyGrid());

		for(int rowIndex = 0; rowIndex < State.formation.getGrid().Length; rowIndex++)
		{
			for(int colIndex = 0; colIndex < State.formation.getGrid()[rowIndex].Length; colIndex++)
			{
				interimFormation.setCharacterAtCoords(rowIndex, colIndex, State.formation.getGrid()[rowIndex][colIndex]);
			}
		}
	}
	
	public void removeCharacterFromFormation(Stats characterToRemove)
	{
		
		for(int rowIndex = 0; rowIndex < interimFormation.getGrid().Length; rowIndex++)
		{
			for(int colIndex = 0; colIndex < interimFormation.getGrid()[rowIndex].Length; colIndex++)
			{
				if(interimFormation.getGrid()[rowIndex][colIndex] == characterToRemove)
				{
					interimFormation.getGrid()[rowIndex][colIndex] = null;
					return;
				}
			}
		}
		
		unassignedPartyMemberGridManager.populateUnnassignedPartyMemberPanels();
	}
	
	public void addCharacterToFormation(AllyStats characterToAdd, int row, int col)
	{
		interimFormation.getGrid()[row][col] = characterToAdd;
		
		unassignedPartyMemberGridManager.populateUnnassignedPartyMemberPanels();
	}
	
	public int getInterimUsedSlots()
	{
		int usedSlots = 0;
		
		foreach(Stats[] row in interimFormation.getGrid())
		{
			foreach(Stats position in row)
			{
				if(!(position is null))
				{
					usedSlots++;
				}
			}
		}
		
		return usedSlots;
	}

	public void setNewFormation()
	{
		State.formation.setGrid(interimFormation.getGrid());
		
		interimFormation = new Formation();
	}
	
	public void activate()
	{
		addPartyMemberScreen.SetActive(true);
		
		readInCurrentFormation();
		
		formationDisplayUI.populate(interimFormation);
		
		unassignedPartyMemberGridManager.populateUnnassignedPartyMemberPanels();
		
		isInPartySelection = true;
	}
	
	public void deactivate(bool overwritePartyPosition)
	{
		addPartyMemberScreen.SetActive(false);
		
		if(overwritePartyPosition)
		{
			setNewFormation();
		}
		
		isInPartySelection = false;
	}
}
