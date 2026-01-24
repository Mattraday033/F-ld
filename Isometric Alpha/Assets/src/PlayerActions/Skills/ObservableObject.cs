using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class SecretDoorInfo : IStoryVariableSource
{

    public string secretDoorKey;

    public SecretDoorInfo(string secretDoorKey)
    {
        this.secretDoorKey = secretDoorKey;
    }

    public virtual bool hasBeenDiscovered()
    {
        return SecretDoorFlags.secretDoorHasBeenDiscovered(secretDoorKey);
    }

    public Story addVariables(Story story)
    {
        if (story.variablesState[nameof(secretDoorKey)] != null)
        {
            story.variablesState[nameof(secretDoorKey)] = secretDoorKey;
        }

        return story;
    }

}

public class TutorialSecretDoorInfo : SecretDoorInfo
{
    private StartSpawningAllTrueFlagList tutorialFlagList;

    public TutorialSecretDoorInfo(string secretDoorKey, StartSpawningAllTrueFlagList tutorialFlagList) :
    base(secretDoorKey)
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
    public string secretDoorKey;
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
        if (!doorToBeHidden.Equals(secretDoorKey))
        {
            return;
        }

        GameObject.DestroyImmediate(gameObject);
    }

}
