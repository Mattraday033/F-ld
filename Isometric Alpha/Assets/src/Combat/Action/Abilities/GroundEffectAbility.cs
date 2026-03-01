using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEffectAbility : Ability
{
    private const string groundEffectUseDescriptionWarning = "\n\nThis Ability leaves a temperary, hostile Effect on the Tile it is targeting. Damage dealt by these Effects ignores the damage reduction provided by Armor and Invulnerability";

	public GroundEffect template;
	
	public GroundEffectAbility(CombatActionSettings settings, GroundEffect template):
		base(settings)
	{
		this.template = template;
	}

	public override void performCombatAction(List<Stats> targets)
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

    public override string getUseDescription()
	{
        return base.getUseDescription() + groundEffectUseDescriptionWarning;
	}
}
