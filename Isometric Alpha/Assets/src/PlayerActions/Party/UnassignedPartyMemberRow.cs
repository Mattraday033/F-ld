using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UnassignedPartyMemberRow : MonoBehaviour
{
	private Stats partyMemberBeingDescribed;
	
	public UnassignedPartyMemberGridManager unassignedPartyMemberGridManager;
	
	public Button portraitButton;
	public Image portraitImage;
	
	public Button partyMemberNameButton; 
	public TextMeshProUGUI partyMemberNameText;
	
	//instead of passing a pointer to the relavent PartyMember object
	//when implementing portraits, pass the pointer to the portrait instead
	public void populate(Stats newPartyMember)
	{		
		partyMemberBeingDescribed = newPartyMember;

		//instantiate portraitImage
		
		if(partyMemberBeingDescribed == PartyManager.getPlayerStats())
		{
			partyMemberNameText.text = "Myself";
		} else
		{
			partyMemberNameText.text = partyMemberBeingDescribed.getName();
		}
		
		
	}

	public void selectPartyMember()
	{
		unassignedPartyMemberGridManager.selectPartyMember(partyMemberBeingDescribed);
		
		setInteractability(false);
	}
	
	public void deselectPartyMember()
	{
		setInteractability(true);
	}

	public void setInteractability(bool newInteractability)
	{
		portraitButton.interactable = newInteractability;
		partyMemberNameButton.interactable = newInteractability;
	}

}
