using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerSetToResolution : CanvasScaler
{

    protected override void Awake()
    {
        if (Camera.main != null)
        {
            Vector2 resolution = Vector2.one;

            if (Camera.main.aspect >= 3.5f)
            {
                resolution = new Vector2(3440f, 1440f);
            }
            else if (Camera.main.aspect >= 2.3f)
            {
                resolution = new Vector2(2560f, 1080f);
            }
            else
            {
                resolution = new Vector2(1920f, 1080f);
            }

            m_ReferenceResolution = resolution;
        }

        base.Awake();
    }

}
