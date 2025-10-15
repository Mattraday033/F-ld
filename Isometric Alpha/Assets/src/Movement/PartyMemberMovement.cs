using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberMovement : MonoBehaviour
{

	public static Transform[] partyMemberTrain;
	public static int stepCounter = 0;

	public static void instantiatePartyMemberTrain()
	{
        return; //turning off party member movement

		Transform player = PlayerMovement.getTransform();

		if (player == null)
		{
			return;
		}

		destroyPartyMemberTrain();

		if (AreaList.currentSceneIsHostile() || Flags.shouldStopPartyTrainSpawning())
		{
			return;
		}
		ArrayList partyMembersInFormation = new ArrayList();
		List<string> trainGameObjectNames = PartyManager.getAllGameObjectNamesInTrain();

		foreach (string objectName in trainGameObjectNames)
		{
			partyMembersInFormation.Add(Instantiate(Resources.Load<GameObject>(objectName), player.transform.parent).transform);
		}

		if (partyMembersInFormation.Count <= 0)
		{
			return;
		}

		partyMemberTrain = new Transform[partyMembersInFormation.Count];

		for (int formationIndex = (partyMemberTrain.Length - 1); formationIndex >= 0; formationIndex--)
		{
			partyMemberTrain[formationIndex] = (Transform)partyMembersInFormation[formationIndex];
			partyMemberTrain[formationIndex].GetComponent<Collider2D>().enabled = false;
		}

		foreach (Transform partyMember in partyMemberTrain)
		{
			partyMember.position = MovementManager.getGrid().CellToWorld(SkillManager.getPlayerCoords());
			setPartyMemberSpriteEnabled(partyMember, false);
		}
	}

	public static void hideOverlappingPartyMembers()
	{
		if (AreaList.currentSceneIsHostile() || partyMemberTrain == null || partyMemberTrain is null)
		{
			return;
		}

		for (int partyMemberIndex = 0; partyMemberIndex < partyMemberTrain.Length; partyMemberIndex++)
		{
			Transform currentPartyMember = partyMemberTrain[partyMemberIndex];
			Vector3Int currentPartyMemberCell = MovementManager.getCellWorld(currentPartyMember.position);

			if (currentPartyMemberCell.Equals(MovementManager.getCellWorld(PlayerMovement.getInstance().transform.position)))
			{
				setPartyMemberSpriteEnabled(currentPartyMember, false);
			}
			else
			{
				for (int previousPartyMemberIndex = (partyMemberIndex - 1); previousPartyMemberIndex >= 0; previousPartyMemberIndex--)
				{
					Transform previousPartyMember = partyMemberTrain[previousPartyMemberIndex];

					Vector3Int previousPartyMemberCell = MovementManager.getCellWorld(previousPartyMember.position);

					if (previousPartyMemberCell.Equals(currentPartyMemberCell))
					{
						setPartyMemberSpriteEnabled(currentPartyMember, false);
					}
				}
			}
		}
	}

	private static void setPartyMemberSpriteEnabled(Transform partyMemberTransform, bool isEnabled)
	{
		PartyMemberTrainPriority trainPriority = partyMemberTransform.GetComponent<PartyMemberTrainPriority>();

		if (trainPriority != null && trainPriority.sprite != null)
		{
			trainPriority.sprite.enabled = isEnabled;
		}
	}

	public static void showAllPartyMembers()
	{
		if (AreaList.currentSceneIsHostile() || partyMemberTrain == null || partyMemberTrain is null)
		{
			return;
		}

		for (int trainIndex = (partyMemberTrain.Length - 1); trainIndex >= 0; trainIndex--)
		{
			setPartyMemberSpriteEnabled(partyMemberTrain[trainIndex], true);
		}
	}

	public static void destroyPartyMemberTrain()
	{
		if (partyMemberTrain == null || partyMemberTrain is null)
		{
			return;
		}

		foreach (Transform npc in partyMemberTrain)
		{
			if (npc != null)
			{
				GameObject.Destroy(npc.gameObject);
			}
		}

		partyMemberTrain = new Transform[0];
	}

	public static void hidePartyMemberTrain()
	{
		if (partyMemberTrain == null || partyMemberTrain is null)
		{
			return;
		}

		foreach (Transform npc in partyMemberTrain)
		{
			if (npc != null)
			{
				npc.gameObject.SetActive(false);
			}
		}
	}
	
	public static void showPartyMemberTrain()
	{
		if(partyMemberTrain == null || partyMemberTrain is null)
		{
			return;
		}
		
		foreach(Transform npc in partyMemberTrain)
		{	
			if(npc != null)
			{
				npc.gameObject.SetActive(true);
			}
		}
	}
}
