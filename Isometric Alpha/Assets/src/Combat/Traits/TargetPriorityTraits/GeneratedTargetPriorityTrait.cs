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
	base(targetParameters.getName(), targetParameters.getDescription(), targetParameters.getIconName())
	{
		this.targetParameters = targetParameters;
		this.amountOfTargets = amountOfTargets;
	}
		
	public override bool deterministic()
	{
		return targetParameters.deterministic();
	}
	
	public override Selector findTargetLocation(Selector selector, List<Stats> listOfTargets)
	{
		return findTargetLocation(listOfTargets);
	}
	
	private Selector findTargetLocation(List<Stats> listOfTargets)
	{
		List<Selector> allTargetSelectors = new List<Selector>();
		List<GridCoords> placeHolderGridCoords = new List<GridCoords>();
		int spacesLeft = 0;
		
        spacesLeft = amountOfTargets;
		
		for(int currentSelector = 0; currentSelector < spacesLeft; currentSelector++)
		{	
			Selector selector = targetParameters.findTargetLocation(SelectorManager.getInstance().selectors[Range.singleTargetIndex], listOfTargets);
			
			if(selector == null)
			{
				continue;
			}

			if(targetParameters.targetsOnlyEmptySpace())
			{
				GridCoords currentCoords = selector.getCoords();
				placeHolderGridCoords.Add(currentCoords);

                if (CombatGrid.getCombatantAtCoords(currentCoords) != null &&
                    !(CombatGrid.getCombatantAtCoords(currentCoords) is null))
                {
                    Debug.LogError("Setting placeholder would overwrite an existing combatant");
                }

                List<GridCoords> emptyCoords = CombatGrid.getAllEmptySpacesInEnemyZone().ToList();

                if(emptyCoords.Count > 0)
                {
                   selector.setToLocation(emptyCoords.OrderBy(a => Guid.NewGuid()).ToList()[0]);
                   CombatStateManager.allQueuedSummonLocations.Add(selector.getCoords());
                }

                // // Debug.LogError("Setting Combatant At Coords : "+currentCoords.ToString()+" disallowed because EnemyStats constructors have changed");
				// CombatGrid.setCombatantAtCoords(currentCoords, new EnemyStats());
			}

            allTargetSelectors.Add(selector);
		}
		
		foreach(GridCoords coords in placeHolderGridCoords)
		{
			CombatGrid.setCombatantAtCoords(coords, null);
		}
		
		return SelectorGenerator.generate(allTargetSelectors.ToArray());
	}
}
