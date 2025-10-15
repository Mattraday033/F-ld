using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualUpdateScrollRect : ScrollRect
{

    protected override void LateUpdate()
    {
        UpdatePrevData();
        
        base.LateUpdate();
    }
}
