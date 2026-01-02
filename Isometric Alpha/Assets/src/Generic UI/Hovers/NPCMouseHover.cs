using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMouseHover : MonoBehaviour
{
    private const float zPosMultiplier  = .0001f;

    //Needs to be attached to an object with a 2DCollider Component

    public IRevealable npc;

    void Start()
    {
        npc = GetComponent<IRevealable>();
        
        Vector3Int currentCell = AreaManager.getMasterGrid().WorldToCell(transform.position);

        transform.position = new Vector3(transform.position.x, transform.position.y, -1f + ((( zPosMultiplier * (float) currentCell.x) + ( zPosMultiplier * (float) currentCell.y))/2f));
    }

    private void OnMouseEnter()
    {
        if(PlayerOOCStateManager.currentActivity != OOCActivity.walking || npc == null)
        {
            return;
        }

        npc.OnPointerEnter(null);
    }

    private void OnMouseExit()
    {
        if(PlayerOOCStateManager.currentActivity != OOCActivity.walking || npc == null)
        {
            return;
        }

        npc.OnPointerExit(null);
    }
}
