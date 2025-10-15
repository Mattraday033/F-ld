using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Formation : ICloneable, IDescribable, IDescribableInBlocks, IEnumerable
{
    private const int rowCount = 4;
    private const int colCount = 4;
    private const bool isHealing = true;

    public static UnityEvent OnFormationChange = new UnityEvent();

    private AllyStats[][] grid;

    public Formation()
    {
        setGrid(getDefaultGrid());
    }

    public Formation(StatsWrapper[] wrappers)
    {
        setGrid(getDefaultGrid());

        foreach (StatsWrapper wrapper in wrappers)
        {
            GridCoords coords = wrapper.partyMemberFormationCoords;

            if (!coords.Equals(GridCoords.getDefaultCoords()))
            {
                grid[coords.row][coords.col] = new AllyStats(wrapper);
            }
        }
    }

    public AllyStats[][] getGrid()
    {
        if (grid == null)
        {
            setGrid(getDefaultGrid());
        }

        return grid;
    }

    public void setGrid(AllyStats[][] newGrid)
    {
        grid = newGrid;
        PartyMemberMovement.instantiatePartyMemberTrain();
    }

    public void setCharacterAtCoords(AllyStats newStats, GridCoords coords)
    {
        setCharacterAtCoords(coords.row, coords.col, newStats);
    }

    public void setCharacterAtCoords(GridCoords coords, AllyStats newStats)
    {
        setCharacterAtCoords(coords.row, coords.col, newStats);
    }


    public void setCharacterAtCoords(AllyStats newStats, int row, int col)
    {
        setCharacterAtCoords(row, col, newStats);
    }

    public void setCharacterAtCoords(int row, int col, AllyStats newStats)
    {
        if (row < 0 || col < 0 || row >= grid.Length || col >= grid[row].Length)
        {
            return;
        }

        grid[row][col] = newStats;

        OnFormationChange.Invoke();
    }

    public Stats getStatsAtCoords(int row, int col)
    {
        return grid[row][col];
    }

    public Stats getStatsAtCoords(GridCoords coords)
    {
        return grid[coords.row][coords.col];
    }

    public GridCoords findLocationOfStats(Stats partyMember)
    {
        GridCoords partyMemberLocation = new GridCoords(-1, -1);

        for (int row = 0; row < State.formation.getGrid().Length; row++)
        {
            for (int col = 0; col < State.formation.getGrid()[row].Length; col++)
            {
                partyMemberLocation = new GridCoords(row, col);
                Stats statsAtLocation = State.formation.getStatsAtCoords(partyMemberLocation);

                if (statsAtLocation != null && partyMember != null && statsAtLocation.getName().Equals(partyMember.getName()))
                {
                    return partyMemberLocation;
                }
            }
        }

        return new GridCoords(-1, -1);
    }

    public void implementGridFromCoordSet(StatsWrapper[] statsWrappers)
    {
        grid = Formation.getEmptyGrid();

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
        for (int rowIndex = 0; rowIndex < grid.Length; rowIndex++)
        {
            for (int colIndex = 0; colIndex < grid[rowIndex].Length; colIndex++)
            {
                if (grid[rowIndex][colIndex] != null && grid[rowIndex][colIndex] != PartyManager.getPlayerStats() && grid[rowIndex][colIndex].getName().Equals(partyMemberName))
                {
                    setCharacterAtCoords(rowIndex, colIndex, null);
                }
            }
        }

        PartyMemberMovement.instantiatePartyMemberTrain();
    }

    public void removeCharacter(Stats characterToRemove)
    {
        for (int rowIndex = 0; rowIndex < getGrid().Length; rowIndex++)
        {
            for (int colIndex = 0; colIndex < getGrid()[rowIndex].Length; colIndex++)
            {
                if (grid[rowIndex][colIndex] == characterToRemove)
                {
                    setCharacterAtCoords(rowIndex, colIndex, null);
                    return;
                }
            }
        }
    }

    public void removeAllPartyMembers()
    {
        for (int rowIndex = 0; rowIndex < grid.Length; rowIndex++)
        {
            for (int colIndex = 0; colIndex < grid[rowIndex].Length; colIndex++)
            {
                if (grid[rowIndex][colIndex] != null && grid[rowIndex][colIndex].removableFromFormation())
                {
                    setCharacterAtCoords(rowIndex, colIndex, null);
                }
            }
        }

        PartyMemberMovement.instantiatePartyMemberTrain();
    }

    public static AllyStats[][] getEmptyGrid()
    {
        AllyStats[][] emptyGrid = new AllyStats[rowCount][];

        for (int rowIndex = 0; rowIndex < emptyGrid.Length; rowIndex++)
        {
            emptyGrid[rowIndex] = new AllyStats[colCount];
        }

        return emptyGrid;
    }

    public bool canWriteToSlot(int row, int col)
    {
        return !isFull() ||
                getGrid()[row][col] != null;
    }

    public bool isFull()
    {
        return getSizeOfFormation() == PartyStats.getPartySizeMaximum();
    }

    public bool contains(string name)
    {
        foreach (Stats ally in this)
        {
            if(ally != null && ally.getName().Equals(name))
            {
                return true;
            }
        }

        return false;
    }

    public bool contains(Stats stats)
    {
        foreach (Stats ally in this)
        {
            if(ally == stats)
            {
                return true;
            }
        }

        return false;
    }

    public static AllyStats[][] getDefaultGrid()
    {
        AllyStats[][] defaultGrid = new AllyStats[rowCount][];

        for (int rowIndex = 0; rowIndex < defaultGrid.Length; rowIndex++)
        {
            defaultGrid[rowIndex] = new AllyStats[colCount];
        }

        defaultGrid[AllyStats.defaultStartingRow][AllyStats.defaultStartingCol] = PartyManager.getPlayerStats();

        return defaultGrid;
    }

    public void addAllyInFirstOpenSpace(AllyStats allyToAdd)
    {
        for (int rowIndex = 0; rowIndex < grid.Length; rowIndex++)
        {
            for (int colIndex = 0; colIndex < grid[rowIndex].Length; colIndex++)
            {
                if (grid[rowIndex][colIndex] == null)
                {
                    grid[rowIndex][colIndex] = allyToAdd;
                    return;
                }
            }
        }
    }

    public int getSizeOfFormation()
    {
        int sizeOfFormation = 0;

        for (int rowIndex = 0; rowIndex < grid.Length; rowIndex++)
        {
            for (int colIndex = 0; colIndex < grid[rowIndex].Length; colIndex++)
            {
                if (grid[rowIndex][colIndex] != null)
                {
                    sizeOfFormation++;
                }
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

        AllyStats[][] newGrid = new AllyStats[rowCount][];

        for (int row = 0; row < rowCount; row++)
        {
            newGrid[row] = new AllyStats[4];
        }

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                newGrid[row][col] = grid[row][col];
            }
        }

        clone.setGrid(newGrid);

        return clone;
    }

    public string getName()
    {
        return PartyManager.getPlayerStats().getName() + "'s Formation";
    }

    public int getTotalStrength()
    {
        int total = 0;

        foreach (Stats[] row in grid)
        {
            total += Helpers.sum<Stats>(row, t => t.getStrength());
        }

        return total;
    }

    public int getTotalDexterity()
    {
        int total = 0;

        foreach (Stats[] row in grid)
        {
            total += Helpers.sum<Stats>(row, t => t.getDexterity());
        }

        return total;
    }

    public int getTotalWisdom()
    {
        int total = 0;

        foreach (Stats[] row in grid)
        {
            total += Helpers.sum<Stats>(row, t => t.getWisdom());
        }

        return total;
    }

    public int getTotalCharisma()
    {
        int total = 0;

        foreach (Stats[] row in grid)
        {
            total += Helpers.sum<Stats>(row, t => t.getCharisma());
        }

        return total;
    }

    public int getHighestLevel()
    {
        int highest = 0;

        foreach (Stats stats in this)
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

    public ArrayList getAllPartyStatsInFormation()
    {
        ArrayList stats = new ArrayList();

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

	public ArrayList getRelatedDescribables()
    {
        return new ArrayList();
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

        blocks.Add(DescriptionPanelBuildingBlock.getVolleyBlock("+" + PartyStats.getVolleyAccuracy() + "%"));

        blocks.Add(DescriptionPanelBuildingBlock.getPartyActionsBlock(PartyStats.getPartyMemberCombatActionSlots().ToString()));

        blocks.Add(DescriptionPanelBuildingBlock.getPartySlotsBlock(PartyStats.getPartySizeMaximum().ToString()));

        blocks.Add(DescriptionPanelBuildingBlock.getRetreatChanceBlock("+" + PartyStats.getRetreatChanceBonus() + "%"));

        blocks.Add(DescriptionPanelBuildingBlock.getSurpriseRoundAmountBlock(PartyStats.getPartySurpriseRounds().ToString()));

        blocks.Add(DescriptionPanelBuildingBlock.getDiscountBlock(PartyStats.getDiscountForDisplay()));

        blocks.Add(DescriptionPanelBuildingBlock.getGoldMultiplierBlock(PartyStats.getGoldMultiplierForDisplay().ToString()));

        return blocks;
    }

    public IEnumerator GetEnumerator()
    {
        foreach (AllyStats[] row in grid)
        {
            foreach (AllyStats stats in row)
            {
                yield return stats;
            }
        }
    }

}
