using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class DialogueList
{

    private readonly static Dialogue wallPatchDialogue = new Dialogue(new string[] { NPCNameList.wallPatch },
                                                            Resources.Load<TextAsset>(DialogueNameList.wallPatchPath));

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

        addDialogueToList(DialogueNameList.vazulPath, new Dialogue(new string[] { "", NPCNameList.thatch + 1, NPCNameList.slate, NPCNameList.thatch + 1 }, Resources.Load<TextAsset>(DialogueNameList.vazulPath)));

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

        addDialogueToList(LocationNameList.slaveShackThree, NPCNameList.guardAndras + 2,
                            new Dialogue(new string[] { NPCNameList.guardAndras + 2 },
                            Resources.Load<TextAsset>(DialogueNameList.andrasPath)));

        addDialogueToList(DialogueNameList.janosAfterKillingAndrasKey,
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
        addDialogueToList(LocationNameList.stockhouse, NPCNameList.crate+1,
                            new Dialogue(new string[] { NPCNameList.crate+1 },
                            Resources.Load<TextAsset>(DialogueNameList.dudCratePath)));
        addDialogueToList(LocationNameList.stockhouse, NPCNameList.crate+2,
                            new Dialogue(new string[] { NPCNameList.crate+2 },
                            Resources.Load<TextAsset>(DialogueNameList.dudCratePath)));

        addDialogueToList(LocationNameList.stockhouse, NPCNameList.barrels,
                            new Dialogue(new string[] { NPCNameList.barrels },
                            Resources.Load<TextAsset>(DialogueNameList.barrelsWithNuggetPath)));
        #endregion
        #region Stables

        addDialogueToList(LocationNameList.stables, NPCNameList.wallPatch, wallPatchDialogue);

        #endregion
        #region Temple

        addDialogueToList(LocationNameList.temple, NPCNameList.wallPatch, wallPatchDialogue);

        #endregion
        #region Mess Hall

        addDialogueToList(LocationNameList.messHall, NPCNameList.kende,
                            new Dialogue(new string[] { NPCNameList.kende},
                            Resources.Load<TextAsset>(DialogueNameList.kendePath)));
        #endregion

        #region NECamp

        addDialogueToList(LocationNameList.campNorthEast, NPCNameList.leafPile,
                            new Dialogue(new string[] { NPCNameList.leafPile },
                            Resources.Load<TextAsset>(DialogueNameList.leafPilePath)));

        #endregion
        #region CenterCamp

        addDialogueToList(LocationNameList.campCenter, NPCNameList.wallPatch, wallPatchDialogue);

        #endregion
        #region SECamp

        addDialogueToList(LocationNameList.campSouthEast, NPCNameList.wallPatch, wallPatchDialogue);

        #endregion
        #region MineEntranceCamp

        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa, NPCNameList.barricade, NPCNameList.guardMuzsa + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.muszaPath)));
        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa+1,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa+1, NPCNameList.barricade, NPCNameList.guardMuzsa + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.muszaPath)));
        addDialogueToList(LocationNameList.campMineEntrance, NPCNameList.guardMuzsa+2,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa+2},
                            Resources.Load<TextAsset>(DialogueNameList.muszaPath)));

        #endregion
        #region ManseCamp

        addDialogueToList(LocationNameList.campManse, NPCNameList.imre,
                            new Dialogue(new string[] { NPCNameList.imre },
                            Resources.Load<TextAsset>(DialogueNameList.imrePath),
                            DialogueCombatInfoList.imreCombatInfo));

        #endregion
    }

    public static void initialize()
	{
		
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

    public static void addDialogueToList(string areaName, string npcName, Dialogue dialogue)
	{
		addDialogueToList(areaName + npcName, dialogue);
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
                Debug.LogError("Dialogue does not exist for areaName + npcName combo: " + areaName + "/" + npcName);
                return null;
            }
        }

        return dialogue;
    }

    public static Dialogue getDialogue(string key)
    {
        key = key.Replace(" ", "");
        if(!dialogueList.ContainsKey(key))
        {
            return null;
        }

        return  dialogueList[key.Replace(" ", "")].clone();
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
