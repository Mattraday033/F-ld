using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//an INameSource so the ActivationListener beside it knows which party member's formation changes to watch for
public class PartyMemberMovement : MovementTracker, INameSource
{
    public PartyMember partyMember;

    public int placeInTrain = -1;

	public override string displayName { get { return partyMember.displayName; } }
	public override string uniqueName { get { return partyMember.uniqueName; } }

    public override int getMovementIndex()
    {
        return -1;
    }

    public override bool isMoving()
    {
        return (PlayerMovement.getInstance().isMoving() || KeyBindingList.movementKeyPressed()) && 
                canMoveInTrain() && !PlayerMovement.getInstance().directionMod.Equals(Vector3Int.zero);
    }

    public override bool canMoveInTrain()
    {
        return placeInTrain > 0 && (PartyMemberTrainManager.stepCounter >= placeInTrain);
    }

    public override int getPlaceInTrain()
    {
        return placeInTrain;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PlacedPartyMember.PartyMemberLocationRequest.AddListener(addToList);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        PlacedPartyMember.PartyMemberLocationRequest.RemoveListener(addToList);
    }
}
