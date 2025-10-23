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

        #region 1SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.balint, new Vector3Int(7, 1), AreaNameList.slaveShackOne));
        list.Add(new NPCSpawnDetails(NPCNameList.seb, new Vector3Int(6, 5), AreaNameList.slaveShackOne));

        oocSpawnDetailsDict.Add(AreaNameList.slaveShackOne, list);
        #endregion
        #region 2SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.broglin, new Vector3Int(4, 4), AreaNameList.slaveShackTwo, new BeginningConversationScript()));
        list.Add(new NPCSpawnDetails(NPCNameList.garcha, new Vector3Int(4, -1), AreaNameList.slaveShackTwo));

        list.Add(new NPCSpawnDetails(NPCNameList.guardLaszlo, new Vector3Int(3, 1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.guardLaszlo + 1, new Vector3Int(-2, -1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.garcha + 1, new Vector3Int(3, 1), notActivated));

        oocSpawnDetailsDict.Add(AreaNameList.slaveShackTwo, list);
        #endregion
        #region 3SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.janos, new Vector3Int(5, 3), AreaNameList.slaveShackThree));

        list.Add(new NPCSpawnDetails(NPCNameList.guardAndras, new Vector3Int(4, 1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.guardAndras+1, new Vector3Int(6, 2), notActivated, AreaNameList.slaveShackThree));
        list.Add(new NPCSpawnDetails(NPCNameList.guardAndras+2, new Vector3Int(6, 2), AreaNameList.slaveShackThree));

        oocSpawnDetailsDict.Add(AreaNameList.slaveShackThree, list);
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
        #region 5SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.ervin, new Vector3Int(3, -2), AreaNameList.slaveShackFive));

        oocSpawnDetailsDict.Add(AreaNameList.slaveShackFive, list);
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

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(5, 2), VaultableObject.vaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(-1, 3), VaultableObject.vaultableBarrelsOneTile));

        list.Add(new ChestSpawnDetails(0, new Vector3Int(2, 4), Facing.SouthEast));

        oocSpawnDetailsDict.Add(AreaNameList.campNorthEast, list);
        #endregion
        #region MineEntranceCamp
        list = new List<OOCSpawnDetails>();


        list.Add(new NPCSpawnDetails(NPCNameList.guardMuzsa, new Vector3Int(8, 4), AreaNameList.campMineEntrance));

        list.Add(new ObstacleSpawnDetails(NPCNameList.barricade, new Vector3Int(8, 5), PrefabNames.squareCratesSmall));

        list.Add(new NPCSpawnDetails(NPCNameList.guardMuzsa+1, new Vector3Int(6, 3), notActivated, AreaNameList.campMineEntrance));
        list.Add(new NPCSpawnDetails(NPCNameList.guardMuzsa+2, new Vector3Int(6, 3), AreaNameList.campMineEntrance));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(13, 9), VaultableObject.vaultableBarrelsTwoTiles));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(12, 9), VaultableObject.vaultableBarrelsTwoTiles));

        oocSpawnDetailsDict.Add(AreaNameList.campMineEntrance, list);
        #endregion
        #region Camp Manse
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.imre, new Vector3Int(-7, -11), AreaNameList.campManse));

        oocSpawnDetailsDict.Add(AreaNameList.campManse, list);
        #endregion
    }

}
