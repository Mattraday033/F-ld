using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class AlliedSummonStats : EnemyStats
{

	public AlliedSummonStats(string name, int armor, int totalHitPoints): 
		base(name, armor, totalHitPoints)
    {
    }
	
	public AlliedSummonStats(EnemyStats enemyStats): 
		base(enemyStats.getName(), enemyStats.getTotalArmorRating(), enemyStats.getTotalHealth())
    {
        addTraits(enemyStats.traits);
        setCreatureTypeToSummoned();
    }

    private void setCreatureTypeToSummoned()
    {
        for(int index = 0; index < traits.Length; index++)
        {
            Trait trait = traits[index];

            if(trait != null && 
                trait.getType().Equals(TraitList.creatureTypeTraitType))
            {
                traits[index] = TraitList.summoned;
                return;
            }
        }
    }

    public override GridCoords findLocationToSpawn()
    {
        if(isFrontline())
        {
            return CreatureSpawner.getNextFreeAllyFrontLineSpace();
        }

        if(isBackline())
        {
            return CreatureSpawner.getNextFreeAllyBackLineSpace();
        }

        return CombatGrid.findRandomOpenSpaceInAllyZone();
    }

	public override bool isPartOfVolley()
	{
		return true;
	}
	
	public override Color getOutlineColor()
	{
		return ColorList.canBeInteractedWith;
	}
}
