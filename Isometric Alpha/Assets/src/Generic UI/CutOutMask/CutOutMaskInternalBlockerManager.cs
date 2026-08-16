using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutOutMaskInternalBlockerManager : MonoBehaviour
{

    private static CutOutMaskInternalBlockerManager instance;

    public static bool isBlocking()
    {
        return instance != null;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    public GameObject internalBlocker;

    public void turnOnInternalBlocker()
    {
        if(internalBlocker)
        {
            internalBlocker.SetActive(true);
        }
    }

}
