using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEffectAbility : Ability
{
	public GroundEffect template;
	
	public GroundEffectAbility(CombatActionSettings settings, GroundEffect template):
		base(settings)
	{
		this.template = template;
	}

	public override void performCombatAction(ArrayList targets)
	{
		GridCoords[] allTargetCoords = getSelector().getAllSelectorCoords();

		int index = 0;
		foreach (GridCoords coords in allTargetCoords)
		{
			sendProjectileAtSpace(coords, index);

			GroundEffectManager.createNewGroundEffect(template, coords);

			index++;
		}
	}
}
