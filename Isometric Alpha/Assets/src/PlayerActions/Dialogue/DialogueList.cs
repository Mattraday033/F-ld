using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class DialogueList
{
    private const string dialogueResourcesPathName = "Dialogue/";

    public static Dictionary<string, Dialogue> dialogueList = new Dictionary<string, Dialogue>();


    static DialogueList()
    {
        #region Dialogues not attached to NPC's
        addDialogueToList(DialogueNameList.nandorAfterKillingGuardsMineLvl3Key,
                         new Dialogue(new string[] { "", "Nándor", "Carter", "Guard Márcos", "Guard Pázmán", "Guard Réka" }, new GameObject[6], Resources.Load<TextAsset>(DialogueNameList.nandorAfterKillingGuardsMineLvl3Key)));

        addDialogueToList(DialogueNameList.slavesAfterKillingOverseerCampNEKey,
                         new Dialogue(new string[] { "", "Nándor", "Carter", "Garcha", "Janos", "Clay", "Slave 1", "Slave 2", "Slave 3", "Slave 4", "The Crowd", "AfterOverseerParent" }, new GameObject[12], Resources.Load<TextAsset>(DialogueNameList.slavesAfterKillingOverseerCampNEKey)));

        addDialogueToList(DialogueNameList.kendeUponEnteringKitchensKey,
                         new Dialogue(new string[] { "", "Kende", "Imre 1", "Imre 2", "Pan", "Guard", "Slave" }, new GameObject[7], Resources.Load<TextAsset>(DialogueNameList.kendeUponEnteringKitchensKey), new NPCCombatInfo(new EnemyPackInfo[]{     EnemyPackInfoList.halfSlavesNoGuardFight,
                                                                                                                                                                                                                                        EnemyPackInfoList.halfSlavesFight,
                                                                                                                                                                                                                                        EnemyPackInfoList.fullSlavesNoGuardFight,
		/*Dont delete this white space*/																																																EnemyPackInfoList.fullSlavesFight},
                                                                                                                                                                                                                     new DeadNameList[]{new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
                                                                                                                                                                                                                                        new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
                                                                                                                                                                                                                                        new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre}),
                                                                                                                                                                                                                                        new DeadNameList(new string[]{NPCNameList.kende, NPCNameList.pan, NPCNameList.imre})})));

        addDialogueToList(DialogueNameList.taborManse2F2BKey,
                         new Dialogue(new string[] { "", "Chief Tabor" }, new GameObject[2], Resources.Load<TextAsset>(DialogueNameList.taborManse2F2BKey), new NPCCombatInfo(new EnemyPackInfo[] { EnemyPackInfoList.taborFight },
                                                                                                                                                               new DeadNameList[] { new DeadNameList(new string[] { NPCNameList.chiefTabor }) })));

        addDialogueToList(DialogueNameList.directorDefeatedConvoKey,
                         new Dialogue(new string[] { "", "Director", "Page", "Carter", "Nándor" }, new GameObject[5], Resources.Load<TextAsset>(DialogueNameList.directorDefeatedConvoKey)));


        addDialogueToList(DialogueNameList.guardPunishmentConvoKey,
                         new Dialogue(new string[] { "", "Nándor", "Carter", "Kastor", "Janos", "Broglin", "Garcha", "Slave 1", "Slave 2", "Slave 3", "The Crowd", "Chief Tabor", "Guard Márcos", "Guard András", "Guard Réka", "Guard Pázmán", "Ervin", "Clay" }, new GameObject[18], Resources.Load<TextAsset>(DialogueNameList.guardPunishmentConvoKey)));


        addDialogueToList(DialogueNameList.afterKillingAndrasConvoKey, new Dialogue(new string[] { "", "Janos" }, new GameObject[2], Resources.Load<TextAsset>(DialogueNameList.afterKillingAndrasConvoKey)));

        addDialogueToList(DialogueNameList.vazulKey, new Dialogue(new string[] { "", "Thatch 1", "Slate", "Thatch 1" }, new GameObject[4], Resources.Load<TextAsset>(DialogueNameList.vazulKey)));

        addDialogueToList(DialogueNameList.taborAfterClayFightKey, new Dialogue(new string[] { "", "Chief Tabor" }, new GameObject[2], Resources.Load<TextAsset>(DialogueNameList.taborAfterClayFightKey), new TextAsset[] { Resources.Load<TextAsset>(DialogueNameList.chiefTaborPunishmentDialogueKey) }));

        #endregion

        #region Interactables

        addDialogueToList(NPCNameList.vaultableBarrels,
                            new Dialogue(new string[] { NPCNameList.vaultableBarrels },
                            Resources.Load<TextAsset>(dialogueResourcesPathName + PrefabNames.vaultableObject)));

        #endregion
   

        #region TestRoom

        addDialogueToList(AreaNameList.slaveShackTwo, NPCNameList.broglin,
                            new Dialogue(new string[] { NPCNameList.broglin, NPCNameList.garcha, NPCNameList.guardLaszlo, NPCNameList.guardLaszlo + 1, NPCNameList.garcha + 1 },
                            Resources.Load<TextAsset>(dialogueResourcesPathName + AreaNameList.slaveShackTwo + Path.DirectorySeparatorChar + DialogueNameList.introDialogueKey)));

        addDialogueToList(AreaNameList.slaveShackTwo, NPCNameList.garcha,
                            new Dialogue(new string[] { NPCNameList.garcha },
                            Resources.Load<TextAsset>(dialogueResourcesPathName + AreaNameList.slaveShackTwo + Path.DirectorySeparatorChar + NPCNameList.garcha)));

        #endregion

        #region NECamp

        addDialogueToList(AreaNameList.campNorthEast, NPCNameList.leafPile,
                            new Dialogue(new string[] { NPCNameList.leafPile},
                            Resources.Load<TextAsset>(dialogueResourcesPathName + AreaNameList.campNorthEast + Path.DirectorySeparatorChar + NPCNameList.leafPile.Replace(" ", ""))));

        #endregion
    }

    public static void initialize()
	{
		
	}

    public static void addDialogueToList(string key, Dialogue dialogue)
    {
        dialogueList.Add(key.Replace(" ", ""), dialogue);
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
