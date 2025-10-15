using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DoubleStrikeAbility : Ability
{
    private int secondAttackRangeIndex;
    private GridCoords secondaryCoords;
    private GridCoords tertiaryCoords = GridCoords.getDefaultCoords();

    public DoubleStrikeAbility(CombatActionSettings settings, int secondAttackRangeIndex) : 
    base(settings)
    {
        this.secondAttackRangeIndex = secondAttackRangeIndex;
    }

    public override GridCoords getSecondaryCoords()
    {
        return secondaryCoords;
    }

    public override void setSecondaryCoords(GridCoords coords)
    {
        secondaryCoords = coords.clone();
    }

    public override Vector3 getTertiaryPosition()
    {
        if (tertiaryCoords.row < 0 || tertiaryCoords.col < 0)
        {
            Debug.LogError("tertiary Coords never set. Are you sure that this action has a tertiary yet?");
        }

        return CombatGrid.fullCombatGrid[tertiaryCoords.row][tertiaryCoords.col];
    }

    public override void setTertiaryCoords(GridCoords coords)
    {
        tertiaryCoords = coords.clone();
    }

    public override bool tertiaryCoordsRequiresEmptySpace()
    {
        return false;
    }

    public override bool requiresTertiaryCoords()
    {
        return true;
    }
	public override bool resetCoordsWhenChoosingTertiary() // if true, when the player chooses the first selector and moves to the tertiary, it will snap to it's default position
    {
        return true;
    }

	public override bool resetCoordsOnBackOutOfTertiary() // if true, when the player backs out of choosing the tertiary target, then the previous selector will snap to it's default position 
	{
		return true;
	}

    public override Selector getTertiarySelector()
    {
        Selector tertiarySelector = SelectorManager.getInstance().selectors[secondAttackRangeIndex];

        if (!tertiaryCoords.Equals(GridCoords.getDefaultCoords()))
        {
            tertiarySelector.setToLocation(tertiaryCoords);
        }
        else
        {
            tertiarySelector.setToStartLocation();
        }

        return tertiarySelector;
    }

    public override void performCombatAction()
    {
        if (getTargetCoords().Equals(GridCoords.getDefaultCoords()))
        {
            Debug.LogError("getTargetCoords() = " + getTargetCoords().ToString());
            return;
        }

        Debug.LogError("getSelector() coords = " + getSelector().getCoords().ToString());
        Debug.LogError("getTertiarySelector() coords = " + getTertiarySelector().getCoords().ToString());

        Selector firstSelector = getSelector();
        ArrayList firstListOfTargets = firstSelector.getAllTargets();

        Debug.LogError("firstListOfTargets.Count = " + firstListOfTargets.Count);

        Selector secondSelector = getTertiarySelector();
        ArrayList secondListOfTargets = secondSelector.getAllTargets();

        Debug.LogError("secondListOfTargets.Count = " + secondListOfTargets.Count);

        performCombatAction(firstListOfTargets);
        performCombatAction(secondListOfTargets);
    }

}
