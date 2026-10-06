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

    //puts back the party members still flagged as placed once an area has spawned, as after loading a save or returning from combat.
    //silent, because nothing was placed by the player: the area spawn builds the train and the UI straight afterwards
	public static void restorePlacedPartyMembers()
    {
		placedPartyMembers = new List<PlacedPartyMember>();

		List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();

        foreach (PartyMember partyMember in allPartyMembers)
        {
            if (partyMember.placed)
            {
                spawnPlacedPartyMember(partyMember, AreaManager.getMasterGrid().WorldToCell(partyMember.placedPosition));
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

        //always the player's cell: a placed flag left over from before is no reason to send a new placement to its old position
        spawnPlacedPartyMember(PartyManager.getPartyMember(nameOfPartyMember), SkillManager.getPlayerCoords());

        SkillManager.OnSkillUse.Invoke();
        OnPartyMemberPlaced.Invoke();
	}

    //the spawned GameObject must not be toggled off and on afterwards, as that stops the coroutines its spawn behaviours are still waiting on
    private static void spawnPlacedPartyMember(PartyMember partyMember, Vector3Int cell)
    {
        GameObject placedPartyMemberObject = new PlacedPartyMemberSpawnDetails(partyMember, cell).spawnInteractables()[0];

        partyMember.placed = true;
        partyMember.placedPosition = placedPartyMemberObject.transform.position;

        placedPartyMembers.Add(placedPartyMemberObject.GetComponent<PlacedPartyMember>());
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
            return placablePartyMembers[placedPartyMembers.Count].uniqueName;
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

			if (currentPartyMember.GetComponent<PlacedPartyMember>().partyMember.uniqueName.Equals(targetPartyMemberName))
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
