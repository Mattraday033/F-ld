using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OOCSpawnDetailsList
{
    private const bool notActivated = false;
    private static List<OOCSpawnDetails> list;

    private static Dictionary<string, List<OOCSpawnDetails>> oocSpawnDetailsDict;

    public static List<OOCSpawnDetails> getOOCSpawnDetails(string areaName)
    {
        if(!oocSpawnDetailsDict.ContainsKey(areaName))
        {
            return new List<OOCSpawnDetails>();
        }

        return oocSpawnDetailsDict[areaName];
    }

    static OOCSpawnDetailsList()
    {

        oocSpawnDetailsDict = new Dictionary<string, List<OOCSpawnDetails>>();

        #region 2SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.broglin, new Vector3Int(4, 4), AreaNameList.slaveShackTwo, new BeginningConversationScript()));
        list.Add(new NPCSpawnDetails(NPCNameList.garcha, new Vector3Int(4, -1), AreaNameList.slaveShackTwo));

        list.Add(new NPCSpawnDetails(NPCNameList.guardLaszlo, new Vector3Int(3, 1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.guardLaszlo + 1, new Vector3Int(-2, -1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.garcha + 1, new Vector3Int(3, 1), notActivated));

        oocSpawnDetailsDict.Add(AreaNameList.slaveShackTwo, list);
        #endregion

        #region 4SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.kastor, new Vector3Int(11, 13), AreaNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.nandor, new Vector3Int(11, 11), AreaNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.carter, new Vector3Int(13, 11), AreaNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(13, 13), AreaNameList.slaveShackFour));
        // list.Add(new NPCSpawnDetails(NPCNameList.thatch, new Vector3Int(10, 12), AreaNameList.slaveShackFour));

        oocSpawnDetailsDict.Add(AreaNameList.slaveShackFour, list);
        #endregion

        #region 6SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.thatch, new Vector3Int(-1, 1), AreaNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.slate, new Vector3Int(9, 1), AreaNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.guardVazul, new Vector3Int(8, -1), AreaNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.rubble, new Vector3Int(-1, -3), AreaNameList.slaveShackSix));

        list.Add(new NPCSpawnDetails(NPCNameList.thatch+1, new Vector3Int(6, -2), notActivated));

        oocSpawnDetailsDict.Add(AreaNameList.slaveShackSix, list);
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

        oocSpawnDetailsDict.Add(AreaNameList.campNorthEast, list);
        #endregion
    }

}
