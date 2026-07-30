using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMouseHover : MonoBehaviour
{

    //Needs to be attached to an object with a 2DCollider Component

    public IRevealable[] revealables;
    public PolygonCollider2D polygonCollider2D;
    public SpriteRenderer spriteRenderer;

    public void OnEnable()
    {
        createListeners();
    }

    public void OnDisable()
    {
        destroyListeners();
    }

    public void createListeners()
    {
        PlayerOOCStateManager.OnStateChangeToSkill.AddListener(disableHover);
        PlayerOOCStateManager.OnStateChangeFromSkill.AddListener(enableHover);
    }

    public void destroyListeners()
    {
        PlayerOOCStateManager.OnStateChangeToSkill.RemoveListener(disableHover);
        PlayerOOCStateManager.OnStateChangeFromSkill.RemoveListener(enableHover);
    }

    private void disableHover()
    {
        polygonCollider2D.enabled = false;
    }

    private void enableHover()
    {
        polygonCollider2D.enabled = true;
    }

    void Start()
    {
        revealables = transform.parent.GetComponents<IRevealable>();
        
        Vector3Int currentCell = AreaManager.getMasterGrid().WorldToCell(transform.parent.position);

        transform.position = new Vector3(transform.position.x, transform.position.y, Helpers.calculateColliderZPosition(currentCell));

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
        if(PlayerOOCStateManager.currentActivity == OOCActivity.inTutorialSequence)
        {
            return;            
        }

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
