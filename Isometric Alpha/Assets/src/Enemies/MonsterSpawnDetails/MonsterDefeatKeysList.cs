using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterDefeatKeysList
{

    public static Dictionary<string, bool> monsterDefeatKeysDict;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeMonsterDefeatKeysList()
    {
        monsterDefeatKeysDict = new Dictionary<string, bool>();
    }

    public static void setDefeatKey(string key, bool status)
    {
        monsterDefeatKeysDict[key] = status;
    }

    public static bool monsterIsDefeated(int index)
    {
        return monsterIsDefeated(generateMonsterDefeatKey(index));
    }

    public static bool monsterIsDefeated(string key)
    {
        if (!monsterDefeatKeysDict.ContainsKey(key))
        {
            return false;
        }
        else
        {
            return monsterDefeatKeysDict[key];
        }
    }

    public static string generateMonsterDefeatKey(int index)
    {
        return AreaManager.locationName + "-" + index;
    }

    public static FlagWrapper[] getAllMonsterDefeatKeyWrappers()
    {
        return FlagWrapper.getAllFlagsInDictionary(monsterDefeatKeysDict);
    }

    public static void extractAllMonsterDefeatKeys(SaveBlueprint saveBlueprint)
    {
        monsterDefeatKeysDict = new Dictionary<string, bool>();

        foreach (FlagWrapper wrapper in saveBlueprint.currentMonsterDefeatKeys)
        {
            monsterDefeatKeysDict[wrapper.flagName] = wrapper.flagStatus;
        }
    }



}
