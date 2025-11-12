using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CunningBlocker : CunningObject
{
    public List<GameObject> blockers = new List<GameObject>();
    public List<Vector3Int> blockerCoords = new List<Vector3Int>();

    public void build(Facing startFacing, Facing endFacing, CunningObjectSpriteCategory type, GameObject blocker, Vector3Int blockerCoords)
    {
        base.build(startFacing, endFacing, type);

        blockers.Add(blocker);
        this.blockerCoords.Add(blockerCoords);

        setToCurrentSprite();
    }

    public override bool validTarget(SkillType skillType)
    {
        if (!base.validTarget(skillType))
        {
            return false;
        }

        if (!activated)
        {
            return true;
        }

        Vector3Int playerCell = AreaManager.getMasterGrid().WorldToCell(PlayerMovement.getInstanceTransform().position);

        foreach (Vector3Int coords in blockerCoords)
        {
            if (playerCell.x == coords.x && playerCell.y == coords.y)
            {
                return false;
            }
        }

        return true;
    }

    public override void setStatus(string key, bool status)
    {
        if (!getKey().Equals(getKey()))
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
        foreach(GameObject blocker in blockers)
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
}
