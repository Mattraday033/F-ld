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
        addTraits(enemyStats.traitContainer);
        setFoeTypeToSummoned();
        animationSuffixes = enemyStats.animationSuffixes;
        gendered = enemyStats.gendered;
    }

    private void setFoeTypeToSummoned()
    {
        traitContainer.removeAllTraitsOfType(TraitType.FoeType);

        traitContainer.addTrait(TraitList.summoned);
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
