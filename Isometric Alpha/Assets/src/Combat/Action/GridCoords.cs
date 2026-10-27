using System;
using UnityEngine;

[System.Serializable]
public struct GridCoords
{
	public readonly static GridCoords incrementNorth = new GridCoords(-1, 0);
	public readonly static GridCoords incrementNorthEast = new GridCoords(-1, 1);
	public readonly static GridCoords incrementEast = new GridCoords(0, 1);
	public readonly static GridCoords incrementSouthEast = new GridCoords(1, 1);
	public readonly static GridCoords incrementSouth = new GridCoords(1, 0);
	public readonly static GridCoords incrementSouthWest = new GridCoords(1, -1);
	public readonly static GridCoords incrementWest = new GridCoords(0, -1);
	public readonly static GridCoords incrementNorthWest = new GridCoords(-1, -1);

	public readonly static GridCoords[] increments = new GridCoords[]{incrementNorth, incrementNorthEast, incrementEast, incrementSouthEast,
																incrementSouth, incrementSouthWest, incrementWest, incrementNorthWest};

	public int row;
	public int col;

	public GridCoords(float r, float c)
	{
		row = (int) r;
		col = (int) c;
	}

	public GridCoords(int r, int c)
	{
		row = r;
		col = c;
	}

    public int sum()
    {
        return row + col;
    }

    public static GridCoords sum(GridCoords coords1, GridCoords coords2)
    {
        return new GridCoords(coords1.row + coords2.row, coords1.col + coords2.col);
    }

	public override string ToString()
	{
		return "(Row: " + row + ", Col: " + col + ")";
	}

	public Vector3 toVector3()
	{
        return CombatGrid.getPositionAt(row, col);
	}

	public Vector3Int toVector3Int()
	{
		return new Vector3Int(col, row, 0);
	}

	//the inverse of toVector3Int, which lets combat cells travel through code that expects Vector3Int cells
	public static GridCoords fromVector3Int(Vector3Int cell)
	{
		return new GridCoords(cell.y, cell.x);
	}

	public override bool Equals(object obj)
	{
        GridCoords coords = getDefaultCoords();

        try
        {
            coords = (GridCoords) obj;
        } catch
        {   
            return false;
        }

		return (coords.row == row) && (coords.col == col);
	}

    public override int GetHashCode()
    {
        return (row.ToString() + "," + col.ToString()).GetHashCode();
    }

	public bool isWithinAllySection()
	{
		if (col < CombatGrid.colLeftBounds || col > CombatGrid.colRightBounds)
		{
			return false;
		}

		if (row > CombatGrid.allyRowLowerBounds || row < CombatGrid.allyRowUpperBounds)
		{
			return false;
		}

		return true;
	}

	public bool isWithinEnemySection()
	{
		if (col < CombatGrid.colLeftBounds || col > CombatGrid.colRightBounds)
		{
			return false;
		}

		if (row > CombatGrid.enemyRowLowerBounds || row < CombatGrid.enemyRowUpperBounds)
		{
			return false;
		}

		return true;
	}

	public GridCoords clone()
	{
		return new GridCoords(row, col);
	}

	public static GridCoords getDefaultCoords()
	{
		return new GridCoords(-1, -1);
	}

	public int distanceTo(GridCoords other)
	{
		return Math.Abs(row - other.row) + Math.Abs(col - other.col);
	}
}
