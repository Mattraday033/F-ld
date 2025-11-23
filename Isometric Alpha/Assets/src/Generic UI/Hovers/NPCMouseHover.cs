using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMouseHover : MonoBehaviour
{

    //Needs to be attached to an object with a 2DCollider Component

    public IRevealable npc;

    void Start()
    {
        npc = transform.parent.GetComponent<IRevealable>();

        if (npc == null)
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnMouseEnter()
    {
        npc.OnPointerEnter(null);
    }

    private void OnMouseExit()
    {
        npc.OnPointerExit(null);
    }

    private void OnEnable()
    {
        PlayerOOCStateManager.OnStateChangeToInUI.AddListener(enableSelf);
        PlayerOOCStateManager.OnStateChangeFromInUI.AddListener(disableSelf);
    }

    private void OnDestroy()
    {
        PlayerOOCStateManager.OnStateChangeToInUI.RemoveListener(enableSelf);
        PlayerOOCStateManager.OnStateChangeFromInUI.RemoveListener(disableSelf);
    }

    private void enableSelf()
    {
        enabled = true;
    }

    private void disableSelf()
    {
        enabled = false;
    }

}
