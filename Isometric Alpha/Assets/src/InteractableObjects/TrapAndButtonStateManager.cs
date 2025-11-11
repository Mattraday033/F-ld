using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class TrapAndButtonStateManager : MonoBehaviour
{
    public readonly static UnityEvent<string, bool> OnSetTraps = new UnityEvent<string, bool>();

    public static Dictionary<string, bool> allActivatedTrapKeys;

    public static bool contains(string key)
    {
        if (!allActivatedTrapKeys.ContainsKey(key))
        {
            return false;
        }

        return allActivatedTrapKeys[key];
    }

    public static void setKey(string key, bool status)
    {
        allActivatedTrapKeys[key] = status;
        OnSetTraps.Invoke(key, status);
    }

    [RuntimeInitializeOnLoadMethod]
    public static void resetTrapKeys()
    {
        allActivatedTrapKeys = new Dictionary<string, bool>();
    }

    public static void setTrapsAndButtons()
    {
        foreach (KeyValuePair<string, bool> kvp in allActivatedTrapKeys)
        {
            OnSetTraps.Invoke(kvp.Key, kvp.Value);
        }
    }

    public static void resetTrapKeys(FlagWrapper[] wrappers)
    {
        resetTrapKeys();

        foreach (FlagWrapper wrapper in wrappers)
        {
            allActivatedTrapKeys[wrapper.flagName] = wrapper.flagStatus;
        }
    }

    public static FlagWrapper[] getAllWrappers()
    {
        List<FlagWrapper> wrappers = new List<FlagWrapper>();

        foreach (KeyValuePair<string, bool> kvp in allActivatedTrapKeys)
        {
            wrappers.Add(new FlagWrapper(kvp));
        }

        return wrappers.ToArray();
    }

    private void OnEnable()
    {
        TransitionManager.BeforeTransition.AddListener(resetTrapKeys);
    }
    
    private void OnDisable()
    {
        TransitionManager.BeforeTransition.RemoveListener(resetTrapKeys);
    }
}


    // void Start()
    // {
    //     if(allActivatedTrapKeys != null && allActivatedTrapKeys.Count > 0)
    // 	{
    // 		GameObject[] cunningTargets = GameObject.FindGameObjectsWithTag(LayerAndTagManager.cunningTargetTag);
    // 		GameObject[] permanentButtons = GameObject.FindGameObjectsWithTag(LayerAndTagManager.permanentButtonTag);

    // 		foreach(GameObject cunningTarget in cunningTargets)
    // 		{
    // 			CunningObject cunningObject = cunningTarget.GetComponent<CunningObject>();

    // 			if(cunningObject != null && contains(cunningObject.getKey()))
    // 			{
    // 				cunningObject.cunning(skipKeyHandling);
    // 			}
    // 		}

    // 		foreach(GameObject permanentButton in permanentButtons)
    // 		{
    // 			FloorButtonPermanent floorButtonPermanent = permanentButton.GetComponent<FloorButtonPermanent>();

    // 			if(floorButtonPermanent != null && contains(floorButtonPermanent.getKey()))
    // 			{
    // 				floorButtonPermanent.handleButtonPress(skipKeyHandling);
    // 			}
    // 		}

    // 	} else if(allActivatedTrapKeys == null)
    // 	{
    // 		allActivatedTrapKeys = new ArrayList();
    // 	}
    // }