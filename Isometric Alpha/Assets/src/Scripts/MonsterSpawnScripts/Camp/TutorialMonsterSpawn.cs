using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMonsterSpawn : MonsterSpawnScript
{

    public override bool evaluateScript() //if true, use alternate monster pack list
    {
        if (Flags.getFlag(FlagNameList.choseWisdomAtStart) || Flags.getFlag(FlagNameList.choseCharismaAtStart))
        {
            return true;
        }

        return false;
    }


}
