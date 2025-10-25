using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerLink : DialogueTrigger
{

    public DialogueTrigger linkedDialogue;

    public override void Start()
    {

    }
	
	public override Dialogue getDialogue()
    {
		return linkedDialogue.dialogue;
	}
	
	public override void triggerDialogue()
	{
        linkedDialogue.triggerDialogue();
	}

}