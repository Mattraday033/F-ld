using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExtraSpawnBehaviour
{
    public void addBehaviour(GameObject gameObject);

} 

public class AnimationManagerSpawnBehaviour : IExtraSpawnBehaviour
{
    
    private IAppearanceSource appearanceSource;
    private Facing facing;
    private CharacterAnimationType animationType;

    public AnimationManagerSpawnBehaviour(IAppearanceSource appearanceSource, Facing facing, CharacterAnimationType animationType = CharacterAnimationType.None)
    {
        this.appearanceSource = appearanceSource;
        this.facing = facing;
        this.animationType = animationType;
    }

    public void addBehaviour(GameObject gameObject)
    {
        NewAnimationManager animationManager = gameObject.AddComponent<NewAnimationManager>();

        animationManager.appearanceSource = appearanceSource;
        animationManager.characterFacing.currentFacing = facing;

        if(animationType != CharacterAnimationType.None)
        {
            animationManager.playAnimation(animationType);
        } else
        {
            animationManager.handleMovementAnimation();
        }
    }
}

public class ContainerSpawnBehaviour: IExtraSpawnBehaviour
{
    private int index;
    private QuestStepActivationScript script;
    private string secretDoorFlag;
    private ChestType type;
    // private string chestName;

    public ContainerSpawnBehaviour(int index,
                                    QuestStepActivationScript script = null,
                                    string secretDoorFlag = null,
                                    ChestType type = ChestType.Chest,
                                    string chestName = null)
    {
        this.index = index;
        this.script = script;
        this.secretDoorFlag = secretDoorFlag;
        this.type = type;
        // this.chestName = chestName;
    }

    public void addBehaviour(GameObject gameObject)
    {
        Container chest = gameObject.AddComponent<Container>();

        chest.populate(index, type);

        chest.script = script;

        chest.setSecretDoorFlag(secretDoorFlag);
    }
}

public class DialogueTriggerSpawnBehaviour : IExtraSpawnBehaviour
{
    private string npcName;
    private SpeakAtStartScript speakAtStartScript;
    private PlaySFXLogic introSFX;
    private bool hasExtraSpaces;
    private IStoryVariableSource variableSource;

    private IDialogueSource _DialogueSource;
    public IDialogueSource dialogueSource
    {
        set
        {
           _DialogueSource = value; 
        }
    }

    public DialogueTriggerSpawnBehaviour(string npcName, PlaySFXLogic introSFX, bool hasExtraSpaces, SpeakAtStartScript speakAtStartScript = null, IStoryVariableSource variableSource = null)
    {
        this.npcName = npcName;
        this.speakAtStartScript = speakAtStartScript;
        this.introSFX = introSFX;
        this.variableSource = variableSource;
        this.hasExtraSpaces = hasExtraSpaces;
    }

    public void addBehaviour(GameObject gameObject)
    {
        DialogueTrigger dialogueTrigger = gameObject.AddComponent<DialogueTrigger>();

        dialogueTrigger.npcName = npcName;
        dialogueTrigger.speakAtStartScript = speakAtStartScript;
        dialogueTrigger.introAudioClipLogic = introSFX;
        dialogueTrigger.dialogueSource = _DialogueSource;

        if(hasExtraSpaces)
        {
            dialogueTrigger.listenForActivationByName();
        }
    }
}

public class PartyMemberDespawnListenerSpawnBehaviour : IExtraSpawnBehaviour
{
    private string npcName;
    
    public PartyMemberDespawnListenerSpawnBehaviour(string npcName)
    {
        this.npcName = npcName;
    }

    public void addBehaviour(GameObject gameObject)
    {
        PartyMemberDespawnListener listener = gameObject.AddComponent<PartyMemberDespawnListener>();

        listener.partyMemberName = npcName;
    }
}