using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteriorCounter : MonoBehaviour
{
    public Image[] interiorCounters;

    public void setInteriorCounters(int interiorsToSpawn)   
    {
        if (interiorsToSpawn <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        for (int index = 0; index < interiorCounters.Length; index++)
        {
            if (index < interiorsToSpawn)
            {
                interiorCounters[index].gameObject.SetActive(true);
            }
            else
            {
                interiorCounters[index].gameObject.SetActive(false);
            }
        }
    }
}
