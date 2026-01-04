using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyPackInfoList
{
    public readonly static string[] flagsToCheckForSlaveAllies = new string[] { FlagNameList.convincedSlavesToHelpYou, FlagNameList.kastorStartedRevolt };

    public readonly static EnemyPackInfo testFight = new EnemyPackInfo(new EnemyAmount[] {      
                                                                                                EnemyAmountList.twoDisciplinarians,
                                                                                                EnemyAmountList.twoExecutioners,
                                                                                                EnemyAmountList.twoJavelineers,
                                                                                                EnemyAmountList.twoLancers,
                                                                                                EnemyAmountList.twoLieutenants,
                                                                                                EnemyAmountList.twoLineBreakers,
                                                                                                EnemyAmountList.twoSignaleers,
                                                                                                EnemyAmountList.twoSpearmen
                                                                                                },
                                                                                                DropTableList.slaveMineDT1Name);

    #region Named Lovashi Guard Fights

    public readonly static EnemyPackInfo guardVazulFight = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.guardVazul }, DropTableList.slaveMineDT1Name,
                                                                                    new ItemListID[]  {new ItemListID(ItemList.usableItemListIndex, ItemList.chewIndex, Constants.sizeThree),
                                                                                                        new ItemListID(ItemList.weaponsListIndex, ItemList.bronzeDirkIndex)});

    public readonly static EnemyPackInfo guardAndrasWithKeyFight = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.guardAndras }, DropTableList.slaveMineDT1Name,
                                                                                    new ItemListID[]  {new ItemListID(ItemList.armorListIndex, ItemList.luckyTalismanIndex),
                                                                                                        new ItemListID(ItemList.keyItemListIndex, ItemList.mineArmoryKeyIndex),
                                                                                                        new ItemListID(ItemList.keyItemListIndex, ItemList.barracksArmoryKeyIndex)});
    public readonly static EnemyPackInfo guardAndrasWithOutKeyFight = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.guardAndras }, DropTableList.slaveMineDT1Name,
                                                                                        new ItemListID[] { new ItemListID(ItemList.armorListIndex, ItemList.luckyTalismanIndex),
                                                                                                        new ItemListID(ItemList.keyItemListIndex, ItemList.barracksArmoryKeyIndex)});
    public readonly static EnemyPackInfo imreFight = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.imre }, DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo ml3GuardsWithBarricades = new EnemyPackInfo(new EnemyAmount[] { 
                                                                                                        // EnemyAmountList.barricade, 
                                                                                                        EnemyAmountList.guardReka, 
                                                                                                        EnemyAmountList.guardPazman, 
                                                                                                        EnemyAmountList.overseerGaspar, 
                                                                                                        EnemyAmountList.guardVirag 
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                    new ItemListID[]  {new ItemListID(ItemList.questItemListIndex, ItemList.blastingJellyIndex)});

    public readonly static EnemyPackInfo ml3GuardsWithoutBarricades = new EnemyPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.guardReka, 
                                                                                                        EnemyAmountList.guardPazman, 
                                                                                                        EnemyAmountList.overseerGaspar, 
                                                                                                        EnemyAmountList.guardVirag 
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                    new ItemListID[]  {new ItemListID(ItemList.questItemListIndex, ItemList.blastingJellyIndex)});

    public readonly static EnemyPackInfo ml3GuardsNoSurrenders = new EnemyPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.guardReka, 
                                                                                                        EnemyAmountList.guardPazman, 
                                                                                                        EnemyAmountList.overseerGaspar, 
                                                                                                        EnemyAmountList.guardVirag 
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo ml3GuardsRekaPazmanSurrender = new EnemyPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.overseerGaspar, 
                                                                                                        EnemyAmountList.guardVirag 
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name);

    #endregion

    #region  Generic Lovashi Guard Fights

    public readonly static EnemyPackInfo barricadeGuardsFront = new EnemyPackInfo(new EnemyAmount[] {      
                                                                                                EnemyAmountList.oneSignaleer,
                                                                                                EnemyAmountList.oneDisciplinarian,
                                                                                                EnemyAmountList.twoSpearmen,
                                                                                                EnemyAmountList.twoAxemen
                                                                                                },
                                                                                                DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo barricadeGuardsBehind = new EnemyPackInfo(new EnemyAmount[] {      
                                                                                                        EnemyAmountList.oneSignaleer,
                                                                                                        EnemyAmountList.oneDisciplinarian,
                                                                                                        EnemyAmountList.twoSpearmen,
                                                                                                        EnemyAmountList.twoAxemen
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneSignaleerOneAxemanOneSpearmenTwoJavalineers = new EnemyPackInfo(new EnemyAmount[] {      
                                                                                                        EnemyAmountList.oneSignaleer,
                                                                                                        EnemyAmountList.oneAxeman,
                                                                                                        EnemyAmountList.oneSpearman,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneDisciplinarianTwoSpearmenTwoJavalineers = new EnemyPackInfo(new EnemyAmount[] {      
                                                                                                        EnemyAmountList.oneDisciplinarian,
                                                                                                        EnemyAmountList.twoSpearmen,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneLieutenantOneAxemanOneSpearmanThreeJavalineers = new EnemyPackInfo(new EnemyAmount[] {     
                                                                                                        EnemyAmountList.oneLieutenant, 
                                                                                                        EnemyAmountList.oneAxeman,
                                                                                                        EnemyAmountList.oneSpearman,
                                                                                                        EnemyAmountList.threeJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo twoSpearmenTwoAxemenTwoJavalineers = new EnemyPackInfo(new EnemyAmount[] {      
                                                                                                        EnemyAmountList.twoSpearmen,
                                                                                                        EnemyAmountList.twoAxemen,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo twoAxemenTwoSpearmenTwoJavalineers = new EnemyPackInfo(new EnemyAmount[] {      
                                                                                                        EnemyAmountList.twoAxemen,
                                                                                                        EnemyAmountList.twoSpearmen,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    #endregion

    public readonly static BossPackInfo campNorthEastOverseerBoss = new BossPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.oneDisciplinarian
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.neCampOverseerKilled,
                                                                                                        DialogueNameList.slavesAfterKillingOverseerCampNEKey);

    public readonly static BossPackInfo kendeKitchensHalfSlavesNoGuard = new BossPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.kendeUponEnteringKitchens,
                                                                                                        new KendeFightQuestScript());

    public readonly static BossPackInfo kendeKitchensHalfSlaves = new BossPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.kendeUponEnteringKitchens,
                                                                                                        new KendeFightQuestScript());

    public readonly static BossPackInfo kendeKitchensFullSlavesNoGuard = new BossPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.kendeUponEnteringKitchens,
                                                                                                        new KendeFightQuestScript());

    public readonly static BossPackInfo kendeKitchensFullSlaves = new BossPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.kendeUponEnteringKitchens,
                                                                                                        new KendeFightQuestScript());

    public readonly static BossPackInfo taborManseSecondFloorFight = new BossPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.killedTaborInManse);

    public readonly static BossPackInfo honorguardCaptainBossFight = new BossPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        new ItemListID[]  {new ItemListID(ItemList.keyItemListIndex, ItemList.directorsOfficeKeyBackIndex)},
                                                                                                        FlagNameList.honorguardCaptainKilled,
                                                                                                        new KeyHalfScript());

    public readonly static BossPackInfo directorWithBarricades = new BossPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.directorDefeated,
                                                                                                        DialogueNameList.directorDefeatedConvoKey);

    public readonly static BossPackInfo directorWithoutBarricades = new BossPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.directorDefeated,
                                                                                                        DialogueNameList.directorDefeatedConvoKey);

    public readonly static BossPackInfo beamAndCsalanFight = new BossPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.foughtHorsesInManse);

    public readonly static BossPackInfo clayFightForTabor = new BossPackInfo(new EnemyAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.foughtCrowdForTabor,
                                                                                                        DialogueNameList.taborAfterClayFightKey);

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

    private readonly static EnemyPackInfo twoGiantBatsTwoBatSwarms = new EnemyPackInfo(new EnemyAmount[] {  EnemyAmountList.twoGiantBats,
                                                                                                            EnemyAmountList.twoBatSwarms
                                                                                                            },
                                                                                                          DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo twoGiantBatsThreeBatSwarmsOneArmoredBat = new EnemyPackInfo(new EnemyAmount[] {  EnemyAmountList.oneArmoredBat,
                                                                                                                            EnemyAmountList.twoGiantBats,
                                                                                                                            EnemyAmountList.threeBatSwarms
                                                                                                                            },
                                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat = new EnemyPackInfo(new EnemyAmount[] {  EnemyAmountList.oneDenMother,
                                                                                                                                    EnemyAmountList.oneGiantBat,
                                                                                                                                    EnemyAmountList.oneArmoredBat,
                                                                                                                                    EnemyAmountList.threeBatSwarms
                                                                                                                                 },
                                                                                                                                DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo twoGiantBatsThreeBatSwarmsOneScreecher = new EnemyPackInfo(new EnemyAmount[] {  EnemyAmountList.oneScreecherBat,
                                                                                                                            EnemyAmountList.twoGiantBats,
                                                                                                                            EnemyAmountList.threeBatSwarms
                                                                                                                            },
                                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo oneArmoredBatOneScreecherOneDenMother = new EnemyPackInfo(new EnemyAmount[] {  EnemyAmountList.oneArmoredBat,
                                                                                                                            EnemyAmountList.oneScreecherBat,
                                                                                                                            EnemyAmountList.oneDenMother
                                                                                                                            },
                                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo twoArmoredBatsOneDenMotherOneGiantBatTwoBatSwarm = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.twoArmoredBats,
                                                                                                                                    EnemyAmountList.oneDenMother,
                                                                                                                                    EnemyAmountList.oneGiantBat,
                                                                                                                                    EnemyAmountList.twoBatSwarms},
                                                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo twoGiantBatsTwoBatSwarmsTwoArmoredBats = new EnemyPackInfo(new EnemyAmount[] {  EnemyAmountList.twoGiantBats,
                                                                                                                            EnemyAmountList.twoBatSwarms,
                                                                                                                            EnemyAmountList.twoArmoredBats
                                                                                                                            },
                                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo threeDenMothersThreeBatSwarmsOneArmoredBat = new EnemyPackInfo(new EnemyAmount[] {  EnemyAmountList.threeDenMothers,
                                                                                                                            EnemyAmountList.threeBatSwarms,
                                                                                                                            EnemyAmountList.oneArmoredBat
                                                                                                                            },
                                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo caveMatronBatBoss = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.caveMatron },
                                                                                                    DropTableList.slaveMineDT1Name,
                                                                                                    new ItemListID[] { new ItemListID(  ItemList.keyItemListIndex,
                                                                                                                                        ItemList.mineArmoryKeyIndex) });

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

        list.Add(testFight);
        // list.Add(oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat);

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
        #endregion

        #region Manse-1F

        #endregion

        #region Manse-2F

        #region Manse-2F-3b

        list = new List<EnemyPackInfo>();

        list.Add(honorguardCaptainBossFight);

        enemyPackInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, list);

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
