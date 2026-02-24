using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class SecretDoorInfo : IStoryVariableSource
{

    public List<string> secretDoorKeys = new List<string>();
    public int difficulty;
    public string description;
    public string customDialoguePath;
    public bool addHostilityIfOutside;

    public SecretDoorInfo(string secretDoorKey = null, List<string> secretDoorKeys = null, int difficulty = Constants.difficultyTwo, string description = null, string customDialoguePath = null, bool addHostilityIfOutside = false)
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
        this.customDialoguePath = customDialoguePath;
        this.addHostilityIfOutside = addHostilityIfOutside;
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

        if (story.variablesState[InkVariableNameList.wisDiffVarName] != null)
        {
            story.variablesState[InkVariableNameList.wisDiffVarName] = difficulty;
        }

        if (description != null && 
            story.variablesState[InkVariableNameList.description] != null)
        {
            story.variablesState[InkVariableNameList.description] = description;
        }

        if(story.variablesState[InkVariableNameList.addHostilityIfOutside] != null)
        {
            story.variablesState[InkVariableNameList.addHostilityIfOutside] = addHostilityIfOutside;
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
    }

    private void OnDestroy()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(hideSecretDoor);
        TerrainVisibilityManager.OnTerrainVisibilityChange.RemoveListener(setTerrainSprite);
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

    public void hideSecretDoor(string doorToBeHidden)
    {
        if (secretDoorKeys.Contains(doorToBeHidden))
        {
            GameObject.DestroyImmediate(gameObject);
        }
    }

}
