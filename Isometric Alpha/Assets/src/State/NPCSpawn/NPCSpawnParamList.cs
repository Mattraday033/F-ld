using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NPCSpawnParamList
{
    private const bool spawnWhileHostile = true;
    private static NPCSpawnParams noNameParams = new NPCSpawnParams(new StartSpawningFlagList(), new StopSpawningFlagList(), true, false);

    private static Dictionary<KeyValuePair<string, string>, NPCSpawnParams> npcSpawnParamsDict;

    #region Reusable Variables

    private readonly static StopSpawningFlagList directorDefeatedStopSpawning = new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated });
    private readonly static StopSpawningFlagList revoltStartedStopSpawning = new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated });

    #endregion

    public static NPCSpawnParams getNPCSpawnParams(string areaName, string npcName)
    {
        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(areaName, npcName);

        if (npcName.Length <= 0)
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

        #region Slave Shack 1

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackOne, NPCNameList.balint),
                               new NPCSpawnParams(revoltStartedStopSpawning));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackOne, NPCNameList.seb),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated })));

        #endregion
        #region Slave Shack 2

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackTwo, NPCNameList.broglin),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.goesWithBroglinsPlan,
                                                                                          FlagNameList.directorDefeated })));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackTwo, NPCNameList.garcha),
                               new NPCSpawnParams(revoltStartedStopSpawning));

        #endregion
        #region Slave Shack 3

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackThree, NPCNameList.janos),
                               new NPCSpawnParams(revoltStartedStopSpawning));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackThree, NPCNameList.guardAndras + 1),
                               new NPCSpawnParams(revoltStartedStopSpawning));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackThree, NPCNameList.guardAndras + 2),
                               new NPCSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.andrasLeftInHut }),
                                                    revoltStartedStopSpawning));

        #endregion
        #region Slave Shack 4

        NPCSpawnParams carterNandorGuardMarcosSS4 = new NPCSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.broughtNandorToKastor,
                                                                                           FlagNameList.directorDefeated }),
                                            new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated }));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackFour, NPCNameList.kastor),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated })));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackFour, NPCNameList.nandor), carterNandorGuardMarcosSS4);
        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackFour, NPCNameList.carter), carterNandorGuardMarcosSS4);
        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackFour, NPCNameList.guardMarcos), carterNandorGuardMarcosSS4);

        #endregion
        #region Slave Shack 5

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackFive, NPCNameList.ervin),
                               new NPCSpawnParams(revoltStartedStopSpawning));
        #endregion
        #region Slave Shack 6

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackSix, NPCNameList.thatch),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated }), spawnWhileHostile));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackSix, NPCNameList.slate), new NPCSpawnParams(directorDefeatedStopSpawning, spawnWhileHostile));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackSix, NPCNameList.guardVazul), new NPCSpawnParams(
                                                                new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated, FlagNameList.foundSlate }), spawnWhileHostile));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackSix, NPCNameList.rubble), new NPCSpawnParams(
                                                                new StopSpawningFlagList(new string[] { FlagNameList.thatchRemovedTutorialRubble }), spawnWhileHostile));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.slaveShackSix, NPCNameList.thatch + 1),
                               new NPCSpawnParams(spawnWhileHostile));

        #endregion

        #region Camp Mine Entrance

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.campMineEntrance, NPCNameList.guardMuzsa),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.mineCratesCleared })));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.campMineEntrance, NPCNameList.barricade),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.mineCratesCleared })));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.campMineEntrance, NPCNameList.guardMuzsa+2),
                               new NPCSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.mineCratesCleared }),
                                            new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));

        #endregion

        #region Camp Manse

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(AreaNameList.campManse, NPCNameList.imre),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));

        #endregion

    }
    




}
