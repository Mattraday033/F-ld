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
        string zoneKey = MapObjectList.getMapObject(AreaManager.locationName).getZoneKey();

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

        #region 6SlaveShack

        list = new List<PlayerInteractionScript>();

        list.Add(new SetUpTutorialShackScript());

        scriptOnAreaEntryDict.Add(LocationNameList.slaveShackSix, list);

        #endregion


    }

}

public class SetUpTutorialShackScript : PlayerInteractionScript
{
    public override void runScript()
    {
        bool choseStrengthAtStart = Flags.getFlag(FlagNameList.choseStrengthAtStart);

        if (choseStrengthAtStart)
        {

        }
        else
        {

        }

        bool choseDexterityAtStart = Flags.getFlag(FlagNameList.choseDexterityAtStart);

        if (choseDexterityAtStart)
        {

        }
        else
        {

        }

        bool choseWisdomAtStart = Flags.getFlag(FlagNameList.choseWisdomAtStart);


        if (choseWisdomAtStart)
        {

        }
        else
        {
            SecretDoorFlags.addSecretDoorFlag(SecretDoorKeyList.wisTutorialSecretDoor);
        }

        bool choseCharismaAtStart = Flags.getFlag(FlagNameList.choseCharismaAtStart);

        if (choseCharismaAtStart)
        {

        }
        else
        {

        }

        Debug.LogError("choseStrengthAtStart = " + choseStrengthAtStart);
        Debug.LogError("choseDexterityAtStart = " + choseDexterityAtStart);
        Debug.LogError("choseWisdomAtStart = " + choseWisdomAtStart);
        Debug.LogError("choseCharismaAtStart = " + choseCharismaAtStart);


    }

}