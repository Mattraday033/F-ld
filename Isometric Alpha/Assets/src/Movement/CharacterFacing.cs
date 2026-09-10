using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Facing{
	Random = 0, NorthEast = 1, NorthWest = 2, SouthWest = 3, SouthEast = 4
}

public class CharacterFacing
{
	private Facing _CurrentFacing;
    public Facing currentFacing
    {
        set
        {
            if(value == Facing.Random)
            {
                _CurrentFacing = getRandomFacing();
            } else
            {
                _CurrentFacing = value;
            }
        }
        get
        {
            return _CurrentFacing;
        }
    }

	public CharacterFacing()
	{
		currentFacing = Facing.Random;
	}

	public Facing getOpposingFacing()
	{
		return currentFacing.getOpposingFacing();
	}

	public void setToOpposingFacing()
	{
		currentFacing = currentFacing.getOpposingFacing();
	}

	public Facing getFacing()
	{
		return currentFacing;
	}

    public bool facingNorth()
    {
        return currentFacing == Facing.NorthEast || 
            currentFacing == Facing.NorthWest;
    }

    public bool facingSouth()
    {
        return currentFacing == Facing.SouthEast || 
            currentFacing == Facing.SouthWest;
    }

    public static Facing getRandomFacing()
	{
		return (Facing) new System.Random().Next((int) Facing.NorthEast, (int) Facing.SouthEast+1);
	}
	
}
