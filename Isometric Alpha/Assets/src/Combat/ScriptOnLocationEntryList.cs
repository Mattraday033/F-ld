using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScriptOnLocationEntryList
{
    private static Dictionary<string, List<PlayerInteractionScript>> scriptOnLocationEntryDict;

    public static List<PlayerInteractionScript> getScriptsOnLocationEntry()
    {
        if (!scriptOnLocationEntryDict.ContainsKey(AreaManager.locationName))
        {
            return new List<PlayerInteractionScript>();
        }
        
        return scriptOnLocationEntryDict[AreaManager.locationName];
    }

    [RuntimeInitializeOnLoadMethod]
    public static void initialize()
    {
        scriptOnLocationEntryDict = new Dictionary<string, List<PlayerInteractionScript>>();
        List<PlayerInteractionScript> list;

        #region MineLvl_1
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredMineLvl1());

        scriptOnLocationEntryDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1a, list);
        #endregion
    }

}
