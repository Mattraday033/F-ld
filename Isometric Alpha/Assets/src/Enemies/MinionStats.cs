using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionStats : VolleyParticipantStats
{
    public MinionStats(string key, int armor, int tHP, CombatAction combatAction, Trait[] traits) :
    base(key, armor, tHP, combatAction, traits)
    {
        List<Trait> newTraits = new List<Trait>();

        newTraits.Add(TraitList.minion);

        foreach(Trait trait in this.traits)
        {
            newTraits.Add(trait.clone());
        }

        this.traits = newTraits.ToArray();
    }




}