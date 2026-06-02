using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Formation : ICloneable, IDescribable, IDescribableInBlocks, IEnumerable, ICreatureSpawnPackage
{
    private const int rowCount = 4;
    private const int colCount = 4;
    private const bool isHealing = true;

    public readonly static UnityEvent OnFormationChange = new UnityEvent();

    private Dictionary<GridCoords, AllyStats> grid = new Dictionary<GridCoords, AllyStats>();

    public Formation()
    {
        initializeDefaultGrid();
    }

    public Formation(StatsWrapper[] wrappers)
    {
        foreach (StatsWrapper wrapper in wrappers)
        {
            GridCoords coords = wrapper.partyMemberFormationCoords;

            if (!coords.Equals(GridCoords.getDefaultCoords()))
            {
                grid[coords] = new AllyStats(wrapper);
            }
        }

        if(!grid.ContainsValue(PartyManager.getPlayerStats()))
        {
            grid[new GridCoords(AllyStats.defaultStartingRow, AllyStats.defaultStartingCol)] = PartyManager.getPlayerStats();
        }
    }

    private void initializeDefaultGrid()
    {
        grid = new Dictionary<GridCoords, AllyStats>();
        grid[new GridCoords(AllyStats.defaultStartingRow, AllyStats.defaultStartingCol)] = PartyManager.getPlayerStats();
    }

    public AllyStats[][] getGrid()
    {
        if (grid == null)
        {
            initializeDefaultGrid();
        }

        return toJaggedArray();
    }

    public void setCharacterAtCoords(GridCoords coords, AllyStats newStats)
    {
        setCharacterAtCoords(coords.row, coords.col, newStats);
    }

    public void setCharacterAtCoords(int row, int col, AllyStats newStats)
    {
        if (newStats == null ||
            row < 0 ||
            col < 0 ||
            row >= rowCount ||
            col >= colCount)
        {
            return;
        }

        GridCoords coords = new GridCoords(row, col);

        if(grid.ContainsKey(coords))
        {
            AllyStats existing = grid[coords];

            if (existing != null &&
            existing.getName().Contains(PartyManager.playerMarker))
            {
                return;
            }
        }

        grid[coords] = newStats;

        if(!LoadSaveFile.midLoad)
        {
            OnFormationChange.Invoke();
        }
    }

    public void removeCharacterAtCoords(int row, int col)
    {
        removeCharacterAtCoords(new GridCoords(row, col));
    }

    public void removeCharacterAtCoords(GridCoords coords)
    {
        if(grid.ContainsKey(coords))
        {
            AllyStats existing = grid[coords];

            if (existing != null &&
            existing.getName().Contains(PartyManager.playerMarker))
            {
                return;
            }

            grid.Remove(coords);

            OnFormationChange.Invoke();
        }
    }

    public bool isVacant()
    {
        return grid.Count <= 0;
    }

    public bool isInParty(Stats stats)
    {
        AllyStats allyStats = stats as AllyStats;

        if(allyStats == null)
        {
            return false;
        }

        return grid.ContainsValue(allyStats);
    }

    public AllyStats getStatsAtCoords(int row, int col)
    {
        return getStatsAtCoords(new GridCoords(row, col));
    }

    public AllyStats getStatsAtCoords(GridCoords coords)
    {
        if (coords.row < 0 || coords.col < 0)
        {
            return null;
        }

        if (grid.ContainsKey(coords))
        {
            return grid[coords];
        }
        else
        {
            return null;
        }
    }

    public static GridCoords findLocationOfStats(AllyStats partyMember)
    {
        if(State.formation != null &&
            State.formation.isInParty(partyMember))
        {
            return State.formation.grid.FirstOrDefault(x => x.Value.Equals(partyMember)).Key;
        } else if(partyMember.getName().Contains(PartyManager.playerMarker))
        {
            return new GridCoords(AllyStats.defaultStartingRow, AllyStats.defaultStartingCol);
        }

        return new GridCoords(-1, -1);
    }

    public void implementGridFromCoordSet(StatsWrapper[] statsWrappers)
    {
        grid = new Dictionary<GridCoords, AllyStats>();

        for (int partyMemberIndex = 0; partyMemberIndex < statsWrappers.Length; partyMemberIndex++)
        {
            GridCoords coords = statsWrappers[partyMemberIndex].partyMemberFormationCoords;

            if (coords.row < 0 || coords.col < 0)
            {
                continue;
            }

            setCharacterAtCoords(coords, PartyManager.getPartyMember(statsWrappers[partyMemberIndex].key).stats);
        }
    }

    public void removePartyMember(string partyMemberName)
    {
        grid.Remove(State.formation.grid.FirstOrDefault(x => x.Value.getName().Equals(partyMemberName)).Key);
    }

    public void removeCharacter(AllyStats characterToRemove)
    {
        if(grid.ContainsValue(characterToRemove))
        {
            grid.Remove(State.formation.grid.FirstOrDefault(x => x.Value.Equals(characterToRemove)).Key);
        }
    }

    public void removeAllPartyMembers()
    {
        List<GridCoords> coordsToClear = new List<GridCoords>();

        foreach (KeyValuePair<GridCoords, AllyStats> entry in grid)
        {
            if (entry.Value != null && entry.Value.removableFromFormation())
            {
                coordsToClear.Add(entry.Key);
            }
        }

        foreach (GridCoords coords in coordsToClear)
        {
            setCharacterAtCoords(coords, null);
        }

        PartyMemberTrainManager.createPartyMemberTrain();
    }

    public bool canWriteToSlotWithoutOverride(int row, int col)
    {
        return !isFull() ||
                getStatsAtCoords(row, col) != null;
    }

    public bool isFull()
    {
        return getSizeOfFormation() == PartyStats.getPartySizeMaximum();
    }

    public bool contains(string name)
    {
        foreach (AllyStats ally in this)
        {
            if(ally != null && ally.getName().Equals(name))
            {
                return true;
            }
        }

        return false;
    }

    public bool contains(AllyStats stats)
    {
        return grid.ContainsValue(stats);
    }

    public void addAllyInFirstOpenSpace(AllyStats allyToAdd)
    {
        for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
        {
            for (int colIndex = 0; colIndex < colCount; colIndex++)
            {
                GridCoords coords = new GridCoords(rowIndex, colIndex);

                if (!grid.ContainsKey(coords))
                {
                    grid[coords] = allyToAdd;
                    return;
                }
            }
        }
    }

    public int getSizeOfFormation()
    {
        int sizeOfFormation = 0;

        foreach (AllyStats stats in grid.Values)
        {
            if (stats != null)
            {
                sizeOfFormation++;
            }
        }

        return sizeOfFormation;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public Formation clone()
    {
        Formation clone = new Formation();

        Dictionary<GridCoords, AllyStats> newGrid = new Dictionary<GridCoords, AllyStats>();

        foreach (KeyValuePair<GridCoords, AllyStats> entry in grid)
        {
            newGrid[entry.Key] = entry.Value;
        }

        clone.grid = newGrid;

        return clone;
    }

    public string getName()
    {
        return PartyManager.getPlayerStats().getName() + "'s Formation";
    }

    public delegate int HighestDelegateInt<T>(T t);

    public int getHighestStat(HighestDelegateInt<AllyStats> getStat)
    {
        int highest = 0;

        foreach (AllyStats ally in grid.Values)
        {
            if (ally != null && getStat(ally) > highest)
            {
                highest = getStat(ally);
            }
        }

        return highest;
    }

    public int getTotalStrength()
    {
        return Helpers.sum<AllyStats>(grid.Values, t => t.getStrength());
    }

    public int getTotalDexterity()
    {
        return Helpers.sum<AllyStats>(grid.Values, t => t.getDexterity());
    }

    public int getTotalWisdom()
    {
        return Helpers.sum<AllyStats>(grid.Values, t => t.getWisdom());
    }

    public int getTotalCharisma()
    {
        return Helpers.sum<AllyStats>(grid.Values, t => t.getCharisma());
    }

    public int getTotalBonusVolleyAccuracy()
    {
        return Helpers.sum<AllyStats>(grid.Values, t => t.getBonusVolleyAccuracy());
    }

    public int getHighestLevel()
    {
        int highest = 0;

        foreach (AllyStats stats in this)
        {
            if (stats != null && stats.getLevel() > highest)
            {
                highest = stats.getLevel();
            }
        }

        return highest;
    }

    public List<CombatActionArray> getAllCombatActionArrays()
    {
        List<CombatActionArray> allCombatActionArrays = new List<CombatActionArray>();

        foreach (AllyStats stats in this)
        {
            if (stats != null)
            {
                allCombatActionArrays.Add(stats.getActionArray());
            }
        }

        return allCombatActionArrays;
    }

    public List<AllyStats> getAllPartyStatsInFormation()
    {
        List<AllyStats> stats = new List<AllyStats>();

        AllyStats player = PartyManager.getPlayerStats();

        stats.Add(player);

        foreach (AllyStats ally in State.formation)
        {
            if (ally != null && ally != player)
            {
                stats.Add(ally);
            }
        }

        return stats;
    }

    public void applyRegeneration()
    {
        foreach (AllyStats ally in State.formation)
        {
            if (ally == null)
            {
                continue;
            }

            ally.modifyCurrentHealth(Strength.getCurrentRegenerationAmount(ally), isHealing);
        }
    }

    //IDescribable Methods

    public bool ineligible()
    {
        return false;
    }

    public GameObject getRowType(RowType rowType)
    {
        return null;
    }

    public GameObject getDescriptionPanelFull()
    {
        return null;
    }

    public GameObject getDescriptionPanelFull(PanelType type)
    {
        return null;
    }

	public GameObject getDecisionPanel()
    {
        return null;
    }

	public bool withinFilter(string[] filterParameters)
    {
        return true;
    }

	public void describeSelfFull(DescriptionPanel panel)
    {
    }

	public void describeSelfRow(DescriptionPanel panel)
    {
    }

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
    {
    }

	public List<IDescribable> getRelatedDescribables()
    {
        return new List<IDescribable>();
    }

	public bool buildableWithBlocks()
    {
        return true;
    }
	public bool buildableWithBlocksRows()
    {
        return true;
    }

    //IDescribableInBlocks methods
    public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        List<DescriptionPanelBuildingBlock> blocks = new List<DescriptionPanelBuildingBlock>();

        blocks.Add(DescriptionPanelBuildingBlock.getStrengthBlock(getTotalStrength().ToString()));
        blocks.Add(DescriptionPanelBuildingBlock.getDexterityBlock(getTotalDexterity().ToString()));
        blocks.Add(DescriptionPanelBuildingBlock.getWisdomBlock(getTotalWisdom().ToString()));
        blocks.Add(DescriptionPanelBuildingBlock.getCharismaBlock(getTotalCharisma().ToString()));

        blocks.Add(DescriptionPanelBuildingBlock.getRedKnifeBlock(PartyStats.getStartingRedKnife().ToString()));
        blocks.Add(DescriptionPanelBuildingBlock.getBlueShieldBlock(PartyStats.getStartingBlueShield().ToString()));
        blocks.Add(DescriptionPanelBuildingBlock.getYellowThornBlock(PartyStats.getStartingYellowThorn().ToString()));
        blocks.Add(DescriptionPanelBuildingBlock.getGreenLeafBlock(PartyStats.getStartingGreenLeaf().ToString()));

        blocks.Add(DescriptionPanelBuildingBlock.getIntimidateBlock(PartyStats.getMaxIntimidateCount().ToString()));
        blocks.Add(DescriptionPanelBuildingBlock.getCunningBlock(PartyStats.getMaxCunningCount().ToString()));
        blocks.Add(DescriptionPanelBuildingBlock.getObservationBlock(PartyStats.getObservationLevel().ToString()));
        blocks.Add(DescriptionPanelBuildingBlock.getLeadershipBlock(PartyStats.getMaxPlacablePartyMembers().ToString()));

        blocks.Add(DescriptionPanelBuildingBlock.getRegenBlock(PartyStats.getPartyRegenAmountForDisplay()));

        blocks.Add(DescriptionPanelBuildingBlock.getVolleyBlock(PartyStats.getVolleyAccuracy() + "%"));

        blocks.Add(DescriptionPanelBuildingBlock.getPartyActionsBlock(PartyStats.getPartyMemberCombatActionSlots().ToString()));

        blocks.Add(DescriptionPanelBuildingBlock.getPartySlotsBlock(PartyStats.getPartySizeMaximum().ToString()));

        blocks.Add(DescriptionPanelBuildingBlock.getRetreatChanceBlock("+" + PartyStats.getRetreatChanceBonus() + "%"));

        blocks.Add(DescriptionPanelBuildingBlock.getSurpriseRoundAmountBlock(PartyStats.getPartySurpriseRounds().ToString()));

        blocks.Add(DescriptionPanelBuildingBlock.getDiscountBlock(PartyStats.getDiscountForDisplay()));

        blocks.Add(DescriptionPanelBuildingBlock.getGoldMultiplierBlock(PartyStats.getGoldMultiplierForDisplay().ToString()));

        return blocks;
    }

    public bool requiresInspectNode()
    {
        return false;
    }

    public bool hasCreaturesToSpawn()
    {
        return true;
    }
    public IEnumerator GetEnumerator()
    {
        foreach (AllyStats stats in grid.Values)
        {
            yield return stats;
        }
    }

    private AllyStats[][] toJaggedArray()
    {
        AllyStats[][] jaggedGrid = new AllyStats[rowCount][];

        for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
        {
            jaggedGrid[rowIndex] = new AllyStats[colCount];
        }

        foreach (KeyValuePair<GridCoords, AllyStats> entry in grid)
        {
            if (entry.Key.row >= 0 && entry.Key.row < rowCount &&
                entry.Key.col >= 0 && entry.Key.col < colCount)
            {
                jaggedGrid[entry.Key.row][entry.Key.col] = entry.Value;
            }
        }

        return jaggedGrid;
    }

}
