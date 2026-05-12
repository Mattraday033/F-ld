using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class DialogueList
{


    private readonly static Dialogue wallPatchDialogue = new Dialogue(new string[] { NPCNameList.wallPatch },
                                                            Resources.Load<TextAsset>(DialogueNameList.wallPatchPath));

    private readonly static Dialogue awkwardRubbleDialogue = new Dialogue(new string[] { NPCNameList.awkwardRubble },
                                                             Resources.Load<TextAsset>(DialogueNameList.awkwardRubblePath));
    private readonly static Dialogue ancientPortcullisDialogue = new Dialogue(new string[] { NPCNameList.ancientPortcullis},
                                                             Resources.Load<TextAsset>(DialogueNameList.ancientPortcullisPath));

    private readonly static Dialogue liftableGateDialogue = new Dialogue(new string[] { "", NPCNameList.liftableGate + 1},
                                                             Resources.Load<TextAsset>(DialogueNameList.liftableGatePath));

    private readonly static StoryStatRequirementVariableSource unstablePillarStrengthRequirement = new StoryStatRequirementVariableSource(StoryVariableNameList.strReqVariableName, Constants.sizeThree);
    private readonly static Dialogue unstablePillarDialogue = new Dialogue(new string[] { "", NPCNameList.unstablePillar},
                                                             Resources.Load<TextAsset>(DialogueNameList.unstablePillarPath),
                                                             unstablePillarStrengthRequirement);

    private readonly static Dialogue liftableRubbleDialogue = new Dialogue(new string[] { NPCNameList.liftableRubble },
                            Resources.Load<TextAsset>(DialogueNameList.liftableRubblePath));

    public static Dictionary<string, Dialogue> dialogueList;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeDialogueList()
    {
        dialogueList = new Dictionary<string, Dialogue>();

        #region Dialogues not attached to NPC's
        // addDialogueToList(DialogueNameList.afterKillingGuardsMineLvl3Key,
        //                  new Dialogue(new string[] { "", "Nándor", "Carter", "Guard Márcos", "Guard Pázmán", "Guard Réka" }, new GameObject[6], Resources.Load<TextAsset>(DialogueNameList.afterKillingGuardsMineLvl3Key)));

        // addDialogueToList(DialogueNameList.taborManse2F2BKey,
        //                  new Dialogue(new string[] { "", "Chief Tabor" }, new GameObject[2], Resources.Load<TextAsset>(DialogueNameList.taborManse2F2BKey), new NPCCombatInfo(new EnemyPackInfo[] { EnemyPackInfoList.taborFight },
        //                                                                                                                                                        new DeadNameList[] { new DeadNameList(new string[] { NPCNameList.chiefTabor }) })));

        // addDialogueToList(DialogueNameList.guardPunishmentConvoKey,
        //                  new Dialogue(new string[] { "", "Nándor", "Carter", "Kastor", "Janos", "Brush", "Géza", "Slave 1", "Slave 2", "Slave 3", "The Crowd", "Chief Tabor", "Guard Márcos", "Guard András", "Guard Réka", "Guard Pázmán", "Ervin", "Clay" }, new GameObject[18], Resources.Load<TextAsset>(DialogueNameList.guardPunishmentConvoKey)));


        // addDialogueToList(DialogueNameList.afterKillingAndrasConvoKey, new Dialogue(new string[] { "", "Janos" }, new GameObject[2], Resources.Load<TextAsset>(DialogueNameList.afterKillingAndrasConvoKey)));

        // addDialogueToList(DialogueNameList.vazulPath, new Dialogue(new string[] { "", NPCNameList.thatch + 1, NPCNameList.slate, NPCNameList.thatch + 1 }, Resources.Load<TextAsset>(DialogueNameList.vazulPath)));

        // addDialogueToList(DialogueNameList.taborAfterClayFightKey, new Dialogue(new string[] { "", "Chief Tabor" }, new GameObject[2], Resources.Load<TextAsset>(DialogueNameList.taborAfterClayFightKey), new TextAsset[] { Resources.Load<TextAsset>(DialogueNameList.chiefTaborPunishmentDialogueKey) }));

        #endregion

        #region Interactables

        addDialogueToList(NPCNameList.vaultableBarrels,
                            new Dialogue(new string[] { NPCNameList.vaultableBarrels },
                            Resources.Load<TextAsset>(DialogueNameList.dialogueResourcesPathName + PrefabNames.vaultableObject)));

        #endregion

        #region Slave Shack 1

        addDialogueToList(LocationNameList.slaveShackOne, NPCNameList.seb,
                            new Dialogue(new string[] { NPCNameList.seb },
                            Resources.Load<TextAsset>(DialogueNameList.sebPath)));

        addDialogueToList(LocationNameList.slaveShackOne, NPCNameList.balint,
                            new Dialogue(new string[] { NPCNameList.balint },
                            Resources.Load<TextAsset>(DialogueNameList.balintPath)));

        #endregion
        #region Slave Shack 2

        addDialogueToList(LocationNameList.slaveShackTwo, NPCNameList.brush,
                            new Dialogue(new string[] { NPCNameList.brush, NPCNameList.géza, NPCNameList.guardLaszlo, NPCNameList.guardLaszlo + 1, NPCNameList.géza + 1, NPCNameList.géza + 2},
                            Resources.Load<TextAsset>(DialogueNameList.introDialoguePath)));

        addDialogueToList(LocationNameList.slaveShackTwo, NPCNameList.géza,
                            new Dialogue(new string[] { NPCNameList.géza },
                            Resources.Load<TextAsset>(DialogueNameList.gézaPath)));

        #endregion
        #region Slave Shack 3

        addDialogueToList(LocationNameList.slaveShackThree, NPCNameList.janos,
                            new Dialogue(new string[] { NPCNameList.janos, NPCNameList.guardAndras, NPCNameList.guardAndras + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.janosPath),
                            DialogueCombatInfoList.andrasCombatInfo));

        addDialogueToList(LocationNameList.slaveShackThree, NPCNameList.guardAndras + 1,
                            new Dialogue(new string[] { NPCNameList.guardAndras + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.andrasPath)));

        addDialogueToList(LocationNameList.slaveShackThree, DialogueNameList.janosAfterKillingAndrasKey,
                            new Dialogue(new string[] { NPCNameList.janos },
                            Resources.Load<TextAsset>(DialogueNameList.janosAfterKillingAndrasPath)));

        #endregion
        #region Slave Shack 4

        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.kastor,
                            new Dialogue(new string[] { NPCNameList.kastor, NPCNameList.nandor, NPCNameList.carter, NPCNameList.guardMarcos, NPCNameList.guardMarcos+1 },
                            Resources.Load<TextAsset>(DialogueNameList.kastorPlanPath)));

        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.guardMarcos,
                            new Dialogue(new string[] { NPCNameList.guardMarcos },
                            Resources.Load<TextAsset>(DialogueNameList.ml3MarcosPath)));

        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.guardMarcos+1,
                            new Dialogue(new string[] { NPCNameList.guardMarcos+1 },
                            Resources.Load<TextAsset>(DialogueNameList.ml3MarcosPath)));

        addPartyMemberDialogue(LocationNameList.slaveShackFour, NPCNameList.nandor);
        addPartyMemberDialogue(LocationNameList.slaveShackFour, NPCNameList.carter);

        #endregion
        #region Slave Shack 5

        addDialogueToList(LocationNameList.slaveShackFive, NPCNameList.ervin,
                            new Dialogue(new string[] { NPCNameList.ervin },
                            Resources.Load<TextAsset>(DialogueNameList.ervinPath)));

        addDialogueToList(LocationNameList.slaveShackFive, NPCNameList.wallPatch, wallPatchDialogue);

        #endregion
        #region Slave Shack 6

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.thatch,
                            new Dialogue(new string[] { NPCNameList.thatch, NPCNameList.rubble },
                            Resources.Load<TextAsset>(DialogueNameList.thatchPath)));

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.slate,
                            new Dialogue(new string[] { NPCNameList.slate },
                            Resources.Load<TextAsset>(DialogueNameList.slatePath)));

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.guardVazul,
                            new Dialogue(new string[] { "", NPCNameList.guardVazul, NPCNameList.slate, NPCNameList.thatch + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.vazulPath),
                            DialogueCombatInfoList.vazulCombatInfo));

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.rubble,
                            new Dialogue(new string[] { NPCNameList.rubble },
                            Resources.Load<TextAsset>(DialogueNameList.immovableRubblePath)));

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.wallPatch, wallPatchDialogue);

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.liftableRubble, liftableRubbleDialogue);

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.awkwardRubble,
                            new Dialogue(new string[] { NPCNameList.awkwardRubble },
                            Resources.Load<TextAsset>(DialogueNameList.fallenBeamPath)));

        #endregion
        #region Slave Shack 7

        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.slave,
                            new SingleCharacterDialogue(NPCNameList.slave,
                            Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath)));

        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.dezso,
                            new Dialogue(new string[]{
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.dezso,
                                                        NPCNameList.loam,
                                                        NPCNameList.guard+1,
                                                        NPCNameList.guard+2,
                                                        NPCNameList.guard+3,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.dezsoPath),
                            DialogueCombatInfoList.dezsoHostageFight));

        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.loam,
                            new Dialogue(new string[]{ NPCNameList.loam},
                            Resources.Load<TextAsset>(DialogueNameList.loamPath)));

        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.guard+1,
                            new SingleCharacterDialogue(NPCNameList.guard+1,
                            Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath)));
        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.guard+2,
                            new SingleCharacterDialogue(NPCNameList.guard+2,
                            Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath)));
        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.guard+3,
                            new SingleCharacterDialogue(NPCNameList.guard+3,
                            Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath)));

        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.guard+4,
                            new SingleCharacterDialogue(NPCNameList.guard+4,
                            Resources.Load<TextAsset>(DialogueNameList.guardsAfterHostagesPath)));

        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.weft,
                            new Dialogue(new string[]{
                                                        NPCNameList.playerNamePlaceHolder, 
                                                        NPCNameList.weft, 
                                                        NPCNameList.weft+1
                                                    },
                            Resources.Load<TextAsset>(DialogueNameList.weftAfterHostagesPath)));
        #endregion
        #region Slave Shack 8

        addDialogueToList(LocationNameList.slaveShackEight, NPCNameList.weft,
                            new Dialogue(new string[] { NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.weft,
                                                        NPCNameList.overseer
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.weftPath)));

        #endregion

        #region Guard Shack
        addDialogueToList(LocationNameList.guardShack, NPCNameList.guardLaszlo,
                            new Dialogue(new string[] { NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.guardLaszlo,
                                                        NPCNameList.weft
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.laszloPath)));
        #endregion

        #region Guard House NE

        addDialogueToList(LocationNameList.guardHouseNorthEast, NPCNameList.barracksGate,
                            new SingleCharacterDialogue(NPCNameList.barracksGate,
                            Resources.Load<TextAsset>(DialogueNameList.barracksGatePath)));

        #endregion

        #region Guard House SW

        addDialogueToList(LocationNameList.guardHouseSouthWest, NPCNameList.barracksGate,
                            new SingleCharacterDialogue(NPCNameList.barracksGate,
                            Resources.Load<TextAsset>(DialogueNameList.barracksGatePath)));


        addDialogueToList(LocationNameList.guardHouseSouthWest, NPCNameList.guard,
                            new SingleCharacterDialogue(NPCNameList.guard,
                            Resources.Load<TextAsset>(DialogueNameList.barracksGuardPath)));

        #endregion

        #region Stockhouse

        addDialogueToList(LocationNameList.stockhouse, NPCNameList.uros,
                            new Dialogue(new string[] { NPCNameList.uros, NPCNameList.quartermasterEmese },
                            Resources.Load<TextAsset>(DialogueNameList.urosPath)));

        addDialogueToList(LocationNameList.stockhouse, NPCNameList.quartermasterEmese,
                            new Dialogue(new string[] { NPCNameList.quartermasterEmese, NPCNameList.uros },
                            Resources.Load<TextAsset>(DialogueNameList.emesePath)));

        addDialogueToList(LocationNameList.stockhouse, NPCNameList.crate,
                            new Dialogue(new string[] { NPCNameList.crate },
                            Resources.Load<TextAsset>(DialogueNameList.dudCratePath)));
        addDialogueToList(LocationNameList.stockhouse, NPCNameList.crate + 1,
                            new Dialogue(new string[] { NPCNameList.crate + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.dudCratePath)));

        addDialogueToList(LocationNameList.stockhouse, NPCNameList.barrels,
                            new Dialogue(new string[] { NPCNameList.barrels },
                            Resources.Load<TextAsset>(DialogueNameList.barrelsWithNuggetPath)));
        addDialogueToList(LocationNameList.stockhouse, NPCNameList.barrels + 1,
                            new Dialogue(new string[] { NPCNameList.barrels + 1},
                            Resources.Load<TextAsset>(DialogueNameList.dudBarrelPath)));

        #endregion
        #region Stables

        addDialogueToList(LocationNameList.stables, NPCNameList.wallPatch, wallPatchDialogue);

        addDialogueToList(LocationNameList.stables, NPCNameList.beam,
                            new Dialogue(new string[] { NPCNameList.beam },
                            Resources.Load<TextAsset>(DialogueNameList.beamPath)));

        addDialogueToList(LocationNameList.stables, NPCNameList.horse,
                            new Dialogue(new string[] { NPCNameList.horse },
                            Resources.Load<TextAsset>(DialogueNameList.horsePath)));
        addDialogueToList(LocationNameList.stables, NPCNameList.horse + 1,
                            new Dialogue(new string[] { NPCNameList.horse + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.horsePath)));
        addDialogueToList(LocationNameList.stables, NPCNameList.horse + 2,
                            new Dialogue(new string[] { NPCNameList.horse + 2 },
                            Resources.Load<TextAsset>(DialogueNameList.horsePath)));

        #endregion
        #region Temple

        addDialogueToList(LocationNameList.temple, NPCNameList.wallPatch, wallPatchDialogue);

        #endregion
        #region Mess Hall

        addDialogueToList(LocationNameList.messHall, NPCNameList.noBrand+1,
                            new Dialogue(new string[] { NPCNameList.noBrand+1 },
                            Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath)));

        addDialogueToList(LocationNameList.messHall, NPCNameList.kende,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.kende,
                                                        NPCNameList.weft
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.kendePath)));
        #endregion

        #region Body Pile

        addDialogueToList(LocationNameList.bodyPile, NPCNameList.body+1,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor,
                                                        NPCNameList.weft
                                                     },
                            Resources.Load<TextAsset>(DialogueNameList.thiefsBodyPath)));

        #endregion

        #region NECamp

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.leafPile,
                            new Dialogue(new string[] { NPCNameList.leafPile },
                            Resources.Load<TextAsset>(DialogueNameList.leafPilePath)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.overseer,
                            new Dialogue(new string[] { NPCNameList.overseer },
                            Resources.Load<TextAsset>(DialogueNameList.overseerPath)));

        addDialogueToList(LocationNameList.campNorthEast, DialogueNameList.slavesAfterKillingOverseerCampNEKey,
                         new Dialogue(new string[] { 
                                                        "", 
                                                        NPCNameList.nandor, 
                                                        NPCNameList.carter, 
                                                        NPCNameList.géza, 
                                                        NPCNameList.janos, 
                                                        NPCNameList.clay, 
                                                        NPCNameList.slaveOne, 
                                                        NPCNameList.slaveTwo, 
                                                        NPCNameList.slaveThree, 
                                                        NPCNameList.slaveFour, 
                                                        NPCNameList.crowd
                                                        }, 
                            Resources.Load<TextAsset>(DialogueNameList.slavesAfterKillingOverseerCampNEPathName)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.temple+1,
                         new Dialogue(new string[] { NPCNameList.temple+1 }, 
                            Resources.Load<TextAsset>(DialogueNameList.slaveFiveNECampPathName)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.slave+6,
                         new Dialogue(new string[] { NPCNameList.slave+6 }, 
                            Resources.Load<TextAsset>(DialogueNameList.slaveSixNECampPathName)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.slave+7,
                         new Dialogue(new string[] { NPCNameList.slave+7 }, 
                            Resources.Load<TextAsset>(DialogueNameList.slaveSevenNECampPathName)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.slave+8,
                         new Dialogue(new string[] { NPCNameList.slave+8 }, 
                            Resources.Load<TextAsset>(DialogueNameList.slaveEightNECampPathName)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.clay+1,
                         new Dialogue(new string[] { NPCNameList.clay+1 }, 
                            Resources.Load<TextAsset>(DialogueNameList.slaveNineNECampPathName)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.woundedSlave,
                         new Dialogue(new string[] { NPCNameList.woundedSlave }, 
                            Resources.Load<TextAsset>(DialogueNameList.woundedSlaveNECampPathName)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.woundedSlave+1,
                         new Dialogue(new string[] { NPCNameList.woundedSlave+1 }, 
                            Resources.Load<TextAsset>(DialogueNameList.woundedSlaveOneNECampPathName)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.woundedSlave+2,
                         new Dialogue(new string[] { NPCNameList.woundedSlave+2 }, 
                            Resources.Load<TextAsset>(DialogueNameList.woundedSlaveTwoNECampPathName)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.guardMarcos,
                         new Dialogue(new string[] { NPCNameList.guardMarcos }, 
                            Resources.Load<TextAsset>(DialogueNameList.ml3MarcosPath)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.uros,
                         new Dialogue(new string[] { NPCNameList.uros }, 
                            Resources.Load<TextAsset>(DialogueNameList.urosNECampPathName)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.kastor,
                         new Dialogue(new string[] { NPCNameList.kastor }, 
                            Resources.Load<TextAsset>(DialogueNameList.kastorNECampPathName)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.géza+1,
                         new Dialogue(new string[] { NPCNameList.géza+1 }, 
                            Resources.Load<TextAsset>(DialogueNameList.gézaNECampPathName)));

        addDialogueToList(LocationNameList.campNorthEast, MonsterNameList.brandedConscript,
                        new SingleCharacterDialogue(MonsterNameList.brandedConscript,
                        Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath)));

        addDialogueToList(LocationNameList.campNorthEast, MonsterNameList.spearman,
                        new SingleCharacterDialogue(MonsterNameList.spearman,
                        Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.guard+2,
                            new SingleCharacterDialogue(NPCNameList.guard+2,
                            Resources.Load<TextAsset>(DialogueNameList.situationGuardPath)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.guard+3,
                            new SingleCharacterDialogue(NPCNameList.guard+3,
                            Resources.Load<TextAsset>(DialogueNameList.situationGuardPath)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.guard+1,
                            new SingleCharacterDialogue(NPCNameList.guard+1,
                            Resources.Load<TextAsset>(DialogueNameList.situationGuardPath)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.honorguard+1,
                            new SingleCharacterDialogue(NPCNameList.honorguard+1,
                            Resources.Load<TextAsset>(DialogueNameList.situationGuardPath)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.chiefTabor,
                            new Dialogue(new string[]{ 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor,
                                                        NPCNameList.captainAdela,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard+2
                                                    },
                            Resources.Load<TextAsset>(DialogueNameList.taborNEPath)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.captainAdela,
                            new Dialogue(new string[]{ NPCNameList.captainAdela},
                            Resources.Load<TextAsset>(DialogueNameList.captainAdelaPathName)));
        #endregion
        #region CenterCamp

        addDialogueToList(LocationNameList.campCenter, NPCNameList.csalan,
                            new Dialogue(new string[] { NPCNameList.csalan },
                            Resources.Load<TextAsset>(DialogueNameList.csalanPath)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.guard + 1,
                            new Dialogue(new string[] { NPCNameList.guard + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.guardWatchingTaborPath)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.temple,
                            new Dialogue(new string[] { NPCNameList.temple },
                            Resources.Load<TextAsset>(DialogueNameList.templePath)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.chiefTabor,
                            new Dialogue(new string[] { NPCNameList.chiefTabor,
                                                        NPCNameList.feher,
                                                        NPCNameList.branded,
                                                        NPCNameList.branded+1,
                                                        NPCNameList.branded+2,
                                                        NPCNameList.weft},
                            Resources.Load<TextAsset>(DialogueNameList.taborPath)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.feher,
                            new Dialogue(new string[] { NPCNameList.feher },
                            Resources.Load<TextAsset>(DialogueNameList.feherPath)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.branded,
                            new Dialogue(new string[] { NPCNameList.branded },
                            Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath)));
        addDialogueToList(LocationNameList.campCenter, NPCNameList.branded+1,
                            new Dialogue(new string[] { NPCNameList.branded+1 },
                            Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath)));
        addDialogueToList(LocationNameList.campCenter, NPCNameList.branded+2,
                            new Dialogue(new string[] { NPCNameList.branded+2 },
                            Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.campGate, 
                            new Dialogue(new string[] { NPCNameList.campGate },
                            Resources.Load<TextAsset>(DialogueNameList.campGatePath)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.wallPatch, wallPatchDialogue);

        addDialogueToList(LocationNameList.campCenter, NPCNameList.barricadeGuards+1,
                                new SingleCharacterDialogue(NPCNameList.barricadeGuards+1,
                                Resources.Load<TextAsset>(DialogueNameList.firstBarricadeGuardsPath),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey1)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.barricade+1,
                                new SingleCharacterDialogue(NPCNameList.barricade+1,
                                Resources.Load<TextAsset>(DialogueNameList.firstBarricadeGuardsPath),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey1)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.page,
                                new Dialogue(new string[] { 
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.page,
                                                            NPCNameList.carter,
                                                            NPCNameList.carter
                                                          },
                            Resources.Load<TextAsset>(DialogueNameList.pageBeforeLeavingPath)));

        #endregion
        #region SECamp

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.wallPatch, wallPatchDialogue);
        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.statue,
                            new SingleCharacterDialogue(NPCNameList.statue,
                            Resources.Load<TextAsset>(DialogueNameList.directorStatuePath)));
                            
        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.toppledStatue,
                            new SingleCharacterDialogue(NPCNameList.toppledStatue,
                            Resources.Load<TextAsset>(DialogueNameList.brokenDirectorStatuePath)));

        #region Guard Punishment Scene

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave,
                            new SingleCharacterDialogue(NPCNameList.slave,
                            Resources.Load<TextAsset>(DialogueNameList.crowdSlave1Path)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+1,
                            new SingleCharacterDialogue(NPCNameList.slave,
                            Resources.Load<TextAsset>(DialogueNameList.crowdSlave1Path)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+2,
                            new SingleCharacterDialogue(NPCNameList.slave,
                            Resources.Load<TextAsset>(DialogueNameList.crowdSlave2Path)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+3,
                            new SingleCharacterDialogue(NPCNameList.slave ,
                            Resources.Load<TextAsset>(DialogueNameList.crowdSlave3Path)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+4,
                            new SingleCharacterDialogue(NPCNameList.slave ,
                            Resources.Load<TextAsset>(DialogueNameList.crowdSlave3Path)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+5,
                            new SingleCharacterDialogue(NPCNameList.slave ,
                            Resources.Load<TextAsset>(DialogueNameList.crowdSlave3Path)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+6,
                            new SingleCharacterDialogue(NPCNameList.slave ,
                            Resources.Load<TextAsset>(DialogueNameList.crowdSlave3Path)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.crowd,
                            new SingleCharacterDialogue(NPCNameList.slave ,
                            Resources.Load<TextAsset>(DialogueNameList.crowdSlave3Path)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.kastor,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.kastor,
                                                            NPCNameList.carter,
                                                            NPCNameList.kastor,
                                                            NPCNameList.marcos,
                                                            NPCNameList.andras,
                                                            NPCNameList.chiefTabor,
                                                            NPCNameList.crowd,
                                                            NPCNameList.clay,
                                                            NPCNameList.géza,
                                                            NPCNameList.thatch
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.kastorGuardPunishmentPath),
                            new TextAsset[]{Resources.Load<TextAsset>(DialogueNameList.nandorGuardPunishmentPath)}));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.brush,
                            new SingleCharacterDialogue(NPCNameList.brush,
                            Resources.Load<TextAsset>(DialogueNameList.brushGuardPunishmentPath)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.géza,
                            new SingleCharacterDialogue(NPCNameList.géza,
                            Resources.Load<TextAsset>(DialogueNameList.gézaGuardPunishmentPath)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.ervin,
                            new SingleCharacterDialogue(NPCNameList.ervin,
                            Resources.Load<TextAsset>(DialogueNameList.ervinGuardPunishmentPath)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.janos,
                            new SingleCharacterDialogue(NPCNameList.janos,
                            Resources.Load<TextAsset>(DialogueNameList.janosGuardPunishmentPath)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.thatch,
                            new SingleCharacterDialogue(NPCNameList.thatch,
                            Resources.Load<TextAsset>(DialogueNameList.thatchGuardPunishmentPath)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.carter,
                            new SingleCharacterDialogue(NPCNameList.carter,
                            Resources.Load<TextAsset>(DialogueNameList.carterGuardPunishmentPath)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.nandor,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.nandor,
                                                            NPCNameList.carter,
                                                            NPCNameList.kastor,
                                                            NPCNameList.janos,
                                                            NPCNameList.brush,
                                                            NPCNameList.géza,
                                                            NPCNameList.crowd,
                                                            NPCNameList.chiefTabor,
                                                            NPCNameList.marcos,
                                                            NPCNameList.andras,
                                                            NPCNameList.reka,
                                                            NPCNameList.pazman,
                                                            NPCNameList.ervin,
                                                            NPCNameList.clay,
                                                            NPCNameList.nandor+1
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.guardPunishmentStartConvoPath),
                            new TextAsset[]{Resources.Load<TextAsset>(DialogueNameList.nandorGuardPunishmentPath)}));


        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.nandor+1,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.nandor+1,
                                                            NPCNameList.carter,
                                                            NPCNameList.kastor,
                                                            NPCNameList.marcos,
                                                            NPCNameList.andras,
                                                            NPCNameList.chiefTabor,
                                                            NPCNameList.crowd,
                                                            NPCNameList.clay,
                                                            NPCNameList.géza,
                                                            NPCNameList.thatch
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.nandorGuardPunishmentPath)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.marcos,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.marcos,
                                                            NPCNameList.crowd
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.marcosGuardPunishmentPath)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.andras,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.andras,
                                                            NPCNameList.crowd,
                                                            NPCNameList.janos
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.andrasGuardPunishmentPath)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.reka,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.reka,
                                                            NPCNameList.crowd
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.rekaGuardPunishmentPath)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.pazman,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.pazman,
                                                            NPCNameList.crowd,
                                                            NPCNameList.ervin,
                                                            NPCNameList.ervin+1
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.pazmanGuardPunishmentPath)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.chiefTabor,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.chiefTabor,
                                                            NPCNameList.crowd,
                                                            NPCNameList.clay
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.taborGuardPunishmentPath),
                            DialogueCombatInfoList.clayFightForTaborCombatInfo));

        addDialogueToList(LocationNameList.campSouthEast, DialogueNameList.taborAfterClayFightKey,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.chiefTabor
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.taborAfterClayFightPath),
                            new TextAsset[]{Resources.Load<TextAsset>(DialogueNameList.taborGuardPunishmentPath)}));             

        #endregion

        #endregion
        #region MineEntranceCamp

        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guard,
                            new SingleCharacterDialogue(NPCNameList.guard,
                            Resources.Load<TextAsset>(DialogueNameList.genericGuardDialoguePath)));
        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guard+1,
                            new SingleCharacterDialogue(NPCNameList.guard+1,
                            Resources.Load<TextAsset>(DialogueNameList.genericGuardDialoguePath)));

        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa, NPCNameList.barricade, NPCNameList.guardMuzsa + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.muszaPath),
                            DialogueCombatInfoList.muzsaCombatInfo));
        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa + 1,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa + 1, NPCNameList.barricade, NPCNameList.guardMuzsa + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.muszaPath)));

        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.barracksGate,
                            new SingleCharacterDialogue(NPCNameList.barracksGate,
                            Resources.Load<TextAsset>(DialogueNameList.barracksGatePath)));

        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.uros,
                            new Dialogue(new string[] { NPCNameList.uros },
                            Resources.Load<TextAsset>(DialogueNameList.urosPath)));

        #endregion
        #region ManseCamp

        addDialogueToList(LocationNameList.campManse, NPCNameList.imre,
                            new Dialogue(new string[] { NPCNameList.imre },
                            Resources.Load<TextAsset>(DialogueNameList.imrePath),
                            DialogueCombatInfoList.imreCombatInfo));

        addDialogueToList(LocationNameList.campManse, NPCNameList.imre+1,
                            new Dialogue(new string[] { NPCNameList.imre+1 },
                            Resources.Load<TextAsset>(DialogueNameList.imrePath)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.manseFrontDoor,
                            new Dialogue(new string[] { NPCNameList.manseFrontDoor },
                            Resources.Load<TextAsset>(DialogueNameList.manseFrontDoorPath)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.manseServiceEntrance + 1,
                            new Dialogue(new string[] { NPCNameList.manseServiceEntrance },
                            Resources.Load<TextAsset>(DialogueNameList.manseServiceEntrancePath)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.barricadeGuards+2,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+2,
                                                        NPCNameList.guardAndras+2
                                                      },
                                Resources.Load<TextAsset>(DialogueNameList.secondBarricadeGuardsPath),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey2)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.barricade+2,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+2,
                                                        NPCNameList.guardAndras+2
                                                      },
                                Resources.Load<TextAsset>(DialogueNameList.secondBarricadeGuardsPath),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey2)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.barricadeGuards+3,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+3,
                                                        NPCNameList.guardAndras+3
                                                      },
                                Resources.Load<TextAsset>(DialogueNameList.secondBarricadeGuardsPath),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey3)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.barricade+3,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+3,
                                                        NPCNameList.guardAndras+3
                                                      },
                                Resources.Load<TextAsset>(DialogueNameList.secondBarricadeGuardsPath),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey3)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.barracksGate+2,
                            new SingleCharacterDialogue(NPCNameList.barracksGate+2,
                            Resources.Load<TextAsset>(DialogueNameList.barracksGatePath)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.chiefTabor,
                            new Dialogue( new string[]{ 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor,
                                                        NPCNameList.weft
                                                        },
                            Resources.Load<TextAsset>(DialogueNameList.taborWeftHutPath)));

        #endregion
        #region NWCamp

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.guard,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor+1
                                                      },
                            Resources.Load<TextAsset>(DialogueNameList.guardNWPath),
                            new TextAsset[]
                            {
                                Resources.Load<TextAsset>(DialogueNameList.taborEndOfTutorialPath)
                            }));

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor+1
                                                      },
                                Resources.Load<TextAsset>(DialogueNameList.taborEndOfTutorialPath)));

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor+1,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor+1,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor+2
                                                      },
                                Resources.Load<TextAsset>(DialogueNameList.taborIntimidateTutorialPath)));

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor+2,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor+2,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor+3
                                                      },
                                Resources.Load<TextAsset>(DialogueNameList.taborCunningTutorialPath)));

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor+3,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor+3,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor+4
                                                      },
                                Resources.Load<TextAsset>(DialogueNameList.taborLeadershipTutorialPath)));

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor+4,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor+4,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor
                                                      },
                                Resources.Load<TextAsset>(DialogueNameList.taborObservationTutorialPath)));


        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.awkwardRubble, awkwardRubbleDialogue);
        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.wallPatch, wallPatchDialogue);

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor+5,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor+5,
                                                        NPCNameList.weft+1,
                                                        NPCNameList.guard+1
                                                      },
                            Resources.Load<TextAsset>(DialogueNameList.taborBodyPilePath)));

        #endregion

        #region MineLvl_1

        addDialogueToList(ZoneKeyList.mineLvl1 + LocationNameList.section1b, NPCNameList.awkwardRubble, awkwardRubbleDialogue);

        addDialogueToList(ZoneKeyList.mineLvl1 + LocationNameList.section1c, NPCNameList.liftableGate, liftableGateDialogue);

        #endregion

        #region MineLvl_2

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section1a, NPCNameList.suspiciousWall,
                                                                                  new Dialogue(new string[] { NPCNameList.suspiciousWall },
                                                                                  Resources.Load<TextAsset>(DialogueNameList.suspiciousWallPathML2 + 1)));
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7b, NPCNameList.suspiciousWall,
                                                                                  new Dialogue(new string[] { NPCNameList.suspiciousWall },
                                                                                  Resources.Load<TextAsset>(DialogueNameList.suspiciousWallPathML2 + 2)));

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.controlPanel, new Dialogue(new string[] { NPCNameList.controlPanel},
                                                                                  Resources.Load<TextAsset>(DialogueNameList.controlPanelPath)));

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardPazman,
                                                            new Dialogue(new string[] { "", NPCNameList.guardPazman},
                                                             Resources.Load<TextAsset>(DialogueNameList.pazmanML3CampPath)));                                        

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardVirag, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardVirag},
                                                             Resources.Load<TextAsset>(DialogueNameList.viragML3CampPath)));

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardReka, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardReka},
                                                             Resources.Load<TextAsset>(DialogueNameList.rekaML3CampPath)));

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.overseerGaspar, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.overseerGaspar,
                                                                NPCNameList.guardPazman,
                                                                NPCNameList.overseerGaspar,
                                                                NPCNameList.guardReka,
                                                                NPCNameList.guardVirag
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3GuardBarricadePath),
                                                             DialogueCombatInfoList.mineLvl3GuardsCombatInfo));

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2b, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2b, NPCNameList.mineArmoryGate + 1,
                                                                                  new Dialogue(new string[] { NPCNameList.mineArmoryGate },
                                                                                  Resources.Load<TextAsset>(DialogueNameList.mineArmoryGatePath)));

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section3a, NPCNameList.awkwardRubble, awkwardRubbleDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section3a, NPCNameList.awkwardRubble + 1, awkwardRubbleDialogue);

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section3b, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section3b, NPCNameList.liftableGate + 1, liftableGateDialogue);

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section6, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis + 1, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis + 2, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis + 3, ancientPortcullisDialogue);

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7b, NPCNameList.awkwardRubble, awkwardRubbleDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7b, NPCNameList.ancientPortcullis + 1, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7b, NPCNameList.liftableGate + 2, liftableGateDialogue);

        #endregion

        #region MineLvl_3

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section1b, NPCNameList.suspiciousWall,
                                                                                  new Dialogue(new string[] { NPCNameList.suspiciousWall },
                                                                                  Resources.Load<TextAsset>(DialogueNameList.suspiciousWallPathML3)));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section2b, NPCNameList.liftableGate, liftableGateDialogue);
        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section2b, NPCNameList.ancientPortcullis+1, ancientPortcullisDialogue);

        #region MineLvl_3-3b

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.liftableGate, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.liftableGate,
                                                                NPCNameList.guardPazman,
                                                                NPCNameList.overseerGaspar,
                                                                NPCNameList.guardReka,
                                                                NPCNameList.guardVirag,
                                                                NPCNameList.barricade
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3GuardCampLiftableGatePath),
                                                             DialogueCombatInfoList.mineLvl3GuardsCombatInfo, 
                                                             new TextAsset[]
                                                             {
                                                                 Resources.Load<TextAsset>(DialogueNameList.ml3GuardBarricadePath)
                                                             }));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardPazman, //unreachable, for pazman behind barricade
                                                            new Dialogue(new string[] { "", NPCNameList.guardPazman},
                                                             Resources.Load<TextAsset>(DialogueNameList.pazmanML3CampPath)));                                        
        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardPazman+1, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardPazman+1},
                                                             Resources.Load<TextAsset>(DialogueNameList.pazmanML3CampPath)));
                                                            
        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardVirag, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardVirag},
                                                             Resources.Load<TextAsset>(DialogueNameList.viragML3CampPath)));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardReka, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardReka},
                                                             Resources.Load<TextAsset>(DialogueNameList.rekaML3CampPath)));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.overseerGaspar, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.overseerGaspar,
                                                                NPCNameList.guardPazman,
                                                                NPCNameList.overseerGaspar,
                                                                NPCNameList.guardReka,
                                                                NPCNameList.guardVirag,
                                                                NPCNameList.barricade
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3GuardBarricadePath),
                                                             DialogueCombatInfoList.mineLvl3GuardsCombatInfo));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.barricade, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.barricade,
                                                                NPCNameList.guardPazman,
                                                                NPCNameList.overseerGaspar,
                                                                NPCNameList.guardReka,
                                                                NPCNameList.guardVirag,
                                                                NPCNameList.barricade
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3GuardBarricadePath),
                                                             DialogueCombatInfoList.mineLvl3GuardsCombatInfo));

        #endregion

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section4b, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section4b, NPCNameList.ancientPortcullis+1, ancientPortcullisDialogue);

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section5, NPCNameList.liftableGate, liftableGateDialogue);

        #region MineLvl_3-Miner Camp

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.barricade, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.barricade,
                                                                NPCNameList.carter,
                                                                NPCNameList.nandor,
                                                                NPCNameList.guardMarcos
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3MinerBarricadePath),
                                                             new TextAsset[]{ 
                                                                                Resources.Load<TextAsset>(DialogueNameList.ml3MarcosPath),
                                                                                Resources.Load<TextAsset>(DialogueNameList.ml3MinerBarricadePath)
                                                                            }));

        // addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter, 
        //                                                     new Dialogue(new string[] 
        //                                                     { 
        //                                                         "", 
        //                                                         NPCNameList.carter, 
        //                                                         NPCNameList.nandor
        //                                                     },
        //                                                      Resources.Load<TextAsset>(DialogueNameList.ml3CarterPath)));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter+1, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.carter+1
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3CarterPath)));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.guardMarcos, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.guardMarcos
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3MarcosPath)));

        addPartyMemberDialogue(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.nandor);
        addPartyMemberDialogue(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter);

        // addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.nandor, 
        //                                                     new Dialogue(new string[] 
        //                                                     { 
        //                                                         "", 
        //                                                         NPCNameList.nandor,
        //                                                         NPCNameList.carter
        //                                                     },
        //                                                      Resources.Load<TextAsset>(DialogueNameList.ml3NandorPath)));

        #endregion

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section6a, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section6a, NPCNameList.unstablePillar, unstablePillarDialogue);

        #region MineLvl_3-7

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.unstablePillar, unstablePillarDialogue);

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.rubble, 
                                                                                new Dialogue(new string[] { 
                                                                                                            NPCNameList.rubble,
                                                                                                            NPCNameList.rubble,
                                                                                                            NPCNameList.guardPazman,
                                                                                                            NPCNameList.guardReka,
                                                                                                            NPCNameList.guardVirag,
                                                                                                            NPCNameList.overseerGaspar,
                                                                                                            NPCNameList.carter,
                                                                                                            NPCNameList.nandor,
                                                                                                            NPCNameList.guardMarcos,
                                                                                                            NPCNameList.guardMarcos+1
                                                                                                        },
                                                                                Resources.Load<TextAsset>(DialogueNameList.pocketRubblePathML3),
                                                                                DialogueCombatInfoList.breachRubbleCombatInfo));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section7, DialogueNameList.afterKillingGuardsMineLvl3Key, 
                                                                                new Dialogue(new string[] { 
                                                                                                            NPCNameList.nandor,
                                                                                                            NPCNameList.carter,
                                                                                                            NPCNameList.guardMarcos,
                                                                                                            NPCNameList.guardPazman,
                                                                                                            NPCNameList.guardReka
                                                                                                        },
                                                                                Resources.Load<TextAsset>(DialogueNameList.afterKillingGuardsMineLvl3Path)));

        // addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section7, NPCNameList.rubble+1, 
        //                                                                         new Dialogue(new string[] { 
        //                                                                                                     NPCNameList.rubble+1,
        //                                                                                                     NPCNameList.rubble,
        //                                                                                                     NPCNameList.guardPazman,
        //                                                                                                     NPCNameList.guardReka,
        //                                                                                                     NPCNameList.guardVirag,
        //                                                                                                     NPCNameList.overseerGaspar,
        //                                                                                                     NPCNameList.carter,
        //                                                                                                     NPCNameList.nandor,
        //                                                                                                     NPCNameList.guardMarcos,
        //                                                                                                     NPCNameList.guardMarcos+1
        //                                                                                                 },
        //                                                                         Resources.Load<TextAsset>(DialogueNameList.pocketRubblePathML3)));
        #endregion

        #endregion

        #region Manse-1f

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, NPCNameList.barricadeGuards+4,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+4,
                                                        NPCNameList.guardAndras+4
                                                      },
                                Resources.Load<TextAsset>(DialogueNameList.secondBarricadeGuardsPath),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey4)));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, NPCNameList.barricade+4,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+4,
                                                        NPCNameList.guardAndras+4
                                                      },
                                Resources.Load<TextAsset>(DialogueNameList.secondBarricadeGuardsPath),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey4)));
                                

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, NPCNameList.gate, ancientPortcullisDialogue);

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.kende,
                                new Dialogue(new string[]   { 
                                                                NPCNameList.playerNamePlaceHolder, 
                                                                NPCNameList.kende, 
                                                                NPCNameList.imre+1, 
                                                                NPCNameList.imre+2, 
                                                                NPCNameList.pan, 
                                                                NPCNameList.guard, 
                                                                NPCNameList.noBrand+2
                                                            }, 
                                 Resources.Load<TextAsset>(DialogueNameList.kendeUponEnteringKitchensPathName), 
                                 DialogueCombatInfoList.kendeInKitchensCombatInfo));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.imre+1,
                            new Dialogue(new string[]   { NPCNameList.imre+1 }, 
                                Resources.Load<TextAsset>(DialogueNameList.loyalImrePathName)));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.pan,
                            new SingleCharacterDialogue(NPCNameList.pan, 
                            Resources.Load<TextAsset>(DialogueNameList.panPathName)));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, NPCNameList.ancientPortcullis,
                            new Dialogue(new string[]   { NPCNameList.ancientPortcullis }, 
                                Resources.Load<TextAsset>(DialogueNameList.ancientPortcullisPath)));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, NPCNameList.ancientPortcullis,
                            new Dialogue(new string[]   { NPCNameList.ancientPortcullis }, 
                                Resources.Load<TextAsset>(DialogueNameList.ancientPortcullisPath)));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.beam,
                            new Dialogue(new string[]   { 
                                                            NPCNameList.playerNamePlaceHolder, 
                                                            NPCNameList.beam,
                                                            NPCNameList.csalan,
                                                            NPCNameList.horse,
                                                            NPCNameList.horse+1,
                                                            NPCNameList.horse+2
                                                        }, 
                                Resources.Load<TextAsset>(DialogueNameList.beamAndCsalanPathName), 
                                DialogueCombatInfoList.beamAndCsalanCombatInfo));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.horse, new Dialogue(new string[]   { 
                                                            NPCNameList.playerNamePlaceHolder, 
                                                            NPCNameList.horse
                                                        }, 
                                Resources.Load<TextAsset>(DialogueNameList.beamAndCsalanPathName)));
        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.horse+1, new Dialogue(new string[]   { 
                                                            NPCNameList.playerNamePlaceHolder, 
                                                            NPCNameList.horse+1
                                                        }, 
                                Resources.Load<TextAsset>(DialogueNameList.beamAndCsalanPathName)));
        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.horse+2, new Dialogue(new string[]   { 
                                                            NPCNameList.playerNamePlaceHolder, 
                                                            NPCNameList.horse+2
                                                        }, 
                                Resources.Load<TextAsset>(DialogueNameList.beamAndCsalanPathName)));
        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.csalan, new Dialogue(new string[]   { 
                                                            NPCNameList.playerNamePlaceHolder, 
                                                            NPCNameList.csalan
                                                        }, 
                                Resources.Load<TextAsset>(DialogueNameList.beamAndCsalanPathName)));

        #endregion

        #region Manse-2f


            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, NPCNameList.gate, ancientPortcullisDialogue);


            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, NPCNameList.honorguard,
                                new SingleCharacterDialogue(NPCNameList.honorguard, 
                                 Resources.Load<TextAsset>(DialogueNameList.directorsBedroomGuardsPath)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.section2c, NPCNameList.chiefTabor,
                                new Dialogue(new string[]   {   
                                                                NPCNameList.playerNamePlaceHolder,
                                                                NPCNameList.chiefTabor,
                                                                NPCNameList.nandor,
                                                                NPCNameList.carter,
                                                                NPCNameList.slave,
                                                                NPCNameList.noBrand
                                                                 }, 
                                 Resources.Load<TextAsset>(DialogueNameList.chiefTaborManseSecondFloorPathName),
                                 DialogueCombatInfoList.taborCombatInfo));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.office, NPCNameList.director,
                                new Dialogue(new string[]   { 
                                                                NPCNameList.playerNamePlaceHolder,
                                                                NPCNameList.director,
                                                                NPCNameList.page,
                                                                NPCNameList.carter,
                                                                NPCNameList.nandor
                                                            }, 
                                 Resources.Load<TextAsset>(DialogueNameList.directorPathName),
                                 DialogueCombatInfoList.directorCombatInfo));

          addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.office, NPCNameList.director+1,
                                new Dialogue(new string[]   { 
                                                                NPCNameList.playerNamePlaceHolder,
                                                                NPCNameList.director+1,
                                                                NPCNameList.takacs+1,
                                                                NPCNameList.takacs+2
                                                            }, 
                                 Resources.Load<TextAsset>(DialogueNameList.prerevoltDirectorPathName)));

          addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.office, NPCNameList.page+1,
                                new Dialogue(new string[]   { 
                                                                NPCNameList.playerNamePlaceHolder,
                                                                NPCNameList.page+1,
                                                                NPCNameList.director+1,
                                                                NPCNameList.chiefTabor,
                                                                NPCNameList.weft,
                                                                NPCNameList.captainAdela
                                                            }, 
                                 Resources.Load<TextAsset>(DialogueNameList.prerevoltPagePathName)));


        // addDialogueToList(DialogueNameList.directorDefeatedConvoKey,
        //                  new Dialogue(new string[] { "", "Director", "Page", "Carter", "Nándor" }, new GameObject[5], Resources.Load<TextAsset>(DialogueNameList.directorDefeatedConvoKey)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.section3a, NPCNameList.ancientPortcullis,
                                new Dialogue(new string[]   { NPCNameList.ancientPortcullis }, 
                                 Resources.Load<TextAsset>(DialogueNameList.ancientPortcullisPath)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, NPCNameList.liftableGate, liftableGateDialogue);

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, NPCNameList.heavyBarrels,
                                new Dialogue(new string[]   { NPCNameList.heavyBarrels }, 
                                 Resources.Load<TextAsset>(DialogueNameList.heavyBarrelsPath)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, NPCNameList.heavyBarrels+1,
                                new Dialogue(new string[]   { NPCNameList.heavyBarrels+1 }, 
                                 Resources.Load<TextAsset>(DialogueNameList.heavyBarrelsPath)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.office, DialogueNameList.directorDefeatedConvoKey,
                                new Dialogue(new string[]   {   
                                                                NPCNameList.playerNamePlaceHolder,
                                                                NPCNameList.director,
                                                                NPCNameList.page,
                                                                NPCNameList.carter,
                                                                NPCNameList.nandor,
                                                                NPCNameList.thatch
                                                            }, 
                                 Resources.Load<TextAsset>(DialogueNameList.directorDefeatedPathName)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.office, NPCNameList.officeDoor,
                                new Dialogue(new string[]   { NPCNameList.officeDoor }, 
                                Resources.Load<TextAsset>(DialogueNameList.officeDoorPathName)));

        #endregion

        #region Pit

            addDialogueToList(ZoneKeyList.pit + LocationNameList.section2b, NPCNameList.cellDoor,
                                new Dialogue(new string[]   { NPCNameList.cellDoor }, 
                                 Resources.Load<TextAsset>(DialogueNameList.pitGatePathName)));

            addDialogueToList(ZoneKeyList.pit + LocationNameList.section2b, NPCNameList.brush,
                                new Dialogue(new string[]   { Constants.emptyString, NPCNameList.brush, NPCNameList.cellDoor }, 
                                 Resources.Load<TextAsset>(DialogueNameList.pitBrushPathName)));

            addDialogueToList(ZoneKeyList.pit + LocationNameList.section2c, NPCNameList.ancientPortcullis,
                                new Dialogue(new string[]   { NPCNameList.ancientPortcullis }, 
                                 Resources.Load<TextAsset>(DialogueNameList.ancientPortcullisPath)));

        #endregion//Assets/Resources/Dialogue/Manse/Pit/Pit Gate.ink

    }

    public static void initialize()
	{
		
	}

    public static void addDialogueToList(string areaName, string npcName, Dialogue dialogue)
	{
		addDialogueToList(areaName + npcName, dialogue);
	}

    public static void addDialogueToList(string key, Dialogue dialogue)
    {
        dialogueList.Add(key.Replace(" ", ""), dialogue);
    }
    
    public static void addPartyMemberDialogue(string areaName, string partyMemberName)
    {
        addDialogueToList(areaName, partyMemberName,
                    new Dialogue(new string[] { partyMemberName},
                    Resources.Load<TextAsset>(DialogueNameList.defaultPartyMemberDialoguePathName)));
    }

    public static Dialogue getDialogue(string areaName, string npcName)
    {
        string key = areaName + npcName;
        key = key.Replace(" ", "");

        Dialogue dialogue = getDialogue(key);

        if(dialogue == null)
        {
            key = npcName + areaName;
            key = key.Replace(" ", "");
            dialogue = getDialogue(key);

            if(dialogue == null)
            {
                Debug.LogError("Dialogue does not exist for areaName + npcName combo: " + key);
                return null;
            }
        }

        return dialogue;
    }

    public static Dialogue getDialogue(string key)
    {
        key = key.Replace(" ", "");
        if (!dialogueList.ContainsKey(key))
        {
            return null;
        }

        return dialogueList[key.Replace(" ", "")].clone();
    }
	
	public static string scrubNameOfEndNumbers(string name)
	{
        if(name == null)
        {
            return name;
        }
		
		int lowestCharDigitInInt = 48; //char dec val for 0
		int highestCharDigitInInt = 57; //char dec val for 9
		
		char[] nameChars = name.ToCharArray();
		
		for(int index = nameChars.Length-1; index >= 0; index--)
		{
			if(nameChars[index] >= lowestCharDigitInInt && nameChars[index] <= highestCharDigitInInt)
			{
				nameChars = name.Substring(0, index).ToCharArray();
			} else
            {
                break;
            }
		}
		
		return new string(nameChars);
	}
}
