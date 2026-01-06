using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoubleCunningBlocker : CunningBlocker
{
    private bool addedFirstSetOfBlockers = false;

    public List<Obstacle> deactivatedBlockers = new List<Obstacle>();
    public List<Vector3Int> deactivatedBlockerCoords = new List<Vector3Int>();

    public override void addBlocker(Obstacle blocker, Vector3Int blockerCoords)
    {
        if(!addedFirstSetOfBlockers)
        {
            base.addBlocker(blocker, blockerCoords);
            return;
        } else
        {
            deactivatedBlockers.Add(blocker);
            this.deactivatedBlockerCoords.Add(blockerCoords);
        }
    }

    public override bool validTarget(SkillType skillType)
    {
        if (!base.validTarget(skillType))
        {
            return false;
        }

        Vector3Int playerCell = AreaManager.getMasterGrid().WorldToCell(PlayerObject.getInstanceTransform().position);

        foreach (Vector3Int coords in deactivatedBlockerCoords)
        {
            if (playerCell.x == coords.x && playerCell.y == coords.y)
            {
                return false;
            }
        }

        return true;
    }

    public override void cunning(bool trackChangeInStateManager)
    {
        base.cunning(trackChangeInStateManager);

        killEverythingInArea(deactivatedBlockerCoords);
    }

    public override void setBlockerStatus()
    {
        base.setBlockerStatus();

        foreach (Obstacle blocker in deactivatedBlockers)
        {
            if (!activated)
            {
                blocker.setToDown();
            }
            else
            {
                blocker.setToUp();
            }
        }

        addedFirstSetOfBlockers = true;
    }
}
