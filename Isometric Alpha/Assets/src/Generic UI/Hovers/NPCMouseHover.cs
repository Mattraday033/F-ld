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

}
