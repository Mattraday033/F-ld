using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SwapAbility : Ability
{
	private const bool healing = true;

    public override bool healsTarget => true;

    public SwapAbility(CombatActionSettings settings, List<TargetingLogic> targetingLogics = null) :
	base(settings, targetingLogics)
	{

	}

	public override void performCombatAction(List<Stats> targets)
	{
        if(actorStats.isMultiTile)
        {
            return;
        }

		GridCoords tempCoords = actorCoords.clone();

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

		actorStats.moveTo(target.positions.Select(p => p.clone()).ToList());
		actorStats.addTrait(appliedTrait);

		target.moveTo(new List<GridCoords> { tempCoords });
		target.addTrait(appliedTrait);

		sendProjectileAt(actorCoords, actorStats, 0, getDamageFormulaTotal(), false);
	}
}
