using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustFramerate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RefreshRate rr = new RefreshRate();
		float vSyncFactor = ((float) rr.value )/ 60.0f;
		QualitySettings.vSyncCount = Mathf.Clamp(Mathf.RoundToInt(vSyncFactor), 1, 4);
    }

}
