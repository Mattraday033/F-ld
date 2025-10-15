using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
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
	
	public const int allyRowLowerBounds = 7;
	public const int allyRowUpperBounds = 4;
	public const int enemyRowLowerBounds = 3;
	public const int enemyRowUpperBounds = 0;
	
	public const int maximumNumberOfSpaces = 16;
	
	public const float gridSpaceIncrementX = 0.5062f;
	public const float gridSpaceIncrementY = 0.2160031f;
	

	public static Vector3 playerSideRow4Col1 = new Vector3(4.53f+(-6f*gridSpaceIncrementX),-1.87f+(-6f*gridSpaceIncrementY),0f); //top left corner of player side
    public static Vector3 playerSideRow4Col2 = new Vector3(4.53f+(-5f*gridSpaceIncrementX),-1.87f+(-7f*gridSpaceIncrementY),0f);
    public static Vector3 playerSideRow4Col3 = new Vector3(4.53f+(-4f*gridSpaceIncrementX),-1.87f+(-8f*gridSpaceIncrementY),0f);
    public static Vector3 playerSideRow4Col4 = new Vector3(4.53f+(-3f*gridSpaceIncrementX),-1.87f+(-9f*gridSpaceIncrementY),0f);
    
	public static Vector3 playerSideRow3Col1 = new Vector3(4.53f+(-7f*gridSpaceIncrementX),-1.87f+(-7f*gridSpaceIncrementY),0f);
    public static Vector3 playerSideRow3Col2 = new Vector3(4.53f+(-6f*gridSpaceIncrementX),-1.87f+(-8f*gridSpaceIncrementY),0f);
    public static Vector3 playerSideRow3Col3 = new Vector3(4.53f+(-5f*gridSpaceIncrementX),-1.87f+(-9f*gridSpaceIncrementY),0f);
    public static Vector3 playerSideRow3Col4 = new Vector3(4.53f+(-4f*gridSpaceIncrementX),-1.87f+(-10f*gridSpaceIncrementY),0f);
	
	public static Vector3 playerSideRow2Col1 = new Vector3(4.53f+(-8f*gridSpaceIncrementX),-1.87f+(-8f*gridSpaceIncrementY),0f);
    public static Vector3 playerSideRow2Col2 = new Vector3(4.53f+(-7f*gridSpaceIncrementX),-1.87f+(-9f*gridSpaceIncrementY),0f);
    public static Vector3 playerSideRow2Col3 = new Vector3(4.53f+(-6f*gridSpaceIncrementX),-1.87f+(-10f*gridSpaceIncrementY),0f);
    public static Vector3 playerSideRow2Col4 = new Vector3(4.53f+(-5f*gridSpaceIncrementX),-1.87f+(-11f*gridSpaceIncrementY),0f);
	
    public static Vector3 playerSideRow1Col1 = new Vector3(4.53f+(-9f*gridSpaceIncrementX),-1.87f+(-9f*gridSpaceIncrementY),0f); 
    public static Vector3 playerSideRow1Col2 = new Vector3(4.53f+(-8f*gridSpaceIncrementX),-1.87f+(-10f*gridSpaceIncrementY),0f);
    public static Vector3 playerSideRow1Col3 = new Vector3(4.53f+(-7f*gridSpaceIncrementX),-1.87f+(-11f*gridSpaceIncrementY),0f);
    public static Vector3 playerSideRow1Col4 = new Vector3(4.53f+(-6f*gridSpaceIncrementX),-1.87f+(-12f*gridSpaceIncrementY),0f);


	public static Vector3 enemySideRow4Col1 = new Vector3(4.53f+(0f*gridSpaceIncrementX),-1.87f+(0f*gridSpaceIncrementY),0f);  // top left corner of enemy side
    public static Vector3 enemySideRow4Col2 = new Vector3(4.53f+(1f*gridSpaceIncrementX),-1.87f+(-1f*gridSpaceIncrementY),0f);
    public static Vector3 enemySideRow4Col3 = new Vector3(4.53f+(2f*gridSpaceIncrementX),-1.87f+(-2f*gridSpaceIncrementY),0f);
    public static Vector3 enemySideRow4Col4 = new Vector3(4.53f+(3f*gridSpaceIncrementX),-1.87f+(-3f*gridSpaceIncrementY),0f);

    public static Vector3 enemySideRow3Col1 = new Vector3(4.53f+(-1f*gridSpaceIncrementX),-1.87f+(-1f*gridSpaceIncrementY),0f);
    public static Vector3 enemySideRow3Col2 = new Vector3(4.53f+(0f*gridSpaceIncrementX),-1.87f+(-2f*gridSpaceIncrementY),0f);
    public static Vector3 enemySideRow3Col3 = new Vector3(4.53f+(1f*gridSpaceIncrementX),-1.87f+(-3f*gridSpaceIncrementY),0f);
    public static Vector3 enemySideRow3Col4 = new Vector3(4.53f+(2f*gridSpaceIncrementX),-1.87f+(-4f*gridSpaceIncrementY),0f);

    public static Vector3 enemySideRow2Col1 = new Vector3(4.53f+(-2f*gridSpaceIncrementX),-1.87f+(-2f*gridSpaceIncrementY),0f);
    public static Vector3 enemySideRow2Col2 = new Vector3(4.53f+(-1f*gridSpaceIncrementX),-1.87f+(-3f*gridSpaceIncrementY),0f);
    public static Vector3 enemySideRow2Col3 = new Vector3(4.53f+(-0f*gridSpaceIncrementX),-1.87f+(-4f*gridSpaceIncrementY),0f);
    public static Vector3 enemySideRow2Col4 = new Vector3(4.53f+(1f*gridSpaceIncrementX),-1.87f+(-5f*gridSpaceIncrementY),0f);
            
	public static Vector3 enemySideRow1Col1 = new Vector3(4.53f+(-3f*gridSpaceIncrementX),-1.87f+(-3f*gridSpaceIncrementY),0f); 
    public static Vector3 enemySideRow1Col2 = new Vector3(4.53f+(-2f*gridSpaceIncrementX),-1.87f+(-4f*gridSpaceIncrementY),0f);
    public static Vector3 enemySideRow1Col3 = new Vector3(4.53f+(-1f*gridSpaceIncrementX),-1.87f+(-5f*gridSpaceIncrementY),0f);
    public static Vector3 enemySideRow1Col4 = new Vector3(4.53f+(-0f*gridSpaceIncrementX),-1.87f+(-6f*gridSpaceIncrementY),0f);
	
	
	
	
    public static Vector3[][] fullCombatGrid = new Vector3[][]{new Vector3[]{enemySideRow4Col1 , enemySideRow4Col2 , enemySideRow4Col3 , enemySideRow4Col4 },
															   new Vector3[]{enemySideRow3Col1 , enemySideRow3Col2 , enemySideRow3Col3 , enemySideRow3Col4 },
                                                               new Vector3[]{enemySideRow2Col1 , enemySideRow2Col2 , enemySideRow2Col3 , enemySideRow2Col4 }, 
                                                               new Vector3[]{enemySideRow1Col1 , enemySideRow1Col2 , enemySideRow1Col3 , enemySideRow1Col4 },
															   new Vector3[]{playerSideRow4Col1, playerSideRow4Col2, playerSideRow4Col3, playerSideRow4Col4},
                                                               new Vector3[]{playerSideRow3Col1, playerSideRow3Col2, playerSideRow3Col3, playerSideRow3Col4},
                                                               new Vector3[]{playerSideRow2Col1, playerSideRow2Col2, playerSideRow2Col3, playerSideRow2Col4},
															   new Vector3[]{playerSideRow1Col1, playerSideRow1Col2, playerSideRow1Col3, playerSideRow1Col4}};
	
	public static RowOfCombatants[] combatantStatsGrid = new RowOfCombatants[8];
	
	//resets combat grid
	public static void cleanCombatGrid()
	{
		combatantStatsGrid = new RowOfCombatants[8];
		
		for(int i = 0; i < combatantStatsGrid.Length; i++)
		{
			combatantStatsGrid[i].row = new Stats[4];
		}
	}
	
	public static Vector3 getPositionAt(int row, int col)
	{
		return fullCombatGrid[row][col];
	}
	
	public static Vector3 getPositionAt(GridCoords coords)
	{
		return fullCombatGrid[coords.row][coords.col];
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
		
		combatant.combatSprite.transform.position = fullCombatGrid[newCoords.row][newCoords.col] + combatant.adjustment;
		combatant.healthBar.transform.position = fullCombatGrid[newCoords.row][newCoords.col] + Stats.healthBarAdjustment;
		Helpers.updateGameObjectPosition(combatant.combatSprite);
		Helpers.updateGameObjectPosition(combatant.healthBar);
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
		return getEnemyTypeCount("Master");
	}
	
	public static int getEnemyMinionCount()
	{
		return getEnemyTypeCount("Minion");
	}
	
	public static int getEnemyTypeCount(string type)
	{
		ArrayList listOfEnemies = getAllAliveEnemyCombatants();
		int enemyTypeCount = 0;
		
		foreach(Stats enemy in listOfEnemies)
		{
			if(enemy.traitNames[0].Equals(type))
			{
				enemyTypeCount++;
			}
		}
		
		return enemyTypeCount;
	}
	
	public static int howManyEmptyEnemySpaces()
	{
		return maximumNumberOfSpaces - getTotalEnemyCount();
	}
	
	public static ArrayList getAllAliveSummonedEnemies()
	{
		ArrayList allAliveSummonedEnemies = new ArrayList();

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
	
	public static ArrayList getAllAliveNonsummonedEnemies()
	{
		ArrayList allAllyCombatants = new ArrayList();

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
	
	public static ArrayList getAllAliveSummonedAllies()
	{
		ArrayList allAliveSummonedAllies = new ArrayList();

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
	
	public static ArrayList getAllAliveNonsummonedAllies()
	{
		ArrayList allAllyCombatants = new ArrayList();

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
	
	public static ArrayList getAllAliveAllyCombatants()
	{
		ArrayList allAllyCombatants = new ArrayList();

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
	
	public static ArrayList getAllAliveEnemyCombatants()
	{
		ArrayList allEnemyCombatants = new ArrayList();

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

	public static ArrayList getAllAliveCombatants()
	{
		ArrayList allCombatants = getAllAliveAllyCombatants();
		
		allCombatants.AddRange(getAllAliveEnemyCombatants());
		
		return allCombatants;
	}
	
	public static ArrayList getAllAllyCombatants()
	{
		ArrayList allAllyCombatants = new ArrayList();

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
		ArrayList allAllyCombatants = getAllAllyCombatants();
		
		foreach(Stats ally in allAllyCombatants)
		{
			if(ally.position.Equals(coords) && ally.isAlive())
			{
				return true;
			}
		}
		
		return false;
	}

	public static ArrayList getAllNonsummonedAllyCombatants()
	{
		ArrayList allAllyCombatants = new ArrayList();

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
	
	public static ArrayList getAllEnemyCombatants()
	{
		ArrayList allEnemyCombatants = new ArrayList();

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

	public static ArrayList getAllCombatants()
	{
		ArrayList allCombatants = getAllAllyCombatants();
		
		allCombatants.AddRange(getAllEnemyCombatants());
		
		return allCombatants;
	}
	
	public static ArrayList getAllZOITargets(GridCoords coords)
	{
		if(coords.row > allyRowLowerBounds || 
			coords.row < allyRowUpperBounds ||
			coords.col < colLeftBounds ||
			coords.col > colRightBounds)
			{
				throw new IOException("Given Coords not within Ally Bounds. Coords = " + coords.ToString());
			}

		ArrayList allZOITargets = new ArrayList();
		
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

	private static Stats getMandatoryTargetFromList(ArrayList targets)
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

				if (currentCombatant != null && currentCombatant.isDead &&
					Helpers.hasQuality<Trait>(currentCombatant.traits, t => t.deleteIfDead()))
				{
					CombatGrid.setCombatantAtCoords(rowIndex, colIndex, null);
				}
			}
		}
	}
	
	private static ArrayList scrubDuplicatesFromList(ArrayList combatantList)
	{
		for(int combatantIndex = 0; combatantIndex < combatantList.Count; combatantIndex++)
		{
			Stats currentCombatant = (Stats) combatantList[combatantIndex];
			
			for(int priorCombatantIndex = combatantIndex-1; priorCombatantIndex >= 0; priorCombatantIndex--)
			{
				Stats currentPriorCombatant = (Stats) combatantList[priorCombatantIndex];
				
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