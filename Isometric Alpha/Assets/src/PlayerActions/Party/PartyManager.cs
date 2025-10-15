using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PartyManager
{
    private static Dictionary<string, PartyMember> partyMemberDict;

    static PartyManager()
    {
        if (partyMemberDict == null)
        {
            resetPartyMembers();
        }
    }

    public static void initialize()
    {
        //empty on purpose
    }

    public static int getNumberOfPartyMembersTotal()
    {
        return partyMemberDict.Count;
    }

    public static List<PartyMember> getAllPartyMembers()
    {
        List<PartyMember> allPartyMembers = new List<PartyMember>();

        foreach (KeyValuePair<string, PartyMember> kvp in partyMemberDict)
        {
            allPartyMembers.Add(kvp.Value);
        }

        return allPartyMembers;
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
        foreach (KeyValuePair<string, PartyMember> kvp in PartyManager.partyMemberDict)
        {
            PartyMember partyMember = kvp.Value;

            if (partyMember != null && !(partyMember is null))

                partyMember.stats.modifyCurrentHealth(partyMember.stats.getTotalHealth(), true);
        }
    }

    public static List<string> getAllGameObjectNamesInTrain()
    {
        List<string> gameObjectNames = new List<string>();

        foreach (KeyValuePair<string, PartyMember> kvp in PartyManager.partyMemberDict)
        {
            PartyMember partyMember = kvp.Value;

            if (partyMember.isInParty())
            {
                gameObjectNames.Add(partyMember.overWorldGameObjectName);
            }
        }

        return gameObjectNames;
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

    public static ArrayList getAllUnassignedPartyMembers(Stats[][] positionGrid)
    {
        ArrayList unassignedPartyMembers = new ArrayList();

        foreach (KeyValuePair<string, PartyMember> kvp in partyMemberDict)
        {
            PartyMember partyMember = kvp.Value;

            if (partyMember.canJoinParty && !partyMember.isInParty(positionGrid))
            {
                unassignedPartyMembers.Add(partyMember);
            }
        }

        return unassignedPartyMembers;
    }

    public static int getCurrentPartyMemberSlotsUsed()
    {
        int usedSlots = 0;

        foreach (Stats[] row in State.formation.getGrid())
        {
            foreach (Stats position in row)
            {
                if (!(position is null))
                {
                    usedSlots++;
                }
            }
        }

        return usedSlots;
    }

    public static void removeAllPartyMembersFromCurrentParty()
    {
        State.formation.removeAllPartyMembers();
    }

    public static bool hasJoinablePartyMembers()
    {
        foreach (KeyValuePair<string, PartyMember> kvp in partyMemberDict)
        {
            PartyMember partyMember = kvp.Value;

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

        foreach (KeyValuePair<string, PartyMember> kvp in partyMemberDict)
        {
            if (kvp.Value.canJoinParty)
            {
            joinablePartyMembers.Add(kvp.Value);
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

    public static void printAllJoinabilities()
    {
        foreach (KeyValuePair<string, PartyMember> kvp in partyMemberDict)
        {
            PartyMember partyMember = kvp.Value;

            Debug.Log("Current PartyMember: " + partyMember.getName() + "'s canJoinParty = " + partyMember.canJoinParty);
        }
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
        foreach (KeyValuePair<string, PartyMember> kvp in partyMemberDict)
        {
            if (!PartyMemberList.isPartyMemberName(kvp.Key))
            {
                return kvp.Value;
            }
        }

        return null;
    }

    public static AllyStats getPlayerStats()
    {
        foreach (KeyValuePair<string, PartyMember> kvp in partyMemberDict)
        {
            if (!PartyMemberList.isPartyMemberName(kvp.Key))
            {
                return kvp.Value.stats;
            }
        }

        return null;
    }

    public static Stats getPlayerStats(List<PartyMember> party)
    {
        foreach (PartyMember partyMember in party)
        {
            if (!PartyMemberList.isPartyMemberName(partyMember.stats.getName()))
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
            if (!PartyMemberList.isPartyMemberName(partyMember.key))
            {
                return new AllyStats(partyMember);
            }
        }

        return null;
    }

    public static ArrayList getAllPossiblePartyMembers()
    {
        ArrayList allPossiblePartyMembers = new ArrayList();

        foreach (KeyValuePair<string, PartyMember> kvp in partyMemberDict)
        {
            allPossiblePartyMembers.Add(kvp.Value);
        }

        // while (allPossiblePartyMembers.Count < totalPossiblePartyMembers)
        // {
        // 	allPossiblePartyMembers.Add(null);
        // }

        return allPossiblePartyMembers;
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
}
