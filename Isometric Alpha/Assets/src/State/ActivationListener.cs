using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ActivationCategory
{
    Cunning,
    Dialogue,
    JoinedFormation,
    LeftFormation
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
    public string uniqueName
    {
        get
        {
            if(nameSource != null)
            {
                return nameSource.uniqueName;
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
        Formation.OnFormationChange.RemoveListener(checkFormation);
    }

    //immediate, because the next area's objects are spawned straight after the old ones are wiped
    private void destroySpawnedGameObject()
    {
        OnDestroy();

        DestroyImmediate(gameObject);
    }

    public void listenForActivationByName(ActivationCategory category)
    {
        if(!channelInfo[ActivationDesignatorType.Name].Contains(category))
        {
            channelInfo[ActivationDesignatorType.Name].Add(category);
        }

        EventList.SetActiveByNameChannel.RemoveListener(setActiveByName);
        EventList.SetActiveByNameChannel.AddListener(setActiveByName);

        if(category == ActivationCategory.JoinedFormation || category == ActivationCategory.LeftFormation)
        {
            Formation.OnFormationChange.RemoveListener(checkFormation);
            Formation.OnFormationChange.AddListener(checkFormation);
        }
    }

    public void listenForActivationByIndex(ActivationCategory category)
    {
        if(!channelInfo[ActivationDesignatorType.Index].Contains(category))
        {
            channelInfo[ActivationDesignatorType.Index].Add(category);
        }

        EventList.SetActiveByIndexChannel.RemoveListener(setActiveByIndex);
        EventList.SetActiveByIndexChannel.AddListener(setActiveByIndex);
    }

    private void setActiveByName(ActivationCategory category, string incomingName, bool status)
    {
        if(ignoreActivationByState())
        {
                return;
        }

        if(gameObject.activeSelf == status)
        {
            return;
        }

        if(channelInfo[ActivationDesignatorType.Name].Contains(category) &&
            uniqueName.Equals(incomingName))
        {
            gameObject.SetActive(status);
        }
    }

    private void setActiveByIndex(ActivationCategory category, int incomingIndex, bool status)
    {
        if(ignoreActivationByState())
        {
                return;
        }

        if(channelInfo[ActivationDesignatorType.Index].Contains(category) && 
            incomingIndex == index)
        {
            gameObject.SetActive(status);
        }
    }

    //only ever deactivates: an NPC hides once its party member joins the formation, and a follower once its party member leaves it
    private void checkFormation()
    {
        if(string.IsNullOrEmpty(uniqueName))
        {
            return;
        }

        bool inFormation = State.formation.contains(uniqueName);

        if((inFormation && channelInfo[ActivationDesignatorType.Name].Contains(ActivationCategory.JoinedFormation)) ||
            (!inFormation && channelInfo[ActivationDesignatorType.Name].Contains(ActivationCategory.LeftFormation)))
        {
            gameObject.SetActive(false);
        }
    }

    private bool ignoreActivationByState()
    {
        switch(PlayerOOCStateManager.currentActivity)
        {
            case OOCActivity.inFade:
            case OOCActivity.Loading:
                return true;
            default:
                return false;
        }

    }
}


public static class ActivationRequirementList
{
    public readonly static KeyValuePair<ActivationDesignatorType, ActivationCategory> cunningByName = new(ActivationDesignatorType.Name, ActivationCategory.Cunning);
    public readonly static KeyValuePair<ActivationDesignatorType, ActivationCategory> cunningByIndex = new(ActivationDesignatorType.Index, ActivationCategory.Cunning);

    public readonly static KeyValuePair<ActivationDesignatorType, ActivationCategory> dialogueByName = new(ActivationDesignatorType.Name, ActivationCategory.Dialogue);
    public readonly static KeyValuePair<ActivationDesignatorType, ActivationCategory> dialogueByIndex = new(ActivationDesignatorType.Index, ActivationCategory.Dialogue);

    public readonly static KeyValuePair<ActivationDesignatorType, ActivationCategory> joinedFormationByName = new(ActivationDesignatorType.Name, ActivationCategory.JoinedFormation);
    public readonly static KeyValuePair<ActivationDesignatorType, ActivationCategory> leftFormationByName = new(ActivationDesignatorType.Name, ActivationCategory.LeftFormation);
}