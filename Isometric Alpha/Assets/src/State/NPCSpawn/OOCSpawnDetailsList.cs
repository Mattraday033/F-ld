using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OOCSpawnDetailsList
{
    private const bool flipX = true;
    private static List<OOCSpawnDetails> list;

    private static Dictionary<string, List<OOCSpawnDetails>> oocSpawnDetailsDict;

    public static List<OOCSpawnDetails> getOOCSpawnDetails(string areaName)
    {
        if(!oocSpawnDetailsDict.ContainsKey(areaName))
        {
            return new List<OOCSpawnDetails>();
        }

        return oocSpawnDetailsDict[areaName];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeSpawnDetailsList()
    {

        oocSpawnDetailsDict = new Dictionary<string, List<OOCSpawnDetails>>();

        #region 1SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.balint, new Vector3Int(7, 1), LocationNameList.slaveShackOne));
        list.Add(new NPCSpawnDetails(NPCNameList.seb, new Vector3Int(6, 5), LocationNameList.slaveShackOne));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackOne, list);
        #endregion
        #region 2SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.broglin, new Vector3Int(4, 4), LocationNameList.slaveShackTwo, new BeginningConversationScript()));
        list.Add(new NPCSpawnDetails(NPCNameList.garcha, new Vector3Int(4, -1), LocationNameList.slaveShackTwo));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardLaszlo, new Vector3Int(3, 1)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardLaszlo + 1, new Vector3Int(-2, -1)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.garcha + 1, new Vector3Int(3, 1)));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackTwo, list);
        #endregion
        #region 3SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.janos, new Vector3Int(5, 3), LocationNameList.slaveShackThree));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardAndras, new Vector3Int(4, 1)));
        list.Add(new NPCSpawnDetails(NPCNameList.guardAndras + 1, new Vector3Int(2, 3), LocationNameList.slaveShackThree));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackThree, list);
        #endregion
        #region 4SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.kastor, new Vector3Int(11, 13), LocationNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.nandor, new Vector3Int(9, 15), LocationNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.carter, new Vector3Int(9, 13), LocationNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(11, 15), LocationNameList.slaveShackFour));
        // list.Add(new NPCSpawnDetails(NPCNameList.thatch, new Vector3Int(10, 12), LocationNameList.slaveShackFour));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackFour, list);
        #endregion
        #region 5SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.southEastCampWallPatchThree, LocationNameList.slaveShackFive, Constants.indexOne));

        list.Add(new NPCSpawnDetails(NPCNameList.ervin, new Vector3Int(3, -2), LocationNameList.slaveShackFive));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackFive, list);
        #endregion
        #region 6SlaveShack
        list = new List<OOCSpawnDetails>();
        
        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.southEastCampWallPatchOne, LocationNameList.slaveShackSix, Constants.indexOne));
        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.southEastCampWallPatchTwo, LocationNameList.slaveShackSix, Constants.indexTwo));
        
        list.Add(new NPCSpawnDetails(NPCNameList.thatch, new Vector3Int(-1, 1), LocationNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.slate, new Vector3Int(9, 1), LocationNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.guardVazul, new Vector3Int(9, 0), LocationNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.rubble, new Vector3Int(-1, -3), LocationNameList.slaveShackSix));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.thatch + 1, new Vector3Int(6, -2)));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(-1, -3), TutorialSequenceList.firstHostilityTutorialSequenceKey,
                                                                          TutorialSequenceList.firstHostitilityTutorialSeenFlag));

        #region Str Tutorial

        list.Add(new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.STRDesignator, new Vector3Int(3, -3), PrefabNames.shackWallHalf));
        list.Add(new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.STRDesignator, new Vector3Int(4, -3), PrefabNames.shackWallHalf));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(5, -3), TutorialSequenceList.intimidateTutorialSequenceKey,
                                                                          TutorialSequenceList.intimidateTutorialSeenFlag,
                         new StartSpawningAllTrueFlagList(new string[] { FlagNameList.choseStrengthAtStart })));


        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(5, -2), TutorialSequenceList.interactableRubbleTutorialSequenceKey,
                                                                          TutorialSequenceList.interactableObjectTutorialSeenFlag,
                        new StartSpawningAllTrueFlagList(new string[] {  FlagNameList.choseStrengthAtStart,
                                                                          TutorialSequenceList.intimidateTutorialSeenFlag}),
                                                                          Constants.indexOne));
        #endregion
        #region Dex Tutorial

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(-1, -4), TutorialSequenceList.vaultableObjectTutorialSequenceKey,
                                                                          TutorialSequenceList.interactableObjectTutorialSeenFlag,
                                new StartSpawningAllTrueFlagList(new string[] {  FlagNameList.choseDexterityAtStart,
                                                                          TutorialSequenceList.firstHostitilityTutorialSeenFlag}),
                                                                          Constants.indexZero));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(5, -3), TutorialSequenceList.firstCunningTutorialSequenceKey,
                                                                          TutorialSequenceList.cunningTutorialSeenFlag,
                                new StartSpawningAllTrueFlagList(new string[] { FlagNameList.choseDexterityAtStart })));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(5, -2), TutorialSequenceList.secondCunningTutorialSequenceKey,
                                                                          TutorialSequenceList.secondCunningTutorialSeenFlag,
                                new StartSpawningAllTrueFlagList(new string[] { TutorialSequenceList.cunningTutorialSeenFlag }),
                                                                          Constants.indexOne));

        list.Add(new CunningBlockerSpawnDetails(Constants.indexZero, new Vector3Int(6, -1), Facing.SouthEast, Facing.NorthEast, CunningObjectSpriteCategory.Statue,
                 new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator, new Vector3Int(6, -2), PrefabNames.shackWallHalf),
                 TutorialSequenceList.tutorialCunningObjectTargetHash));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(0, -4), VaultableObject.diffTwoVaultableBarrelsOneTile, TutorialSequenceList.vaultableBarrelsTargetHash));

        list.Add(new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator, new Vector3Int(3, -3), PrefabNames.shackWallHalf));
        list.Add(new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator, new Vector3Int(4, -3), PrefabNames.shackWallHalf));

        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.DEXDesignator, new Vector3Int(0, -6), PrefabNames.southDescendingRubble));
        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.DEXDesignator, new Vector3Int(1, -6), PrefabNames.blockRubble));
        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.DEXDesignator, new Vector3Int(0, -5), PrefabNames.northWestDescendingRubble));
        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.DEXDesignator, new Vector3Int(1, -5), PrefabNames.northWestDescendingRubble));
        #endregion
        #region Wis Tutorial

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(-1, -4), TutorialSequenceList.observationTutorialSequenceKey,
                                                                          TutorialSequenceList.observationTutorialSeenFlag,
                        new StartSpawningAllTrueFlagList(new string[] {   FlagNameList.choseWisdomAtStart,
                                                                          TutorialSequenceList.firstHostitilityTutorialSeenFlag}),
                                                                          Constants.indexZero));
        #endregion
        #region Cha Tutorial
        list.Add(new ButtonSpawnDetails(new Vector3Int(3, -3), TutorialSequenceList.tutorialButtonOneTargetHash));
        list.Add(new ButtonSpawnDetails(new Vector3Int(5, -3), TutorialSequenceList.tutorialButtonTwoTargetHash));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(3, -4), TutorialSequenceList.leadershipTutorialSequenceKey,
                                                                          TutorialSequenceList.leadershipTutorialSeenFlag,
                        new StartSpawningAllTrueFlagList(new string[] {   FlagNameList.choseCharismaAtStart,
                                                                          TutorialSequenceList.firstHostitilityTutorialSeenFlag}),
                                                                          Constants.indexZero));

        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.CHADesignator, new Vector3Int(0, -6), PrefabNames.southDescendingRubble));
        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.CHADesignator, new Vector3Int(1, -6), PrefabNames.blockRubble));
        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.CHADesignator, new Vector3Int(0, -5), PrefabNames.northWestDescendingRubble));
        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.CHADesignator, new Vector3Int(1, -5), PrefabNames.northWestDescendingRubble));

        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.CHADesignator, new Vector3Int(3, -5), PrefabNames.southWestDescendingRubble));
        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.CHADesignator, new Vector3Int(5, -5), PrefabNames.blockRubble));
        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.CHADesignator, new Vector3Int(4, -5), PrefabNames.blockRubble));
        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.CHADesignator, new Vector3Int(5, -4), PrefabNames.northWestDescendingRubble));
        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble + Constants.CHADesignator, new Vector3Int(4, -4), PrefabNames.northWestDescendingRubble));

        #endregion

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackSix, list);
        #endregion

        #region Mess Hall
        list = new List<OOCSpawnDetails>();

        list.Add(new ShopkeeperSpawnDetails(NPCNameList.kende, new Vector3Int(3, 10), LocationNameList.messHall, new Vector3Int[] { new Vector3Int(3, 9) }));

        oocSpawnDetailsDict.Add(LocationNameList.messHall, list);
        #endregion
        #region Stables
        list = new List<OOCSpawnDetails>();

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.centerCampWallPatchOne, LocationNameList.stables, Constants.indexOne));

        list.Add(new NPCSpawnDetails(NPCNameList.beam, new Vector3Int(5, 5), LocationNameList.stables));

        list.Add(new NPCSpawnDetails(NPCNameList.horse, new Vector3Int(3, -1), LocationNameList.stables));
        list.Add(new NPCSpawnDetails(NPCNameList.horse + 1, new Vector3Int(12, 9), LocationNameList.stables));
        list.Add(new NPCSpawnDetails(NPCNameList.horse + 2, new Vector3Int(3, 8), LocationNameList.stables));

        oocSpawnDetailsDict.Add(LocationNameList.stables, list);
        #endregion
        #region Stockhouse
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.uros, new Vector3Int(7, -1), LocationNameList.stockhouse));
        list.Add(new NPCSpawnDetails(NPCNameList.quartermasterEmese, new Vector3Int(11, 1), LocationNameList.stockhouse, new Vector3Int[] { new Vector3Int(10, 1) }));

        list.Add(new NPCSpawnDetails(NPCNameList.crate, new Vector3Int(10, 4), LocationNameList.stockhouse, PrefabNames.squareCratesSmall));
        list.Add(new NPCSpawnDetails(NPCNameList.crate + 1, new Vector3Int(5, 3), LocationNameList.stockhouse, PrefabNames.squareCratesSmall));

        list.Add(new NPCSpawnDetails(NPCNameList.barrels, new Vector3Int(6, 5), LocationNameList.stockhouse, PrefabNames.tripleBarrel));
        list.Add(new NPCSpawnDetails(NPCNameList.barrels + 1, new Vector3Int(6, -1), LocationNameList.stockhouse, PrefabNames.tripleBarrel));

        oocSpawnDetailsDict.Add(LocationNameList.stockhouse, list);
        #endregion
        #region Temple
        list = new List<OOCSpawnDetails>();

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.centerCampWallPatchTwo, LocationNameList.temple, Constants.indexOne));

        oocSpawnDetailsDict.Add(LocationNameList.temple, list);
        #endregion

        #region NECamp
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -4), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -5), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -7), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(17, -8), LocationNameList.campNorthEast));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(0, 3), VaultableObject.diffTwoVaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(6, 1), VaultableObject.diffTwoVaultableBarrelsOneTile));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(3, 5), Facing.SouthEast));

        #region Rallying Slaves Dialogue

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.carter, new Vector3Int(8, 5)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.nandor, new Vector3Int(8, 3)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.garcha, new Vector3Int(11, 4)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.janos, new Vector3Int(10, 6)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.clay, new Vector3Int(9, 2)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveOne, new Vector3Int(11, 3)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveTwo, new Vector3Int(11, 6)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveThree, new Vector3Int(8, 2)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveFour, new Vector3Int(9, 6)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(11, 2)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(11, 1)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(11, 0)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(11, -1)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, 2)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, 1)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, 0)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, -1)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, 1)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, 0)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, -1)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, 1)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, 0)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, -1)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(7, 1)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(5, 3)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.crowd, new Vector3Int(5, 2))); //Crowd
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(5, 1)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(5, 0)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(5, -1)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(4, 3)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(4, 2)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(4, 1)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(4, 0)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(3, 2)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(3, 2)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(11, 9)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(11, 8)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(11, 7)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, 10)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, 9)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, 8)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, 7)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, 10)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, 9)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, 8)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, 7)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, 9)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, 8)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, 7)));

        #endregion

        #region After Slaves Recruited

        list.Add(new NPCSpawnDetails(NPCNameList.slave+5, new Vector3Int(-2, 9), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.slave+6, new Vector3Int(6, 10), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.slave+7, new Vector3Int(-3, -8), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.slave+8, new Vector3Int(2, -3), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.slave+9, new Vector3Int(-6, -1), LocationNameList.campNorthEast));
        list.Add(new ShopkeeperSpawnDetails(NPCNameList.uros, new Vector3Int(-6, 2), LocationNameList.campNorthEast));

        list.Add(new NPCSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(11, 1), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.woundedSlave, new Vector3Int(11, 3), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.woundedSlave+1, new Vector3Int(11, 6), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.woundedSlave+2, new Vector3Int(11, 8), LocationNameList.campNorthEast));

        list.Add(new NPCSpawnDetails(NPCNameList.kastor, new Vector3Int(11, 2), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.garcha+1, new Vector3Int(-2, 1), LocationNameList.campNorthEast));

        #endregion

        oocSpawnDetailsDict.Add(LocationNameList.campNorthEast, list);
        #endregion
        #region CenterCamp
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.csalan, new Vector3Int(17, 17), LocationNameList.campCenter));

        list.Add(new NPCSpawnDetails(NPCNameList.temple, new Vector3Int(9, 11), LocationNameList.campCenter));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard + 1, new Vector3Int(6, 3), LocationNameList.campCenter));

        list.Add(new NPCSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(4, 5), LocationNameList.campCenter));
        list.Add(new NPCSpawnDetails(NPCNameList.branded, new Vector3Int(0, 6), LocationNameList.campCenter));
        list.Add(new NPCSpawnDetails(NPCNameList.branded, new Vector3Int(0, 4), LocationNameList.campCenter));
        list.Add(new NPCSpawnDetails(NPCNameList.branded, new Vector3Int(0, 3), LocationNameList.campCenter));
        list.Add(new NPCSpawnDetails(NPCNameList.feher, new Vector3Int(4, 4), LocationNameList.campCenter));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.carter, new Vector3Int(8, 0)));
        list.Add(new NPCSpawnDetails(NPCNameList.page, new Vector3Int(6, 1), LocationNameList.campSouthEast));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(-16, -6), VaultableObject.diffTwoVaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(-7, -10), VaultableObject.diffTwoVaultableBarrelsOneTile));

        oocSpawnDetailsDict.Add(LocationNameList.campCenter, list);
        #endregion
        #region SECamp
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCOffGridSpawnDetails(NPCNameList.statue, new Vector3Int(7, 3), LocationNameList.campSouthEast, PrefabNames.directorStatuePath, 
                                    new Vector3Int[] { 
                                                        new Vector3Int(7, 4),
                                                        new Vector3Int(8, 4),
                                                        new Vector3Int(8, 3)
                                                     }));

        list.Add(new NPCOffGridSpawnDetails(NPCNameList.toppledStatue, new Vector3Int(7, 3), LocationNameList.campSouthEast, PrefabNames.brokenDirectorStatuePath, 
                                    new Vector3Int[] { 
                                                        new Vector3Int(7, 4),
                                                        new Vector3Int(8, 4),
                                                        new Vector3Int(8, 3)
                                                     }));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(19, 14), Facing.SouthEast));

        #region Guard Punishment Scene

            #region Nameless Slaves
                list.Add(new NPCSpawnDetails(NPCNameList.slave+1, new Vector3Int(10, -3), LocationNameList.campSouthEast));
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, -3))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+4, new Vector3Int(8, -3)));  
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(7, -3)));  
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+6, new Vector3Int(6, -3)));  
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(4, -3)));  
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+5, new Vector3Int(3, -3)));  
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(2, -3))); 
                list.Add(new NPCSpawnDetails(NPCNameList.slave+3, new Vector3Int(1, -3), LocationNameList.campSouthEast));

                list.Add(new NPCSpawnDetails(NPCNameList.slave+2, new Vector3Int(11, -2), LocationNameList.campSouthEast));
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, -2))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, -2))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, -2))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(7, -2))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(6, -2))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(5, -2))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(4, -2))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(3, -2))); 
                list.Add(new NPCSpawnDetails(NPCNameList.slave+2, new Vector3Int(2, -2), LocationNameList.campSouthEast));

                list.Add(new NPCSpawnDetails(NPCNameList.slave+3, new Vector3Int(11, -1), LocationNameList.campSouthEast));
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, -1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, -1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, -1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(7, -1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(6, -1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(5, -1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.crowd, new Vector3Int(5, -1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(4, -1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(3, -1))); 
                list.Add(new NPCSpawnDetails(NPCNameList.slave+1, new Vector3Int(2, -1), LocationNameList.campSouthEast));

                list.Add(new NPCSpawnDetails(NPCNameList.slave+1, new Vector3Int(12, 0), LocationNameList.campSouthEast));
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(11, 0))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, 0))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, 0))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, 0))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(7, 0))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(6, 0))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(5, 0))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(4, 0))); 
                list.Add(new NPCSpawnDetails(NPCNameList.slave+3, new Vector3Int(3, 0), LocationNameList.campSouthEast));

                list.Add(new NPCSpawnDetails(NPCNameList.slave+2, new Vector3Int(12, 1), LocationNameList.campSouthEast));
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(11, 1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, 1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, 1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, 1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(7, 1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(6, 1))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(5, 1))); 
                list.Add(new NPCSpawnDetails(NPCNameList.slave+2, new Vector3Int(4, 1), LocationNameList.campSouthEast));

                list.Add(new NPCSpawnDetails(NPCNameList.slave+3, new Vector3Int(11, 2), LocationNameList.campSouthEast));
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(10, 2))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, 2))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, 2))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(7, 2))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(6, 2))); 
                list.Add(new NPCSpawnDetails(NPCNameList.slave+1, new Vector3Int(5, 2), LocationNameList.campSouthEast));

                list.Add(new NPCSpawnDetails(NPCNameList.slave+1, new Vector3Int(10, 3), LocationNameList.campSouthEast));
                list.Add(new NPCSpawnDetails(NPCNameList.slave+2, new Vector3Int(9, 3), LocationNameList.campSouthEast));
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(9, 3))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(8, 3))); 
                list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(7, 3))); 
                list.Add(new NPCSpawnDetails(NPCNameList.slave+3, new Vector3Int(6, 3), LocationNameList.campSouthEast));
            #endregion

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.clay, new Vector3Int(5, -3))); 


        list.Add(new NPCSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(9, -5), LocationNameList.campSouthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.guardReka, new Vector3Int(7, -5), LocationNameList.campSouthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.guardAndras, new Vector3Int(5, -5), LocationNameList.campSouthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.guardPazman, new Vector3Int(3, -5), LocationNameList.campSouthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(1, -5), LocationNameList.campSouthEast));

        list.Add(new NPCSpawnDetails(NPCNameList.garcha, new Vector3Int(6, -11), LocationNameList.campSouthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.broglin, new Vector3Int(5, -11), LocationNameList.campSouthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.kastor, new Vector3Int(2, -9), LocationNameList.campSouthEast));

        list.Add(new NPCSpawnDetails(NPCNameList.ervin, new Vector3Int(3, -10), LocationNameList.campSouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.ervin+1, new Vector3Int(2, -7))); 
        list.Add(new NPCSpawnDetails(NPCNameList.janos, new Vector3Int(8, -11), LocationNameList.campSouthEast));
        
        list.Add(new NPCSpawnDetails(NPCNameList.nandor, new Vector3Int(6, -9), LocationNameList.campSouthEast, new GuardPunishmentNandorStartScript())); // Nandor during guard punishment start convo
        list.Add(new NPCSpawnDetails(NPCNameList.nandor+1, new Vector3Int(6, -9), LocationNameList.campSouthEast)); // Nandor after guard punishment start convo
        list.Add(new NPCSpawnDetails(NPCNameList.carter, new Vector3Int(1, -11), LocationNameList.campSouthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.thatch, new Vector3Int(10, -9), LocationNameList.campSouthEast));


        #endregion

        oocSpawnDetailsDict.Add(LocationNameList.campSouthEast, list);
        #endregion
        #region MineEntranceCamp
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.guardMuzsa, new Vector3Int(9, 10), LocationNameList.campMineEntrance));

        list.Add(new ObstacleSpawnDetails(NPCNameList.barricade, new Vector3Int(9, 11), PrefabNames.squareCratesSmall));

        list.Add(new NPCSpawnDetails(NPCNameList.guardMuzsa + 1, new Vector3Int(7, 10), LocationNameList.campMineEntrance));

        list.Add(new NPCSpawnDetails(NPCNameList.uros, new Vector3Int(13, -1), LocationNameList.campMineEntrance));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(12, 13), VaultableObject.diffTwoVaultableBarrelsOneTile));

        oocSpawnDetailsDict.Add(LocationNameList.campMineEntrance, list);
        #endregion
        #region Camp Manse
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.imre, new Vector3Int(-6, -9), LocationNameList.campManse));
        list.Add(new NPCSpawnDetails(NPCNameList.imre+1, new Vector3Int(-8, 1), LocationNameList.campManse));

        oocSpawnDetailsDict.Add(LocationNameList.campManse, list); //3,9 / kitchen -6,5
        #endregion

        #region MineLvl_1

        #region MineLvl_1-1b

        list = new List<OOCSpawnDetails>();

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(13, 7), VaultableObject.diffTwoVaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(10, 9), VaultableObject.diffTwoVaultableBarrelsOneTile));

        list.Add(new ButtonSpawnDetails(new Vector3Int(6, 1)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(6, -1)));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_1-1c

        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(6, -1), Facing.SouthWest));
    
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(-1, 4), Facing.SouthEast));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1c, list);

        #endregion

        #endregion

        #region MineLvl_2

        #region MineLvl_2-1a

        list = new List<OOCSpawnDetails>();

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl2FirstSecretDoor, ZoneKeyList.mineLvl2, LocationNameList.section1a, Constants.indexOne));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(-1, -3), TutorialSequenceList.firstHostilityTutorialSequenceKey,
                                                                          TutorialSequenceList.firstHostitilityTutorialSeenFlag));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1a, list);

        #endregion

        #region MineLvl_2-1b

        list = new List<OOCSpawnDetails>();

        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(8, 7), Constants.difficultyTwo, Constants.sizeTwo));
        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(7, 7), Constants.difficultyTwo, Constants.sizeTwo));

        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(5, 10), Constants.difficultyTwo, Constants.sizeOne));

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(11, 3), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(4, 5), Facing.SouthEast));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_2-1c

        list = new List<OOCSpawnDetails>();

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(-6, 2), Facing.SouthEast));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(-3, 11), Facing.SouthWest));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1c, list);

        #endregion

        #region MineLvl_2-2a

        list = new List<OOCSpawnDetails>();

        list.Add(new CustomMouseHoverNPCSpawnDetails(NPCNameList.controlPanel, new Vector3Int(5, 3), ZoneKeyList.mineLvl2 + LocationNameList.section2a, PrefabNames.controlPanel, flipX, Constants.onTableHeightOffset*2));
        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(5, 7), Facing.SouthWest));

        list.Add(new NPCSpawnDetails(NPCNameList.guardPazman, new Vector3Int(-1, 4), ZoneKeyList.mineLvl2 + LocationNameList.section2a));

        list.Add(new NPCSpawnDetails(NPCNameList.guardReka, new Vector3Int(3, 9), ZoneKeyList.mineLvl2 + LocationNameList.section2a));

        list.Add(new NPCSpawnDetails(NPCNameList.guardVirag, new Vector3Int(3, 6), ZoneKeyList.mineLvl2 + LocationNameList.section2a));

        list.Add(new NPCSpawnDetails(NPCNameList.overseerGaspar, new Vector3Int(0, 9), ZoneKeyList.mineLvl2 + LocationNameList.section2a));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2a, list);

        #endregion

        #region MineLvl_2-2b

        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(11, 4), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(11, -2), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexTwo, new Vector3Int(1, 5), Facing.SouthEast));
        list.Add(new ChestSpawnDetails(Constants.indexThree, new Vector3Int(1, -3), Facing.NorthWest));

        list.Add(new ButtonSpawnDetails(new Vector3Int(7, -5)));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_2-3a

        list = new List<OOCSpawnDetails>();

        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(0, 1), Constants.difficultyTwo, Constants.sizeFour));
        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(-1, 1), Constants.difficultyTwo, Constants.sizeFour));
        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(0, -2), Constants.difficultyTwo, Constants.sizeFour));
        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(-1, -2), Constants.difficultyTwo, Constants.sizeFour));

        list.Add(new ButtonSpawnDetails(new Vector3Int(2, 8)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(2, 5)));

        list.Add(new ButtonSpawnDetails(new Vector3Int(9, 8), Constants.indexOne));
        list.Add(new ButtonSpawnDetails(new Vector3Int(9, 5), Constants.indexOne));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(8, 9), Facing.SouthEast));

        list.Add(new BookSpawnDetails(NPCNameList.diary, new Vector3Int(0, 13), PrefabNames.note, ItemList.mineGuardsDiaryIndex));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section3a, list);

        #endregion

        #region MineLvl_2-3b

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(new Vector3Int(3, 5)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-1, 5)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-4, 5)));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section3b, list);

        #endregion

        #region MineLvl_2-4

        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(7, 5), Facing.SouthWest));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section4, list);

        #endregion

        #region MineLvl_2-5

        list = new List<OOCSpawnDetails>();

        List<ObstacleSpawnDetails> blockerSpawnDetails = new List<ObstacleSpawnDetails>();
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(7, 8)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(6, 8)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(5, 8)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(7, 7)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(6, 7)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(7, 6)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(8, 7)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(8, 6)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(8, 5)));

        list.Add(new LinkedCunningBlockerSpawnDetails(Constants.indexZero, new Vector3Int(8, 8), Facing.SouthWest, Facing.SouthEast, CunningObjectSpriteCategory.Statue, blockerSpawnDetails, Constants.indexTwo));

        blockerSpawnDetails = new List<ObstacleSpawnDetails>();
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(7, 11)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(6, 11)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(5, 11)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(7, 12)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(6, 12)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(7, 13)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(8, 12)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(8, 13)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(8, 14)));

        list.Add(new LinkedCunningBlockerSpawnDetails(Constants.indexOne, new Vector3Int(8, 11), Facing.SouthWest, Facing.NorthWest, CunningObjectSpriteCategory.Statue, blockerSpawnDetails, Constants.indexThree));

        blockerSpawnDetails = new List<ObstacleSpawnDetails>();
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(12, 8)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(13, 8)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(14, 8)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(12, 7)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(13, 7)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(12, 6)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(11, 7)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(11, 6)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(11, 5)));

        list.Add(new LinkedCunningBlockerSpawnDetails(Constants.indexTwo, new Vector3Int(11, 8), Facing.NorthEast, Facing.SouthEast, CunningObjectSpriteCategory.Statue, blockerSpawnDetails, Constants.indexZero));
        
        blockerSpawnDetails = new List<ObstacleSpawnDetails>();
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(11, 12)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(11, 13)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(11, 14)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(12, 12)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(12, 13)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(13, 12)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(12, 11)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(13, 11)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(14, 11)));

        list.Add(new LinkedCunningBlockerSpawnDetails(Constants.indexThree, new Vector3Int(11, 11), Facing.NorthEast, Facing.NorthWest, CunningObjectSpriteCategory.Statue, blockerSpawnDetails, Constants.indexOne));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(8, 10), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(8, 9), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexTwo, new Vector3Int(6, 27), Facing.SouthEast));
        list.Add(new ChestSpawnDetails(Constants.indexThree, new Vector3Int(6, -9), Facing.NorthWest));
                                

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_2-6

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(new Vector3Int(5, 10))); //b0 in A1
        list.Add(new ButtonSpawnDetails(new Vector3Int(5, 2), Constants.indexOne)); // b1 in C1
        list.Add(new ButtonSpawnDetails(new Vector3Int(9, 6), Constants.indexTwo)); // b2 in B2
        list.Add(new ButtonSpawnDetails(new Vector3Int(9, 2), Constants.indexThree)); // b3 in C2
        list.Add(new ButtonSpawnDetails(new Vector3Int(9, 10), Constants.indexFour)); // b4 in A2
        list.Add(new ButtonSpawnDetails(new Vector3Int(1, 6), Constants.indexFive)); // b5 in S2
        list.Add(new ButtonSpawnDetails(new Vector3Int(1, 10), Constants.indexSix)); // b6 in S1
        list.Add(new ButtonSpawnDetails(new Vector3Int(13, 6), Constants.indexSeven)); // b7 in 7a

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(4, 5), Facing.SouthWest));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section6, list);

        #endregion

        #region MineLvl_2-7a

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(new Vector3Int(-2, 7)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(4, 0), Constants.indexOne));
        list.Add(new ButtonSpawnDetails(new Vector3Int(5, -2), Constants.indexTwo));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-3, -9), Constants.indexThree));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section7a, list);

        #endregion

        #region MineLvl_2-7b

        list = new List<OOCSpawnDetails>();

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(5, -4), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(-3, 11), Facing.SouthEast, new FoundWinch()));

        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(-1, -3), Constants.difficultyTwo, Constants.sizeOne));

        list.Add(new ButtonSpawnDetails(new Vector3Int(-9, -8)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-11, -8)));

        list.Add(new ButtonSpawnDetails(new Vector3Int(-9, -2), Constants.indexOne));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section7b, list);

        #endregion

        #endregion
    
        #region MineLvl_3

        #region MineLvl_3-1a

        list = new List<OOCSpawnDetails>();

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1a, list);

        #endregion

        #region MineLvl_3-1b

        list = new List<OOCSpawnDetails>();

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl3PuzzleDoor, ZoneKeyList.mineLvl3,  LocationNameList.section1b, Constants.indexOne));

        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(0, -6), PrefabNames.mineLvl3WallSecretDoor, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(1, -3), PrefabNames.mineLvl3WallSecretDoor, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(1, -4), PrefabNames.mineLvl3WallSecretDoor, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(1, -5), PrefabNames.mineLvl3WallSecretDoor, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(1, -6), PrefabNames.mineLvl3WallSecretDoor, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(2, -2), PrefabNames.mineLvl3WallSecretDoor, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(2, -3), PrefabNames.mineLvl3WallSecretDoor, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(2, -4), PrefabNames.mineLvl3WallSecretDoor, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(2, -5), PrefabNames.mineLvl3WallSecretDoor, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(2, -6), PrefabNames.mineLvl3WallSecretDoor, SecretDoorKeyList.mineLvl3PuzzleDoor));
        
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall+2, new Vector3Int(3, -1), PrefabNames.mineLvl3GroundSecretDoor, SortingLayerManager.secondSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall+2, new Vector3Int(3, -2), PrefabNames.mineLvl3GroundSecretDoor, SortingLayerManager.secondSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall+2, new Vector3Int(3, -3), PrefabNames.mineLvl3GroundSecretDoor, SortingLayerManager.secondSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall+2, new Vector3Int(3, -4), PrefabNames.mineLvl3GroundSecretDoor, SortingLayerManager.secondSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall+2, new Vector3Int(4, 0), PrefabNames.mineLvl3GroundSecretDoor, SortingLayerManager.secondSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall+2, new Vector3Int(4, -1), PrefabNames.mineLvl3GroundSecretDoor, SortingLayerManager.secondSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall+2, new Vector3Int(4, -2), PrefabNames.mineLvl3GroundSecretDoor, SortingLayerManager.secondSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall+2, new Vector3Int(4, -3), PrefabNames.mineLvl3GroundSecretDoor, SortingLayerManager.secondSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall+2, new Vector3Int(4, -4), PrefabNames.mineLvl3GroundSecretDoor, SortingLayerManager.secondSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleDoor));

        list.Add(new HiddenButtonSpawnDetails(new Vector3Int(14, 0), SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new HiddenButtonSpawnDetails(new Vector3Int(13, -4), Constants.indexOne, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new HiddenButtonSpawnDetails(new Vector3Int(9, -3), Constants.indexTwo, SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new HiddenButtonSpawnDetails(new Vector3Int(7, 1), Constants.indexThree, SecretDoorKeyList.mineLvl3PuzzleDoor));

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl3PuzzleFinished, ZoneKeyList.mineLvl3,  LocationNameList.section1b, Constants.indexTwo));

        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(new Vector3Int(12, -8), SecretDoorKeyList.mineLvl3PuzzleFinished));
  
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.water, new Vector3Int(11, -9), PrefabNames.water, SortingLayerManager.groundSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleFinished));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.water, new Vector3Int(11, -10), PrefabNames.water, SortingLayerManager.groundSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleFinished));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.water, new Vector3Int(11, -11), PrefabNames.water, SortingLayerManager.groundSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleFinished));
        

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(-5, -7), Facing.NorthWest));  
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(13, -11), Facing.SouthWest));  

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_3-2a

        list = new List<OOCSpawnDetails>();

        blockerSpawnDetails = new List<ObstacleSpawnDetails>();

        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(6, 4)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(6, 3)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(6, 2)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(6, 1)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(5, 3)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(5, 2)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(5, 1)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(5, 0)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(4, 2)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(4, 1)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(4, 0)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(3, 1)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(3, 0)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(2, 0)));

        List<ObstacleSpawnDetails> deactivatedblockerSpawnDetails = new List<ObstacleSpawnDetails>();

        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(5, 5)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(4, 5)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(3, 5)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(2, 5)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(4, 4)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(3, 4)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(2, 4)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(1, 4)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(3, 3)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(2, 3)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(1, 3)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(2, 2)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(1, 2)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(1, 1)));

        list.Add(new DoubleCunningBlockerSpawnDetails(Constants.indexZero, new Vector3Int(4, -1), Facing.SouthWest, Facing.SouthEast, CunningObjectSpriteCategory.Statue, blockerSpawnDetails, deactivatedblockerSpawnDetails));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(-1, -1), Facing.SouthEast));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(0, -2), Facing.SouthWest));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section2a, list);

        #endregion

        #region MineLvl_3-2b

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(new Vector3Int(2, 9)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(0, 9)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(4, 9)));

        list.Add(new ButtonSpawnDetails(new Vector3Int(4, 13), Constants.indexOne));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_3-3a

        list = new List<OOCSpawnDetails>();

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section3a, list);

        #endregion

        #region MineLvl_3-3b

        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.barricade, new Vector3Int(5, 11), ZoneKeyList.mineLvl3 + LocationNameList.section3b));

        list.Add(new NPCSpawnDetails(NPCNameList.guardPazman, new Vector3Int(3, 10), ZoneKeyList.mineLvl3 + LocationNameList.section3b));

        list.Add(new DependantSpawnDetails(NPCNameList.guardPazman+1, new Vector3Int(5, 10), ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.barricade));

        list.Add(new NPCSpawnDetails(NPCNameList.guardReka, new Vector3Int(2, 7), ZoneKeyList.mineLvl3 + LocationNameList.section3b));

        list.Add(new NPCSpawnDetails(NPCNameList.guardVirag, new Vector3Int(4, 5), ZoneKeyList.mineLvl3 + LocationNameList.section3b));

        list.Add(new NPCSpawnDetails(NPCNameList.overseerGaspar, new Vector3Int(6, 7), ZoneKeyList.mineLvl3 + LocationNameList.section3b));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(6, 6), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(6, 0), Facing.SouthEast));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section3b, list);

        #endregion

        #region MineLvl_3-4a

        list = new List<OOCSpawnDetails>();

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(2, 4), PrefabNames.lavaVaultableGapHalf, SortingLayerManager.buttonSortingLayerInfo, VaultableObject.diffThreeVaultableGap));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(2, 2), PrefabNames.lavaVaultableGapHalf, SortingLayerManager.groundSortingLayerInfo, VaultableObject.diffThreeVaultableGap));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(8, 9), Facing.SouthWest));  

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section4a, list);

        #endregion

        #region MineLvl_3-4b

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(Constants.sizeFive, new Vector3Int(-2, 16)));  

        list.Add(new ButtonSpawnDetails(new Vector3Int(0, -4)));  
        list.Add(new ButtonSpawnDetails(new Vector3Int(-2, -4)));  

        list.Add(new ButtonSpawnDetails(new Vector3Int(1, 13), Constants.indexOne));  
        list.Add(new ButtonSpawnDetails(new Vector3Int(3, 11), Constants.indexOne));  

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section4b, list);

        #endregion

        #region MineLvl_3-5

        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(20, 1), Facing.SouthWest));  

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_3-Miner Camp

        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.barricade, new Vector3Int(3, 3), ZoneKeyList.mineLvl3 + LocationNameList.minerCamp));

        list.Add(new NPCSpawnDetails(NPCNameList.carter, new Vector3Int(1, 2), ZoneKeyList.mineLvl3 + LocationNameList.minerCamp));

        list.Add(new DependantSpawnDetails(NPCNameList.carter+1, new Vector3Int(3, 2), ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.barricade));

        list.Add(new NPCSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(3, -1), ZoneKeyList.mineLvl3 + LocationNameList.minerCamp));

        list.Add(new NPCSpawnDetails(NPCNameList.nandor, new Vector3Int(4, 1), ZoneKeyList.mineLvl3 + LocationNameList.minerCamp));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, list);

        #endregion

        #region MineLvl_3-6a

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(new Vector3Int(-7, 15)));

        list.Add(new ButtonSpawnDetails(new Vector3Int(-4, 11)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-4, 10)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-4, 9)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-6, 10)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-6, 9)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-6, 8)));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(-6, 2), Facing.SouthWest));  
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(-3, 5), Facing.SouthEast));  

        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.water, new Vector3Int(-7, -3), PrefabNames.water, SortingLayerManager.groundSortingLayerInfo, SecretDoorKeyList.mineLvl3_6aUnstablePillarHiddenTerrain));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.water, new Vector3Int(-7, -4), PrefabNames.water, SortingLayerManager.groundSortingLayerInfo, SecretDoorKeyList.mineLvl3_6aUnstablePillarHiddenTerrain));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.water, new Vector3Int(-7, -5), PrefabNames.water, SortingLayerManager.groundSortingLayerInfo, SecretDoorKeyList.mineLvl3_6aUnstablePillarHiddenTerrain));

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl3_6aUnstablePillarHiddenTerrain, ZoneKeyList.mineLvl3,  LocationNameList.section6a, Constants.indexOne));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(2, 4), PrefabNames.stoneVaultableGap, Constants.onTableHeightOffset*10, SortingLayerManager.groundSortingLayerInfo, VaultableObject.diffThreeVaultableGap));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(0, 4), PrefabNames.stoneVaultableGap, Constants.onTableHeightOffset*10, SortingLayerManager.groundSortingLayerInfo, VaultableObject.diffThreeVaultableGap));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(-11, 2), PrefabNames.stoneVaultableGap, Constants.onTableHeightOffset*10, SortingLayerManager.groundSortingLayerInfo, VaultableObject.diffThreeVaultableGap));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(-13, 2), PrefabNames.stoneVaultableGap, Constants.onTableHeightOffset*10, SortingLayerManager.groundSortingLayerInfo, VaultableObject.diffThreeVaultableGap));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section6a, list);

        #endregion

        #region MineLvl_3-7

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(new Vector3Int(5, 2)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(4, 2)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(3, 2)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(2, -1)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(4, -3)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(5, -3)));

        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(new Vector3Int(-9, 1), SecretDoorKeyList.mineLvl3_7UnstablePillarHiddenTerrain));

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl3_7UnstablePillarHiddenTerrain, ZoneKeyList.mineLvl3,  LocationNameList.section7, Constants.indexOne));

        blockerSpawnDetails = new List<ObstacleSpawnDetails>();

        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-11, -4)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-12, -4)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-13, -4)));

        deactivatedblockerSpawnDetails = new List<ObstacleSpawnDetails>();

        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-11, -6)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-12, -6)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-13, -6)));

        list.Add(new DoubleCunningBlockerSpawnDetails(Constants.indexZero, new Vector3Int(-12, -5), Facing.SouthWest, Facing.SouthEast, CunningObjectSpriteCategory.Statue, blockerSpawnDetails, deactivatedblockerSpawnDetails));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(-5, -6), Facing.SouthWest));  

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(-10, -9), PrefabNames.lavaVaultableGapHalf, SortingLayerManager.buttonSortingLayerInfo, VaultableObject.diffThreeVaultableGap));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(-10, -11), PrefabNames.lavaVaultableGapHalf, SortingLayerManager.groundSortingLayerInfo, VaultableObject.diffThreeVaultableGap));

        list.Add(new NPCSpawnDetails(NPCNameList.rubble, new Vector3Int(-6, 9), ZoneKeyList.mineLvl3 + LocationNameList.section7, PrefabNames.lowStalagmite));
        list.Add(new NPCSpawnDetails(NPCNameList.rubble, new Vector3Int(-7, 9), ZoneKeyList.mineLvl3 + LocationNameList.section7, PrefabNames.lowStalagmite));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardPazman, new Vector3Int(-6, 6)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardReka, new Vector3Int(-8, 7)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardVirag, new Vector3Int(-7, 8)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.overseerGaspar, new Vector3Int(-6, 8)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.carter, new Vector3Int(-6, 3)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.nandor, new Vector3Int(-8, 2)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(-9, 3)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardMarcos+1, new Vector3Int(-6, 6)));

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl3_7PocketSealedRubble, ZoneKeyList.mineLvl3,  LocationNameList.section7, Constants.indexTwo));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section7, list);

        #endregion

        #endregion

        #region Manse-1F

        #region Manse-1F-Kitchens

        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.kende, new Vector3Int(0, 1), ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, new KendeInKitchenDuringRiotScript()));

        list.Add(new NPCSpawnDetails(NPCNameList.imre+1, new Vector3Int(0, -3), ZoneKeyList.manseFirstFloor + LocationNameList.kitchens)); //loyal imre

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.imre+2, new Vector3Int(0, 0))); //disloyal imre

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.pan, new Vector3Int(0, 2)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guard, new Vector3Int(0, 3)));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(-5, -1))); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveOne, new Vector3Int(-5, 0))); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveTwo, new Vector3Int(-5, 1)));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveThree, new Vector3Int(-5, 2))); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveFour, new Vector3Int(-5, 3))); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveFive, new Vector3Int(-5, 4))); 


        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, list);

        #endregion

        #region Manse-1F-3b

        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.beam, new Vector3Int(3, 9), ZoneKeyList.manseFirstFloor + LocationNameList.section3b, new BeamAndCsalanInManseScript()));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.csalan, new Vector3Int(5, 8))); 

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.horse, new Vector3Int(6, 4))); 

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.horse+1, new Vector3Int(3, 3)));  

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.horse+2, new Vector3Int(1, 6)));  

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, list);

        #endregion

        #endregion

        #region Manse-2F

        #region Manse-2F-2c

        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(3, 0), ZoneKeyList.manseSecondFloor + LocationNameList.section2c, new ChiefTaborManseSecondFloorScript()));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(3, -4), Facing.SouthWest, new KeyHalfScript()));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section2c, list);

        #endregion

        #region Manse-2F-Office
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.director, new Vector3Int(2, -1), ZoneKeyList.manseSecondFloor + LocationNameList.office));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.carter, new Vector3Int(-3, -3))); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.nandor, new Vector3Int(-3, 0))); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.page, new Vector3Int(2, 2)));

        list.Add(new BookSpawnDetails(NPCNameList.orders, new Vector3Int(-6, 0), PrefabNames.note, ItemList.orderTranscriptIndex));
        
        // list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.southEastCampWallPatchThree, ZoneKeyList.manseSecondFloor + LocationNameList.office, Constants.indexOne));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.office, list);

        #endregion

        #endregion

    }

}
