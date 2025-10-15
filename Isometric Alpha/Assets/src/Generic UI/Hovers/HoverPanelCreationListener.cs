using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPanelCreationListener : MonoBehaviour
{

    private void OnEnable()
    {
        MouseHoverManager.OnHoverPanelCreation.AddListener(destroyHover);
    }

    private void OnDestroy()
    {
        MouseHoverManager.OnHoverPanelCreation.RemoveListener(destroyHover);
    }

    private void destroyHover()
    {
        DestroyImmediate(gameObject);
    }

}
