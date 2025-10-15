using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyMemberButton : MonoBehaviour
{
	public Button button;
	public TextMeshProUGUI nameText;
	
	private string partyMemberName = "";
	
	public PartyMemberSelectionScreen partyMemberSelectionScreen;
	
	public void setPartyMemberName(string partyMemberName)
	{
		PartyMember partyMember = PartyManager.getPartyMember(partyMemberName);
		
		if(partyMember.canJoinParty)
		{
			this.partyMemberName = partyMemberName;
			nameText.text = PartyManager.getPartyMember(partyMemberName).stats.getName();
			button.interactable = true;
		} else
		{
			this.partyMemberName = "";
			button.interactable = false;
			nameText.text = "";
		}
	}
	
	public void setCurrentButtonName()
	{
		partyMemberSelectionScreen.currentPartyMemberName = partyMemberName;
	}
	
	public void useItemOnPartyMember()
	{
		setCurrentButtonName();

        partyMemberSelectionScreen.useItemOnPartyMember();
		partyMemberSelectionScreen.populate();
    }

	public void setInteractibility(bool newInteractability)
	{
		button.interactable = newInteractability;
	}
	
	public string getPartyMemberName()
	{
		return partyMemberName;
	}

}
