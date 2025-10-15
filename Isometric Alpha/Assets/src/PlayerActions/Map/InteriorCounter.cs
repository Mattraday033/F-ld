using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteriorCounter : MonoBehaviour
{
    public Transform questCounter;
    public Image[] interiorCounters;

    public void setInteriorCounters(int interiorsToSpawn)   
    {
        if (interiorsToSpawn <= 0)
        {
            gameObject.SetActive(false);
            questCounter.localPosition = new Vector3(-67f, 0f, 0f);
            return;
        }
        else
        {
            questCounter.localPosition = new Vector3(-105f, 0f, 0f);
        }

        for (int index = 0; index < interiorCounters.Length; index++)
            {
                if (index < interiorsToSpawn)
                {
                    interiorCounters[index].color = Color.white;
                }
                else
                {
                    interiorCounters[index].color = Color.black;
                }
            }
    }
}
