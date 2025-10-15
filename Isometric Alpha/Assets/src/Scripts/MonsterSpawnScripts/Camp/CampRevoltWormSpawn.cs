using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampRevoltWormSpawn : MonsterSpawnScript
{

    public override bool evaluateScript()
    {
        if(Flags.getFlag(FlagNameList.kastorStartedRevolt) && !Flags.getFlag(FlagNameList.mineLvl3BreachSealed))
        {
            Flags.setFlag(FlagNameList.spawnWormsInsteadOfGuards, true);
        }

        return Flags.getFlag(FlagNameList.spawnWormsInsteadOfGuards);
    }


}
