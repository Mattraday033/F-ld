using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CunningBlocker : CunningObject
{
    public List<Obstacle> blockers = new List<Obstacle>();
    public List<Vector3Int> blockerCoords = new List<Vector3Int>();

    public virtual void addBlocker(Obstacle blocker, Vector3Int blockerCoords)
    {
        blockers.Add(blocker);
        this.blockerCoords.Add(blockerCoords);
    }

    private void OnDisable()
    {
        if(SpawnInfoManager.wipingSlate)
        {
            return;
        }

        foreach(Obstacle blocker in blockers)
        {
            blocker.setToDown();
        }
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

        Vector3Int playerCell = AreaManager.getMasterGrid().WorldToCell(PlayerObject.getInstanceTransform().position);

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

        if (!getKey().Equals(key))
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

        if(!activated)
        {
            killEverythingInArea(blockerCoords);
        }

        if (trackChangeInStateManager)
        {
            trackKey();
        }
    }

    public static void killEverythingInArea(List<Vector3Int> blockerCoords)
    {
        foreach(Vector3Int coord in blockerCoords)
        {
            foreach(MovementTracker movementTracker in MovementManager.allMovementTrackers)
            {
                if(movementTracker != PlayerMovement.getInstance() &&
                    movementTracker != null &&
                    movementTracker.getAnimationManager() != null &&
                    movementTracker.getCell().Equals(coord))
                {
                    movementTracker.getAnimationManager().playDeathAnimationThenHide();
                    EnemyMovement enemyMovement = movementTracker as EnemyMovement;

                    if(enemyMovement != null)
                    {
                        enemyMovement.setToDefeated();
                    }
                }
            }
        }
    }

    public virtual void setBlockerStatus()
    {
        foreach (Obstacle blocker in blockers)
        {
            if (activated)
            {
                blocker.setToDown();
            }
            else
            {
                blocker.setToUp();
            }
        }
    }
}
