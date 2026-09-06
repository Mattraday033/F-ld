using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


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
	
    public static Dictionary<GridCoords,Stats> combatantsDict = new Dictionary<GridCoords,Stats>();
    public readonly static UnityEvent LastEnemyKilled = new UnityEvent();
    
    [RuntimeInitializeOnLoadMethod]
    public static void cleanCombatGrid()
    {
        combatantsDict = new Dictionary<GridCoords,Stats>();
    }

	public static Vector3 getPositionAt(GridCoords coords)
    {
        return getPositionAt(coords.row, coords.col);
	}

	public static Vector3 getEffectPositionAt(GridCoords coords)
    {
        return getPositionAt(coords.row, coords.col) + new Vector3(0f, 0.4f);
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

	public static void setCombatantAtCoords(int rowIndex, int colIndex, Stats newCombatant)
	{
		setCombatantAtCoords(new GridCoords(rowIndex, colIndex), newCombatant);
	}
	
	public static void setCombatantAtCoords(GridCoords coords, Stats newCombatant)
	{
        if(newCombatant == null && combatantsDict.ContainsKey(coords))
        {
            if(combatantsDict[coords] != null && combatantsDict[coords].getName().Contains(PartyManager.playerMarker))
            {
                return;
            }

            combatantsDict.Remove(coords);

            if(getTotalAliveEnemyCount() == 0)
            {
                LastEnemyKilled.Invoke();
            }
        } else if(newCombatant == null)
        {
            return;
        }

        combatantsDict[coords] = newCombatant;

        removeNullCombatants();
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
        List<Stats> stats = new List<Stats>();

        if(ally)
        {
            stats = new List<Stats>(combatantsDict.Values.Where(s => s.positions.Count > 0 && positionIsOnAlliedSide(s.positions[0])));
        } else
        {
            stats = new List<Stats>(combatantsDict.Values.Where(s => s.positions.Count > 0 && positionIsOnEnemySide(s.positions[0])));
        }

        stats = stats.Distinct().ToList();

        if(stats.Count <= 1)
        {
            return currentCoords;
        } else if(!combatantExistsAtCoords(currentCoords) ||
                (ally && positionIsOnEnemySide(SelectorManager.currentSelector.getCoords())) ||
                (!ally && positionIsOnAlliedSide(SelectorManager.currentSelector.getCoords())))
        {
            return stats[0].positions[0];
        }

        Stats targetStats = null;

        int i = 0;
        foreach(Stats combatant in stats)
        {
            if(!combatantExistsAtCoords(currentCoords, out Stats target) && 
                target.Equals(combatant))
            {
                i++;
                continue;
            }

            if(forward)
            {
                if(i < stats.Count-1)
                {
                    targetStats = stats[i+1];
                    break;
                } else
                {
                    targetStats = stats[0];
                    break;
                }
            } else
            {
                if(i > 0)
                {
                    targetStats = stats[i-1];
                    break;
                } else
                {
                    targetStats = stats[stats.Count-1];
                    break;
                }
            }
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

    public static void addCombatantToGrid(Stats combatant)
    {
        foreach(GridCoords coords in combatant.positions)
        {
            setCombatantAtCoords(coords, combatant);
        }
    }

    public static void removeCombatantFromGrid(Stats combatant)
    {
        foreach(GridCoords coords in combatant.positions)
        {
            setCombatantAtCoords(coords, null);
        }
    }
	
	public static bool combatantExistsAtCoords(int rowIndex, int colIndex)
	{
		return combatantExistsAtCoords(new GridCoords(rowIndex,colIndex));
	}
	
	public static bool combatantExistsAtCoords(int rowIndex, int colIndex, out Stats combatant)
	{
		return combatantExistsAtCoords(new GridCoords(rowIndex,colIndex), out combatant);
	}

	public static bool combatantExistsAtCoords(GridCoords coords, out Stats combatant)
	{
		if(coords.row < 0 || coords.col < 0 || 
            !combatantsDict.ContainsKey(coords))
		{
            combatant = null;
		} else
        {
            combatant = combatantsDict[coords];
        } 
    
        return combatant != null;
    }

	public static bool combatantExistsAtCoords(GridCoords coords)
	{
        return combatantExistsAtCoords(coords, out Stats combatant);
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
            foreach(Trait trait in enemy.traitContainer)
            {
                if(trait.getName().Equals(typeTrait.getName()))
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

        foreach(Stats combatant in combatantsDict.Values)
        {
            if(combatant != null && meetsCriteria(combatant))
            {
                matches.Add(combatant);
            }
        }
		
		return scrubDuplicatesFromList(matches);
    }

	public static List<Stats> getAllAliveSummonedEnemies()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnEnemySide(p)) && c.isAlive() && c.isTargetable() && c.isPartOfVolley());
	}

	public static List<Stats> getAllAliveNonsummonedEnemies()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnEnemySide(p)) && c.isAlive() && c.isTargetable() &&  !c.isPartOfVolley());
	}

	public static List<Stats> getAllAliveSummonedAllies()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnAlliedSide(p)) && c.isAlive() && c.isTargetable() && c.isSummon());
	}

	public static List<Stats> getAllAliveNonsummonedAllies()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnAlliedSide(p)) && c.isAlive() && c.isTargetable() && !c.isSummon());
	}

	public static List<Stats> getAllAliveAllyCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnAlliedSide(p)) && c.isAlive() && c.isTargetable());
	}

	public static List<Stats> getAllAliveEnemyCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnEnemySide(p)) && c.isAlive() && c.isTargetable());
	}

	public static List<Stats> getAllAliveCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.isAlive() && c.isTargetable());
	}

	public static List<Stats> getAllAllyCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnAlliedSide(p)) && c.isTargetable());
	}

	public static bool selectableAllyAtLocation(GridCoords coords)
	{
        return combatantExistsAtCoords(coords, out Stats ally) && ally.isAlive();
	}

	public static List<Stats> getAllNonsummonedAllyCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnAlliedSide(p)) && c.isTargetable() && !c.isSummon());
	}

	public static List<Stats> getAllEnemyCombatants()
	{
		return getAllCombatantsThatMeetCriteria(c => c.positions.Any(p => positionIsOnEnemySide(p)) && c.isTargetable());
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
		return getAllCombatantsThatMeetCriteria(c => c.isTargetable());
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
			if(Helpers.hasQuality<Trait>(target.traitContainer, t => t.isMandatoryTarget()))
			{
				return target;
			}
		}
		
		return null;
	}
	
	public static void deleteDeadOnDeathEffectActors()
	{
        List<Stats> combatants = new List<Stats>(combatantsDict.Values);

		for (int index = 0; index < combatants.Count; index++)
		{
            Stats currentCombatant = combatants[index];

            if (currentCombatant != null && currentCombatant.isDead() &&
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

            // if(currentCombatant.multiSpaceEnemy())
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
        if(!combatantExistsAtCoords(targetCoords, out Stats stats))
        {
            return false;
        }

        return stats.isRepositionClone();
    }
}