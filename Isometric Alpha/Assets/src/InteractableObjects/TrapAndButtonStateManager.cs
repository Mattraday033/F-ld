using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class TrapAndButtonStateManager : MonoBehaviour
{
    private static bool skipKeyHandling = true;
	
	public static ArrayList allActivatedTrapKeys = new ArrayList();
	
    void Start()
    {
        if(allActivatedTrapKeys != null && allActivatedTrapKeys.Count > 0)
		{
			GameObject[] cunningTargets = GameObject.FindGameObjectsWithTag(LayerAndTagManager.cunningTargetTag);
			GameObject[] permanentButtons = GameObject.FindGameObjectsWithTag(LayerAndTagManager.permanentButtonTag);
			
			foreach(GameObject cunningTarget in cunningTargets)
			{
				CunningObject cunningObject = cunningTarget.GetComponent<CunningObject>();
				
				if(cunningObject != null && contains(cunningObject.getKey()))
				{
					cunningObject.cunning(skipKeyHandling);
				}
			}
			
			foreach(GameObject permanentButton in permanentButtons)
			{
				FloorButtonPermanent floorButtonPermanent = permanentButton.GetComponent<FloorButtonPermanent>();
				
				if(floorButtonPermanent != null && contains(floorButtonPermanent.getKey()))
				{
					floorButtonPermanent.handleButtonPress(skipKeyHandling);
				}
			}
		
		} else if(allActivatedTrapKeys == null)
		{
			allActivatedTrapKeys = new ArrayList();
		}
    }
	
	public static void addKey(string key)
	{
		allActivatedTrapKeys.Add(key);
		//printAllKeys();
	}

	public static bool contains(string key)
	{	
		return allActivatedTrapKeys.Contains(key);
	}
	
	public static void removeKey(string key)
	{
		for(int keyIndex = 0; keyIndex < allActivatedTrapKeys.Count; keyIndex++)
		{
			if(allActivatedTrapKeys[keyIndex].Equals(key))
			{
				allActivatedTrapKeys.Remove(keyIndex);
			}
		}
	}
	
	public static void removeAllActivatedTrapKeys()
	{
		allActivatedTrapKeys = new ArrayList(); 
	}
	
	public static void printAllKeys()
	{
		Debug.Log("Printing all keys in TrapAndButtonStateManager.allActivatedTrapKeys: ");
		
		foreach(string key in allActivatedTrapKeys)
		{
			Debug.Log(key);
		}
		
		Debug.Log("Finished printing all keys in TrapAndButtonStateManager.allActivatedTrapKeys");
	}
}
