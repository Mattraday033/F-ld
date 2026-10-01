using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INameSource
{
    //shown to the player, so it never carries indexes or underscores
    public string displayName { get; }

    //used for dictionary look ups, so it may carry indexes and underscores
    public string uniqueName { get; }
}

public static class NameSourceExtensions
{
    //strips the index, trailing separators and underscores a uniqueName can carry
    public static string toNPCName(string uniqueName)
    {
        if(string.IsNullOrEmpty(uniqueName))
        {
            return Constants.emptyString;
        }

        return DialogueList.scrubNameOfEndNumbers(uniqueName).TrimEnd('-', '_', ' ').Replace('_', ' ');
    }

    //turns an enum name such as MattockRack into Mattock Rack
    public static string splitCamelCase(string camelCase)
    {
        return System.Text.RegularExpressions.Regex.Replace(camelCase, "(?<=[a-z])(?=[A-Z])", " ");
    }

    public static bool hasGenericName(this INameSource source)
    {
        switch(source.displayName)
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
    public Dialogue dialogue { get; }
}

public interface IDialogueSource
{
    public Dialogue dialogue { get; }
}

public class DialogueTrigger : MonoBehaviour, IDialogueParticipant
{

    //set by the spawn details, which tell NPCs sharing a name apart by their index
    private string _UniqueName = "";
    public string uniqueName
    {
        get
        {
            return _UniqueName;
        }
        set
        {
            _UniqueName = value;
            setNPCName();
        }
    }

    private string _NPCName = "";
    public string displayName { get { return _NPCName; } }

    private IDialogueSource _DialogueSource;
    public IDialogueSource dialogueSource
    {
        set
        {
           _DialogueSource = value;
           setNPCName();
        }
    }

    //the dialogue's speaker name wins over the uniqueName when there is one
    private void setNPCName()
    {
        Dialogue currentDialogue = dialogue;

        if(currentDialogue == null)
        {
            _NPCName = NameSourceExtensions.toNPCName(_UniqueName);
        } else
        {
            _NPCName = NameSourceExtensions.toNPCName(currentDialogue.getName());
        }
    }
    public Dialogue dialogue { get { 
                                        if(_DialogueSource != null)
                                        {
                                            return _DialogueSource.dialogue;
                                        } else
                                        {
                                            return null;
                                        }
                                    }
                             }
    public SpeakAtStartScript speakAtStartScript;

    public PlaySFXLogic introAudioClipLogic;

    public NewAnimationManager animationManager;

    // public GameObject[] extraSpaces;

    public virtual void Start()
    {
        if (speakAtStartScript != null)
        {
            speakAtStartScript.dialogueTrigger = this;
            speakAtStartScript.runScript();
        }
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

        PlayerStateManager.setCurrentActivity(CurrentActivity.InDialogue);
        
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

    private void OnEnable()
    {
        EventList.SetActiveByNameChannel.Invoke(ActivationCategory.Dialogue, uniqueName, true);

        if(animationManager == null)
        {
            animationManager = GetComponent<NewAnimationManager>();
        }
    }

    private void OnDisable()
    {
        EventList.SetActiveByNameChannel.Invoke(ActivationCategory.Dialogue, uniqueName, false);
    }

}
