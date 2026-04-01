using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMouseHover : MonoBehaviour
{
    private const float zPosMultiplier  = .0001f;

    //Needs to be attached to an object with a 2DCollider Component

    public IRevealable[] revealables;
    public PolygonCollider2D polygonCollider2D;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        revealables = transform.parent.GetComponents<IRevealable>();
        
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
            
                foreach(IRevealable revealable in revealables)
                {
                    if(revealable == null)
                    {
                        continue;
                    }

                    revealable.OnPointerEnter(null);
                }

                return;
            default:
                return;
        }
    }

    private void OnMouseExit()
    {
        foreach(IRevealable revealable in revealables)
        {
            if(revealable == null)
            {
                continue;
            }

            revealable.OnPointerExit(null);
        }
    }
}
