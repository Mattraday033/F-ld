using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum CombatGridSide
{
    Enemy,
    Ally,
    NoPosition
}

public delegate bool CombatantSearchCriteria(Stats stats);

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
	
    public static Dictionary<GridCoords,Combatant> combatantsDict = new Dictionary<GridCoords,Combatant>();
    public readonly static UnityEvent LastEnemyKilled = new UnityEvent();

    [RuntimeInitializeOnLoadMethod]
    public static void cleanCombatGrid()
    {
        combatantsDict = new Dictionary<GridCoords,Combatant>();
    }

	//use removeCombatantAtCoords to clear a cell
	public static void setCombatantAtCoords(GridCoords coords, Stats newStats)
	{
        if(newStats == null)
        {
            removeCombatantAtCoords(coords);
            return;
        }

        Combatant combatant = getCombatantForCell(newStats, coords);

        if(combatant == null)
        {
            Debug.LogWarning("CombatGrid: " + newStats.uniqueName + " has no Combatant, so it can't be placed at " + coords.ToString());
            return;
        }

        combatantsDict[coords] = combatant;

        removeNullCombatants();
	}

    //the player's cells are never cleared, so the player stays on the grid. The player's placeholder carries the
    //player's name but isn't the player, so it can be cleared
    public static void removeCombatantAtCoords(GridCoords coords)
    {
        if(!combatantsDict.TryGetValue(coords, out Combatant combatant))
        {
            return;
        }

        if(combatant != null && !combatant.isPlaceholder && combatant.uniqueName.Contains(PartyManager.playerMarker))
        {
            return;
        }

        combatantsDict.Remove(coords);

        if(getTotalAliveEnemyCount() == 0)
        {
            LastEnemyKilled.Invoke();
        }
    }

    //the Combatant drawn on that cell, which for a combatant with a single sprite is its primary on every cell it covers
    private static Combatant getCombatantForCell(Stats stats, GridCoords coords)
    {
        foreach(Combatant combatant in stats.combatants)
        {
            if(combatant != null && combatant.cell.Equals(coords))
            {
                return combatant;
            }
        }

        return stats.getCombatant();
    }

    //every Combatant on the grid, skipping destroyed ones. A multi cell combatant appears once per cell
    private static IEnumerable<Combatant> getCombatantsOnGrid()
    {
        return combatantsDict.Values.Where(c => c != null && c.stats != null);
    }

    //the Combatant drawn on that cell; ask it for its stats when the data is needed
    public static bool tryGetCombatantAt(GridCoords coords, out Combatant combatant)
    {
        combatant = null;

        if(coords.row >= 0 && coords.col >= 0 &&
            combatantsDict.TryGetValue(coords, out Combatant combatantAtCoords) &&
            combatantAtCoords != null &&
            combatantAtCoords.stats != null)
        {
            combatant = combatantAtCoords;
        }

        return combatant != null;
    }

    public static bool tryGetCombatantAt(int rowIndex, int colIndex, out Combatant combatant)
    {
        return tryGetCombatantAt(new GridCoords(rowIndex, colIndex), out combatant);
    }

	public static Vector3 getPositionAt(GridCoords coords)
    {
        return getPositionAt(coords.row, coords.col);
	}

    public static Vector3 getAveragePositionAt(IEnumerable<GridCoords> coords)
    {
        return Helpers.getAveragePosition(coords.Select(getPositionAt).ToList());
    }

    public static Vector3 getPositionAt(int row, int col)
    {
        Grid creatureGrid = CombatStateManager.getCreatureGrid();

        if(row >= firstNoMansLandRow)
        {
            row += noMansLandWidth;
        }

        Vector3 position = creatureGrid.GetCellCenterWorld(new Vector3Int(-1 * row, -1 * col));

        return new Vector3(position.x, position.y, -1*(row+col) - .01f*row);
    }

    public static CombatGridSide getCombatGridSide(this Stats stats)
    {
        if(stats == null || 
            stats.positions == null ||
            stats.positions.Count <= 0)
        {
            return CombatGridSide.NoPosition;
        }

        if(positionIsOnAlliedSide(stats.positions[0]))
        {
            return CombatGridSide.Ally;
        }

            return CombatGridSide.Enemy;
    }

	public static bool positionIsOnAlliedSide(GridCoords coords)
	{
		return coords.row >= allyRowUpperBounds && coords.row <= allyRowLowerBounds;
	}

    public static bool positionIsOnEnemySide(GridCoords coords)
    {
        return coords.row >= enemyRowUpperBounds && coords.row <= enemyRowLowerBounds;
    }
    
    public static bool positionsAreOnSameSide(GridCoords firstCoords, GridCoords secondCoords)
    {
		return (positionIsOnAlliedSide(firstCoords) && positionIsOnAlliedSide(secondCoords)) || (positionIsOnEnemySide(firstCoords) && positionIsOnEnemySide(secondCoords));
    }
	
    private static void removeNullCombatants()
    {
        List<GridCoords> nullCoords = new List<GridCoords>();

        foreach(GridCoords coords in combatantsDict.Keys)
        {
            if(combatantsDict[coords] == null)
            {
                nullCoords.Add(coords);
            }
        }

        foreach(GridCoords nullCoord in nullCoords)
        {
            combatantsDict.Remove(nullCoord);
        }
    }
    public static GridCoords getNextAllyToJumpSelectorTo(GridCoords currentCoords)
    {
        return getNextCombatantToJumpSelectorTo(currentCoords, forward: true, ally: true);
    }

    public static GridCoords getPreviousAllyToJumpSelectorTo(GridCoords currentCoords)
    {
        return getNextCombatantToJumpSelectorTo(currentCoords, forward: false, ally: true);
    }

    public static GridCoords getNextEnemyToJumpSelectorTo(GridCoords currentCoords)
    {
        return getNextCombatantToJumpSelectorTo(currentCoords, forward: true, ally: false);
    }

    public static GridCoords getPreviousEnemyToJumpSelectorTo(GridCoords currentCoords)
    {
        return getNextCombatantToJumpSelectorTo(currentCoords, forward: false, ally: false);
    }

    private static GridCoords getNextCombatantToJumpSelectorTo(GridCoords currentCoords, bool forward, bool ally)
    {
        List<Stats> statsOnSide = new List<Stats>();
        IEnumerable<Stats> statsOnGrid = getCombatantsOnGrid().Select(c => c.stats);

        if(ally)
        {
            statsOnSide = new List<Stats>(statsOnGrid.Where(s => s.positions.Count > 0 && positionIsOnAlliedSide(s.positions[0])));
        } else
        {
            statsOnSide = new List<Stats>(statsOnGrid.Where(s => s.positions.Count > 0 && positionIsOnEnemySide(s.positions[0])));
        }

        statsOnSide = statsOnSide.Distinct().ToList();

        if(statsOnSide.Count <= 1)
        {
            return currentCoords;
        } else if(!combatantExistsAtCoords(currentCoords) ||
                (ally && positionIsOnEnemySide(SelectorManager.currentSelector.getCoords())) ||
                (!ally && positionIsOnAlliedSide(SelectorManager.currentSelector.getCoords())))
        {
            return statsOnSide[0].positions[0];
        }

        Stats targetStats = null;
        bool currentFound = tryGetCombatantAt(currentCoords, out Combatant current);

        int i = 0;
        foreach(Stats stats in statsOnSide)
        {
            //walk up to the combatant the selector is on, then step one past it
            if(!(currentFound && current.stats.Equals(stats)))
            {
                i++;
                continue;
            }

            if(forward)
            {
                if(i < statsOnSide.Count-1)
                {
                    targetStats = statsOnSide[i+1];
                    break;
                } else
                {
                    targetStats = statsOnSide[0];
                    break;
                }
            } else
            {
                if(i > 0)
                {
                    targetStats = statsOnSide[i-1];
                    break;
                } else
                {
                    targetStats = statsOnSide[statsOnSide.Count-1];
                    break;
                }
            }
        }

        //the selector's combatant wasn't among them, so start from the first
        if(targetStats == null)
        {
            targetStats = statsOnSide[0];
        }

        if(SelectorManager.currentSelector.singleTile() || targetStats.positions.Count <= 1)
        {
            return SelectorManager.findLegalCoordsContainingMandatoryTarget(SelectorManager.currentSelector, targetStats.positions[0]);
        } else
        {
            return getCoordsWithHighestCoverageOfTargetPositions(targetStats);
        }
    }

    public static GridCoords getCoordsWithHighestCoverageOfTargetPositions(Stats targetStats)
    {
        int bestOverlap = 0;
        GridCoords coordsWithHighestCoveragee = SelectorManager.findLegalCoordsContainingMandatoryTarget(SelectorManager.currentSelector, targetStats.positions[0]);

        for(int i = 0; i < targetStats.positions.Count; i++)
        {
            GridCoords legalPosition = SelectorManager.findLegalCoordsContainingMandatoryTarget(SelectorManager.currentSelector, targetStats.positions[i]);
            Selector clone = SelectorManager.currentSelector.clone();
            clone.setToLocation(legalPosition, declareSelectors: false);

            int currentOverlap = targetStats.positions.Intersect(clone.getAllSelectorCoords().ToList()).Count();

            if(bestOverlap < currentOverlap)
            {
                bestOverlap = currentOverlap;
                coordsWithHighestCoveragee = legalPosition;
            }
        }

        return coordsWithHighestCoveragee;
    }

    public static void addCombatantToGrid(Stats stats)
    {
        foreach(GridCoords coords in stats.positions)
        {
            setCombatantAtCoords(coords, stats);
        }
    }

    public static void removeCombatantFromGrid(Stats stats)
    {
        foreach(GridCoords coords in stats.positions)
        {
            removeCombatantAtCoords(coords);
        }
    }

	public static bool combatantExistsAtCoords(GridCoords coords)
	{
        return tryGetCombatantAt(coords, out Combatant combatant);
	}
	
	public static int getTotalAliveAllyCount()
	{
		return getAllAliveAllyCombatants().Count;
	}
	
	public static int getTotalAliveEnemyCount()
	{
		return getAllAliveEnemyCombatants().Count;
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
            foreach(Trait trait in enemy.traitContainer)
            {
                if(trait.uniqueName.Equals(typeTrait.uniqueName))
                {
   				    enemyTypeCount++;                 
                }
            }
		}
		
		return enemyTypeCount;
	}
	
    public static List<Stats> getAllCombatantsThatMeetCriteria(CombatantSearchCriteria meetsCriteria)
    {
        List<Stats> matches = new List<Stats>();

        foreach(Combatant combatant in getCombatantsOnGrid())
        {
            if(meetsCriteria(combatant.stats))
            {
                matches.Add(combatant.stats);
            }
        }
		
		return scrubDuplicatesFromList(matches);
    }

	public static List<Stats> getAllAliveSummonedEnemies()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnEnemySide(p)) && c.isAlive && c.isTargetable && c.isPartOfVolley);
	}

	public static List<Stats> getAllAliveNonsummonedEnemies()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnEnemySide(p)) && c.isAlive && c.isTargetable &&  !c.isPartOfVolley);
	}

	public static List<Stats> getAllAliveSummonedAllies()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnAlliedSide(p)) && c.isAlive && c.isTargetable && c.isSummon);
	}

	public static List<Stats> getAllAliveNonsummonedAllies()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnAlliedSide(p)) && c.isAlive && c.isTargetable && !c.isSummon);
	}

	public static List<Stats> getAllAliveAllyCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnAlliedSide(p)) && c.isAlive && c.isTargetable);
	}

	public static List<Stats> getAllAliveEnemyCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnEnemySide(p)) && c.isAlive && c.isTargetable);
	}

	public static List<Stats> getAllAliveCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.isAlive && c.isTargetable);
	}

	public static List<Stats> getAllAllyCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnAlliedSide(p)) && c.isTargetable);
	}

	public static bool selectableAllyAtLocation(GridCoords coords)
	{
        return tryGetCombatantAt(coords, out Combatant ally) && ally.stats.isAlive;
	}

	public static List<Stats> getAllNonsummonedAllyCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnAlliedSide(p)) && c.isTargetable && !c.isSummon);
	}

	public static List<Stats> getAllEnemyCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnEnemySide(p)) && c.isTargetable);
	}

    public static Stats findOriginalCombatant(Stats repositionClone)
    {
        List<Stats> stats = getAllCombatants();

        foreach(Stats stat in stats)
        {
            if(stat != null && stat.repositionClone != null && stat.repositionClone.Equals(repositionClone))
            {
                return stat;
            }
        }

        return null;
    }

	public static List<Stats> getAllCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.isTargetable);
	}

	public static List<Stats> getAllZOITargets(GridCoords coords)
	{
		if(coords.row > allyRowLowerBounds ||
			coords.row < allyRowUpperBounds ||
			coords.col < colLeftBounds ||
			coords.col > colRightBounds)
		{
			return new List<Stats>();
		}

		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnAlliedSide(p) && Math.Abs(p.row - coords.row) + Math.Abs(p.col - coords.col) == 1));
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
		List<GridCoords> allEmptySpaces = new List<GridCoords>();
		
		for(int rowIndex = startRow; rowIndex <= endRow; rowIndex++)
		{
            for(int colIndex = 0; colIndex <= colRightBounds; colIndex++)
            {
                GridCoords coords = new GridCoords(rowIndex, colIndex);

                if(!combatantsDict.ContainsKey(coords))
                {
                    allEmptySpaces.Add(coords);
                }
            }
		}
		
		return allEmptySpaces.ToArray();
	}

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
	
	public static bool enemyHasMandatoryTarget(out Stats mandatoryTarget)
	{
        mandatoryTarget = getMandatoryTargetFromList(getAllAliveEnemyCombatants());

		return mandatoryTarget != null;
	}
	
	public static bool allyHasMandatoryTarget(out Stats mandatoryTarget)
	{
        mandatoryTarget = getMandatoryTargetFromList(getAllAliveAllyCombatants());

		return mandatoryTarget != null;
	}

	private static Stats getMandatoryTargetFromList(List<Stats> targets)
	{
		foreach(Stats target in targets)
		{
			if(Helpers.hasQuality<Trait>(target.traitContainer, t => t.isMandatoryTarget()))
			{
				return target;
			}
		}
		
		return null;
	}
	
	public static void deleteDeadOnDeathEffectActors()
	{
        List<Stats> combatants = getCombatantsOnGrid().Select(c => c.stats).Distinct().ToList();

		for (int index = 0; index < combatants.Count; index++)
		{
            Stats currentCombatant = combatants[index];

            if (currentCombatant != null && currentCombatant.isDead &&
                Helpers.hasQuality<Trait>(currentCombatant.traitContainer, t => t.deleteIfDead()))
            {

                removeCombatantFromGrid(currentCombatant);
            }
		}
	}
	
	private static List<Stats> scrubDuplicatesFromList(List<Stats> combatantList)
	{
		for(int combatantIndex = 0; combatantIndex < combatantList.Count; combatantIndex++)
		{
			Stats currentCombatant = combatantList[combatantIndex];

            // if(currentCombatant.multiSpaceEnemy)
            // {
            //     continue;
            // }
			
			for(int priorCombatantIndex = combatantIndex-1; priorCombatantIndex >= 0; priorCombatantIndex--)
			{
				Stats currentPriorCombatant = combatantList[priorCombatantIndex];

				if(currentCombatant.positions.Any(p => currentPriorCombatant.positions.Contains(p)))
				{
					combatantList.RemoveAt(combatantIndex);
					combatantIndex--;
					break;
				}
			}
		}
		
		return combatantList;
	}

    public static bool combatantIsRepositionClone(GridCoords targetCoords)
    {
        if(!tryGetCombatantAt(targetCoords, out Combatant combatant))
        {
            return false;
        }

        return combatant.stats.isRepositionClone;
    }
}