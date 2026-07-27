using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueLine : IDescribable
{
    public readonly static UnityEvent UnboldAllText = new UnityEvent();

    private DialogueDescriptionPanel dialoguePanel;

	public string speakerName;
	public string contents;

	public DialogueLine(string speakerName, string contents)
	{
		string[] speakerNameWords = speakerName.Split(" ");

		bool speakerNameContainsNumbers = int.TryParse(speakerNameWords[speakerNameWords.Length - 1], out int number);

		if (speakerNameContainsNumbers)
		{
			this.speakerName = speakerName.Split(" ")[0];
		}
		else
		{
			this.speakerName = speakerName;
		}

		this.contents = contents;
	}

	public string getName()
	{
		return speakerName;
	}

    public bool isBoldable()
    {
        return !Conversation.nameIsUpdate(speakerName);
    }

    public void boldText()
    {
        speakerName = Constants.boldTextStart + speakerName + Constants.boldTextEnd;
        contents = Constants.boldTextStart + contents + Constants.boldTextEnd;

        UnboldAllText.AddListener(unboldText);
    }

    public void unboldText()
    {
        UnboldAllText.RemoveListener(unboldText);
    
        speakerName = speakerName.Replace(Constants.boldTextStart, "").Replace(Constants.boldTextEnd, "");
        contents = contents.Replace(Constants.boldTextStart, "").Replace(Constants.boldTextEnd, "");

        if(dialoguePanel != null)
        {
            describeSelfRow(dialoguePanel);
        }
    }

	public bool ineligible()
	{
		return false;
	}

	public GameObject getRowType(RowType rowType)
	{
		return Resources.Load<GameObject>(PrefabNames.dialogueLineRow);
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
		return;
	}

	public void describeSelfRow(DescriptionPanel panel)
	{
		dialoguePanel = (DialogueDescriptionPanel)panel;

		dialoguePanel.setObjectBeingDescribed(this);
		DescriptionPanel.setText(dialoguePanel.loreDescriptionText, getName() + " :  " + contents);

		dialoguePanel.updateSize();
	}

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
	{
		return;
	}

	public List<IDescribable> getRelatedDescribables()
	{
		return new List<IDescribable>();
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
/*
 * Brittle is any peace built by human hands. Or so the sages say, having divined the cyclical ebbings of history from their scrolls and stories.
 * Over a decade has passed since the Lovashi were put to route, fleeing back to the cities they stole from our cousin kingdoms.
 */