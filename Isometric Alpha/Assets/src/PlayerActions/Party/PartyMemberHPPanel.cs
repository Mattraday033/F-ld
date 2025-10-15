using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyMemberHPPanel : MonoBehaviour
{
	public Image backgroundImage;
	public TextMeshProUGUI hpText;
	private string partyMemberName = "";
	
	public void setPartyMemberName(string partyMemberName)
	{
		if(PartyManager.getPartyMember(partyMemberName).canJoinParty)
		{
			this.partyMemberName = partyMemberName;
			updateHPText();
		} else
		{
            partyMemberName = "";
			setToBlank();
		}
	}
	
	public void updateHPText()
	{
		hpText.text = PartyManager.getPartyMember(partyMemberName).stats.currentHealth + "/" + PartyManager.getPartyMember(partyMemberName).stats.getTotalHealth();
		backgroundImage.color = Color.white;
	}
	
	public void setToBlank()
	{
		hpText.text = "";
		backgroundImage.color = Color.grey;
	}
	
	public string getPartyMemberName()
	{
		return partyMemberName;
	}	
}
