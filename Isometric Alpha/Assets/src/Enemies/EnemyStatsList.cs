using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyStatsList
{
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
    public static void initialize()
    {
        if(enemyStatsDict != null)
        {
            return;
        }

        enemyStatsDict = new Dictionary<string, EnemyStats>();
        AlliedSummonStatsList.allyStatsDict = new Dictionary<string, AlliedSummonStats>();

        #region Named NPCs
        #region Lovashi Guards
        enemyStatsDict.Add(NPCNameList.guardVazul, new EnemyStats(NPCNameList.guardVazul,
                                                                                    Constants.fiftyArmor,
                                                                                            50,
new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.bladeBlitzKey) as Ability),
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.territorial
                                                                            }));

        enemyStatsDict.Add(NPCNameList.guardAndras, new EnemyStats(NPCNameList.guardAndras,
                                                                                    Constants.thirtyArmor,
                                                                                            40,
                                    AbilityList.getAbility(null, AbilityList.slashKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.chaotic
                                                                            }));

        enemyStatsDict.Add(NPCNameList.guardReka, new EnemyStats(NPCNameList.guardReka,
                                                                        Constants.fortyFiveArmor,
                                                                                    95,
                        AbilityList.getAbility(null, AbilityList.guardAxeKey) as Ability,
                                                            new Trait[] { TraitList.master,
                                                                        TraitList.chaotic,
                                                                        TraitList.frontLine
                                                                        }));

        enemyStatsDict.Add(NPCNameList.guardVirag, new EnemyStats(NPCNameList.guardVirag,
                                                                            Constants.thirtyArmor,
                                                                                    75,
                    AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                        new Trait[] { TraitList.master,
                                                                    TraitList.chaotic,
                                                                    TraitList.backLine
                                                                    }));

        enemyStatsDict.Add(NPCNameList.overseerGaspar, new EnemyStats(NPCNameList.overseerGaspar,
                                                                                    Constants.fortyArmor,
                                                                                            130,
                            AbilityList.getAbility(null, AbilityList.guardLashKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.predatory
                                                                            }));

        enemyStatsDict.Add(NPCNameList.guardPazman, new EnemyStats(NPCNameList.guardPazman,
                                                                                Constants.thirtyArmor,
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

        enemyStatsDict.Add(NPCNameList.barricade, new LargeEnemyStats(NPCNameList.barricade,
                                                                                Constants.fiftyArmor,
                                                                                        125,
                                                            new Trait[] { 
                                                                        TraitList.minion,
                                                                        TraitList.large,
                                                                        TraitList.blocker
                                                                        },
                                                                        spawnDetails));

        #endregion

        #region Brandless Slaves
        enemyStatsDict.Add(NPCNameList.imre, new EnemyStats(NPCNameList.imre,
                                                                            Constants.zeroArmor,
                                                                                    50,
                    AbilityList.getAbility(null, AbilityList.punchKey) as Ability,
                                                        new Trait[] { TraitList.master,
                                                                      TraitList.chaotic
                                                                            }));
        #endregion
        #endregion

        #region Lovashi Guards

        enemyStatsDict.Add(MonsterNameList.axeman, new EnemyStats(MonsterNameList.axeman,
                                                                                    Constants.fiftyFiveArmor,
                                                                                            55,
                            AbilityList.getAbility(null, AbilityList.guardAxeKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.predatory,
                                                                          TraitList.frontLine
                                                                            }));

        enemyStatsDict.Add(MonsterNameList.disciplinarian, new EnemyStats(MonsterNameList.disciplinarian,
                                                                                    Constants.fortyArmor,
                                                                                            55,
                            AbilityList.getAbility(null, AbilityList.guardLashKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.predatory
                                                                            }));

        enemyStatsDict.Add(MonsterNameList.executioner, new EnemyStats(MonsterNameList.executioner,
                                                                                    Constants.fortyArmor,
                                                                                            70,
                            AbilityList.getAbility(null, AbilityList.executeKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.predatory
                                                                            }));

        enemyStatsDict.Add(MonsterNameList.javelineer, new EnemyStats(MonsterNameList.javelineer,
                                                                                    Constants.thirtyArmor,
                                                                                            25,
                            AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                                new Trait[] { TraitList.minion,
                                                                          TraitList.chaotic,
                                                                          TraitList.backLine
                                                                            }));

        enemyStatsDict.Add(MonsterNameList.lancer, new EnemyStats(MonsterNameList.lancer,
                                                                                    Constants.fortyArmor,
                                                                                            60,
                            AbilityList.getAbility(null, AbilityList.skewerKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                            TraitList.territorial
                                                                            }));

        enemyStatsDict.Add(MonsterNameList.lieutenant, new EnemyStats(MonsterNameList.lieutenant,
                                                                                    Constants.fortyArmor,
                                                                                            60,
                            AbilityList.getAbility(null, AbilityList.squadStrikeKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.territorial
                                                                            }));

        enemyStatsDict.Add(MonsterNameList.lineBreaker, new EnemyStats(MonsterNameList.lineBreaker,
                                                                                    Constants.fortyArmor,
                                                                                            60,
                            AbilityList.getAbility(null, AbilityList.skullBashKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.chaotic
                                                                            }));        
        
        enemyStatsDict.Add(MonsterNameList.signaleer, new EnemyStats(MonsterNameList.signaleer,
                                                                                    Constants.thirtyArmor,
                                                                                            45,
new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.guardArrowBarrageKey) as Ability),
                                                                new Trait[] { 
                                                                                TraitList.master,
                                                                                TraitList.rapidInaccurateBombardment,
                                                                                TraitList.backLine
                                                                            }));

        enemyStatsDict.Add(MonsterNameList.spearman, new EnemyStats(MonsterNameList.spearman,
                                                                                    Constants.thirtyArmor,
                                                                                            40,
                            AbilityList.getAbility(null, AbilityList.guardSpearKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                            TraitList.territorial
                                                                            }));

        #endregion

        #region Bats
        #region Giant Bat
        enemyStatsDict.Add(MonsterNameList.giantBat, new EnemyStats(MonsterNameList.giantBat,
                                                                                      Constants.tenArmor,
                                                                                            25,
                                               AbilityList.getAbility(null, AbilityList.batClawName),
                                                                new Trait[] { TraitList.master,
                                                                             TraitList.chaotic
                                                                            }));
        #endregion
        #region Bat Swarm
        enemyStatsDict.Add(MonsterNameList.batSwarm, new EnemyStats(MonsterNameList.batSwarm,
                                                                                      Constants.zeroArmor,
                                                                                            5,
                                               AbilityList.getAbility(null, AbilityList.swarmRushKey),
                                                                new Trait[] { TraitList.minion,
                                                                             TraitList.chaotic
                                                                            }));
        #endregion

        #region Screecher
        enemyStatsDict.Add(MonsterNameList.screecher, new EnemyStats(MonsterNameList.screecher,
                                                                                      Constants.twentyArmor,
                                                                                            45,
  new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.screechKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.chaotic
                                                                                }));
        #endregion
        #region Armored Bat
        enemyStatsDict.Add(MonsterNameList.armoredBat, new EnemyStats(MonsterNameList.armoredBat,
                                                                                      Constants.thirtyArmor,
                                                                                            45,
  new ChargeUpAbility(TraitList.shielded, AbilityList.getAbility(null, AbilityList.flurryKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.territorial
                                                                                }));
                                                                                
        enemyStatsDict.Add(MonsterNameList.armoredBatShielded, new EnemyStats(MonsterNameList.armoredBat,
                                                                                      Constants.thirtyArmor,
                                                                                            45,
  new ChargeUpAbility(TraitList.shielded, AbilityList.getAbility(null, AbilityList.flurryKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.territorial,
                                                                                  TraitList.shielded
                                                                                }));
        #endregion
        #region Den Mother
        enemyStatsDict.Add(MonsterNameList.denMother, new EnemyStats(MonsterNameList.denMother,
                                                                                      Constants.twentyArmor,
                                                                                            35,
                                            AbilityList.getAbility(null, AbilityList.spawnPupsKey),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.empty
                                                                                }));
        #endregion
        #region Cave Matron
        enemyStatsDict.Add(MonsterNameList.caveMatron, new EnemyStats(MonsterNameList.caveMatron,
                                                                                      Constants.twentyArmor,
                                                                                            155,
    new LastManStandingAbility(TraitList.extraShielded, AbilityList.getAbility(null, AbilityList.rouseColonyKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.emptyGenerated2
                                                                                }));
        #endregion
        #endregion

        AlliedSummonStatsList.addEnemyBasedSummons();
    }

}
