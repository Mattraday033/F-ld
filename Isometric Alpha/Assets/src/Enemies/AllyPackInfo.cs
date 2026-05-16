using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyPackInfo : EnemyPackInfo
{
    protected string[] flagsToCheckForAllies;

    public AllyPackInfo(CreatureAmount FoeType, string flagToCheckForAllies):
    base(new CreatureAmount[]{ FoeType }, null)
    {
        this.flagsToCheckForAllies = new string[]{ flagToCheckForAllies };
    }

    public AllyPackInfo(CreatureAmount[] FoeTypes, string[] flagsToCheckForAllies):
    base(FoeTypes, null)
    {
        this.flagsToCheckForAllies = flagsToCheckForAllies;
    }

    public override IEnumerator GetEnumerator()
    {
        List<CreatureAmount> relevantAllies = new List<CreatureAmount>();

        for(int index = 0; index < FoeTypes.Length; index++)
        {
            if (flagsToCheckForAllies.Length <= Constants.sizeZero|| 
                Flags.getFlag(getFlagAtIndex(index)))
            {
                relevantAllies.Add(FoeTypes[index]);
            }
        }

        List<Stats> allStatsInPack = new List<Stats>();

        foreach (CreatureAmount amount in relevantAllies)
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
