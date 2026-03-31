using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public struct NewAbilityWrapper
{
    public string allyKey;    
    public string[] newAbilityKeys;

    public NewAbilityWrapper(string allyKey, List<Ability> newAbilityList)
    {
        this.allyKey = allyKey;        
        
        if(newAbilityList == null)
        {
            newAbilityKeys = new string[0];
            return;
        }

        List<string> newAbilityKeysList = new List<string>();

        foreach(Ability ability in newAbilityList)
        {
            newAbilityKeysList.Add(ability.getKey());
        }

        newAbilityKeys = newAbilityKeysList.ToArray();
    }    
}

public static class NewAbilityManager
{

    public readonly static UnityEvent AbilityMarkedAsNew = new UnityEvent();
    public readonly static UnityEvent AbilityNoLongerNew = new UnityEvent();

	public static Dictionary<AllyStats, List<Ability>> newAbilityDict;

	public static void setAbilityAsNew(AllyStats owner, Ability newAbility)
	{
        if(owner == null || newAbility == null)
        {
            return;
        }

		if(!newAbilityDict.ContainsKey(owner))
        {
            newAbilityDict[owner] = new List<Ability>();
        }

        newAbilityDict[owner].Add(newAbility);

        AbilityMarkedAsNew.Invoke();
	}

    public static void removeAbility(AllyStats owner, Ability newAbility)
	{
        if(owner == null || newAbility == null)
        {
            return;
        }

		if(!newAbilityDict.ContainsKey(owner))
        {
            newAbilityDict[owner] = new List<Ability>();
        }

        newAbilityDict[owner].Remove(newAbility);

        AbilityNoLongerNew.Invoke();
	}

    public static List<Ability> getAllNewAbilities(AllyStats owner)
    {
		if(!newAbilityDict.ContainsKey(owner))
        {
            return new List<Ability>();
        }

        return newAbilityDict[owner];
    }

    public static bool abilityIsNew(AllyStats owner, Ability ability)
    {
		if(owner == null || !newAbilityDict.ContainsKey(owner) || ability == null)
        {
            return false;
        }

        return newAbilityDict[owner].Contains(ability);
    }

    public static bool anAbilityIsMarkedAsNew()
    {
        foreach(List<Ability> list in newAbilityDict.Values)
        {
            if(list != null && list.Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    public static bool anAbilityIsMarkedAsNewPerStat(AllyStats owner, PrimaryStat stat)
    {
        if(owner == null || !newAbilityDict.ContainsKey(owner))
        {
            return false;
        }

        char statCharacter = AbilityList.getPrimaryStatCharacter(stat);

        foreach(Ability newAbility in newAbilityDict[owner])
        {
            if(newAbility != null && newAbility.statKey.Length > 0 && newAbility.statKey[0] == statCharacter)
            {
                return true;
            }
        }

        return false;
    }


    public static NewAbilityWrapper[] getAllNewAbilityWrappers()
    {
        List<NewAbilityWrapper> newAbilityWrappers = new List<NewAbilityWrapper>();        

        foreach(KeyValuePair<AllyStats, List<Ability>> kvp in newAbilityDict)        
        {
            newAbilityWrappers.Add(new NewAbilityWrapper(kvp.Key.getName(), kvp.Value));
        }

        return newAbilityWrappers.ToArray();
    }

    public static void resetNewAbilityManager(NewAbilityWrapper[] newAbilityWrappers)
    {
        newAbilityDict = new Dictionary<AllyStats, List<Ability>>();

        if(newAbilityWrappers == null)
        {
            return;
        }

        foreach(NewAbilityWrapper wrapper in newAbilityWrappers)
        {
            AllyStats owner = PartyManager.getPartyMember(wrapper.allyKey).stats;

            List<Ability> abilities = new List<Ability>();

            foreach(string key in wrapper.newAbilityKeys)
            {
                Ability ability = AbilityList.getAbility(owner, key) as Ability;

                if(ability != null)
                {
                    abilities.Add(ability);
                }
            }

            newAbilityDict[owner] = abilities;
        }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeNewAbilityManager()
    {
        newAbilityDict = new Dictionary<AllyStats, List<Ability>>();
    }
}
