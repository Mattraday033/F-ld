using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public static class NewPartyMemberManager
{

    public readonly static UnityEvent PartyMemberMarkedAsNew = new UnityEvent();
    public readonly static UnityEvent PartyMemberNoLongerNew = new UnityEvent();

	public static List<string> newPartyMemberNames;

	public static void setPartyMemberAsNew(string newPartyMember)
	{
        if(newPartyMemberNames.Contains(newPartyMember))
        {
            return;
        }

        newPartyMemberNames.Add(newPartyMember);

        PartyMemberMarkedAsNew.Invoke();
	}

    public static void removePartyMember(PartyMember partyMember)
	{
        if(partyMember == null)
        {
            return;
        }

        removePartyMember(partyMember.getName());
	}

    public static void removePartyMember(string newPartyMember)
	{
        if(newPartyMember == null || 
            newPartyMember.Length <= 0 || 
            newPartyMember.Contains(PartyManager.playerMarker))
        {
            return;
        }

        newPartyMemberNames.Remove(newPartyMember);

        PartyMemberNoLongerNew.Invoke();
	}

    public static bool partyMemberIsNew(PartyMember partyMember)
    {
        if(partyMember == null)
        {
            return false;
        }

        return partyMemberIsNew(partyMember.getName());
    }

    public static bool partyMemberIsNew(AllyStats partyMember)
    {
        if(partyMember == null)
        {
            return false;
        }

        return partyMemberIsNew(partyMember.getName());
    }

    public static bool partyMemberIsNew(string partyMember)
    {
        if(partyMember == null)
        {
            return false;
        }
        return newPartyMemberNames.Contains(partyMember);
    }

    public static bool anPartyMemberIsMarkedAsNew()
    {
        return newPartyMemberNames.Count > 0;
    }

    public static string[] getAllNewPartyMembersForSave()
    {
        return newPartyMemberNames.ToArray();
    }

    public static void resetNewPartyMemberManager(string[] newPartyMembers)
    {
        if(newPartyMembers == null)
        {
            newPartyMemberNames = new List<string>();
            return;
        }

        newPartyMemberNames = new List<string>(newPartyMembers);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeNewPartyMemberManager()
    {
        newPartyMemberNames = new List<string>();

        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }

    private static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        resetNewPartyMemberManager(blueprint.newPartyMemberNames);
    }
}
