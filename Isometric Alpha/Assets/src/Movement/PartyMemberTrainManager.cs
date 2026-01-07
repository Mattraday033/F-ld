using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PartyMemberTrainManager
{
    public static int stepCounter;
    public static List<PartyMemberMovement> partyMemberTrain;

    [RuntimeInitializeOnLoadMethod]
    private static void initializePartyMemberTrainManager()
    {
        TransitionManager.AfterTransition.AddListener(createPartyMemberTrain);
        MovementManager.OnMoveFinished.AddListener(incrementStepCounter);
        MovementManager.OnMoveFinished.AddListener(hideOverlappingPartyMembersOnMoveEnded);
        MovementManager.OnMoveStarted.AddListener(showPartyMemberTrain);
        partyMemberTrain = new List<PartyMemberMovement>();
        stepCounter = 1;
    }

    public static void createPartyMemberTrain()
    {
        stepCounter = 1;
        destroyPartyMemberTrain();

        if(AreaManager.locationName == null || AreaManager.locationName.Length == 0 || AreaList.currentAreaIsHostile())
        {
            return;
        }

        List<PartyMember> formationPartyMembers = PartyManager.getAllPartyMembersInTrain();

        PartyMemberMovement previousLinkInTrain = null;

        int index = 0;
        foreach(PartyMember partyMember in formationPartyMembers)
        {
            PartyMemberMovement partyMemberMovement = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.partyMemberFollower), AreaManager.getPlayerParent()).GetComponent<PartyMemberMovement>();
            
            partyMemberMovement.partyMember = partyMember;
            partyMemberMovement.placeInTrain = index+1;
            
            partyMemberMovement.getAnimationManager().setAnimations(partyMemberMovement.getName());

            partyMemberMovement.getAnimationManager().setFacing(State.playerFacing.getFacing());

            partyMemberMovement.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(PlayerMovement.getInstance().getCell());

            partyMemberTrain.Add(partyMemberMovement);

            if(index == 0)
            {
                PlayerMovement.setNextInTrain(partyMemberMovement);
            } else
            {
                previousLinkInTrain.nextInTrain = partyMemberMovement;
            }

            previousLinkInTrain = partyMemberMovement;
            index++;
        }

        hidePartyMemberTrain();
    }

    public static void incrementStepCounter(int movementIndex)
    {
        if(movementIndex == MovementManager.playerSpriteIndex)
        {
            stepCounter++;
        }
    }

	public static void destroyPartyMemberTrain()
	{
		if (partyMemberTrain == null || partyMemberTrain is null)
		{
			return;
		}

		foreach (PartyMemberMovement partyMemberMovement in partyMemberTrain)
		{
			if (partyMemberMovement != null)
			{
				GameObject.Destroy(partyMemberMovement.gameObject);
			}
		}

		partyMemberTrain = new List<PartyMemberMovement>();
	}

	public static void hidePartyMemberTrain()
	{
        foreach(PartyMemberMovement partyMemberMovement in partyMemberTrain)
        {
            partyMemberMovement.hideSprite();
        }
	}
	
	public static void showPartyMemberTrain()
	{
        foreach(PartyMemberMovement partyMemberMovement in partyMemberTrain)
        {
            if(stepCounter < partyMemberMovement.placeInTrain)
            {
                continue;
            }

            partyMemberMovement.showSprite();
        }
	}

	public static void hideOverlappingPartyMembers()
	{
        foreach(PartyMemberMovement partyMemberMovement in partyMemberTrain)
        {
            Vector3Int cell = partyMemberMovement.getCell();

            if(cell.Equals(PlayerMovement.getInstance().getCell()))
            {
                partyMemberMovement.hideSprite();
                continue;
            }

            foreach(PartyMemberMovement otherPartyMember in partyMemberTrain)
            {
                if(cell.Equals(otherPartyMember.getCell()) && !otherPartyMember.partyMember.Equals(partyMemberMovement.partyMember))
                {
                    MovementTracker.determineLowestTrainPriority(partyMemberMovement, otherPartyMember).hideSprite();
                    break;
                }
            }
        }
	}

    public static void hideOverlappingPartyMembersOnMoveEnded(int index)
    {
        if(index != MovementManager.playerSpriteIndex)
        {
            return;
        }

        hideOverlappingPartyMembers();
    }
}
