using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUponEntryTrigger : MonoBehaviour
{
	public string dialogueKey = "";

	public string[] promptDialogueFlags; //all flags must be true

    public string[] preventDialogueFlags; //any flag must be true

    public string[] listOfDeadNames;

	public DialogueActivationScript[] scripts;

    private void Awake()
    {
		DialogueList.initialize();
		
		if(PlayerInteractionScript.evaluateAnyScript(scripts))
		{
            Flags.setFlag(dialogueKey, true);

            State.dialogueUponSceneLoadKey = dialogueKey;
        }

        if(flagsAllowDialogue() && !mandatoryFigureIsDead() && !Flags.getFlag(dialogueKey))
		{
			Flags.setFlag(dialogueKey, true);
			
			State.dialogueUponSceneLoadKey = dialogueKey;
		}
    }
	
	private bool flagsAllowDialogue()
	{
		return promptRequirementsMet() && !preventionRequirementsMet();

    }

	private bool promptRequirementsMet()
	{
		if(promptDialogueFlags == null || promptDialogueFlags.Length < 1)
		{
            return true;
        }

		foreach(string flag in promptDialogueFlags)
		{
			if (!Flags.getFlag(flag))
			{
				return false;
			}
		}

		return true;
	}

	private bool preventionRequirementsMet()
	{
        if (preventDialogueFlags == null || preventDialogueFlags.Length < 1) 
        {
            return false;
        }

        foreach (string flag in preventDialogueFlags)
        {
            if (Flags.getFlag(flag))
            {
                return true;
            }
        }

        return false;
	}


    private bool mandatoryFigureIsDead()
	{
		foreach(string deadName in listOfDeadNames)
		{
			 if(DeathFlagManager.isDead(deadName))
			 {
				 return true;
			 }
		}
		
		return false;
	}
}
