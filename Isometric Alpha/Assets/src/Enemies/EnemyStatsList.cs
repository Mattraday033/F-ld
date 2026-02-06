using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyStatsList
{
    private const string wormMinionAcid = "WormMinionAcid";

    private const string slaveWarrior = "Slave Warrior";

    private const string smallStoneMaterials = "StoneSaintBuildingMaterialsSmall";

    public readonly static EnemyStats[] wormSplitSpawnCombo = new EnemyStats[] { Resources.Load<EnemyStats>(wormMinionAcid), Resources.Load<EnemyStats>(wormMinionAcid) };

    public readonly static EnemyStats[] wormSplitBossSpawnCombo = new EnemyStats[] {Resources.Load<EnemyStats>(wormMinionAcid), Resources.Load<EnemyStats>(wormMinionAcid),
                                                                                    Resources.Load<EnemyStats>(wormMinionAcid), Resources.Load<EnemyStats>(wormMinionAcid)};

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
                                                                          TraitList.territorial,
                                                                          TraitList.frontLine
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

        enemyStatsDict.Add(NPCNameList.kende, new EnemyStats(NPCNameList.kende,
                                                                    Constants.sixtyArmor,
                                                                                    160,
                            AbilityList.getAbility(null, AbilityList.guardWarriorSummonKey) as Ability,
                                                                new Trait[] { 
                                                                                TraitList.master,
                                                                                TraitList.emptyGenerated2,
                                                                                TraitList.backLine
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
                                                                            TraitList.blocker,
                                                                            TraitList.frontLine
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

        enemyStatsDict.Add(MonsterNameList.javelineer, new MinionStats(MonsterNameList.javelineer,
                                                                                    Constants.thirtyArmor,
                                                                                            25,
                            AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                                new Trait[] { 
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

        enemyStatsDict.Add(MonsterNameList.linebreaker, new EnemyStats(MonsterNameList.linebreaker,
                                                                                    Constants.fortyArmor,
                                                                                            60,
                            AbilityList.getAbility(null, AbilityList.skullBashKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.chaotic
                                                                            }));        
    
        enemyStatsDict.Add(MonsterNameList.overseer, new EnemyStats(MonsterNameList.overseer,
                                                                                    Constants.seventyArmor,
                                                                                            145,
                            AbilityList.getAbility(null, AbilityList.guardSlaveSummonKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.empty,
                                                                          TraitList.backLine
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

        #region NonBranded Slaves

        enemyStatsDict.Add(MonsterNameList.noBrandLoyalist, new MinionStats(MonsterNameList.noBrandLoyalist,
                                                                            Constants.tenArmor,
                                                                                    20,
                        AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                            new Trait[] { 
                                                                            TraitList.minion,
                                                                            TraitList.chaotic,
                                                                            TraitList.frontLine
                                                                        }, 
                                                                        gendered: true));

        enemyStatsDict.Add(MonsterNameList.noBrandRioter, new MinionStats(MonsterNameList.noBrandRioter,
                                                                            Constants.tenArmor,
                                                                                    20,
                    AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                        new Trait[] { 
                                                                        TraitList.minion,
                                                                        TraitList.chaotic
                                                                    },
                                                                    gendered: true));
        #endregion

        #region Branded Slaves

        enemyStatsDict.Add(MonsterNameList.brandedConscript, new MinionStats(MonsterNameList.brandedConscript,
                                                                            Constants.tenArmor,
                                                                                    15,
                    AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                        new Trait[] { 
                                                                        TraitList.minion,
                                                                        TraitList.chaotic,
                                                                        TraitList.blocker,
                                                                        TraitList.frontLine
                                                                    }));

        enemyStatsDict.Add(MonsterNameList.brandedRioter, new MinionStats(MonsterNameList.brandedRioter,
                                                                                            Constants.tenArmor,
                                                                                                    15,
                                    AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                                        new Trait[] { 
                                                                                        TraitList.minion,
                                                                                        TraitList.chaotic
                                                                                    },
                                                    animationSuffixes: new string[] {
                                                                                        MonsterNameList.pickMarker,
                                                                                        MonsterNameList.shivMarker,
                                                                                        MonsterNameList.shovelMarker
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
        enemyStatsDict.Add(MonsterNameList.batSwarm, new MinionStats(MonsterNameList.batSwarm,
                                                                                      Constants.zeroArmor,
                                                                                            5,
                                               AbilityList.getAbility(null, AbilityList.swarmRushKey),
                                                                traits: new Trait[] { 
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
