using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoverBase : MonoBehaviour
{
    public static int frameCount = 0;

    public RectTransform baseRectTransform;

    private void Awake()
    {
        frameCount = 0;
        Update();
    }

    public void Update()
    {
        if (frameCount % 5 == 0)
        {
            Vector3 mousePos = Input.mousePosition;

            baseRectTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
        }

        frameCount++; 
    }

}
