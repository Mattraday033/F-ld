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

        LoadSaveFile.OnLoadReadBlueprint.AddListener(readSaveBlueprint);
    }

    public static void readSaveBlueprint(SaveBlueprint blueprint)
    {
        State.formation = new Formation();
        Dictionary<string, PartyMember> partyMemberDict = new Dictionary<string, PartyMember>();

        for (int partyMemberIndex = 0; partyMemberIndex < blueprint.partyMemberStats.Length; partyMemberIndex++)
        {
            string partyMemberName = blueprint.partyMemberStats[partyMemberIndex].key;
            StatsWrapper partyMemberStatsWrapper = blueprint.partyMemberStats[partyMemberIndex];

            AllyStats partyMemberStats = new AllyStats(partyMemberStatsWrapper);
            PartyMember partyMember = new PartyMember(partyMemberStats);

            partyMemberStats.xp = partyMemberStatsWrapper.xp;
            partyMember.canJoinParty = blueprint.partyMemberStats[partyMemberIndex].canJoinParty;
            partyMember.placed = blueprint.partyMemberStats[partyMemberIndex].placed;

            string[] placedPosition = blueprint.partyMemberStats[partyMemberIndex].partyMemberPlacedPosition.Split("_");
            partyMember.placedPosition = new Vector3(float.Parse(placedPosition[0]),
                                                     float.Parse(placedPosition[1]),
                                                     float.Parse(placedPosition[2]));

            partyMember.stats.equippedItems = new EquippedItems(partyMember.stats, SaveBlueprint.extractEquippedItemsFromJson(partyMemberStatsWrapper.currentEquipment));

            partyMemberDict.Add(partyMemberName, partyMember);

            State.formation.setCharacterAtCoords(partyMemberStatsWrapper.partyMemberFormationCoords, partyMember.stats);
        }

        setPartyMemberDict(partyMemberDict);
        NewAbilityManager.resetNewAbilityManager(blueprint.newAbilityWrappers);
        State.currentSkillType = SkillManager.getHighestSkillType(getPlayerStats());
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
        
        if (partyMemberName == null)
        {
            Debug.LogError(partyMemberName + " is not a valid party member name.");
            return partyMemberDict[NPCNameList.carter];
        }

        partyMemberName = DialogueList.scrubNameOfEndNumbers(partyMemberName);

        if(partyMemberName.Contains(NPCNameList.overseer) || 
            partyMemberName.Contains(NPCNameList.chief))
        {
            partyMemberName = partyMemberName.Split(" ")[1];
        }

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

    public static void healAllPartyMembersToFull()
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

        partyMemberDict.Add(NPCNameList.carter, PartyMemberList.getResetPartyMember(NPCNameList.carter));
        partyMemberDict.Add(NPCNameList.gaspar, PartyMemberList.getResetPartyMember(NPCNameList.gaspar));
        partyMemberDict.Add(NPCNameList.nandor, PartyMemberList.getResetPartyMember(NPCNameList.nandor));
        partyMemberDict.Add(NPCNameList.thatch, PartyMemberList.getResetPartyMember(NPCNameList.thatch));
        partyMemberDict.Add(NPCNameList.weft, PartyMemberList.getResetPartyMember(NPCNameList.weft));
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

        joinablePartyMembers.Add(getPlayer());

        foreach (PartyMember partyMember in partyMemberDict.Values)
        {
            if (partyMember.canJoinParty && !partyMember.Equals(getPlayer()))
            {
                joinablePartyMembers.Add(partyMember);
            }
        }

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

            if (partyMember.canJoinParty && story.variablesState[InkVariableNameList.partyFlagPrefix + partyMember.stats.getName()] != null)
            {
                story.variablesState[InkVariableNameList.partyFlagPrefix + partyMember.stats.getName()] = true;
            }

            if (partyMember.isInParty() && story.variablesState[InkVariableNameList.formationFlagPrefix + partyMember.stats.getName()] != null)
            {
                story.variablesState[InkVariableNameList.formationFlagPrefix + partyMember.stats.getName()] = true;
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
        resetPartyMembers();

        foreach(KeyValuePair<string, PartyMember> kvp in newDict)
        {
            partyMemberDict[kvp.Key] = kvp.Value;
        }
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
            if(partyMember.canJoinParty)
            {
                partyMember.stats.addXP(xpToAdd);
            }
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