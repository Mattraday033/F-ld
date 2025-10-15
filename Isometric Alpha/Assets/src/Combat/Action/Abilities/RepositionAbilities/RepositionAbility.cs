using System.Collections;
using System.Collections.Generic;
using System.IO;
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

		Stats combatantToBeMoved = getCombatantToBeMoved();

		if (combatantToBeMoved == null)
		{
			return;
		}

		combatantToBeMoved.moveTo(getDestinationCoords());

		applyTrait(combatantToBeMoved);

		if (!inPreviewMode && actorIsAlly())
		{
			Exuberances.addExuberance(MultiStackProcType.BlueShield, singleExuberanceStack);
		}
	}

	public override void queueingAction()
	{
		Stats combatantToBeMoved = getCombatantToBeMoved();
		placeHolderObject = RepositionPlaceholderGenerator.generatePlaceholderObject(combatantToBeMoved, getDestinationCoords());

		statsClone = getCombatantToBeMoved().clone();
		statsClone.position = getDestinationCoords().clone();
		statsClone.addTrait(TraitList.repositioningInvulnerability);
		statsClone.addTrait(getAppliedTrait());
		statsClone.addHiddenTrait(TraitList.untargetable);

		GridCoords tarCoords = getTargetCoords();

		if (!combatantToBeMoved.position.Equals(getDestinationCoords()))
		{
			CombatGrid.setCombatantAtCoords(statsClone.position, statsClone);
		}

		if (targetsAllySection())
		{
			CombatActionManager.getInstance().promptLaterCombatActionsToFindNewTarget();
		}
	}

	public override void activatingAction()
	{
		base.activatingAction();

		if (getCombatantToBeMoved() != null && !getCombatantToBeMoved().position.Equals(getDestinationCoords()))
		{
			CombatGrid.setCombatantAtCoords(getDestinationCoords(), null);
		}

		setStatsClone(null);

		destroyPlaceHolderObject();
	}

	public override void unqueueingAction()
	{
		if (!statsClone.position.Equals(getCombatantToBeMoved().position))
		{
			CombatGrid.setCombatantAtCoords(statsClone.position, null);
		}

		setStatsClone(null);

		CombatActionManager.getInstance().promptLaterCombatActionsToReturnToPreviousTarget();

		destroyPlaceHolderObject();
	}

	public virtual Stats getCombatantToBeMoved()
	{
		return CombatGrid.getCombatantAtCoords(getSecondaryCoords());
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
		return CombatGrid.fullCombatGrid[tertiaryCoords.row][tertiaryCoords.col]; 
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
		GameObject.Destroy(placeHolderObject);
	}

	public void setStatsClone(Stats statsClone)
	{
		this.statsClone = statsClone;
	}

	public Stats getStatsClone()
	{
		return statsClone;
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
