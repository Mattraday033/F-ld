using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VolleyParticipantStats : EnemyStats
{
    public VolleyParticipantStats(string key, int armor, int tHP, CombatAction combatAction, Trait[] traits = null) :
    base(key, armor, tHP, combatAction, traits)
    {
        
    }

	public override bool isPartOfVolley()
	{
		return true;
	}
}