using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class AlliedSummonStats : VolleyParticipantStats
{
	
	public AlliedSummonStats(EnemyStats enemyStats): 
		base(enemyStats.getName(), enemyStats.getTotalArmorRating(), enemyStats.getTotalHealth(), enemyStats.getCombatAction())
    {
        addTraits(enemyStats.traits);
        setFoeTypeToSummoned();
    }

    private void setFoeTypeToSummoned()
    {
        for(int index = 0; index < traits.Length; index++)
        {
            Trait trait = traits[index];

            if(trait != null && 
                trait.traitType == TraitType.FoeType)
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
	
	public override Color getOutlineColor()
	{
		return ColorList.canBeInteractedWith;
	}

    public override int getVolleyAccuracy()
    {
        return PartyStats.getVolleyAccuracy();
    }
}
