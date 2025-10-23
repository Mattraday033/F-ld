using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class DialogueList
{

    public static Dictionary<string, Dialogue> dialogueList = new Dictionary<string, Dialogue>();


    static DialogueList()
    {
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

        addDialogueToList(AreaNameList.slaveShackOne, NPCNameList.seb,
                            new Dialogue(new string[] { NPCNameList.seb },
                            Resources.Load<TextAsset>(DialogueNameList.sebPath)));

        addDialogueToList(AreaNameList.slaveShackOne, NPCNameList.balint,
                            new Dialogue(new string[] { NPCNameList.balint },
                            Resources.Load<TextAsset>(DialogueNameList.balintPath)));

        #endregion
        #region Slave Shack 2

        addDialogueToList(AreaNameList.slaveShackTwo, NPCNameList.broglin,
                            new Dialogue(new string[] { NPCNameList.broglin, NPCNameList.garcha, NPCNameList.guardLaszlo, NPCNameList.guardLaszlo + 1, NPCNameList.garcha + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.introDialoguePath)));

        addDialogueToList(AreaNameList.slaveShackTwo, NPCNameList.garcha,
                            new Dialogue(new string[] { NPCNameList.garcha },
                            Resources.Load<TextAsset>(DialogueNameList.garchaPath)));

        #endregion
        #region Slave Shack 3

        addDialogueToList(AreaNameList.slaveShackThree, NPCNameList.janos,
                            new Dialogue(new string[] { NPCNameList.janos, NPCNameList.guardAndras, NPCNameList.guardAndras + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.janosPath),
                            DialogueCombatInfoList.andrasCombatInfo));

        addDialogueToList(AreaNameList.slaveShackThree, NPCNameList.guardAndras + 1,
                            new Dialogue(new string[] { NPCNameList.guardAndras + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.andrasPath)));

        addDialogueToList(AreaNameList.slaveShackThree, NPCNameList.guardAndras + 2,
                            new Dialogue(new string[] { NPCNameList.guardAndras + 2 },
                            Resources.Load<TextAsset>(DialogueNameList.andrasPath)));

        addDialogueToList(DialogueNameList.janosAfterKillingAndrasKey,
                            new Dialogue(new string[] { NPCNameList.janos },
                            Resources.Load<TextAsset>(DialogueNameList.janosAfterKillingAndrasPath)));

        #endregion
        #region Slave Shack 4

        addDialogueToList(AreaNameList.slaveShackFour, NPCNameList.kastor,
                            new Dialogue(new string[] { NPCNameList.kastor, NPCNameList.nandor, NPCNameList.carter, NPCNameList.guardMarcos },
                            Resources.Load<TextAsset>(DialogueNameList.kastorPlanPath)));

        addDialogueToList(AreaNameList.slaveShackFour, NPCNameList.guardMarcos,
                            new Dialogue(new string[] { NPCNameList.guardMarcos },
                            Resources.Load<TextAsset>(DialogueNameList.guardMarcosSS4Path)));

        addPartyMemberDialogue(AreaNameList.slaveShackFour, NPCNameList.nandor);
        addPartyMemberDialogue(AreaNameList.slaveShackFour, NPCNameList.carter);

        #endregion
        #region Slave Shack 5

        addDialogueToList(AreaNameList.slaveShackFive, NPCNameList.ervin,
                            new Dialogue(new string[] { NPCNameList.ervin },
                            Resources.Load<TextAsset>(DialogueNameList.ervinPath)));

        #endregion
        #region Slave Shack 6

        addDialogueToList(AreaNameList.slaveShackSix, NPCNameList.thatch,
                            new Dialogue(new string[] { NPCNameList.thatch, NPCNameList.rubble },
                            Resources.Load<TextAsset>(DialogueNameList.thatchPath)));

        addDialogueToList(AreaNameList.slaveShackSix, NPCNameList.slate,
                            new Dialogue(new string[] { NPCNameList.slate },
                            Resources.Load<TextAsset>(DialogueNameList.slatePath)));

        addDialogueToList(AreaNameList.slaveShackSix, NPCNameList.guardVazul,
                            new Dialogue(new string[] { "", NPCNameList.guardVazul, NPCNameList.slate, NPCNameList.thatch + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.vazulPath),
                            DialogueCombatInfoList.vazulCombatInfo));

        addDialogueToList(AreaNameList.slaveShackSix, NPCNameList.rubble,
                            new Dialogue(new string[] { NPCNameList.rubble },
                            Resources.Load<TextAsset>(DialogueNameList.immovableRubblePath)));

        #endregion
        #region NECamp

        addDialogueToList(AreaNameList.campNorthEast, NPCNameList.leafPile,
                            new Dialogue(new string[] { NPCNameList.leafPile },
                            Resources.Load<TextAsset>(DialogueNameList.leafPilePath)));

        #endregion
        #region MineEntranceCamp

        addDialogueToList(AreaNameList.campMineEntrance, NPCNameList.guardMuzsa,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa, NPCNameList.barricade, NPCNameList.guardMuzsa + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.muszaPath)));
        addDialogueToList(AreaNameList.campMineEntrance, NPCNameList.guardMuzsa+1,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa, NPCNameList.barricade, NPCNameList.guardMuzsa + 1 },
                            Resources.Load<TextAsset>(DialogueNameList.muszaPath)));
        addDialogueToList(AreaNameList.campMineEntrance, NPCNameList.guardMuzsa+2,
                            new Dialogue(new string[] { NPCNameList.guardMuzsa+2},
                            Resources.Load<TextAsset>(DialogueNameList.muszaPath)));

        #endregion
        #region ManseCamp

        addDialogueToList(AreaNameList.campManse, NPCNameList.imre,
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
