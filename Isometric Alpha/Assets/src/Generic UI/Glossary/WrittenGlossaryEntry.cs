using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrittenGlossaryEntry : GlossaryEntry, IDescribableInBlocks 
{
	private string journalDescription;
	
	public WrittenGlossaryEntry(string title, string category, string journalDescription):
	base(title, category)
	{
		this.journalDescription = journalDescription;
	}
	
	public override bool buildableWithBlocks()
    {
        return true;
    }

    public string getDescription()
    {
        string modifiedDescription = "";

        for(int i = 0; i < journalDescription.Length; i++)
        {
            if(i < journalDescription.Length-1 && journalDescription.Substring(i, 2).Equals(". "))
            {
                modifiedDescription += ".\n\n";
                i++;
            } else
            {
                modifiedDescription += journalDescription[i];
            }
        }

        return modifiedDescription;
    }

	//IDescribableInBlocks methods
	public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(getName()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, getDescription()));

		return buildingBlocks;
	}

    public bool requiresInspectNode()
    {
        return false;
    }
}

public class StatGlossaryEntry: WrittenGlossaryEntry
{
    private string iconName;

	public StatGlossaryEntry(string title, string category, string journalDescription):
    base(title, category, journalDescription)
	{
		this.iconName = title;
	}

	public StatGlossaryEntry(string title, string category, string journalDescription, string iconName):
    base(title, category, journalDescription)
	{
		this.iconName = iconName;
	}

	public override void describeSelfRow(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, getName());
        DescriptionPanel.setImage(panel.iconPanel, Helpers.loadSpriteFromResources(iconName));
        
        if(panel.iconBackgroundPanel != null)
        {
            panel.iconBackgroundPanel.gameObject.SetActive(true);
        }
	}
}
