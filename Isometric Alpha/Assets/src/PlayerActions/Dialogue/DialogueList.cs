using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class DialogueList
{


    private readonly static Dialogue wallPatchDialogue = new Dialogue(new string[] { NPCNameList.wallPatch },
                                                            InkAssetList.getInkJSON(DialogueKey.WallPatch));

    private readonly static Dialogue awkwardRubbleDialogue = new Dialogue(new string[] { NPCNameList.awkwardRubble },
                                                             InkAssetList.getInkJSON(DialogueKey.AwkwardRubble));
    private readonly static Dialogue ancientPortcullisDialogue = new Dialogue(new string[] { NPCNameList.ancientPortcullis},
                                                             InkAssetList.getInkJSON(DialogueKey.AncientPortcullis));

    private readonly static Dialogue liftableGateDialogue = new Dialogue(new string[] { "", NPCNameList.liftableGate + 1},
                                                             InkAssetList.getInkJSON(DialogueKey.LiftableGate));

    private readonly static StoryStatRequirementVariableSource unstablePillarStrengthRequirement = new StoryStatRequirementVariableSource(StoryVariableNameList.strReqVariableName, Constants.sizeThree);
    private readonly static Dialogue unstablePillarDialogue = new Dialogue(new string[] { "", NPCNameList.unstablePillar},
                                                             InkAssetList.getInkJSON(DialogueKey.UnstablePillar),
                                                             unstablePillarStrengthRequirement);

    private readonly static Dialogue liftableRubbleDialogue = new Dialogue(new string[] { NPCNameList.liftableRubble },
                            InkAssetList.getInkJSON(DialogueKey.LiftableRubble));

    public static Dictionary<string, Dialogue> dialogueList;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeDialogueList()
    {
        InkAssetList.init();

        dialogueList = new Dictionary<string, Dialogue>();

        #region Interactables

        addDialogueToList(NPCNameList.vaultableBarrels,
                            new Dialogue(new string[] { NPCNameList.vaultableBarrels },
                            InkAssetList.getInkJSON(DialogueKey.VaultableObject)));

        #endregion

        #region Slave Shack 1

        addDialogueToList(LocationNameList.slaveShackOne, NPCNameList.seb,
                            new Dialogue(new string[] { NPCNameList.seb },
                            InkAssetList.getInkJSON(DialogueKey.Seb)));

        addDialogueToList(LocationNameList.slaveShackOne, NPCNameList.balint,
                            new Dialogue(new string[] { NPCNameList.balint },
                            InkAssetList.getInkJSON(DialogueKey.Balint)));

        #endregion
        #region Slave Shack 2

        addDialogueToList(LocationNameList.slaveShackTwo, NPCNameList.brush,
                            new Dialogue(new string[] { NPCNameList.brush, NPCNameList.géza, NPCNameList.guardLaszlo, NPCNameList.guardLaszlo + 1, NPCNameList.géza + 1, NPCNameList.géza + 2},
                            InkAssetList.getInkJSON(DialogueKey.IntroDialogue)));

        addDialogueToList(LocationNameList.slaveShackTwo, NPCNameList.géza,
                            new Dialogue(new string[] { NPCNameList.géza },
                            InkAssetList.getInkJSON(DialogueKey._2SlaveShack_Géza)));

        #endregion
        #region Slave Shack 3

        addDialogueToList(LocationNameList.slaveShackThree, NPCNameList.janos,
                            new Dialogue(new string[] { NPCNameList.janos, NPCNameList.guardAndras, NPCNameList.guardAndras + 1 },
                            InkAssetList.getInkJSON(DialogueKey._3SlaveShack_Janos),
                            DialogueCombatInfoList.andrasCombatInfo));

        addDialogueToList(LocationNameList.slaveShackThree, NPCNameList.guardAndras + 1,
                            new Dialogue(new string[] { NPCNameList.guardAndras + 1 },
                            InkAssetList.getInkJSON(DialogueKey._3SlaveShack_Andras)));

        addDialogueToList(LocationNameList.slaveShackThree, DialogueNameList.janosAfterKillingAndrasKey,
                            new Dialogue(new string[] { NPCNameList.janos },
                            InkAssetList.getInkJSON(DialogueKey.JanosAfterKillingAndras)));

        #endregion
        #region Slave Shack 4

        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.kastor,
                            new Dialogue(new string[] { 
                                                        NPCNameList.kastor,
                                                        NPCNameList.nandor, 
                                                        NPCNameList.carter, 
                                                        NPCNameList.guardMarcos, 
                                                        NPCNameList.guardMarcos+1,
                                                        NPCNameList.thatch,
                                                        NPCNameList.kastor+1,
                                                        NPCNameList.rubble,
                                                        NPCNameList.dibber,
                                                        NPCNameList.dibber+1,
                                                        NPCNameList.weft
                                                    },
                            InkAssetList.getInkJSON(DialogueKey.KastorPlan)));

        Dialogue kastorSkillTutorialDialogue = new Dialogue(new string[] { 
                                                        NPCNameList.kastor,
                                                        NPCNameList.kastor+1,
                                                        NPCNameList.kastor+2,
                                                        NPCNameList.kastor+3,
                                                        NPCNameList.kastor+4
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.KastorSkillTutorial));

        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.kastor+1, kastorSkillTutorialDialogue);
        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.kastor+2, kastorSkillTutorialDialogue);
        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.kastor+3, kastorSkillTutorialDialogue);
        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.kastor+4, kastorSkillTutorialDialogue);

        Dialogue dibberDialogue = new Dialogue(new string[] { 
                                                        NPCNameList.dibber,
                                                        NPCNameList.dibber+1,
                                                        NPCNameList.kastor,
                                                        NPCNameList.kastor+4,
                                                        NPCNameList.kastor+5,
                                                        NPCNameList.thatch
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.Dibber));

        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.dibber, dibberDialogue);
        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.dibber+1, dibberDialogue);

        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.guardMarcos,
                            new Dialogue(new string[] { NPCNameList.guardMarcos },
                            InkAssetList.getInkJSON(DialogueKey.GuardMarcos)));

        addDialogueToList(LocationNameList.slaveShackFour, NPCNameList.guardMarcos+1,
                            new Dialogue(new string[] { NPCNameList.guardMarcos+1 },
                            InkAssetList.getInkJSON(DialogueKey.GuardMarcos)));

        addPartyMemberDialogue(LocationNameList.slaveShackFour, NPCNameList.nandor);
        addPartyMemberDialogue(LocationNameList.slaveShackFour, NPCNameList.carter);

        #endregion
        #region Slave Shack 5

        addDialogueToList(LocationNameList.slaveShackFive, NPCNameList.ervin,
                            new Dialogue(new string[] { 
                                                        NPCNameList.ervin
                                                        // ,NPCNameList.thatch
                                                        },
                            InkAssetList.getInkJSON(DialogueKey._5SlaveShack_Ervin)));

        addDialogueToList(LocationNameList.slaveShackFive, NPCNameList.wallPatch, wallPatchDialogue);

        #endregion
        #region Slave Shack 6

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.thatch,
                            new Dialogue(new string[] { NPCNameList.thatch, NPCNameList.rubble },
                            InkAssetList.getInkJSON(DialogueKey._6SlaveShack_Thatch)));

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.slate,
                            new Dialogue(new string[] { NPCNameList.slate },
                            InkAssetList.getInkJSON(DialogueKey.Slate)));

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.guardVazul,
                            new Dialogue(new string[] { "", NPCNameList.guardVazul, NPCNameList.slate, NPCNameList.thatch + 1 },
                            InkAssetList.getInkJSON(DialogueKey.Vazul),
                            DialogueCombatInfoList.vazulCombatInfo));

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.rubble,
                            new Dialogue(new string[] { NPCNameList.rubble },
                            InkAssetList.getInkJSON(DialogueKey.ImmovableRubble)));

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.wallPatch, wallPatchDialogue);

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.liftableRubble, liftableRubbleDialogue);

        addDialogueToList(LocationNameList.slaveShackSix, NPCNameList.awkwardRubble,
                            new Dialogue(new string[] { NPCNameList.awkwardRubble },
                            InkAssetList.getInkJSON(DialogueKey.FallenBeam)));

        #endregion
        #region Slave Shack 7

        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.slave,
                            new SingleCharacterDialogue(NPCNameList.slave,
                            InkAssetList.getInkJSON(DialogueKey.SlavesWatchingTabor)));

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
                            InkAssetList.getInkJSON(DialogueKey.Dezso),
                            DialogueCombatInfoList.dezsoHostageFight));

        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.loam,
                            new Dialogue(new string[]{ NPCNameList.loam},
                            InkAssetList.getInkJSON(DialogueKey.Loam)));

        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.guard+1,
                            new SingleCharacterDialogue(NPCNameList.guard+1,
                            InkAssetList.getInkJSON(DialogueKey.SlavesWatchingTabor)));
        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.guard+2,
                            new SingleCharacterDialogue(NPCNameList.guard+2,
                            InkAssetList.getInkJSON(DialogueKey.SlavesWatchingTabor)));
        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.guard+3,
                            new SingleCharacterDialogue(NPCNameList.guard+3,
                            InkAssetList.getInkJSON(DialogueKey.SlavesWatchingTabor)));

        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.guard+4,
                            new SingleCharacterDialogue(NPCNameList.guard+4,
                            InkAssetList.getInkJSON(DialogueKey.GuardsAfterHostages)));

        addDialogueToList(LocationNameList.slaveShackSeven, NPCNameList.weft,
                            new Dialogue(new string[]{
                                                        NPCNameList.playerNamePlaceHolder, 
                                                        NPCNameList.weft, 
                                                        NPCNameList.weft+1
                                                    },
                            InkAssetList.getInkJSON(DialogueKey.WeftAfterHostages)));
        #endregion
        #region Slave Shack 8

        addDialogueToList(LocationNameList.slaveShackEight, NPCNameList.weft,
                            new Dialogue(new string[] { NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.weft,
                                                        NPCNameList.overseer
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.Weft)));

        #endregion

        #region Guard Shack
        addDialogueToList(LocationNameList.guardShack, NPCNameList.guardLaszlo,
                            new Dialogue(new string[] { NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.guardLaszlo,
                                                        NPCNameList.weft
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.GuardLászló)));
        #endregion

        #region Guard House NE

        addDialogueToList(LocationNameList.guardHouseNorthEast, NPCNameList.barracksGate,
                            new SingleCharacterDialogue(NPCNameList.barracksGate,
                            InkAssetList.getInkJSON(DialogueKey.BarracksGate)));

        #endregion

        #region Guard House SW

        addDialogueToList(LocationNameList.guardHouseSouthWest, NPCNameList.barracksGate,
                            new SingleCharacterDialogue(NPCNameList.barracksGate,
                            InkAssetList.getInkJSON(DialogueKey.BarracksGate)));


        addDialogueToList(LocationNameList.guardHouseSouthWest, NPCNameList.guard,
                            new SingleCharacterDialogue(NPCNameList.guard,
                            InkAssetList.getInkJSON(DialogueKey.BarracksGuard)));

        #endregion

        #region Stockhouse

        addDialogueToList(LocationNameList.stockhouse, NPCNameList.uros,
                            new Dialogue(new string[] { NPCNameList.uros, NPCNameList.quartermasterEmese },
                            InkAssetList.getInkJSON(DialogueKey.Uros)));

        addDialogueToList(LocationNameList.stockhouse, NPCNameList.quartermasterEmese,
                            new Dialogue(new string[] { NPCNameList.quartermasterEmese, NPCNameList.uros },
                            InkAssetList.getInkJSON(DialogueKey.Emese)));

        addDialogueToList(LocationNameList.stockhouse, NPCNameList.crate,
                            new Dialogue(new string[] { NPCNameList.crate },
                            InkAssetList.getInkJSON(DialogueKey.DudCrates)));
        addDialogueToList(LocationNameList.stockhouse, NPCNameList.crate + 1,
                            new Dialogue(new string[] { NPCNameList.crate + 1 },
                            InkAssetList.getInkJSON(DialogueKey.DudCrates)));

        addDialogueToList(LocationNameList.stockhouse, NPCNameList.barrels,
                            new Dialogue(new string[] { NPCNameList.barrels },
                            InkAssetList.getInkJSON(DialogueKey.BarrelsWithNugget)));
        addDialogueToList(LocationNameList.stockhouse, NPCNameList.barrels + 1,
                            new Dialogue(new string[] { NPCNameList.barrels + 1},
                            InkAssetList.getInkJSON(DialogueKey.DudBarrels)));

        #endregion
        #region Stables

        addDialogueToList(LocationNameList.stables, NPCNameList.wallPatch, wallPatchDialogue);

        addDialogueToList(LocationNameList.stables, NPCNameList.beam,
                            new Dialogue(new string[] { NPCNameList.beam },
                            InkAssetList.getInkJSON(DialogueKey.Beam)));

        addDialogueToList(LocationNameList.stables, NPCNameList.horse,
                            new Dialogue(new string[] { NPCNameList.horse },
                            InkAssetList.getInkJSON(DialogueKey.Horse)));
        addDialogueToList(LocationNameList.stables, NPCNameList.horse + 1,
                            new Dialogue(new string[] { NPCNameList.horse + 1 },
                            InkAssetList.getInkJSON(DialogueKey.Horse)));
        addDialogueToList(LocationNameList.stables, NPCNameList.horse + 2,
                            new Dialogue(new string[] { NPCNameList.horse + 2 },
                            InkAssetList.getInkJSON(DialogueKey.Horse)));

        #endregion
        #region Temple

        addDialogueToList(LocationNameList.temple, NPCNameList.wallPatch, wallPatchDialogue);

        addDialogueToList(LocationNameList.temple, NPCNameList.priestRikard,
                            new SingleCharacterDialogue(NPCNameList.priestRikard,
                            InkAssetList.getInkJSON(DialogueKey.PriestRikard)));

        #endregion
        #region Mess Hall

        addDialogueToList(LocationNameList.messHall, NPCNameList.noBrand+1,
                            new Dialogue(new string[] { NPCNameList.noBrand+1 },
                            InkAssetList.getInkJSON(DialogueKey.SlavesWatchingTabor)));

        addDialogueToList(LocationNameList.messHall, NPCNameList.kende,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.kende,
                                                        NPCNameList.weft
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.Kende)));
        #endregion

        #region Body Pile

        addDialogueToList(LocationNameList.bodyPile, NPCNameList.body+1,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor,
                                                        NPCNameList.weft
                                                     },
                            InkAssetList.getInkJSON(DialogueKey.ThiefBody)));

        addDialogueToList(LocationNameList.bodyPile, DialogueNameList.afterTakacsFightKey,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.firstPrefix + NPCNameList.guard,
                                                        NPCNameList.secondPrefix + NPCNameList.guard,
                                                        NPCNameList.ladder,
                                                        NPCNameList.weft+1,
                                                        NPCNameList.thatch,
                                                        NPCNameList.gaspar,
                                                        NPCNameList.gaspar + NPCNameList.shadowSuffix,
                                                        NPCNameList.gaspar+1,
                                                        NPCNameList.protagUnderstudy,
                                                        NPCNameList.weft+2,
                                                        NPCNameList.thatch+1,
                                                        NPCNameList.rubble

                                                     },
                            InkAssetList.getInkJSON(DialogueKey.AfterTakacsFight)));

        #endregion

        #region NECamp

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.leafPile,
                            new Dialogue(new string[] { NPCNameList.leafPile },
                            InkAssetList.getInkJSON(DialogueKey.LeafPile)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.overseer,
                            new Dialogue(new string[] { NPCNameList.overseer },
                            InkAssetList.getInkJSON(DialogueKey.Overseer)));

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
                            InkAssetList.getInkJSON(DialogueKey.slavesAfterKillingOverseerCampNE)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.temple+1,
                         new Dialogue(new string[] { NPCNameList.temple+1 }, 
                            InkAssetList.getInkJSON(DialogueKey.Slave5)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.slave+6,
                         new Dialogue(new string[] { NPCNameList.slave+6 }, 
                            InkAssetList.getInkJSON(DialogueKey.Slave6)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.slave+7,
                         new Dialogue(new string[] { NPCNameList.slave+7 }, 
                            InkAssetList.getInkJSON(DialogueKey.Slave7)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.slave+8,
                         new Dialogue(new string[] { NPCNameList.slave+8 }, 
                            InkAssetList.getInkJSON(DialogueKey.Slave8)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.clay+1,
                         new Dialogue(new string[] { NPCNameList.clay+1 }, 
                            InkAssetList.getInkJSON(DialogueKey.Slave9)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.woundedSlave,
                         new Dialogue(new string[] { NPCNameList.woundedSlave }, 
                            InkAssetList.getInkJSON(DialogueKey.WoundedSlave)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.woundedSlave+1,
                         new Dialogue(new string[] { NPCNameList.woundedSlave+1 }, 
                            InkAssetList.getInkJSON(DialogueKey.WoundedSlave1)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.woundedSlave+2,
                         new Dialogue(new string[] { NPCNameList.woundedSlave+2 }, 
                            InkAssetList.getInkJSON(DialogueKey.WoundedSlave2)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.guardMarcos,
                         new Dialogue(new string[] { NPCNameList.guardMarcos }, 
                            InkAssetList.getInkJSON(DialogueKey.GuardMarcos)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.uros,
                         new Dialogue(new string[] { NPCNameList.uros }, 
                            InkAssetList.getInkJSON(DialogueKey.UrosDuringRevolution)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.kastor,
                         new Dialogue(new string[] { NPCNameList.kastor }, 
                            InkAssetList.getInkJSON(DialogueKey.KastorDuringRevolution)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.géza+1,
                         new Dialogue(new string[] { NPCNameList.géza+1 }, 
                            InkAssetList.getInkJSON(DialogueKey.GézaDuringRevolution)));

        addDialogueToList(LocationNameList.campNorthEast, MonsterNameList.brandedConscript,
                        new SingleCharacterDialogue(MonsterNameList.brandedConscript,
                        InkAssetList.getInkJSON(DialogueKey.SlavesWatchingTabor)));

        addDialogueToList(LocationNameList.campNorthEast, MonsterNameList.spearman,
                        new SingleCharacterDialogue(MonsterNameList.spearman,
                        InkAssetList.getInkJSON(DialogueKey.SlavesWatchingTabor)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.guard+2,
                            new SingleCharacterDialogue(NPCNameList.guard+2,
                            InkAssetList.getInkJSON(DialogueKey.SituationGuard)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.guard+3,
                            new SingleCharacterDialogue(NPCNameList.guard+3,
                            InkAssetList.getInkJSON(DialogueKey.SituationGuard)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.guard+4,
                            new SingleCharacterDialogue(NPCNameList.guard+4,
                            InkAssetList.getInkJSON(DialogueKey.SituationGuardBlocker)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.guard+1,
                            new SingleCharacterDialogue(NPCNameList.guard+1,
                            InkAssetList.getInkJSON(DialogueKey.SituationGuard)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.honorguard+1,
                            new SingleCharacterDialogue(NPCNameList.honorguard+1,
                            InkAssetList.getInkJSON(DialogueKey.SituationGuard)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.chiefTabor,
                            new Dialogue(new string[]{ 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor,
                                                        NPCNameList.captainAdela,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard+2
                                                    },
                            InkAssetList.getInkJSON(DialogueKey.NECamp_Tabor)));

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.captainAdela,
                            new Dialogue(new string[]{ NPCNameList.captainAdela},
                            InkAssetList.getInkJSON(DialogueKey.Adela)));
        #endregion
        #region CenterCamp

        addDialogueToList(LocationNameList.campCenter, NPCNameList.csalan,
                            new Dialogue(new string[] { NPCNameList.csalan },
                            InkAssetList.getInkJSON(DialogueKey.Csalan)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.guard + 1,
                            new Dialogue(new string[] { NPCNameList.guard + 1 },
                            InkAssetList.getInkJSON(DialogueKey.GuardWatchingTabor)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.temple,
                            new Dialogue(new string[] { NPCNameList.temple },
                            InkAssetList.getInkJSON(DialogueKey.Temple)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.chiefTabor,
                            new Dialogue(new string[] { NPCNameList.chiefTabor,
                                                        NPCNameList.feher,
                                                        NPCNameList.branded,
                                                        NPCNameList.branded+1,
                                                        NPCNameList.branded+2,
                                                        NPCNameList.weft},
                            InkAssetList.getInkJSON(DialogueKey.CenterCamp_Tabor)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.feher,
                            new Dialogue(new string[] { NPCNameList.feher },
                            InkAssetList.getInkJSON(DialogueKey.Feher)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.branded,
                            new Dialogue(new string[] { NPCNameList.branded },
                            InkAssetList.getInkJSON(DialogueKey.SlavesWatchingTabor)));
        addDialogueToList(LocationNameList.campCenter, NPCNameList.branded+1,
                            new Dialogue(new string[] { NPCNameList.branded+1 },
                            InkAssetList.getInkJSON(DialogueKey.SlavesWatchingTabor)));
        addDialogueToList(LocationNameList.campCenter, NPCNameList.branded+2,
                            new Dialogue(new string[] { NPCNameList.branded+2 },
                            InkAssetList.getInkJSON(DialogueKey.SlavesWatchingTabor)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.campGate, 
                            new Dialogue(new string[] { NPCNameList.campGate },
                            InkAssetList.getInkJSON(DialogueKey.CampGate)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.wallPatch, wallPatchDialogue);

        addDialogueToList(LocationNameList.campCenter, NPCNameList.barricadeGuards+1,
                                new SingleCharacterDialogue(NPCNameList.barricadeGuards+1,
                                InkAssetList.getInkJSON(DialogueKey.FirstBarricadeGuards),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey1)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.barricade+1,
                                new SingleCharacterDialogue(NPCNameList.barricade+1,
                                InkAssetList.getInkJSON(DialogueKey.FirstBarricadeGuards),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey1)));

        addDialogueToList(LocationNameList.campCenter, NPCNameList.page,
                                new Dialogue(new string[] { 
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.page,
                                                            NPCNameList.carter,
                                                            NPCNameList.carter
                                                          },
                            InkAssetList.getInkJSON(DialogueKey.Page)));

        #endregion
        #region SECamp

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.wallPatch, wallPatchDialogue);
        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.statue,
                            new SingleCharacterDialogue(NPCNameList.statue,
                            InkAssetList.getInkJSON(DialogueKey.DirectorStatue)));
                            
        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.toppledStatue,
                            new SingleCharacterDialogue(NPCNameList.toppledStatue,
                            InkAssetList.getInkJSON(DialogueKey.BrokenDirectorStatue)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+7,
                            new SingleCharacterDialogue(NPCNameList.slave,
                            InkAssetList.getInkJSON(DialogueKey.MessHallSlave1)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+8,
                            new SingleCharacterDialogue(NPCNameList.slave,
                            InkAssetList.getInkJSON(DialogueKey.MessHallSlave2)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.guard,
                            new SingleCharacterDialogue(NPCNameList.guard,
                            InkAssetList.getInkJSON(DialogueKey.MessHallGuard)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.guardHenrik,
                            new SingleCharacterDialogue(NPCNameList.guardHenrik,
                            InkAssetList.getInkJSON(DialogueKey.StatueGuard)));


        #region Guard Punishment Scene

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave,
                            new SingleCharacterDialogue(NPCNameList.slave,
                            InkAssetList.getInkJSON(DialogueKey.CrowdSlave1)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+1,
                            new SingleCharacterDialogue(NPCNameList.slave,
                            InkAssetList.getInkJSON(DialogueKey.CrowdSlave1)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+2,
                            new SingleCharacterDialogue(NPCNameList.slave,
                            InkAssetList.getInkJSON(DialogueKey.CrowdSlave2)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+3,
                            new SingleCharacterDialogue(NPCNameList.slave ,
                            InkAssetList.getInkJSON(DialogueKey.CrowdSlave3)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+4,
                            new SingleCharacterDialogue(NPCNameList.slave ,
                            InkAssetList.getInkJSON(DialogueKey.CrowdSlave3)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+5,
                            new SingleCharacterDialogue(NPCNameList.slave ,
                            InkAssetList.getInkJSON(DialogueKey.CrowdSlave3)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.slave+6,
                            new SingleCharacterDialogue(NPCNameList.slave ,
                            InkAssetList.getInkJSON(DialogueKey.CrowdSlave3)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.crowd,
                            new SingleCharacterDialogue(NPCNameList.slave ,
                            InkAssetList.getInkJSON(DialogueKey.CrowdSlave3)));

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
                            InkAssetList.getInkJSON(DialogueKey.Kastor),
                            new TextAsset[]{InkAssetList.getInkJSON(DialogueKey.SECamp_Nandor)}));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.brush,
                            new SingleCharacterDialogue(NPCNameList.brush,
                            InkAssetList.getInkJSON(DialogueKey.SECamp_Brush)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.géza,
                            new SingleCharacterDialogue(NPCNameList.géza,
                            InkAssetList.getInkJSON(DialogueKey.SECamp_Géza)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.ervin,
                            new SingleCharacterDialogue(NPCNameList.ervin,
                            InkAssetList.getInkJSON(DialogueKey.SECamp_Ervin)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.janos,
                            new SingleCharacterDialogue(NPCNameList.janos,
                            InkAssetList.getInkJSON(DialogueKey.SECamp_Janos)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.thatch,
                            new SingleCharacterDialogue(NPCNameList.thatch,
                            InkAssetList.getInkJSON(DialogueKey.SECamp_Thatch)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.carter,
                            new SingleCharacterDialogue(NPCNameList.carter,
                            InkAssetList.getInkJSON(DialogueKey.SECamp_Carter)));

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
                            InkAssetList.getInkJSON(DialogueKey.GuardPunishmentStartConvo),
                            new TextAsset[]{InkAssetList.getInkJSON(DialogueKey.SECamp_Nandor)}));


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
                            InkAssetList.getInkJSON(DialogueKey.SECamp_Nandor)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.marcos,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.marcos,
                                                            NPCNameList.crowd
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.Marcos)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.andras,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.andras,
                                                            NPCNameList.crowd,
                                                            NPCNameList.janos
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.SECamp_Andras)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.reka,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.reka,
                                                            NPCNameList.crowd
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.Reka)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.pazman,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.pazman,
                                                            NPCNameList.crowd,
                                                            NPCNameList.ervin,
                                                            NPCNameList.ervin+1
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.Pazman)));

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.chiefTabor,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.chiefTabor,
                                                            NPCNameList.crowd,
                                                            NPCNameList.clay
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.SECamp_ChiefTabor),
                            DialogueCombatInfoList.clayFightForTaborCombatInfo));

        addDialogueToList(LocationNameList.campSouthEast, DialogueNameList.taborAfterClayFightKey,
                            new Dialogue(new string[]   {  
                                                            NPCNameList.playerNamePlaceHolder,
                                                            NPCNameList.chiefTabor
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.TaborAfterClayFight),
                            new TextAsset[]{InkAssetList.getInkJSON(DialogueKey.SECamp_ChiefTabor)}));             

        #endregion

        #endregion
        #region MineEntranceCamp

        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guard,
                            new SingleCharacterDialogue(NPCNameList.guard,
                            InkAssetList.getInkJSON(DialogueKey.GenericGuard)));
        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guard+1,
                            new SingleCharacterDialogue(NPCNameList.guard+1,
                            InkAssetList.getInkJSON(DialogueKey.GenericGuard)));

        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa, NPCNameList.barricade, NPCNameList.guardMuzsa + 1 },
                            InkAssetList.getInkJSON(DialogueKey.GuardMuzsa),
                            DialogueCombatInfoList.muzsaCombatInfo));
        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa + 1,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa + 1, NPCNameList.barricade, NPCNameList.guardMuzsa + 1 },
                            InkAssetList.getInkJSON(DialogueKey.GuardMuzsa)));

        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.barracksGate,
                            new SingleCharacterDialogue(NPCNameList.barracksGate,
                            InkAssetList.getInkJSON(DialogueKey.BarracksGate)));

        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.uros,
                            new Dialogue(new string[] { NPCNameList.uros },
                            InkAssetList.getInkJSON(DialogueKey.Uros)));

        #endregion
        #region ManseCamp

        addDialogueToList(LocationNameList.campManse, NPCNameList.imre,
                            new Dialogue(new string[] { NPCNameList.imre },
                            InkAssetList.getInkJSON(DialogueKey.Imre),
                            DialogueCombatInfoList.imreCombatInfo));

        addDialogueToList(LocationNameList.campManse, NPCNameList.imre+1,
                            new Dialogue(new string[] { NPCNameList.imre+1 },
                            InkAssetList.getInkJSON(DialogueKey.Imre)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.manseFrontDoor,
                            new Dialogue(new string[] { NPCNameList.manseFrontDoor },
                            InkAssetList.getInkJSON(DialogueKey.ManseFrontDoor)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.manseServiceEntrance + 1,
                            new Dialogue(new string[] { NPCNameList.manseServiceEntrance },
                            InkAssetList.getInkJSON(DialogueKey.ManseServiceEntrance)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.barricadeGuards+2,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+2,
                                                        NPCNameList.guardAndras+2
                                                      },
                                InkAssetList.getInkJSON(DialogueKey.SecondBarricadeGuards),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey2)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.barricade+2,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+2,
                                                        NPCNameList.guardAndras+2
                                                      },
                                InkAssetList.getInkJSON(DialogueKey.SecondBarricadeGuards),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey2)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.barricadeGuards+3,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+3,
                                                        NPCNameList.guardAndras+3
                                                      },
                                InkAssetList.getInkJSON(DialogueKey.SecondBarricadeGuards),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey3)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.barricade+3,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+3,
                                                        NPCNameList.guardAndras+3
                                                      },
                                InkAssetList.getInkJSON(DialogueKey.SecondBarricadeGuards),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey3)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.barracksGate+2,
                            new SingleCharacterDialogue(NPCNameList.barracksGate+2,
                            InkAssetList.getInkJSON(DialogueKey.BarracksGate)));

        addDialogueToList(LocationNameList.campManse, NPCNameList.chiefTabor,
                            new Dialogue( new string[]{ 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor,
                                                        NPCNameList.weft
                                                        },
                            InkAssetList.getInkJSON(DialogueKey.TaborWeftHut)));

        #endregion
        #region NWCamp

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.guard,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor+1
                                                      },
                            InkAssetList.getInkJSON(DialogueKey.NWCampGuard),
                            new TextAsset[]
                            {
                                InkAssetList.getInkJSON(DialogueKey.TaborEndOfTutorial)
                            }));

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.guard+2,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.guard+2,
                                                        NPCNameList.chiefTabor+6,
                                                        NPCNameList.chiefTabor+7,
                                                        NPCNameList.director,
                                                        NPCNameList.captainAdela,
                                                        NPCNameList.crowd,
                                                        NPCNameList.weft+2,
                                                        NPCNameList.thatch,
                                                        NPCNameList.takacs,
                                                        NPCNameList.hangman,
                                                        NPCNameList.gaspar
                                                      },
                            InkAssetList.getInkJSON(DialogueKey.NWCampGuard),
                            DialogueCombatInfoList.takacsPuppetCombatInfo));

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor+1
                                                      },
                                InkAssetList.getInkJSON(DialogueKey.TaborEndOfTutorial)));

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor+1,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor+1,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor+2
                                                      },
                                InkAssetList.getInkJSON(DialogueKey.TaborIntimidateTutorial)));

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor+2,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor+2,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor+3
                                                      },
                                InkAssetList.getInkJSON(DialogueKey.TaborCunningTutorial)));

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor+3,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor+3,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor+4
                                                      },
                                InkAssetList.getInkJSON(DialogueKey.TaborLeadershipTutorial)));

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor+4,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor+4,
                                                        NPCNameList.weft,
                                                        NPCNameList.guard,
                                                        NPCNameList.chiefTabor
                                                      },
                                InkAssetList.getInkJSON(DialogueKey.TaborObservationTutorial)));


        // addDialogueToList(LocationNameList.campNorthWest, NPCNameList.awkwardRubble, awkwardRubbleDialogue);
        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.wallPatch, wallPatchDialogue);

        addDialogueToList(LocationNameList.campNorthWest, NPCNameList.chiefTabor+5,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.chiefTabor+5,
                                                        NPCNameList.weft+1,
                                                        NPCNameList.guard+1
                                                      },
                            InkAssetList.getInkJSON(DialogueKey.TaborAfterBodyPile)));

        #endregion

        #region MineLvl_1

        // addDialogueToList(ZoneKeyList.mineLvl1 + LocationNameList.section1b, NPCNameList.awkwardRubble, awkwardRubbleDialogue);

        addDialogueToList(ZoneKeyList.mineLvl1 + LocationNameList.section1c, NPCNameList.liftableGate, liftableGateDialogue);

        #endregion

        #region MineLvl_2

        // TODO: no ink story was ever written for the mine's secret walls - these three entries
        // pointed at Mine/MineLvl_2/Suspicious Wall1, ...Wall2 and Mine/MineLvl_3/Suspicious Wall,
        // none of which exist, so they have always resolved to nothing. Author the stories and
        // swap in their keys, or delete these entries and let OOCSpawnDetails fall back to
        // DialogueKey.SuspiciousWall like every other secret door.
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section1a, NPCNameList.suspiciousWall,
                                                                                  new Dialogue(new string[] { NPCNameList.suspiciousWall },
                                                                                  InkAssetList.getInkJSON(DialogueKey.NoDialogue)));
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7b, NPCNameList.suspiciousWall,
                                                                                  new Dialogue(new string[] { NPCNameList.suspiciousWall },
                                                                                  InkAssetList.getInkJSON(DialogueKey.NoDialogue)));

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.controlPanel, new Dialogue(new string[] { NPCNameList.controlPanel},
                                                                                  InkAssetList.getInkJSON(DialogueKey.ControlPanel)));

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardPazman,
                                                            new Dialogue(new string[] { "", NPCNameList.guardPazman},
                                                             InkAssetList.getInkJSON(DialogueKey.GuardPazman)));                                        

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardVirag, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardVirag},
                                                             InkAssetList.getInkJSON(DialogueKey.GuardVirag)));

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2a, NPCNameList.guardReka, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardReka},
                                                             InkAssetList.getInkJSON(DialogueKey.GuardReka)));

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
                                                             InkAssetList.getInkJSON(DialogueKey.GuardsCrate),
                                                             DialogueCombatInfoList.mineLvl3GuardsCombatInfo));

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2b, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section2b, NPCNameList.mineArmoryGate + 1,
                                                                                  new Dialogue(new string[] { NPCNameList.mineArmoryGate },
                                                                                  InkAssetList.getInkJSON(DialogueKey.MineArmoryGate)));

        // addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section3a, NPCNameList.awkwardRubble, awkwardRubbleDialogue);
        // addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section3a, NPCNameList.awkwardRubble + 1, awkwardRubbleDialogue);

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section3b, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section3b, NPCNameList.liftableGate + 1, liftableGateDialogue);

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section6, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);

        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis + 1, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis + 2, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7a, NPCNameList.ancientPortcullis + 3, ancientPortcullisDialogue);

        // addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7b, NPCNameList.awkwardRubble, awkwardRubbleDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7b, NPCNameList.ancientPortcullis + 1, ancientPortcullisDialogue);
        addDialogueToList(ZoneKeyList.mineLvl2 + LocationNameList.section7b, NPCNameList.liftableGate + 2, liftableGateDialogue);

        #endregion

        #region MineLvl_3

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section1b, NPCNameList.suspiciousWall,
                                                                                  new Dialogue(new string[] { NPCNameList.suspiciousWall },
                                                                                  InkAssetList.getInkJSON(DialogueKey.NoDialogue)));

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
                                                             InkAssetList.getInkJSON(DialogueKey.GuardLiftableGate),
                                                             DialogueCombatInfoList.mineLvl3GuardsCombatInfo, 
                                                             new TextAsset[]
                                                             {
                                                                 InkAssetList.getInkJSON(DialogueKey.GuardsCrate)
                                                             }));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardPazman, //unreachable, for pazman behind barricade
                                                            new Dialogue(new string[] { "", NPCNameList.guardPazman},
                                                             InkAssetList.getInkJSON(DialogueKey.GuardPazman)));                                        
        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardPazman+1, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardPazman+1},
                                                             InkAssetList.getInkJSON(DialogueKey.GuardPazman)));
                                                            
        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardVirag, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardVirag},
                                                             InkAssetList.getInkJSON(DialogueKey.GuardVirag)));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section3b, NPCNameList.guardReka, 
                                                            new Dialogue(new string[] { "", NPCNameList.guardReka},
                                                             InkAssetList.getInkJSON(DialogueKey.GuardReka)));

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
                                                             InkAssetList.getInkJSON(DialogueKey.GuardsCrate),
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
                                                             InkAssetList.getInkJSON(DialogueKey.GuardsCrate),
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
                                                             InkAssetList.getInkJSON(DialogueKey.MinersCrate),
                                                             new TextAsset[]{ 
                                                                                InkAssetList.getInkJSON(DialogueKey.GuardMarcos),
                                                                                InkAssetList.getInkJSON(DialogueKey.MinersCrate)
                                                                            }));

        // addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter, 
        //                                                     new Dialogue(new string[] 
        //                                                     { 
        //                                                         "", 
        //                                                         NPCNameList.carter, 
        //                                                         NPCNameList.nandor
        //                                                     },
        //                                                      InkAssetList.getInkJSON(DialogueKey.MineLvl_3_Carter)));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter+1, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.carter+1
                                                            },
                                                             InkAssetList.getInkJSON(DialogueKey.MineLvl_3_Carter)));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.guardMarcos, 
                                                            new Dialogue(new string[] 
                                                            { 
                                                                "", 
                                                                NPCNameList.guardMarcos
                                                            },
                                                             InkAssetList.getInkJSON(DialogueKey.GuardMarcos)));

        addPartyMemberDialogue(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.nandor);
        addPartyMemberDialogue(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.carter);

        // addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, NPCNameList.nandor, 
        //                                                     new Dialogue(new string[] 
        //                                                     { 
        //                                                         "", 
        //                                                         NPCNameList.nandor,
        //                                                         NPCNameList.carter
        //                                                     },
        //                                                      InkAssetList.getInkJSON(DialogueKey.MineLvl_3_Nandor)));

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
                                                                                                            NPCNameList.guardMarcos+1,
                                                                                                            NPCNameList.weft,
                                                                                                            NPCNameList.thatch
                                                                                                        },
                                                                                InkAssetList.getInkJSON(DialogueKey.Rubble),
                                                                                DialogueCombatInfoList.breachRubbleCombatInfo));

        addDialogueToList(ZoneKeyList.mineLvl3 + LocationNameList.section7, DialogueNameList.afterKillingGuardsMineLvl3Key, 
                                                                                new Dialogue(new string[] { 
                                                                                                            NPCNameList.nandor,
                                                                                                            NPCNameList.carter,
                                                                                                            NPCNameList.guardMarcos,
                                                                                                            NPCNameList.guardPazman,
                                                                                                            NPCNameList.guardReka,
                                                                                                            NPCNameList.guardVirag,
                                                                                                            NPCNameList.overseerGaspar,
                                                                                                            NPCNameList.weft+1,
                                                                                                            NPCNameList.thatch+1,
                                                                                                            NPCNameList.thatch+2
                                                                                                        },
                                                                                InkAssetList.getInkJSON(DialogueKey.AfterKillingGuardsMineLvl3)));

        #endregion

        #endregion

        #region Manse-1f

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, NPCNameList.barricadeGuards+4,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+4,
                                                        NPCNameList.guardAndras+4
                                                      },
                                InkAssetList.getInkJSON(DialogueKey.SecondBarricadeGuards),
                                DialogueCombatInfoList.barricadeGuardsCombatInfo,
                                new StoryFlagList(InkVariableNameList.defeatFlag, FlagNameList.barricadeGuardDefeatKey4)));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, NPCNameList.barricade+4,
                            new Dialogue(new string[] { 
                                                        NPCNameList.playerNamePlaceHolder,
                                                        NPCNameList.barricadeGuards+4,
                                                        NPCNameList.guardAndras+4
                                                      },
                                InkAssetList.getInkJSON(DialogueKey.SecondBarricadeGuards),
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
                                 InkAssetList.getInkJSON(DialogueKey.kendeUponEnteringKitchens), 
                                 DialogueCombatInfoList.kendeInKitchensCombatInfo));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.imre+1,
                            new Dialogue(new string[]   { NPCNameList.imre+1 }, 
                                InkAssetList.getInkJSON(DialogueKey.LoyalImre)));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, NPCNameList.pan,
                            new SingleCharacterDialogue(NPCNameList.pan, 
                            InkAssetList.getInkJSON(DialogueKey.Pan)));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, NPCNameList.ancientPortcullis,
                            new Dialogue(new string[]   { NPCNameList.ancientPortcullis }, 
                                InkAssetList.getInkJSON(DialogueKey.AncientPortcullis)));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, NPCNameList.ancientPortcullis,
                            new Dialogue(new string[]   { NPCNameList.ancientPortcullis }, 
                                InkAssetList.getInkJSON(DialogueKey.AncientPortcullis)));

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
                                InkAssetList.getInkJSON(DialogueKey.BeamAndCsalan), 
                                DialogueCombatInfoList.beamAndCsalanCombatInfo));

        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.horse, new Dialogue(new string[]   { 
                                                            NPCNameList.playerNamePlaceHolder, 
                                                            NPCNameList.horse
                                                        }, 
                                InkAssetList.getInkJSON(DialogueKey.BeamAndCsalan)));
        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.horse+1, new Dialogue(new string[]   { 
                                                            NPCNameList.playerNamePlaceHolder, 
                                                            NPCNameList.horse+1
                                                        }, 
                                InkAssetList.getInkJSON(DialogueKey.BeamAndCsalan)));
        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.horse+2, new Dialogue(new string[]   { 
                                                            NPCNameList.playerNamePlaceHolder, 
                                                            NPCNameList.horse+2
                                                        }, 
                                InkAssetList.getInkJSON(DialogueKey.BeamAndCsalan)));
        addDialogueToList(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, NPCNameList.csalan, new Dialogue(new string[]   { 
                                                            NPCNameList.playerNamePlaceHolder, 
                                                            NPCNameList.csalan
                                                        }, 
                                InkAssetList.getInkJSON(DialogueKey.BeamAndCsalan)));

        #endregion

        #region Manse-2f


            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, NPCNameList.gate, ancientPortcullisDialogue);


            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, NPCNameList.honorguard,
                                new SingleCharacterDialogue(NPCNameList.honorguard, 
                                 InkAssetList.getInkJSON(DialogueKey.Honorguard)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.section2c, NPCNameList.chiefTabor,
                                new Dialogue(new string[]   {   
                                                                NPCNameList.playerNamePlaceHolder,
                                                                NPCNameList.chiefTabor,
                                                                NPCNameList.nandor,
                                                                NPCNameList.carter,
                                                                NPCNameList.slave,
                                                                NPCNameList.noBrand
                                                                 }, 
                                 InkAssetList.getInkJSON(DialogueKey.Manse2F_ChiefTabor),
                                 DialogueCombatInfoList.taborCombatInfo));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.office, NPCNameList.director,
                                new Dialogue(new string[]   { 
                                                                NPCNameList.playerNamePlaceHolder,
                                                                NPCNameList.director,
                                                                NPCNameList.page,
                                                                NPCNameList.carter,
                                                                NPCNameList.nandor
                                                            }, 
                                 InkAssetList.getInkJSON(DialogueKey.Director),
                                 DialogueCombatInfoList.directorCombatInfo));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.office, NPCNameList.director+1,
                                new Dialogue(new string[]   { 
                                                                NPCNameList.playerNamePlaceHolder,
                                                                NPCNameList.director+1,
                                                                NPCNameList.takacs+1,
                                                                NPCNameList.takacs+2
                                                            }, 
                                 InkAssetList.getInkJSON(DialogueKey.DirectorPreRevolt)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.office, NPCNameList.page+1,
                                new Dialogue(new string[]   { 
                                                                NPCNameList.playerNamePlaceHolder,
                                                                NPCNameList.page+1,
                                                                NPCNameList.director+1,
                                                                NPCNameList.chiefTabor,
                                                                NPCNameList.weft,
                                                                NPCNameList.captainAdela,
                                                                NPCNameList.guard,
                                                                NPCNameList.guard+1,
                                                                NPCNameList.overseerGaspar,
                                                                NPCNameList.chiefTabor+1,
                                                                NPCNameList.nandor+1,
                                                                NPCNameList.carter+1,
                                                                NPCNameList.weft+1
                                                            }, 
                                 InkAssetList.getInkJSON(DialogueKey.PagePreRevolt)));


        // addDialogueToList(DialogueNameList.directorDefeatedConvoKey,
        //                  new Dialogue(new string[] { "", "Director", "Page", "Carter", "Nándor" }, new GameObject[5], InkAssetList.getInkJSON(DialogueKey.DirectorDefeatedConvo)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.section3a, NPCNameList.ancientPortcullis,
                                new Dialogue(new string[]   { NPCNameList.ancientPortcullis }, 
                                 InkAssetList.getInkJSON(DialogueKey.AncientPortcullis)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, NPCNameList.captainAdela, 
                                                            new SingleCharacterDialogue(NPCNameList.captainAdela,
                                                            InkAssetList.getInkJSON(DialogueKey.CaptainAdéla), 
                                                            npcCombatInfo: DialogueCombatInfoList.adelaCombatInfo));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, NPCNameList.liftableGate, liftableGateDialogue);

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, NPCNameList.heavyBarrels,
                                new Dialogue(new string[]   { NPCNameList.heavyBarrels }, 
                                 InkAssetList.getInkJSON(DialogueKey.HeavyBarrels)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, NPCNameList.heavyBarrels+1,
                                new Dialogue(new string[]   { NPCNameList.heavyBarrels+1 }, 
                                 InkAssetList.getInkJSON(DialogueKey.HeavyBarrels)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.office, DialogueNameList.directorDefeatedConvoKey,
                                new Dialogue(new string[]   {   
                                                                NPCNameList.playerNamePlaceHolder,
                                                                NPCNameList.director,
                                                                NPCNameList.page,
                                                                NPCNameList.carter,
                                                                NPCNameList.nandor,
                                                                NPCNameList.thatch
                                                            }, 
                                 InkAssetList.getInkJSON(DialogueKey.DirectorDefeatedConvo)));

            addDialogueToList(ZoneKeyList.manseSecondFloor + LocationNameList.office, NPCNameList.officeDoor,
                                new Dialogue(new string[]   { NPCNameList.officeDoor }, 
                                InkAssetList.getInkJSON(DialogueKey.OfficeDoor)));

        #endregion

        #region Pit

            addDialogueToList(ZoneKeyList.pit + LocationNameList.section2b, NPCNameList.cellDoor,
                                new Dialogue(new string[]   { NPCNameList.cellDoor }, 
                                 InkAssetList.getInkJSON(DialogueKey.PitGate)));

            addDialogueToList(ZoneKeyList.pit + LocationNameList.section2b, NPCNameList.brush,
                                new Dialogue(new string[]   { Constants.emptyString, NPCNameList.brush, NPCNameList.cellDoor }, 
                                 InkAssetList.getInkJSON(DialogueKey.Pit_Brush)));

            addDialogueToList(ZoneKeyList.pit + LocationNameList.section2c, NPCNameList.ancientPortcullis,
                                new Dialogue(new string[]   { NPCNameList.ancientPortcullis }, 
                                 InkAssetList.getInkJSON(DialogueKey.AncientPortcullis)));

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
                    InkAssetList.getInkJSON(DialogueKey.DefaultPartyMemberDialogue)));
    }

    public static Dialogue getDialogue(string areaName, string npcName)
    {
        if(getDialogueBasedOffNPCName(npcName, out Dialogue nameSpecificDialogue))
        {
            return nameSpecificDialogue;
        }

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

    private static bool getDialogueBasedOffNPCName(string npcName, out Dialogue nameSpecificDialogue)
    {
        switch(scrubNameOfEndNumbers(npcName))
        {
            case NPCNameList.awkwardRubble:
                nameSpecificDialogue = awkwardRubbleDialogue;
                return true;
            default:
                nameSpecificDialogue = null;
                return false;
        }
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
