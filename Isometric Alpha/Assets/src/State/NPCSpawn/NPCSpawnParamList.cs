using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NPCSpawnParamList
{
    private static NPCSpawnParams noNameParams = new NPCSpawnParams(new StartSpawningFlagList(), new StopSpawningFlagList(), true, false);

    private static Dictionary<KeyValuePair<string, string>, NPCSpawnParams> npcSpawnParamsDict;

    public static NPCSpawnParams getNPCSpawnParams(string areaName, string npcName)
    {
        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(areaName, npcName);

        if(npcName.Length <= 0)
        {
            return noNameParams;
        }

        if (!npcSpawnParamsDict.ContainsKey(kvp))
        {
            return new NPCSpawnParams();
        }

        return npcSpawnParamsDict[kvp];
    }

    static NPCSpawnParamList()
    {
        //npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList., NPCNameList.), new NPCSpawnParams(new string[]{}, new string[]{}));

        npcSpawnParamsDict = new Dictionary<KeyValuePair<string, string>, NPCSpawnParams>();

        #region TestRoom

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackTwo, NPCNameList.broglin),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.goesWithBroglinsPlan,
                                                                                          FlagNameList.directorDefeated })));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackTwo, NPCNameList.garcha),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated,
                                                                                          FlagNameList.kastorStartedRevolt })));

        #endregion

    }
    




}
