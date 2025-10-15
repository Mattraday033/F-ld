using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing{
	Random = 0, NorthEast = 1, NorthWest = 2, SouthWest = 3, SouthEast = 4
}

public class CharacterFacing
{
	private Facing currentFacing;

	public CharacterFacing()
	{
		setFacing(Facing.Random);
	}

	public void setFacing(Facing direction)
    {
		if(direction == Facing.Random)
		{
			randomizeFacing();
        } else
		{
			currentFacing = direction;
		}
    }

	public void setToOpposingFacing()
	{
		currentFacing = getOpposingFacing(currentFacing);
	}

	public Facing getFacing()
	{
		return currentFacing;
	}

    public override string ToString()
    {
        return getFacing().ToString();
    }

    public int getFacingInt()
	{
		return (int) currentFacing;
	}
	
	
	private void randomizeFacing()
	{
		setFacing((Facing) getRandomFacing());
	}
	
	public static bool withinRange(int possibleFacing)
	{
		if(possibleFacing < (int) Facing.NorthEast || possibleFacing > (int) Facing.SouthEast)
		{
			return false;
		} else
		{
			return true;
		}
	}

    public static bool withinRange(Facing possibleFacing)
    {
        if (possibleFacing < Facing.NorthEast || possibleFacing > Facing.SouthEast)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static Facing getRandomFacing()
	{
		return (Facing) UnityEngine.Random.Range((int) Facing.NorthEast, (int) Facing.SouthEast+1);
	}
	
	public static Facing getOpposingFacing(int facing)
	{
		return getOpposingFacing((Facing) facing);
	}
	
	public static Facing getOpposingFacing(Facing facing)
	{
		if(facing.Equals(Facing.NorthEast))
		{
			return Facing.SouthWest;
		} else if(facing.Equals(Facing.NorthWest))
		{
			return Facing.SouthEast;
		} else if(facing.Equals(Facing.SouthWest))
		{
			return Facing.NorthEast;
		} else if(facing.Equals(Facing.SouthEast))
		{
			return Facing.NorthWest;
		} else
		{
			throw new IOException("Unknown facing: " + facing);
		}
	}

}
