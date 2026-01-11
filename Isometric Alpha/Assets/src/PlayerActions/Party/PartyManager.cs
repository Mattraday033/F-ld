using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PartyManager
{
    public readonly static UnityEvent OnPartyChange = new UnityEvent();
    public const string playerMarker = "_P";
    private static Dictionary<string, PartyMember> partyMemberDict = new Dictionary<string, PartyMember>();

    [RuntimeInitializeOnLoadMethod]
    private static void initializePartyManager()
    {
        partyMemberDict = new Dictionary<string, PartyMember>();
        resetPartyMembers();
    }

    public static int getNumberOfPartyMembersTotal()
    {
        return partyMemberDict.Count;
    }

    public static List<PartyMember> getAllPartyMembers()
    {
        return new List<PartyMember>(partyMemberDict.Values);
    }

    public static PartyMember getPartyMember(string partyMemberName)
    {
        if (partyMemberName == null || !partyMemberDict.ContainsKey(partyMemberName))
        {
            Debug.LogError(partyMemberName + " is not a valid party member name.");
            return partyMemberDict[NPCNameList.carter];
        }

        return partyMemberDict[partyMemberName];
    }

    public static bool nameIsInParty(string potentialMemberName)
    {
        if (potentialMemberName != null && partyMemberDict.ContainsKey(potentialMemberName))
        {
            return partyMemberDict[potentialMemberName].isInParty();
        }

        return false;
    }

    public static void healFullAllPartyMembers()
    {
        foreach (PartyMember partyMember in partyMemberDict.Values)
        {
            if (partyMember != null && !(partyMember is null))

                partyMember.stats.modifyCurrentHealth(partyMember.stats.getTotalHealth(), true);
        }
    }

    public static void resetAllPartyMemberCooldowns()
    {
        foreach (KeyValuePair<string, PartyMember> kvp in partyMemberDict)
        {
            PartyMember partyMember = kvp.Value;

            partyMember.stats.resetAllCooldowns();
        }
    }


    public static void resetPartyMembers()
    {
        partyMemberDict = new Dictionary<string, PartyMember>();

        partyMemberDict.Add(NPCNameList.thatch, PartyMemberList.getResetPartyMember(NPCNameList.thatch));
        partyMemberDict.Add(NPCNameList.nandor, PartyMemberList.getResetPartyMember(NPCNameList.nandor));
        partyMemberDict.Add(NPCNameList.carter, PartyMemberList.getResetPartyMember(NPCNameList.carter));
    }

    public static void removeAllPartyMembersFromCurrentParty()
    {
        State.formation.removeAllPartyMembers();
    }

    public static bool hasJoinablePartyMembers()
    {
        foreach (PartyMember partyMember in partyMemberDict.Values)
        {
            if (partyMember.canJoinParty)
            {
                return true;
            }
        }

        return false;
    }

    public static List<PartyMember> getAllJoinablePartyMembers()
    {
        List<PartyMember> joinablePartyMembers = new List<PartyMember>();

        foreach (PartyMember partyMember in partyMemberDict.Values)
        {
            if (partyMember.canJoinParty)
            {
                joinablePartyMembers.Add(partyMember);
            }
        }

        joinablePartyMembers.Remove(getPlayer());

        joinablePartyMembers.Insert(0, getPlayer());

        return joinablePartyMembers;
    }

    public static List<PartyMember> getAllUpgradablePartyMembers()
    {
        List<PartyMember> joinablePartyMembers = getAllJoinablePartyMembers();
        List<PartyMember> upgradablePartyMembers = new List<PartyMember>();

        foreach (PartyMember partyMember in joinablePartyMembers)
        {
            if (partyMember.canBeUpgraded())
            {
                upgradablePartyMembers.Add(partyMember);
            }
        }

        return upgradablePartyMembers;
    }

    public static int getNumberOfUpgradablePartyMembers()
    {
        return getAllUpgradablePartyMembers().Count;
    }

    public static Story addAllVariables(Story story)
    {
        foreach (KeyValuePair<string, PartyMember> kvp in partyMemberDict)
        {
            PartyMember partyMember = kvp.Value;

            if (partyMember.canJoinParty && story.variablesState["partyFlag" + partyMember.stats.getName()] != null)
            {
                story.variablesState["partyFlag" + partyMember.stats.getName()] = true;
            }

            if (partyMember.isInParty() && story.variablesState["formationFlag" + partyMember.stats.getName()] != null)
            {
                story.variablesState["formationFlag" + partyMember.stats.getName()] = true;
            }
        }

        return story;
    }

    public static PartyMember getPlayer()
    {
        foreach (PartyMember partyMember in partyMemberDict.Values)
        {
            if (partyMember.getName().Contains(playerMarker))
            {
                return partyMember;
            }
        }

        return null;
    }

    public static AllyStats getPlayerStats()
    {
        foreach (PartyMember partyMember in partyMemberDict.Values)
        {
            if (partyMember.getName().Contains(playerMarker))
            {
                return partyMember.stats;
            }
        }

        return null;
    }

    public static Stats getPlayerStats(List<PartyMember> party)
    {
        foreach (PartyMember partyMember in party)
        {
            if (partyMember.getName().Contains(playerMarker))
            {
                return partyMember.stats;
            }
        }

        return null;
    }

    public static Stats getPlayerStats(StatsWrapper[] party)
    {
        foreach (StatsWrapper partyMember in party)
        {
            if (partyMember.key.Contains(playerMarker))
            {
                return new AllyStats(partyMember);
            }
        }

        return null;
    }

    public static void addPlayerStatsToDict(AllyStats playerStats)
    {
        PartyMember player = new PartyMember(playerStats);

        player.canJoinParty = true;

        partyMemberDict.Add(playerStats.getName(), player);
    }

    public static void setPartyMemberDict(Dictionary<string, PartyMember> newDict)
    {
        partyMemberDict = newDict;
    }

    public static void addXP(string xpToAdd)
    {
        addXP(int.Parse(xpToAdd));
    }

    public static void addXP(int xpToAdd)
    {
        List<PartyMember> partyMembers = getAllPartyMembers();

        foreach (PartyMember partyMember in partyMembers)
        {
            partyMember.stats.addXP(xpToAdd);
        }
    }

    public static List<PartyMember> getAllPartyMembersInTrain()
    {
        List<PartyMember> train = new List<PartyMember>();

        foreach(PartyMember partyMember in partyMemberDict.Values)
        {
            if( !partyMember.getName().Contains(playerMarker) &&
                State.formation.contains(partyMember.stats.getName()) &&
                !partyMember.stats.isDead())
            {
                train.Add(partyMember);
            }
        }

        return train;
    }

    public static string getPlayerNameForDisplay()
    {
        return getPlayerStats().getName().Replace(playerMarker,"");
    }
}
