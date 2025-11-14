using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyPackInfoList
{
    private const int quantityOfTwo = 2;
    private const int quantityOfThree = 3;
    private const int quantityOfFour = 4;

    public const string kendeTheCook = "KendeTheCook";
    public const string kendeTheCookWithoutSummon = "KendeTheCookWithoutSummon";
    public const string slaveWarrior = "Slave Warrior";
    public const string kitchenGuards = "GuardJavalineer";

    public const string chiefTabor = "ChiefTabor";

    public readonly static string[] flagsToCheckForSlaveAllies = new string[] { "convincedSlavesToHelpYou", "kastorStartedRevolt" };

    public readonly static EnemyPackInfo guardVazulFight = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.guardVazul }, DropTableList.slaveMineDT1Name,
                                                                                    new ItemListID[]  {new ItemListID(ItemList.usableItemListIndex, ItemList.chewIndex, quantityOfThree),
                                                                                                        new ItemListID(ItemList.weaponsListIndex, ItemList.bronzeDirkIndex)});

    public readonly static EnemyPackInfo guardAndrasWithKeyFight = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.guardAndras }, DropTableList.slaveMineDT1Name,
                                                                                    new ItemListID[]  {new ItemListID(ItemList.armorListIndex, ItemList.andrasLuckyTalismanIndex),
                                                                                                        new ItemListID(ItemList.keyItemListIndex, ItemList.mineArmoryKeyIndex)});
    public readonly static EnemyPackInfo guardAndrasWithOutKeyFight = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.guardAndras }, DropTableList.slaveMineDT1Name,
                                                                                        new ItemListID[] { new ItemListID(ItemList.armorListIndex, ItemList.andrasLuckyTalismanIndex) });
    public readonly static EnemyPackInfo imreFight = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.imre }, DropTableList.slaveMineDT1Name);


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

    private readonly static EnemyPackInfo twoGiantBats = new EnemyPackInfo(new EnemyAmount[] {  EnemyAmountList.twoGiantBats},
                                                                                                DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo twoGiantBatsTwoBatSwarms = new EnemyPackInfo(new EnemyAmount[] {  EnemyAmountList.twoGiantBats,
                                                                                                            EnemyAmountList.twoBatSwarms
                                                                                                            },
                                                                                                          DropTableList.slaveMineDT1Name);
    private readonly static EnemyPackInfo twoGiantBatsThreeBatSwarmsOneScreecher = new EnemyPackInfo(new EnemyAmount[] {  EnemyAmountList.twoGiantBats,
                                                                                                                            EnemyAmountList.oneScreecherBat,
                                                                                                                            EnemyAmountList.threeBatSwarms
                                                                                                                            },
                                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo twoArmoredBats = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.twoArmoredBats},
                                                                                            DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo twoGiantBatsTwoBatSwarmsTwoArmoredBats = new EnemyPackInfo(new EnemyAmount[] {  EnemyAmountList.twoGiantBats,
                                                                                                                            EnemyAmountList.twoBatSwarms,
                                                                                                                            EnemyAmountList.twoArmoredBats
                                                                                                                            },
                                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo caveMatronBatBoss = new EnemyPackInfo(new EnemyAmount[] { EnemyAmountList.twoArmoredBats },
                                                                                                    DropTableList.slaveMineDT1Name,
                                                                                                    new ItemListID[] { new ItemListID(  ItemList.questItemListIndex,
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

        // list.Add(twoGiantBatsTwoBatSwarms);
        // list.Add(twoGiantBatsTwoBatSwarms);

        list.Add(twoGiantBatsTwoBatSwarmsTwoArmoredBats);
        list.Add(twoGiantBatsTwoBatSwarmsTwoArmoredBats);

        enemyPackInfoDict.Add(LocationNameList.slaveShackSix, list);
        #endregion

        #region MineLvl_1-1b
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);

        enemyPackInfoDict.Add(LocationNameList.mineLvl1 + LocationNameList.section1b, list);
        #endregion

        #region MineLvl_2-1b
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);

        enemyPackInfoDict.Add(LocationNameList.mineLvl2 + LocationNameList.section1b, list);
        #endregion

        #region MineLvl_2-1c
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);
        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);

        enemyPackInfoDict.Add(LocationNameList.mineLvl2 + LocationNameList.section1c, list);
        #endregion

        #region MineLvl_2-2a
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);

        enemyPackInfoDict.Add(LocationNameList.mineLvl2 + LocationNameList.section2a, list);
        #endregion

        #region MineLvl_2-3a
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);

        enemyPackInfoDict.Add(LocationNameList.mineLvl2 + LocationNameList.section3a, list);
        #endregion

        #region MineLvl_2-3b
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);
        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);
        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);

        enemyPackInfoDict.Add(LocationNameList.mineLvl2 + LocationNameList.section3b, list);
        #endregion

        #region MineLvl_2-5
        list = new List<EnemyPackInfo>();

        list.Add(twoArmoredBats);
        list.Add(twoArmoredBats);

        enemyPackInfoDict.Add(LocationNameList.mineLvl2 + LocationNameList.section5, list);
        #endregion

        #region MineLvl_2-7b
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);
        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);
        list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);

        list.Add(caveMatronBatBoss);

        enemyPackInfoDict.Add(LocationNameList.mineLvl2 + LocationNameList.section7b, list);
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
