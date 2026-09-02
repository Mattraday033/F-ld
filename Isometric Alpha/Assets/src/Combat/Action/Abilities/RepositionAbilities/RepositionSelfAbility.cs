using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionSelfAbility : RepositionAbility
{
	public RepositionSelfAbility(CombatActionSettings settings) :
		base(settings)
	{

	}

	public override bool combatantToBeMovedExists(out Stats combatant)
	{
		return CombatGrid.combatantExistsAtCoords(getActorCoords(), out combatant);
	}

	public override GridCoords getDestinationCoords()
	{
		return getTargetCoords();
	}

	public override bool targetsAllySection()
	{
		return true;
	}

	public override bool repositionsCaster()
	{
		return true;
	}

	public override bool targetsOnlyEmptySpace()
	{
		return true;
	}

	public override bool requiresTertiaryCoords()
	{
		return false;
	}
}
