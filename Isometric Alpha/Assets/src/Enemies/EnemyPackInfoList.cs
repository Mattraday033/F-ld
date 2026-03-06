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

    #region Bosses

    #region Camp Boss Fights

    public readonly static BossPackInfo campNorthEastOverseerBoss = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.oneOverseer,
                                                                                                        EnemyAmountList.eightBrandedConscripts,
                                                                                                        EnemyAmountList.twoSpearmen
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.neCampOverseerKilled,
                                                                                                        DialogueNameList.slavesAfterKillingOverseerCampNEKey,
                                                                                                        xpDrop: 100);

    public readonly static BossPackInfo clayFightForTabor = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.foughtCrowdForTabor,
                                                                                                        DialogueNameList.taborAfterClayFightKey,
                                                                                                        xpDrop: 100);

    #endregion

    #region Mine Boss Fights

    private readonly static EnemyPackInfo caveMatronBatBoss = new BossPackInfo(new CreatureAmount[] { EnemyAmountList.caveMatron },
                                                                                                    DropTableList.slaveMineDT1Name,
                                                                                                    guaranteedDrops: new ItemListID[] { new ItemListID(  ItemList.keyItemListIndex,
                                                                                                                                        ItemList.mineArmoryKeyIndex) }, 
                                                                                                                                        xpDrop: 100);

    private readonly static EnemyPackInfo wormBoss = new BossPackInfo(new CreatureAmount[] {  
                                                                                              EnemyAmountList.wormNest,
                                                                                              EnemyAmountList.hiveHeraldNest,
                                                                                              EnemyAmountList.martyrWormNest,
                                                                                              EnemyAmountList.toxicWormNest
                                                                                                },
                                                                                                DropTableList.slaveMineDT1Name, 
                                                                                                xpDrop: 100,
                                                                                                spawnDetailsList: new List<SpawnDetails>()
                                                                                                    {
                                                                                                        SpawnDetails.bottomRight2x2,
                                                                                                        SpawnDetails.topLeft2x2,
                                                                                                        SpawnDetails.topRight2x2,
                                                                                                        SpawnDetails.bottomLeft2x2
                                                                                                    });

    #endregion

    #region Manse Boss Fights

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
                                                                                                        EnemyAmountList.csalan,
                                                                                                        EnemyAmountList.oneHorseStomper,
                                                                                                        EnemyAmountList.twoHorseChargers,
                                                                                                        EnemyAmountList.beam
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        FlagNameList.foughtHorsesInManse, 
                                                                                                        xpDrop: 100,
                                                                                                        spawnDetailsList: new List<SpawnDetails>()
                                                                                                        {
                                                                                                            new SpawnDetails(new GridCoords[]{ new GridCoords(1,2), new GridCoords(0,2) }),
                                                                                                            new SpawnDetails(new GridCoords[]{ new GridCoords(1,0), new GridCoords(0,0) }),
                                                                                                            new SpawnDetails(new GridCoords[]{ new GridCoords(1,1), new GridCoords(0,1) }),
                                                                                                            new SpawnDetails(new GridCoords[]{ new GridCoords(1,3), new GridCoords(0,3) })
                                                                                                        });

    #endregion

    #region Pit Boss Fights

    public readonly static BossPackInfo stoneSaintPitBoss = new BossPackInfo(new CreatureAmount[] { 
                                                                                                        EnemyAmountList.oneExecutioner
                                                                                                       },
                                                                                                        DropTableList.slaveMineDT1Name,
                                                                                                        xpDrop: 100);

    #endregion

    #endregion

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

    #region Generic Lovashi Guard Fights

    #region Barracks Top Floor

    public readonly static EnemyPackInfo oneAxemanOneSpearmanTwoJavalineers = new EnemyPackInfo(new CreatureAmount[] {      
                                                                                                        EnemyAmountList.oneAxeman,
                                                                                                        EnemyAmountList.oneSpearman,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneExecutionerTwoJavalineers = new EnemyPackInfo(new CreatureAmount[] {      
                                                                                                        EnemyAmountList.oneExecutioner,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    #endregion

    #region Barricade Guards
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
    #endregion
    
    #region Camp Packs
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

    #region Manse 1F Packs
    public readonly static EnemyPackInfo oneExecutionerOneLieutenantTwoAxemenOneDisciplinarion = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                        EnemyAmountList.oneExecutioner,
                                                                                                        EnemyAmountList.twoAxemen,
                                                                                                        EnemyAmountList.oneDisciplinarian,
                                                                                                        EnemyAmountList.oneLieutenant
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneExecutionerOneAxemanTwoSpearmanThreeJavelineers = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                        EnemyAmountList.oneLinebreaker,
                                                                                                        EnemyAmountList.oneAxeman,
                                                                                                        EnemyAmountList.twoSpearmen,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneLinebreakerOneAxemanOneSpearmanThreeJavelineers = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                        EnemyAmountList.oneLinebreaker,
                                                                                                        EnemyAmountList.oneAxeman,
                                                                                                        EnemyAmountList.oneSpearman,
                                                                                                        EnemyAmountList.threeJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneLinebreakerOneAxemanOneSpearmanOneDisciplinarion = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                        EnemyAmountList.oneLinebreaker,
                                                                                                        EnemyAmountList.oneAxeman,
                                                                                                        EnemyAmountList.oneSpearman,
                                                                                                        EnemyAmountList.oneDisciplinarian
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneLancerThreeAxemenTwoJavelineers = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                        EnemyAmountList.oneLancer,
                                                                                                        EnemyAmountList.threeAxemen,
                                                                                                        EnemyAmountList.oneSpearman,
                                                                                                        EnemyAmountList.twoJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneLieutenantOneDisciplinarianTenJavelineers = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                        EnemyAmountList.oneLieutenant,
                                                                                                        EnemyAmountList.oneDisciplinarian,
                                                                                                        EnemyAmountList.tenJavelineers
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneLieutenantOneDisciplinarianTenLoyalists = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                        EnemyAmountList.oneLieutenant,
                                                                                                        EnemyAmountList.oneDisciplinarian,
                                                                                                        EnemyAmountList.tenNonBrandedLoyalists
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    #endregion

    #region Manse 2F Packs
    public readonly static EnemyPackInfo oneExecutionerOneLancerOneOverseer = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                        EnemyAmountList.oneExecutioner,
                                                                                                        EnemyAmountList.oneLancer,
                                                                                                        EnemyAmountList.oneOverseer
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneLancerOneExecutionerOneLineBreaker = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                        EnemyAmountList.oneLancer,
                                                                                                        EnemyAmountList.oneExecutioner,
                                                                                                        EnemyAmountList.oneLinebreaker
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    public readonly static EnemyPackInfo oneExecutionerOneLancerOneOverseerOneLieutenantOneLinebreaker = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                        EnemyAmountList.oneExecutioner,
                                                                                                        EnemyAmountList.oneLancer,
                                                                                                        EnemyAmountList.oneOverseer,
                                                                                                        EnemyAmountList.oneLinebreaker,
                                                                                                        EnemyAmountList.oneLieutenant
                                                                                                     },
                                                                                    DropTableList.slaveMineDT1Name);

    #endregion

    #endregion

    #region Bats

    private readonly static EnemyPackInfo twoGiantBatsTwoBatSwarmsFirstTutorial = new BossPackInfo(new CreatureAmount[] {  EnemyAmountList.twoGiantBats,
                                                                                                            EnemyAmountList.twoBatSwarms
                                                                                                            },
                                                                                                          DropTableList.slaveMineDT1Name, 
                                                                                                          script: new PreventTutorialsAfterBatsKilledScript());


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
                                                                                                                        tutorialSequenceKey: TutorialSequenceList.traitTutorialSequenceKey);

    private readonly static EnemyPackInfo oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat = new EnemyPackInfo(new CreatureAmount[] {  EnemyAmountList.oneDenMother,
                                                                                                                                    EnemyAmountList.oneGiantBat,
                                                                                                                                    EnemyAmountList.oneArmoredBatShielded,
                                                                                                                                    EnemyAmountList.threeBatSwarms
                                                                                                                                 },
                                                                                                                                DropTableList.slaveMineDT1Name,
                                                                                                                                tutorialSequenceKey: TutorialSequenceList.traitTutorialSequenceKey);

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
                                                                                                                                    tutorialSequenceKey: TutorialSequenceList.traitTutorialSequenceKey);

    private readonly static EnemyPackInfo twoGiantBatsTwoBatSwarmsTwoArmoredBats = new EnemyPackInfo(new CreatureAmount[] {  EnemyAmountList.twoGiantBats,
                                                                                                                            EnemyAmountList.twoBatSwarms, 
                                                                                                                            EnemyAmountList.oneArmoredBat,
                                                                                                                            EnemyAmountList.oneArmoredBatShielded,
                                                                                                                            },
                                                                                                                            DropTableList.slaveMineDT1Name,
                                                                                                                                tutorialSequenceKey: TutorialSequenceList.traitTutorialSequenceKey);

    private readonly static EnemyPackInfo threeDenMothersThreeBatSwarmsOneArmoredBat = new EnemyPackInfo(new CreatureAmount[] {  EnemyAmountList.threeDenMothers,
                                                                                                                            EnemyAmountList.threeBatSwarms,
                                                                                                                            EnemyAmountList.oneArmoredBat
                                                                                                                            },
                                                                                                                    DropTableList.slaveMineDT1Name);

    #endregion

    #region Worms

    private readonly static EnemyPackInfo oneGuardianWormTwoHiveHeralds = new EnemyPackInfo(new CreatureAmount[] {  
                                                                                                                    EnemyAmountList.oneGuardianWorm,
                                                                                                                    EnemyAmountList.twoHiveHeralds,
                                                                                                                    EnemyAmountList.twoBroodlings
                                                                                                                 },
                                                                                                                 DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo twoGuardianWormsOneArmoredWormTwoHiveHeralds = new EnemyPackInfo(new CreatureAmount[] {  
                                                                                                                    EnemyAmountList.twoGuardianWorms,
                                                                                                                    EnemyAmountList.oneArmoredWorm,
                                                                                                                    EnemyAmountList.twoHiveHeralds
                                                                                                                 },
                                                                                                                 DropTableList.slaveMineDT1Name);


    private readonly static EnemyPackInfo oneArmoredWormFourWorms = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                    EnemyAmountList.oneArmoredWorm,
                                                                                                    EnemyAmountList.threeWorms
                                                                                                    },
                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo twoWormsTwoToxicWorms = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                    EnemyAmountList.twoToxicWorms,
                                                                                                    EnemyAmountList.twoWorms
                                                                                                    },
                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo oneWormTwoHiveHeraldsTwoBroodlings = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                    EnemyAmountList.twoHiveHeralds,
                                                                                                    EnemyAmountList.oneWorm,
                                                                                                    EnemyAmountList.twoBroodlings
                                                                                                    },
                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo oneDireWormTwoHiveHeraldsThreeWorms = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                    EnemyAmountList.oneDireWorm,
                                                                                                    EnemyAmountList.twoHiveHeralds,
                                                                                                    EnemyAmountList.threeWorms
                                                                                                    },
                                                                                                    DropTableList.slaveMineDT1Name,
                                                                                                    spawnDetailsList: new List<SpawnDetails>()
                                                                                                    {
                                                                                                        SpawnDetails.middle2x2
                                                                                                    });

    private readonly static EnemyPackInfo twoMartyWormsOneToxicWormOneHiveHeraldOneArmoredWorm = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                    EnemyAmountList.twoMartyrWorms,
                                                                                                    EnemyAmountList.oneHiveHerald,
                                                                                                    EnemyAmountList.oneToxicWorm,
                                                                                                    EnemyAmountList.oneArmoredWorm
                                                                                                    },
                                                                                                    DropTableList.slaveMineDT1Name);

    private readonly static EnemyPackInfo oneDireWormTwoToxicWormsTwoMartyrWormsOneHiveHerald = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                    EnemyAmountList.oneDireWorm,
                                                                                                    EnemyAmountList.twoToxicWorms,
                                                                                                    EnemyAmountList.twoMartyrWorms,
                                                                                                    EnemyAmountList.oneHiveHerald
                                                                                                    },
                                                                                                    DropTableList.slaveMineDT1Name,
                                                                                                    spawnDetailsList: new List<SpawnDetails>()
                                                                                                    {
                                                                                                        SpawnDetails.middle2x2
                                                                                                    });

    private readonly static EnemyPackInfo twoArmoredWormsFourHiveHeralds = new EnemyPackInfo(new CreatureAmount[] {
                                                                                                    EnemyAmountList.oneHiveHerald,
                                                                                                    EnemyAmountList.twoArmoredWorms,
                                                                                                    EnemyAmountList.threeHiveHeralds
                                                                                                    },
                                                                                                    DropTableList.slaveMineDT1Name);

    #endregion

    // public readonly static EnemyPackInfo taborFight = new EnemyPackInfo(new int[] { 1 }, new int[] { 1 }, new EnemyStats[] { loadEnemyStatsFromResources(chiefTabor) }, flagsToCheckForSlaveAllies, DropTableList.slaveMineDT1Name);


    private static Dictionary<string, List<EnemyPackInfo>> enemyPackInfoDict;

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

        #region Camp

        #region Slave Shack Six
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsTwoBatSwarmsFirstTutorial);
        list.Add(twoGiantBatsTwoBatSwarms);

        enemyPackInfoDict.Add(LocationNameList.slaveShackSix, list);
        #endregion

        #region GuardHouse TopF loor
        list = new List<EnemyPackInfo>();

        list.Add(oneAxemanOneSpearmanTwoJavalineers);
        list.Add(oneAxemanOneSpearmanTwoJavalineers);
        list.Add(oneExecutionerTwoJavalineers);

        enemyPackInfoDict.Add(LocationNameList.guardHouseTopFloor, list);
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

        #endregion

        #region Mine

        #region MineLvl_1-1b
        list = new List<EnemyPackInfo>();

        list.Add(twoGiantBatsThreeBatSwarmsOneArmoredBat);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1b, list);
        #endregion

        #region MineLvl_2

        #region MineLvl_2-1b
        list = new List<EnemyPackInfo>();

        list.Add(oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1b, list);
        #endregion

        #region MineLvl_2-1c
        list = new List<EnemyPackInfo>();

        // list.Add(oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat);
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

        // list.Add(oneArmoredBatOneScreecherOneDenMother);
        // list.Add(twoGiantBatsThreeBatSwarmsOneScreecher);
        // list.Add(oneGiantBatTwoBatSwarmsOneDenMotherOneArmoredBat);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section3b, list);
        #endregion

        #region MineLvl_2-4
        list = new List<EnemyPackInfo>();

        list.Add(oneGuardianWormTwoHiveHeralds);
        list.Add(twoGuardianWormsOneArmoredWormTwoHiveHeralds);
        list.Add(oneGuardianWormTwoHiveHeralds);

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

        #endregion

        #region MineLvl_3

        #region MineLvl_3-1a
        list = new List<EnemyPackInfo>();

        list.Add(oneArmoredWormFourWorms);
        list.Add(oneArmoredWormFourWorms);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1a, list);
        #endregion

        #region MineLvl_3-1b
        list = new List<EnemyPackInfo>();

        list.Add(twoArmoredWormsFourHiveHeralds);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1b, list);
        #endregion

        #region MineLvl_3-2a
        list = new List<EnemyPackInfo>();

        list.Add(oneWormTwoHiveHeraldsTwoBroodlings);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section2a, list);
        #endregion

        #region MineLvl_3-2b
        list = new List<EnemyPackInfo>();

        list.Add(twoMartyWormsOneToxicWormOneHiveHeraldOneArmoredWorm);
        list.Add(twoMartyWormsOneToxicWormOneHiveHeraldOneArmoredWorm);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section2b, list);
        #endregion

        #region MineLvl_3-3a
        list = new List<EnemyPackInfo>();

        list.Add(oneDireWormTwoToxicWormsTwoMartyrWormsOneHiveHerald);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section3a, list);
        #endregion

        #region MineLvl_3-4a
        list = new List<EnemyPackInfo>();

        list.Add(twoWormsTwoToxicWorms);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section4a, list);
        #endregion

        #region MineLvl_3-4b
        list = new List<EnemyPackInfo>();

        list.Add(oneDireWormTwoHiveHeraldsThreeWorms);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section4b, list);
        #endregion

        #region MineLvl_3-5 
        list = new List<EnemyPackInfo>();

        list.Add(twoWormsTwoToxicWorms);
        list.Add(twoWormsTwoToxicWorms);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section5, list);
        #endregion

        #region MineLvl_3-6a
        list = new List<EnemyPackInfo>();

        list.Add(oneArmoredWormFourWorms);
        // list.Add(oneDireWormTwoHiveHeraldsThreeWorms);
        list.Add(twoMartyWormsOneToxicWormOneHiveHeraldOneArmoredWorm);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section6a, list);
        #endregion

        #region MineLvl_3-7
        list = new List<EnemyPackInfo>();

        list.Add(wormBoss);
        
        list.Add(twoMartyWormsOneToxicWormOneHiveHeraldOneArmoredWorm);
        list.Add(oneDireWormTwoToxicWormsTwoMartyrWormsOneHiveHerald);

        enemyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section7, list);
        #endregion

        #endregion

        #endregion

        #region Manse-1F

        #region Manse-1F-1c

        list = new List<EnemyPackInfo>();

        list.Add(oneExecutionerOneLieutenantTwoAxemenOneDisciplinarion);
        list.Add(oneLinebreakerOneAxemanOneSpearmanThreeJavelineers);
        list.Add(oneLancerThreeAxemenTwoJavelineers);
        list.Add(oneLieutenantOneDisciplinarianTenJavelineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section1c, list);

        #endregion

        #region Manse-1F-Dining Room

        list = new List<EnemyPackInfo>();

        list.Add(oneLinebreakerOneAxemanOneSpearmanOneDisciplinarion);

        enemyPackInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, list);

        #endregion

        #region Manse-1F-2a

        list = new List<EnemyPackInfo>();

        list.Add(oneExecutionerOneAxemanTwoSpearmanThreeJavelineers);
        list.Add(oneLancerThreeAxemenTwoJavelineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, list);

        #endregion

        #region Manse-1F-2b

        list = new List<EnemyPackInfo>();

        list.Add(oneLieutenantOneDisciplinarianTenLoyalists);

        enemyPackInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, list);

        #endregion

        #region Manse-1F-3a

        list = new List<EnemyPackInfo>();

        list.Add(oneLancerThreeAxemenTwoJavelineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3a, list);

        #endregion

        #endregion

        #region Manse-2F

        #region Manse-2F-2a

        list = new List<EnemyPackInfo>();

        list.Add(oneExecutionerOneLancerOneOverseer);

        enemyPackInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section2a, list);

        #endregion

        #region Manse-2F-3a

        list = new List<EnemyPackInfo>();

        list.Add(oneExecutionerOneLancerOneOverseer);
        list.Add(oneLinebreakerOneAxemanOneSpearmanThreeJavelineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3a, list);

        #endregion

        #region Manse-2F-3b

        list = new List<EnemyPackInfo>();

        list.Add(honorguardCaptainBossFight);

        enemyPackInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, list);

        #endregion

        #region Manse-2F-3c

        list = new List<EnemyPackInfo>();

        list.Add(oneLancerThreeAxemenTwoJavelineers);

        enemyPackInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3c, list);

        #endregion

        #region Manse-2F-Stockroom

        list = new List<EnemyPackInfo>();

        list.Add(oneLancerOneExecutionerOneLineBreaker);

        enemyPackInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, list);

        #endregion

        #region Manse-2F-Office

        list = new List<EnemyPackInfo>();

        list.Add(oneExecutionerOneLancerOneOverseerOneLieutenantOneLinebreaker);

        enemyPackInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.office, list);

        #endregion

        #endregion

        #region Pit

        #region Pit-1b

        list = new List<EnemyPackInfo>();

        list.Add(oneLinebreakerOneAxemanOneSpearmanOneDisciplinarion);

        enemyPackInfoDict.Add(ZoneKeyList.pit + LocationNameList.section1b, list);

        #endregion

        #region Pit-2d

        list = new List<EnemyPackInfo>();

        list.Add(stoneSaintPitBoss);

        enemyPackInfoDict.Add(ZoneKeyList.pit + LocationNameList.section2d, list);

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
