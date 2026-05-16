using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadAbility : Ability
{
	private string overridingDamageFormula;

	public SquadAbility(CombatActionSettings settings, string overridingDamageFormula) :
	base(settings)
	{
		this.overridingDamageFormula = overridingDamageFormula;
	}

    public override string getDamageFormula()
    {
        if(adjacentToAlly())
        {
            return overridingDamageFormula;
        } 

        return base.getDamageFormula();
    }

	private bool adjacentToAlly()
	{
		if(checkTopCoords() || checkBottomCoords() || checkLeftCoords() || checkRightCoords())
		{
			return true;
		}
		
		return false;
	}

	private bool checkCoords(GridCoords coords)
	{
		return CombatGrid.getCombatantAtCoords(coords) != null && !(CombatGrid.getCombatantAtCoords(coords) is null) && !CombatGrid.getCombatantAtCoords(coords).isDead();
	}

	private bool checkTopCoords()
	{
		GridCoords topCoords = new GridCoords(getActorCoords().row - 1, getActorCoords().col);
		
		if(topCoords.row < CombatGrid.enemyRowUpperBounds || getActorCoords().row == CombatGrid.allyRowUpperBounds)
		{
			return false;
		}
		
		return checkCoords(topCoords);
	}
	
	private bool checkBottomCoords()
	{
		GridCoords bottomCoords = new GridCoords(getActorCoords().row + 1, getActorCoords().col);
		
		if(bottomCoords.row > CombatGrid.allyRowLowerBounds || getActorCoords().row == CombatGrid.enemyRowLowerBounds)
		{
			return false;
		}
		
		return checkCoords(bottomCoords);
	}
	
	private bool checkLeftCoords()
	{
		GridCoords leftCoords = new GridCoords(getActorCoords().row, getActorCoords().col - 1);
		
		if(getActorCoords().col == CombatGrid.colLeftBounds)
		{
			return false;
		}
	
		return checkCoords(leftCoords);
	}
	
	private bool checkRightCoords()
	{
		GridCoords rightCoords = new GridCoords(getActorCoords().row, getActorCoords().col + 1);
		
		if(getActorCoords().col == CombatGrid.colRightBounds)
		{
			return false;
		}
		
		return checkCoords(rightCoords);
	}
}
