using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMouseHover : MonoBehaviour
{

    public IRevealable[] revealables;
    public PolygonCollider2D polygonCollider2D
    {
        get
        {
            return rendererList.bodyCollider;
        }
    }

    public SpriteLayerRendererList rendererList; 

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
        // MovementManager.OnMoveFinished.AddListener(setColliderPosition);
        // TransitionManager.AfterTransition.AddListener(setColliderPosition);

        PlayerOOCStateManager.OnStateChangeToSkill.AddListener(disableHover);
        PlayerOOCStateManager.OnStateChangeFromSkill.AddListener(enableHover);
    }

    public void destroyListeners()
    {
        // MovementManager.OnMoveFinished.RemoveListener(setColliderPosition);
        // TransitionManager.AfterTransition.RemoveListener(setColliderPosition);

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
