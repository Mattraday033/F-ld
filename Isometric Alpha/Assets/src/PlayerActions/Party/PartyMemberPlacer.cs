using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartyMemberPlacer : MonoBehaviour
{
	public static List<PlacedPartyMember> placedPartyMembers = new List<PlacedPartyMember>();

    public readonly static UnityEvent DestroyAllFollowers = new UnityEvent();

    public readonly static UnityEvent HideAllFollowers = new UnityEvent();
    public readonly static UnityEvent RevealAllFollowers = new UnityEvent();

    public readonly static UnityEvent OnPartyMemberPlaced = new UnityEvent();
    public readonly static UnityEvent OnPartyMemberRemoved = new UnityEvent();

	public static PartyMemberPlacer instance;

    [RuntimeInitializeOnLoadMethod]
    private static void instantiatePartyMemberPlacer()
    {
        placedPartyMembers = new List<PlacedPartyMember>();

        instance = null;
    }

    private void Awake()
    {
        instance = this;
    }

	public static void placeAllPartyMembers()
    {
        DestroyAllFollowers.Invoke();
		placedPartyMembers = new List<PlacedPartyMember>();

		List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();

        foreach (PartyMember partyMember in allPartyMembers)
        {
            if (partyMember.placed)
            {
                placeNextPartyMember(partyMember.getName());
            }
        }

        OnPartyMemberPlaced.Invoke();
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

        GameObject placedPartyMemberObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.placedPartyMember), AreaManager.getNPCParentWithoutScale());

        OOCSpawnDetails.addTutorialTargetComponent(placedPartyMemberObject, TutorialSequenceList.placedCharacterTargetHash);

        if (PartyManager.getPartyMember(nameOfPartyMember).placed)
        {
            placedPartyMemberObject.transform.position = PartyManager.getPartyMember(nameOfPartyMember).placedPosition;
            Helpers.updateGameObjectPosition(placedPartyMemberObject);
        }
        else
        {
            placedPartyMemberObject.transform.position = AreaManager.getMasterGrid().GetCellCenterWorld(SkillManager.getPlayerCoords());
            Helpers.updateGameObjectPosition(placedPartyMemberObject);

            PartyManager.getPartyMember(nameOfPartyMember).placed = true;
            PartyManager.getPartyMember(nameOfPartyMember).placedPosition = placedPartyMemberObject.transform.position;
        }        

        PlacedPartyMember placedPartyMember = placedPartyMemberObject.GetComponent<PlacedPartyMember>();

        placedPartyMember.partyMember = PartyManager.getPartyMember(nameOfPartyMember);

        placedPartyMembers.Add(placedPartyMember);
        
        SkillManager.OnSkillUse.Invoke();
        OnPartyMemberPlaced.Invoke();
	}

    public static bool hasBeenPlaced(PartyMember partyMember)
    {
        foreach(PlacedPartyMember placedPartyMember in placedPartyMembers)
        {
            if(placedPartyMember.partyMember.Equals(partyMember))
            {
                return true;
            }
        }

        return false;
    }

    private static string findNextPlaceablePartyMember()
    {
        List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();
        List<PartyMember> placablePartyMembers = new List<PartyMember>();

        allPartyMembers.Remove(PartyManager.getPlayer());

        foreach (PartyMember partyMember in allPartyMembers)
        {
            if (partyMember.isInParty())
            {
                placablePartyMembers.Insert(Constants.indexZero, partyMember);
            } else if(partyMember.canJoinParty)
            {
                placablePartyMembers.Add(partyMember);
            }
        }

        if(placablePartyMembers.Count > placedPartyMembers.Count)
        {
            return placablePartyMembers[placedPartyMembers.Count].getName();
        } else
        {
            return null;
        }
    }

	public static void removePlacedPartyMember(string targetPartyMemberName)
	{
		PartyManager.getPartyMember(targetPartyMemberName).placed = false;
		PartyManager.getPartyMember(targetPartyMemberName).placedPosition = Vector3.zero;

		for (int partyMemberIndex = 0; partyMemberIndex < placedPartyMembers.Count; partyMemberIndex++)
		{
			GameObject currentPartyMember = placedPartyMembers[partyMemberIndex].gameObject;

			if (currentPartyMember.GetComponent<PlacedPartyMember>().partyMember.getName().Equals(targetPartyMemberName))
			{
				GameObject.Destroy(currentPartyMember);
                placedPartyMembers.RemoveAt(partyMemberIndex);
                MovementManager.OnMoveFinished.Invoke(Constants.indexZero);
			}
		}

        OnPartyMemberRemoved.Invoke();
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

        placedPartyMembers = new List<PlacedPartyMember>();
    }

	public static int getPlacedPartyMemberCount()
	{
		return placedPartyMembers.Count;
	}
    
}
