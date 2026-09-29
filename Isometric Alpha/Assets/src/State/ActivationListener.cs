using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ActivationCategory
{
    Cunning,
    Dialogue
}

public enum ActivationDesignatorType
{
    Name,
    Index
}

public class ActivationListener : MonoBehaviour
{
    
    private Dictionary<ActivationDesignatorType, List<ActivationCategory>> channelInfo = new ()
    {
        [ActivationDesignatorType.Name] = new List<ActivationCategory>(),
        [ActivationDesignatorType.Index] = new List<ActivationCategory>()
    };

    public int index;
    public INameSource nameSource;
    public string npcName
    {
        get
        {
            if(nameSource != null)
            {
                return nameSource.getName();
            }
            
            return "";
        }
    }

    private void Awake()
    {
        EventList.DestroyAllSpawnedGameObjects.AddListener(destroySpawnedGameObject);
        // EventList.SetActiveByNameChannel.AddListener(setActiveByName);
        // EventList.SetObstaclesActiveByIntChannel.AddListener(setActiveByIndex);
    }

    private void OnDestroy()
    {
        EventList.DestroyAllSpawnedGameObjects.RemoveListener(destroySpawnedGameObject);
        EventList.SetActiveByNameChannel.RemoveListener(setActiveByName);
        EventList.SetActiveByIndexChannel.RemoveListener(setActiveByIndex);
    }

    //immediate, because the next area's objects are spawned straight after the old ones are wiped
    private void destroySpawnedGameObject()
    {
        DestroyImmediate(gameObject);
    }

    public void listenForActivationByName(ActivationCategory category)
    {
        EventList.SetActiveByNameChannel.RemoveListener(setActiveByName);
        EventList.SetActiveByNameChannel.AddListener(setActiveByName);
    }

    public void listenForActivationByIndex(ActivationCategory category)
    {
        EventList.SetActiveByIndexChannel.RemoveListener(setActiveByIndex);
        EventList.SetActiveByIndexChannel.AddListener(setActiveByIndex);
    }

    private void setActiveByName(ActivationCategory category, string incomingName, bool status)
    {
        if(channelInfo[ActivationDesignatorType.Name].Contains(category) && 
            npcName.Equals(incomingName))
        {
            gameObject.SetActive(status);
        }
    }

    private void setActiveByIndex(ActivationCategory category, int incomingIndex, bool status)
    {
        if(channelInfo[ActivationDesignatorType.Index].Contains(category) && 
            incomingIndex == index)
        {
            gameObject.SetActive(status);
        }
    }
}
