using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ButtonScriptList
{

    private static Dictionary<string, List<ButtonLogicScript>> scriptDict;

    public static List<ButtonLogicScript> getButtonScripts(string locationName)
    {
        if (!scriptDict.ContainsKey(locationName))
        {
            return new List<ButtonLogicScript>();
        }

        return scriptDict[locationName];

    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeScriptDict()
    {
        scriptDict = new Dictionary<string, List<ButtonLogicScript>>();
        List<ButtonLogicScript> list;

        #region 6SlaveShack

        list = new List<ButtonLogicScript>();

        list.Add(new OpenGateButtonLogicScript(Constants.indexZero, Constants.sizeTwo, NPCNameList.fallenBeam));

        scriptDict.Add(LocationNameList.slaveShackSix, list);

        #endregion        
    }



}
