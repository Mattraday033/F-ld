using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INameSource
{
    public string getName();
}

public interface IDialogueParticipant: INameSource
{
    public Dialogue getDialogue();
}

public class DialogueTrigger : MonoBehaviour, IDialogueParticipant
{

    public Dialogue dialogue;
    public SpeakAtStartScript speakAtStartScript;

    public GameObject[] extraSpaces;

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

    public string getName()
    {
        return getDialogue().getName();
    }

    private void OnEnable()
    {
        setExtraSpacesActive(true);
    }

    private void OnDisable()
    {
        setExtraSpacesActive(false);
    }

    private void setExtraSpacesActive(bool status)
    {
        foreach(GameObject extraSpace in extraSpaces)
        {
            if(extraSpace == null)
            {
                continue;
            }

            extraSpace.SetActive(status);
        }
    }

}
