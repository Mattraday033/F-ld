using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyPackInfoList
{
    public readonly static string[] flagsToCheckForSlaveAllies = new string[] { FlagNameList.convincedSlavesToHelpYou, FlagNameList.kastorStartedRevolt };

    public readonly static EnemyPackInfo testFight = new EnemyPackInfo(new CreatureAmount[] {      
                                                                                                EnemyAmountList.barricade, 
                                                                                                // EnemyAmountList.twoDisciplinarians,
                                                                                                // EnemyAmountList.twoExecutioners,
                                                                                                // EnemyAmountList.twoJavelineers,
                                                                                                // EnemyAmountList.twoLancers,
                                                                                                // EnemyAmountList.twoLieutenants,
                                                                                                // EnemyAmountList.twoLinebreakers,
                                                                                                // EnemyAmountList.twoSignaleers,
                                                                                                EnemyAmountList.twoSpearmen
                                                                                                },
                                                                                                DropTableList.slaveMineDT1Name);

    #region Named Lovashi Guard Fights

    public readonly static EnemyPackInfo guardVazulFight = new EnemyPackInfo(new CreatureAmount[] { EnemyAmountList.guardVazul }, DropTableList.slaveMineDT1Name,
                                                                                    new ItemListID[]  {new ItemListID(ItemList.usableItemListIndex, ItemList.chewIndex, Constants.sizeThree),
                                                                                                        new ItemListID(ItemList.armorListIndex, ItemList.bronzeDirkIndex)});

    public readonly static EnemyPackInfo guardAndrasWithKeyFight = new EnemyPackInfo(new CreatureAmount[] { EnemyAmountList.guardAndras }, DropTableList.slaveMineDT1Name,
                                                                                    new ItemListID[]  {new ItemListID(ItemList.armorListIndex, ItemList.luckyTalismanIndex),
                                                                                                        new ItemListID(ItemList.keyItemListIndex, ItemList.mineArmoryKeyIndex),
                                                                                                        new ItemListID(ItemList.keyItemListIndex, ItemList.barracksArmoryKeyIndex)});
    public readonly static EnemyPackInfo guardAndrasWithOutKeyFight = new EnemyPackInfo(new CreatureAmount[] { EnemyAmountList.guardAndras }, DropTableList.slaveMineDT1Name,
                                                                                        new ItemListID[] { new ItemListID(ItemList.armorListIndex, ItemList.luckyTalismanIndex),
                                                                                                        new ItemListID(ItemList.keyItemListIndex, ItemList.barracksArmoryKeyIndex)});
    public readonly static EnemyPackInfo imreFight = new EnemyPackInfo(new CreatureAmount[] { EnemyAmountList.imre }, DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo ml3GuardsWithBarricades = new EnemyPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.barricade, 
                                                                                                        EnemyAmountList.guardReka, 
                                                                                                        EnemyAmountList.guardPazman, 
                                                                                                        EnemyAmountList.overseerGaspar, 
                                                                                                        EnemyAmountList.guardVirag 
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                    new ItemListID[]  {new ItemListID(ItemList.questItemListIndex, ItemList.blastingJellyIndex)});

    public readonly static EnemyPackInfo ml3GuardsWithoutBarricades = new EnemyPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.guardReka, 
                                                                                                        EnemyAmountList.guardPazman, 
                                                                                                        EnemyAmountList.overseerGaspar, 
                                                                                                        EnemyAmountList.guardVirag 
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                    new ItemListID[]  {new ItemListID(ItemList.questItemListIndex, ItemList.blastingJellyIndex)});

    public readonly static EnemyPackInfo ml3GuardsNoSurrenders = new EnemyPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.guardReka, 
                                                                                                        EnemyAmountList.guardPazman, 
                                                                                                        EnemyAmountList.overseerGaspar, 
                                                                                                        EnemyAmountList.guardVirag 
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo ml3GuardsRekaPazmanSurrender = new EnemyPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.overseerGaspar, 
                                                                                                        EnemyAmountList.guardVirag 
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name);

    #endregion

    #region  Generic Lovashi Guard Fights

    public readonly static EnemyPackInfo barricadeGuardsFront = new EnemyPackInfo(new CreatureAmount[] {      
                                                                                                EnemyAmountList.barricade,
                                                                                                EnemyAmountList.oneSignaleer,
                                                                                                EnemyAmountList.oneDisciplinarian,
                                                                                                EnemyAmountList.twoSpearmen,
                                                                                                EnemyAmountList.twoAxemen
                                                                                                },
                                                                                                DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo barricadeGuardsBehind = new EnemyPackInfo(new CreatureAmount[] {      
                                                                                                        EnemyAmountList.oneSignaleer,
                                                                                                        EnemyAmountList.oneDisciplinarian,
                                                                                                        EnemyAmountList.twoSpearmen,
                                                                                                        EnemyAmountList.twoAxemen
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneSignaleerOneAxemanOneSpearmenTwoJavalineers = new EnemyPackInfo(new CreatureAmount[] {      
                                                                                                        EnemyAmountList.oneSignaleer,
                                                                                                        EnemyAmountList.oneAxeman,
                                                                                                        EnemyAmountList.oneSpearman,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneDisciplinarianTwoSpearmenTwoJavalineers = new EnemyPackInfo(new CreatureAmount[] {      
                                                                                                        EnemyAmountList.oneDisciplinarian,
                                                                                                        EnemyAmountList.twoSpearmen,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneLieutenantOneAxemanOneSpearmanThreeJavalineers = new EnemyPackInfo(new CreatureAmount[] {     
                                                                                                        EnemyAmountList.oneLieutenant, 
                                                                                                        EnemyAmountList.oneAxeman,
                                                                                                        EnemyAmountList.oneSpearman,
                                                                                                        EnemyAmountList.threeJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo twoSpearmenTwoAxemenTwoJavalineers = new EnemyPackInfo(new CreatureAmount[] {      
                                                                                                        EnemyAmountList.twoSpearmen,
                                                                                                        EnemyAmountList.twoAxemen,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo twoAxemenTwoSpearmenTwoJavalineers = new EnemyPackInfo(new CreatureAmount[] {      
                                                                                                        EnemyAmountList.twoAxemen,
                                                                                                        EnemyAmountList.twoSpearmen,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    #endregion

    public readonly static BossPackInfo campNorthEastOverseerBoss = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.oneOverseer,
                                                                                                        EnemyAmountList.eightBrandedConscripts,
                                                                                                        EnemyAmountList.twoSpearmen
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.neCampOverseerKilled,
                                                                                                        DialogueNameList.slavesAfterKillingOverseerCampNEKey,
                                                                                                        xpDrop: 100);

    public readonly static BossPackInfo kendeKitchensHalfSlavesNoGuard = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.kende,
                                                                                                        EnemyAmountList.fourNonBrandedLoyalists
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.kendeUponEnteringKitchens,
                                                                                                        script: new KendeFightQuestScript(),
                                                                                                        xpDrop: 100);

    public readonly static BossPackInfo kendeKitchensHalfSlaves = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.kende,
                                                                                                        EnemyAmountList.fourNonBrandedLoyalists,
                                                                                                        EnemyAmountList.oneLinebreaker
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.kendeUponEnteringKitchens,
                                                                                                        script: new KendeFightQuestScript());

    public readonly static BossPackInfo kendeKitchensFullSlavesNoGuard = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.kende,
                                                                                                        EnemyAmountList.eightNonBrandedLoyalists
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.kendeUponEnteringKitchens,
                                                                                                        script: new KendeFightQuestScript());

    public readonly static BossPackInfo kendeKitchensFullSlaves = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.kende,
                                                                                                        EnemyAmountList.eightNonBrandedLoyalists,
                                                                                                        EnemyAmountList.oneLinebreaker
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.kendeUponEnteringKitchens,
                                                                                                        script: new KendeFightQuestScript());

    public readonly static BossPackInfo taborManseSecondFloorFight = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.killedTaborInManse);

    public readonly static BossPackInfo honorguardCaptainBossFight = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.honorguardCaptainKilled,
                                                                                                        guaranteedDrops: new ItemListID[]  {new ItemListID(ItemList.keyItemListIndex, ItemList.directorsOfficeKeyBackIndex)},
                                                                                                        script: new KeyHalfScript(),
                                                                                                        xpDrop: 100);

    public readonly static BossPackInfo directorWithBarricades = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.barricade,
                                                                                                        EnemyAmountList.director
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.directorDefeated,
                                                                                                        DialogueNameList.directorDefeatedConvoKey,
                                                                                                        xpDrop: 400);

    public readonly static BossPackInfo directorWithoutBarricades = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.director
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.directorDefeated,
                                                                                                        DialogueNameList.directorDefeatedConvoKey,
                                                                                                        xpDrop: 400);

    public readonly static BossPackInfo beamAndCsalanFight = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.foughtHorsesInManse, 
                                                                                                        xpDrop: 100);

    public readonly static BossPackInfo clayFightForTabor = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.foughtCrowdForTabor,
                                                                                                        DialogueNameList.taborAfterClayFightKey,
                                                                                                        xpDrop: 100);

    // //used in the dialogue started upon entering the Manse kitchens
    // public readonly static EnemyPackInfo halfSlavesNoGuardFight = new EnemyPackInfo(new int[] { 1, 6 }, new int[] { 1, 6 }, new EnemyStats[]{loadEnemyStatsFromResources(kendeTheCookWithoutSummon),
    //                                                                                                                         loadEnemyStatsFromResources(slaveWarrior)
    //                                                                                                                        },
    //                                                                                                                         flagsToCheckForSlaveAllies,
    //                                                                                                                         DropTableList.slaveMineDT1Name,
    //                                                                                                                         new KendeFightQuestScript());

    // //used in the dialogue started upon entering the Manse kitchens
    // public readonly static EnemyPackInfo halfSlavesFight = new EnemyPackInfo(new int[] { 1, 6, 2 }, new int[] { 1, 6, 2 }, new EnemyStats[]{loadEnemyStatsFromResources(kendeTheCookWithoutSummon),
    //                                                                                                                      loadEnemyStatsFromResources(slaveWarrior),
    //                                                                                                                      loadEnemyStatsFromResources(kitchenGuards)
    //                                                                                                                     },
    //                                                                                                                     flagsToCheckForSlaveAllies,
    //                                                                                                                     DropTableList.slaveMineDT1Name,
    //                                                                                                                     new KendeFightQuestScript());

    // //used in the dialogue started upon entering the Manse kitchens
    // public readonly static EnemyPackInfo fullSlavesNoGuardFight = new EnemyPackInfo(new int[] { 1, 12 }, new int[] { 1, 10 }, new EnemyStats[]{loadEnemyStatsFromResources(kendeTheCook),
    //                                                                                                                           loadEnemyStatsFromResources(slaveWarrior)
    //                                                                                                                          },
    //                                                                                                                           flagsToCheckForSlaveAllies,
    //                                                                                                                           DropTableList.slaveMineDT1Name,
    //                                                                                                                           new KendeFightQuestScript());

    // //used in the dialogue started upon entering the Manse kitchens
    // public readonly static EnemyPackInfo fullSlavesFight = new EnemyPackInfo(new int[] { 1, 12, 2 }, new int[] { 1, 10, 2 }, new EnemyStats[]{loadEnemyStatsFromResources(kendeTheCook),
    //                                                                                                                        loadEnemyStatsFromResources(slaveWarrior),
    //                                                                                                                        loadEnemyStatsFromResources(kitchenGuards)
    //                                                                                                                       },
    //                                                                                                                        flagsToCheckForSlaveAllies,
    //                                                                                                                        DropTableList.slaveMineDT1Name,
    //                                                                                                                        new KendeFightQuestScript());

    // public readonly static EnemyPackInfo taborFight = new EnemyPackInfo(new int[] { 1 }, new int[] { 1 }, new EnemyStats[] { loadEnemyStatsFromResources(chiefTabor) }, flagsToCheckForSlaveAllies, DropTableList.slaveMineDT1Name);


    private static Dictionary<string, List<EnemyPackInfo>> enemyPackInfoDict;

    #region Bats

    private readonly static EnemyPackInfo twoGiantBatsTwoBatSwarms = new EnemyPackInfo(new CreatureAmount[] {  EnemyAmountList.twoGiantBats,
                                                                                                            EnemyAmountList.twoBatSwarms
                                                                                                            },
                                                                                                          DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo twoGiantBatsThreeBatSwarmsOneArmoredBat = new EnemyPackInfo(new CreatureAmount[] {  
                                                                                                                            EnemyAmountList.oneArmoredBatShielded,
                                                                                                                            EnemyAmountList.twoGiantBats,
                                                                                                                            EnemyAmountList.threeBatSwarms
                                                                                                                        },
                                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                                        TutorialSequenceList.traitTutorialSequenceKey);

    private readonly static EnemyPackInfo oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat = new EnemyPackInfo(new CreatureAmount[] {  EnemyAmountList.oneDenMother,
                                                                                                                                    EnemyAmountList.oneGiantBat,
                                                                                                                                    EnemyAmountList.oneArmoredBatShielded,
                                                                                                                                    EnemyAmountList.threeBatSwarms
                                                                                                                                 },
                                                                                                                                DropTableList.slaveMineDT1Name,
                                                                                                                                TutorialSequenceList.traitTutorialSequenceKey);

    private readonly static EnemyPackInfo twoGiantBatsThreeBatSwarmsOneScreecher = new EnemyPackInfo(new CreatureAmount[] {  EnemyAmountList.oneScreecherBat,
                                                                                                                            EnemyAmountList.twoGiantBats,
                                                                                                                            EnemyAmountList.threeBatSwarms
                                                                                                                            },
                                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo oneArmoredBatOneScreecherOneDenMother = new EnemyPackInfo(new CreatureAmount[] {  EnemyAmountList.oneArmoredBat,
                                                                                                                            EnemyAmountList.oneScreecherBat,
                                                                                                                            EnemyAmountList.oneDenMother
                                                                                                                            },
                                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo twoArmoredBatsOneDenMotherOneGiantBatTwoBatSwarm = new EnemyPackInfo(new CreatureAmount[] { 
                                                                                                                                    EnemyAmountList.oneArmoredBat,
                                                                                                                                    EnemyAmountList.oneArmoredBatShielded,
                                                                                                                                    EnemyAmountList.oneDenMother,
                                                                                                                                    EnemyAmountList.oneGiantBat,
                                                                                                                                    EnemyAmountList.twoBatSwarms
                                                                                                                                  },
                                                                                                                                    DropTableList.slaveMineDT1Name,
                                                                                                                                TutorialSequenceList.traitTutorialSequenceKey);

    private readonly static EnemyPackInfo twoGiantBatsTwoBatSwarmsTwoArmoredBats = new EnemyPackInfo(new CreatureAmount[] {  EnemyAmountList.twoGiantBats,
                                                                                                                            EnemyAmountList.twoBatSwarms, 
                                                                                                                            EnemyAmountList.oneArmoredBat,
                                                                                                                            EnemyAmountList.oneArmoredBatShielded,
                                                                                                                            },
                                                                                                                            DropTableList.slaveMineDT1Name,
                                                                                                                            TutorialSequenceList.traitTutorialSequenceKey);

    private readonly static EnemyPackInfo threeDenMothersThreeBatSwarmsOneArmoredBat = new EnemyPackInfo(new CreatureAmount[] {  EnemyAmountList.threeDenMothers,
                                                                                                                            EnemyAmountList.threeBatSwarms,
                                                                                                                            EnemyAmountList.oneArmoredBat
                                                                                                                            },
                                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo caveMatronBatBoss = new BossPackInfo(new CreatureAmount[] { EnemyAmountList.caveMatron },
                                                                                                    DropTableList.slaveMineDT1Name,
                                                                                                    guaranteedDrops: new ItemListID[] { new ItemListID(  ItemList.keyItemListIndex,
                                                                                                                                        ItemList.mineArmoryKeyIndex) }, 
                                                                                                                                        xpDrop: 100);

    private readonly static EnemyPackInfo wormBoss = new BossPackInfo(new CreatureAmount[] {  EnemyAmountList.threeDenMothers,
                                                                                                EnemyAmountList.threeBatSwarms,
                                                                                                EnemyAmountList.oneArmoredBat
                                                                                                },
                                                                                                DropTableList.slaveMineDT1Name, 
                                                                                                xpDrop: 100);
    #endregion

    public static EnemyPackInfo getEnemyPackInfo(string areaName, int index)
    {
        if (!enemyPackInfoDict.ContainsKey(areaName))
        {
            return twoGiantBatsTwoBatSwarmsTwoArmoredBats;
        }

        return enemyPackInfoDict[areaName][index];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeEnemyPackInfoList()
    {
        List<EnemyPackInfo> list;
        enemyPackInfoDict = new Dictionary<string, List<EnemyPackInfo>>();


        #region Slave Shack Six
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsTwoBatSwarms);
        list.Add(twoGiantBatsTwoBatSwarms);

        enemyPackInfoDict.Add(LocationNameList.slaveShackSix, list);
        #endregion

        #region GuardHouse NE
        list = new List<EnemyPackInfo>();

        list.Add(twoAxemenTwoSpearmenTwoJavalineers);

        enemyPackInfoDict.Add(LocationNameList.guardHouseNorthEast, list);
        #endregion

        #region GuardHouse SW
        list = new List<EnemyPackInfo>();

        list.Add(twoSpearmenTwoAxemenTwoJavalineers);

        enemyPackInfoDict.Add(LocationNameList.guardHouseSouthWest, list);
        #endregion

        #region North East Camp
        list = new List<EnemyPackInfo>();

        list.Add(campNorthEastOverseerBoss);

        enemyPackInfoDict.Add(LocationNameList.campNorthEast, list);
        #endregion

        #region Center Camp
        list = new List<EnemyPackInfo>();

        list.Add(oneDisciplinarianTwoSpearmenTwoJavalineers);
        list.Add(oneLieutenantOneAxemanOneSpearmanThreeJavalineers);
        list.Add(twoSpearmenTwoAxemenTwoJavalineers);
        list.Add(oneSignaleerOneAxemanOneSpearmenTwoJavalineers);
        list.Add(twoAxemenTwoSpearmenTwoJavalineers);

        enemyPackInfoDict.Add(LocationNameList.campCenter, list);
        #endregion

        #region South East Camp
        list = new List<EnemyPackInfo>();

        list.Add(twoAxemenTwoSpearmenTwoJavalineers);
        list.Add(twoSpearmenTwoAxemenTwoJavalineers);
        list.Add(oneLieutenantOneAxemanOneSpearmanThreeJavalineers);
        list.Add(oneSignaleerOneAxemanOneSpearmenTwoJavalineers);
        list.Add(oneDisciplinarianTwoSpearmenTwoJavalineers);

        enemyPackInfoDict.Add(LocationNameList.campSouthEast, list);
        #endregion

        #region Manse Camp
        list = new List<EnemyPackInfo>();

        list.Add(oneDisciplinarianTwoSpearmenTwoJavalineers);
        list.Add(oneLieutenantOneAxemanOneSpearmanThreeJavalineers);
        list.Add(oneSignaleerOneAxemanOneSpearmenTwoJavalineers);

        enemyPackInfoDict.Add(LocationNameList.campManse, list);
        #endregion

        #region Mine

        #region MineLvl_1-1b
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsThreeBatSwarmsOneArmoredBat);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1b, list);
        #endregion

        #region MineLvl_2-1b
        list = new List<EnemyPackInfo>();

        list.Add(oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1b, list);
        #endregion

        #region MineLvl_2-1c
        list = new List<EnemyPackInfo>();

        list.Add(oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat);
        list.Add(twoGiantBatsThreeBatSwarmsOneArmoredBat);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1c, list);
        #endregion

        #region MineLvl_2-2b
        list = new List<EnemyPackInfo>();

        list.Add(twoArmoredBatsOneDenMotherOneGiantBatTwoBatSwarm);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2b, list);
        #endregion

        #region MineLvl_2-3a
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section3a, list);
        #endregion

        #region MineLvl_2-3b
        list = new List<EnemyPackInfo>();

        list.Add(oneArmoredBatOneScreecherOneDenMother);
        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);
        list.Add(oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section3b, list);
        #endregion

        #region MineLvl_2-4
        list = new List<EnemyPackInfo>();


        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);
        list.Add(twoArmoredBatsOneDenMotherOneGiantBatTwoBatSwarm);
        list.Add(threeDenMothersThreeBatSwarmsOneArmoredBat);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section4, list);
        #endregion

       #region MineLvl_2-5
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);
        list.Add(twoArmoredBatsOneDenMotherOneGiantBatTwoBatSwarm);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section5, list);
        #endregion

        #region MineLvl_2-7b
        list = new List<EnemyPackInfo>();

        list.Add(oneArmoredBatOneScreecherOneDenMother);
        list.Add(oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat);
        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);

        list.Add(caveMatronBatBoss);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section7b, list);
        #endregion


        #region MineLvl_3-1a
        list = new List<EnemyPackInfo>();

        list.Add(oneArmoredBatOneScreecherOneDenMother);
        list.Add(oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat);
        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1a, list);
        #endregion

        #region MineLvl_3-7
        list = new List<EnemyPackInfo>();

        list.Add(wormBoss);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section7, list);
        #endregion

        #endregion

        #region Manse-1F

        #region Manse-1F-1c

        list = new List<EnemyPackInfo>();

        list.Add(twoSpearmenTwoAxemenTwoJavalineers);
        list.Add(twoSpearmenTwoAxemenTwoJavalineers);
        list.Add(twoSpearmenTwoAxemenTwoJavalineers);
        list.Add(twoSpearmenTwoAxemenTwoJavalineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section1c, list);

        #endregion

        #region Manse-1F-Dining Room

        list = new List<EnemyPackInfo>();

        list.Add(twoSpearmenTwoAxemenTwoJavalineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, list);

        #endregion

        #region Manse-1F-2a

        list = new List<EnemyPackInfo>();

        list.Add(twoSpearmenTwoAxemenTwoJavalineers);
        list.Add(twoSpearmenTwoAxemenTwoJavalineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, list);

        #endregion

        #region Manse-1F-2b

        list = new List<EnemyPackInfo>();

        list.Add(twoSpearmenTwoAxemenTwoJavalineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, list);

        #endregion

        #endregion

        #region Manse-2F

        #region Manse-2F-3a

        list = new List<EnemyPackInfo>();

        list.Add(twoSpearmenTwoAxemenTwoJavalineers);
        list.Add(twoSpearmenTwoAxemenTwoJavalineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3a, list);

        #endregion

        #region Manse-2F-3b

        list = new List<EnemyPackInfo>();

        list.Add(honorguardCaptainBossFight);

        enemyPackInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, list);

        #endregion

        #region Manse-2F-Stockroom

        list = new List<EnemyPackInfo>();

        list.Add(twoSpearmenTwoAxemenTwoJavalineers);
        list.Add(twoSpearmenTwoAxemenTwoJavalineers);
        list.Add(twoSpearmenTwoAxemenTwoJavalineers);
        list.Add(twoSpearmenTwoAxemenTwoJavalineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, list);

        #endregion

        #region Manse-2F-Office

        list = new List<EnemyPackInfo>();

        list.Add(twoSpearmenTwoAxemenTwoJavalineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.office, list);

        #endregion

        #endregion

        #region Pit

        #region Pit-1b

        list = new List<EnemyPackInfo>();

        list.Add(twoSpearmenTwoAxemenTwoJavalineers);

        enemyPackInfoDict.Add(ZoneKeyList.pit + LocationNameList.section1b, list);

        #endregion

        #endregion
    }





    private static EnemyStats loadEnemyStatsFromResources(string enemyStatsName)
    {
        EnemyStats loadedStats = Resources.Load<EnemyStats>(enemyStatsName);

        if (loadedStats == null)
        {
            Debug.LogError("Couldn't find any EnemyStats object named: '" + enemyStatsName + "'");
        }

        return loadedStats;
    }



}
