using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INameSource
{
    public string getName();
}

public static class NameSourceExtensions
{
    public static bool hasGenericName(this INameSource source)
    {
        switch(DialogueList.scrubNameOfEndNumbers(source.getName()))
        {
            //inanimate object
            case NPCNameList.chest:
            case NPCNameList.shelf:
            case NPCNameList.crate:
            case NPCNameList.crates:
            case NPCNameList.barrels:
            case NPCNameList.barricade:
            case NPCNameList.statue:
            case NPCNameList.rubble:
            case NPCNameList.awkwardRubble:

            //occupation
            case NPCNameList.guard:
            case NPCNameList.branded:
            case NPCNameList.noBrand:
            case NPCNameList.slave:
            case NPCNameList.horse:
                return true;
        }

        return false;
    }
}

public interface IDialogueParticipant: INameSource
{
    public Dialogue getDialogue();
}

public class DialogueTrigger : MonoBehaviour, IDialogueParticipant
{

    public Dialogue dialogue;
    public SpeakAtStartScript speakAtStartScript;

    public PlaySFXLogic introAudioClipLogic;

    public NewAnimationManager animationManager;

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
        if(dialogue == null)
        {
            Debug.LogError("dialogue == null");
            return;
        }

        if(dialogue.inkJSON == null)
        {
            Debug.LogError("dialogue.inkJSON == null");
            return;
        }

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inDialogue);
        
        setFacing();

        playIntroAudioClip();

        DialogueManager.getInstance().startDialogue(dialogue);
    }

    public void playIntroAudioClip()
    {
        if(introAudioClipLogic != null)
        {
            introAudioClipLogic();
        }
    }

    public void setFacing()
    {
        if(animationManager == null || !animationManager.changesFacing)
        {
            return;
        }

        animationManager.characterFacing.currentFacing = State.playerFacing.getOpposingFacing();
    }

    public string getName()
    {
        return getDialogue().getName();
    }

    private void OnEnable()
    {
        setExtraSpacesActive(true);

        if(animationManager == null)
        {
            animationManager = GetComponent<NewAnimationManager>();
        }
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
