using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransitionSpawnInfoList
{
    private const int twoSpaceMultiplier = 2;

    private static Dictionary<string, List<Transition>> transitionSpawnInfoDict;
    private static List<Transition> list;

    public static List<Transition> getTransitionSpawnInfo(string areaName)
    {
        if (!transitionSpawnInfoDict.ContainsKey(areaName))
        {
            return new List<Transition>();
        }

        return transitionSpawnInfoDict[areaName];
    }

    static TransitionSpawnInfoList()
    {
        transitionSpawnInfoDict = new Dictionary<string, List<Transition>>();

        #region Slave Shack 

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.slaveShackOne, AreaNameList.campCenter, new Vector3Int(3,1), 0, Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackOne, list);

        #endregion

        #region Slave Shack 2

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.slaveShackTwo, AreaNameList.campNorthEast, new Vector3Int(2, 1), 0, Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackTwo, list);

        #endregion

        #region Slave Shack 3

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.slaveShackThree, AreaNameList.campNorthEast, new Vector3Int(4,0), 0, Facing.NorthWest));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackThree, list);

        #endregion

        #region Slave Shack 4

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.slaveShackFour, AreaNameList.campSouthEast, new Vector3Int(12,11), 0, Facing.SouthWest));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackFour, list);

        #endregion

        #region Slave Shack 5

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.slaveShackFive, AreaNameList.campSouthEast, new Vector3Int(2, -7), 0, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.slaveShackFive, AreaNameList.campSouthEast, new Vector3Int(1, -7), 1, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.slaveShackFive, AreaNameList.campSouthEast, new Vector3Int(-3, -2), 2, Facing.NorthEast));                                        

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackFive, list);

        #endregion

        #region Slave Shack 6

        list = new List<Transition>();


        list.Add(new Transition(AreaNameList.slaveShackSix, AreaNameList.campSouthEast, new Vector3Int(5, 3), 0, Facing.SouthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.slaveShackSix, AreaNameList.campSouthEast, new Vector3Int(4, 3), 1, Facing.SouthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.slaveShackSix, AreaNameList.campSouthEast, new Vector3Int(-3, -2), 2, Facing.NorthEast));
        list.Add(new Transition(AreaNameList.slaveShackSix, AreaNameList.campSouthEast, new Vector3Int(7, -7), 3, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.slaveShackSix, AreaNameList.campSouthEast, new Vector3Int(8, -7), 4, Facing.NorthWest, twoSpaceMultiplier));
        transitionSpawnInfoDict.Add(AreaNameList.slaveShackSix, list);

        #endregion

        #region Slave Shack 7

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.slaveShackSeven, AreaNameList.campNorthEast, new Vector3Int(5, 7), 0, Facing.SouthEast));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackSeven, list);

        #endregion

        #region Slave Shack 8

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.slaveShackEight, AreaNameList.campManse, new Vector3Int(-1,-5), 0, Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackEight, list);

        #endregion

        #region Slave Shack 9

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.slaveShackNine, AreaNameList.campManse, new Vector3Int(1,3), 0, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.slaveShackNine, AreaNameList.campManse, new Vector3Int(6,-2), 1, Facing.SouthWest));
        list.Add(new Transition(AreaNameList.slaveShackNine, AreaNameList.campManse, new Vector3Int(0,-7), 2, Facing.NorthWest));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackNine, list);

        #endregion

        #region MessHall

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.messHall, AreaNameList.campSouthEast, new Vector3Int(6,14), 0, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.messHall, AreaNameList.campSouthEast, new Vector3Int(0,14), 1, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.messHall, AreaNameList.campSouthEast, new Vector3Int(-3,9), 2, Facing.NorthEast));
        list.Add(new Transition(AreaNameList.messHall, AreaNameList.campSouthEast, new Vector3Int(-3,8), 3, Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.messHall, list);

        #endregion

        #region Stables

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.stables, AreaNameList.campCenter, new Vector3Int(1,4), 0, Facing.NorthEast));
        list.Add(new Transition(AreaNameList.stables, AreaNameList.campCenter, new Vector3Int(1,3), 1, Facing.NorthEast));
        list.Add(new Transition(AreaNameList.stables, AreaNameList.campCenter, new Vector3Int(8,-5), 2, Facing.NorthWest));
        list.Add(new Transition(AreaNameList.stables, AreaNameList.campCenter, new Vector3Int(9,-5), 3, Facing.NorthWest));
        list.Add(new Transition(AreaNameList.stables, AreaNameList.campCenter, new Vector3Int(14,4), 4, Facing.SouthWest));
        list.Add(new Transition(AreaNameList.stables, AreaNameList.campCenter, new Vector3Int(14,3), 5, Facing.SouthWest));

        transitionSpawnInfoDict.Add(AreaNameList.stables, list);

        #endregion

        #region Temple

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.temple, AreaNameList.campCenter, new Vector3Int(6,-5), 0, Facing.NorthWest));
        list.Add(new Transition(AreaNameList.temple, AreaNameList.campCenter, new Vector3Int(12,3), 1, Facing.SouthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.temple, AreaNameList.campCenter, new Vector3Int(12,4), 2, Facing.SouthWest, twoSpaceMultiplier));

        transitionSpawnInfoDict.Add(AreaNameList.temple, list);

        #endregion

        #region GuardShack

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.guardShack, AreaNameList.campCenter, new Vector3Int(11,9), 0, Facing.SouthWest));

        transitionSpawnInfoDict.Add(AreaNameList.guardShack, list);

        #endregion

        #region GuardHouseNE

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.guardHouseNorthEast, AreaNameList.campManse, new Vector3Int(7,-2), 0, Facing.SouthWest));
        list.Add(new Transition(AreaNameList.guardHouseNorthEast, AreaNameList.campManse, new Vector3Int(7,-1), 1, Facing.SouthWest));
        list.Add(new Transition(AreaNameList.guardHouseNorthEast, AreaNameList.campManse, new Vector3Int(0,4), 2, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.guardHouseNorthEast, AreaNameList.campManse, new Vector3Int(-1,4), 3, Facing.SouthEast));

        list.Add(new Transition(AreaNameList.guardHouseNorthEast, AreaNameList.guardHouseSouthWest, new Vector3Int(-3,-2), 0, Facing.NorthEast));
        list.Add(new Transition(AreaNameList.guardHouseNorthEast, AreaNameList.guardHouseSouthWest, new Vector3Int(-3,-1), 1, Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.guardHouseNorthEast, list);

        #endregion

        #region GuardHouseSW

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.guardHouseSouthWest, AreaNameList.campMineEntrance, new Vector3Int(-17,-2), 0, Facing.NorthEast));
        list.Add(new Transition(AreaNameList.guardHouseSouthWest, AreaNameList.campMineEntrance, new Vector3Int(-17,-1), 1, Facing.NorthEast));

        list.Add(new Transition(AreaNameList.guardHouseSouthWest, AreaNameList.guardHouseTopFloor, new Vector3Int(-11,0), 0, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.guardHouseSouthWest, AreaNameList.guardHouseTopFloor, new Vector3Int(-10,0), 1, Facing.SouthEast));

        list.Add(new Transition(AreaNameList.guardHouseSouthWest, AreaNameList.guardHouseNorthEast, new Vector3Int(-5,-2), 0, Facing.SouthWest));
        list.Add(new Transition(AreaNameList.guardHouseSouthWest, AreaNameList.guardHouseNorthEast, new Vector3Int(-5,-1), 1, Facing.SouthWest));

        transitionSpawnInfoDict.Add(AreaNameList.guardHouseSouthWest, list);

        #endregion

        #region GuardHouseTopFloor

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.guardHouseTopFloor, AreaNameList.guardHouseSouthWest, new Vector3Int(-11,1), 0, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.guardHouseTopFloor, AreaNameList.guardHouseSouthWest, new Vector3Int(-10,1), 1, Facing.NorthWest, twoSpaceMultiplier));

        transitionSpawnInfoDict.Add(AreaNameList.guardHouseTopFloor, list);

        #endregion

        #region Stockhouse

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.stockhouse, AreaNameList.campMineEntrance, new Vector3Int(9, 6), 0, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.stockhouse, AreaNameList.campMineEntrance, new Vector3Int(8, 6), 1, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.stockhouse, AreaNameList.campMineEntrance, new Vector3Int(7, 6), 2, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.stockhouse, AreaNameList.campMineEntrance, new Vector3Int(3, 2), 3, Facing.NorthEast));
        list.Add(new Transition(AreaNameList.stockhouse, AreaNameList.campMineEntrance, new Vector3Int(3, 1), 4, Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.stockhouse, list);

        #endregion

        #region NECamp

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.campNorthEast, AreaNameList.slaveShackTwo, new Vector3Int(11, 3), 0, Facing.SouthWest, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campNorthEast, AreaNameList.slaveShackThree, new Vector3Int(5, 12), 0, Facing.SouthEast, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campNorthEast, AreaNameList.slaveShackSeven, new Vector3Int(3, -5), 0, Facing.NorthWest, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campNorthEast, AreaNameList.campCenter, new Vector3Int(-17, 1), 0, Facing.NorthEast));
        list.Add(new Transition(AreaNameList.campNorthEast, AreaNameList.campCenter, new Vector3Int(-17, 0), 1, Facing.NorthEast));
        list.Add(new Transition(AreaNameList.campNorthEast, AreaNameList.campCenter, new Vector3Int(-17, -1), 2, Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.campNorthEast, list);

        #endregion

        #region CenterCamp

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.slaveShackOne, new Vector3Int(8,-2), 0, Facing.SouthWest, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.guardShack, new Vector3Int(-4,-3), 0, Facing.NorthEast, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.temple, new Vector3Int(-9,9), 0, Facing.SouthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.temple, new Vector3Int(-6,13), 1, Facing.NorthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.temple, new Vector3Int(-6,14), 2, Facing.NorthEast, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.stables, new Vector3Int(4,14), 0, Facing.SouthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.stables, new Vector3Int(4,13), 1, Facing.SouthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.stables, new Vector3Int(7,9), 2, Facing.SouthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.stables, new Vector3Int(8,9), 3, Facing.SouthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.stables, new Vector3Int(9,14), 4, Facing.NorthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.stables, new Vector3Int(9,13), 5, Facing.NorthEast, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.campNorthEast, new Vector3Int(18,8), 0, Facing.SouthWest));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.campNorthEast, new Vector3Int(18,7), 1, Facing.SouthWest));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.campNorthEast, new Vector3Int(18,6), 2, Facing.SouthWest));

        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.campManse, new Vector3Int(1,24), 0, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.campManse, new Vector3Int(0,24), 1, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.campManse, new Vector3Int(-1,24), 2, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.campManse, new Vector3Int(-2,24), 3, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.campManse, new Vector3Int(-3,24), 4, Facing.SouthEast));

        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.campSouthEast, new Vector3Int(-17,1), 0, Facing.NorthEast));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.campSouthEast, new Vector3Int(-17,2), 1, Facing.NorthEast));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.campSouthEast, new Vector3Int(-17,3), 2, Facing.NorthEast));

        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.forest, new Vector3Int(1,-35), 0, Facing.NorthWest));
        list.Add(new Transition(AreaNameList.campCenter, AreaNameList.forest, new Vector3Int(2,-35), 1, Facing.NorthWest));

        transitionSpawnInfoDict.Add(AreaNameList.campCenter, list);

        #endregion

        #region SECamp

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.slaveShackFour, new Vector3Int(0,11), 0, Facing.NorthEast, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.slaveShackFive, new Vector3Int(12, 13), 0, Facing.SouthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.slaveShackFive, new Vector3Int(11, 13), 1, Facing.SouthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.slaveShackFive, new Vector3Int(10, 16), 2, Facing.SouthWest, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.slaveShackSix, new Vector3Int(15, 8), 0, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.slaveShackSix, new Vector3Int(14, 8), 1, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.slaveShackSix, new Vector3Int(10, 6), 2, Facing.SouthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.slaveShackSix, new Vector3Int(15, 4), 3, Facing.SouthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.slaveShackSix, new Vector3Int(16, 4), 4, Facing.SouthEast, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.messHall, new Vector3Int(14,-6), 0, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.messHall, new Vector3Int(8,-6), 1, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.messHall, new Vector3Int(6,-9), 2, Facing.SouthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.messHall, new Vector3Int(6,-10), 3, Facing.SouthWest, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.campCenter, new Vector3Int(18,1), 0, Facing.SouthWest));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.campCenter, new Vector3Int(18,0), 1, Facing.SouthWest));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.campCenter, new Vector3Int(18,-1), 2, Facing.SouthWest));

        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.campMineEntrance, new Vector3Int(6,22), 0, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.campMineEntrance, new Vector3Int(5,22), 1, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.campSouthEast, AreaNameList.campMineEntrance, new Vector3Int(4,22), 2, Facing.SouthEast));

        transitionSpawnInfoDict.Add(AreaNameList.campSouthEast, list);

        #endregion

        #region MineEntranceCamp

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.guardHouseSouthWest, new Vector3Int(12,-15), 0, Facing.SouthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.guardHouseSouthWest, new Vector3Int(12,-14), 1, Facing.SouthWest, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.stockhouse, new Vector3Int(16,3), 0, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.stockhouse, new Vector3Int(15,3), 1, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.stockhouse, new Vector3Int(14,3), 2, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.stockhouse, new Vector3Int(12,0), 3, Facing.SouthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.stockhouse, new Vector3Int(12,-1), 4, Facing.SouthWest, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.campSouthEast, new Vector3Int(9,-19), 0, Facing.NorthWest));
        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.campSouthEast, new Vector3Int(8,-19), 1, Facing.NorthWest));
        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.campSouthEast, new Vector3Int(7,-19), 2, Facing.NorthWest));

        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.mineLvl1 + AreaNameList.section1a, new Vector3Int(11,16), 0, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.mineLvl1 + AreaNameList.section1a, new Vector3Int(10,16), 1, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.mineLvl1 + AreaNameList.section1a, new Vector3Int(9,16), 2, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.mineLvl1 + AreaNameList.section1a, new Vector3Int(8,16), 3, Facing.SouthEast));
        list.Add(new Transition(AreaNameList.campMineEntrance, AreaNameList.mineLvl1 + AreaNameList.section1a, new Vector3Int(7,16), 4, Facing.SouthEast));

        transitionSpawnInfoDict.Add(AreaNameList.campMineEntrance, list);

        #endregion

        #region ManseCamp

        list = new List<Transition>();

        list.Add(new Transition(AreaNameList.campManse, AreaNameList.guardHouseNorthEast, new Vector3Int(-2,-17), 0, Facing.NorthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.guardHouseNorthEast, new Vector3Int(-2,-16), 1, Facing.NorthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.guardHouseNorthEast, new Vector3Int(-10,-14), 2, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.guardHouseNorthEast, new Vector3Int(-11,-14), 3, Facing.NorthWest, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campManse, AreaNameList.slaveShackEight, new Vector3Int(8,-14), 0, Facing.SouthWest, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campManse, AreaNameList.slaveShackNine, new Vector3Int(-8,-5), 0, Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.slaveShackNine, new Vector3Int(-6,-7), 1, Facing.NorthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.slaveShackNine, new Vector3Int(-8,-9), 2, Facing.SouthEast, twoSpaceMultiplier));

        list.Add(new Transition(AreaNameList.campManse, AreaNameList.campCenter, new Vector3Int(5,-22), 0, Facing.NorthWest));
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.campCenter, new Vector3Int(4,-22), 1, Facing.NorthWest));
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.campCenter, new Vector3Int(3,-22), 2, Facing.NorthWest));
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.campCenter, new Vector3Int(2,-22), 3, Facing.NorthWest));
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.campCenter, new Vector3Int(1,-22), 4, Facing.NorthWest));

        list.Add(new Transition(AreaNameList.campManse, AreaNameList.manseFirstFloor + AreaNameList.section1a, new Vector3Int(3,10), 0, Facing.SouthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.manseFirstFloor + AreaNameList.section1a, new Vector3Int(2,10), 1, Facing.SouthEast, twoSpaceMultiplier));
        
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.manseFirstFloor + AreaNameList.kitchens, new Vector3Int(-6,6), 0, Facing.SouthEast, twoSpaceMultiplier));
        list.Add(new Transition(AreaNameList.campManse, AreaNameList.manseFirstFloor + AreaNameList.kitchens, new Vector3Int(-7,6), 1, Facing.SouthEast, twoSpaceMultiplier));

        transitionSpawnInfoDict.Add(AreaNameList.campManse, list);

        #endregion
    }



}
