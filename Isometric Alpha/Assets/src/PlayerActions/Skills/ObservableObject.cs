using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Ink.Runtime;

public class SecretDoorInfo : IStoryVariableSource
{

    public List<string> secretDoorKeys = new List<string>();
    public int difficulty;
    public string description;
    public string searchChoice;
    public string successDescription;
    public string successChoice;
    public string failureDescription;
    public string openDescription;

    // NoDialogue means "use the generic secret door story"; see OOCSpawnDetails.getDialogue.
    public DialogueKey customDialogueKey;

    public bool addHostilityIfOutside;
    public string questName;
    public string questStepName;
    public bool completeQuest;

    public SecretDoorInfo(  string secretDoorKey = null,
                            List<string> secretDoorKeys = null,
                            int difficulty = Constants.difficultyTwo,
                            string description = null,
                            string searchChoice = null,
                            string successDescription = null,
                            string successChoice = null,
                            string failureDescription = null,
                            string openDescription = null,
                            DialogueKey customDialogueKey = DialogueKey.NoDialogue,
                            bool addHostilityIfOutside = false,
                            string questName = null,
                            string questStepName = null,
                            bool completeQuest = false)
    {
        if(secretDoorKey != null)
        {
            this.secretDoorKeys.Add(secretDoorKey);
        }

        if(secretDoorKeys != null)
        {
            this.secretDoorKeys.AddRange(secretDoorKeys);
        }

        this.difficulty = difficulty;
        this.description = description;
        this.searchChoice = searchChoice;
        this.successDescription = successDescription;
        this.successChoice = successChoice;
        this.failureDescription = failureDescription;
        this.openDescription = openDescription;
        this.customDialogueKey = customDialogueKey;
        this.addHostilityIfOutside = addHostilityIfOutside;
        this.questName = questName;
        this.questStepName = questStepName;
        this.completeQuest = completeQuest;
    }

    public virtual bool hasBeenDiscovered()
    {
        foreach(string key in secretDoorKeys)
        {
            if(SecretDoorFlags.secretDoorHasBeenDiscovered(key))
            {
                return true;
            }
        }

        return false;
    }

    public Story addVariables(Story story)
    {

        if (story.variablesState[InkVariableNameList.secretDoorKey] != null)
        {
            story.variablesState[InkVariableNameList.secretDoorKey] = secretDoorKeys[0];
        }

        if (story.variablesState[InkVariableNameList.obsLvlVarName] != null)
        {
            story.variablesState[InkVariableNameList.obsLvlVarName] = PartyStats.getObservationLevel();
        }

        if (story.variablesState[InkVariableNameList.obsDiffVarName] != null)
        {
            story.variablesState[InkVariableNameList.obsDiffVarName] = difficulty;
        }

        if (description != null && 
            story.variablesState[InkVariableNameList.description] != null)
        {
            story.variablesState[InkVariableNameList.description] = description;
        }

        if (searchChoice != null &&
            story.variablesState[InkVariableNameList.searchChoice] != null)
        {
            story.variablesState[InkVariableNameList.searchChoice] = searchChoice;
        }

        if (successDescription != null &&
            story.variablesState[InkVariableNameList.successDescription] != null)
        {
            story.variablesState[InkVariableNameList.successDescription] = successDescription;
        }

        if (successChoice != null && 
            story.variablesState[InkVariableNameList.successChoice] != null)
        {
            story.variablesState[InkVariableNameList.successChoice] = successChoice;
        }

        if (failureDescription != null &&
            story.variablesState[InkVariableNameList.failureDescription] != null)
        {
            story.variablesState[InkVariableNameList.failureDescription] = failureDescription;
        }

        if (openDescription != null &&
            story.variablesState[InkVariableNameList.openDescription] != null)
        {
            story.variablesState[InkVariableNameList.openDescription] = openDescription;
        }

        if(story.variablesState[InkVariableNameList.addHostilityIfOutside] != null)
        {
            story.variablesState[InkVariableNameList.addHostilityIfOutside] = addHostilityIfOutside;
        }

        if (questName != null && story.variablesState[InkVariableNameList.questName] != null)
        {
            story.variablesState[InkVariableNameList.questName] = questName;
        }

        if (questStepName != null && story.variablesState[InkVariableNameList.questStepName] != null)
        {
            story.variablesState[InkVariableNameList.questStepName] = questStepName;
        }

        if (story.variablesState[InkVariableNameList.completeQuest] != null)
        {
            story.variablesState[InkVariableNameList.completeQuest] = completeQuest;
        }

        return story;
    }

}

public class TutorialSecretDoorInfo : SecretDoorInfo
{
    private StartSpawningAllTrueFlagList tutorialFlagList;

    public TutorialSecretDoorInfo(string secretDoorKey, StartSpawningAllTrueFlagList tutorialFlagList) :
    base(secretDoorKey, difficulty: Constants.difficultyTwo)
    {
        this.tutorialFlagList = tutorialFlagList;
    }

    public override bool hasBeenDiscovered()
    {
        return !tutorialFlagList.evaluateFlags() || base.hasBeenDiscovered();
    }
}

public class ObservableObject : MonoBehaviour, IRevealable
{
    public bool observed = false;
    public List<string> secretDoorKeys = new List<string>();

    [SerializeField]
    private SpriteLayerRendererList _RendererList;

    //SpriteDescription appearances only draw to the body layer, so it holds the secret door's sprite
    private SpriteRenderer doorRenderer { get { return _RendererList[SpriteLayer.Body]; } }

    //the renderer list shows or hides this with the player's terrain state
    public Sprite terrainSprite
    {
        get
        {
            SpriteRenderer terrainRenderer = _RendererList[SpriteLayer.Terrain];
            return terrainRenderer != null ? terrainRenderer.sprite : null;
        }
        set
        {
            _RendererList.setTerrainSprite(value);
        }
    }

    public DialogueTrigger dialogueTrigger;
    
    public QuestStepActivationScript script;
    public bool isRevealable
    {
        get
        {
            if(gameObject.layer == LayerAndTagManager.npcLayer)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }

    public readonly static UnityEvent SetAllSecretDoorsObservable = new UnityEvent();

    public string displayName { get { return dialogueTrigger.displayName; } }
    public string uniqueName { get { return dialogueTrigger.uniqueName; } }

    private void Awake()
    {
        if(_RendererList == null)
        {
            _RendererList = GetComponent<SpriteLayerRendererList>();
        }
    }

    //the DialogueTrigger arrives with the universal spawn behaviours, after this component is added as an aesthetic one
    private void Start()
    {
        if(dialogueTrigger == null)
        {
            dialogueTrigger = GetComponent<DialogueTrigger>();
        }
    }

    private void OnEnable()
    {
        createListeners();
    }

    private void OnDestroy()
    {
        destroyListeners();
    }

    //IRevealable interface methods

    public SpriteLayerRendererList rendererList
    {
        get
        {
            return _RendererList;
        }
    }

    public void createListeners()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.AddListener(hideSecretDoor);
        SetAllSecretDoorsObservable.AddListener(setGameObjectObservable);

        RevealManager.OnReveal.AddListener(onReveal);
    }

    public void destroyListeners()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(hideSecretDoor);
        SetAllSecretDoorsObservable.RemoveListener(setGameObjectObservable);

        RevealManager.OnReveal.RemoveListener(onReveal);
    }

    public void onReveal(bool toggleReveal)
    {
        if(!isRevealable || rendererList == null)
        {
            return;
        }

        if(toggleReveal)
        {
            rendererList.createOutline(getRevealColor());
        } else
        {
            rendererList.removeOutline();
        }
    }

    public Color getRevealColor()
    {
        return ColorList.observationColor;
    }

    public void createHoverTag()
    {
        //Empty on purpose (the secret door is meant to read as terrain until it is observed)
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onReveal(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onReveal(false);
    }

    private void setGameObjectObservable()
    {
        gameObject.layer = LayerAndTagManager.observableLayer;
    }

    public void markAsObserved()
    {
        if (observed)
        {
            return;
        }

        observed = true;
        gameObject.layer = LayerAndTagManager.npcLayer;

        doorRenderer.color = Color.magenta;

        SpriteRenderer terrainRenderer = _RendererList[SpriteLayer.Terrain];

        if(terrainRenderer != null)
        {
            terrainRenderer.color = Color.magenta;
        }
    }

    private static void playAudioClip()
    {
        SFXType sfxType = SFXType.NoSFX;

        switch(MapObjectList.getCurrentZoneKey())
        {
            case ZoneKeyList.mineLvl1:
            case ZoneKeyList.mineLvl2:
            case ZoneKeyList.mineLvl3:
                sfxType = SFXType.RockIntro;
                break;
            default:
                
                sfxType = SFXType.GateOpen;
                break;
        }

        AudioManager.playAudioClipAsSingleton(sfxType);
    }   

    public void hideSecretDoor(string doorToBeHidden)
    {
        if (secretDoorKeys.Contains(doorToBeHidden))
        {
            if(script != null)
            {
                script.runScript();
            }

            playAudioClip();
            GameObject.Destroy(gameObject);
        }
    }

}
