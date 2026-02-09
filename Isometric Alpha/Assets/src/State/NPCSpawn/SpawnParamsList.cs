using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnParamsList
{
    private const bool doesNotSpawnWhileHostile = false;
    private const bool spawnWhileHostile = true;
    private const bool onlySpawnWhileHostile = true;
    private readonly static InteractableSpawnParams noNameParams = new InteractableSpawnParams(spawnWhileHostile: spawnWhileHostile);
    private readonly static InteractableSpawnParams spawnOnlyWhenHostileParams = new InteractableSpawnParams(spawnWhileHostile: spawnWhileHostile, onlySpawnWhileHostile: onlySpawnWhileHostile);

    private static Dictionary<KeyValuePair<string, string>, InteractableSpawnParams> interactableSpawnParamsDict;

    #region Reusable Variables

    private readonly static StopSpawningFlagList directorDefeatedStopSpawning = new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated });
    private readonly static StopSpawningFlagList revoltStartedStopSpawning = new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated });

    private readonly static StartSpawningFlagList rallySlavesCampNE = new StartSpawningFlagList(new string[] { FlagNameList.duringSlaveRallyConversation });
    private readonly static StartSpawningFlagList nandorCarterRallySlavesCampNE = new StartSpawningAllTrueFlagList(new string[] { FlagNameList.duringSlaveRallyConversation, FlagNameList.mineLvl3CarterAndNandorInParty});

    private readonly static InteractableSpawnParams slavesInNorthEastCamp = new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou}),
                                                                                                        new StopSpawningFlagList(new string[] { FlagNameList.waitingOnGarchaToSpeak, FlagNameList.directorDefeated }), spawnWhileHostile);


    private readonly static InteractableSpawnParams strTutorial = new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.choseStrengthAtStart }), spawnWhileHostile: spawnWhileHostile);
    private readonly static InteractableSpawnParams dexTutorial = new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.choseDexterityAtStart }), spawnWhileHostile: spawnWhileHostile);
    private readonly static InteractableSpawnParams chaTutorial = new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.choseCharismaAtStart }), spawnWhileHostile: spawnWhileHostile);

    #endregion

    //list of npcNames that can only ever spawn in a hostile area, like Hastily Built Barricades
    public static bool npcNameOnlySpawnsWhileHostile(string npcName)
    {
        npcName = DialogueList.scrubNameOfEndNumbers(npcName);

        switch(npcName)
        {
            case NPCNameList.hastilyBuiltBarricade:
                return true;
            default:
                return false;
        }
    }

    public static InteractableSpawnParams getSpawnParams(string areaName, string npcName)
    {
        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(areaName, npcName);

        if (npcName.Length <= 0)
        {
            return noNameParams;
        }

        if(npcNameOnlySpawnsWhileHostile(npcName))
        {
            return spawnOnlyWhenHostileParams;
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
                               new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackOne, NPCNameList.seb),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated })));

        #endregion
        #region Slave Shack 2

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackTwo, NPCNameList.broglin),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.goesWithBroglinsPlan,
                                                                                          FlagNameList.directorDefeated })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackTwo, NPCNameList.garcha),
                               new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning, spawnWhileHostile: spawnWhileHostile));

        #endregion
        #region Slave Shack 3

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackThree, NPCNameList.janos),
                               new InteractableSpawnParams(stopSpawningFlagList: directorDefeatedStopSpawning));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackThree, NPCNameList.guardAndras + 1),
                               new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.andrasLeftInHut }),
                                                    stopSpawningFlagList: directorDefeatedStopSpawning));

        #endregion
        #region Slave Shack 4

        InteractableSpawnParams marcosSS4 = new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.broughtNandorToKastor }),
                                            stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated,
                                                                                          FlagNameList.kastorStartedRevolt,
                                                                                          FlagNameList.marcosSleepingSS4 }), spawnWhileHostile: spawnWhileHostile);

        InteractableSpawnParams marcosSleepingSS4 = new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.broughtNandorToKastor }),
                                            stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated}), spawnWhileHostile: spawnWhileHostile);

        PartyMemberSpawnParams carterAndNandorSS4 = new PartyMemberSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.broughtNandorToKastor }),
                                            stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated }), spawnWhileHostile: spawnWhileHostile);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.kastor),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.convincedSlavesToHelpYou,
                                                                                          FlagNameList.directorDefeated }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.nandor), carterAndNandorSS4);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.carter), carterAndNandorSS4);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.guardMarcos), marcosSS4);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFour, NPCNameList.guardMarcos+1), marcosSleepingSS4);

        #endregion
        #region Slave Shack 5

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackFive, NPCNameList.ervin),
                               new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning));
        #endregion
        #region Slave Shack 6

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.thatch),
                               new PartyMemberSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.slate), new InteractableSpawnParams(stopSpawningFlagList: directorDefeatedStopSpawning, spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.guardVazul), new InteractableSpawnParams(
                                                                stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated, FlagNameList.foundSlate }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.rubble), new InteractableSpawnParams(
                                                                stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.thatchRemovedTutorialRubble }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.thatch + 1),
                               new InteractableSpawnParams(spawnWhileHostile: spawnWhileHostile));

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
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, CunningObjectSpriteCategory.Crank.ToString()), dexTutorial);

        #endregion
        #region Wis Tutorial (Empty)
        #endregion
        #region Cha Tutorial

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.awkwardRubble), chaTutorial);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.button), chaTutorial);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.slaveShackSix, NPCNameList.rubble + Constants.CHADesignator), chaTutorial);
        #endregion


        #endregion

        #region Guard House NE

        InteractableSpawnParams barracksGateSpawnParams = new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated }), spawnWhileHostile: spawnWhileHostile, onlySpawnWhileHostile: onlySpawnWhileHostile);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.guardHouseNorthEast, NPCNameList.barracksGate), barracksGateSpawnParams);

        #endregion

        #region Guard House SW

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.guardHouseSouthWest, NPCNameList.barracksGate), barracksGateSpawnParams);
        
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.guardHouseSouthWest, NPCNameList.guard), new InteractableSpawnParams(spawnWhileHostile: doesNotSpawnWhileHostile));

        #endregion

        #region MessHall

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.messHall, NPCNameList.noBrand+1),
                                new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.messHall, NPCNameList.kende),
                                new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning));

        #endregion

        #region Stables

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.beam),
                                new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse),
                                new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse + 1),
                                new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stables, NPCNameList.horse + 2),
                                new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning));

        #endregion

        #region Stockhouse

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stockhouse, NPCNameList.uros),
                                new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.snitchedOnUros })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.stockhouse, NPCNameList.quartermasterEmese),
                                new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning));

        #endregion

        #region Camp North East
        
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.overseer),
                               new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning));

        #region Rally Slaves Convo

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slaveOne),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slaveTwo),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slaveThree),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slaveFour),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave+10),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave+11),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave+12),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.crowd),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.thatch),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.ervin),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.kastor+1),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.uros+1),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.balint),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.temple),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));


        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.janos),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.clay),
                               new InteractableSpawnParams(rallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
                               
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.nandor),
                               new InteractableSpawnParams(nandorCarterRallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.carter),
                               new InteractableSpawnParams(nandorCarterRallySlavesCampNE, spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.garcha),
                               new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.waitingOnGarchaToSpeak }), spawnWhileHostile: spawnWhileHostile));

        #endregion

        #region After Successful Rally Convo

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.temple+1), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave+6), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave+7), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.slave+8), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.clay+1), slavesInNorthEastCamp);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.woundedSlave), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.woundedSlave+1), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.woundedSlave+2), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.bed), slavesInNorthEastCamp);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.garcha+1), slavesInNorthEastCamp);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.uros), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.kastor), slavesInNorthEastCamp);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campNorthEast, NPCNameList.guardMarcos), slavesInNorthEastCamp);

        #endregion

        #endregion

        #region Camp Center

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.csalan),
                               new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning, spawnWhileHostile: doesNotSpawnWhileHostile));


        InteractableSpawnParams taborLessonSpawnParams = new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted,
                                                                                          FlagNameList.directorDefeated,
                                                                                          FlagNameList.heardTaborsLesson }), spawnWhileHostile: doesNotSpawnWhileHostile);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.chiefTabor), taborLessonSpawnParams);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.branded), taborLessonSpawnParams);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.branded + 1), taborLessonSpawnParams);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.branded + 2), taborLessonSpawnParams);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.feher),
                               new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning, spawnWhileHostile: doesNotSpawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.temple),
                               new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning, spawnWhileHostile: doesNotSpawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.guard),
                               new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning, spawnWhileHostile: doesNotSpawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.guard+1),
                               new InteractableSpawnParams(stopSpawningFlagList: revoltStartedStopSpawning, spawnWhileHostile: doesNotSpawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.page),
                                        new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[]  { 
                                                                                            FlagNameList.directorDefeated
                                                                                        }), 
                                        stopSpawningFlagList: new StopSpawningFlagList(new string[]  { 
                                                                                    FlagNameList.enteredCivilizationAfterLeavingCamp
                                                                                })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.carter),
                                        new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[]  { 
                                                                                            FlagNameList.directorDefeated,
                                                                                            FlagNameList.inLeavingCampPageCarterConvo
                                                                                        })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.vaultableBarrels+1), barracksGateSpawnParams);


        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.barricadeGuards+1),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[]{FlagNameList.directorDefeated, FlagNameList.barricadeGuardDefeatKey1}), onlySpawnWhileHostile: onlySpawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campCenter, NPCNameList.barricade+1),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[]{FlagNameList.directorDefeated, FlagNameList.barricadeGuardDefeatKey1}), onlySpawnWhileHostile: onlySpawnWhileHostile));

        #endregion

        #region Camp South East

        string[] directorStatueBrokenConditions = new string[]  { 
                                                                    FlagNameList.convincedSlavesToHelpYou,
                                                                    FlagNameList.directorDefeated
                                                                };

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.statue),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(directorStatueBrokenConditions), spawnWhileHostile: spawnWhileHostile));
                                        

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.toppledStatue),
                                        new InteractableSpawnParams(new StartSpawningFlagList(directorStatueBrokenConditions)));

        #region Guard Punishment Scene

        StartSpawningAllTrueFlagList guardPunishmentCrowdStartSpawning = new StartSpawningAllTrueFlagList(new string[] { FlagNameList.directorDefeated, FlagNameList.mineLvl3CarterAndNandorInParty });
        StopSpawningFlagList guardPunishmentCrowdStopSpawning = new StopSpawningFlagList(new string[] { FlagNameList.spokeWithNandorAfterPrisoners, FlagNameList.foughtCrowdForTabor });

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.slave),
                               new InteractableSpawnParams(guardPunishmentCrowdStartSpawning, guardPunishmentCrowdStopSpawning));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.slave+1),
                               new InteractableSpawnParams(guardPunishmentCrowdStartSpawning, guardPunishmentCrowdStopSpawning));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.slave+2),
                               new InteractableSpawnParams(guardPunishmentCrowdStartSpawning, guardPunishmentCrowdStopSpawning));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.slave+3),
                               new InteractableSpawnParams(guardPunishmentCrowdStartSpawning, guardPunishmentCrowdStopSpawning));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.slave+4),
                               new InteractableSpawnParams(guardPunishmentCrowdStartSpawning, guardPunishmentCrowdStopSpawning));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.slave+5),
                               new InteractableSpawnParams(guardPunishmentCrowdStartSpawning, guardPunishmentCrowdStopSpawning));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.slave+6),
                               new InteractableSpawnParams(guardPunishmentCrowdStartSpawning, guardPunishmentCrowdStopSpawning));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.clay),
                               new InteractableSpawnParams(guardPunishmentCrowdStartSpawning, guardPunishmentCrowdStopSpawning));

            #region Pazman Ervin Reka

                InteractableSpawnParams pazmanErvinPunishmentSpawnParams = new InteractableSpawnParams(new StartSpawningAllTrueMetaFlagList(new string[]    
                                                                                                            { 
                                                                                                                MetaFlagNameList.guardPazmanAndRekaAtTrial,
                                                                                                                MetaFlagNameList.pazmanNeedsHandling
                                                                                                            },

                                                            new StartSpawningAllTrueFlagList(new string[]   { 
                                                                                                                FlagNameList.directorDefeated, 
                                                                                                                FlagNameList.mineLvl3CarterAndNandorInParty
                                                                                                            })));

                interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.ervin),
                                        pazmanErvinPunishmentSpawnParams);

                interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.pazman),
                                                pazmanErvinPunishmentSpawnParams);
                
                interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.reka),
                                new InteractableSpawnParams(new StartSpawningAllTrueMetaFlagList(new string[]    
                                                                                                            { 
                                                                                                                MetaFlagNameList.guardPazmanAndRekaAtTrial,
                                                                                                                MetaFlagNameList.rekaNeedsHandling
                                                                                                            },
                                                            new StartSpawningAllTrueFlagList(new string[]   { 
                                                                                                                FlagNameList.directorDefeated, 
                                                                                                                FlagNameList.mineLvl3CarterAndNandorInParty
                                                                                                            }))));
            #endregion

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.marcos),
                                new InteractableSpawnParams(new StartSpawningAllTrueMetaFlagList(new string[]    
                                                                                                    { 
                                                                                                        MetaFlagNameList.marcosIsAtTrial,
                                                                                                        MetaFlagNameList.marcosNeedsHandling
                                                                                                    },

                                                    new StartSpawningAllTrueFlagList(new string[]   { 
                                                                                                        FlagNameList.directorDefeated, 
                                                                                                        FlagNameList.mineLvl3CarterAndNandorInParty
                                                                                                    }))));

        InteractableSpawnParams andrasJanosPunishmentSpawnParams = new InteractableSpawnParams(new StartSpawningAllTrueMetaFlagList(new string[]    
                                                                                                    { 
                                                                                                        MetaFlagNameList.andrasIsAtTrial,
                                                                                                        MetaFlagNameList.andrasNeedsHandling
                                                                                                    },

                                                    new StartSpawningAllTrueFlagList(new string[]   { 
                                                                                                        FlagNameList.directorDefeated, 
                                                                                                        FlagNameList.mineLvl3CarterAndNandorInParty
                                                                                                    })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.andras),
                                        new InteractableSpawnParams(new StartSpawningAllTrueMetaFlagList(new string[]    
                                                                                                    { 
                                                                                                        MetaFlagNameList.andrasIsAtTrial,
                                                                                                        MetaFlagNameList.andrasNeedsHandling
                                                                                                    },

                                                    new StartSpawningAllTrueFlagList(new string[]   { 
                                                                                                        FlagNameList.directorDefeated, 
                                                                                                        FlagNameList.mineLvl3CarterAndNandorInParty
                                                                                                    }))));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.janos),
                                        new InteractableSpawnParams(new StartSpawningAllTrueMetaFlagList(new string[]    
                                                                                                    { 
                                                                                                        MetaFlagNameList.janosIsAtTrial
                                                                                                    },

                                                    new StartSpawningAllTrueFlagList(new string[]   { 
                                                                                                        FlagNameList.directorDefeated, 
                                                                                                        FlagNameList.mineLvl3CarterAndNandorInParty,
                                                                                                    }))));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.chiefTabor),
                                        new InteractableSpawnParams(new StartSpawningAllTrueMetaFlagList(new string[]    
                                                                                                    { 
                                                                                                        MetaFlagNameList.taborIsAtTrial,
                                                                                                        MetaFlagNameList.taborNeedsHandling
                                                                                                    },

                                                    new StartSpawningAllTrueFlagList(new string[]   { 
                                                                                                        FlagNameList.directorDefeated, 
                                                                                                        FlagNameList.mineLvl3CarterAndNandorInParty
                                                                                                    }))));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.nandor),
                                        new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[]  { 
                                                                                                                        FlagNameList.directorDefeated, 
                                                                                                                        FlagNameList.mineLvl3CarterAndNandorInParty,
                                                                                                                        FlagNameList.enteredMessHallYardAfterRevolt
                                                                                                                    }), 
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[]  { 
                                                                                                                FlagNameList.nandorStartedGuardPunishmentConvo
                                                                                                            })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.nandor+1),
                                        new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[]  { 
                                                                                                                        FlagNameList.nandorStartedGuardPunishmentConvo
                                                                                                                    }), 
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[]  { 
                                                                                                                FlagNameList.nandorLeftParty,
                                                                                                                FlagNameList.nandorLeftPartyOverPrisonerPunishment,
                                                                                                                FlagNameList.spokeWithNandorAfterPrisoners
                                                                                                            })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.carter),
                                        new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[]  { 
                                                                                                                        FlagNameList.directorDefeated, 
                                                                                                                        FlagNameList.mineLvl3CarterAndNandorInParty
                                                                                                                    }), 
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[]  { 
                                                                                                                FlagNameList.spokeWithNandorAfterPrisoners
                                                                                                            })));

        InteractableSpawnParams miscGuardPunishmentSpawnParams = new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[]  { 
                                                                                                                        FlagNameList.directorDefeated, 
                                                                                                                        FlagNameList.mineLvl3CarterAndNandorInParty
                                                                                                                    }), 
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[]  { 
                                                                                                                FlagNameList.spokeWithNandorAfterPrisoners
                                                                                                            }));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.kastor),
                                        new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[]  { 
                                                                                                                        FlagNameList.directorDefeated, 
                                                                                                                        FlagNameList.mineLvl3CarterAndNandorInParty
                                                                                                                    }), 
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[]  { 
                                                                                                                FlagNameList.orderedTheHorsesBurned,
                                                                                                                FlagNameList.orderedTheHorsesEaten,
                                                                                                                FlagNameList.enteredCivilizationAfterLeavingCamp
                                                                                                            })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.garcha),
                                        miscGuardPunishmentSpawnParams);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.thatch),
                                        new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[]  { 
                                                                                                                        FlagNameList.directorDefeated, 
                                                                                                                        FlagNameList.mineLvl3CarterAndNandorInParty,
                                                                                                                        FlagNameList.toldKastorOfThatchsFate
                                                                                                                    }), 
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[]  { 
                                                                                                                FlagNameList.spokeWithNandorAfterPrisoners
                                                                                                            })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campSouthEast, NPCNameList.broglin),
                                        new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[]  { 
                                                                                                                        FlagNameList.directorDefeated, 
                                                                                                                        FlagNameList.mineLvl3CarterAndNandorInParty,
                                                                                                                        FlagNameList.freedBroglin
                                                                                                                    }), 
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[]  { 
                                                                                                                FlagNameList.spokeWithNandorAfterPrisoners
                                                                                                            })));
        #endregion

        #endregion

        #region Camp Mine Entrance

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.mineCratesCleared })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.barricade),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated, FlagNameList.mineCratesCleared })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa + 1),
                               new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.mineCratesCleared }),
                                            stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.uros),
                                new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.snitchedOnUros }),
                                            stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campMineEntrance, NPCNameList.barracksGate), barracksGateSpawnParams);

        #endregion

        #region Camp Manse

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.imre),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.directorDefeated })));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.imre+1),
                               new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.revoltStarted, FlagNameList.convincedImre }),
                                stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.askedImreToLeadTheWay, FlagNameList.directorDefeated }), spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.barracksGate+2),
                               new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.revoltStarted }),
                                stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.directorDefeated }), spawnWhileHostile: spawnWhileHostile, onlySpawnWhileHostile: onlySpawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.barricadeGuards+2),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[]{FlagNameList.directorDefeated, FlagNameList.barricadeGuardDefeatKey2}), onlySpawnWhileHostile: onlySpawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.barricade+2),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[]{FlagNameList.directorDefeated, FlagNameList.barricadeGuardDefeatKey2}), onlySpawnWhileHostile: onlySpawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.guardAndras+2),
                               new InteractableSpawnParams(new NeverSpawnFlagList()));


        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.barricadeGuards+3),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[]{FlagNameList.directorDefeated, FlagNameList.barricadeGuardDefeatKey3}), onlySpawnWhileHostile: onlySpawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.barricade+3),
                               new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[]{FlagNameList.directorDefeated, FlagNameList.barricadeGuardDefeatKey3}), onlySpawnWhileHostile: onlySpawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(LocationNameList.campManse, NPCNameList.guardAndras+3),
                               new InteractableSpawnParams(new NeverSpawnFlagList()));
        #endregion

        #region Mine

        #region MineLvl_2-2a

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardPazman),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl2GuardsFinishedMove
                                                                                                          }),
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty
                                                                                                          }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardVirag),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl2GuardsFinishedMove
                                                                                                          }),
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty
                                                                                                           }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardReka),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl2GuardsFinishedMove
                                                                                                          }),
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty
                                                                                                           }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.overseerGaspar),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl2GuardsFinishedMove
                                                                                                          }),
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty
                                                                                                           }), spawnWhileHostile: spawnWhileHostile));

        #endregion

        #region MineLvl_2-3a

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl2 + LocationNameList.section3a, NPCNameList.diary),
                                        new StatBasedSpawnParams(PrimaryStat.Wisdom, Constants.statLevelTwo,
                                        stopSpawningFlagList: new StopSpawningFlagList(new string[] { BookList.mineGuardsJournalReadFlag }), spawnWhileHostile: spawnWhileHostile));

        #endregion

        #region MineLvl_3-3b

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardPazman),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3ClearedCratesToGuards
                                                                                                          }),
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsBackToSurface, 
                                                                                                            FlagNameList.mineLvl2GuardsMovedToSecondLevelGate, 
                                                                                                            FlagNameList.mineLvl3ConvincedRekaAndPazman
                                                                                                          }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardVirag),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty, 
                                                                                                            FlagNameList.mineLvl2GuardsMovedToSecondLevelGate 
                                                                                                           }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardReka),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty, 
                                                                                                            FlagNameList.mineLvl2GuardsMovedToSecondLevelGate 
                                                                                                           }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.overseerGaspar),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3GuardsInParty, 
                                                                                                            FlagNameList.mineLvl2GuardsMovedToSecondLevelGate 
                                                                                                           }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.barricade),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3ClearedCratesToGuards
                                                                                                           }), spawnWhileHostile: spawnWhileHostile));

        #endregion

        #region MineLvl_3-Miner Camp

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter),
                                        new PartyMemberSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3ClearedCratesToMiners
                                                                                                          }),
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.directorDefeated, 
                                                                                                            FlagNameList.mineLvl3BreachSealed
                                                                                                          }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter+1),
                                        new PartyMemberSpawnParams( stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3ClearedCratesToMiners
                                                                                                          }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.nandor),
                                        new PartyMemberSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.directorDefeated, 
                                                                                                            FlagNameList.mineLvl3BreachSealed
                                                                                                           }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.guardMarcos),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.directorDefeated, 
                                                                                                            FlagNameList.mineLvl3BreachSealed,
                                                                                                            FlagNameList.mineLvl3MarcosAgreedToIgniteJelly
                                                                                                           }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.barricade),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3ClearedCratesToMiners
                                                                                                           }), spawnWhileHostile: spawnWhileHostile));
        #endregion

        #region MineLvl_3-7

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.rubble),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3BreachSealed
                                                                                                          }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.rubble+1),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3BreachSealed
                                                                                                          }), spawnWhileHostile: spawnWhileHostile));

        InteractableSpawnParams rubbleConvoSpawnParams = new InteractableSpawnParams(new StartSpawningFlagList(new string[] { 
                                                                                                            FlagNameList.mineLvl3InRubbleConversation
                                                                                                                            }),
                                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[]   {
                                                                                                                                FlagNameList.mineLvl3SlavesBackToSurface,
                                                                                                                                FlagNameList.mineLvl3GuardsBackToSurface
                                                                                                                            }), spawnWhileHostile: spawnWhileHostile);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.guardReka), rubbleConvoSpawnParams);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.guardVirag), rubbleConvoSpawnParams);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.guardPazman), rubbleConvoSpawnParams);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.overseerGaspar), rubbleConvoSpawnParams);

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.carter), rubbleConvoSpawnParams);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.nandor), rubbleConvoSpawnParams);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.guardMarcos), rubbleConvoSpawnParams);
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.guardMarcos+1), 
                                        new InteractableSpawnParams(new StartSpawningAllTrueFlagList(new string[]  { 
                                                                                                                FlagNameList.mineLvl3InRubbleConversation,
                                                                                                                FlagNameList.mineLvl3MarcosAgreedToIgniteJelly
                                                                                                            }), spawnWhileHostile: spawnWhileHostile));

        #endregion

        #endregion
    
        #region Manse

        #region Manse-1F

        #region Manse-1F-Kitchens

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.kende),
                                        new InteractableSpawnParams(spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.imre+1), //Loyal Imre
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.convincedImre }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.imre+2), //Disloyal Imre
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.terrifiedImre }),
                                                                    stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.convincedImre }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.pan),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.convincedImre }), spawnWhileHostile: spawnWhileHostile));

        StopSpawningFlagList stopSpawningAfterKendeKitchenConvo = new StopSpawningFlagList(new string[] { FlagNameList.kendeUponEnteringKitchens });

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.guard),
                                        new InteractableSpawnParams(stopSpawningFlagList: stopSpawningAfterKendeKitchenConvo, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.noBrand),
                                        new InteractableSpawnParams(stopSpawningFlagList: stopSpawningAfterKendeKitchenConvo, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.noBrand+1),
                                        new InteractableSpawnParams(stopSpawningFlagList: stopSpawningAfterKendeKitchenConvo, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.noBrand+2),
                                        new InteractableSpawnParams(stopSpawningFlagList: stopSpawningAfterKendeKitchenConvo, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.noBrand+3),
                                        new InteractableSpawnParams(stopSpawningFlagList: stopSpawningAfterKendeKitchenConvo, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.noBrand+4),
                                        new InteractableSpawnParams(stopSpawningFlagList: stopSpawningAfterKendeKitchenConvo, spawnWhileHostile: spawnWhileHostile));
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.noBrand+5),
                                        new InteractableSpawnParams(stopSpawningFlagList: stopSpawningAfterKendeKitchenConvo, spawnWhileHostile: spawnWhileHostile));
        #endregion

        #region Manse-1F-1a

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, NPCNameList.crate),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[]{ FlagNameList.revoltStarted }), stopSpawningFlagList: new StopSpawningFlagList(new string[]{ FlagNameList.directorDefeated }), spawnWhileHostile: spawnWhileHostile));
        #endregion

        #region Manse-1F-2a
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, NPCNameList.orders),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { BookList.ordersTranscriptReadFlag }), spawnWhileHostile: spawnWhileHostile));
        #endregion

        #region Manse-1F-2c
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.section2c, NPCNameList.orders),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { BookList.pitSecondEntranceNoteReadFlag }), spawnWhileHostile: spawnWhileHostile));
        #endregion

        #region Manse-1F-3a
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.section3a, NPCNameList.orders),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { BookList.ordersTranscriptReadFlag }), spawnWhileHostile: spawnWhileHostile));
        #endregion

        #region Manse-1F-3b

        StopSpawningFlagList killedHorses = new StopSpawningFlagList(new string[] { FlagNameList.foughtHorsesInManse });

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.beam),
                                        new InteractableSpawnParams(stopSpawningFlagList: killedHorses, spawnWhileHostile: spawnWhileHostile));
        
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.csalan),
                                        new InteractableSpawnParams(stopSpawningFlagList: killedHorses, spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.horse),
                                        new InteractableSpawnParams(stopSpawningFlagList: killedHorses, spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.horse+1),
                                        new InteractableSpawnParams(stopSpawningFlagList: killedHorses, spawnWhileHostile: spawnWhileHostile));
                                    
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.horse+2),
                                        new InteractableSpawnParams(stopSpawningFlagList: killedHorses, spawnWhileHostile: spawnWhileHostile));
        #endregion

        #region Manse-1F-3d
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.section3d, NPCNameList.diary),
                                        new StatBasedSpawnParams(PrimaryStat.Wisdom, Constants.statLevelThree,
                                        stopSpawningFlagList: new StopSpawningFlagList(new string[] { BookList.pageFirstDiaryEntryReadFlag }), spawnWhileHostile: spawnWhileHostile));
        #endregion

        #region Manse-1F-33
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseFirstFloor + LocationNameList.section3e, NPCNameList.diary),
                                        new StatBasedSpawnParams(PrimaryStat.Wisdom, Constants.statLevelThree,
                                        stopSpawningFlagList: new StopSpawningFlagList(new string[] { BookList.pageSecondDiaryEntryReadFlag }), spawnWhileHostile: spawnWhileHostile));
        #endregion

        #endregion

        #region Manse-2F

        #region Manse-2F-2c

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseSecondFloor + LocationNameList.section2c, NPCNameList.chiefTabor),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.acceptedTaborsSurrenderAfterDirectorFight, FlagNameList.killedTaborInManse}),spawnWhileHostile: spawnWhileHostile));

        #endregion

        #region Manse-2F-Office
        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseSecondFloor + LocationNameList.office, NPCNameList.orders),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { BookList.ordersTranscriptReadFlag }), spawnWhileHostile: spawnWhileHostile));

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.manseSecondFloor + LocationNameList.office, NPCNameList.director),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.keptDirectorAlive}),spawnWhileHostile: spawnWhileHostile));
        #endregion

        #endregion

        #region Pit

        #region Pit-1b

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.pit + LocationNameList.section1b, NPCNameList.orders),
                                        new InteractableSpawnParams(stopSpawningFlagList: new StopSpawningFlagList(new string[] { BookList.pitClosureNoteReadFlag }),spawnWhileHostile: spawnWhileHostile));
        #endregion

        #region Pit-2b

        interactableSpawnParamsDict.Add(new KeyValuePair<string, string>(ZoneKeyList.pit + LocationNameList.section2b, NPCNameList.broglin),
                                        new InteractableSpawnParams(new StartSpawningFlagList(new string[] { FlagNameList.goesWithBroglinsPlan}), stopSpawningFlagList: new StopSpawningFlagList(new string[] { FlagNameList.freedBroglin}),spawnWhileHostile: spawnWhileHostile));

        #endregion

        #endregion

        #endregion
    }
    




}
