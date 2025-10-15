using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoverParentCanvas : MonoBehaviour
{

    private static Transform mouseHoverParentCanvas;

    private void Awake()
    {
        mouseHoverParentCanvas = transform;
    }

    public static Transform getMouseHoverParent()
    {
        return mouseHoverParentCanvas;
    }


}
