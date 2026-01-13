using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartyMemberPlacer : MonoBehaviour
{
	public static List<GameObject> placedPartyMemberObjects = new List<GameObject>();

    public readonly static UnityEvent DestroyAllFollowers = new UnityEvent();

    public readonly static UnityEvent HideAllFollowers = new UnityEvent();
    public readonly static UnityEvent RevealAllFollowers = new UnityEvent();

	public static PartyMemberPlacer instance;

    [RuntimeInitializeOnLoadMethod]
    private static void instantiatePartyMemberPlacer()
    {
        placedPartyMemberObjects = new List<GameObject>();

        instance = null;
    }

    private void Awake()
    {
        instance = this;
    }

	public static void placeAllPartyMembers()
    {
        DestroyAllFollowers.Invoke();
		placedPartyMemberObjects = new List<GameObject>();

		List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();

        foreach (PartyMember partyMember in allPartyMembers)
        {
            if (partyMember.placed)
            {
                placeNextPartyMember(partyMember.getName());
            }
        }
	}

	public static PartyMemberPlacer getInstance()
	{
		return instance;
	}

	public static void placeNextPartyMember()
	{
        string nameOfPartyMember = findNextPlaceablePartyMember();

        placeNextPartyMember(nameOfPartyMember);
	}

	public static void placeNextPartyMember(string nameOfPartyMember)
	{
		if (nameOfPartyMember == null)
		{
			return;
		}

        GameObject placedPartyMember = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.placedPartyMember), AreaManager.getNPCParent());

        PartyMemberTrainPriority trainPriority = placedPartyMember.GetComponent<PartyMemberTrainPriority>();
        trainPriority.partyMemberName = nameOfPartyMember;

        OOCSpawnDetails.addTutorialTargetComponent(placedPartyMember, TutorialSequenceList.placedCharacterTargetHash);

        if (PartyManager.getPartyMember(nameOfPartyMember).placed)
        {
            placedPartyMember.transform.position = PartyManager.getPartyMember(nameOfPartyMember).placedPosition;
            Helpers.updateGameObjectPosition(placedPartyMember);
        }
        else
        {
            placedPartyMember.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(SkillManager.getPlayerCoords());
            Helpers.updateGameObjectPosition(placedPartyMember);

            PartyManager.getPartyMember(nameOfPartyMember).placed = true;
            PartyManager.getPartyMember(nameOfPartyMember).placedPosition = placedPartyMember.transform.position;
        }        

        placedPartyMemberObjects.Add(placedPartyMember);
        
        SkillManager.OnSkillUse.Invoke();
	}

    private static string findNextPlaceablePartyMember()
    {
        int skippedPartyMembers = 0;

        List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();

        foreach (PartyMember partyMember in allPartyMembers)
        {
            if (partyMember.isInParty() && skippedPartyMembers == placedPartyMemberObjects.Count)
            {
                return partyMember.getName();
            }
            else if (partyMember.isInParty() && skippedPartyMembers != placedPartyMemberObjects.Count)
            {
                skippedPartyMembers++;
            }
        }

        return null;
    }

	public static void removePlacedPartyMember(string targetPartyMemberName)
	{
		PartyManager.getPartyMember(targetPartyMemberName).placed = false;
		PartyManager.getPartyMember(targetPartyMemberName).placedPosition = Vector3.zero;

		for (int partyMemberIndex = 0; partyMemberIndex < placedPartyMemberObjects.Count; partyMemberIndex++)
		{
			GameObject currentPartyMember = (GameObject)placedPartyMemberObjects[partyMemberIndex];

			if (currentPartyMember.GetComponent<PartyMemberTrainPriority>().partyMemberName.Equals(targetPartyMemberName))
			{
				GameObject.Destroy(currentPartyMember);
                placedPartyMemberObjects.RemoveAt(partyMemberIndex);
                MovementManager.OnMoveFinished.Invoke(Constants.indexZero);
			}
		}
	}

    [RuntimeInitializeOnLoadMethod]
    private static void addListener()
    {
        TransitionManager.BeforeTransition.AddListener(removeAllPlacedPartyMembers);
        TransitionManager.AfterTransition.AddListener(placeAllPartyMembers);
    }

    public static void removeAllPlacedPartyMembers()
    {
        List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();

        foreach (PartyMember partyMember in allPartyMembers)
        {
            partyMember.placed = false;
            partyMember.placedPosition = Vector3.zero;
        }

        DestroyAllFollowers.Invoke();
        MovementManager.OnMoveFinished.Invoke(Constants.indexZero);

        placedPartyMemberObjects = new List<GameObject>();
    }

	public static int getPlacedPartyMemberCount()
	{
		return placedPartyMemberObjects.Count;
	}
    
}
