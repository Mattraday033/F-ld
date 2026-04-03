using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterSpawnConditionsList
{
    
    public static bool wormsSpawnInsideCamp()
    {
        return Flags.getFlag(FlagNameList.wormsAttackedCamp) &&
                    !Flags.getFlag(FlagNameList.mineLvl3BreachSealed);
    }

}
