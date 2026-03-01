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

        #region MineLvl_1-1a
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredMineLvl1());

        scriptOnLocationEntryDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1a, list);
        #endregion

        #region MineLvl_2-1a
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredMineLvl2());

        scriptOnLocationEntryDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1a, list);
        #endregion

        #region MineLvl_2-2a
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredMineLvl2_2a());

        scriptOnLocationEntryDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2a, list);
        #endregion

        #region MineLvl_3-1a
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredMineLvl3());

        scriptOnLocationEntryDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1a, list);
        #endregion
    }

}
