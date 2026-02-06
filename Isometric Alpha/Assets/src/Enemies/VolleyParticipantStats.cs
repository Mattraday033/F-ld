using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class VolleyParticipantStats : EnemyStats
{
    public VolleyParticipantStats(string key, int armor, int tHP, CombatAction combatAction, Trait[] traits = null, bool gendered = false, string[] animationSuffixes = null) :
    base(key, armor, tHP, combatAction, traits, gendered: gendered, animationSuffixes: animationSuffixes)
    {
        
    }

	public override bool isPartOfVolley()
	{
		return true;
	}
}