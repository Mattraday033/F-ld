using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllyPackInfoList
{

    private readonly static AllyPackInfo mineLevel3Guards = new AllyPackInfo(new CreatureAmount[]   { 
                                                                                                        AllyAmountList.guardReka,
                                                                                                        AllyAmountList.guardPazman,
                                                                                                        AllyAmountList.overseerGaspar,
                                                                                                        AllyAmountList.guardVirag
                                                                                                    }, 
                                                                            new string[]
                                                                                        {
                                                                                            FlagNameList.mineLvl3GuardsInParty
                                                                                        });

    private readonly static AllyPackInfo campSlaveAllies = new AllyPackInfo(new CreatureAmount[]   { 
                                                                                                        AllyAmountList.southEastSlaves,
                                                                                                        AllyAmountList.northEastSlaves,
                                                                                                        AllyAmountList.manseSlaves
                                                                                                    }, 
                                                                            new string[]
                                                                                        {
                                                                                            FlagNameList.kastorStartedRevolt,
                                                                                            FlagNameList.convincedSlavesToHelpYou,
                                                                                            FlagNameList.haveManseSlaveHelp
                                                                                        });

//declaredHostagesDead

    private readonly static AllyPackInfo dezsoAlliesGuards = new AllyPackInfo(new CreatureAmount[]   { 
                                                                                                        AllyAmountList.guardReka,
                                                                                                        AllyAmountList.guardPazman,
                                                                                                        AllyAmountList.overseerGaspar,
                                                                                                        AllyAmountList.guardVirag
                                                                                                    }, 
                                                                            new string[]
                                                                                        {
                                                                                            FlagNameList.hostagesDead
                                                                                        });


    private readonly static AllyPackInfo dezsoAlliesSlaves = new AllyPackInfo(new CreatureAmount[]   { 
                                                                                                        AllyAmountList.northEastSlaves
                                                                                                    }, 
                                                                            new string[]
                                                                                        {
                                                                                            FlagNameList.failedRushDezso
                                                                                        });


    private static Dictionary<string, List<AllyPackInfo>> allyPackInfoDict;

    public static AllyPackInfo defaultAllyPackInfoByZone()
    {
        switch(MapObjectList.getCurrentZoneKey())
        {
            case ZoneKeyList.pit:
                switch(AreaManager.locationName)
                {
                    case ZoneKeyList.pit + LocationNameList.section1a:
                        return campSlaveAllies;
                    default:
                        return null;
                }
            case ZoneKeyList.lovashiCamp:
                if(AreaManager.locationName.Equals(LocationNameList.slaveShackSeven))
                {
                    if(Flags.getFlag(FlagNameList.hostagesDead))
                    {
                        return dezsoAlliesGuards;
                    } else
                    {
                        return dezsoAlliesSlaves;
                    }
                } else
                {
                    return campSlaveAllies;
                }
            case ZoneKeyList.manseFirstFloor:
            case ZoneKeyList.manseSecondFloor:
                return campSlaveAllies;
            default:
                return null;
        }
    }

    public static AllyPackInfo getAllyPackInfo(string areaName, int index)
    {
        if (!allyPackInfoDict.ContainsKey(areaName))
        {
            return defaultAllyPackInfoByZone();
        }

        List<AllyPackInfo> allyPacksInArea = allyPackInfoDict[areaName];

        if(index >= allyPacksInArea.Count)
        {
            return defaultAllyPackInfoByZone();
        }

        return allyPacksInArea[index];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeAllyPackInfoList()
    {
        List<AllyPackInfo> list;
        allyPackInfoDict = new Dictionary<string, List<AllyPackInfo>>();

        #region GuardHouse SE
        list = new List<AllyPackInfo>();

        list.Add(campSlaveAllies);

        allyPackInfoDict.Add(LocationNameList.guardHouseSouthWest, list);
        #endregion

        #region MineLvl_3-7
        list = new List<AllyPackInfo>();

        list.Add(mineLevel3Guards);
        list.Add(mineLevel3Guards);
        list.Add(mineLevel3Guards);

        allyPackInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section7, list);
        #endregion

    }
}