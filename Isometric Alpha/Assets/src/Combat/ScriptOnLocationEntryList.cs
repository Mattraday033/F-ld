using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScriptOnLocationEntryList
{


    private static Dictionary<string, List<PlayerInteractionScript>> scriptOnAreaEntryDict;


    public static List<PlayerInteractionScript> getScriptsOnLocationEntry()
    {
        string zoneKey = MapObjectList.getCurrentZoneKey();

        if (!scriptOnAreaEntryDict.ContainsKey(AreaManager.locationName) ||
            MapLocation.hasBeenDiscovered(zoneKey, AreaManager.locationName))
        {
            return new List<PlayerInteractionScript>();
        }
        
        return scriptOnAreaEntryDict[AreaManager.locationName];
    }

    [RuntimeInitializeOnLoadMethod]
    public static void initialize()
    {
        scriptOnAreaEntryDict = new Dictionary<string, List<PlayerInteractionScript>>();
        List<PlayerInteractionScript> list;

        #region Mine_Lvl_1
        list = new List<PlayerInteractionScript>();

        list.Add(new EnteredMineLvl1());

        scriptOnAreaEntryDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1a, list);
        #endregion
    }

}
