using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SwapAbility : Ability
{
	private const bool healing = true;

    public SwapAbility(CombatActionSettings settings) :
	base(settings)
	{

	}

	public override bool healsTarget()
	{
		return true;
	}

	public override void performCombatAction(ArrayList targets)
	{
		GridCoords tempCoords = getActorCoords().clone();

		Stats target = CombatGrid.getCombatantAtCoords(getTargetCoords().clone());

		if (target == null)
		{
			return;
		}

		getActorStats().moveTo(target.position);
		getActorStats().addTrait(getAppliedTrait());

		target.moveTo(tempCoords);
		target.addTrait(getAppliedTrait());

		sendProjectileAt(getActorCoords(), getActorStats(), 0, getDamageFormulaTotal(), false);
	}
}
