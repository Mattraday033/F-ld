using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraSetter : MonoBehaviour
{

    public Canvas canvas;
    public string sortingLayerName;

    private void Awake()
    {
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerName = sortingLayerName;
    }

}
