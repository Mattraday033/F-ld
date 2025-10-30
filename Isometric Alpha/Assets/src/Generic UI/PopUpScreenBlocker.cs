using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScreenBlocker : MonoBehaviour
{

    private void OnEnable()
    {
        PopUpScreenBlockerManager.DestroyPopUpScreenBlockers.AddListener(destroySelf);
    }

    private void OnDisable()
    {
        PopUpScreenBlockerManager.DestroyPopUpScreenBlockers.RemoveListener(destroySelf);
    }
    
    private void destroySelf()
    {
        DestroyImmediate(gameObject);
    }
}
