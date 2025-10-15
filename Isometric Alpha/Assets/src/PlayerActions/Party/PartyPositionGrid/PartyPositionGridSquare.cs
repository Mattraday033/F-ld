using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyPositionGridSquare : MonoBehaviour
{

	public IPartyEditor partyEditor;
	public TextMeshProUGUI characterNameText;
	
	public AllyStats characterInSquare;
	
	public string defaultChar;

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
		if (character == null)
		{
			characterInSquare = null;
			disableNameText();
		}
		else
		{
			characterInSquare = character;

			enableNameText(characterInSquare.getName());
		}

	}

	public void disableNameText()
	{
		if (defaultChar == null || defaultChar.Length <= 0 || characterInSquare != null)
		{
			characterNameText.enabled = false;
		}
		else if(characterInSquare == null)
		{
			enableNameText(defaultChar);
		}
	}

	public void enableNameText(string newNameText)
	{
		characterNameText.text = newNameText;
		characterNameText.enabled = true;
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
