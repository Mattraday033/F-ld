using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnParamsList
{
    private const bool spawnWhileHostile = true;
    private readonly static InteractableSpawnParams noNameParams = new InteractableSpawnParams(spawnWhileHostile);

    private static Dictionary<KeyValuePair<string, string>, InteractableSpawnParams> interactableSpawnParamsDict;

    #region Reusable Variables

    private readonly static StopSpawningFlagList directorDefeatedStopSpawning = new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated });
    private readonly static StopSpawningFlagList revoltStartedStopSpawning = new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated });

    private readonly static StartSpawningFlagList rallySlavesCampNE = new StartSpawningFlagList(new string[] { FlagNameList.duringSlaveRallyConversation });
    private readonly static InteractableSpawnParams slavesInNorthEastCamp = new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou}),
                                                                                                        new StopSpawningFlagList(new string[] { FlagNameList.waitingOnGarchaToSpeak }), spawnWhileHostile);


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

        if (!interactableSpawnParamsDict.ContainsKey(kvp))
        {
            return new InteractableSpawnParams();
        }

        return interactableSpawnParamsDict[kvp];
    }

    public static InteractableSpawnParams getMonsterSpawnParams(string areaName, string npcName)
    {
        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(areaName, npcName);

        if (!interactableSpawnParamsDict.ContainsKey(kvp))
        {
            return new MonsterSpawnParams();
        }

        return interactableSpawnParamsDict[kvp];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateNPCSpawnParamList()
    {
        //interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList., NPCNameList.), new InteractableSpawnParams(new string[]{}, new string[]{}));

        interactableSpawnParamsDict = new Dictionary<KeyValuePair<string, string>, InteractableSpawnParams>();

        #region Slave Shack 1

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackOne, NPCNameList.balint),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackOne, NPCNameList.seb),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated })));

        #endregion
        #region Slave Shack 2

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackTwo, NPCNameList.broglin),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.goesWithBroglinsPlan,
                                                                                          FlagNameList.directorDefeated })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackTwo, NPCNameList.garcha),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        #endregion
        #region Slave Shack 3

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackThree, NPCNameList.janos),
                               new InteractableSpawnParams(directorDefeatedStopSpawning));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackThree, NPCNameList.guardAndras + 1),
                               new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.andrasLeftInHut }),
                                                    directorDefeatedStopSpawning));

        #endregion
        #region Slave Shack 4

        InteractableSpawnParams marcosSS4 = new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.broughtNandorToKastor }),
                                            new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated }), spawnWhileHostile);

        PartyMemberSpawnParams carterAndNandorSS4 = new PartyMemberSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.broughtNandorToKastor }),
                                            new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated }), spawnWhileHostile);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.kastor),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.nandor), carterAndNandorSS4);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.carter), carterAndNandorSS4);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.guardMarcos), marcosSS4);

        #endregion
        #region Slave Shack 5

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFive, NPCNameList.ervin),
                               new InteractableSpawnParams(revoltStartedStopSpawning));
        #endregion
        #region Slave Shack 6

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.thatch),
                               new PartyMemberSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.slate), new InteractableSpawnParams(directorDefeatedStopSpawning, spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.guardVazul), new InteractableSpawnParams(
                                                                new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated, FlagNameList.foundSlate }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.rubble), new InteractableSpawnParams(
                                                                new StopSpawningFlagList(new string[] { FlagNameList.thatchRemovedTutorialRubble }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.thatch + 1),
                               new InteractableSpawnParams(spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, Constants.indexOne.ToString()),
                               new MonsterSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.choseStrengthAtStart, FlagNameList.choseDexterityAtStart })));

        #region Str Tutorial

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.halfWall + Constants.STRDesignator), strTutorial);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.liftableRubble), strTutorial);

        #endregion
        #region Dex Tutorial

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.halfWall + Constants.DEXDesignator), dexTutorial);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.rubble + Constants.DEXDesignator), dexTutorial);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.vaultableBarrels), dexTutorial);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, CunningObjectSpriteCategory.Statue.ToString()), dexTutorial);

        #endregion
        #region Wis Tutorial (Empty)
        #endregion
        #region Cha Tutorial

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.fallenBeam), chaTutorial);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.button), chaTutorial);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.rubble + Constants.CHADesignator), chaTutorial);
        #endregion


        #endregion

        #region MessHall

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.messHall, NPCNameList.kende),
                                new InteractableSpawnParams(revoltStartedStopSpawning));

        #endregion

        #region Stables

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.beam),
                                new InteractableSpawnParams(revoltStartedStopSpawning));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse),
                                new InteractableSpawnParams(revoltStartedStopSpawning));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse + 1),
                                new InteractableSpawnParams(revoltStartedStopSpawning));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse + 2),
                                new InteractableSpawnParams(revoltStartedStopSpawning));

        #endregion

        #region Stockhouse

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stockhouse, NPCNameList.uros),
                                new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.snitchedOnUros })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stockhouse, NPCNameList.quartermasterEmese),
                                new InteractableSpawnParams(revoltStartedStopSpawning));

        #endregion

        #region Camp North East
        
        #region Rally Slaves Convo

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slaveOne),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slaveTwo),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slaveThree),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slaveFour),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.crowd),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.janos),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.nandor),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.carter),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.clay),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.garcha),
                               new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.waitingOnGarchaToSpeak }), spawnWhileHostile));

        #endregion

        #region After Successful Rally Convo

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave+5), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave+6), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave+7), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave+8), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave+9), slavesInNorthEastCamp);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.woundedSlave), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.woundedSlave+1), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.woundedSlave+2), slavesInNorthEastCamp);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.garcha+1), slavesInNorthEastCamp);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.uros), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.kastor), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.guardMarcos), slavesInNorthEastCamp);

        #endregion

        #endregion

        #region Camp Center

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.csalan),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.chiefTabor),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted,
                                                                                          FlagNameList.directorDefeated,
                                                                                          FlagNameList.heardTaborsLesson })));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.branded),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted,
                                                                                          FlagNameList.directorDefeated,
                                                                                          FlagNameList.heardTaborsLesson })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.feher),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.temple),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.guard),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.guard+1),
                               new InteractableSpawnParams(revoltStartedStopSpawning));

        #endregion

        #region Camp Mine Entrance

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.mineCratesCleared })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.barricade),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.mineCratesCleared })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa + 1),
                               new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.mineCratesCleared }),
                                            new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.uros),
                                new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.snitchedOnUros }),
                                            new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));


        #endregion

        #region Camp Manse

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.imre),
                               new InteractableSpawnParams(new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));

        #endregion

        #region MineLvl_2-2a

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardPazman),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl2GuardsFinishedMove
                                                                                                          }),
                                                                    new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty
                                                                                                          }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardVirag),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl2GuardsFinishedMove
                                                                                                          }),
                                                                    new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty
                                                                                                           }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardReka),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl2GuardsFinishedMove
                                                                                                          }),
                                                                    new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty
                                                                                                           }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl2 + LocationNameList.section2a, NPCNameList.overseerGaspar),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl2GuardsFinishedMove
                                                                                                          }),
                                                                    new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty
                                                                                                           }), spawnWhileHostile));

        #endregion

        #region MineLvl_2-3a

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl2 + LocationNameList.section3a, NPCNameList.diary),
                                        new StatBasedSpawnParams(PrimaryStat.Wisdom, Constants.statLevelTwo,
                                        new StopSpawningFlagList(new string[] { BookList.mineGuardsJournalReadFlag }), spawnWhileHostile));

        #endregion

        #region MineLvl_3-3b

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardPazman),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3ClearedCratesToGuards
                                                                                                          }),
                                                                    new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsBackToSurface, 
                                                                                                            FlagNameList.mineLvl2GuardsMovedToSecondLevelGate, 
                                                                                                            FlagNameList.mineLvl3ConvincedRekaAndPazman
                                                                                                          }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardVirag),
                                        new InteractableSpawnParams(new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty, 
                                                                                                            FlagNameList.mineLvl2GuardsMovedToSecondLevelGate 
                                                                                                           }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardReka),
                                        new InteractableSpawnParams(new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty, 
                                                                                                            FlagNameList.mineLvl2GuardsMovedToSecondLevelGate 
                                                                                                           }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.overseerGaspar),
                                        new InteractableSpawnParams(new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty, 
                                                                                                            FlagNameList.mineLvl2GuardsMovedToSecondLevelGate 
                                                                                                           }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.barricade),
                                        new InteractableSpawnParams(new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3ClearedCratesToGuards
                                                                                                           }), spawnWhileHostile));

        #endregion

        #region MineLvl_3-Miner Camp

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter),
                                        new PartyMemberSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3ClearedCratesToMiners
                                                                                                          }),
                                                                    new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.directorDefeated, 
                                                                                                            FlagNameList.mineLvl3BreachSealed
                                                                                                          }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter+1),
                                        new PartyMemberSpawnParams( new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3ClearedCratesToMiners
                                                                                                          }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.nandor),
                                        new PartyMemberSpawnParams(new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.directorDefeated, 
                                                                                                            FlagNameList.mineLvl3BreachSealed
                                                                                                           }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.guardMarcos),
                                        new InteractableSpawnParams(new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.directorDefeated, 
                                                                                                            FlagNameList.mineLvl3BreachSealed,
                                                                                                            FlagNameList.mineLvl3MarcosAgreedToIgniteJelly
                                                                                                           }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.barricade),
                                        new InteractableSpawnParams(new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3ClearedCratesToMiners
                                                                                                           }), spawnWhileHostile));
        #endregion

        #region MineLvl_3-7

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.section7, NPCNameList.rubble),
                                        new InteractableSpawnParams(new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3BreachSealed
                                                                                                          }), spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.mineLvl3 + LocationNameList.section7, NPCNameList.rubble+1),
                                        new InteractableSpawnParams(new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3BreachSealed
                                                                                                          }), spawnWhileHostile));

        #endregion

    }
    




}
