using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OOCSpawnDetailsList
{
    private const bool notActivated = false;
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

        list.Add(new NPCSpawnDetails(NPCNameList.guardLaszlo, new Vector3Int(3, 1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.guardLaszlo + 1, new Vector3Int(-2, -1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.garcha + 1, new Vector3Int(3, 1), notActivated));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackTwo, list);
        #endregion
        #region 3SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.janos, new Vector3Int(5, 3), LocationNameList.slaveShackThree));

        list.Add(new NPCSpawnDetails(NPCNameList.guardAndras, new Vector3Int(4, 1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.guardAndras + 1, new Vector3Int(6, 2), notActivated, LocationNameList.slaveShackThree));
        list.Add(new NPCSpawnDetails(NPCNameList.guardAndras + 2, new Vector3Int(6, 2), LocationNameList.slaveShackThree));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackThree, list);
        #endregion
        #region 4SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.kastor, new Vector3Int(11, 13), LocationNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.nandor, new Vector3Int(11, 11), LocationNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.carter, new Vector3Int(13, 11), LocationNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(13, 13), LocationNameList.slaveShackFour));
        // list.Add(new NPCSpawnDetails(NPCNameList.thatch, new Vector3Int(10, 12), LocationNameList.slaveShackFour));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackFour, list);
        #endregion
        #region 5SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.ervin, new Vector3Int(3, -2), LocationNameList.slaveShackFive));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackFive, list);
        #endregion
        #region 6SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.thatch, new Vector3Int(-1, 1), LocationNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.slate, new Vector3Int(9, 1), LocationNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.guardVazul, new Vector3Int(9, 0), LocationNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.rubble, new Vector3Int(-1, -3), LocationNameList.slaveShackSix));

        list.Add(new NPCSpawnDetails(NPCNameList.thatch + 1, new Vector3Int(6, -2), notActivated));

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

        list.Add(new CunningBlockerSpawnDetails(new Vector3Int(6, -1), Facing.SouthEast, Facing.NorthEast, CunningObjectSpriteCategory.Statue,
                 new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator, new Vector3Int(6, -2), PrefabNames.shackWallHalf),
                 TutorialSequenceList.tutorialCunningObjectTargetHash));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(0, -4), VaultableObject.vaultableBarrelsOneTile, TutorialSequenceList.vaultableBarrelsTargetHash));

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
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(3, -3), TutorialSequenceList.tutorialButtonOneTargetHash));
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(5, -3), TutorialSequenceList.tutorialButtonTwoTargetHash));

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

        list.Add(new NPCSpawnDetails(NPCNameList.crate, new Vector3Int(10, 4), LocationNameList.stockhouse));
        list.Add(new NPCSpawnDetails(NPCNameList.crate + 1, new Vector3Int(6, -1), LocationNameList.stockhouse));
        list.Add(new NPCSpawnDetails(NPCNameList.crate + 2, new Vector3Int(5, 3), LocationNameList.stockhouse));

        list.Add(new NPCSpawnDetails(NPCNameList.barrels, new Vector3Int(6, 5), LocationNameList.stockhouse));

        oocSpawnDetailsDict.Add(LocationNameList.stockhouse, list);
        #endregion

        #region NECamp
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -4), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -5), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -7), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(17, -8), LocationNameList.campNorthEast));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(5, 2), VaultableObject.vaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(-1, 3), VaultableObject.vaultableBarrelsOneTile));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(2, 4), Facing.SouthEast));

        oocSpawnDetailsDict.Add(LocationNameList.campNorthEast, list);
        #endregion
        #region CenterCamp
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.csalan, new Vector3Int(16, 14), LocationNameList.campCenter));

        list.Add(new NPCSpawnDetails(NPCNameList.temple, new Vector3Int(6, 7), LocationNameList.campCenter));

        list.Add(new NPCSpawnDetails(NPCNameList.guard + 1, new Vector3Int(5, 3), LocationNameList.campCenter));

        list.Add(new NPCSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(3, 2), LocationNameList.campCenter));
        list.Add(new NPCSpawnDetails(NPCNameList.branded, new Vector3Int(1, 0), LocationNameList.campCenter));
        list.Add(new NPCSpawnDetails(NPCNameList.branded, new Vector3Int(1, 1), LocationNameList.campCenter));
        list.Add(new NPCSpawnDetails(NPCNameList.branded, new Vector3Int(1, 3), LocationNameList.campCenter));
        list.Add(new NPCSpawnDetails(NPCNameList.feher, new Vector3Int(3, 1), LocationNameList.campCenter));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(-4, -7), VaultableObject.vaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(-11, 0), VaultableObject.vaultableBarrelsOneTile));

        oocSpawnDetailsDict.Add(LocationNameList.campCenter, list);
        #endregion
        #region SECamp
        list = new List<OOCSpawnDetails>();


        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(13, 11), Facing.SouthEast));

        oocSpawnDetailsDict.Add(LocationNameList.campSouthEast, list);
        #endregion
        #region MineEntranceCamp
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.guardMuzsa, new Vector3Int(8, 4), LocationNameList.campMineEntrance));

        list.Add(new ObstacleSpawnDetails(NPCNameList.barricade, new Vector3Int(8, 5), PrefabNames.squareCratesSmall));

        list.Add(new NPCSpawnDetails(NPCNameList.guardMuzsa + 1, new Vector3Int(6, 3), notActivated, LocationNameList.campMineEntrance));
        list.Add(new NPCSpawnDetails(NPCNameList.guardMuzsa + 2, new Vector3Int(6, 3), LocationNameList.campMineEntrance));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(13, 9), VaultableObject.vaultableBarrelsTwoTiles));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(12, 9), VaultableObject.vaultableBarrelsTwoTiles));

        oocSpawnDetailsDict.Add(LocationNameList.campMineEntrance, list);
        #endregion
        #region Camp Manse
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.imre, new Vector3Int(-7, -11), LocationNameList.campManse));

        oocSpawnDetailsDict.Add(LocationNameList.campManse, list); //3,9 / kitchen -6,5
        #endregion

        #region MineLvl_2

        #region MineLvl_2-1a

        list = new List<OOCSpawnDetails>();

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl2FirstSecretDoor, LocationNameList.mineLvl2, LocationNameList.section1a, Constants.indexOne));

        oocSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section1a, list);

        #endregion

        #region MineLvl_2-1b

        list = new List<OOCSpawnDetails>();

        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(8, 7), Constants.sizeTwo));
        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(7, 7), Constants.sizeTwo));

        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(5, 10), Constants.sizeOne));

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(11, 3), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(4, 5), Facing.SouthEast));

        oocSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_2-1c

        list = new List<OOCSpawnDetails>();

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(-6, 2), Facing.SouthEast));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(-3, 11), Facing.SouthWest));

        oocSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section1c, list);

        #endregion

        #region MineLvl_2-2b

        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(11, 4), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(11, -2), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexTwo, new Vector3Int(1, 5), Facing.SouthEast));
        list.Add(new ChestSpawnDetails(Constants.indexThree, new Vector3Int(1, -3), Facing.NorthWest));

        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(7, -5)));

        oocSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_2-3a

        list = new List<OOCSpawnDetails>();

        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(0, 1), Constants.sizeFour));
        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(-1, 1), Constants.sizeFour));
        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(0, -2), Constants.sizeFour));
        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(-1, -2), Constants.sizeFour));

        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(2, 8)));
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(2, 5)));

        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(9, 8), Constants.indexOne));
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(9, 5), Constants.indexOne));

        list.Add(new BookSpawnDetails(NPCNameList.diary, new Vector3Int(0, 13), PrefabNames.note, ItemList.mineGuardsDiaryIndex));

        oocSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section3a, list);

        #endregion

        #region MineLvl_2-3b

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(3, 5)));
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(-1, 5)));
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(-4, 5)));

        oocSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section3b, list);

        #endregion

        #region MineLvl_2-6

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(5, 10))); //b0 in A1
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(5, 2), Constants.indexOne)); // b1 in C1
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(9, 6), Constants.indexTwo)); // b2 in B2
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(9, 2), Constants.indexThree)); // b3 in C2
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(9, 10), Constants.indexFour)); // b4 in A2
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(1, 6), Constants.indexFive)); // b5 in S2
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(1, 10), Constants.indexSix)); // b6 in S1
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(13, 6), Constants.indexSeven)); // b7 in 7a

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(4, 5), Facing.SouthWest));

        oocSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section6, list);

        #endregion

        #region MineLvl_2-5

        list = new List<OOCSpawnDetails>();

        List<ObstacleSpawnDetails> blockerSpawnDetails = new List<ObstacleSpawnDetails>();
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(7, 8), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(6, 8), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(5, 8), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(7, 7), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(6, 7), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(7, 6), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(8, 7), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(8, 6), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(8, 5), PrefabNames.mineLvl2WallCunningObstacle));

        list.Add(new CunningBlockerSpawnDetails(new Vector3Int(8, 8), Facing.SouthWest, Facing.SouthEast, CunningObjectSpriteCategory.Statue, blockerSpawnDetails));

        blockerSpawnDetails = new List<ObstacleSpawnDetails>();
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(7, 11), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(6, 11), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(5, 11), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(7, 12), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(6, 12), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(7, 13), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(8, 12), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(8, 13), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(8, 14), PrefabNames.mineLvl2WallCunningObstacle));

        list.Add(new CunningBlockerSpawnDetails(new Vector3Int(8, 11), Facing.SouthWest, Facing.NorthWest, CunningObjectSpriteCategory.Statue, blockerSpawnDetails));

        blockerSpawnDetails = new List<ObstacleSpawnDetails>();
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(12, 8), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(13, 8), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(14, 8), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(12, 7), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(13, 7), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(12, 6), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(11, 7), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(11, 6), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(11, 5), PrefabNames.mineLvl2WallCunningObstacle));

        list.Add(new CunningBlockerSpawnDetails(new Vector3Int(11, 8), Facing.NorthEast, Facing.SouthEast, CunningObjectSpriteCategory.Statue, blockerSpawnDetails));
        
        blockerSpawnDetails = new List<ObstacleSpawnDetails>();
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(11, 12), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(11, 13), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(11, 14), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(12, 12), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(12, 13), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(13, 12), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(12, 11), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(13, 11), PrefabNames.mineLvl2WallCunningObstacle));
        blockerSpawnDetails.Add(new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(14, 11), PrefabNames.mineLvl2WallCunningObstacle));

        list.Add(new CunningBlockerSpawnDetails(new Vector3Int(11, 11), Facing.NorthEast, Facing.NorthWest, CunningObjectSpriteCategory.Statue, blockerSpawnDetails));

        oocSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_2-7a

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(-2, 7)));
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(4, 0), Constants.indexOne));
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(5, -2), Constants.indexTwo));
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(-3, -9), Constants.indexThree));

        oocSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section7a, list);

        #endregion

        #region MineLvl_2-7b

        list = new List<OOCSpawnDetails>();

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(5, -4), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(-3, 11), Facing.SouthEast));

        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(-1, -3), Constants.sizeOne));

        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(-9, -8)));
        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(-11, -8)));

        list.Add(new ButtonSpawnDetails(NPCNameList.floorButton, new Vector3Int(-9, -2), Constants.indexOne));

        oocSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section7b, list);

        #endregion

        #endregion
    }

}
