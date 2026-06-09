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

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.balint, new Vector3Int(7, 1), LocationNameList.slaveShackOne, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.seb, new Vector3Int(6, 5), LocationNameList.slaveShackOne, animationType: CharacterAnimationType.Death_Front_Weaponless));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackOne, list);
        #endregion
        #region 2SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.brush, new Vector3Int(4, 2), LocationNameList.slaveShackTwo, facing: Facing.SouthEast, speakAtStartScript: new BeginningConversationScript()));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.géza, new Vector3Int(4, 0), LocationNameList.slaveShackTwo, facing: Facing.NorthWest));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardLaszlo, new Vector3Int(2, 1), facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardLaszlo + 1, new Vector3Int(-2, -1), facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.géza + 1, new Vector3Int(2, 1), facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.géza + 2, new Vector3Int(6, -2), facing: Facing.SouthEast));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackTwo, list);
        #endregion
        #region 3SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.janos, new Vector3Int(5, 3), LocationNameList.slaveShackThree, facing: Facing.SouthEast));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardAndras, new Vector3Int(4, 1), facing: Facing.NorthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardAndras + 1, new Vector3Int(2, 3), LocationNameList.slaveShackThree, facing: Facing.SouthEast));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackThree, list);
        #endregion
        #region 4SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.kastor, new Vector3Int(11, 13), LocationNameList.slaveShackFour, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.nandor, new Vector3Int(9, 15), LocationNameList.slaveShackFour, facing: Facing.SouthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(10, 15), LocationNameList.slaveShackFour, facing: Facing.SouthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.carter, new Vector3Int(11, 15), LocationNameList.slaveShackFour, facing: Facing.SouthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardMarcos+1, new Vector3Int(11, 17), LocationNameList.slaveShackFour, facing: Facing.SouthEast, animationType: CharacterAnimationType.Death_Back, offset: Constants.onTableHeightOffset*-2f));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackFour, list);
        #endregion
        #region 5SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.southEastCampWallPatchThree, locationName: LocationNameList.slaveShackFive, index: Constants.indexOne));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.ervin, new Vector3Int(3, -2), LocationNameList.slaveShackFive, facing: Facing.SouthWest));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackFive, list);
        #endregion
        #region 6SlaveShack
        list = new List<OOCSpawnDetails>();
        
        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.southEastCampWallPatchOne, locationName: LocationNameList.slaveShackSix, index: Constants.indexOne));
        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.southEastCampWallPatchTwo, locationName: LocationNameList.slaveShackSix, index: Constants.indexTwo));
        
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.thatch, new Vector3Int(-1, 1), LocationNameList.slaveShackSix, facing: Facing.SouthEast, animationType: CharacterAnimationType.Death_Back_Weaponless, sleepingDialogueIntro: true));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slate, new Vector3Int(9, 1), LocationNameList.slaveShackSix, animationType: CharacterAnimationType.Death_Front_Weaponless));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardVazul, new Vector3Int(9, 0), LocationNameList.slaveShackSix, facing: Facing.NorthWest));
        list.Add(new RubbleObstacleSpawnDetails(NPCNameList.rubble, new Vector3Int(-1, -3), PrefabNames.tutorialRubble));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.thatch + 1, new Vector3Int(6, -2), facing: Facing.NorthEast));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(-1, -3), TutorialSequenceList.firstHostilityTutorialSequenceKey,
                                                                          TutorialSequenceList.firstHostilityTutorialSeenFlag));

        #region Str Tutorial

        list.Add(new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.STRDesignator, new Vector3Int(3, -3), PrefabNames.shackWallHalf));
        list.Add(new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.STRDesignator, new Vector3Int(4, -3), PrefabNames.shackWallHalf));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(5, -3), TutorialSequenceList.intimidateTutorialSequenceKey,
                                                                          TutorialSequenceList.intimidateTutorialSeenFlag,
                         new StartSpawningAllTrueFlagList(new string[] { FlagNameList.choseStrengthAtStart }, useTutorialFlags: true)));


        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(5, -2), TutorialSequenceList.interactableRubbleTutorialSequenceKey,
                                                                          TutorialSequenceList.interactableObjectTutorialSeenFlag,
                        new StartSpawningAllTrueFlagList(new string[] {  FlagNameList.choseStrengthAtStart,
                                                                          TutorialSequenceList.intimidateTutorialSeenFlag}, useTutorialFlags: true),
                                                                          Constants.indexOne));
        #endregion
        #region Dex Tutorial

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(-1, -4), TutorialSequenceList.vaultableObjectTutorialSequenceKey,
                                                                          TutorialSequenceList.interactableObjectTutorialSeenFlag,
                                new StartSpawningAllTrueFlagList(new string[] {  FlagNameList.choseDexterityAtStart,
                                                                          TutorialSequenceList.firstHostilityTutorialSeenFlag}, useTutorialFlags: true),
                                                                          Constants.indexZero));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(5, -3), TutorialSequenceList.firstCunningTutorialSequenceKey,
                                                                          TutorialSequenceList.cunningTutorialSeenFlag,
                                new StartSpawningAllTrueFlagList(new string[] { FlagNameList.choseDexterityAtStart }, useTutorialFlags: true)));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(5, -2), TutorialSequenceList.secondCunningTutorialSequenceKey,
                                                                          TutorialSequenceList.secondCunningTutorialSeenFlag,
                                new StartSpawningAllTrueFlagList(new string[] { TutorialSequenceList.cunningTutorialSeenFlag }, useTutorialFlags: true),
                                                                          Constants.indexOne));

        list.Add(new CunningBlockerSpawnDetails(Constants.indexZero, new Vector3Int(6, -1), Facing.SouthEast, CunningObjectSpriteCategory.Crank, endFacing: Facing.NorthWest, blockerSpawnDetails:
                 new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator, new Vector3Int(6, -2), PrefabNames.shackWallHalf),
                 tutorialTargetHash: TutorialSequenceList.tutorialCunningObjectTargetHash));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(0, -4), VaultableObject.diffTwoVaultableBarrelsOneTile, tutorialTargetHash: TutorialSequenceList.vaultableBarrelsTargetHash));

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
                                                                          TutorialSequenceList.firstHostilityTutorialSeenFlag}, useTutorialFlags: true),
                                                                          Constants.indexZero));
        #endregion
        #region Cha Tutorial
        list.Add(new ButtonSpawnDetails(new Vector3Int(3, -3), tutorialTargetHash: TutorialSequenceList.tutorialButtonOneTargetHash));
        list.Add(new ButtonSpawnDetails(new Vector3Int(5, -3), tutorialTargetHash: TutorialSequenceList.tutorialButtonTwoTargetHash));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(3, -4), TutorialSequenceList.leadershipTutorialSequenceKey,
                                                                          TutorialSequenceList.leadershipTutorialSeenFlag,
                        new StartSpawningAllTrueFlagList(new string[] {   FlagNameList.choseCharismaAtStart,
                                                                          TutorialSequenceList.firstHostilityTutorialSeenFlag}, useTutorialFlags: true),
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
        #region 7SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.dezso, new Vector3Int(6, 6), LocationNameList.slaveShackSeven, facing: Facing.SouthWest, speakAtStartScript: new DezsoLoamDialogueScript()));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.loam, new Vector3Int(5, 9), LocationNameList.slaveShackSeven, facing: Facing.SouthEast));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(9, 9), LocationNameList.slaveShackSeven, facing: Facing.SouthWest, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(3, 3), LocationNameList.slaveShackSeven, animationName: NPCNameList.slave+1, facing: Facing.NorthWest, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(7, 12), LocationNameList.slaveShackSeven, animationName: NPCNameList.slave+2, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(7, 3), LocationNameList.slaveShackSeven, animationName: NPCNameList.temple, facing: Facing.SouthWest, ignoresSecretDoors: false));

        list.Add(new DeadBodySpawnDetails(NPCNameList.slave+1, new Vector3Int(6, 6), NPCNameList.dezso+1, facing: Facing.SouthWest, ignoresSecretDoors: false));

        list.Add(new DeadBodySpawnDetails(NPCNameList.slave+1, new Vector3Int(9, 9), NPCNameList.slave, facing: Facing.NorthWest, ignoresSecretDoors: false));
        list.Add(new DeadBodySpawnDetails(NPCNameList.slave+1, new Vector3Int(3, 3), NPCNameList.slave+1, facing: Facing.NorthWest, ignoresSecretDoors: false));
        list.Add(new DeadBodySpawnDetails(NPCNameList.slave+1, new Vector3Int(7, 12), NPCNameList.slave+2, facing: Facing.NorthWest, ignoresSecretDoors: false));
        list.Add(new DeadBodySpawnDetails(NPCNameList.slave+1, new Vector3Int(7, 3), NPCNameList.slave+3, facing: Facing.NorthWest, ignoresSecretDoors: false));
        
        list.Add(new DeadBodySpawnDetails(NPCNameList.guard+4, new Vector3Int(6, 5), MonsterNameList.spearman, facing: Facing.SouthWest, weaponless: true, ignoresSecretDoors: false));
        list.Add(new DeadBodySpawnDetails(NPCNameList.guard+4, new Vector3Int(6, 7), MonsterNameList.axeman, facing: Facing.SouthWest, weaponless: true, ignoresSecretDoors: false));
        
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+4, new Vector3Int(6, 4), LocationNameList.slaveShackSeven, animationName: MonsterNameList.spearman, facing: Facing.NorthWest, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+4, new Vector3Int(4, 3), LocationNameList.slaveShackSeven, animationName: MonsterNameList.axeman, facing: Facing.SouthWest, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+4, new Vector3Int(6, 12), LocationNameList.slaveShackSeven, animationName: MonsterNameList.disciplinarian, facing: Facing.NorthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+4, new Vector3Int(3, 10), LocationNameList.slaveShackSeven, animationName: MonsterNameList.spearman, facing: Facing.NorthWest, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+4, new Vector3Int(9, 8), LocationNameList.slaveShackSeven, animationName: MonsterNameList.axeman, facing: Facing.NorthWest, ignoresSecretDoors: false));
        
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+1, new Vector3Int(6, 5), LocationNameList.slaveShackSeven, animationName: MonsterNameList.spearman, animationType: CharacterAnimationType.Secondary_Idle, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+2, new Vector3Int(6, 7), LocationNameList.slaveShackSeven, animationName: MonsterNameList.axeman, animationType: CharacterAnimationType.Secondary_Idle, facing: Facing.SouthWest));

        list.Add(new DeadBodySpawnDetails(NPCNameList.guard+3, new Vector3Int(3, 11), MonsterNameList.executioner, facing: Facing.SouthWest, weaponless: true));
        
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.weft, new Vector3Int(3, 7), LocationNameList.slaveShackSeven, facing: Facing.NorthEast, speakAtStartScript: new WeftAfterHostageCombatScript()));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guard, new Vector3Int(-1, 6)));
        
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft+1, new Vector3Int(5, 0), animationType: CharacterAnimationType.Death_Front_Weaponless, facing: Facing.NorthEast));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackSeven, list);
        #endregion
        #region 8SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.weft, new Vector3Int(3, -5), LocationNameList.slaveShackEight, facing: Facing.NorthWest, speakAtStartScript: new WeftIntroScript()));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.overseer, new Vector3Int(3, -6), facing: Facing.NorthWest));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackEight, list);
        #endregion

        #region Guard Shack
        list = new List<OOCSpawnDetails>();

        // list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardLaszlo, new Vector3Int(12, 4), LocationNameList.guardShack, facing: Facing.NorthEast));

        oocSpawnDetailsDict.Add(LocationNameList.guardShack, list);
        #endregion
        #region GuardHouse Top Floor
        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(6, -1), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(-5, -4), Facing.SouthEast));
        list.Add(new ChestSpawnDetails(Constants.indexTwo, new Vector3Int(-13, -1), Facing.SouthWest));

        list.Add(new LadderSpawnDetails(new Vector3Int(7, -4), PrefabNames.ladderTallSW,
                                        new Ladder(Constants.difficultyTwo, LocationNameList.guardHouseTopFloor, LocationNameList.campManse, 
                                                    Ladder.barracksLadderDescription, Facing.SouthWest)));

        oocSpawnDetailsDict.Add(LocationNameList.guardHouseTopFloor, list);
        #endregion
        #region GuardHouse SW
        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(-5, 1), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexTwo, new Vector3Int(-5,2), Facing.SouthWest));
        list.Add(new WeaponRackSpawnDetails(Constants.indexZero, new Vector3Int(-6,3), Facing.SouthEast, ChestType.ShovelRack, script: new FoundToolBundle()));
        list.Add(new WeaponRackSpawnDetails(Constants.indexThree, new Vector3Int(-7,3), Facing.SouthEast, ChestType.SpearRack));

        if(Application.isEditor)
        {
            list.Add(new ChestSpawnDetails(Constants.indexFour, new Vector3Int(-7, -2), Facing.NorthWest));
        }

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard, new Vector3Int(-8, -2), LocationNameList.guardHouseSouthWest, MonsterNameList.spearman, Facing.NorthWest));

        oocSpawnDetailsDict.Add(LocationNameList.guardHouseSouthWest, list);
        #endregion
        #region Mess Hall
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.noBrand+1, new Vector3Int(3, 13), LocationNameList.messHall, facing: Facing.SouthEast));

        list.Add(new ShopkeeperSpawnDetails(NPCNameList.kende, new Vector3Int(3, 10), LocationNameList.messHall, extraSpaces: new Vector3Int[] { new Vector3Int(3, 9) }, facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft, new Vector3Int(4,8), facing: Facing.NorthWest));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(4, 13), Facing.SouthWest));

        oocSpawnDetailsDict.Add(LocationNameList.messHall, list);
        #endregion
        #region Stables
        list = new List<OOCSpawnDetails>();

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.centerCampWallPatchOne, locationName: LocationNameList.stables, index: Constants.indexOne));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.beam, new Vector3Int(5, 5), LocationNameList.stables, facing: Facing.SouthEast));

        list.Add(new HorseSpawnDetails(NPCNameList.horse, new Vector3Int(3, -1), Facing.NorthWest, LocationNameList.stables));
        list.Add(new HorseSpawnDetails(NPCNameList.horse + 1, new Vector3Int(12, 9), Facing.SouthEast, LocationNameList.stables));
        list.Add(new HorseSpawnDetails(NPCNameList.horse + 2, new Vector3Int(3, 8), Facing.SouthEast, LocationNameList.stables));

        oocSpawnDetailsDict.Add(LocationNameList.stables, list);
        #endregion
        #region Stockhouse
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.uros, new Vector3Int(7, -1), LocationNameList.stockhouse, facing: Facing.SouthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.quartermasterEmese, new Vector3Int(11, 1), LocationNameList.stockhouse, facing: Facing.SouthWest, extraSpaces: new Vector3Int[] { new Vector3Int(10, 1) }));

        list.Add(new NPCSpawnDetails(NPCNameList.crate, new Vector3Int(10, 4), LocationNameList.stockhouse, PrefabNames.squareCratesSmall));
        list.Add(new NPCSpawnDetails(NPCNameList.crate + 1, new Vector3Int(5, 3), LocationNameList.stockhouse, PrefabNames.squareCratesSmall));

        list.Add(new NPCSpawnDetails(NPCNameList.barrels, new Vector3Int(6, 5), LocationNameList.stockhouse, PrefabNames.tripleBarrel)); //, withScale: false
        list.Add(new NPCSpawnDetails(NPCNameList.barrels + 1, new Vector3Int(6, -1), LocationNameList.stockhouse, PrefabNames.tripleBarrel)); //, withScale: false

        oocSpawnDetailsDict.Add(LocationNameList.stockhouse, list);
        #endregion
        #region Temple
        list = new List<OOCSpawnDetails>();

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.centerCampWallPatchTwo, locationName: LocationNameList.temple, index: Constants.indexOne));

        oocSpawnDetailsDict.Add(LocationNameList.temple, list);
        #endregion
        #region Body Pile

        list = new List<OOCSpawnDetails>();

        list.Add(new LadderSpawnDetails(new Vector3Int(-9, -10), PrefabNames.ladderTallNE,
                                new Ladder(Constants.noDifficulty, LocationNameList.bodyPile, LocationNameList.campNorthWest, 
                                            Ladder.bodyPileExitLadderDescription, Facing.NorthWest), sortingLayerInfo: SortingLayerManager.firstSortingLayerInfo));

        list.Add(new DeadBodySpawnDetails(NPCNameList.slave, new Vector3Int(0, 2), NPCNameList.slave, facing: Facing.NorthEast, weaponless: true));
        list.Add(new DeadBodySpawnDetails(NPCNameList.slave, new Vector3Int(-3, -3), NPCNameList.seb, facing: Facing.SouthWest, weaponless: true));
        list.Add(new DeadBodySpawnDetails(NPCNameList.slave, new Vector3Int(-4, -7), NPCNameList.slave, facing: Facing.NorthWest, weaponless: true));
        list.Add(new DeadBodySpawnDetails(NPCNameList.slave, new Vector3Int(-3, -9), NPCNameList.slave+1, facing: Facing.NorthEast, weaponless: true));
        list.Add(new DeadBodySpawnDetails(NPCNameList.slave, new Vector3Int(-1, -8), NPCNameList.slave, facing: Facing.NorthWest, weaponless: true));
        list.Add(new DeadBodySpawnDetails(NPCNameList.slave, new Vector3Int(-7, -8), NPCNameList.seb, facing: Facing.SouthWest, weaponless: true));
        list.Add(new DeadBodySpawnDetails(NPCNameList.slave, new Vector3Int(-9, -6), NPCNameList.slave+2, facing: Facing.NorthWest, weaponless: true));
        list.Add(new DeadBodySpawnDetails(NPCNameList.slave, new Vector3Int(1, -4), NPCNameList.slave+1, facing: Facing.NorthEast, weaponless: true));
        list.Add(new DeadBodySpawnDetails(NPCNameList.slave, new Vector3Int(4, -4), NPCNameList.slave+3, facing: Facing.NorthWest, weaponless: true));

        list.Add(new SingleSpriteChestSpawnDetails(Constants.indexZero, NPCNameList.body, new Vector3Int(-6, -5), Facing.NorthEast, NPCNameList.slave+3, deadBody: true, weaponless: true));
        list.Add(new SingleSpriteChestSpawnDetails(Constants.indexOne, NPCNameList.body, new Vector3Int(-2, -7), Facing.NorthWest, NPCNameList.slave+2, deadBody: true, weaponless: true));
        list.Add(new SingleSpriteChestSpawnDetails(Constants.indexTwo, NPCNameList.body, new Vector3Int(2, 0), Facing.SouthWest, NPCNameList.seb, deadBody: true, weaponless: true));
        list.Add(new SingleSpriteChestSpawnDetails(Constants.indexThree, NPCNameList.body+1,  new Vector3Int(5, -2), Facing.NorthEast, NPCNameList.slave+1, deadBody: true, weaponless: true, script: new ThiefsBodyScript()));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(2, -2), facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft, new Vector3Int(4, -3), facing: Facing.SouthWest));

        oocSpawnDetailsDict.Add(LocationNameList.bodyPile, list);
        #endregion

        #region NECamp
        list = new List<OOCSpawnDetails>();

        #region Chief Tabor + Adela "Situation" quest

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(-8, -9), LocationNameList.campNorthEast, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.captainAdela, new Vector3Int(-9, -9), LocationNameList.campNorthEast, facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft, new Vector3Int(-8, -11), facing: Facing.NorthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+3, new Vector3Int(-4, -11), LocationNameList.campNorthEast, animationName: MonsterNameList.spearman, animationType: CharacterAnimationType.Idle_Back, facing: Facing.NorthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+2, new Vector3Int(-5, -12), LocationNameList.campNorthEast, animationName: MonsterNameList.javelineer, animationType: CharacterAnimationType.Idle_Back, facing: Facing.NorthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+3, new Vector3Int(-4, -13), LocationNameList.campNorthEast, animationName: MonsterNameList.spearman, animationType: CharacterAnimationType.Idle_Back, facing: Facing.NorthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+3, new Vector3Int(-3, -14), LocationNameList.campNorthEast, animationName: MonsterNameList.axeman, animationType: CharacterAnimationType.Idle_Back, facing: Facing.NorthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.honorguard+1, new Vector3Int(-3, -8), LocationNameList.campNorthEast, animationName: MonsterNameList.linebreaker, facing: Facing.SouthWest, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+1, new Vector3Int(-12, -8), LocationNameList.campNorthEast, animationName: MonsterNameList.spearman, facing: Facing.SouthEast, ignoresSecretDoors: false));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.honorguard+1, new Vector3Int(-7, -16), LocationNameList.campNorthEast, animationName: MonsterNameList.lieutenant, facing: Facing.SouthWest, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+1, new Vector3Int(-8, -17), LocationNameList.campNorthEast, animationName: MonsterNameList.signaleer, facing: Facing.NorthWest, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+1, new Vector3Int(-9, -16), LocationNameList.campNorthEast, animationName: MonsterNameList.axeman, facing: Facing.NorthEast, ignoresSecretDoors: false));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+1, new Vector3Int(-12, -13), LocationNameList.campNorthEast, animationName: MonsterNameList.disciplinarian, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.honorguard+1, new Vector3Int(-12, -14), LocationNameList.campNorthEast, animationName: MonsterNameList.lancer, facing: Facing.NorthWest, ignoresSecretDoors: false));

        #endregion

        list.Add(new HostilityTerrainSpawnDetails(LocationNameList.campNorthEast, Constants.indexZero));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.overseer, new Vector3Int(-11, 6), LocationNameList.campNorthEast, facing: Facing.SouthEast));

        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -4), LocationNameList.campNorthEast, spriteName: PrefabNames.leafPile));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -5), LocationNameList.campNorthEast, spriteName: PrefabNames.leafPile));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -7), LocationNameList.campNorthEast, spriteName: PrefabNames.leafPile));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(17, -8), LocationNameList.campNorthEast, spriteName: PrefabNames.leafPile));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(0, 3), VaultableObject.diffTwoVaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(6, 1), VaultableObject.diffTwoVaultableBarrelsOneTile));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(3, 5), Facing.SouthEast));

        #region Overseer Fight
        
        list.Add(new NPCWithAnimationsSpawnDetails(MonsterNameList.brandedConscript, new Vector3Int(-3, 5), LocationNameList.campNorthEast, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(MonsterNameList.brandedConscript, new Vector3Int(-6, 4), LocationNameList.campNorthEast, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(MonsterNameList.spearman, new Vector3Int(-2, 4), LocationNameList.campNorthEast, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(MonsterNameList.brandedConscript, new Vector3Int(-4, 3), LocationNameList.campNorthEast, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(MonsterNameList.brandedConscript, new Vector3Int(-2, 2), LocationNameList.campNorthEast, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(MonsterNameList.brandedConscript, new Vector3Int(-4, 1), LocationNameList.campNorthEast, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(MonsterNameList.brandedConscript, new Vector3Int(-4, -1), LocationNameList.campNorthEast, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(MonsterNameList.brandedConscript, new Vector3Int(-6, -1), LocationNameList.campNorthEast, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(MonsterNameList.spearman, new Vector3Int(-2, -2), LocationNameList.campNorthEast, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(MonsterNameList.brandedConscript, new Vector3Int(-5, -4), LocationNameList.campNorthEast, facing: Facing.SouthWest));

        #endregion

        #region Rallying Slaves Dialogue

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.carter, new Vector3Int(8, 5), facing: Facing.NorthEast, ignoresSecretDoors: false));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.nandor, new Vector3Int(8, 3), facing: Facing.NorthEast, ignoresSecretDoors: false));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.géza, new Vector3Int(11, 4), facing: Facing.SouthWest, ignoresSecretDoors: false));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.janos, new Vector3Int(10, 6), facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.clay, new Vector3Int(9, 2), facing: Facing.NorthWest, ignoresSecretDoors: false));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveOne, new Vector3Int(11, 3), ignoresSecretDoors: false, facing: Facing.SouthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveTwo, new Vector3Int(11, 6), ignoresSecretDoors: false, facing: Facing.SouthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveThree, new Vector3Int(8, 2), ignoresSecretDoors: false, facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slaveFour, new Vector3Int(9, 6), ignoresSecretDoors: false, facing: Facing.SouthEast));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.temple, new Vector3Int(11, 2), ignoresSecretDoors: false, facing: Facing.SouthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.kastor+1, new Vector3Int(11, 1), ignoresSecretDoors: false, facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+12, new Vector3Int(11, 0), ignoresSecretDoors: false, facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.balint, new Vector3Int(11, -1), ignoresSecretDoors: false, facing: Facing.NorthWest));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+10, new Vector3Int(10, 2), ignoresSecretDoors: false, facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.temple, new Vector3Int(10, 1), ignoresSecretDoors: false, facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+12, new Vector3Int(10, 0), ignoresSecretDoors: false, facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.clay, new Vector3Int(10, -1), ignoresSecretDoors: false, facing: Facing.NorthWest));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.balint, new Vector3Int(9, 1), ignoresSecretDoors: false, facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.ervin, new Vector3Int(9, 0), ignoresSecretDoors: false, facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.thatch, new Vector3Int(9, -1), ignoresSecretDoors: false, facing: Facing.NorthWest));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.uros+1, new Vector3Int(8, 1), ignoresSecretDoors: false, facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+10, new Vector3Int(8, 0), ignoresSecretDoors: false, facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.temple, new Vector3Int(8, -1), ignoresSecretDoors: false, facing: Facing.NorthWest));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+12, new Vector3Int(7, 1), ignoresSecretDoors: false, facing: Facing.NorthWest));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.ervin, new Vector3Int(5, 3), ignoresSecretDoors: false, facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.crowd, new Vector3Int(5, 2), ignoresSecretDoors: false, facing: Facing.NorthEast)); //Crowd
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+10, new Vector3Int(5, 1), ignoresSecretDoors: false, facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.uros+1, new Vector3Int(5, 0), ignoresSecretDoors: false, facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.kastor+1, new Vector3Int(5, -1), ignoresSecretDoors: false, facing: Facing.NorthEast));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.thatch, new Vector3Int(4, 3), ignoresSecretDoors: false, facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+10, new Vector3Int(4, 2), ignoresSecretDoors: false, facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+11, new Vector3Int(4, 1), ignoresSecretDoors: false, facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+12, new Vector3Int(4, 0), ignoresSecretDoors: false, facing: Facing.NorthEast));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.kastor+1, new Vector3Int(3, 2), ignoresSecretDoors: false, facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+10, new Vector3Int(3, 1), ignoresSecretDoors: false, facing: Facing.NorthEast));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.balint, new Vector3Int(11, 9), ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.ervin, new Vector3Int(11, 8), ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+10, new Vector3Int(11, 7), ignoresSecretDoors: false, facing: Facing.SouthEast));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+11, new Vector3Int(10, 10), ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.uros+1, new Vector3Int(10, 9), ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+10, new Vector3Int(10, 8), ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.thatch, new Vector3Int(10, 7), ignoresSecretDoors: false, facing: Facing.SouthEast));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+12, new Vector3Int(9, 10), ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.ervin, new Vector3Int(9, 9), ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+11, new Vector3Int(9, 8), ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+12, new Vector3Int(9, 7), ignoresSecretDoors: false, facing: Facing.SouthEast));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.balint, new Vector3Int(8, 9), ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave+11, new Vector3Int(8, 8), ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.thatch, new Vector3Int(8, 7), ignoresSecretDoors: false, facing: Facing.SouthEast));

        #endregion

        #region After Slaves Recruited

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.temple+1, new Vector3Int(-2, 9), LocationNameList.campNorthEast, ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+6, new Vector3Int(6, 10), LocationNameList.campNorthEast, ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+7, new Vector3Int(-3, -8), LocationNameList.campNorthEast, ignoresSecretDoors: false, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+8, new Vector3Int(2, -3), LocationNameList.campNorthEast, ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.clay+1, new Vector3Int(-6, -1), LocationNameList.campNorthEast, ignoresSecretDoors: false, facing: Facing.SouthWest));
        list.Add(new ShopkeeperSpawnDetails(NPCNameList.uros, new Vector3Int(-6, 2), LocationNameList.campNorthEast, ignoresSecretDoors: false, facing: Facing.SouthEast));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(11, 1), LocationNameList.campNorthEast, facing: Facing.SouthWest, ignoresSecretDoors: false, animationType: CharacterAnimationType.Death_Back, offset: Constants.onTableHeightOffset*-2f, sleepingDialogueIntro: true));
        list.Add(new ObstacleSpawnDetails(NPCNameList.bed, new Vector3Int(11, 1), PrefabNames.slaveBed, offset: Constants.onTableHeightOffset*-3f, flipX: true, ignoresSecretDoors: false));  
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.woundedSlave, new Vector3Int(11, 3), LocationNameList.campNorthEast, ignoresSecretDoors: false, facing: Facing.SouthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.woundedSlave+1, new Vector3Int(11, 6), LocationNameList.campNorthEast, ignoresSecretDoors: false, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.woundedSlave+2, new Vector3Int(11, 8), LocationNameList.campNorthEast, ignoresSecretDoors: false, facing: Facing.NorthEast, animationType: CharacterAnimationType.Death_Back, offset: Constants.onTableHeightOffset*-2.5f, sleepingDialogueIntro: true));
        list.Add(new ObstacleSpawnDetails(NPCNameList.bed, new Vector3Int(11, 8), PrefabNames.slaveBed, offset: Constants.onTableHeightOffset*-3f, flipX: true, ignoresSecretDoors: false));  


        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.kastor, new Vector3Int(11, 2), LocationNameList.campNorthEast, facing: Facing.NorthWest, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.géza+1, new Vector3Int(-2, 1), LocationNameList.campNorthEast, facing: Facing.SouthWest, ignoresSecretDoors: false));

        #endregion

        oocSpawnDetailsDict.Add(LocationNameList.campNorthEast, list);
        #endregion
        #region CenterCamp
        list = new List<OOCSpawnDetails>();

        list.Add(new HostilityTerrainSpawnDetails(LocationNameList.campCenter, Constants.indexZero));

        list.Add(new HorseSpawnDetails(NPCNameList.csalan, new Vector3Int(17, 17), Facing.SouthEast, LocationNameList.campCenter));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.temple, new Vector3Int(9, 11), LocationNameList.campCenter, facing: Facing.SouthEast));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard + 1, new Vector3Int(6, 3), LocationNameList.campCenter, MonsterNameList.spearman, facing: Facing.SouthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(4, 5), LocationNameList.campCenter, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.branded, new Vector3Int(0, 6), LocationNameList.campCenter, animationName: NPCNameList.slaveTwo, facing: Facing.NorthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.branded+1, new Vector3Int(0, 4), LocationNameList.campCenter, animationName: NPCNameList.ervin, facing: Facing.NorthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.branded+2, new Vector3Int(0, 3), LocationNameList.campCenter, animationName: NPCNameList.slaveThree, facing: Facing.NorthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.feher, new Vector3Int(4, 4), LocationNameList.campCenter, facing: Facing.SouthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft, new Vector3Int(2, 6), facing: Facing.NorthEast));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.carter, new Vector3Int(8, -1), facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.page, new Vector3Int(5, 0), LocationNameList.campCenter, facing: Facing.SouthEast));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(-16, -6), VaultableObject.diffTwoVaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(-7, -10), VaultableObject.diffTwoVaultableBarrelsOneTile));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricadeGuards+1, new Vector3Int(-7, 3), LocationNameList.campCenter, MonsterNameList.axeman, Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricadeGuards+1, new Vector3Int(-7, 2), LocationNameList.campCenter, MonsterNameList.disciplinarian, Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricadeGuards+1, new Vector3Int(-7, 1), LocationNameList.campCenter, MonsterNameList.signaleer, Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricadeGuards+1, new Vector3Int(-7, 0), LocationNameList.campCenter, MonsterNameList.spearman, Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade+1, new Vector3Int(-8, 3), LocationNameList.campCenter, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade+1, new Vector3Int(-8, 2), LocationNameList.campCenter, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade+1, new Vector3Int(-8, 1), LocationNameList.campCenter, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade+1, new Vector3Int(-8, 0), LocationNameList.campCenter, facing: Facing.SouthWest));

        list.Add(new VaultableOrDestroyableObjectSpawnDetails(NPCNameList.hastilyBuiltBarricade, new Vector3Int(10, 11), VaultableOrDestroyableObject.diffThreeVaultableBarricadeOneTileIndexZero));
        list.Add(new VaultableOrDestroyableObjectSpawnDetails(NPCNameList.hastilyBuiltBarricade, new Vector3Int(10, 10), VaultableOrDestroyableObject.diffThreeVaultableBarricadeOneTileIndexZero));
        list.Add(new VaultableOrDestroyableObjectSpawnDetails(NPCNameList.hastilyBuiltBarricade, new Vector3Int(10, 9), VaultableOrDestroyableObject.diffThreeVaultableBarricadeOneTileIndexZero));

        #region Dead Bodies (Worm Attack)
        
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(12, -7), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-10, 4), MonsterNameList.javelineer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-10, -2), MonsterNameList.lancer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-15, -4), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-14, 1), MonsterNameList.axeman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-15, 7), NPCNameList.slave+2));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-7, 6), MonsterNameList.signaleer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-6, 2), MonsterNameList.lieutenant, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-7, -2), MonsterNameList.linebreaker, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-5, 7), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(5, -1), MonsterNameList.executioner, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(4, 5), MonsterNameList.axeman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(3, 11), MonsterNameList.javelineer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(1, 12), NPCNameList.slave));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(0, 7), MonsterNameList.javelineer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-3, 13), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-4, 10), MonsterNameList.javelineer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(8, 6), MonsterNameList.lancer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(6, 7), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(6, 4), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(7, 2), MonsterNameList.signaleer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(13,11), NPCNameList.slave+1));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(14, 8), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(17, -4), MonsterNameList.executioner, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(20, 11), MonsterNameList.axeman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(20, 5), MonsterNameList.javelineer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(2, 21), MonsterNameList.lieutenant, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-1, 19), NPCNameList.slave+2));


        #endregion

        oocSpawnDetailsDict.Add(LocationNameList.campCenter, list);
        #endregion
        #region SECamp
        list = new List<OOCSpawnDetails>();

        list.Add(new HostilityTerrainSpawnDetails(LocationNameList.campSouthEast, Constants.indexZero));


        float statueOffset = -.1f;
        list.Add(new NPCOffGridSpawnDetails(NPCNameList.statue, new Vector3Int(7, 3), LocationNameList.campSouthEast, PrefabNames.directorStatuePath, 
                                    new Vector3Int[] { 
                                                        new Vector3Int(7, 4),
                                                        new Vector3Int(8, 4),
                                                        new Vector3Int(8, 3)
                                                     },
                                                    offset: statueOffset));

        list.Add(new NPCOffGridSpawnDetails(NPCNameList.toppledStatue, new Vector3Int(7, 3), LocationNameList.campSouthEast, PrefabNames.brokenDirectorStatuePath, 
                                    new Vector3Int[] { 
                                                        new Vector3Int(7, 4),
                                                        new Vector3Int(8, 4),
                                                        new Vector3Int(8, 3)
                                                     },
                                                    offset: statueOffset));

        list.Add(new VaultableOrDestroyableObjectSpawnDetails(NPCNameList.hastilyBuiltBarricade, new Vector3Int(2, 1), VaultableOrDestroyableObject.diffThreeVaultableBarricadeOneTileIndexZero));
        list.Add(new VaultableOrDestroyableObjectSpawnDetails(NPCNameList.hastilyBuiltBarricade, new Vector3Int(1, 1), VaultableOrDestroyableObject.diffThreeVaultableBarricadeOneTileIndexZero));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(19, 14), Facing.SouthEast));

        #region Dead Bodies (Worm Attack)
        
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(25, -1), MonsterNameList.lieutenant, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(24, 2), NPCNameList.slave));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(21, 3), NPCNameList.slave+1));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(16, 1), MonsterNameList.javelineer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(14, 23), NPCNameList.slave));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(13, 13), NPCNameList.slave+1));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(12, 15), NPCNameList.slave+2));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(12, 24), MonsterNameList.signaleer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(12, 7), MonsterNameList.lancer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(10, -5), NPCNameList.slave+1));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(8, -3), NPCNameList.slave+2));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(7, 20), NPCNameList.slave));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(6, 22), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(6, 3), NPCNameList.slave+1));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(5, 7), NPCNameList.slave+2));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(4, 11), NPCNameList.slave));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(3, 1), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(3, -3), NPCNameList.slave));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(1, 16), NPCNameList.slave+1));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-1, 22), MonsterNameList.axeman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(-4, 12), NPCNameList.slave));

        #endregion

        #region Guard Punishment Scene

        #region Nameless Slaves

            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+1, new Vector3Int(10, -3), LocationNameList.campSouthEast, animationName: NPCNameList.temple, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+4, new Vector3Int(9, -3), LocationNameList.campSouthEast, animationName: NPCNameList.slave+2, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(8, -3), LocationNameList.campSouthEast, animationName: NPCNameList.slave+1, facing: Facing.SouthEast, ignoresSecretDoors: false));  
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+5, new Vector3Int(7, -3), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false));  
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(6, -3), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false));  
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+2, new Vector3Int(4, -3), LocationNameList.campSouthEast, animationName: NPCNameList.uros, facing: Facing.SouthEast, ignoresSecretDoors: false));  
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+6, new Vector3Int(3, -3), LocationNameList.campSouthEast, animationName: NPCNameList.noBrand, facing: Facing.SouthEast, ignoresSecretDoors: false));  
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(2, -3), LocationNameList.campSouthEast, animationName: NPCNameList.slave+1, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+3, new Vector3Int(1, -3), LocationNameList.campSouthEast, animationName: NPCNameList.kastor, facing: Facing.SouthEast, ignoresSecretDoors: false));

            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(11, -2), LocationNameList.campSouthEast, animationName: NPCNameList.slave+1, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(10, -2), LocationNameList.campSouthEast, animationName: NPCNameList.kastor, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+3, new Vector3Int(9, -2), LocationNameList.campSouthEast, animationName: NPCNameList.slave+2, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+1, new Vector3Int(8, -2), LocationNameList.campSouthEast, animationName: NPCNameList.uros, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+6, new Vector3Int(7, -2), LocationNameList.campSouthEast, animationName: NPCNameList.noBrand+1, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(6, -2), LocationNameList.campSouthEast, animationName: NPCNameList.slave+2, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+2, new Vector3Int(5, -2), LocationNameList.campSouthEast, animationName: NPCNameList.brush, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(4, -2), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+5, new Vector3Int(3, -2), LocationNameList.campSouthEast, animationName: NPCNameList.balint, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+4, new Vector3Int(2, -2), LocationNameList.campSouthEast, animationName: NPCNameList.uros, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(1, -2), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false));

            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+2, new Vector3Int(11, -1), LocationNameList.campSouthEast, animationName: NPCNameList.balint, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(10, -1), LocationNameList.campSouthEast, animationName: NPCNameList.slave+1, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(9, -1), LocationNameList.campSouthEast, animationName: NPCNameList.temple, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+6, new Vector3Int(8, -1), LocationNameList.campSouthEast, animationName: NPCNameList.noBrand+2, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(7, -1), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(6, -1), LocationNameList.campSouthEast, animationName: NPCNameList.janos, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+4, new Vector3Int(5, -1), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.crowd, new Vector3Int(5, -1), LocationNameList.campSouthEast, animationName: NPCNameList.slave+1, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+3, new Vector3Int(4, -1), LocationNameList.campSouthEast, animationName: NPCNameList.temple, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+1, new Vector3Int(3, -1), LocationNameList.campSouthEast, animationName: NPCNameList.slave+2, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+5, new Vector3Int(2, -1), LocationNameList.campSouthEast, animationName: NPCNameList.uros, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(1, -1), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false));

            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(12, 0), LocationNameList.campSouthEast, animationName: NPCNameList.slave+2, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(11, 0), LocationNameList.campSouthEast, animationName: NPCNameList.ervin, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+5, new Vector3Int(10, 0), LocationNameList.campSouthEast, animationName: NPCNameList.brush, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(9, 0), LocationNameList.campSouthEast, animationName: NPCNameList.uros, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(8, 0), LocationNameList.campSouthEast, animationName: NPCNameList.slave+1, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+2, new Vector3Int(7, 0), LocationNameList.campSouthEast, animationName: NPCNameList.kastor, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+1, new Vector3Int(6, 0), LocationNameList.campSouthEast, animationName: NPCNameList.balint, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(5, 0), LocationNameList.campSouthEast, animationName: NPCNameList.slave+2, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+6, new Vector3Int(4, 0), LocationNameList.campSouthEast, animationName: NPCNameList.noBrand+3, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+3, new Vector3Int(3, 0), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(2, 0), LocationNameList.campSouthEast, animationName: NPCNameList.slave+1, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+4, new Vector3Int(1, 0), LocationNameList.campSouthEast, animationName: NPCNameList.brush, facing: Facing.SouthEast, ignoresSecretDoors: false));

            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(12, 1), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+4, new Vector3Int(11, 1), LocationNameList.campSouthEast, animationName: NPCNameList.kastor, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(10, 1), LocationNameList.campSouthEast, animationName: NPCNameList.slave+1, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+1, new Vector3Int(9, 1), LocationNameList.campSouthEast, animationName: NPCNameList.balint, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+5, new Vector3Int(8, 1), LocationNameList.campSouthEast, animationName: NPCNameList.temple, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(7, 1), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+6, new Vector3Int(6, 1), LocationNameList.campSouthEast, animationName: NPCNameList.noBrand+4, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(5, 1), LocationNameList.campSouthEast, animationName: NPCNameList.ervin, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+3, new Vector3Int(4, 1), LocationNameList.campSouthEast, animationName: NPCNameList.balint, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(3, 1), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+2, new Vector3Int(2, 1), LocationNameList.campSouthEast, animationName: NPCNameList.slave+2, facing: Facing.SouthEast, ignoresSecretDoors: false));

            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(11, 2), LocationNameList.campSouthEast, animationName: NPCNameList.janos, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+2, new Vector3Int(10, 2), LocationNameList.campSouthEast, animationName: NPCNameList.temple, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+5, new Vector3Int(9, 2), LocationNameList.campSouthEast, animationName: NPCNameList.slave+1, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(8, 2), LocationNameList.campSouthEast, animationName: NPCNameList.kastor, facing: Facing.SouthEast, ignoresSecretDoors: false, offset: Constants.onTableHeightOffset/2)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+4, new Vector3Int(7, 2), LocationNameList.campSouthEast, animationName: NPCNameList.temple, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(6, 2), LocationNameList.campSouthEast, animationName: NPCNameList.slave+2, facing: Facing.SouthEast, ignoresSecretDoors: false)); 
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+3, new Vector3Int(5, 2), LocationNameList.campSouthEast, animationName: NPCNameList.brush, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+6, new Vector3Int(4, 2), LocationNameList.campSouthEast, animationName: NPCNameList.noBrand+5, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+1, new Vector3Int(3, 2), LocationNameList.campSouthEast, animationName: NPCNameList.slave, facing: Facing.SouthEast, ignoresSecretDoors: false));

            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+2, new Vector3Int(10, 3), LocationNameList.campSouthEast, animationName: NPCNameList.brush, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+5, new Vector3Int(9, 3), LocationNameList.campSouthEast, animationName: NPCNameList.slave+2, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave, new Vector3Int(6, 3), LocationNameList.campSouthEast, animationName: NPCNameList.slave+1, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+4, new Vector3Int(5, 3), LocationNameList.campSouthEast, animationName: NPCNameList.balint, facing: Facing.SouthEast, ignoresSecretDoors: false));
            list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.slave+3, new Vector3Int(4, 3), LocationNameList.campSouthEast, animationName: NPCNameList.uros, facing: Facing.SouthEast, ignoresSecretDoors: false));
        #endregion

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.clay, new Vector3Int(5, -3), facing: Facing.SouthEast)); 

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.marcos, new Vector3Int(9, -5), LocationNameList.campSouthEast, facing: Facing.SouthEast, animationType: CharacterAnimationType.Secondary_Idle, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.reka, new Vector3Int(7, -5), LocationNameList.campSouthEast, facing: Facing.SouthEast, animationType: CharacterAnimationType.Secondary_Idle, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.andras, new Vector3Int(5, -5), LocationNameList.campSouthEast, facing: Facing.SouthEast, animationType: CharacterAnimationType.Secondary_Idle, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.pazman, new Vector3Int(3, -5), LocationNameList.campSouthEast, facing: Facing.SouthEast, animationType: CharacterAnimationType.Secondary_Idle, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(1, -5), LocationNameList.campSouthEast, facing: Facing.SouthEast, animationType: CharacterAnimationType.Secondary_Idle, ignoresSecretDoors: false));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.géza, new Vector3Int(6, -11), LocationNameList.campSouthEast, facing: Facing.NorthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.brush, new Vector3Int(5, -11), LocationNameList.campSouthEast, facing: Facing.NorthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.kastor, new Vector3Int(2, -9), LocationNameList.campSouthEast, facing: Facing.NorthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.ervin, new Vector3Int(3, -10), LocationNameList.campSouthEast, facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.ervin+1, new Vector3Int(2, -7), facing: Facing.NorthWest)); 
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.janos, new Vector3Int(8, -11), LocationNameList.campSouthEast, facing: Facing.NorthWest));
        
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.nandor, new Vector3Int(6, -9), LocationNameList.campSouthEast, facing: Facing.NorthWest, speakAtStartScript: new GuardPunishmentNandorStartScript())); // Nandor during guard punishment start convo
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.nandor+1, new Vector3Int(6, -9), LocationNameList.campSouthEast, facing: Facing.NorthWest)); // Nandor after guard punishment start convo
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.carter, new Vector3Int(1, -11), LocationNameList.campSouthEast, facing: Facing.NorthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.thatch, new Vector3Int(10, -9), LocationNameList.campSouthEast, animationType: CharacterAnimationType.Death_Front, facing: Facing.SouthWest, sleepingDialogueIntro: true));
        #endregion

        oocSpawnDetailsDict.Add(LocationNameList.campSouthEast, list);
        #endregion
        #region MineEntranceCamp
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard, new Vector3Int(5, 12), LocationNameList.campMineEntrance, facing: Facing.SouthEast, animationName: MonsterNameList.spearman));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard+1, new Vector3Int(5, 11), LocationNameList.campMineEntrance, facing: Facing.NorthWest, animationName: MonsterNameList.spearman));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardMuzsa, new Vector3Int(9, 10), LocationNameList.campMineEntrance, facing: Facing.SouthEast));

        list.Add(new ObstacleSpawnDetails(NPCNameList.barricade, new Vector3Int(9, 11), PrefabNames.squareCratesSmall));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardMuzsa + 1, new Vector3Int(7, 10), LocationNameList.campMineEntrance, facing: Facing.SouthEast));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.uros, new Vector3Int(13, -1), LocationNameList.campMineEntrance, facing: Facing.SouthWest));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(12, 13), VaultableObject.diffTwoVaultableBarrelsOneTile));

        #region Dead Bodies (Worm Attack)
        
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(13, 12), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(12, 10), MonsterNameList.javelineer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(10, 7), MonsterNameList.lancer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(8, 10), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(7, 8), NPCNameList.slave));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(6, 10), MonsterNameList.signaleer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(5, 12), MonsterNameList.lieutenant, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(2, 8), NPCNameList.slave+1));

        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(5, -11), MonsterNameList.spearman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(10, -13), MonsterNameList.executioner, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(10, -18), MonsterNameList.axeman, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(7, -16), MonsterNameList.javelineer, facing: Facing.SouthEast));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(10, -9), NPCNameList.slave));
        list.Add(new DeadBodySpawnDetails(NPCNameList.body, new Vector3Int(6, -13), NPCNameList.slave+1));

        #endregion

        oocSpawnDetailsDict.Add(LocationNameList.campMineEntrance, list);
        #endregion
        #region Camp Manse
        list = new List<OOCSpawnDetails>();

        list.Add(new HostilityTerrainSpawnDetails(LocationNameList.campManse, Constants.indexZero));

        list.Add(new VaultableOrDestroyableObjectSpawnDetails(NPCNameList.hastilyBuiltBarricade, new Vector3Int(6, -19), VaultableOrDestroyableObject.diffThreeVaultableBarricadeOneTileIndexZero));
        list.Add(new VaultableOrDestroyableObjectSpawnDetails(NPCNameList.hastilyBuiltBarricade, new Vector3Int(5, -19), VaultableOrDestroyableObject.diffThreeVaultableBarricadeOneTileIndexZero));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricadeGuards+2, new Vector3Int(2, -18), LocationNameList.campManse, MonsterNameList.signaleer, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricadeGuards+2, new Vector3Int(1, -18), LocationNameList.campManse, MonsterNameList.spearman, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade+2, new Vector3Int(2, -19), LocationNameList.campManse, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade+2, new Vector3Int(1, -19), LocationNameList.campManse, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardAndras+2, new Vector3Int(3, -21), facing: Facing.NorthEast)); 

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricadeGuards+3, new Vector3Int(3, 3), LocationNameList.campManse, MonsterNameList.axeman, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricadeGuards+3, new Vector3Int(2, 3), LocationNameList.campManse, MonsterNameList.signaleer, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade+3, new Vector3Int(3, 2), LocationNameList.campManse, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade+3, new Vector3Int(2, 2), LocationNameList.campManse, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardAndras+3, new Vector3Int(3, -2), facing: Facing.NorthEast)); 

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.imre, new Vector3Int(-6, -9), LocationNameList.campManse, facing: Facing.SouthEast));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.imre+1, new Vector3Int(-8, 1), LocationNameList.campManse, facing: Facing.SouthEast));

        list.Add(new LadderSpawnDetails(new Vector3Int(-5, -20), PrefabNames.ladderTallSW,
                                        new Ladder(Constants.difficultyTwo, LocationNameList.campManse, LocationNameList.guardHouseTopFloor, 
                                                    Ladder.barracksLadderDescription, Facing.SouthEast), flipX: Constants.flipX));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(6, -16), LocationNameList.campManse, facing: Facing.SouthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft, new Vector3Int(4, -17), facing: Facing.NorthEast));

        oocSpawnDetailsDict.Add(LocationNameList.campManse, list);
        #endregion
        #region NWCamp
        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(2, 11), Facing.SouthEast));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(-13,-10), Facing.SouthEast));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guard, new Vector3Int(-14, 0), LocationNameList.campNorthWest,  animationName: MonsterNameList.axeman, facing: Facing.SouthWest));

        list.Add(new LadderSpawnDetails(new Vector3Int(-8, 16), PrefabNames.ladderShortNE,
                                        new Ladder(Constants.noDifficulty, LocationNameList.campNorthWest, LocationNameList.bodyPile, 
                                                    Ladder.bodyPileLadderDescription, Facing.SouthEast), sortingLayerInfo: SortingLayerManager.groundSortingLayerInfo, offset: .70f));

        #region Exiting body pile


        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft+1, new Vector3Int(-6, 14), facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.chiefTabor+5, new Vector3Int(-7, 12), facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guard+1, new Vector3Int(-7, 8), animationName: MonsterNameList.axeman, facing: Facing.NorthWest));

        #endregion

        #region End Of Tutorial

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(-7, -1), LocationNameList.campNorthWest, facing: Facing.SouthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft, new Vector3Int(-9, -2), facing: Facing.NorthEast));

        #endregion

        #region Str Tutorial

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.chiefTabor+1, new Vector3Int(-7, -1), LocationNameList.campNorthWest, facing: Facing.SouthWest));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(-5, 0), TutorialSequenceList.intimidateTutorialSequenceKey,
                                                                          TutorialSequenceList.intimidateTutorialSeenFlag,
                                                                          alwaysSpawn: true));

        #endregion

        #region Dex Tutorial

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.chiefTabor+2, new Vector3Int(-4, -5), LocationNameList.campNorthWest, facing: Facing.SouthWest));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(-4, -4), TutorialSequenceList.secondCunningTutorialSequenceKey,
                                                                          TutorialSequenceList.secondCunningTutorialSeenFlag,
                                                                          alwaysSpawn: true));

        list.Add(new CunningBlockerSpawnDetails(Constants.indexZero, new Vector3Int(-3, -3), Facing.SouthEast, CunningObjectSpriteCategory.Crank, endFacing: Facing.NorthWest, allBlockerSpawnDetails:
                 new List<ObstacleSpawnDetails>()
                 {
                    new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator+1, new Vector3Int(-3, -1), PrefabNames.shackWallHalf),
                    new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator+1, new Vector3Int(-3, -2), PrefabNames.shackWallHalf),
                    new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator+1, new Vector3Int(-4, 0), PrefabNames.shackWallHalf),
                    new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator+1, new Vector3Int(-4, -1), PrefabNames.shackWallHalf),
                    new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator+1, new Vector3Int(-4, -2), PrefabNames.shackWallHalf)
                 },
                 script: new TaborCunningTutorialScript(),
                 tutorialTargetHash: TutorialSequenceList.tutorialCunningObjectTargetHash));

        list.Add(new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator, new Vector3Int(-3, -1), PrefabNames.shackWallHalf));
        list.Add(new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator, new Vector3Int(-3, -2), PrefabNames.shackWallHalf));
        list.Add(new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator, new Vector3Int(-4, 0), PrefabNames.shackWallHalf));
        list.Add(new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator, new Vector3Int(-4, -1), PrefabNames.shackWallHalf));
        list.Add(new ObstacleSpawnDetails(NPCNameList.halfWall + Constants.DEXDesignator, new Vector3Int(-4, -2), PrefabNames.shackWallHalf));

        #endregion

        #region Wis Tutorial

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.chiefTabor+4, new Vector3Int(1, 0), LocationNameList.campNorthWest, facing: Facing.SouthWest));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(1, 0), TutorialSequenceList.observationTutorialSequenceKey,
                                                                          TutorialSequenceList.observationTutorialSeenFlag,
                                                                          alwaysSpawn: true));

        #endregion

        #region Cha Tutorial

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.chiefTabor+3, new Vector3Int(-3, 0), LocationNameList.campNorthWest, facing: Facing.SouthWest));

        list.Add(new ButtonSpawnDetails(new Vector3Int(-3, 1), tutorialTargetHash: TutorialSequenceList.tutorialButtonOneTargetHash));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-1, 1), tutorialTargetHash: TutorialSequenceList.tutorialButtonTwoTargetHash));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(-3, 0), TutorialSequenceList.leadershipTutorialSequenceKey,
                                                                          TutorialSequenceList.leadershipTutorialSeenFlag,
                                                                          alwaysSpawn: true));

        #endregion

        oocSpawnDetailsDict.Add(LocationNameList.campNorthWest, list);
        #endregion

        #region MineLvl_1

        #region MineLvl_1-1b

        list = new List<OOCSpawnDetails>();

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(13, 7), VaultableObject.diffTwoVaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(10, 9), VaultableObject.diffTwoVaultableBarrelsOneTile));

        list.Add(new ButtonSpawnDetails(new Vector3Int(6, 1), charismaRequirement: 2));
        list.Add(new ButtonSpawnDetails(new Vector3Int(6, -1), charismaRequirement: 2));

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

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl2FirstSecretDoor, areaName: ZoneKeyList.mineLvl2, sectionName: LocationNameList.section1a, index: Constants.indexOne));

        list.Add(new TutorialColliderSpawnDetails(new Vector3Int(-1, -3), TutorialSequenceList.firstHostilityTutorialSequenceKey,
                                                                          TutorialSequenceList.firstHostilityTutorialSeenFlag));

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

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardPazman, new Vector3Int(-1, 4), ZoneKeyList.mineLvl2 + LocationNameList.section2a, facing: Facing.NorthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardReka, new Vector3Int(3, 9), ZoneKeyList.mineLvl2 + LocationNameList.section2a, facing: Facing.SouthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardVirag, new Vector3Int(3, 6), ZoneKeyList.mineLvl2 + LocationNameList.section2a, facing: Facing.SouthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.overseerGaspar, new Vector3Int(0, 9), ZoneKeyList.mineLvl2 + LocationNameList.section2a, facing: Facing.SouthWest));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2a, list);

        #endregion

        #region MineLvl_2-2b

        list = new List<OOCSpawnDetails>();

        list.Add(new ObstacleSpawnDetails(NPCNameList.table, new Vector3Int(11, 5), PrefabNames.emptyPolearmRack, flipX: true));
        list.Add(new ObstacleSpawnDetails(NPCNameList.table, new Vector3Int(11, 4), PrefabNames.emptyPolearmRack, flipX: true));
        list.Add(new WeaponRackSpawnDetails(Constants.indexZero, new Vector3Int(11, 2), Facing.SouthWest, ChestType.ShovelRack, script: new FoundToolBundle()));
        list.Add(new WeaponRackSpawnDetails(Constants.indexZero, new Vector3Int(11, 1), Facing.SouthWest, ChestType.ShovelRack, script: new FoundToolBundle()));
        list.Add(new ObstacleSpawnDetails(NPCNameList.table, new Vector3Int(11, -1), PrefabNames.emptyPolearmRack, flipX: true));
        list.Add(new WeaponRackSpawnDetails(Constants.indexZero, new Vector3Int(11, -2), Facing.SouthWest, ChestType.ShovelRack, script: new FoundToolBundle()));

        list.Add(new WeaponRackSpawnDetails(Constants.indexZero, new Vector3Int(9, 3), Facing.SouthWest, ChestType.ShovelRack, script: new FoundToolBundle()));
        list.Add(new WeaponRackSpawnDetails(Constants.indexZero, new Vector3Int(9, 2), Facing.SouthWest, ChestType.ShovelRack, script: new FoundToolBundle()));
        list.Add(new ObstacleSpawnDetails(NPCNameList.table, new Vector3Int(9, 1), PrefabNames.emptyPolearmRack, flipX: true));
        list.Add(new WeaponRackSpawnDetails(Constants.indexZero, new Vector3Int(9, -1), Facing.SouthWest, ChestType.ShovelRack, script: new FoundToolBundle()));

        list.Add(new ObstacleSpawnDetails(NPCNameList.table, new Vector3Int(7, 3), PrefabNames.emptyPolearmRack, flipX: true));
        list.Add(new WeaponRackSpawnDetails(Constants.indexZero, new Vector3Int(7, 2), Facing.SouthWest, ChestType.ShovelRack, script: new FoundToolBundle()));
        list.Add(new WeaponRackSpawnDetails(Constants.indexZero, new Vector3Int(7, 0), Facing.SouthWest, ChestType.ShovelRack, script: new FoundToolBundle()));
        list.Add(new ObstacleSpawnDetails(NPCNameList.table, new Vector3Int(7, -1), PrefabNames.emptyPolearmRack, flipX: true));

        list.Add(new WeaponRackSpawnDetails(Constants.indexTwo, new Vector3Int(9, 5), Facing.SouthEast, ChestType.PickaxeTable));
        list.Add(new WeaponRackSpawnDetails(Constants.indexOne, new Vector3Int(8, 5), Facing.SouthEast, ChestType.PickaxeTable));
        list.Add(new ObstacleSpawnDetails(NPCNameList.table, new Vector3Int(7, 5), PrefabNames.emptyWeaponTable));
        list.Add(new ObstacleSpawnDetails(NPCNameList.table, new Vector3Int(4, 5), PrefabNames.emptyWeaponTable));
        list.Add(new ObstacleSpawnDetails(NPCNameList.table, new Vector3Int(3, 5), PrefabNames.emptyWeaponTable));

        list.Add(new ChestSpawnDetails(Constants.indexThree, new Vector3Int(9, -3), Facing.NorthWest));

        list.Add(new ButtonSpawnDetails(new Vector3Int(7, -5)));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_2-3a

        list = new List<OOCSpawnDetails>();

        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(0, 1), Constants.difficultyTwo, Constants.sizeFour));
        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(-1, 1), Constants.difficultyTwo, Constants.sizeFour));
        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(0, -2), Constants.difficultyTwo, Constants.sizeFour));
        list.Add(new VaultableRubbleSpawnDetails(NPCNameList.vaultableRocks, new Vector3Int(-1, -2), Constants.difficultyTwo, Constants.sizeFour));

        list.Add(new ButtonSpawnDetails(new Vector3Int(2, 8), charismaRequirement: 2));
        list.Add(new ButtonSpawnDetails(new Vector3Int(2, 5), charismaRequirement: 2));

        list.Add(new ButtonSpawnDetails(new Vector3Int(9, 8), Constants.indexOne, charismaRequirement: 2));
        list.Add(new ButtonSpawnDetails(new Vector3Int(9, 5), Constants.indexOne, charismaRequirement: 2));

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

        list.Add(new LinkedCunningBlockerSpawnDetails(Constants.indexZero, new Vector3Int(8, 8), Facing.SouthWest, Facing.SouthEast, CunningObjectSpriteCategory.Crank, blockerSpawnDetails, Constants.indexTwo));

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

        list.Add(new LinkedCunningBlockerSpawnDetails(Constants.indexOne, new Vector3Int(8, 11), Facing.SouthWest, Facing.SouthEast, CunningObjectSpriteCategory.Crank, blockerSpawnDetails, Constants.indexThree, tutorialTargetHash: TutorialSequenceList.tutorialCunningObjectTargetHash));

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

        list.Add(new LinkedCunningBlockerSpawnDetails(Constants.indexTwo, new Vector3Int(11, 8), Facing.SouthWest, Facing.SouthEast, CunningObjectSpriteCategory.Crank, blockerSpawnDetails, Constants.indexZero));
        
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

        list.Add(new LinkedCunningBlockerSpawnDetails(Constants.indexThree, new Vector3Int(11, 11), Facing.SouthWest, Facing.SouthEast, CunningObjectSpriteCategory.Crank, blockerSpawnDetails, Constants.indexOne));

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

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl3PuzzleDoor, areaName: ZoneKeyList.mineLvl3, sectionName:LocationNameList.section1b, index: Constants.indexOne));

        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(0, -6), PrefabNames.mineLvl3WallSecretDoor, secretDoorFlag: SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(1, -3), PrefabNames.mineLvl3WallSecretDoor, secretDoorFlag: SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(1, -4), PrefabNames.mineLvl3WallSecretDoor, secretDoorFlag: SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(1, -5), PrefabNames.mineLvl3WallSecretDoor, secretDoorFlag: SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(1, -6), PrefabNames.mineLvl3WallSecretDoor, secretDoorFlag: SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(2, -2), PrefabNames.mineLvl3WallSecretDoor, secretDoorFlag: SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(2, -3), PrefabNames.mineLvl3WallSecretDoor, secretDoorFlag: SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(2, -4), PrefabNames.mineLvl3WallSecretDoor, secretDoorFlag: SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(2, -5), PrefabNames.mineLvl3WallSecretDoor, secretDoorFlag: SecretDoorKeyList.mineLvl3PuzzleDoor));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall, new Vector3Int(2, -6), PrefabNames.mineLvl3WallSecretDoor, secretDoorFlag: SecretDoorKeyList.mineLvl3PuzzleDoor));
        
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.mineLvl3Wall+2, new Vector3Int(3, 0), PrefabNames.mineLvl3GroundSecretDoor, SortingLayerManager.secondSortingLayerInfo, SecretDoorKeyList.mineLvl3PuzzleDoor));
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
        list.Add(new HiddenButtonSpawnDetails(new Vector3Int(13, -4), SecretDoorKeyList.mineLvl3PuzzleDoor, Constants.indexOne));
        list.Add(new HiddenButtonSpawnDetails(new Vector3Int(9, -3), SecretDoorKeyList.mineLvl3PuzzleDoor, Constants.indexTwo));
        list.Add(new HiddenButtonSpawnDetails(new Vector3Int(7, 1), SecretDoorKeyList.mineLvl3PuzzleDoor, Constants.indexThree));

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl3PuzzleFinished, areaName: ZoneKeyList.mineLvl3, sectionName: LocationNameList.section1b, index: Constants.indexTwo));

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

        list.Add(new DoubleCunningBlockerSpawnDetails(Constants.indexZero, new Vector3Int(4, -1), Facing.SouthWest, Facing.SouthEast, CunningObjectSpriteCategory.Crank, blockerSpawnDetails, deactivatedblockerSpawnDetails));

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

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade, new Vector3Int(5, 11), ZoneKeyList.mineLvl3 + LocationNameList.section3b, facing: Facing.NorthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardPazman, new Vector3Int(3, 10), ZoneKeyList.mineLvl3 + LocationNameList.section3b, facing: Facing.NorthEast));

        list.Add(new DependantSpawnDetails(NPCNameList.guardPazman+1, new Vector3Int(5, 10), ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.barricade, facing: Facing.NorthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardReka, new Vector3Int(2, 7), ZoneKeyList.mineLvl3 + LocationNameList.section3b, facing: Facing.NorthEast));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardVirag, new Vector3Int(4, 5), ZoneKeyList.mineLvl3 + LocationNameList.section3b, facing: Facing.NorthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.overseerGaspar, new Vector3Int(6, 7), ZoneKeyList.mineLvl3 + LocationNameList.section3b, facing: Facing.SouthWest));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(6, 6), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(6, 0), Facing.SouthEast));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section3b, list);

        #endregion

        #region MineLvl_3-4a

        list = new List<OOCSpawnDetails>();

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(2, 4), VaultableObject.diffThreeVaultableGap, sortingLayerInfo: SortingLayerManager.groundSortingLayerInfo, spriteName: PrefabNames.stoneVaultableGap, offset: Constants.onTableHeightOffset*10));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(2, 2), VaultableObject.diffThreeVaultableGap, sortingLayerInfo: SortingLayerManager.groundSortingLayerInfo, spriteName: PrefabNames.stoneVaultableGap, offset: Constants.onTableHeightOffset*10));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(8, 9), Facing.SouthWest));  

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section4a, list);

        #endregion

        #region MineLvl_3-4b

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(new Vector3Int(-2, 16), weight: Constants.sizeFive));  

        list.Add(new ButtonSpawnDetails(new Vector3Int(0, -4)));  
        list.Add(new ButtonSpawnDetails(new Vector3Int(-2, -4)));  

        list.Add(new ButtonSpawnDetails(new Vector3Int(1, 13), index: Constants.indexOne));  
        list.Add(new ButtonSpawnDetails(new Vector3Int(3, 11), index: Constants.indexOne));  

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section4b, list);

        #endregion

        #region MineLvl_3-5

        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(20, 1), Facing.SouthWest));  

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_3-Miner Camp

        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade, new Vector3Int(3, 3), ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, facing: Facing.NorthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.carter, new Vector3Int(1, 2), ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, facing: Facing.NorthEast));

        list.Add(new DependantSpawnDetails(NPCNameList.carter+1, new Vector3Int(3, 2), ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.barricade, facing: Facing.NorthWest, animationType: CharacterAnimationType.Idle_Back));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(3, -1), ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, facing: Facing.NorthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.nandor, new Vector3Int(4, 1), ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, facing: Facing.SouthWest));

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

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl3_6aUnstablePillarHiddenTerrain, areaName: ZoneKeyList.mineLvl3, sectionName: LocationNameList.section6a, index: Constants.indexOne));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(2, 4), VaultableObject.diffThreeVaultableGap, spriteName: PrefabNames.stoneVaultableGap, offset: Constants.onTableHeightOffset*10, sortingLayerInfo: SortingLayerManager.groundSortingLayerInfo));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(0, 4), VaultableObject.diffThreeVaultableGap, spriteName: PrefabNames.stoneVaultableGap, offset: Constants.onTableHeightOffset*10, sortingLayerInfo: SortingLayerManager.groundSortingLayerInfo));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(-11, 2), VaultableObject.diffThreeVaultableGap, spriteName: PrefabNames.stoneVaultableGap, offset: Constants.onTableHeightOffset*10, sortingLayerInfo: SortingLayerManager.groundSortingLayerInfo));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(-13, 2), VaultableObject.diffThreeVaultableGap, spriteName: PrefabNames.stoneVaultableGap, offset: Constants.onTableHeightOffset*10, sortingLayerInfo: SortingLayerManager.groundSortingLayerInfo));

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

        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.unseenBarrier, new Vector3Int(-9, 1), SecretDoorKeyList.mineLvl3_7UnstablePillarHiddenTerrain));

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl3_7UnstablePillarHiddenTerrain, areaName:ZoneKeyList.mineLvl3,  sectionName: LocationNameList.section7, index: Constants.indexOne));

        blockerSpawnDetails = new List<ObstacleSpawnDetails>();

        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-11, -4)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-12, -4)));
        blockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-13, -4)));

        deactivatedblockerSpawnDetails = new List<ObstacleSpawnDetails>();

        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-11, -6)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-12, -6)));
        deactivatedblockerSpawnDetails.Add(new SpikeSpawnDetails(new Vector3Int(-13, -6)));

        list.Add(new DoubleCunningBlockerSpawnDetails(Constants.indexZero, new Vector3Int(-12, -5), Facing.SouthWest, Facing.SouthEast, CunningObjectSpriteCategory.Crank, blockerSpawnDetails, deactivatedblockerSpawnDetails));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(-5, -6), Facing.SouthWest));  

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(-10, -9), VaultableObject.diffThreeVaultableGap, spriteName: PrefabNames.lavaVaultableGapHalf, sortingLayerInfo: SortingLayerManager.buttonSortingLayerInfo));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableGap, new Vector3Int(-10, -11), VaultableObject.diffThreeVaultableGap, spriteName: PrefabNames.lavaVaultableGapHalf, sortingLayerInfo: SortingLayerManager.groundSortingLayerInfo));

        list.Add(new NPCSpawnDetails(NPCNameList.rubble, new Vector3Int(-6, 10), ZoneKeyList.mineLvl3 + LocationNameList.section7, PrefabNames.lowStalagmite));
        list.Add(new NPCSpawnDetails(NPCNameList.rubble, new Vector3Int(-7, 10), ZoneKeyList.mineLvl3 + LocationNameList.section7, PrefabNames.lowStalagmite));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardPazman, new Vector3Int(-6, 6), facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardReka, new Vector3Int(-8, 7), facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardVirag, new Vector3Int(-7, 8), facing: Facing.SouthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.overseerGaspar, new Vector3Int(-6, 8), facing: Facing.SouthEast));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft, new Vector3Int(-10, 5), facing: Facing.NorthEast));
        // list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.thatch, new Vector3Int(-5, 5), facing: Facing.SouthWest));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft+1, new Vector3Int(-8,4), facing: Facing.NorthWest));
        // list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.thatch+1, new Vector3Int(-6,4), facing: Facing.NorthWest));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.carter, new Vector3Int(-6, 3), facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.nandor, new Vector3Int(-8, 2), facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(-9, 3), facing: Facing.NorthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardMarcos+1, new Vector3Int(-6, 6), facing: Facing.NorthWest));

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.mineLvl3_7PocketSealedRubble, areaName:ZoneKeyList.mineLvl3,  sectionName: LocationNameList.section7, index: Constants.indexTwo));

        oocSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section7, list);

        #endregion

        #endregion

        #region Manse-1F

        #region Manse-1F-Kitchens

        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.kende, new Vector3Int(0, 1), ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, speakAtStartScript: new KendeInKitchenDuringRiotScript(), facing: Facing.SouthWest));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.imre+1, new Vector3Int(0, -3), ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, facing: Facing.SouthWest)); //loyal imre

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.imre+2, new Vector3Int(0, 0), facing: Facing.SouthEast)); //disloyal imre

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.pan, new Vector3Int(0, 2), ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, facing: Facing.SouthWest, animationType: CharacterAnimationType.Death_Back));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guard, new Vector3Int(0, 3), facing: Facing.SouthWest, animationName: MonsterNameList.linebreaker));
 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.noBrand+1, new Vector3Int(-5, -1), facing: Facing.NorthEast)); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.noBrand, new Vector3Int(-5, 0), facing: Facing.NorthEast)); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.noBrand+2, new Vector3Int(-5, 1), facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.noBrand+3, new Vector3Int(-5, 2), facing: Facing.NorthEast)); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.noBrand+4, new Vector3Int(-5, 3), facing: Facing.NorthEast)); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.noBrand+3, new Vector3Int(-5, 4), facing: Facing.NorthEast)); 

        list.Add(new LadderSpawnDetails(new Vector3Int(1, 4), PrefabNames.ladderTallNE,
                                        new Ladder(Constants.difficultyThree, ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, 
                                                    ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, 
                                                    Ladder.kitchensLadderDescription, Facing.SouthWest))); //, flipX: Constants.flipX

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, list);

        #endregion

        #region Manse-1F-1a

        list = new List<OOCSpawnDetails>();

        list.Add(new ObstacleSpawnDetails(NPCNameList.crate, new Vector3Int(2, 3), PrefabNames.squareCratesSmall, offset: Constants.onTableHeightOffset*-1));
        list.Add(new ObstacleSpawnDetails(NPCNameList.crate, new Vector3Int(1, 3), PrefabNames.squareCratesSmall, offset: Constants.onTableHeightOffset*-1));
        list.Add(new ObstacleSpawnDetails(NPCNameList.crate, new Vector3Int(3, 2), PrefabNames.squareCratesSmall, offset: Constants.onTableHeightOffset*-1));
        list.Add(new ObstacleSpawnDetails(NPCNameList.crate, new Vector3Int(2, 2), PrefabNames.squareCratesSmall, offset: Constants.onTableHeightOffset*-1));
        list.Add(new ObstacleSpawnDetails(NPCNameList.crate, new Vector3Int(1, 2), PrefabNames.squareCratesSmall, offset: Constants.onTableHeightOffset*-1));
        list.Add(new ObstacleSpawnDetails(NPCNameList.crate, new Vector3Int(0, 2), PrefabNames.squareCratesSmall, offset: Constants.onTableHeightOffset*-1));

        list.Add(new ObstacleSpawnDetails(NPCNameList.crate, new Vector3Int(3, -4), PrefabNames.squareCratesSmall, offset: Constants.onTableHeightOffset*-1));
        list.Add(new ObstacleSpawnDetails(NPCNameList.crate, new Vector3Int(0, -3), PrefabNames.squareCratesSmall, offset: Constants.onTableHeightOffset*-1));
        list.Add(new ObstacleSpawnDetails(NPCNameList.crate, new Vector3Int(0, -4), PrefabNames.squareCratesSmall, offset: Constants.onTableHeightOffset*-1));

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricadeGuards+4, new Vector3Int(2, -3), ZoneKeyList.manseFirstFloor + LocationNameList.section1a, MonsterNameList.axeman, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricadeGuards+4, new Vector3Int(1, -3), ZoneKeyList.manseFirstFloor + LocationNameList.section1a, MonsterNameList.javelineer, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade+4, new Vector3Int(2, -4), ZoneKeyList.manseFirstFloor + LocationNameList.section1a, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.barricade+4, new Vector3Int(1, -4), ZoneKeyList.manseFirstFloor + LocationNameList.section1a, facing: Facing.SouthEast, ignoresSecretDoors: false));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guardAndras+4, new Vector3Int(3, -6), facing: Facing.NorthWest)); 

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, list);

        #endregion

        #region Manse-1F-1b

        list = new List<OOCSpawnDetails>();

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(1, 3), Facing.SouthEast));
        list.Add(new ShelfSpawnDetails(Constants.indexOne, new Vector3Int(0, 3), Facing.SouthEast));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section1b, list);

        #endregion

        #region Manse-1F-1c

        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(15, -2), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(12, -2), Facing.NorthEast));

        list.Add(new VaultableOrDestroyableObjectSpawnDetails(NPCNameList.hastilyBuiltBarricade, new Vector3Int(8, -2), VaultableOrDestroyableObject.diffThreeVaultableBarricadeOneTileIndexZero));
        list.Add(new VaultableOrDestroyableObjectSpawnDetails(NPCNameList.hastilyBuiltBarricade, new Vector3Int(8, -3), VaultableOrDestroyableObject.diffThreeVaultableBarricadeOneTileIndexZero));
        
        list.Add(new VaultableOrDestroyableObjectSpawnDetails(NPCNameList.hastilyBuiltBarricade, new Vector3Int(14, -8), VaultableOrDestroyableObject.diffThreeVaultableBarricadeOneTileIndexZero, index: Constants.indexOne));
        list.Add(new VaultableOrDestroyableObjectSpawnDetails(NPCNameList.hastilyBuiltBarricade, new Vector3Int(13, -8), VaultableOrDestroyableObject.diffThreeVaultableBarricadeOneTileIndexZero, index: Constants.indexOne));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section1c, list);

        #endregion

        #region Manse-1F-Dining Room

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(new Vector3Int(-4, -1)));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, list);

        #endregion

        #region Manse-1F-2a

        list = new List<OOCSpawnDetails>();

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.manseHiddenGardenFlag, areaName:ZoneKeyList.manseFirstFloor, sectionName: LocationNameList.section2a, index: Constants.indexOne));

        list.Add(new BookSpawnDetails(NPCNameList.orders, new Vector3Int(7, 0), PrefabNames.note, ItemList.orderTranscriptIndex, offset: Constants.onGroundHeightOffset));

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(8, 3), Facing.SouthWest));
        list.Add(new ShelfSpawnDetails(Constants.indexOne, new Vector3Int(8, 2), Facing.SouthWest));

        list.Add(new ChestSpawnDetails(Constants.indexTwo, new Vector3Int(-4, 21), Facing.SouthEast, secretDoorFlag: SecretDoorKeyList.manseHiddenGardenFlag));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, list);

        #endregion

        #region Manse-1F-2b

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(new Vector3Int(-4, -5)));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, list);

        #endregion

        #region Manse-1F-2c

        list = new List<OOCSpawnDetails>();

        list.Add(new BookSpawnDetails(NPCNameList.orders, new Vector3Int(3, 3), PrefabNames.note, ItemList.pitSecondEntranceNoteIndex, offset: Constants.onGroundHeightOffset));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(3, 0), Facing.SouthWest));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2c, list);

        #endregion

        #region Manse-1F-StairsToPit

        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(2, 0), Facing.NorthWest));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(3, -3), Facing.SouthWest));
        list.Add(new ChestSpawnDetails(Constants.indexTwo, new Vector3Int(-3, -1), Facing.NorthWest));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, list);

        #endregion

        #region Manse-1F-3a

        list = new List<OOCSpawnDetails>();

        list.Add(new BookSpawnDetails(NPCNameList.orders, new Vector3Int(6, 0), PrefabNames.note, ItemList.orderTranscriptIndex));

        list.Add(new HiddenTerrainSpawnDetails(secretDoorKeys: new List<string>(){SecretDoorKeyList.meetingRoomSecretEntrance, SecretDoorKeyList.officeSecretEntranceFlag}, areaName:ZoneKeyList.manseFirstFloor, sectionName: LocationNameList.section3a, index: Constants.indexOne));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3a, list);

        #endregion

        #region Manse-1F-3b

        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.beam, new Vector3Int(3, 9), ZoneKeyList.manseFirstFloor + LocationNameList.section3b, facing: Facing.SouthEast, speakAtStartScript: new BeamAndCsalanInManseScript()));

        list.Add(new HorseSpawnDetails(NPCNameList.csalan, new Vector3Int(5, 8), Facing.SouthEast, ZoneKeyList.manseFirstFloor + LocationNameList.section3b)); 

        list.Add(new HorseSpawnDetails(NPCNameList.horse, new Vector3Int(6, 4), Facing.SouthEast, ZoneKeyList.manseFirstFloor + LocationNameList.section3b)); 

        list.Add(new HorseSpawnDetails(NPCNameList.horse+1, new Vector3Int(3, 3), Facing.SouthEast, ZoneKeyList.manseFirstFloor + LocationNameList.section3b));  

        list.Add(new HorseSpawnDetails(NPCNameList.horse+2, new Vector3Int(1, 6), Facing.SouthEast, ZoneKeyList.manseFirstFloor + LocationNameList.section3b));  

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, list);

        #endregion

        #region Manse-1F-3c

        list = new List<OOCSpawnDetails>();

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(2, 7), Facing.SouthEast));
        list.Add(new ShelfSpawnDetails(Constants.indexOne, new Vector3Int(3, 3), Facing.SouthEast));
        list.Add(new ShelfSpawnDetails(Constants.indexTwo, new Vector3Int(8, 3), Facing.SouthWest));
        list.Add(new ShelfSpawnDetails(Constants.indexThree, new Vector3Int(5, 3), Facing.SouthEast));

        list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.secretBookShelfFlag, areaName: ZoneKeyList.manseFirstFloor, sectionName: LocationNameList.section3c, index: Constants.indexOne));

        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.halfWall, new Vector3Int(3, -1), PrefabNames.manseHalfWallSecretDoor, secretDoorFlag: SecretDoorKeyList.secretBookShelfFlag));
        list.Add(new ObstacleWithSecretDoorFlagSpawnDetails(NPCNameList.halfWall, new Vector3Int(2, -1), PrefabNames.manseHalfWallSecretDoor, secretDoorFlag: SecretDoorKeyList.secretBookShelfFlag));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3c, list);

        #endregion

        #region Manse-1F-3d

        list = new List<OOCSpawnDetails>();

        list.Add(new BookSpawnDetails(NPCNameList.diary, new Vector3Int(3, 1), PrefabNames.note, ItemList.pageDiaryFirstEntryIndex));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3d, list);

        #endregion

        #region Manse-1F-3e

        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(1, 2), Facing.SouthEast));
        
        list.Add(new BookSpawnDetails(NPCNameList.diary, new Vector3Int(0, -1), PrefabNames.note, ItemList.pageDiarySecondEntryIndex));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3e, list);

        #endregion

        #endregion

        #region Manse-2F

        #region Manse-2F-1a

        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.honorguard, new Vector3Int(9, 6), ZoneKeyList.manseSecondFloor + LocationNameList.section1a, animationName: MonsterNameList.linebreaker, facing: Facing.SouthWest));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.honorguard, new Vector3Int(9, 5), ZoneKeyList.manseSecondFloor + LocationNameList.section1a, animationName: MonsterNameList.lancer, facing: Facing.SouthWest));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, list);

        #endregion

        #region Manse-2F-1b

        list = new List<OOCSpawnDetails>();

        list.Add(new BookSpawnDetails(NPCNameList.diary, new Vector3Int(6, -3), PrefabNames.note, ItemList.directorsJournalThreeIndex));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section1b, list);

        #endregion

        #region Manse-2F-1c

        list = new List<OOCSpawnDetails>();

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(-2, -3), Facing.SouthWest));
        list.Add(new ShelfSpawnDetails(Constants.indexOne, new Vector3Int(-2, -2), Facing.SouthWest));
        
        list.Add(new BookSpawnDetails(NPCNameList.memo, new Vector3Int(-2, 1), PrefabNames.note, ItemList.brandedMemoIndex));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section1c, list);

        #endregion

        #region Manse-2F-2c

        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(3, 0), ZoneKeyList.manseSecondFloor + LocationNameList.section2c, facing: Facing.SouthWest, speakAtStartScript: new ChiefTaborManseSecondFloorScript()));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(3, -4), Facing.SouthWest, new KeyHalfScript()));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.slave, new Vector3Int(-2,-3), NPCNameList.slave, Facing.NorthEast, animationType: CharacterAnimationType.Idle_Back));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.noBrand, new Vector3Int(-1,0), NPCNameList.noBrand, Facing.NorthEast, animationType: CharacterAnimationType.Idle_Back));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.nandor, new Vector3Int(-1,-4), NPCNameList.nandor, Facing.NorthEast, animationType: CharacterAnimationType.Idle_Back));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.carter, new Vector3Int(-1,-1), NPCNameList.carter, Facing.NorthEast, animationType: CharacterAnimationType.Idle_Back));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section2c, list);

        #endregion

        #region Manse-2F-2d

        list = new List<OOCSpawnDetails>();

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(-4, -2), Facing.SouthEast));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section2d, list);

        #endregion

        #region Manse-2F-3a

        list = new List<OOCSpawnDetails>();

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(5, 11), Facing.SouthEast));

        list.Add(new ShelfSpawnDetails(Constants.indexOne, new Vector3Int(4, 3), Facing.SouthWest));
        list.Add(new ShelfSpawnDetails(Constants.indexTwo, new Vector3Int(4, -1), Facing.SouthWest));
        list.Add(new ShelfSpawnDetails(Constants.indexThree, new Vector3Int(4, -2), Facing.SouthWest));
        list.Add(new ShelfSpawnDetails(Constants.indexFour, new Vector3Int(4, -3), Facing.SouthWest));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(0, 6), VaultableObject.diffThreeVaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(-1, 6), VaultableObject.diffThreeVaultableBarrelsOneTile));

        list.Add(new BookSpawnDetails(NPCNameList.diary, new Vector3Int(4, 5), PrefabNames.note, ItemList.directorsJournalTwoIndex, offset: Constants.onGroundHeightOffset));

        list.Add(new ChestSpawnDetails(Constants.indexFive, new Vector3Int(4, 4), Facing.SouthWest));

        list.Add(new ButtonSpawnDetails(new Vector3Int(-4, 5)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-4, 8)));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3a, list);

        #endregion

        #region Manse-2F-3b

        list = new List<OOCSpawnDetails>();

        list.Add(new WeaponRackSpawnDetails(Constants.indexZero, new Vector3Int(1, 1), Facing.SouthEast, ChestType.SpearRack));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, list);

        #endregion

        #region Manse-2F-3c

        list = new List<OOCSpawnDetails>();

        list.Add(new ShelfSpawnDetails(Constants.indexZero, new Vector3Int(-1, 0), Facing.SouthEast));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3c, list);

        #endregion

        #region Manse-2F-Stockroom
        list = new List<OOCSpawnDetails>();

        list.Add(new LadderSpawnDetails(new Vector3Int(0, -2), PrefabNames.ladderShortNE,
                                        new Ladder(Constants.difficultyThree, ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, 
                                                    ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, 
                                                    Ladder.kitchensLadderDescription, Facing.SouthWest))); //, flipX: Constants.flipX

        list.Add(new ButtonSpawnDetails(new Vector3Int(0, -9)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-7, -11)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(0, -5)));

        list.Add(new CunningBlockerSpawnDetails(Constants.indexZero, new Vector3Int(-1, -7), Facing.NorthWest, CunningObjectSpriteCategory.Crank, endFacing: Facing.SouthEast,
                 allBlockerSpawnDetails: new List<ObstacleSpawnDetails>(){new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(-1, -6), PrefabNames.shackWallHalf)}));

        list.Add(new ButtonSpawnDetails(new Vector3Int(-4, 4), Constants.indexOne));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-5, 4), Constants.indexOne));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-6, 4), Constants.indexOne));
        // list.Add(new CunningBlockerSpawnDetails(Constants.indexTwo, new Vector3Int(-4, -2), Facing.NorthWest, Facing.SouthEast, CunningObjectSpriteCategory.Crank,
        //          new List<ObstacleSpawnDetails>(){new ObstacleSpawnDetails(NPCNameList.halfWall, new Vector3Int(-4, -2), PrefabNames.shackWallHalf)}));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(-7, 7), Facing.SouthEast));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(-8, -6), Facing.NorthEast));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, list);

        #endregion

        #region Manse-2F-Office
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.director, new Vector3Int(2, -1), ZoneKeyList.manseSecondFloor + LocationNameList.office, facing: Facing.SouthWest, extraSpaces: new Vector3Int[]{ new Vector3Int(0, -1) }));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.director+1, new Vector3Int(2, -1), ZoneKeyList.manseSecondFloor + LocationNameList.office, facing: Facing.NorthWest, extraSpaces: new Vector3Int[]{ new Vector3Int(0, -1) }, speakAtStartScript: new PreRevoltDirectorConvoScript()));
        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.page+1, new Vector3Int(-5, 1), ZoneKeyList.manseSecondFloor + LocationNameList.office, facing: Facing.SouthWest, extraSpaces: new Vector3Int[]{ new Vector3Int(-6, 1) })); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.takacs+1, new Vector3Int(-1, -1), facing: Facing.NorthEast)); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.takacs+2, new Vector3Int(2, 0), facing: Facing.SouthEast)); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.chiefTabor, new Vector3Int(-2, -3), facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.chiefTabor+1, new Vector3Int(2, -3), facing: Facing.SouthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft, new Vector3Int(-2, 0), facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.captainAdela, new Vector3Int(2, 1), facing: Facing.SouthWest));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.nandor+1, new Vector3Int(-6, -3), facing: Facing.NorthWest)); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.carter+1, new Vector3Int(-5, -3), facing: Facing.NorthWest)); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.weft+1, new Vector3Int(-5, -2), facing: Facing.NorthWest));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guard, new Vector3Int(-3, 0), animationName: MonsterNameList.lancer, facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.guard+1, new Vector3Int(-3, -3), animationName: MonsterNameList.lancer, facing: Facing.NorthEast));
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.overseerGaspar, new Vector3Int(-2, -3), facing: Facing.NorthEast));

        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.carter, new Vector3Int(-3, -3), facing: Facing.NorthEast)); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.nandor, new Vector3Int(-3, 0), facing: Facing.NorthEast)); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.page, new Vector3Int(2, 2), facing: Facing.SouthEast)); 
        list.Add(new NonDialogueNPCSpawnDetails(NPCNameList.thatch, new Vector3Int(2, -4), facing: Facing.NorthWest)); 

        list.Add(new HiddenTerrainSpawnDetails(secretDoorKeys: new List<string>(){SecretDoorKeyList.meetingRoomSecretEntrance, SecretDoorKeyList.officeSecretEntranceFlag}, areaName: ZoneKeyList.manseSecondFloor, sectionName: LocationNameList.office, index: Constants.indexOne));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(1, 2), VaultableObject.diffTwoVaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(1, 1), VaultableObject.diffTwoVaultableBarrelsOneTile));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(1, -4), VaultableObject.diffTwoVaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(1, -5), VaultableObject.diffTwoVaultableBarrelsOneTile));

        list.Add(new BookSpawnDetails(NPCNameList.orders, new Vector3Int(-6, 0), PrefabNames.note, ItemList.orderTranscriptIndex));
        
        // list.Add(new HiddenTerrainSpawnDetails(SecretDoorKeyList.southEastCampWallPatchThree, ZoneKeyList.manseSecondFloor + LocationNameList.office, Constants.indexOne));

        oocSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.office, list);

        #endregion

        #endregion

        #region Pit

        #region Pit-1a
        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(3, 2), Facing.SouthWest));
        list.Add(new BookSpawnDetails(NPCNameList.orders, new Vector3Int(3, 1), PrefabNames.note, ItemList.pitClosureNoteIndex, offset: Constants.onGroundHeightOffset));

        oocSpawnDetailsDict.Add(ZoneKeyList.pit + LocationNameList.section1a, list);

        #endregion

        #region Pit-2b
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCWithAnimationsSpawnDetails(NPCNameList.brush, new Vector3Int(1, -1), ZoneKeyList.pit + LocationNameList.section2b, facing: Facing.SouthWest, extraSpaces: new Vector3Int[]{new Vector3Int(0, -1)}));

        oocSpawnDetailsDict.Add(ZoneKeyList.pit + LocationNameList.section2b, list);

        #endregion

        #region Pit-2c

        list = new List<OOCSpawnDetails>();

        list.Add(new ButtonSpawnDetails(new Vector3Int(12, -8)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-5, -4)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(-8, 5)));
        list.Add(new ButtonSpawnDetails(new Vector3Int(16, 2)));

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(5, 19), Facing.SouthEast));
        list.Add(new ChestSpawnDetails(Constants.indexOne, new Vector3Int(4, 19), Facing.SouthEast));
        list.Add(new ChestSpawnDetails(Constants.indexTwo, new Vector3Int(3, 19), Facing.SouthEast));

        oocSpawnDetailsDict.Add(ZoneKeyList.pit + LocationNameList.section2c, list);

        #endregion

        #region Pit-2d
        list = new List<OOCSpawnDetails>();

        list.Add(new ChestSpawnDetails(Constants.indexZero, new Vector3Int(-6, 5), Facing.SouthEast));
        list.Add(new BookSpawnDetails(NPCNameList.diary, new Vector3Int(-5, 2), PrefabNames.note, ItemList.directorsJournalFourIndex, offset: Constants.onGroundHeightOffset));

        oocSpawnDetailsDict.Add(ZoneKeyList.pit + LocationNameList.section2d, list);

        #endregion

        #endregion

    }

}
