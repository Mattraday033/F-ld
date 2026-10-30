using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadAbility : Ability
{
	private string overridingDamageFormula;

	public SquadAbility(CombatActionSettings settings, string overridingDamageFormula, List<TargetingLogic> targetingLogics = null) :
	base(settings, targetingLogics)
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
		return CombatGrid.combatantExistsAtCoords(coords, out Stats target) && target.isAlive;
	}

	private bool checkTopCoords()
	{
		GridCoords topCoords = new GridCoords(actorCoords.row - 1, actorCoords.col);
		
		if(topCoords.row < CombatGrid.enemyRowUpperBounds || actorCoords.row == CombatGrid.allyRowUpperBounds)
		{
			return false;
		}
		
		return checkCoords(topCoords);
	}
	
	private bool checkBottomCoords()
	{
		GridCoords bottomCoords = new GridCoords(actorCoords.row + 1, actorCoords.col);
		
		if(bottomCoords.row > CombatGrid.allyRowLowerBounds || actorCoords.row == CombatGrid.enemyRowLowerBounds)
		{
			return false;
		}
		
		return checkCoords(bottomCoords);
	}
	
	private bool checkLeftCoords()
	{
		GridCoords leftCoords = new GridCoords(actorCoords.row, actorCoords.col - 1);
		
		if(actorCoords.col == CombatGrid.colLeftBounds)
		{
			return false;
		}
	
		return checkCoords(leftCoords);
	}
	
	private bool checkRightCoords()
	{
		GridCoords rightCoords = new GridCoords(actorCoords.row, actorCoords.col + 1);
		
		if(actorCoords.col == CombatGrid.colRightBounds)
		{
			return false;
		}
		
		return checkCoords(rightCoords);
	}
}
