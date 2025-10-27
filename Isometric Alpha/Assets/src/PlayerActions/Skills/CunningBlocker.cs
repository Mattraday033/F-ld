using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CunningBlocker : CunningObject
{
    public GameObject blocker;

    public void build(Facing startFacing, Facing endFacing, CunningObjectSpriteCategory type, GameObject blocker)
    {
        base.build(startFacing, endFacing, type);

        this.blocker = blocker;
    }

    public override void setStatus(string key, bool status)
    {
        if(!getKey().Equals(getKey()))
        {
            return;
        }

        activated = status;

        setBlockerStatus();

        setToCurrentSprite();
    }
    
    public override void cunning(bool trackChangeInStateManager)
    {
        activated = !activated;

        setBlockerStatus();

        setToCurrentSprite();

        if (trackChangeInStateManager)
        {
            trackKey();
        }
    }

    private void setBlockerStatus()
    {
        if (activated)
        {
            blocker.SetActive(false);
        }
        else
        {
            blocker.SetActive(true);
        }
    }
}
