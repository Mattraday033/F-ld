using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class GlossaryCategoryList
{

	public static GlossaryCategory ranges = new GlossaryCategory("Ranges", Range.getAllRangesGlossaryEntries());
	public static GlossaryCategory actionTypes = new GlossaryCategory("Action Types", CombatAction.getAllActionTypeGlossaryEntries());
	public static GlossaryCategory traitTypes = new GlossaryCategory("Trait Types", Trait.getAllTraitTypeGlossaryEntries());

	public static ArrayList allGlossaryCategories;

	static GlossaryCategoryList()
	{
		allGlossaryCategories = new ArrayList();

		allGlossaryCategories.Add(actionTypes);
		allGlossaryCategories.Add(ranges);
		allGlossaryCategories.Add(traitTypes);
	}

	public static ArrayList getAllGlossaryCategories()
	{
		return allGlossaryCategories;
	}

}
