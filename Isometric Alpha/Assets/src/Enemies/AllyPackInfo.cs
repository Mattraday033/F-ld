using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyPackInfo : EnemyPackInfo
{
    protected string[] flagsToCheckForAllies;

    public AllyPackInfo(CreatureAmount creatureType, string flagToCheckForAllies):
    base(new CreatureAmount[]{ creatureType }, null)
    {
        this.flagsToCheckForAllies = new string[]{ flagToCheckForAllies };
    }

    public AllyPackInfo(CreatureAmount[] creatureTypes, string[] flagsToCheckForAllies):
    base(creatureTypes, null)
    {
        this.flagsToCheckForAllies = flagsToCheckForAllies;
    }

    public override IEnumerator GetEnumerator()
    {
        List<CreatureAmount> relevantAllies = new List<CreatureAmount>();

        for(int index = 0; index < creatureTypes.Length; index++)
        {
            if (Flags.getFlag(getFlagAtIndex(index)))
            {
                relevantAllies.Add(creatureTypes[index]);
            }
        }

        List<Stats> allStatsInPack = new List<Stats>();

        //CreatureAmount[] creatureTypes
        foreach (CreatureAmount amount in creatureTypes)
        {
            for(int index = 0; index < amount.amount; index++)
            {
                allStatsInPack.Add(amount.enemyStats);
            }
        }

        foreach (Stats stats in allStatsInPack)
        {
            if(stats == null)
            {
                continue;
            }

            yield return stats.clone();
        }
    }

    private string getFlagAtIndex(int index)
    {
        if(index >= flagsToCheckForAllies.Length)
        {
            return flagsToCheckForAllies[flagsToCheckForAllies.Length-1];
        }

        return flagsToCheckForAllies[index];
    }

}
