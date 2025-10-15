using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyMemberSelectionScreen : PopUpWindow
{	
	public UsableItem itemBeingDescribed;

	public PartyMemberButton[] partyMemberButtons;
	public PartyMemberHPPanel[] partyMemberHPPanels;

	public string currentPartyMemberName = "";

	public DescriptionPanel currentDescriptionPanel;

	private static PartyMemberSelectionScreen instance;

	public static PartyMemberSelectionScreen getInstance()
	{
		return instance;
	}

    private void Awake()
    {
        if(instance != null)
		{
			throw new IOException("There are duplicate instances of PartyMemberSelectionScreen");
		}

		instance = this;
    }

    public void useItemOnPartyMember()
	{		
		itemBeingDescribed.use(PartyManager.getPartyMember(currentPartyMemberName).stats);
		
		if(!itemBeingDescribed.infiniteUses())
		{
			Inventory.removeItem(itemBeingDescribed, 1);
			OverallUIManager.currentScreenManager.populateAllGrids();
		}
		
		populate();
	}

	public void populate()
	{	
		
		if(Inventory.pocketContainsItem(itemBeingDescribed.getKey(), State.junkPocket) || 
			Inventory.pocketContainsItem(itemBeingDescribed.getKey(), State.inventory))
		{
			currentDescriptionPanel.setObjectBeingDescribed(Inventory.getItem(itemBeingDescribed.getKey()));
			itemBeingDescribed = (UsableItem) currentDescriptionPanel.getObjectBeingDescribed();

            currentDescriptionPanel.nameText.text = "Use " + itemBeingDescribed.getKey() + " on a Party Member?";
			currentDescriptionPanel.hpText.text = itemBeingDescribed.getAmountToHeal() + " HP";
			currentDescriptionPanel.amountText.text = "x" + itemBeingDescribed.getQuantity();
		} else
		{
			currentDescriptionPanel.amountText.text = "x0";
		}
		
		populatePartyMemberButtons();
		populatePartyMemberHPPanels();
	}

	public void populatePartyMemberButtons()
	{
		int itemQuantity = int.Parse(currentDescriptionPanel.amountText.text.Replace("x",""));
		
		int buttonIndex = 0;
		List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();
	
        foreach (PartyMember partyMember in allPartyMembers)
		{
            partyMemberButtons[buttonIndex].setPartyMemberName(partyMember.getName());
			partyMemberButtons[buttonIndex].partyMemberSelectionScreen = this;
			
			if(itemQuantity <= 0 || 
				partyMember.stats.currentHealth >= partyMember.stats.getTotalHealth())
			{
				partyMemberButtons[buttonIndex].setInteractibility(false);
			}
			
			buttonIndex++;
		}
	}
	
	public void populatePartyMemberHPPanels()
	{
		int hpPanelIndex = 0;
		List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();
	
        foreach (PartyMember partyMember in allPartyMembers)
		{
            if (hpPanelIndex < allPartyMembers.Count)
			{
				partyMemberHPPanels[hpPanelIndex].setPartyMemberName(partyMember.getName());
			} else
			{
				partyMemberHPPanels[hpPanelIndex].setToBlank();
			}

			hpPanelIndex++;
        }

		for(int index = hpPanelIndex; index < partyMemberHPPanels.Length; index++)
		{
            partyMemberHPPanels[index].setToBlank();
        }
	}
	
	public void setAllPopulatedPartyMemberButtonsInteractability(bool newInteractability)
	{
		foreach(PartyMemberButton partyMemberButton in partyMemberButtons)
		{
			if(partyMemberButton.getPartyMemberName() != null && partyMemberButton.getPartyMemberName().Length > 0)
			{
				partyMemberButton.button.interactable = newInteractability;
			} 
		}
	}
}
