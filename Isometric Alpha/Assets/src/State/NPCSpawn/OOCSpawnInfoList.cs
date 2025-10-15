using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OOCSpawnInfoList
{
    private const bool notActivated = false;
    private static List<OOCSpawnDetails> list;

    private static Dictionary<string, List<OOCSpawnDetails>> oocSpawnInfoDict;

    public static List<OOCSpawnDetails> getOOCSpawnDetails(string areaName)
    {
        if(!oocSpawnInfoDict.ContainsKey(areaName))
        {
            return new List<OOCSpawnDetails>();
        }

        return oocSpawnInfoDict[areaName];
    }

    static OOCSpawnInfoList()
    {

        oocSpawnInfoDict = new Dictionary<string, List<OOCSpawnDetails>>();

        #region 2SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.broglin, new Vector3Int(4, 4), AreaNameList.slaveShackTwo, new BeginningConversationScript()));
        list.Add(new NPCSpawnDetails(NPCNameList.garcha, new Vector3Int(4, -1), AreaNameList.slaveShackTwo));

        list.Add(new NPCSpawnDetails(NPCNameList.guardLaszlo, new Vector3Int(3, 1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.guardLaszlo + 1, new Vector3Int(-2, -1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.garcha + 1, new Vector3Int(3, 1), notActivated));

        oocSpawnInfoDict.Add(AreaNameList.slaveShackTwo, list);
        #endregion

        #region NECamp
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -4), AreaNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -5), AreaNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -7), AreaNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(17, -8), AreaNameList.campNorthEast));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(5, 2), VaultableObject.vaultableBarrels));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(-1, 3), VaultableObject.vaultableBarrels));

        list.Add(new ChestSpawnDetails(0, new Vector3Int(2, 4), Facing.SouthEast));

        oocSpawnInfoDict.Add(AreaNameList.campNorthEast, list);
        #endregion
    }

}
