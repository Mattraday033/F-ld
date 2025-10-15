using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceDescription : IDescribable
{
	private const int nameContentsSubstringMaxLength = 15;
	private const string numberContentsSeparator = ":";
	public const string unimplementedTag = "<Not Implemented>";

	public int number;
	public string contents;
	public ChoiceKey choiceKey;

	public ChoiceDescription(int number, string contents, ChoiceKey choiceKey)
	{
		this.number = number;
		this.contents = contents;
		this.choiceKey = choiceKey;
	}

	public string getName()
	{
		if (nameContentsSubstringMaxLength < contents.Length)
		{
			return number + numberContentsSeparator + contents.Substring(0, nameContentsSubstringMaxLength) + "...";
		}
		else
		{
			return number + numberContentsSeparator + contents;
		}
	}

	public bool ineligible()
	{
		return ChoiceManager.hasBeenChosenBefore(choiceKey);
	}

	public GameObject getRowType(RowType rowType)
	{
		if (contents.Contains(unimplementedTag))
		{
			return Resources.Load<GameObject>(PrefabNames.unimplementedChoice);
		}
		else
		{
			return Resources.Load<GameObject>(PrefabNames.choiceRow);
		}
	}

	public GameObject getDescriptionPanelFull()
	{
		return null;
	}

	public GameObject getDescriptionPanelFull(PanelType type)
	{
		return null;
	}

	public GameObject getDecisionPanel()
	{
		return null;
	}

	public bool withinFilter(string[] filterParameters)
	{
		return true;
	}

	public void describeSelfFull(DescriptionPanel panel)
	{

	}

	public void describeSelfRow(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, number + numberContentsSeparator);
		DescriptionPanel.setText(panel.loreDescriptionText, contents);

		panel.gameObject.GetComponent<DialogueChoice>().setChoiceNumber(number - 1);
	}

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
	{

	}

	public ArrayList getRelatedDescribables()
	{
		return new ArrayList();
	}

	public bool buildableWithBlocks()
	{
		return false;
	}
	
	public bool buildableWithBlocksRows()
    {
        return false;
    }
}
