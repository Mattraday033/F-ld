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

    public bool hasBeenDiscovered()
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

public class ObservableObject : MonoBehaviour
{
    public bool observed = false;
    public string secretDoorKey;
    public SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.AddListener(hideSecretDoor);
    }

    private void OnDestroy()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(hideSecretDoor);
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
