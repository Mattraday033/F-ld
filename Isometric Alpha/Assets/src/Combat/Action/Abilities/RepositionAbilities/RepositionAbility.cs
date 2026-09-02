using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class RepositionAbility : Ability, IJSONConvertable
{
    private GameObject placeHolderObject; //Object showing where the actor is repositioning to
    private Stats statsClone;
    private GridCoords secondaryCoords;

    public GridCoords tertiaryCoords;

    public RepositionAbility(CombatActionSettings settings) :
        base(settings)
    {
    }

    public override void performCombatAction()
    {
        base.performCombatAction();

        if (combatantToBeMovedExists(out Stats combatantToBeMoved) && 
            getActorStats() != null)
        {
            return;
        }

        CombatAnimationManager.getInstance().StartCoroutine(waitForAttackAnimationToStop(getActorStats().animationManager, combatantToBeMoved));
    }

    public virtual IEnumerator waitForAttackAnimationToStop(AnimationManager animationManager, Stats combatantToBeMoved)
    {

        while(CombatAnimationManager.trackerBeingTracked(animationManager))
        {
            yield return null;
        }

        combatantToBeMoved.moveTo(new List<GridCoords> { getDestinationCoords() });

        applyTrait(combatantToBeMoved);

        if (!inPreviewMode && actorIsAlly())
        {
            Exuberances.addExuberance(MultiStackProcType.BlueShield, singleExuberanceStack);
        }
    }

    public override void queueingAction()
    {

        if(!combatantToBeMovedExists(out Stats combatantToBeMoved))
        {
            return;
        }

        if (combatantToBeMoved.repositionClone != null)
        {
            statsClone = combatantToBeMoved.repositionClone;
            return;
        }

        if (!combatantToBeMoved.isInsideCoordinates(getDestinationCoords()))
        {
            setStatsClone(combatantToBeMoved.clone());

            statsClone.positions = new List<GridCoords> { getDestinationCoords().clone() };
            statsClone.addTrait(TraitList.repositioningInvulnerability);
            statsClone.inPreviewMode = true;
            statsClone.addTrait(getAppliedTrait());
            statsClone.inPreviewMode = false;
            statsClone.addTrait(TraitList.untargetable);

            foreach (GridCoords cloneCoords in statsClone.positions)
            {
                CombatGrid.setCombatantAtCoords(cloneCoords, statsClone);
            }
            combatantToBeMoved.repositionClone = statsClone;

            placeHolderObject = RepositionPlaceholderGenerator.generatePlaceholderObject(statsClone, getDestinationCoords());
        }
        else
        {
            placeHolderObject = null;
        }

        if (targetsAllySection())
        {
            CombatActionManager.getInstance().promptLaterCombatActionsToFindNewTarget();
        }
    }

    public override void activatingAction()
    {
        base.activatingAction();

        if (combatantToBeMovedExists(out Stats combatantToBeMoved) && 
            !combatantToBeMoved.isInsideCoordinates(getDestinationCoords()))
        {
            CombatGrid.setCombatantAtCoords(getDestinationCoords(), null);
        }

        setStatsClone(null);

        destroyPlaceHolderObject();
    }

    public override void unqueueingAction()
    {
        if (combatantToBeMovedExists(out Stats combatantToBeMoved) && 
            !statsClone.positions.Any(p => combatantToBeMoved.positions.Contains(p)))
        {
            foreach (GridCoords cloneCoords in statsClone.positions)
            {
                CombatGrid.setCombatantAtCoords(cloneCoords, null);
            }
        }

        setStatsClone(null);

        CombatActionManager.getInstance().promptLaterCombatActionsToReturnToPreviousTarget();

        destroyPlaceHolderObject();
    }

    public virtual bool combatantToBeMovedExists(out Stats combatant)
    {
        return CombatGrid.combatantExistsAtCoords(getSecondaryCoords(), out combatant);
    }

    public virtual GridCoords getDestinationCoords()
    {
        return tertiaryCoords;
    }

    public override void setTertiaryCoords(GridCoords coords)
    {
        tertiaryCoords = coords.clone();
    }

    public override GridCoords getSecondaryCoords()
    {
        return secondaryCoords;
    }

    public override Vector3 getTertiaryPosition()
    {
        return CombatGrid.getPositionAt(tertiaryCoords);
    }

    public override void setSecondaryCoords(GridCoords coords)
    {
        secondaryCoords = coords.clone();
    }

    public override bool secondaryCoordsRequiresEmptySpace()
    {
        return true;
    }

    public override bool requiresSecondaryCoords()
    {
        return true;
    }

    public void setPlaceHolderObject(GameObject placeHolderObject)
    {
        this.placeHolderObject = placeHolderObject;
    }

    public GameObject getPlaceHolderObject()
    {
        return placeHolderObject;
    }

    public void destroyPlaceHolderObject()
    {
        if (placeHolderObject != null)
        {
            GameObject.Destroy(placeHolderObject);
        }

        if (combatantToBeMovedExists(out Stats combatantToBeMoved) &&
            combatantToBeMoved.repositionClone != null &&
            combatantToBeMoved.repositionClone.positions.Any(p => combatantToBeMoved.positions.Contains(p)))
        {
            combatantToBeMoved.repositionClone = combatantToBeMoved.repositionClone.repositionClone;
        }
        else if (combatantToBeMoved != null)
        {
            combatantToBeMoved.repositionClone = null;
        }
    }

    public void setStatsClone(Stats statsClone)
    {
        this.statsClone = statsClone;
    }

    public bool hasStatsClone()
    {
        return hasStatsClone(out Stats clone);
    }

    public bool hasStatsClone(out Stats clone)
    {
        clone = statsClone;
        return clone != null;
    }

    public override bool tertiaryCoordsRequiresEmptySpace()
    {
        return true;
    }

    public override bool requiresTertiaryCoords()
    {
        return true;
    }
}
