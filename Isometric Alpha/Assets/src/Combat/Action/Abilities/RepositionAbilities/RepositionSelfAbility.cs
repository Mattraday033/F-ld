using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionSelfAbility : RepositionAbility
{
	public override bool targetsAllySection => true;
	public override bool repositionsCaster => true;
	public override bool targetsOnlyEmptySpace => true;
	public override bool requiresTertiaryCoords => false;

	public RepositionSelfAbility(CombatActionSettings settings) :
		base(settings)
	{

	}

	public override bool combatantToBeMovedExists(out Stats combatant)
	{
		return CombatGrid.combatantExistsAtCoords(actorCoords, out combatant);
	}

	public override GridCoords getDestinationCoords()
	{
		return getTargetCoords();
	}
}
