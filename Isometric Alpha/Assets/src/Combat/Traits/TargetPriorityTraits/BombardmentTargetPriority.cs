using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct HitsAndMisses
{
	public bool[] hits;
	public bool[] misses;
	
	public HitsAndMisses(bool[] hits, bool[] misses)
	{
		this.hits = hits;
		this.misses = misses;
	}
}

public class BombardmentTargetPriorityTrait: TargetPriorityTrait
{
	private static string initialName = "Bombardment";
	private static string initialTraitDescription = "This creature attacks randomly, sometimes missing it's target.";
	private static string initialTraitIconName = "Bombardment";
	private static Color initialIconBackgroundColor = Color.blue;
	
	private int guaranteedHitChance;
	private int numberOfTiles;
	
	private static bool hit = true;
	private static bool miss = false;
	
	public BombardmentTargetPriorityTrait(int guaranteedHitChance, int numberOfTiles):
		base(initialName, initialTraitDescription, initialTraitIconName, initialIconBackgroundColor)
	{
		this.guaranteedHitChance = guaranteedHitChance;
		this.numberOfTiles = numberOfTiles;
	}
	
	public override bool targetsGround()
	{
		return true;
	}
	
	public override Selector findTargetLocation(Selector selector, ArrayList listOfTargets)
	{		
		return SelectorGenerator.generate(getAllTargetCoords(listOfTargets)).clone();
	}
	
	private GridCoords[] getAllTargetCoords(ArrayList listOfTargets)
	{
		HitsAndMisses hitsAndMisses = getHitsAndMisses();
		GridCoords[] targetCoords = new GridCoords[0];
		
		foreach(bool hit in hitsAndMisses.hits)
		{
			GridCoords[] allPossibleTargets = new GridCoords[listOfTargets.Count];
			
			for(int targetIndex = 0; targetIndex < listOfTargets.Count; targetIndex++)
			{
				allPossibleTargets[targetIndex] = ((Stats) listOfTargets[targetIndex]).position.clone();
			}
			
			GridCoords[] nontargetedAllies = getAllNonTargetedSpaces(targetCoords, allPossibleTargets);
			
			int allyToHitIndex = UnityEngine.Random.Range(0, nontargetedAllies.Length);
			
			targetCoords = Helpers.appendArray<GridCoords>(targetCoords, nontargetedAllies[allyToHitIndex]);
		}
		
		GridCoords[] nonGuaranteedHitSpaces;
		
		if(CombatGrid.positionIsOnAlliedSide(((Stats) listOfTargets[0]).position))
		{
			nonGuaranteedHitSpaces = CombatGrid.getAllSpacesInAllyZone();
		} else
		{
			nonGuaranteedHitSpaces = CombatGrid.getAllSpacesInEnemyZone();
		}
		
		foreach(bool miss in hitsAndMisses.misses)
		{			
			GridCoords[] nontargetedSpaces = getAllNonTargetedSpaces(targetCoords, nonGuaranteedHitSpaces);
			
			if(nontargetedSpaces.Length == 0)
			{
				break;
			}
			
			int spaceToHitIndex = UnityEngine.Random.Range(0, nontargetedSpaces.Length);
			
			targetCoords = Helpers.appendArray<GridCoords>(targetCoords, nontargetedSpaces[spaceToHitIndex]);
		}
		
		return targetCoords;
	}
	
	private GridCoords[] getAllNonTargetedSpaces(GridCoords[] alreadyTargetedCoords, GridCoords[] allPossibleTargets)
	{
		GridCoords[] nontargetedSpaces = new GridCoords[0];
		
		foreach(GridCoords possibleTarget in allPossibleTargets)
		{
			if(!Helpers.hasQuality<GridCoords>(nontargetedSpaces, coords => coords.Equals(possibleTarget)))
			{
				nontargetedSpaces = Helpers.appendArray<GridCoords>(nontargetedSpaces, possibleTarget.clone());
			}
		}
		
		return nontargetedSpaces;
	}
	
	private HitsAndMisses getHitsAndMisses()
	{
		bool[] hits = new bool[0];
		bool[] misses = new bool[0];
		
		for(int rollIndex = 0; rollIndex < numberOfTiles; rollIndex++)
		{
			int currentRoll = UnityEngine.Random.Range(1, 101);
			
			if(hits.Length < CombatGrid.getTotalAliveAllyCount() && currentRoll <= guaranteedHitChance)
			{
				hits = Helpers.appendArray<bool>(hits, true);
			} else
			{
				misses = Helpers.appendArray<bool>(misses, false);
			}
		}
		
		return new HitsAndMisses(hits, misses);
	}
}
