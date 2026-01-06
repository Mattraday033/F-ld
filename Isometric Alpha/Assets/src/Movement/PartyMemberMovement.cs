using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberMovement : MovementTracker
{
    public PartyMember partyMember;
    public AnimationManager animationManager;

    public int placeInTrain = -1;

	public override string getName()
	{
        return partyMember.getName();
	}

    public override int getMovementIndex()
    {
        return -1;
    }

    public override bool isMoving()
    {
        return (PlayerMovement.getInstance().isMoving() || KeyBindingList.movementKeyPressed()) && 
                canMoveInTrain() && !PlayerMovement.getInstance().directionMod.Equals(Vector3Int.zero);
    }

    public override AnimationManager getAnimationManager()
    {
        return animationManager;
    }

	public override CharacterFacing getCharacterFacing()
	{
        return animationManager.facing;
	}

    public override bool canMoveInTrain()
    {
        return placeInTrain > 0 && (PartyMemberTrainManager.stepCounter >= placeInTrain);
    }

    public override int getPlaceInTrain()
    {
        return placeInTrain;
    }
}
