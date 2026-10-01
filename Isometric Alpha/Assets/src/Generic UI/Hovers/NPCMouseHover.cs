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

        PlayerStateManager.OnStateChangeToSkill.AddListener(disableHover);
        PlayerStateManager.OnStateChangeFromSkill.AddListener(enableHover);
    }

    public void destroyListeners()
    {
        // MovementManager.OnMoveFinished.RemoveListener(setColliderPosition);
        // TransitionManager.AfterTransition.RemoveListener(setColliderPosition);

        PlayerStateManager.OnStateChangeToSkill.RemoveListener(disableHover);
        PlayerStateManager.OnStateChangeFromSkill.RemoveListener(enableHover);
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
        switch(PlayerStateManager.currentActivity)
        {
            case CurrentActivity.Walking:
            case CurrentActivity.Cunning:
            case CurrentActivity.Observing:
            case CurrentActivity.Intimidating:
            case CurrentActivity.InChestUI:
            
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
        if(PlayerStateManager.currentActivity == CurrentActivity.InTutorialSequence)
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
