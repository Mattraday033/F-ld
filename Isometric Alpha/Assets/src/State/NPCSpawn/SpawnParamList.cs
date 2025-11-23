using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnParamList
{
    private const bool spawnWhileHostile = true;
    private readonly static InteractableSpawnParams noNameParams = new InteractableSpawnParams(spawnWhileHostile);

    private static Dictionary<KeyValuePair<string, string>, InteractableSpawnParams> InteractableSpawnParamsDict;

    #region Reusable Variables

    private readonly static StopSpawningFlagList directorDefeatedStopSpawning = new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated });
    private readonly static StopSpawningFlagList revoltStartedStopSpawning = new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated });

    private readonly static InteractableSpawnParams strTutorial = new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.choseStrengthAtStart }), spawnWhileHostile);
    private readonly static InteractableSpawnParams dexTutorial = new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.choseDexterityAtStart }), spawnWhileHostile);
    private readonly static InteractableSpawnParams chaTutorial = new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.choseCharismaAtStart }), spawnWhileHostile);

    #endregion

    public static InteractableSpawnParams getSpawnParams(string areaName, string npcName)
    {
        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(areaName, npcName);

        if (npcName.Length <= 0)
        {
            return noNameParams;
        }

        if (!InteractableSpawnParamsDict.ContainsKey(kvp))
        {
            return new InteractableSpawnParams();
        }

        return InteractableSpawnParamsDict[kvp];
    }

    public static InteractableSpawnParams getMonsterSpawnParams(string areaName, string npcName)
    {
        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(areaName, npcName);

        if (!InteractableSpawnParamsDict.ContainsKey(kvp))
        {
            return new MonsterSpawnParams();
        }

        return InteractableSpawnParamsDict[kvp];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateNPCSpawnParamList()
    {
        //InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList., NPCNameList.), new InteractableSpawnParams(new string[]{}, new string[]{}));

        InteractableSpawnParamsDict = new Dictionary<KeyValuePair<string, string>, InteractableSpawnParams>();

        #region Slave Shack 1

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackOne, NPCNameList.balint),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackOne, NPCNameList.seb),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated })));

        #endregion
        #region Slave Shack 2

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackTwo, NPCNameList.broglin),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.goesWithBroglinsPlan,
                                                                                          FlagNameList.directorDefeated })));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackTwo, NPCNameList.garcha),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        #endregion
        #region Slave Shack 3

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackThree, NPCNameList.janos),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackThree, NPCNameList.guardAndras + 1),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackThree, NPCNameList.guardAndras + 2),
                               new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.andrasLeftInHut }),
                                                    revoltStartedStopSpawning));

        #endregion
        #region Slave Shack 4

        InteractableSpawnParams carterNandorGuardMarcosSS4 = new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.broughtNandorToKastor,
                                                                                           FlagNameList.directorDefeated }),
                                            new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated }));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.kastor),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated })));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.nandor), carterNandorGuardMarcosSS4);
        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.carter), carterNandorGuardMarcosSS4);
        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.guardMarcos), carterNandorGuardMarcosSS4);

        #endregion
        #region Slave Shack 5

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFive, NPCNameList.ervin),
                               new InteractableSpawnParams(revoltStartedStopSpawning));
        #endregion
        #region Slave Shack 6

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.thatch),
                               new PartyMemberInteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated }), spawnWhileHostile));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.slate), new InteractableSpawnParams(directorDefeatedStopSpawning, spawnWhileHostile));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.guardVazul), new InteractableSpawnParams(
                                                                new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated, FlagNameList.foundSlate }), spawnWhileHostile));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.rubble), new InteractableSpawnParams(
                                                                new StopSpawningFlagList(new string[] { FlagNameList.thatchRemovedTutorialRubble }), spawnWhileHostile));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.thatch + 1),
                               new InteractableSpawnParams(spawnWhileHostile));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, Constants.indexOne.ToString()),
                               new MonsterSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.choseStrengthAtStart, FlagNameList.choseDexterityAtStart })));

        #region Str Tutorial

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.halfWall + Constants.STRDesignator), strTutorial);

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.liftableRubble), strTutorial);

        #endregion
        #region Dex Tutorial

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.halfWall + Constants.DEXDesignator), dexTutorial);
        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.rubble + Constants.DEXDesignator), dexTutorial);
        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.vaultableBarrels), dexTutorial);
        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, CunningObjectSpriteCategory.Statue.ToString()), dexTutorial);

        #endregion
        #region Wis Tutorial (Empty)
        #endregion
        #region Cha Tutorial

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.fallenBeam), chaTutorial);
        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.button), chaTutorial);
        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.rubble + Constants.CHADesignator), chaTutorial);
        #endregion


        #endregion

        #region Stables

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.beam),
                                new InteractableSpawnParams(revoltStartedStopSpawning));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse),
                                new InteractableSpawnParams(revoltStartedStopSpawning));
        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse + 1),
                                new InteractableSpawnParams(revoltStartedStopSpawning));
        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse + 2),
                                new InteractableSpawnParams(revoltStartedStopSpawning));

        #endregion

        #region Camp Center

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.csalan),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.chiefTabor),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted,
                                                                                          FlagNameList.directorDefeated,
                                                                                          FlagNameList.heardTaborsLesson })));
        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.branded),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted,
                                                                                          FlagNameList.directorDefeated,
                                                                                          FlagNameList.heardTaborsLesson })));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.feher),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        #endregion

        #region Camp Mine Entrance

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.mineCratesCleared })));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.barricade),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.mineCratesCleared })));

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa + 2),
                               new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.mineCratesCleared }),
                                            new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));

        #endregion

        #region Camp Manse

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.imre),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));

        #endregion

        #region MineLvl_2-3a

        InteractableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl2 + LocationNameList.section3a, NPCNameList.diary),
                                        new StatBasedSpawnParams(PrimaryStat.Wisdom, Constants.statLevelTwo,
                                        new StopSpawningFlagList(new string[] { BookList.mineGuardsJournalReadFlag }), spawnWhileHostile));

        #endregion

    }
    




}
