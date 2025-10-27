using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NPCSpawnParamList
{
    private const bool spawnWhileHostile = true;
    private readonly static NPCSpawnParams noNameParams = new NPCSpawnParams(new StartSpawningFlagList(), new StopSpawningFlagList(), spawnWhileHostile);

    private static Dictionary<KeyValuePair<string, string>, NPCSpawnParams> npcSpawnParamsDict;

    #region Reusable Variables

    private readonly static StopSpawningFlagList directorDefeatedStopSpawning = new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated });
    private readonly static StopSpawningFlagList revoltStartedStopSpawning = new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated });

    private readonly static NPCSpawnParams dexTutorial = new NPCSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.choseDexterityAtStart }),spawnWhileHostile);

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

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateNPCSpawnParamList()
    {
        //npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList., NPCNameList.), new NPCSpawnParams(new string[]{}, new string[]{}));

        npcSpawnParamsDict = new Dictionary<KeyValuePair<string, string>, NPCSpawnParams>();

        #region Slave Shack 1

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackOne, NPCNameList.balint),
                               new NPCSpawnParams(revoltStartedStopSpawning));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackOne, NPCNameList.seb),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated })));

        #endregion
        #region Slave Shack 2

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackTwo, NPCNameList.broglin),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.goesWithBroglinsPlan,
                                                                                          FlagNameList.directorDefeated })));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackTwo, NPCNameList.garcha),
                               new NPCSpawnParams(revoltStartedStopSpawning));

        #endregion
        #region Slave Shack 3

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackThree, NPCNameList.janos),
                               new NPCSpawnParams(revoltStartedStopSpawning));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackThree, NPCNameList.guardAndras + 1),
                               new NPCSpawnParams(revoltStartedStopSpawning));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackThree, NPCNameList.guardAndras + 2),
                               new NPCSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.andrasLeftInHut }),
                                                    revoltStartedStopSpawning));

        #endregion
        #region Slave Shack 4

        NPCSpawnParams carterNandorGuardMarcosSS4 = new NPCSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.broughtNandorToKastor,
                                                                                           FlagNameList.directorDefeated }),
                                            new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated }));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.kastor),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated })));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.nandor), carterNandorGuardMarcosSS4);
        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.carter), carterNandorGuardMarcosSS4);
        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.guardMarcos), carterNandorGuardMarcosSS4);

        #endregion
        #region Slave Shack 5

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFive, NPCNameList.ervin),
                               new NPCSpawnParams(revoltStartedStopSpawning));
        #endregion
        #region Slave Shack 6

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.thatch),
                               new PartyMemberNPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated }), spawnWhileHostile));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.slate), new NPCSpawnParams(directorDefeatedStopSpawning, spawnWhileHostile));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.guardVazul), new NPCSpawnParams(
                                                                new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated, FlagNameList.foundSlate }), spawnWhileHostile));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.rubble), new NPCSpawnParams(
                                                                new StopSpawningFlagList(new string[] { FlagNameList.thatchRemovedTutorialRubble }), spawnWhileHostile));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.thatch + 1),
                               new NPCSpawnParams(spawnWhileHostile));

        #region Str Tutorial

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.liftableRubble),
                               new NPCSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.choseStrengthAtStart }), spawnWhileHostile));

        #endregion
        #region Dex Tutorial

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.halfWall + Constants.DEXDesignator), dexTutorial);
        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.rubble + Constants.DEXDesignator), dexTutorial);
        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.vaultableBarrels), dexTutorial);
        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, CunningObjectSpriteCategory.Statue.ToString()), dexTutorial);

        #endregion
        #region Wis Tutorial

        #endregion
        #region Cha Tutorial

        #endregion


        #endregion

        #region Stables

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.beam),
                                new NPCSpawnParams(revoltStartedStopSpawning));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse),
                                new NPCSpawnParams(revoltStartedStopSpawning));
        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse + 1),
                                new NPCSpawnParams(revoltStartedStopSpawning));
        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse + 2),
                                new NPCSpawnParams(revoltStartedStopSpawning));

        #endregion

        #region Camp Center

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.csalan),
                               new NPCSpawnParams(revoltStartedStopSpawning));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.chiefTabor),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted,
                                                                                          FlagNameList.directorDefeated,
                                                                                          FlagNameList.heardTaborsLesson })));
        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.branded),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted,
                                                                                          FlagNameList.directorDefeated,
                                                                                          FlagNameList.heardTaborsLesson })));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.feher),
                               new NPCSpawnParams(revoltStartedStopSpawning));

        #endregion

        #region Camp Mine Entrance

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.mineCratesCleared })));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.barricade),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.mineCratesCleared })));

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa + 2),
                               new NPCSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.mineCratesCleared }),
                                            new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));

        #endregion

        #region Camp Manse

        npcSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.imre),
                               new NPCSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));

        #endregion

    }
    




}
