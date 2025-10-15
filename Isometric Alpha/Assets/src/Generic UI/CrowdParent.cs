using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdParent : MonoBehaviour
{

    public GameObject[] crowdNPCs;

    private void OnDisable()
    {
        foreach (GameObject npc in crowdNPCs)
        {
            if (npc != null)
            {
                npc.SetActive(false);
            }
        }
    }

}
