using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EffectPathList
{
    private static Dictionary<string, string> effectByActionNameDict;

    private const string defaultEffectFolderPath = "";

    public static string getEffectFolderPath(string actionName)
    {
        if (!effectByActionNameDict.ContainsKey(actionName))
        {
            return defaultEffectFolderPath;
        }

        return effectByActionNameDict[actionName];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeEffectPathList()
    {
        effectByActionNameDict = new Dictionary<string, string>();

        //Positive Effects
        effectByActionNameDict.Add(ChargeUpAbility.chargingUpName, PrefabNames.positiveEffectFolderPath);

        //Damaging Effects
        effectByActionNameDict.Add(AbilityList.batClawName, PrefabNames.damagingEffectFolderPath);
        // effectByActionNameDict.Add(AbilityList.flurryKey, PrefabNames.damagingEffectFolderPath);
        effectByActionNameDict.Add(AbilityList.godSpellAbilityKey, PrefabNames.damagingEffectFolderPath);

        //Bat Swarm Effects
        effectByActionNameDict.Add(AbilityList.swarmRushKey, PrefabNames.batSwarmEffectFolderPath);
        effectByActionNameDict.Add(AbilityList.flurryKey, PrefabNames.batSwarmEffectFolderPath);
    }

}
