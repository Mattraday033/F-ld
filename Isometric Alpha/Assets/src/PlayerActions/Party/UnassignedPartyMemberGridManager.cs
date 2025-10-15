using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnassignedPartyMemberGridManager : MonoBehaviour
{

	public ArrayList listOfUnassignedPartyMemberRows = new ArrayList();

	public static Stats selectedPartyMember;

	public GameObject unassignedPartyMemberRowPrefab;
	
	public GameObject unassignedPartyMemberGrid; 	
	public GameObject scrollContainer; 
	public GameObject scrollableArea; 	

	public static UnassignedPartyMemberGridManager instance;
	
	private void Awake(){
		if(instance != null){
			Debug.LogError("Found more than one Selector Manager in the scene.");
		}
		
		instance = this;
	}
	
	public static UnassignedPartyMemberGridManager getInstance()
	{
		return instance;
	}
	

	public void populateUnnassignedPartyMemberPanels(){
		
		deleteAllUnassignedPartMemberPanels();
		
		ArrayList allUnassignedPartyMembers = getAllUnassignedPartyMembers(AdjustPartyRosterManager.interimFormation.getGrid());
		
		int partyMemberIndex = 0;
		foreach(Stats stats in allUnassignedPartyMembers)
		{
			Vector3 prefabLocalPosition = unassignedPartyMemberRowPrefab.GetComponent<RectTransform>().localPosition; //RectTransform of disabled inventoryRow example

			GameObject current = Instantiate(unassignedPartyMemberRowPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
			current.name = "Unassigned_Party_Member_Panel_" + partyMemberIndex;
		
			current.transform.parent = scrollableArea.transform;
			RectTransform currentRectTransform = current.GetComponent<RectTransform>();
		
			current.GetComponent<RectTransform>().localPosition = new Vector3(prefabLocalPosition.x, prefabLocalPosition.y - (currentRectTransform.rect.height*(partyMemberIndex)), 0f);
			current.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
			current.SetActive(true);

			UnassignedPartyMemberRow currentRow = current.GetComponent<UnassignedPartyMemberRow>();
			
			currentRow.populate(stats);
			
			if(AdjustPartyRosterManager.getInstance().getInterimUsedSlots() >= PartyStats.getPartySizeMaximum())
			{
				currentRow.setInteractability(false);
			}
			
			listOfUnassignedPartyMemberRows.Add(current);
			partyMemberIndex++;
		}
	} 

	public ArrayList getAllUnassignedPartyMembers(Stats[][] positionGrid)
	{
		ArrayList allUnassignedPartyMembers = new ArrayList();
		
		if(!PartyManager.getPlayerStats().isInParty(positionGrid))
		{
			allUnassignedPartyMembers.Add(PartyManager.getPlayerStats());
		}

		List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();
		foreach (PartyMember partyMember in allPartyMembers)
		{
            if (!partyMember.isInParty(positionGrid) && partyMember.canJoinParty)
			{				
				allUnassignedPartyMembers.Add(partyMember.stats);
			}			
		}
		
		return allUnassignedPartyMembers;
	}

	public void deleteAllUnassignedPartMemberPanels()
	{
		if(listOfUnassignedPartyMemberRows is null)
		{
			return;
		}
		
		foreach(GameObject row in listOfUnassignedPartyMemberRows)
		{
			GameObject.Destroy(row);
		}
		
		listOfUnassignedPartyMemberRows = new ArrayList();
	}
	
	public void selectPartyMember(Stats partyMember)
	{
		deselectPartyMember();
		
		selectedPartyMember = partyMember;
	}
	
	public void deselectPartyMember()
	{
		selectedPartyMember = null;
		
		foreach(GameObject row in listOfUnassignedPartyMemberRows)
		{
			UnassignedPartyMemberRow unassignedPartyMemberRow = row.GetComponent<UnassignedPartyMemberRow>();
			
			unassignedPartyMemberRow.deselectPartyMember();
		}
	}
	
	public void setAllPanelsInteractability(bool newInteractability)
	{
		foreach(GameObject row in listOfUnassignedPartyMemberRows)
		{
			row.GetComponent<UnassignedPartyMemberRow>().setInteractability(newInteractability);
		}
	}
}
