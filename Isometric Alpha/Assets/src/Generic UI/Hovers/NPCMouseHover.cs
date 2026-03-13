using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMouseHover : MonoBehaviour
{
    private const float zPosMultiplier  = .0001f;

    //Needs to be attached to an object with a 2DCollider Component

    public IRevealable npc;
    public PolygonCollider2D polygonCollider2D;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        npc = transform.parent.GetComponent<IRevealable>();
        
        Vector3Int currentCell = AreaManager.getMasterGrid().WorldToCell(transform.parent.position);

        transform.position = new Vector3(transform.position.x, transform.position.y, -1f + ((( zPosMultiplier * (float) currentCell.x) + ( zPosMultiplier * (float) currentCell.y))/2f));

        if(spriteRenderer == null)
        {
            spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        }

        Helpers.updatePolygonCollider(spriteRenderer, polygonCollider2D);
    }

    private void OnMouseEnter()
    {
        switch(PlayerOOCStateManager.currentActivity)
        {
            case OOCActivity.walking:
            case OOCActivity.cunning:
            case OOCActivity.observing:
            case OOCActivity.intimidating:
            case OOCActivity.inChestUI:
            
                if(npc != null)
                {
                    npc.OnPointerEnter(null);
                }
                
                return;
            default:
                return;
        }
    }

    private void OnMouseExit()
    {
        if(npc == null)
        {
            return;
        }

        npc.OnPointerExit(null);
    }
}
