using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

	public override void performCombatAction(List<Stats> targets)
	{
        if(getActorStats().isMultiTile())
        {
            return;
        }

		GridCoords tempCoords = getActorCoords().clone();

		Stats target = null;

        foreach(Stats stats in targets)
        {
            if(stats != null && stats.positions.Count == Constants.sizeOne)
            {
                target = stats;
                break;
            }
        }

		if (target == null)
		{
			return;
		}

		getActorStats().moveTo(target.positions.Select(p => p.clone()).ToList());
		getActorStats().addTrait(getAppliedTrait());

		target.moveTo(new List<GridCoords> { tempCoords });
		target.addTrait(getAppliedTrait());

		sendProjectileAt(getActorCoords(), getActorStats(), 0, getDamageFormulaTotal(), false);
	}
}
