using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartyMemberPlacer : MonoBehaviour
{
	public static ArrayList placedPartyMemberObjects = new ArrayList();

	public static UnityEvent DestroyAllFollowersEvent = new UnityEvent();

	public static PartyMemberPlacer instance;

	private void Awake()
	{
		instance = this;
	}

	void Start()
	{
		placedPartyMemberObjects = new ArrayList();

		Transform playerTransform = PlayerMovement.getInstance().gameObject.transform;

		List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();

		foreach (PartyMember partyMember in allPartyMembers)
		{
			if (partyMember.placed)
			{
				GameObject partyMemberObject = Instantiate(Resources.Load<GameObject>(partyMember.overWorldGameObjectName),
												playerTransform.parent);

				partyMemberObject.transform.localPosition = partyMember.placedPosition;

				Helpers.updateGameObjectPosition(partyMemberObject);

				placedPartyMemberObjects.Add(partyMemberObject);
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
		Transform playerTransform = PlayerMovement.getInstance().gameObject.transform;

		if (nameOfPartyMember == null)
		{
			return;
		}

		GameObject placedPartyMember = GameObject.Instantiate(Resources.Load<GameObject>(PartyManager.getPartyMember(nameOfPartyMember).overWorldGameObjectName),
																playerTransform.parent);
		placedPartyMember.transform.position = MovementManager.getGrid().CellToWorld(SkillManager.getPlayerCoords());
		Helpers.updateGameObjectPosition(placedPartyMember);

		PartyManager.getPartyMember(nameOfPartyMember).placed = true;
		PartyManager.getPartyMember(nameOfPartyMember).placedPosition = placedPartyMember.transform.localPosition;

		placedPartyMemberObjects.Add(placedPartyMember);
		OOCUIManager.updateOOCUI();
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
			}
		}
	}

	public static void removeAllPlacedPartyMembers()
	{
		List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();

		foreach (PartyMember partyMember in allPartyMembers)
		{
			partyMember.placed = false;
			partyMember.placedPosition = Vector3.zero;
		}

		DestroyAllFollowersEvent.Invoke();

		placedPartyMemberObjects = new ArrayList();
	}

	public static int getPlacedPartyMemberCount()
	{
		return placedPartyMemberObjects.Count;
	}
}
