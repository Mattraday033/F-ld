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

	//IDescribableInBlocks methods
	public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, getName()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, journalDescription));

		return buildingBlocks;
	}
}
