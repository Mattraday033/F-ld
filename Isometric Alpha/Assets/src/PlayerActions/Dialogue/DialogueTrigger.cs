using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

	public Dialogue dialogue;
	public SpeakAtStartScript speakAtStartScript;
	
	void Start()
	{
		if(speakAtStartScript != null)
		{
			speakAtStartScript.dialogueTrigger = this;
			speakAtStartScript.runScript();
        }
	}
	
	public Dialogue getDialogue()
    {
		return dialogue;
	}
	
	public void TriggerDialogue()
	{
		DialogueManager.getInstance().startDialogue(dialogue);
	}

}