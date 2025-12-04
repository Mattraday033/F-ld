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
        // addDialogueToList(DialogueNameList.nandorAfterKillingGuardsMineLvl3Key,
        //                  new Dialogue(new string[] { "", "Nándor", "Carter", "Guard Márcos", "Guard Pázmán", "Guard Réka" }, new GameObject[6], Resources.Load<TextAsset>(DialogueNameList.nandorAfterKillingGuardsMineLvl3Key)));

        // addDialogueToList(DialogueNameList.slavesAfterKillingOverseerCampNEKey,
        //                  new Dialogue(new string[] { "", "Nándor", "Carter", "Garcha", "Janos", "Clay", "Slave 1", "Slave 2", "Slave 3", "Slave 4", "The Crowd", "AfterOverseerParent" }, new GameObject[12], Resources.Load<TextAsset>(DialogueNameList.slavesAfterKillingOverseerCampNEKey)));

        // addDialogueToList(DialogueNameList.kendeUponEnteringKitchensKey,
        //                  new Dialogue(new string[] { "", "Kende", "Imre 1", "Imre 2", "Pan", "Guard", "Slave" }, new GameObject[7], Resources.Load<TextAsset>(DialogueNameList.kendeUponEnteringKitchensKey), new NPCCombatInfo(new EnemyPackInfo[]{     EnemyPackInfoList.halfSlavesNoGuardFight,
        //                                                                                                                                                                                                                                 EnemyPackInfoList.halfSlavesFight,
        //                                                                                                                                                                                                                                 EnemyPackInfoList.fullSlavesNoGuardFight,
        // /*Dont delete this white space*/																																																EnemyPackInfoList.fullSlavesFight},
        //                                                                                                                                                                                                              new DeadNameList[]{new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
        //                                                                                                                                                                                                                                 new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
        //                                                                                                                                                                                                                                 new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
        //                                                                                                                                                                                                                                 new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre})})));

        // addDialogueToList(DialogueNameList.taborManse2F2BKey,
        //                  new Dialogue(new string[] { "", "Chief Tabor" }, new GameObject[2], Resources.Load<TextAsset>(DialogueNameList.taborManse2F2BKey), new NPCCombatInfo(new EnemyPackInfo[] { EnemyPackInfoList.taborFight },
        //                                                                                                                                                        new DeadNameList[] { new DeadNameList(new string[] { NPCNameList.chiefTabor }) })));

        // addDialogueToList(DialogueNameList.directorDefeatedConvoKey,
        //                  new Dialogue(new string[] { "", "Director", "Page", "Carter", "Nándor" }, new GameObject[5], Resources.Load<TextAsset>(DialogueNameList.directorDefeatedConvoKey)));


        // addDialogueToList(DialogueNameList.guardPunishmentConvoKey,
        //                  new Dialogue(new string[] { "", "Nándor", "Carter", "Kastor", "Janos", "Broglin", "Garcha", "Slave 1", "Slave 2", "Slave 3", "The Crowd", "Chief Tabor", "Guard Márcos", "Guard András", "Guard Réka", "Guard Pázmán", "Ervin", "Clay" }, new GameObject[18], Resources.Load<TextAsset>(DialogueNameList.guardPunishmentConvoKey)));


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

        addDialogueToList(LocationNameList.slaveShackTwo, NPCNameList.broglin,
                            new Dialogue(new string[] { NPCNameList.broglin, NPCNameList.garcha, NPCNameList.guardLaszlo, NPCNameList.guardLaszlo + 1, NPCNameList.garcha + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.introDialoguePath)));

        addDialogueToList(LocationNameList.slaveShackTwo, NPCNameList.garcha,
                            new Dialogue(new string[] { NPCNameList.garcha },
                            Resources.Load<TextAsset>(DialogueNameList.garchaPath)));

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
                            new Dialogue(new string[] { NPCNameList.kastor, NPCNameList.nandor, NPCNameList.carter, NPCNameList.guardMarcos },
                            Resources.Load<TextAsset>(DialogueNameList.kastorPlanPath)));

        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.guardMarcos,
                            new Dialogue(new string[] { NPCNameList.guardMarcos },
                            Resources.Load<TextAsset>(DialogueNameList.guardMarcosSS4Path)));

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

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.fallenBeam,
                            new Dialogue(new string[] { NPCNameList.fallenBeam },
                            Resources.Load<TextAsset>(DialogueNameList.fallenBeamPath)));

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

        addDialogueToList(LocationNameList.messHall, NPCNameList.kende,
                            new Dialogue(new string[] { NPCNameList.kende },
                            Resources.Load<TextAsset>(DialogueNameList.kendePath)));
        #endregion

        #region NECamp

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.leafPile,
                            new Dialogue(new string[] { NPCNameList.leafPile },
                            Resources.Load<TextAsset>(DialogueNameList.leafPilePath)));

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
                                                        NPCNameList.branded},
                            Resources.Load<TextAsset>(DialogueNameList.taborPath)));

        // Dialogue slavesWatchingTaborDialogue = new Dialogue(new string[] { NPCNameList.branded }, Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.feher,
                            new Dialogue(new string[] { NPCNameList.feher },
                            Resources.Load<TextAsset>(DialogueNameList.feherPath)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.branded,
                            new Dialogue(new string[] { NPCNameList.branded },
                            Resources.Load<TextAsset>(DialogueNameList.slavesWatchingTaborPath)));
        // addDialogueToList(LocationNameList.campCenter, NPCNameList.branded+1, slavesWatchingTaborDialogue);
        // addDialogueToList(LocationNameList.campCenter, NPCNameList.branded+2, slavesWatchingTaborDialogue);

        addDialogueToList(LocationNameList.campCenter, NPCNameList.wallPatch, wallPatchDialogue);

        #endregion
        #region SECamp

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.wallPatch, wallPatchDialogue);
        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.statue,
                            new Dialogue(new string[] { "", NPCNameList.statue},
                            Resources.Load<TextAsset>(DialogueNameList.directorStatuePathPath)));

        #endregion
        #region MineEntranceCamp

        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa, NPCNameList.barricade, NPCNameList.guardMuzsa + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.muszaPath)));
        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa + 1,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa + 1, NPCNameList.barricade, NPCNameList.guardMuzsa + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.muszaPath)));


        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.uros,
                            new Dialogue(new string[] { NPCNameList.uros },
                            Resources.Load<TextAsset>(DialogueNameList.urosPath)));

        #endregion
        #region ManseCamp

        addDialogueToList(LocationNameList.campManse, NPCNameList.imre,
                            new Dialogue(new string[] { NPCNameList.imre },
                            Resources.Load<TextAsset>(DialogueNameList.imrePath),
                            DialogueCombatInfoList.imreCombatInfo));

        addDialogueToList(LocationNameList.campManse, NPCNameList.manseFrontDoor,
                            new Dialogue(new string[] { NPCNameList.manseFrontDoor },
                            Resources.Load<TextAsset>(DialogueNameList.manseFrontDoorPath)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.manseServiceEntrance + 1,
                            new Dialogue(new string[] { NPCNameList.manseServiceEntrance },
                            Resources.Load<TextAsset>(DialogueNameList.manseServiceEntrancePath)));

        #endregion

        #region MineLvl_1

        addDialogueToList(LocationNameList.mineLvl1 + LocationNameList.section1b, NPCNameList.awkwardRubble, awkwardRubbleDialogue);

        addDialogueToList(LocationNameList.mineLvl1 + LocationNameList.section1c, NPCNameList.liftableGate, liftableGateDialogue);

        #endregion

        #region MineLvl_2

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section1a, NPCNameList.suspiciousWall,
                                                                                  new Dialogue(new string[] { NPCNameList.suspiciousWall },
                                                                                  Resources.Load<TextAsset>(DialogueNameList.suspiciousWallPathML2 + 1)));
        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section7b, NPCNameList.suspiciousWall,
                                                                                  new Dialogue(new string[] { NPCNameList.suspiciousWall },
                                                                                  Resources.Load<TextAsset>(DialogueNameList.suspiciousWallPathML2 + 2)));

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section2a, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section2a, NPCNameList.controlPanel, new Dialogue(new string[] { NPCNameList.controlPanel},
                                                                                  Resources.Load<TextAsset>(DialogueNameList.controlPanelPath)));

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardPazman,
                                                            new Dialogue(new string[] { "", NPCNameList.guardPazman},
                                                             Resources.Load<TextAsset>(DialogueNameList.pazmanML3CampPath)));                                        

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardVirag, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardVirag},
                                                             Resources.Load<TextAsset>(DialogueNameList.viragML3CampPath)));

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardReka, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardReka},
                                                             Resources.Load<TextAsset>(DialogueNameList.rekaML3CampPath)));

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section2a, NPCNameList.overseerGaspar, 
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

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section2b, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section2b, NPCNameList.mineArmoryGate + 1,
                                                                                  new Dialogue(new string[] { NPCNameList.mineArmoryGate },
                                                                                  Resources.Load<TextAsset>(DialogueNameList.mineArmoryGatePath)));

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section3a, NPCNameList.awkwardRubble, awkwardRubbleDialogue);
        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section3a, NPCNameList.awkwardRubble + 1, awkwardRubbleDialogue);

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section3b, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section3b, NPCNameList.liftableGate + 1, liftableGateDialogue);

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section6, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis + 1, ancientPortcullisDialogue);
        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis + 2, ancientPortcullisDialogue);
        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis + 3, ancientPortcullisDialogue);

        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section7b, NPCNameList.awkwardRubble, awkwardRubbleDialogue);
        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section7b, NPCNameList.ancientPortcullis + 1, ancientPortcullisDialogue);
        addDialogueToList(LocationNameList.mineLvl2 + LocationNameList.section7b, NPCNameList.liftableGate + 2, liftableGateDialogue);

        #endregion

        #region MineLvl_3

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section1b, NPCNameList.suspiciousWall,
                                                                                  new Dialogue(new string[] { NPCNameList.suspiciousWall },
                                                                                  Resources.Load<TextAsset>(DialogueNameList.suspiciousWallPathML3)));

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section2b, NPCNameList.liftableGate, liftableGateDialogue);
        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section2b, NPCNameList.ancientPortcullis+1, ancientPortcullisDialogue);

        #region MineLvl_3-3b

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.liftableGate, 
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

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardPazman, //unreachable, for pazman behind barricade
                                                            new Dialogue(new string[] { "", NPCNameList.guardPazman},
                                                             Resources.Load<TextAsset>(DialogueNameList.pazmanML3CampPath)));                                        
        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardPazman+1, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardPazman+1},
                                                             Resources.Load<TextAsset>(DialogueNameList.pazmanML3CampPath)));
                                                            
        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardVirag, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardVirag},
                                                             Resources.Load<TextAsset>(DialogueNameList.viragML3CampPath)));

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardReka, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardReka},
                                                             Resources.Load<TextAsset>(DialogueNameList.rekaML3CampPath)));

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.overseerGaspar, 
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

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section3b, NPCNameList.barricade, 
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

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section4b, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section4b, NPCNameList.ancientPortcullis+1, ancientPortcullisDialogue);

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section5, NPCNameList.liftableRubble, liftableRubbleDialogue);

        #region MineLvl_3-Miner Camp

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.barricade, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.barricade,
                                                                NPCNameList.carter,
                                                                NPCNameList.nandor,
                                                                NPCNameList.guardMarcos
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3MinerBarricadePath)));

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.carter
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3CarterPath)));

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter+1, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.carter+1
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3CarterPath)));

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.guardMarcos, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.guardMarcos
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3MarcosPath)));

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.nandor, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.nandor
                                                            },
                                                             Resources.Load<TextAsset>(DialogueNameList.ml3NandorPath)));

        #endregion

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section6a, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section6a, NPCNameList.unstablePillar, unstablePillarDialogue);

        #region MineLvl_3-7

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section7, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section7, NPCNameList.unstablePillar, unstablePillarDialogue);

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section7, NPCNameList.rubble, 
                                                                                new Dialogue(new string[] { 
                                                                                                            NPCNameList.rubble,
                                                                                                            NPCNameList.rubble+1,
                                                                                                            NPCNameList.guardPazman,
                                                                                                            NPCNameList.guardReka,
                                                                                                            NPCNameList.guardVirag,
                                                                                                            NPCNameList.overseerGaspar,
                                                                                                            NPCNameList.carter,
                                                                                                            NPCNameList.nandor,
                                                                                                            NPCNameList.guardMarcos,
                                                                                                            NPCNameList.guardMarcos+1
                                                                                                        },
                                                                                Resources.Load<TextAsset>(DialogueNameList.pocketRubblePathML3)));

        addDialogueToList(LocationNameList.mineLvl3 + LocationNameList.section7, NPCNameList.rubble+1, 
                                                                                new Dialogue(new string[] { 
                                                                                                            NPCNameList.rubble+1,
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
                                                                                Resources.Load<TextAsset>(DialogueNameList.pocketRubblePathML3)));
        #endregion

        #endregion

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
                    Resources.Load<TextAsset>(DialogueNameList.partyMemberFolderPathName + partyMemberName)));
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
	
    public static Dialogue getVaultableObjectDialogue(string name)
    {
        return new Dialogue(new string[] { Constants.emptyString, name }, Resources.Load<TextAsset>(DialogueNameList.vaultableObjectPath));
    }

	public static string scrubNameOfEndNumbers(string name)
	{
		
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
