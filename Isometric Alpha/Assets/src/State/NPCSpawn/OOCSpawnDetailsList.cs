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

    [RuntimeInitializeOnLoadMethod]
    private static void initializeSpawnDetailsList()
    {

        oocSpawnDetailsDict = new Dictionary<string, List<OOCSpawnDetails>>();

        #region 1SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.balint, new Vector3Int(7, 1), LocationNameList.slaveShackOne));
        list.Add(new NPCSpawnDetails(NPCNameList.seb, new Vector3Int(6, 5), LocationNameList.slaveShackOne));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackOne, list);
        #endregion
        #region 2SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.broglin, new Vector3Int(4, 4), LocationNameList.slaveShackTwo, new BeginningConversationScript()));
        list.Add(new NPCSpawnDetails(NPCNameList.garcha, new Vector3Int(4, -1), LocationNameList.slaveShackTwo));

        list.Add(new NPCSpawnDetails(NPCNameList.guardLaszlo, new Vector3Int(3, 1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.guardLaszlo + 1, new Vector3Int(-2, -1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.garcha + 1, new Vector3Int(3, 1), notActivated));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackTwo, list);
        #endregion
        #region 3SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.janos, new Vector3Int(5, 3), LocationNameList.slaveShackThree));

        list.Add(new NPCSpawnDetails(NPCNameList.guardAndras, new Vector3Int(4, 1), notActivated));
        list.Add(new NPCSpawnDetails(NPCNameList.guardAndras + 1, new Vector3Int(6, 2), notActivated, LocationNameList.slaveShackThree));
        list.Add(new NPCSpawnDetails(NPCNameList.guardAndras + 2, new Vector3Int(6, 2), LocationNameList.slaveShackThree));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackThree, list);
        #endregion
        #region 4SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.kastor, new Vector3Int(11, 13), LocationNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.nandor, new Vector3Int(11, 11), LocationNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.carter, new Vector3Int(13, 11), LocationNameList.slaveShackFour));
        list.Add(new NPCSpawnDetails(NPCNameList.guardMarcos, new Vector3Int(13, 13), LocationNameList.slaveShackFour));
        // list.Add(new NPCSpawnDetails(NPCNameList.thatch, new Vector3Int(10, 12), LocationNameList.slaveShackFour));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackFour, list);
        #endregion
        #region 5SlaveShack
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.ervin, new Vector3Int(3, -2), LocationNameList.slaveShackFive));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackFive, list);
        #endregion
        #region 6SlaveShack
        list = new List<OOCSpawnDetails>();
        list.Add(new NPCSpawnDetails(NPCNameList.thatch, new Vector3Int(-1, 1), LocationNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.slate, new Vector3Int(9, 1), LocationNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.guardVazul, new Vector3Int(8, -1), LocationNameList.slaveShackSix));
        list.Add(new NPCSpawnDetails(NPCNameList.rubble, new Vector3Int(-1, -3), LocationNameList.slaveShackSix));

        list.Add(new NPCSpawnDetails(NPCNameList.thatch + 1, new Vector3Int(6, -2), notActivated));

        oocSpawnDetailsDict.Add(LocationNameList.slaveShackSix, list);
        #endregion

        #region Mess Hall
        list = new List<OOCSpawnDetails>();

        list.Add(new ShopkeeperSpawnDetails(NPCNameList.kende, new Vector3Int(3, 10), LocationNameList.messHall, new Vector3Int[] { new Vector3Int(3, 9) }));

        oocSpawnDetailsDict.Add(LocationNameList.messHall, list);
        #endregion

        #region Stockhouse
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.uros, new Vector3Int(7, -1), LocationNameList.stockhouse));
        list.Add(new NPCSpawnDetails(NPCNameList.quartermasterEmese, new Vector3Int(11, 1), LocationNameList.stockhouse, new Vector3Int[] { new Vector3Int(10, 1) }));

        list.Add(new NPCSpawnDetails(NPCNameList.crate, new Vector3Int(10, 4), LocationNameList.stockhouse));
        list.Add(new NPCSpawnDetails(NPCNameList.crate + 1, new Vector3Int(6, -1), LocationNameList.stockhouse));
        list.Add(new NPCSpawnDetails(NPCNameList.crate + 2, new Vector3Int(5, 3), LocationNameList.stockhouse));
        
        list.Add(new NPCSpawnDetails(NPCNameList.barrels, new Vector3Int(6, 5), LocationNameList.stockhouse));

        oocSpawnDetailsDict.Add(LocationNameList.stockhouse, list);
        #endregion

        #region NECamp
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -4), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -5), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(18, -7), LocationNameList.campNorthEast));
        list.Add(new NPCSpawnDetails(NPCNameList.leafPile, new Vector3Int(17, -8), LocationNameList.campNorthEast));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(5, 2), VaultableObject.vaultableBarrelsOneTile));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(-1, 3), VaultableObject.vaultableBarrelsOneTile));

        list.Add(new ChestSpawnDetails(0, new Vector3Int(2, 4), Facing.SouthEast));

        oocSpawnDetailsDict.Add(LocationNameList.campNorthEast, list);
        #endregion
        #region MineEntranceCamp
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.guardMuzsa, new Vector3Int(8, 4), LocationNameList.campMineEntrance));

        list.Add(new ObstacleSpawnDetails(NPCNameList.barricade, new Vector3Int(8, 5), PrefabNames.squareCratesSmall));

        list.Add(new NPCSpawnDetails(NPCNameList.guardMuzsa + 1, new Vector3Int(6, 3), notActivated, LocationNameList.campMineEntrance));
        list.Add(new NPCSpawnDetails(NPCNameList.guardMuzsa + 2, new Vector3Int(6, 3), LocationNameList.campMineEntrance));

        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(13, 9), VaultableObject.vaultableBarrelsTwoTiles));
        list.Add(new VaultableObjectSpawnDetails(NPCNameList.vaultableBarrels, new Vector3Int(12, 9), VaultableObject.vaultableBarrelsTwoTiles));

        oocSpawnDetailsDict.Add(LocationNameList.campMineEntrance, list);
        #endregion
        #region Camp Manse
        list = new List<OOCSpawnDetails>();

        list.Add(new NPCSpawnDetails(NPCNameList.imre, new Vector3Int(-7, -11), LocationNameList.campManse));

        oocSpawnDetailsDict.Add(LocationNameList.campManse, list);
        #endregion
    }

}
