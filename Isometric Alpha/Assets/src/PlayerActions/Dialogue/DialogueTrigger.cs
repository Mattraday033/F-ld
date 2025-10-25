using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueParticipant
{
    public string getMainNPCName();

    public Dialogue getDialogue();
}
public class DialogueTrigger : MonoBehaviour, IDialogueParticipant
{

    public Dialogue dialogue;
    public SpeakAtStartScript speakAtStartScript;

    public virtual void Start()
    {
        if (speakAtStartScript != null)
        {
            speakAtStartScript.dialogueTrigger = this;
            speakAtStartScript.runScript();
        }
    }

    public virtual Dialogue getDialogue()
    {
        return dialogue;
    }

    public virtual void triggerDialogue()
    {
        DialogueManager.getInstance().startDialogue(dialogue);
    }

    public string getMainNPCName()
    {
        return getDialogue().getMainNPCName();
    }

}
