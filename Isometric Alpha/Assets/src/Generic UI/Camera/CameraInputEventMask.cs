using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInputEventMask : MonoBehaviour
{


    void Start()
    {
        Camera.main.eventMask = LayerAndTagManager.cameraInputLayerMask;
    }

}
