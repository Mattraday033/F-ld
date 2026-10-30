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

    public override bool isPartOfVolley => partOfVolley;

    #region Properties

	public override Color outlineColor => ColorList.canBeInteractedWith;

    public override int volleyAccuracy => PartyStats.getVolleyAccuracy();

    #endregion

	public AlliedSummonStats(EnemyStats enemyStats, bool partOfVolley = true): 
		base(enemyStats.characterName, enemyStats.totalArmorRating, enemyStats.totalHealth, enemyStats.combatAction)
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
        if(isFrontline)
        {
            return new List<GridCoords> { CreatureSpawner.getNextFreeAllyFrontLineSpace() };
        }

        if(isBackline)
        {
            return new List<GridCoords> { CreatureSpawner.getNextFreeAllyBackLineSpace() };
        }

        return new List<GridCoords> { CombatGrid.findRandomOpenSpaceInAllyZone() };
    }

    //tagged as an NPC by CombatantSpawnDetails, which is how allied summons are told apart from party members
    public override void spawningActions()
    {
        Dexterity.addExitStrategy(this);
    }
}
