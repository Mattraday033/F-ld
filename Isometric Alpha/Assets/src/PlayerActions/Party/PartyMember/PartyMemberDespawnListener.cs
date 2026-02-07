using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartyMemberDespawnListener : MonoBehaviour
{
    public string partyMemberName;

    private void OnEnable()
    {
        Formation.OnFormationChange.AddListener(checkToDespawn);
    }

    private void OnDisable()
    {
        Formation.OnFormationChange.RemoveListener(checkToDespawn);
    }

    private void checkToDespawn()
    {
        if(partyMemberName == null)
        {
            return;
        }

        if(State.formation.contains(partyMemberName))
        {
            gameObject.SetActive(false);
        }
    }
}
