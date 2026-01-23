using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllyPackInfoList
{

    private readonly static AllyPackInfo mineLevel3Guards = new AllyPackInfo(new CreatureAmount[]   { 
                                                                                                        AllyAmountList.guardReka,
                                                                                                        AllyAmountList.guardPazman,
                                                                                                        AllyAmountList.overseerGaspar,
                                                                                                        // AllyAmountList.guardVirag,
                                                                                                        // AllyAmountList.guardVirag,
                                                                                                        // AllyAmountList.guardVirag,
                                                                                                        AllyAmountList.guardVirag
                                                                                                    }, 
                                                                            new string[]
                                                                                        {
                                                                                            FlagNameList.mineLvl3GuardsInParty
                                                                                        });

    private static Dictionary<string, List<AllyPackInfo>> allyPackInfoDict;

    public static AllyPackInfo getAllyPackInfo(string areaName, int index)
    {
        if (!allyPackInfoDict.ContainsKey(areaName))
        {
            return null;
        }

        return allyPackInfoDict[areaName][index];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeAllyPackInfoList()
    {
        List<AllyPackInfo> list;
        allyPackInfoDict = new Dictionary<string, List<AllyPackInfo>>();

        #region MineLvl_3-7
        list = new List<AllyPackInfo>();

        list.Add(mineLevel3Guards);

        allyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section7, list);
        #endregion

    }
}