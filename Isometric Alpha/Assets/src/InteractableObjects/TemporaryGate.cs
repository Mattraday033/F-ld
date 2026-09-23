using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryGate : Gate
{
    private OverHeadIconManager overHeadIconManager;
    private Collider2D colliderTileMap;

    protected override void Awake()
    {
        base.Awake();

        colliderTileMap = GetComponent<Collider2D>();
        overHeadIconManager = GetComponent<OverHeadIconManager>();
    }

    public override void checkGateStatus()
    {
        if (TrapAndButtonStateManager.contains(getGateKey()))
        {
            hideSelf();
        } else
        {
            showSelf();
        }
    }

    public override void createListeners()
    {
        RevealManager.OnReveal.AddListener(onReveal);
        TrapAndButtonStateManager.OnSetTraps.AddListener(setStatus);
    }

    public override void destroyListeners()
    {
        RevealManager.OnReveal.RemoveListener(onReveal);
        TrapAndButtonStateManager.OnSetTraps.RemoveListener(setStatus);
    }

    private void setStatus(string key, bool hide)
    {
        if(key.Equals(getGateKey()))
        {
            if(hide)
            {
                hideSelf();
            } else
            {
                showSelf();
            }
        }
    }

    private void hideSelf()
    {
        // colliderTileMap.enabled = false;
        // spriteRenderer.color = Color.clear;

        // if(overHeadIconManager != null)
        // {
        //     overHeadIconManager.onReveal(false);
        // }

        // if(PlayerOOCStateManager.currentActivity != OOCActivity.inFade)
        // {
        //     AudioManager.playGateOpenShortSFX();
        // }
    }

    private void showSelf() 
    {
        // colliderTileMap.enabled = true;
        // spriteRenderer.color = Color.white;

        // if(overHeadIconManager != null)
        // {
        //     overHeadIconManager.onReveal(RevealManager.currentlyRevealed);
        // }

        // if(PlayerOOCStateManager.currentActivity != OOCActivity.inFade)
        // {
        //     AudioManager.playGateOpenShortSFX();
        // }
    }

}
