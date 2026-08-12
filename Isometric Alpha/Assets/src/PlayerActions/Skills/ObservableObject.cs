using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    public string customDialoguePath;
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
                            string customDialoguePath = null,
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
        this.customDialoguePath = customDialoguePath;
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

public class ObservableObject : MonoBehaviour, INonRevealableNameSource
{
    public bool observed = false;
    public List<string> secretDoorKeys = new List<string>();
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer terrainRenderer;

    private Sprite _TerrainSprite;
    public Sprite terrainSprite
    {
        get
        {
            return _TerrainSprite;
        }
        set
        {
            _TerrainSprite = value;
            terrainRenderer.sprite = value;
        }
    }

    public DialogueTrigger dialogueTrigger;
    
    public QuestStepActivationScript script;

    public readonly static UnityEvent SetAllSecretDoorsObservable = new UnityEvent();

    public string getName()
    {
        return dialogueTrigger.getName();
    }

    public bool isRevealable()
    {
        if(gameObject.layer == LayerAndTagManager.npcLayer)
        {
            return true;
        } else
        {
            return false;
        }
    }


    private void OnEnable()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.AddListener(hideSecretDoor);
        TerrainVisibilityManager.OnTerrainVisibilityChange.AddListener(setTerrainSprite);
        SetAllSecretDoorsObservable.AddListener(setGameObjectObservable);
    }

    private void OnDestroy()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(hideSecretDoor);
        TerrainVisibilityManager.OnTerrainVisibilityChange.RemoveListener(setTerrainSprite);
        SetAllSecretDoorsObservable.RemoveListener(setGameObjectObservable);
    }

    private void setGameObjectObservable()
    {
        gameObject.layer = LayerAndTagManager.observableLayer;
    }

    public void setTerrainSprite(TerrainHiddenState terrainState)
    {
        if(_TerrainSprite == null)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            terrainRenderer.enabled = false;
            TerrainVisibilityManager.OnTerrainVisibilityChange.RemoveListener(setTerrainSprite);
            return;
        }

        switch(terrainState)
        {
            case TerrainHiddenState.InFrontOfTerrain:

                spriteRenderer.enabled = true;
                spriteRenderer.maskInteraction = SpriteMaskInteraction.None;

                terrainRenderer.enabled = false;
                break;
            case TerrainHiddenState.BehindTerrain:


                spriteRenderer.enabled = true;
                spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

                terrainRenderer.enabled = true;
                terrainRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                break;
            case TerrainHiddenState.TerrainHidden:
                spriteRenderer.enabled = false;

                terrainRenderer.enabled = true;
                terrainRenderer.maskInteraction = SpriteMaskInteraction.None;
                break;
        }
    }

    public void markAsObserved()
    {
        if (observed)
        {
            return;
        }

        observed = true;
        gameObject.layer = LayerAndTagManager.npcLayer;

        spriteRenderer.color = Color.magenta;
        terrainRenderer.color = Color.magenta;
    }

    private static void playAudioClip()
    {
        string sfxPath = "";

        switch(MapObjectList.getCurrentZoneKey())
        {
            case ZoneKeyList.mineLvl1:
            case ZoneKeyList.mineLvl2:
            case ZoneKeyList.mineLvl3:
                sfxPath = AudioClipList.rockIntroSFX;
                break;
            default:
                
                sfxPath = AudioClipList.gateOpen;
                break;
        }

        AudioManager.playAudioClipAsSingleton(sfxPath);
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
            GameObject.DestroyImmediate(gameObject);
        }
    }

}
