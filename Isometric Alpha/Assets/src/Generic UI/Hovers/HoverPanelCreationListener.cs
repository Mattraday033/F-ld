using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPanelCreationListener : MonoBehaviour
{

    private void OnEnable()
    {
        MouseHoverManager.OnHoverPanelCreation.AddListener(destroyHover);
        InspectNode.OnInspect.AddListener(disableDestroyHoverOnPanelCreation);
    }

    private void OnDestroy()
    {
        MouseHoverManager.OnHoverPanelCreation.RemoveListener(destroyHover);
        InspectNode.OnInspect.RemoveListener(disableDestroyHoverOnPanelCreation);
    }

    private void destroyHover()
    {
        DestroyImmediate(gameObject);
    }

    private void disableDestroyHoverOnPanelCreation()
    {
        if(InspectNode.inspecting)
        {
            MouseHoverManager.OnHoverPanelCreation.RemoveListener(destroyHover);
        } else
        {
            MouseHoverManager.OnHoverPanelCreation.RemoveListener(destroyHover);
            MouseHoverManager.OnHoverPanelCreation.AddListener(destroyHover);     
        }
    }

}
