using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RepositionPlaceholderGenerator
{
	public const float placeHolderSpriteOpaqueness = .4f;

	//a see-through copy of the combatant, built from its Stats the same way the combatant itself was
	public static GameObject generatePlaceholderObject(Stats combatantToBeMoved, GridCoords placeHolderPosition)
	{
		CombatantSpawnDetails spawnDetails = new CombatantSpawnDetails(combatantToBeMoved,
																		new List<GridCoords> { placeHolderPosition },
																		placeholder: true);

		List<Combatant> placeholders = spawnDetails.spawnCombatant();

		return placeholders.Count > 0 ? placeholders[0].gameObject : null;
	}

}
