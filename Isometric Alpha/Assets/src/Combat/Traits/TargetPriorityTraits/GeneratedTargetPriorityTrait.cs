using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GeneratedTargetPriorityTrait : TargetPriorityTrait
{
	private TargetPriorityTrait targetParameters;
	private int amountOfTargets;
	
	public GeneratedTargetPriorityTrait(TargetPriorityTrait targetParameters, int amountOfTargets):
	base(targetParameters.getName(), targetParameters.getDescription(), targetParameters.getIconName(), targetParameters.getTraitIconBackgroundColor())
	{
		this.targetParameters = targetParameters;
		this.amountOfTargets = amountOfTargets;
	}
		
	public override bool deterministic()
	{
		return targetParameters.deterministic();
	}
	
	public override Selector findTargetLocation(Selector selector, ArrayList listOfTargets)
	{
		return findTargetLocation(listOfTargets);
	}
	
	private Selector findTargetLocation(ArrayList listOfTargets)
	{
		Selector[] allTargetSelectors = new Selector[0];
		ArrayList placeHolderGridCoords = new ArrayList();
		int spacesLeft = 0;
		
		if(targetParameters.targetsOnlyEmptySpace())
		{
			spacesLeft = CombatGrid.howManyEmptyEnemySpaces();
		} else
		{
			spacesLeft = amountOfTargets;
		}
		
		for(int currentSelector = 0; currentSelector < spacesLeft; currentSelector++)
		{	
			Selector selector = targetParameters.findTargetLocation(SelectorManager.getInstance().selectors[Range.singleTargetIndex], listOfTargets);
			
			if(selector == null)
			{
				continue;
			}

			allTargetSelectors = Helpers.appendArray(allTargetSelectors, selector);
			
			if(targetParameters.targetsOnlyEmptySpace())
			{
				GridCoords currentCoords = selector.getCoords();
				placeHolderGridCoords.Add(currentCoords);
				
				if(CombatGrid.getCombatantAtCoords(currentCoords) != null && 
					!(CombatGrid.getCombatantAtCoords(currentCoords) is null))
				{
					Debug.LogError("Setting placeholder would overwrite an existing combatant");
				}
				
				CombatGrid.setCombatantAtCoords(currentCoords, new EnemyStats());
			}
		}
		
		foreach(GridCoords coords in placeHolderGridCoords)
		{
			CombatGrid.setCombatantAtCoords(coords, null);
		}
		
		return SelectorGenerator.generate(allTargetSelectors);
	}
}
