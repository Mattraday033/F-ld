using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class GlossaryCategoryList
{

	public readonly static GlossaryCategory ranges = new GlossaryCategory("Ranges", Range.getAllRangesGlossaryEntries());
	public readonly static GlossaryCategory actionTypes = new GlossaryCategory("Action Types", CombatAction.getAllActionTypeGlossaryEntries());
	public readonly static GlossaryCategory traitTypes = new GlossaryCategory("Trait Types", Trait.getAllTraitTypeGlossaryEntries());

	public static List<GlossaryCategory> allGlossaryCategories;

    [RuntimeInitializeOnLoadMethod]
	private static void instantiateGlossaryCategoryList()
	{
		allGlossaryCategories = new List<GlossaryCategory>();

		allGlossaryCategories.Add(actionTypes);
		allGlossaryCategories.Add(ranges);
		allGlossaryCategories.Add(traitTypes);
	}

	public static List<GlossaryCategory> getAllGlossaryCategories()
	{
		return allGlossaryCategories;
	}

}
