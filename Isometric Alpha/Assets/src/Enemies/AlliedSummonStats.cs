using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class AlliedSummonStats : VolleyParticipantStats
{
	
    private bool partOfVolley;

	public AlliedSummonStats(EnemyStats enemyStats, bool partOfVolley = true): 
		base(enemyStats.uniqueName, enemyStats.getTotalArmorRating(), enemyStats.getTotalHealth(), enemyStats.getCombatAction())
    {
        addTraits(enemyStats.traitContainer);
        setFoeTypeToSummoned();
        gendered = enemyStats.gendered;
        
        this.partOfVolley = partOfVolley;
    }

    private void setFoeTypeToSummoned()
    {
        traitContainer.removeAllTraitsOfType(TraitType.FoeType);

        traitContainer.addTrait(TraitList.summoned);
    }

    public override List<GridCoords> findLocationToSpawn()
    {
        if(isFrontline())
        {
            return new List<GridCoords> { CreatureSpawner.getNextFreeAllyFrontLineSpace() };
        }

        if(isBackline())
        {
            return new List<GridCoords> { CreatureSpawner.getNextFreeAllyBackLineSpace() };
        }

        return new List<GridCoords> { CombatGrid.findRandomOpenSpaceInAllyZone() };
    }
	
	public override Color getOutlineColor()
	{
		return ColorList.canBeInteractedWith;
	}

    public override int getVolleyAccuracy()
    {
        return PartyStats.getVolleyAccuracy();
    }

	public override bool isPartOfVolley()
	{
		return partOfVolley;
	}

    //tagged as an NPC by CombatantSpawnDetails, which is how allied summons are told apart from party members
    public override void spawningActions()
    {
        Dexterity.addExitStrategy(this);
    }
}
