using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionSelfAbility : RepositionAbility
{
	public RepositionSelfAbility(CombatActionSettings settings) :
		base(settings)
	{

	}

	public override Stats getCombatantToBeMoved()
	{
		return CombatGrid.getCombatantAtCoords(getActorCoords());
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
