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
        MovementManager.BeforeMoveStarted.AddListener(showPartyMemberTrain);

        PlayerOOCStateManager.OnStateChangeToInDialogue.AddListener(createPartyMemberTrain);
        PlayerOOCStateManager.OnStateChangeFromInDialogue.AddListener(destroyPartyMemberTrainIfAreaIsHostile);

        PartyMemberPlacer.OnPartyMemberPlaced.AddListener(createPartyMemberTrain);
        PartyMemberPlacer.OnPartyMemberRemoved.AddListener(createPartyMemberTrain);

        Formation.OnFormationChange.AddListener(createPartyMemberTrain);

        partyMemberTrain = new List<PartyMemberMovement>();
        stepCounter = 1;
    }

    public static void createPartyMemberTrain()
    {
        stepCounter = 1;
        destroyPartyMemberTrain();

        if(string.IsNullOrEmpty(AreaManager.locationName) || 
            AreaList.currentAreaIsHostile() || 
            AreaManager.getPlayerParent() == null ||
            cannotCreateTrainInArea())
        {
            return;
        }

        List<PartyMember> formationPartyMembers = PartyManager.getAllPartyMembersInTrain();

        PartyMemberMovement previousLinkInTrain = null;

        int index = 0;
        foreach(PartyMember partyMember in formationPartyMembers)
        {
            if(PartyMemberPlacer.hasBeenPlaced(partyMember))
            {
                continue;
            }

            //the spawn details add the PartyMemberMovement through PartyMemberMovementSpawnBehaviour and place it on the player's cell
            GameObject follower = new PartyMemberTrainSpawnDetails(partyMember, index+1).spawnInteractables()[0];
            PartyMemberMovement partyMemberMovement = follower.GetComponent<PartyMemberMovement>();

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

    public static void destroyPartyMemberTrainIfAreaIsHostile()
    {
        if(AreaList.currentAreaIsHostile())
        {
            destroyPartyMemberTrain();
        }
    }

    public static bool cannotCreateTrainInArea()
    {
        switch(AreaManager.locationName)
        {
            case LocationNameList.slaveShackFour:
            case LocationNameList.slaveShackFive:
            case LocationNameList.slaveShackSix:
            case LocationNameList.messHall:
            case LocationNameList.campSouthEast:
                return SpawnParamsList.getSpawnParams(LocationNameList.campSouthEast, NPCNameList.thatch).canSpawn(NPCNameList.thatch);
            default:
                return false;
        }
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
        // foreach(PartyMemberMovement partyMemberMovement in partyMemberTrain)
        // {
        //     Vector3Int cell = partyMemberMovement.getCell();

        //     if(cell.Equals(PlayerMovement.getInstance().getCell()))
        //     {
        //         partyMemberMovement.hideSprite();
        //         continue;
        //     }

        //     foreach(PartyMemberMovement otherPartyMember in partyMemberTrain)
        //     {
        //         if(cell.Equals(otherPartyMember.getCell()) && !otherPartyMember.partyMember.Equals(partyMemberMovement.partyMember))
        //         {
        //             MovementTracker.determineLowestTrainPriority(partyMemberMovement, otherPartyMember).hideSprite();
        //             break;
        //         }
        //     }
        // }
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
