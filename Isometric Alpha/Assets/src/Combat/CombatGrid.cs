using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;

public struct RowOfCombatants
{
	public Stats[] row;
	
	public Stats getCol(int index)
	{
		return row[index];
	}
	
	public void setCol(int index, Stats newCombatant)
	{
		row[index] = newCombatant;
	}
}

public static class CombatGrid
{	
	public const int rowLowerBounds = 7;
	public const int colLeftBounds = 0;
	public const int rowUpperBounds = 0;
    public const int colRightBounds = 3;

    public const int firstNoMansLandRow = 4;
    public const int noMansLandWidth = 2;

	public const int allyRowLowerBounds = 7;
	public const int allyRowUpperBounds = 4;
	public const int enemyRowLowerBounds = 3;
	public const int enemyRowUpperBounds = 0;
	
	public const int maximumNumberOfSpaces = 16;
	
	public static RowOfCombatants[] combatantStatsGrid = new RowOfCombatants[8];

    //resets combat grid
    public static void cleanCombatGrid()
    {
        combatantStatsGrid = new RowOfCombatants[8];

        for (int i = 0; i < combatantStatsGrid.Length; i++)
        {
            combatantStatsGrid[i].row = new Stats[4];
        }
    }

	public static Vector3 getPositionAt(GridCoords coords)
    {
        return getPositionAt(coords.row, coords.col);
	}

    public static Vector3 getPositionAt(int row, int col)
    {
        Grid creatureGrid = CombatStateManager.getCreatureGrid();

        if(row >= firstNoMansLandRow)
        {
            row += noMansLandWidth;
        }

        return creatureGrid.GetCellCenterWorld(new Vector3Int(-1 * row, -1 * col));
    }

	public static bool positionIsOnAlliedSide(GridCoords coords)
	{
		return (coords.row >= allyRowUpperBounds && coords.row <= allyRowLowerBounds);
	}

    public static bool positionIsOnEnemySide(GridCoords coords)
    {
        return (coords.row >= enemyRowUpperBounds && coords.row <= enemyRowLowerBounds);
    }
    public static bool positionsAreOnSameSide(GridCoords firstCoords, GridCoords secondCoords)
    {
		return (positionIsOnAlliedSide(firstCoords) && positionIsOnAlliedSide(secondCoords)) || (positionIsOnEnemySide(firstCoords) && positionIsOnEnemySide(secondCoords));
    }
    /*
	public const int allyRowLowerBounds = 7;
	public const int allyRowUpperBounds = 4;	
	*/

    public static int getNumberOfRows()
	{
		return combatantStatsGrid.Length;
	}
	
	public static int getNumberOfCols()
	{
		return combatantStatsGrid[0].row.Length;
	}
	
	public static void updateStatsSpritePosition(GridCoords newCoords)
	{
		Stats combatant = getCombatantAtCoords(newCoords);
		
		if(combatant == null)
		{
			return;
		}
		
		combatant.combatSprite.transform.position = getPositionAt(newCoords.row, newCoords.col);
		Helpers.updateGameObjectPosition(combatant.combatSprite);
	}
	
	//careful when using, if there is already something in the given grid space
	//and you aren't setting it to null, this will throw an error
	public static void setCombatantAtCoords(int rowIndex, int colIndex, Stats newCombatant)
	{
		setCombatantAtCoords(new GridCoords(rowIndex, colIndex), newCombatant);
	}
	
	//careful when using, if there is already something in the given grid space
	//and you aren't setting it to null, this will throw an error
	public static void setCombatantAtCoords(GridCoords coords, Stats newCombatant)
	{
		combatantStatsGrid[coords.row].setCol(coords.col, newCombatant);
	}
	
	//null means no one is at given coords
	public static Stats getCombatantAtCoords(int rowIndex, int colIndex)
	{
		if(rowIndex < 0 || colIndex < 0)
		{
			return null;
		}
		
		return getCombatantAtCoords(new GridCoords(rowIndex,colIndex));
	}
	
	//null means no one is at given coords
	public static Stats getCombatantAtCoords(GridCoords coords)
	{
		if(coords.row < 0 || coords.col < 0)
		{
			return null;
		}
		
		return combatantStatsGrid[coords.row].getCol(coords.col);
	}
	
	public static int actualEnemyMinionCombatActionCount()
	{
		return getEnemyMinionCount(); 
	}
	
	public static int expectedEnemyMasterCombatActionCount()
	{
		return getEnemyMasterCount();
	}
	
	public static int expectedEnemyMinionCombatActionCount()
	{
		return getEnemyMinionCount(); 
	}
	
	public static int getTotalAliveAllyCount()
	{
		return getAllAliveAllyCombatants().Count;
	}
	
	public static int getTotalAliveEnemyCount()
	{
		return getAllAliveEnemyCombatants().Count;
	}
	
	public static int getTotalEnemyCount()
	{
		return getAllEnemyCombatants().Count;
	}
	
	public static int getEnemyMasterCount()
	{
		return getEnemyTypeCount(TraitList.master);
	}
	
	public static int getEnemyMinionCount()
	{
		return getEnemyTypeCount(TraitList.minion);
	}
	
	public static int getEnemyTypeCount(Trait typeTrait)
	{
		List<Stats> listOfEnemies = getAllAliveEnemyCombatants();
		int enemyTypeCount = 0;
		
		foreach(Stats enemy in listOfEnemies)
		{
            foreach(Trait trait in enemy.traits)
            {
                if(trait.getName().Equals(typeTrait.getName()))
                {
   				    enemyTypeCount++;                 
                }
            }
		}
		
		return enemyTypeCount;
	}
	
	public static int howManyEmptyEnemySpaces()
	{
		return maximumNumberOfSpaces - getTotalEnemyCount();
	}
	
	public static List<Stats> getAllAliveSummonedEnemies()
	{
		List<Stats> allAliveSummonedEnemies = new List<Stats>();

		for(int rowIndex = enemyRowUpperBounds; rowIndex <= enemyRowLowerBounds; rowIndex++)
		{
			foreach(Stats combatantSlot in combatantStatsGrid[rowIndex].row)
			{
				if(combatantSlot != null && combatantSlot.isAlive() && combatantSlot.isTargetable() && combatantSlot.wasSummoned())
				{
					allAliveSummonedEnemies.Add(combatantSlot);
				}
			}
		}
		
		return scrubDuplicatesFromList(allAliveSummonedEnemies);
	}
	
	public static List<Stats> getAllAliveNonsummonedEnemies()
	{
		List<Stats> allAllyCombatants = new List<Stats>();

		for(int rowIndex = enemyRowUpperBounds; rowIndex <= enemyRowLowerBounds; rowIndex++)
		{
			foreach(Stats combatantSlot in combatantStatsGrid[rowIndex].row)
			{
				if(combatantSlot != null && combatantSlot.isAlive() && combatantSlot.isTargetable() && !combatantSlot.wasSummoned())
				{
					allAllyCombatants.Add(combatantSlot);
				}
			}
		}
		
		return scrubDuplicatesFromList(allAllyCombatants);
	}
	
	public static List<Stats> getAllAliveSummonedAllies()
	{
		List<Stats> allAliveSummonedAllies = new List<Stats>();

		for(int rowIndex = allyRowUpperBounds; rowIndex <= allyRowLowerBounds; rowIndex++)
		{
			foreach(Stats combatantSlot in combatantStatsGrid[rowIndex].row)
			{
				if(combatantSlot != null && combatantSlot.isAlive() && combatantSlot.isTargetable() && combatantSlot.wasSummoned())
				{
					allAliveSummonedAllies.Add(combatantSlot);
				}
			}
		}
		
		return scrubDuplicatesFromList(allAliveSummonedAllies);
	}
	
	public static List<Stats> getAllAliveNonsummonedAllies()
	{
		List<Stats> allAllyCombatants = new List<Stats>();

		for(int rowIndex = allyRowUpperBounds; rowIndex <= allyRowLowerBounds; rowIndex++)
		{
			foreach(Stats combatantSlot in combatantStatsGrid[rowIndex].row)
			{
				if(combatantSlot != null && combatantSlot.isAlive() && combatantSlot.isTargetable() && !combatantSlot.wasSummoned())
				{
					allAllyCombatants.Add(combatantSlot);
				}
			}
		}
		
		return scrubDuplicatesFromList(allAllyCombatants);
	}
	
	public static List<Stats> getAllAliveAllyCombatants()
	{
		List<Stats> allAllyCombatants = new List<Stats>();

		for(int rowIndex = allyRowUpperBounds; rowIndex <= allyRowLowerBounds; rowIndex++)
		{
			foreach(Stats combatantSlot in combatantStatsGrid[rowIndex].row)
			{
				if(combatantSlot != null && combatantSlot.isAlive() && combatantSlot.isTargetable())
				{
					allAllyCombatants.Add(combatantSlot);
				}
			}
		}
		
		return scrubDuplicatesFromList(allAllyCombatants);
	}
	
	public static List<Stats> getAllAliveEnemyCombatants()
	{
		List<Stats> allEnemyCombatants = new List<Stats>();

		for(int rowIndex = enemyRowUpperBounds; rowIndex <= enemyRowLowerBounds; rowIndex++)
		{
			foreach(Stats combatantSlot in combatantStatsGrid[rowIndex].row)
			{
				if(combatantSlot != null && combatantSlot.isAlive() && combatantSlot.isTargetable())
				{	
					allEnemyCombatants.Add(combatantSlot);
				}
			}
		}
		
		return scrubDuplicatesFromList(allEnemyCombatants);
	}

	public static List<Stats> getAllAliveCombatants()
	{
		List<Stats> allCombatants = getAllAliveAllyCombatants();
		
		allCombatants.AddRange(getAllAliveEnemyCombatants());
		
		return allCombatants;
	}
	
	public static List<Stats> getAllAllyCombatants()
	{
		List<Stats> allAllyCombatants = new List<Stats>();

		for(int rowIndex = allyRowUpperBounds; rowIndex <= allyRowLowerBounds; rowIndex++)
		{
			foreach(Stats combatantSlot in combatantStatsGrid[rowIndex].row)
			{
				if(combatantSlot != null && combatantSlot.isTargetable())
				{
					allAllyCombatants.Add(combatantSlot);
				}
			}
		}
		
		return scrubDuplicatesFromList(allAllyCombatants);
	}

	public static bool selectableAllyAtLocation(GridCoords coords)
	{
		List<Stats> allAllyCombatants = getAllAllyCombatants();
		
		foreach(Stats ally in allAllyCombatants)
		{
			if(ally.position.Equals(coords) && ally.isAlive())
			{
				return true;
			}
		}
		
		return false;
	}

	public static List<Stats> getAllNonsummonedAllyCombatants()
	{
		List<Stats> allAllyCombatants = new List<Stats>();

		for (int rowIndex = allyRowUpperBounds; rowIndex <= allyRowLowerBounds; rowIndex++)
		{
			foreach (Stats combatantSlot in combatantStatsGrid[rowIndex].row)
			{
				if (combatantSlot != null && combatantSlot.isTargetable() && !combatantSlot.wasSummoned())
				{
					allAllyCombatants.Add(combatantSlot);
				}
			}
		}

		return scrubDuplicatesFromList(allAllyCombatants);
	}
	
	public static List<Stats> getAllEnemyCombatants()
	{
		List<Stats> allEnemyCombatants = new List<Stats>();

		for(int rowIndex = enemyRowUpperBounds; rowIndex <= enemyRowLowerBounds; rowIndex++)
		{
			foreach(Stats combatantSlot in combatantStatsGrid[rowIndex].row)
			{
				if(combatantSlot != null && combatantSlot.isTargetable())
				{	
					allEnemyCombatants.Add(combatantSlot);
				}
			}
		}
		
		return scrubDuplicatesFromList(allEnemyCombatants);
	}

	public static List<Stats> getAllCombatants()
	{
		List<Stats> allCombatants = getAllAllyCombatants();
		
		allCombatants.AddRange(getAllEnemyCombatants());
		
		return allCombatants;
	}
	
	public static List<Stats> getAllZOITargets(GridCoords coords)
	{
		if(coords.row > allyRowLowerBounds || 
			coords.row < allyRowUpperBounds ||
			coords.col < colLeftBounds ||
			coords.col > colRightBounds)
			{
				throw new IOException("Given Coords not within Ally Bounds. Coords = " + coords.ToString());
			}

		List<Stats> allZOITargets = new List<Stats>();
		
		if(coords.row-1 >= allyRowUpperBounds && getCombatantAtCoords(coords.row-1,coords.col) != null)
		{
			allZOITargets.Add(getCombatantAtCoords(coords.row-1,coords.col));
		}
		
		if(coords.row+1 <= allyRowLowerBounds && getCombatantAtCoords(coords.row+1,coords.col) != null)
		{
			allZOITargets.Add(getCombatantAtCoords(coords.row+1,coords.col));
		}
		
		if(coords.col-1 >= colLeftBounds && getCombatantAtCoords(coords.row,coords.col-1) != null)
		{
			allZOITargets.Add(getCombatantAtCoords(coords.row,coords.col-1));
		}
		
		if(coords.col+1 <= colRightBounds  && getCombatantAtCoords(coords.row,coords.col+1) != null)
		{
			allZOITargets.Add(getCombatantAtCoords(coords.row,coords.col+1));
		}
		
		return allZOITargets;
	}
	
	public static GridCoords findRandomOpenSpace(int startRow, int endRow)
	{
		return findRandomOpenSpace(getAllEmptySpacesInArea(startRow, endRow));
	}
	
	public static GridCoords findRandomOpenSpace(GridCoords[] emptySpaceCoords)
	{
		if(emptySpaceCoords.Length <= 0)
		{
			return GridCoords.getDefaultCoords();
		}
		
		return emptySpaceCoords[UnityEngine.Random.Range(0, emptySpaceCoords.Length)];
	}

	public static GridCoords[] getAllEmptySpacesInAllyZone()
	{
		return getAllEmptySpacesInArea(allyRowUpperBounds, allyRowLowerBounds);
	}
	
	public static GridCoords findRandomOpenSpaceInAllyZone()
	{
		return findRandomOpenSpace(getAllEmptySpacesInArea(allyRowUpperBounds, allyRowLowerBounds));
	}
	
	public static GridCoords[] getAllEmptySpacesInEnemyZone()
	{
		return getAllEmptySpacesInArea(enemyRowUpperBounds, enemyRowLowerBounds);
	}

	public static GridCoords findRandomOpenSpaceInEnemyZone()
	{
		return findRandomOpenSpace(getAllEmptySpacesInArea(enemyRowUpperBounds, enemyRowLowerBounds));
	}

	private static GridCoords[] getAllEmptySpacesInArea(int startRow, int endRow)
	{
		GridCoords[] allEmptySpaces = new GridCoords[0];
		
		for(int rowIndex = startRow; rowIndex <= endRow; rowIndex++)
		{
			int colIndex = 0;

			foreach(Stats space in combatantStatsGrid[rowIndex].row)
			{
				if(space == null && space is null && !CombatStateManager.allQueuedSummonLocations.Contains(new GridCoords(rowIndex, colIndex)))
				{
					allEmptySpaces = Helpers.appendArray(allEmptySpaces, new GridCoords(rowIndex, colIndex));
				}
				colIndex++;
			}
		}
		
		return allEmptySpaces;
	}
	/*
		public const int allyRowLowerBounds = 7;
	public const int allyRowUpperBounds = 4;
	public const int enemyRowLowerBounds = 3;
	public const int enemyRowUpperBounds = 0;
	*/

	public static GridCoords[] getAllSpacesInAllyZone()
	{
		return getAllSpacesInArea(allyRowUpperBounds, allyRowLowerBounds);
	}

	public static GridCoords[] getAllSpacesInEnemyZone()
	{
		return getAllSpacesInArea(enemyRowUpperBounds, enemyRowLowerBounds);
	}

	private static GridCoords[] getAllSpacesInArea(int startRow, int endRow)
	{
		GridCoords[] allSpaces = new GridCoords[(endRow-startRow+1)*4];
		
		int arrayIndex = 0;
		for(int rowIndex = startRow; rowIndex <= endRow; rowIndex++)
		{
			for(int colIndex = colLeftBounds; colIndex <= colRightBounds; colIndex++)
			{
				allSpaces[arrayIndex] = new GridCoords(rowIndex, colIndex);
				arrayIndex++;
			}
		}
		
		return allSpaces;
	}
	
	public static Stats enemyHasMandatoryTarget()
	{
		return getMandatoryTargetFromList(getAllAliveEnemyCombatants());
	}
	
	public static Stats allyHasMandatoryTarget()
	{		
		return getMandatoryTargetFromList(getAllAliveAllyCombatants());
	}

	private static Stats getMandatoryTargetFromList(List<Stats> targets)
	{
		foreach(Stats target in targets)
		{
			if(Helpers.hasQuality<Trait>(target.traits, t => t.isMandatoryTarget()))
			{
				return target;
			}
		}
		
		return null;
	}
	
	public static void deleteDeadOnDeathEffectActors()
	{
		for (int rowIndex = 0; rowIndex < CombatGrid.combatantStatsGrid.Length; rowIndex++)
		{
			for (int colIndex = 0; colIndex < CombatGrid.combatantStatsGrid[rowIndex].row.Length; colIndex++)
			{
				Stats currentCombatant = CombatGrid.getCombatantAtCoords(rowIndex, colIndex);

				if (currentCombatant != null && currentCombatant.isDead() &&
					Helpers.hasQuality<Trait>(currentCombatant.traits, t => t.deleteIfDead()))
				{
					CombatGrid.setCombatantAtCoords(rowIndex, colIndex, null);
				}
			}
		}
	}
	
	private static List<Stats> scrubDuplicatesFromList(List<Stats> combatantList)
	{
		for(int combatantIndex = 0; combatantIndex < combatantList.Count; combatantIndex++)
		{
			Stats currentCombatant = combatantList[combatantIndex];
			
			for(int priorCombatantIndex = combatantIndex-1; priorCombatantIndex >= 0; priorCombatantIndex--)
			{
				Stats currentPriorCombatant = combatantList[priorCombatantIndex];
				
				if(currentCombatant.position.Equals(currentPriorCombatant.position))
				{
					combatantList.RemoveAt(combatantIndex);
					combatantIndex--;
					break;
				}
			}
		}
		
		return combatantList;
	}
}