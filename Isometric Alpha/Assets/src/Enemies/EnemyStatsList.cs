using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyStatsList
{
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
                                                                                    Constants.twentyFiveArmor,
                                                                                            50,
new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.skewerKey) as Ability),
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.territorial,
                                                                          TraitList.frontLine
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(NPCNameList.guardAndras, new EnemyStats(NPCNameList.guardAndras,
                                                                                    Constants.fifteenArmor,
                                                                                            40,
                                    AbilityList.getAbility(null, AbilityList.slashKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.chaotic
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(NPCNameList.guardReka, new EnemyStats(NPCNameList.guardReka,
                                                                        Constants.twentyFiveArmor,
                                                                                    95,
                        AbilityList.getAbility(null, AbilityList.guardAxeKey) as Ability,
                                                            new Trait[] { TraitList.master,
                                                                        TraitList.chaotic,
                                                                        TraitList.frontLine
                                                                        },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.femaleHumanAudioDictionary));

        enemyStatsDict.Add(NPCNameList.guardVirag, new EnemyStats(NPCNameList.guardVirag,
                                                                            Constants.fifteenArmor,
                                                                                    75,
                    AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                        new Trait[] { TraitList.master,
                                                                    TraitList.chaotic,
                                                                    TraitList.backLine
                                                                    },
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.femaleHumanAudioDictionary));

        enemyStatsDict.Add(NPCNameList.overseerGaspar, new EnemyStats(NPCNameList.overseerGaspar,
                                                                                    Constants.twentyArmor,
                                                                                            130,
                            AbilityList.getAbility(null, AbilityList.guardLashKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.predatory
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.whipAudioDictionary));

        enemyStatsDict.Add(NPCNameList.guardPazman, new EnemyStats(NPCNameList.guardPazman,
                                                                                Constants.fifteenArmor,
                                                                                        95,
                        AbilityList.getAbility(null, AbilityList.guardSpearKey) as Ability,
                                                            new Trait[] { TraitList.master,
                                                                        TraitList.territorial
                                                                        },
                                                                        animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));


        enemyStatsDict.Add(NPCNameList.director, new EnemyStats(NPCNameList.director,
                                                                                    Constants.fortyArmor,
                                                                                            25,
                            AbilityList.getAbility(null, AbilityList.guardSpearKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                            TraitList.territorial,
                                                                            TraitList.backLine
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(NPCNameList.kende, new EnemyStats(NPCNameList.kende,
                                                                    Constants.thirtyArmor,
                                                                                    160,
                            AbilityList.getAbility(null, AbilityList.guardWarriorSummonKey) as Ability,
                                                                new Trait[] { 
                                                                                TraitList.master,
                                                                                TraitList.emptyGenerated2,
                                                                                TraitList.backLine
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(NPCNameList.guardMuzsa, new EnemyStats(NPCNameList.guardMuzsa,
                                                                Constants.twentyFiveArmor,
                                                                                        65,
new BuffChargeUpAbility(TraitList.coordinated, AbilityList.getAbility(null, AbilityList.bladeBlitzKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                            TraitList.territorial,
                                                                                TraitList.frontLine
                                                                                },
                                                                                animationAudioClipDictionary: AnimationSFXDictionaryList.femaleHumanAudioDictionary));

        enemyStatsDict.Add(NPCNameList.chiefTabor, new EnemyStats(NPCNameList.chiefTabor,
                                                                    Constants.thirtyArmor,
                                                                                            220,
                                            AbilityList.getAbility(null, AbilityList.takeHostageKey),
                                                                new Trait[] { 
                                                                                TraitList.master,
                                                                                TraitList.nonMasterChaotic,
                                                                                TraitList.indomitable, 
                                                                                TraitList.backLine
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.whipAudioDictionary));

        enemyStatsDict.Add(NPCNameList.captainAdela, new LargeEnemyStats(NPCNameList.captainAdela,
                                                                    Constants.sixtyArmor,
                                                                                            275,
                                                                new Trait[] { 
                                                                                TraitList.master,
                                                                                TraitList.clockwiseFourCornersEnemySide,
                                                                                TraitList.indomitable
                                                                            },
                                                                            AbilityList.getAbility(null, AbilityList.shoreUpKey),
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.femaleHumanAudioDictionary,
                                                                            useAverageSpritePosition: true));

        enemyStatsDict.Add(NPCNameList.barricade, new MultiAnimationEnemyStats(NPCNameList.barricade,
                                                                                Constants.twentyFiveArmor,
                                                                                        125,
                                                            new Trait[] { 
                                                                            TraitList.minion,
                                                                            TraitList.large,
                                                                            TraitList.blocker,
                                                                            TraitList.frontLine
                                                                        }));

        #endregion

        #region Brandless Slaves
        enemyStatsDict.Add(NPCNameList.imre, new EnemyStats(NPCNameList.imre,
                                                                            Constants.fiveArmor,
                                                                                    50,
                    AbilityList.getAbility(null, AbilityList.punchKey) as Ability,
                                                        new Trait[] { TraitList.master,
                                                                      TraitList.chaotic
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));
        #endregion

        #region Branded Slaves
        enemyStatsDict.Add(NPCNameList.clay, new EnemyStats(NPCNameList.clay,
                                                                            Constants.tenArmor,
                                                                                    250,
                    AbilityList.getAbility(null, AbilityList.gutKey) as Ability,
                                                        new Trait[] { 
                                                                      TraitList.master,
                                                                      TraitList.territorial
                                                                    },
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(NPCNameList.dezso, new EnemyStats(NPCNameList.dezso+1,
                                                                            Constants.twentyFiveArmor,
                                                                                    95,
new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.guardAxeKey) as Ability),
                                                        new Trait[] { 
                                                                      TraitList.master,
                                                                      TraitList.territorial,
                                                                      TraitList.backLine
                                                                    },
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));
        #endregion
        #endregion

        #region Vada

        enemyStatsDict.Add(NPCNameList.takacs, new EnemyStats(NPCNameList.takacs,
                                                                            Constants.zeroArmor,
                                                                                                5,
                                            AbilityList.getAbility(null, AbilityList.guardAxeKey),
                                                        new Trait[] { 
                                                                      TraitList.master,
                                                                      TraitList.chaotic,
                                                                      TraitList.frontLine
                                                                    },
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.femaleHumanAudioDictionary));

        #endregion

        #region Lovashi Puppets

        enemyStatsDict.Add(MonsterNameList.puppetedPrefix + MonsterNameList.axeman, new MinionStats(MonsterNameList.puppetedPrefix + MonsterNameList.axeman,
                                                                            Constants.fifteenArmor,
                                                                                                50,
                                            AbilityList.getAbility(null, AbilityList.guardAxeKey),
                                                        new Trait[] { 
                                                                      TraitList.minion,
                                                                      TraitList.chaotic,
                                                                      TraitList.frontLine
                                                                    },
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.puppetedPrefix + MonsterNameList.spearman, new MinionStats(MonsterNameList.puppetedPrefix + MonsterNameList.spearman,
                                                                            Constants.tenArmor,
                                                                                                35,
                                            AbilityList.getAbility(null, AbilityList.guardSpearKey),
                                                        new Trait[] { 
                                                                      TraitList.minion,
                                                                      TraitList.chaotic,
                                                                      TraitList.frontLine
                                                                    },
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.puppetedPrefix + MonsterNameList.disciplinarian, new MinionStats(MonsterNameList.puppetedPrefix + MonsterNameList.disciplinarian,
                                                                            Constants.fifteenArmor,
                                                                                                45,
                                            AbilityList.getAbility(null, AbilityList.guardLashKey),
                                                        new Trait[] { 
                                                                      TraitList.minion,
                                                                      TraitList.chaotic,
                                                                      TraitList.frontLine
                                                                    },
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.puppetedPrefix + MonsterNameList.javelineer, new MinionStats(MonsterNameList.puppetedPrefix + MonsterNameList.javelineer,
                                                                            Constants.fiveArmor,
                                                                                                20,
                                            AbilityList.getAbility(null, AbilityList.guardJavelinKey),
                                                        new Trait[] { 
                                                                      TraitList.minion,
                                                                      TraitList.chaotic,
                                                                      TraitList.frontLine
                                                                    },
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        #endregion

        #region Lovashi Guards

        enemyStatsDict.Add(MonsterNameList.axeman, new EnemyStats(MonsterNameList.axeman,
                                                                                    Constants.twentyFiveArmor,
                                                                                            55,
                            AbilityList.getAbility(null, AbilityList.guardAxeKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.predatory,
                                                                          TraitList.frontLine
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.disciplinarian, new EnemyStats(MonsterNameList.disciplinarian,
                                                                                    Constants.twentyArmor,
                                                                                            55,
                            AbilityList.getAbility(null, AbilityList.guardLashKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.predatory
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.whipAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.executioner, new EnemyStats(MonsterNameList.executioner,
                                                                                    Constants.twentyArmor,
                                                                                            70,
                            AbilityList.getAbility(null, AbilityList.executeKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.predatory
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.javelineer, new MinionStats(MonsterNameList.javelineer,
                                                                                    Constants.fiveArmor,
                                                                                            25,
                            AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                                new Trait[] { 
                                                                                TraitList.chaotic,
                                                                                TraitList.backLine
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.lancer, new EnemyStats(MonsterNameList.lancer,
                                                                                    Constants.twentyArmor,
                                                                                            60,
                            AbilityList.getAbility(null, AbilityList.skewerKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                            TraitList.territorial
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.lieutenant, new EnemyStats(MonsterNameList.lieutenant,
                                                                                    Constants.twentyArmor,
                                                                                            60,
                            AbilityList.getAbility(null, AbilityList.squadStrikeKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.territorial
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.linebreaker, new EnemyStats(MonsterNameList.linebreaker,
                                                                                    Constants.twentyArmor,
                                                                                            60,
                            AbilityList.getAbility(null, AbilityList.skullBashKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.chaotic
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));        
    
        enemyStatsDict.Add(MonsterNameList.overseer, new EnemyStats(MonsterNameList.overseer,
                                                                                    Constants.twentyArmor,
                                                                                            120,
                            AbilityList.getAbility(null, AbilityList.guardSlaveSummonKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                          TraitList.empty,
                                                                          TraitList.backLine
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.whipAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.signaleer, new EnemyStats(MonsterNameList.signaleer,
                                                                                    Constants.fifteenArmor,
                                                                                            45,
new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.guardArrowBarrageKey) as Ability),
                                                                new Trait[] { 
                                                                                TraitList.master,
                                                                                TraitList.rapidInaccurateBombardment,
                                                                                TraitList.backLine
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.spearman, new EnemyStats(MonsterNameList.spearman,
                                                                                    Constants.fifteenArmor,
                                                                                            40,
                            AbilityList.getAbility(null, AbilityList.guardSpearKey) as Ability,
                                                                new Trait[] { TraitList.master,
                                                                            TraitList.territorial
                                                                            },
                                                                            animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        #endregion

        #region NonBranded Slaves

        enemyStatsDict.Add(MonsterNameList.noBrandLoyalist, new MinionStats(MonsterNameList.noBrandLoyalist,
                                                                            Constants.fiveArmor,
                                                                                    20,
                        AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                            new Trait[] { 
                                                                            TraitList.minion,
                                                                            TraitList.chaotic,
                                                                            TraitList.frontLine
                                                                        }, 
                                                                        gendered: true,
                                                                        animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.noBrandRioter, new MinionStats(MonsterNameList.noBrandRioter,
                                                                            Constants.fiveArmor,
                                                                                    20,
                    AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                        new Trait[] { 
                                                                        TraitList.minion,
                                                                        TraitList.chaotic
                                                                    },
                                                                    gendered: true,
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));
                                                                
        enemyStatsDict.Add(NPCNameList.beam, new EnemyStats(NPCNameList.beam,
                                                                            Constants.tenArmor,
                                                                                    70,
                    AbilityList.getAbility(null, AbilityList.feedKey) as Ability,
                                                        new Trait[] { 
                                                                        TraitList.minion,
                                                                        TraitList.healer
                                                                    },
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));
        #endregion

        #region Branded Slaves

        enemyStatsDict.Add(MonsterNameList.angryBranded, new MinionStats(MonsterNameList.angryBranded,
                                                                            Constants.fiveArmor,
                                                                                    15,
                    AbilityList.getAbility(null, AbilityList.brandedVolleyKey) as Ability,
                                                        new Trait[] { 
                                                                        TraitList.minion,
                                                                        TraitList.chaotic,
                                                                        TraitList.blocker,
                                                                        TraitList.frontLine
                                                                    },
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.brandedConscript, new MinionStats(MonsterNameList.brandedConscript,
                                                                            Constants.fiveArmor,
                                                                                    15,
                    AbilityList.getAbility(null, AbilityList.guardJavelinKey) as Ability,
                                                        new Trait[] { 
                                                                        TraitList.minion,
                                                                        TraitList.chaotic,
                                                                        TraitList.blocker,
                                                                        TraitList.frontLine
                                                                    },
                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.brandedRioter, new MinionStats(MonsterNameList.brandedRioter,
                                                                                            Constants.fiveArmor,
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
                                                                                    },
                                                                                    animationAudioClipDictionary: AnimationSFXDictionaryList.maleHumanAudioDictionary));
        #endregion

        #region Bats
        #region Giant Bat
        enemyStatsDict.Add(MonsterNameList.giantBat, new EnemyStats(MonsterNameList.giantBat,
                                                                                      Constants.fiveArmor,
                                                                                            25,
                                               AbilityList.getAbility(null, AbilityList.batClawName),
                                                                new Trait[] { TraitList.master,
                                                                             TraitList.chaotic
                                                                            },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.largeBatAudioDictionary));
        #endregion
        #region Bat Swarm
        enemyStatsDict.Add(MonsterNameList.batSwarm, new MinionStats(MonsterNameList.batSwarm,
                                                                                      Constants.zeroArmor,
                                                                                            5,
                                               AbilityList.getAbility(null, AbilityList.swarmRushKey),
                                                                traits: new Trait[] { 
                                                                                        TraitList.chaotic
                                                                                    },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.batSwarmAudioDictionary));
        #endregion

        #region Screecher
        enemyStatsDict.Add(MonsterNameList.screecher, new EnemyStats(MonsterNameList.screecher,
                                                                                      Constants.tenArmor,
                                                                                            38,
  new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.screechKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.chaotic
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.bipedalBatAudioDictionary));
        #endregion
        #region Armored Bat
        enemyStatsDict.Add(MonsterNameList.armoredBat, new EnemyStats(MonsterNameList.armoredBat,
                                                                                      Constants.fifteenArmor,
                                                                                            45,
  new ChargeUpAbility(TraitList.shielded, AbilityList.getAbility(null, AbilityList.flurryKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.territorial
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.bipedalBatAudioDictionary));
                                                                                
        enemyStatsDict.Add(MonsterNameList.armoredBatShielded, new EnemyStats(MonsterNameList.armoredBat,
                                                                                      Constants.fifteenArmor,
                                                                                            45,
  new ChargeUpAbility(TraitList.shielded, AbilityList.getAbility(null, AbilityList.flurryKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.territorial,
                                                                                  TraitList.shielded
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.bipedalBatAudioDictionary));
        #endregion
        #region Den Mother
        enemyStatsDict.Add(MonsterNameList.denMother, new EnemyStats(MonsterNameList.denMother,
                                                                                      Constants.tenArmor,
                                                                                            32,
                                            AbilityList.getAbility(null, AbilityList.spawnPupsKey),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.empty
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.largeBatAudioDictionary));
        #endregion
        #region Cave Matron
        enemyStatsDict.Add(MonsterNameList.caveMatron, new EnemyStats(MonsterNameList.caveMatron,
                                                                                      Constants.zeroArmor,
                                                                                            70,
    new LastManStandingAbility(TraitList.extraShielded, AbilityList.getAbility(null, AbilityList.rouseColonyKey) as Ability),
                                                                    new Trait[] { TraitList.master,
                                                                                  TraitList.emptyGenerated2
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.bipedalBatAudioDictionary));
        #endregion
        #endregion

        #region Worms

        #region Worm Nests
        enemyStatsDict.Add(MonsterNameList.hiveHeraldNest, new LargeEnemyStats(MonsterNameList.hiveHeraldNest,
                                                                              Constants.tenArmor,
                                                                                                190,
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.chaotic,
                                                                                    TraitList.wormBossExplodes
                                                                                },
                                               AbilityList.getAbility(null, AbilityList.trampleKey),
                    animationAudioClipDictionary : AnimationSFXDictionaryList.biteWormAudioDictionary));
        enemyStatsDict.Add(MonsterNameList.martyrWormNest, new LargeEnemyStats(MonsterNameList.martyrWormNest,
                                                                              Constants.tenArmor,
                                                                                                145,
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.territorial,
                                                                                    TraitList.wormBossRevive
                                                                                },
                                               AbilityList.getAbility(null, AbilityList.wallopKey),
                    animationAudioClipDictionary : AnimationSFXDictionaryList.biteWormAudioDictionary));
        enemyStatsDict.Add(MonsterNameList.toxicWormNest, new LargeEnemyStats(MonsterNameList.toxicWormNest,
                                                                              Constants.tenArmor,
                                                                                                225,
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.rapidInaccurateBombardment
                                                                                },
new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.bossWormFumesKey) as Ability),
animationAudioClipDictionary : AnimationSFXDictionaryList.vomitWormAudioDictionary));
        enemyStatsDict.Add(MonsterNameList.wormNest, new LargeEnemyStats(MonsterNameList.wormNest,
                                                                              Constants.tenArmor,
                                                                                                190,
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.predatory,
                                                                                    TraitList.wormBossSplits
                                                                                },
                                               AbilityList.getAbility(null, AbilityList.slamKey),
                animationAudioClipDictionary : AnimationSFXDictionaryList.biteWormAudioDictionary));
        #endregion

        #region Armored Worm
        enemyStatsDict.Add(MonsterNameList.armoredWorm, new EnemyStats(MonsterNameList.armoredWorm,
                                                                              Constants.fiftyArmor,
                                                                                                65,
                                               AbilityList.getAbility(null, AbilityList.wallopKey),
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.territorial,
                                                                                    TraitList.frontLine
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.biteWormAudioDictionary));
        #endregion
        #region Broodling
        enemyStatsDict.Add(MonsterNameList.broodling, new MinionStats(MonsterNameList.broodling,
                                                                              Constants.zeroArmor,
                                                                                                5,
                                               AbilityList.getAbility(null, AbilityList.acidVomitKey),
                                                                    new Trait[] { 
                                                                                  TraitList.chaotic
                                                                            },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.vomitWormAudioDictionary));
        #endregion
        #region Dire Worm
        enemyStatsDict.Add(MonsterNameList.direWorm, new LargeEnemyStats(MonsterNameList.direWorm,
                                                                                Constants.twentyArmor,
                                                                                        275,
                                                            new Trait[] { 
                                                                            TraitList.master,
                                                                            TraitList.large,
                                                                            TraitList.territorial
                                                                        },
                    AbilityList.getAbility(null, AbilityList.trampleKey) as Ability));
        #endregion
        #region Dire Guardian Worm
        enemyStatsDict.Add(MonsterNameList.direGuardianWorm, new LargeEnemyStats(MonsterNameList.direGuardianWorm,
                                                                                Constants.ninetyArmor,
                                                                                        250,
                                                            new Trait[] { 
                                                                            TraitList.master,
                                                                            TraitList.large,
                                                                            TraitList.chaotic,
                                                                            TraitList.bossLinked
                                                                        },
                    AbilityList.getAbility(null, AbilityList.trampleKey) as Ability));
        #endregion
        #region Guardian Worm
        enemyStatsDict.Add(MonsterNameList.guardianWorm, new EnemyStats(MonsterNameList.guardianWorm,
                                                                              Constants.seventyFiveArmor,
                                                                                                120,
                                               AbilityList.getAbility(null, AbilityList.wallopKey),
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.predatory,
                                                                                    TraitList.frontLine,
                                                                                    TraitList.mobLinked
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.biteWormAudioDictionary));
        #endregion
        #region Hive Herald
        enemyStatsDict.Add(MonsterNameList.hiveHerald, new EnemyStats(MonsterNameList.hiveHerald,
                                                                              Constants.fifteenArmor,
                                                                                                45,
                                               AbilityList.getAbility(null, AbilityList.spawnBroodlingKey),
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.emptyGenerated2,
                                                                                    TraitList.frontLine,
                                                                                    TraitList.wormExplodes
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.summonWormAudioDictionary));
        #endregion
        #region Martyr Worm
        enemyStatsDict.Add(MonsterNameList.martyrWorm, new EnemyStats(MonsterNameList.martyrWorm,
                                                                              Constants.fifteenArmor,
                                                                                                25,
                                               AbilityList.getAbility(null, AbilityList.wallopKey),
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.chaotic,
                                                                                    TraitList.frontLine,
                                                                                    TraitList.wormRevive
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.biteWormAudioDictionary));
        #endregion
        #region Toxic Worm
        enemyStatsDict.Add(MonsterNameList.toxicWorm, new EnemyStats(MonsterNameList.toxicWorm,
                                                                              Constants.fifteenArmor,
                                                                                                75,
new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.wormAcidBarrageKey) as Ability),
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.chaotic,
                                                                                    TraitList.frontLine
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.vomitWormAudioDictionary));
        #endregion
        #region Worm
        enemyStatsDict.Add(MonsterNameList.worm, new EnemyStats(MonsterNameList.worm,
                                                                              Constants.tenArmor,
                                                                                                45,
                                               AbilityList.getAbility(null, AbilityList.wallopKey),
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.territorial,
                                                                                    TraitList.frontLine,
                                                                                    TraitList.wormSplits
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.biteWormAudioDictionary));
        #endregion
        #endregion

        #region Horses
        enemyStatsDict.Add(MonsterNameList.horseCharger, new LargeEnemyStats(MonsterNameList.horseCharger,
                                                                                Constants.fifteenArmor,
                                                                                        170,
                                                            new Trait[] { 
                                                                            TraitList.master,
                                                                            TraitList.large,
                                                                            TraitList.territorial
                                                                        },
                    AbilityList.getAbility(null, AbilityList.chargeKey) as Ability, 
                animationAudioClipDictionary: AnimationSFXDictionaryList.horseAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.horseStomper, new LargeEnemyStats(MonsterNameList.horseStomper,
                                                                                Constants.twentyArmor,
                                                                                        170,
                                                            new Trait[] { 
                                                                            TraitList.master,
                                                                            TraitList.large,
                                                                            TraitList.chaotic
                                                                        },
new ChargeUpAbility(TraitList.charged, AbilityList.getAbility(null, AbilityList.stompKey) as Ability), 
                animationAudioClipDictionary: AnimationSFXDictionaryList.horseAudioDictionary));
                                                            
        enemyStatsDict.Add(NPCNameList.csalan, new LargeEnemyStats(NPCNameList.csalan,
                                                                                Constants.twentyFiveArmor,
                                                                                        210,
                                                            new Trait[] { 
                                                                            TraitList.master,
                                                                            TraitList.large,
                                                                            TraitList.predatory
                                                                        },
                    AbilityList.getAbility(null, AbilityList.chargeKey) as Ability, 
                animationAudioClipDictionary: AnimationSFXDictionaryList.horseAudioDictionary));
        #endregion

        #region Saints

        enemyStatsDict.Add(MonsterNameList.stoneSaint, new EnemyStats(MonsterNameList.stoneSaint,
                                                                              Constants.sixtyFiveArmor,
                                                                                                185,
                                               AbilityList.getAbility(null, AbilityList.evolveKey),
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.saintly,
                                                                                    TraitList.backLine
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.stoneSaintAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.lesserStoneSaint, new EnemyStats(MonsterNameList.stoneSaint,
                                                                              Constants.sixtyFiveArmor,
                                                                                                100,
                                               AbilityList.getAbility(null, AbilityList.lesserBoulderRollKey),
                                                                    new Trait[] { 
                                                                                    TraitList.master,
                                                                                    TraitList.territorial,
                                                                                    TraitList.cannotSummon
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.stoneSaintAudioDictionary));


        enemyStatsDict.Add(MonsterNameList.largeRock, new EvolutionaryEnemyStats(MonsterNameList.largeRock,
                                                                                    Constants.zeroArmor,
                                                                                                    1,
                                                                                    MonsterNameList.lesserStoneSaint,
                                                                    new Trait[] { 
                                                                                    TraitList.minion,
                                                                                    TraitList.immobile
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.stoneSaintAudioDictionary));

        enemyStatsDict.Add(MonsterNameList.smallRock, new EvolutionaryEnemyStats(MonsterNameList.smallRock,
                                                                              Constants.zeroArmor,
                                                                                                1,
                                                                                MonsterNameList.largeRock,
                                                                    new Trait[] { 
                                                                                    TraitList.minion,
                                                                                    TraitList.immobile
                                                                                },
                                                animationAudioClipDictionary : AnimationSFXDictionaryList.stoneSaintAudioDictionary));

        #endregion

        AlliedSummonStatsList.addEnemyBasedSummons();
    }

}
