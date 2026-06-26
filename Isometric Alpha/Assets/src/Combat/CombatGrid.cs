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

        return new Vector3(position.x, position.y, -1*(row+col));
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

	public static void updateStatsSpritePosition(GridCoords newCoords)
	{
		Stats combatant = getCombatantAtCoords(newCoords);
		
		if(combatant == null || combatant.combatSprite == null)
		{
			return;
		}
		
		combatant.combatSprite.transform.position = getPositionAt(newCoords.row, newCoords.col);
		// Helpers.updateGameObjectPosition(combatant.combatSprite);
	}
	
	public static void setCombatantAtCoords(int rowIndex, int colIndex, Stats newCombatant)
	{
		setCombatantAtCoords(new GridCoords(rowIndex, colIndex), newCombatant);
	}
	
	public static void setCombatantAtCoords(GridCoords coords, Stats newCombatant)
	{
        if(newCombatant == null && combatantsDict.ContainsKey(coords))
        {
            if(combatantsDict[coords].getName().Contains(PartyManager.playerMarker))
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
		
        if(combatantsDict.ContainsKey(coords))
        {
            return combatantsDict[coords];
        } else
        {
            return null;
        }
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
		Stats ally = getCombatantAtCoords(coords);
		
        return ally != null && ally.isAlive();
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
        Stats stats = getCombatantAtCoords(targetCoords);

        if(stats == null)
        {
            return false;
        }

        return stats.isRepositionClone();
    }
}