using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarCanvas : MonoBehaviour
{

    private static HealthBarCanvas instance;

    public static HealthBarCanvas getInstance()
    {
        return instance;
    }

    public static void disableHealthBarCanvas()
    {
        if (getInstance() == null)
        {
            return;
        }

        getInstance().gameObject.SetActive(false);
    }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one HealthBarCanvas in the scene.");
        }

        instance = this;
    }

}
