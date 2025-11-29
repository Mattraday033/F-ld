using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//One day may contain all EnemyStats, right now just loads EnemyStats from Resources
public static class EnemyStatsList
{
    private const int zeroArmor = 0;
    private const int fiveArmor = 5;
    private const int tenArmor = 10;
    private const int fifteenArmor = 15;
    private const int twentyArmor = 20;
    private const int twentyFiveArmor = 25;
    private const int thirtyArmor = 30;
    private const int thirtyFiveArmor = 35;
    private const int fortyArmor = 40;
    private const int fortyFiveArmor = 45;
    private const int fiftyArmor = 50;
    private const int fiftyFiveArmor = 55;
    private const int sixtyArmor = 60;
    private const int sixtyFiveArmor = 65;
    private const int seventyArmor = 70;
    private const int seventyFiveArmor = 75;
    private const int eightyArmor = 80;
    private const int eightyFiveArmor = 85;
    private const int ninetyArmor = 90;
    private const int ninetyFiveArmor = 95;
    private const int oneHundredArmor = 100;
    private const int oneHundredFiveArmor = 105;
    private const int oneHundredTenArmor = 110;
    private const int oneHundredFifteenArmor = 115;
    private const int oneHundredTwentyArmor = 120;
    private const int oneHundredTwentyFiveArmor = 125;
    private const int oneHundredThirtyArmor = 130;
    private const int oneHundredThirtyFiveArmor = 135;
    private const int oneHundredFortyArmor = 140;


    private const string armoredBat = "BatArmoredSecondRound";
    private const string chargedBat = "BatCharged";
    private const string spawnerBat = "BatSpawner";
    private const string giantBat = "BatMaster";
    private const string explosiveBat = "BatMinionExplosion";

    private const string wormMinionAcid = "WormMinionAcid";

    private const string slaveBlocker = "Slave Conscript";
    private const string slaveWarrior = "Slave Warrior";

    private const string smallStoneMaterials = "StoneSaintBuildingMaterialsSmall";

    // public readonly static EnemyStats[][] pupSpawnCombos =  {new EnemyStats[] {Resources.Load<EnemyStats>(explosiveBat), Resources.Load<EnemyStats>(chargedBat)},
    //                                                         new EnemyStats[] {Resources.Load<EnemyStats>(armoredBat), Resources.Load<EnemyStats>(giantBat)},
    //                                                         new EnemyStats[] {Resources.Load<EnemyStats>(spawnerBat), Resources.Load<EnemyStats>(spawnerBat)},
    //                                                         new EnemyStats[] {Resources.Load<EnemyStats>(explosiveBat), Resources.Load<EnemyStats>(explosiveBat)},
    //                                                         new EnemyStats[] {Resources.Load<EnemyStats>(chargedBat), Resources.Load<EnemyStats>(giantBat)}};

    public readonly static EnemyStats[] wormSplitSpawnCombo = new EnemyStats[] { Resources.Load<EnemyStats>(wormMinionAcid), Resources.Load<EnemyStats>(wormMinionAcid) };

    public readonly static EnemyStats[] wormSplitBossSpawnCombo = new EnemyStats[] {Resources.Load<EnemyStats>(wormMinionAcid), Resources.Load<EnemyStats>(wormMinionAcid),
                                                                                    Resources.Load<EnemyStats>(wormMinionAcid), Resources.Load<EnemyStats>(wormMinionAcid)};

    public readonly static EnemyStats[] slaveBlockerCombo = new EnemyStats[] { Resources.Load<EnemyStats>(slaveBlocker), Resources.Load<EnemyStats>(slaveBlocker) };
    public readonly static EnemyStats[] slaveWarriorCombo = new EnemyStats[] {Resources.Load<EnemyStats>(slaveWarrior),
                                                                            Resources.Load<EnemyStats>(slaveWarrior),
                                                                            Resources.Load<EnemyStats>(slaveWarrior)};

    public readonly static EnemyStats[] smallStonesCombo = new EnemyStats[] {Resources.Load<EnemyStats>(smallStoneMaterials),
                                                                            Resources.Load<EnemyStats>(smallStoneMaterials),
                                                                            Resources.Load<EnemyStats>(smallStoneMaterials)};


    private static Dictionary<string, EnemyStats> enemyStatsDict;

    public static EnemyStats getEnemyStats(string key)
    {
        if(enemyStatsDict == null)
        {
            initialize();
        }

        if (!enemyStatsDict.ContainsKey(key))
        {
            Debug.LogError("No Enemy at key: (" + key + ")");
            return null;
        }

        return enemyStatsDict[key];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initialize()
    {
        enemyStatsDict = new Dictionary<string, EnemyStats>();

        #region NPCs
        #region Lovashi Guards
        enemyStatsDict.Add(NPCNameList.guardVazul, new EnemyStats(NPCNameList.guardVazul,
                                                                                    fiftyArmor,
                                                                                            50,
new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.bladeBlitzKey) as Ability),
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.territorial
                                                                            }));

        enemyStatsDict.Add(NPCNameList.guardAndras, new EnemyStats(NPCNameList.guardAndras,
                                                                                    thirtyArmor,
                                                                                            40,
                                    AbilityList.getAbility(null, AbilityList.slashKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.chaotic
                                                                            }));

        enemyStatsDict.Add(NPCNameList.guardReka, new EnemyStats(NPCNameList.guardReka,
                                                                        fortyFiveArmor,
                                                                                    95,
                        AbilityList.getAbility(null, AbilityList.guardAxeKey) as Ability,
                                                            new Trait[] { TraitList.master,
                                                                        TraitList.chaotic,
                                                                        TraitList.frontLine
                                                                        }));

        enemyStatsDict.Add(NPCNameList.guardVirag, new EnemyStats(NPCNameList.guardVirag,
                                                                            thirtyArmor,
                                                                                    75,
                    AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                        new Trait[] { TraitList.master,
                                                                    TraitList.chaotic,
                                                                    TraitList.backLine
                                                                    }));

        enemyStatsDict.Add(NPCNameList.overseerGaspar, new EnemyStats(NPCNameList.overseerGaspar,
                                                                                    fortyArmor,
                                                                                            130,
                            AbilityList.getAbility(null, AbilityList.guardLashKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.predatory
                                                                            }));

        enemyStatsDict.Add(NPCNameList.guardPazman, new EnemyStats(NPCNameList.guardPazman,
                                                                                thirtyArmor,
                                                                                        95,
                        AbilityList.getAbility(null, AbilityList.guardSpearKey) as Ability,
                                                            new Trait[] { TraitList.master,
                                                                        TraitList.territorial
                                                                        }));

        SpawnDetails spawnDetails = new SpawnDetails(new GridCoords[] {
                                                                        new GridCoords(Constants.indexThree, Constants.indexZero),
                                                                        new GridCoords(Constants.indexThree, Constants.indexOne),
                                                                        new GridCoords(Constants.indexThree, Constants.indexTwo),
                                                                        new GridCoords(Constants.indexThree, Constants.indexThree)
                                                                      }, 
                                                                       new GridCoords(Constants.indexThree, Constants.indexOne),
                                                                       new GridCoords(Constants.indexThree, Constants.indexOne), 
                                                                       true);

        enemyStatsDict.Add(NPCNameList.barricade, new EnemyStats(NPCNameList.barricade,
                                                                                thirtyArmor,
                                                                                        125,
                                                            new Trait[] { TraitList.minion,
                                                                        TraitList.large,
                                                                        TraitList.blocker
                                                                        },
                                                                        spawnDetails));

        #endregion

        #region Brandless Slaves
        enemyStatsDict.Add(NPCNameList.imre, new EnemyStats(NPCNameList.imre,
                                                                            zeroArmor,
                                                                                    50,
                    AbilityList.getAbility(null, AbilityList.punchKey) as Ability,
                                                        new Trait[] { TraitList.master,
                                                                      TraitList.chaotic
                                                                            }));
        #endregion
        #endregion


        #region Giant Bat
        enemyStatsDict.Add(MonsterNameList.giantBat, new EnemyStats(MonsterNameList.giantBat,
                                                                                      tenArmor,
                                                                                            25,
                                               AbilityList.getAbility(null, AbilityList.batClawName),
                                                                new Trait[] { TraitList.master,
                                                                             TraitList.chaotic
                                                                            }));
        #endregion
        #region Bat Swarm
        enemyStatsDict.Add(MonsterNameList.batSwarm, new EnemyStats(MonsterNameList.batSwarm,
                                                                                      zeroArmor,
                                                                                            5,
                                               AbilityList.getAbility(null, AbilityList.swarmRushKey),
                                                                new Trait[] { TraitList.minion,
                                                                             TraitList.chaotic
                                                                            }));
        #endregion

        #region Screecher
        enemyStatsDict.Add(MonsterNameList.screecher, new EnemyStats(MonsterNameList.screecher,
                                                                                      twentyArmor,
                                                                                            45,
  new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.screechKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.chaotic
                                                                                }));
        #endregion
        #region Armored Bat
        enemyStatsDict.Add(MonsterNameList.armoredBat, new EnemyStats(MonsterNameList.armoredBat,
                                                                                      thirtyArmor,
                                                                                            45,
  new ChargeUpAbility(TraitList.shielded, AbilityList.getAbility(null, AbilityList.flurryKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.territorial
                                                                                }));
        #endregion
        #region Den Mother
        enemyStatsDict.Add(MonsterNameList.denMother, new EnemyStats(MonsterNameList.denMother,
                                                                                      twentyArmor,
                                                                                            35,
                                            AbilityList.getAbility(null, AbilityList.spawnPupsKey),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.empty
                                                                                }));
        #endregion
        #region Cave Matron
        enemyStatsDict.Add(MonsterNameList.caveMatron, new EnemyStats(MonsterNameList.caveMatron,
                                                                                      twentyArmor,
                                                                                            155,
    new LastManStandingAbility(TraitList.extraShielded, AbilityList.getAbility(null, AbilityList.rouseColonyKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.emptyGenerated2
                                                                                }));
        #endregion
    }

}
