using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TemporaryGate : Gate, INameTagSuppressor
{
    private OverHeadIconManager overHeadIconManager;
    private Collider2D movementBlockingCollider;

    private bool _Open;
    public bool open { get { return _Open; } }

    //an open gate is only waiting to close again, so it shows no name tag or outline in the meantime
    public bool suppressNameTag { get { return _Open; } }

    //matches getGateKey, as a temporary gate's uniqueName always carries its index
    public static string generateKey(string locationName, string gateName, int index)
    {
        return locationName + gateName + index;
    }

    protected override void Awake()
    {
        base.Awake();

        //the tilemap collider on the root blocks movement, and is what the player finds the gate by when interacting
        movementBlockingCollider = GetComponent<TilemapCollider2D>();
        overHeadIconManager = GetComponent<OverHeadIconManager>();
    }

    //setKey calls in as the gate spawns, so this puts it where the saved state says it belongs without a sound
    public override void checkGateStatus()
    {
        setOpen(TrapAndButtonStateManager.contains(getGateKey()));
    }

    public override void createListeners()
    {
        if(rendererList != null)
        {
            RevealManager.OnReveal.AddListener(revealUnlessOpen);
        }

        TrapAndButtonStateManager.OnSetTraps.AddListener(setStatus);
    }

    public override void destroyListeners()
    {
        if(rendererList != null)
        {
            RevealManager.OnReveal.RemoveListener(revealUnlessOpen);
        }

        TrapAndButtonStateManager.OnSetTraps.RemoveListener(setStatus);
    }

    private void revealUnlessOpen(bool toggleReveal)
    {
        onReveal(toggleReveal && !_Open);
    }

    private void setStatus(string key, bool status)
    {
        if(!key.Equals(getGateKey()))
        {
            return;
        }

        bool wasOpen = _Open;

        setOpen(status);

        if(wasOpen != _Open)
        {
            AudioManager.playGateOpenShortSFX();
        }
    }

    //the gate stays active while open so it keeps listening for the button that closes it again
    private void setOpen(bool open)
    {
        if(_Open == open)
        {
            return;
        }

        _Open = open;

        if(movementBlockingCollider != null)
        {
            movementBlockingCollider.enabled = !open;
        }

        //an extra space of a multi-cell gate is the blank Extra Space prefab, which only has the collider to toggle
        if(rendererList == null)
        {
            return;
        }

        rendererList[SpriteLayer.Body].enabled = !open;

        if(open)
        {
            rendererList.removeOutline();
        }

        //left on, the body collider would take the mouse hover from whatever sits behind the doorway
        rendererList.setBodyColliderEnabled(!open);

        if(overHeadIconManager != null)
        {
            overHeadIconManager.onReveal(!open && RevealManager.currentlyRevealed);
        }
    }

}
