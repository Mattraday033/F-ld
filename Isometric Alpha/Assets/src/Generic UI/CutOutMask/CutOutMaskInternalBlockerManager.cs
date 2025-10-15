using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutOutMaskInternalBlockerManager : MonoBehaviour
{

    public GameObject internalBlocker;

    public void turnOnInternalBlocker()
    {
        if(internalBlocker)
        {
            internalBlocker.SetActive(true);
        }
    }

}
