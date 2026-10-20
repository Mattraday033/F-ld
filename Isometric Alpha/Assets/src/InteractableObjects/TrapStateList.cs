using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TrapStateList
{

    private static Dictionary<string, List<KeyValuePair<string, bool>>> defaultTrapStates;

    public static List<KeyValuePair<string, bool>> getDefaultTrapStates()
    {
        if (!defaultTrapStates.ContainsKey(AreaManager.locationName))
        {
            return new List<KeyValuePair<string, bool>>();
        }
        
        return defaultTrapStates[AreaManager.locationName];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeDefaultTrapStates()
    {
        defaultTrapStates = new Dictionary<string, List<KeyValuePair<string, bool>>>();
        List<KeyValuePair<string, bool>> list;

        #region MineLvl_2-5

        list = new List<KeyValuePair<string, bool>>();

        list.Add(new KeyValuePair<string, bool>(CunningObject.generateKey(ZoneKeyList.mineLvl2 + LocationNameList.section5, Constants.indexZero), true));
        list.Add(new KeyValuePair<string, bool>(CunningObject.generateKey(ZoneKeyList.mineLvl2 + LocationNameList.section5, Constants.indexThree), true));

        defaultTrapStates.Add(ZoneKeyList.mineLvl2 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_2-6

        list = new List<KeyValuePair<string, bool>>();

        //the temporary gates that start the puzzle open
        list.Add(new KeyValuePair<string, bool>(TemporaryGate.generateKey(ZoneKeyList.mineLvl2 + LocationNameList.section6, NPCNameList.ancientPortcullis, Constants.indexZero), true));
        list.Add(new KeyValuePair<string, bool>(TemporaryGate.generateKey(ZoneKeyList.mineLvl2 + LocationNameList.section6, NPCNameList.ancientPortcullis, Constants.indexTwo), true));
        list.Add(new KeyValuePair<string, bool>(TemporaryGate.generateKey(ZoneKeyList.mineLvl2 + LocationNameList.section6, NPCNameList.ancientPortcullis, Constants.indexFive), true));

        defaultTrapStates.Add(ZoneKeyList.mineLvl2 + LocationNameList.section6, list);

        #endregion

    }

}
