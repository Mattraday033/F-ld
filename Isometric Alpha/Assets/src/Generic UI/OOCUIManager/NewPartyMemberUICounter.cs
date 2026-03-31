using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class NewPartyMemberUICounter : MonoBehaviour
{
    private void Awake()
    {
        NewPartyMemberManager.PartyMemberMarkedAsNew.AddListener(determineVisibility);
        NewPartyMemberManager.PartyMemberNoLongerNew.AddListener(determineVisibility);

        determineVisibility();
    }

    private void OnDestroy()
    {
        NewPartyMemberManager.PartyMemberMarkedAsNew.RemoveListener(determineVisibility);
        NewPartyMemberManager.PartyMemberNoLongerNew.RemoveListener(determineVisibility);
    }

    public void determineVisibility()
    {
        determineVisibility(null);
    }

    public void determineVisibility(object obj)
    {
        gameObject.SetActive(NewPartyMemberManager.anPartyMemberIsMarkedAsNew());
        return;
    }
}
