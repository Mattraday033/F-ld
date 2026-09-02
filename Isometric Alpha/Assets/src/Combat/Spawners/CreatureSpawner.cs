using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public interface ICreatureSpawnPackage : IEnumerable
{
    public bool hasCreaturesToSpawn();
}

public static class CreatureSpawner
{

    #region Spawn Specific Packages

    public static void spawnFormation()
    {
        spawn(State.formation);
    }

    public static void spawnEnemyPackInfo()
    {
        spawn(State.enemyPackInfo);
    }

    public static void spawnAllyPackInfo()
    {
        spawn(State.allyPackInfo);
    }

    #endregion

	private static void spawn(ICreatureSpawnPackage spawnPackage)
	{
		if(spawnPackage == null || !spawnPackage.hasCreaturesToSpawn())
		{
			return;
		}
		
        foreach(Stats stats in spawnPackage)
        {
            spawn(stats);
        }
	}

	public static void spawn(Stats stats)
	{
		if(stats == null)
		{
			return;
		}

        spawn(stats, stats.findLocationToSpawn());
	}

	public static void spawn(Stats stats, List<GridCoords> spawnCoords)
	{
		if(stats == null || spawnCoords == null || spawnCoords.Count == 0)
		{
			return;
		}

		foreach (GridCoords coords in spawnCoords)
		{
			if (coords.Equals(GridCoords.getDefaultCoords()) ||
				CombatGrid.combatantExistsAtCoords(coords))
			{
				return;
			}
		}

        stats.instantiateCombatSprite(spawnCoords);

        if(CombatStateManager.whoseTurn == WhoseTurn.Start)
        {
            stats.instateEnvironmentalCombatAction();
        } else
        {
            stats.playSpawnAnimation();
        }

        stats.spawningActions();
	}

    #region Find Spawn Space

	public static GridCoords getNextFreeEnemyFrontLineSpace()
	{
        return getNextFreeFrontBackLineSpace(CombatGrid.enemyRowLowerBounds, 
                                             row => row >= CombatGrid.enemyRowUpperBounds, 
                                             row => row-1);
	}

	public static GridCoords getNextFreeEnemyBackLineSpace()
	{
        return getNextFreeFrontBackLineSpace(CombatGrid.enemyRowUpperBounds, 
                                             row => row <= CombatGrid.enemyRowLowerBounds, 
                                             row => row+1);
	}

	public static GridCoords getNextFreeAllyFrontLineSpace()
	{
        return getNextFreeFrontBackLineSpace(CombatGrid.allyRowUpperBounds, 
                                             row => row <= CombatGrid.allyRowLowerBounds, 
                                             row => row+1);
	}

	public static GridCoords getNextFreeAllyBackLineSpace()
	{
        return getNextFreeFrontBackLineSpace(CombatGrid.allyRowLowerBounds, 
                                             row => row >= CombatGrid.allyRowUpperBounds, 
                                             row => row-1);
	}

	private delegate bool IndexCompareDelegate(int rowOrColumn);
	private delegate int RowColumnChangeDelegate(int rowOrColumn);

	private static GridCoords getNextFreeFrontBackLineSpace(int startRow, IndexCompareDelegate compareDelegate, RowColumnChangeDelegate rowChange)
	{
        for(int row = startRow; compareDelegate(row); row = rowChange(row))
        {
            List<GridCoords> gridCoordsOrder = getJumbledCoordsInRow(row);

            foreach(GridCoords coords in gridCoordsOrder)
            {
                if(!CombatGrid.combatantExistsAtCoords(coords))
                {
                    return coords;
                }
            }
        }

        return GridCoords.getDefaultCoords();
	}

    private static List<GridCoords> getJumbledCoordsInRow(int rowIndex)
    {
		List<GridCoords> gridCoordsInRow = new List<GridCoords>();

        for(int col = CombatGrid.colLeftBounds; col <= CombatGrid.colRightBounds; col++)
        {
            gridCoordsInRow.Add(new GridCoords(rowIndex, col));   
        }

        return gridCoordsInRow.OrderBy(a => Guid.NewGuid()).ToList();
    }

    #endregion
}
