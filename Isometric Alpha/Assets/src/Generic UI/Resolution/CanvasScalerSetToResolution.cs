using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerSetToResolution : CanvasScaler
{

    protected override void Awake()
    {
        m_ReferenceResolution = new Vector2(1920f, 1080f);
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

        base.Awake();
    }

}
