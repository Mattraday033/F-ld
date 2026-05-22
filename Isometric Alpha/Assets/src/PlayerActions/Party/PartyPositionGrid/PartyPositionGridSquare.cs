using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyPositionGridSquare : MonoBehaviour
{

	public IPartyEditor partyEditor;
	
	public AllyStats characterInSquare;
	
	public const string defaultChar = "";

	public int row;
	public int col;

	public Button button;
	public Image image;

    public virtual void determineButtonEnabled()
    {
		button.enabled = true;
    }

	public GridCoords getCoords()
	{
		return new GridCoords(row, col);
	}

	public void populate()
	{
		populate(null);
	}

	public virtual void populate(AllyStats character)
	{

        characterInSquare = character;
	}

	public virtual void handleButtonPress()
	{
		AllyStats selectedPartyMember = partyEditor.getSelectedPartyMember(); 

		if (selectedPartyMember == null || selectedPartyMember is null)
		{
			partyEditor.removeCharacter(characterInSquare);
		}
		else
		{
			//Helpers.debugNullCheck("selectedPartyMember",selectedPartyMember);
			partyEditor.addCharacterToFormation(selectedPartyMember, row, col);
		}
	}	
}
