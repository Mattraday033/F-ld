using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransitionSpawnInfoList
{
    private const int twoSpaceMultiplier = 2;

    private const int sizeTwo = 2;
    private const int sizeThree = 3;
    private const int sizeFour = 4;
    private const int sizeFive = 5;

    private const int startingIndexOne = 1;
    private const int startingIndexTwo = 2;
    private const int startingIndexThree = 3;
    private const int startingIndexFour = 4;

    private static Dictionary<string, List<TransitionSpawnInfo>> transitionSpawnInfoDict;
    private static List<TransitionSpawnInfo> list;

    public static List<TransitionSpawnInfo> getTransitionSpawnInfo(string areaName)
    {
        if (!transitionSpawnInfoDict.ContainsKey(areaName))
        {
            return new List<TransitionSpawnInfo>();
        }

        return transitionSpawnInfoDict[areaName];
    }

    static TransitionSpawnInfoList()
    {
        transitionSpawnInfoDict = new Dictionary<string, List<TransitionSpawnInfo>>();

        #region Slave Shack 

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.slaveShackOne, AreaNameList.campCenter, new Vector3Int(3, 1), Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackOne, list);

        #endregion
        #region Slave Shack 2

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.slaveShackTwo, AreaNameList.campNorthEast, new Vector3Int(2, 1), Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackTwo, list);

        #endregion
        #region Slave Shack 3

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.slaveShackThree, AreaNameList.campNorthEast, new Vector3Int(4, 0), Facing.NorthWest));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackThree, list);

        #endregion
        #region Slave Shack 4

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.slaveShackFour, AreaNameList.campSouthEast, new Vector3Int(12, 11), Facing.SouthWest));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackFour, list);

        #endregion
        #region Slave Shack 5

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.slaveShackFive, AreaNameList.campSouthEast, new Vector3Int(2, -7), Facing.NorthWest, twoSpaceMultiplier, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.slaveShackFive, AreaNameList.campSouthEast, new Vector3Int(-3, -2), Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackFive, list);

        #endregion
        #region Slave Shack 6

        list = new List<TransitionSpawnInfo>();


        list.Add(new TransitionSpawnInfo(AreaNameList.slaveShackSix, AreaNameList.campSouthEast, new Vector3Int(5, 3), Facing.SouthEast, twoSpaceMultiplier, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.slaveShackSix, AreaNameList.campSouthEast, new Vector3Int(-3, -2), Facing.NorthEast));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexThree, AreaNameList.slaveShackSix, AreaNameList.campSouthEast, new Vector3Int(8, -7), Facing.NorthWest, twoSpaceMultiplier, sizeTwo, Axis.DescendingX));
        transitionSpawnInfoDict.Add(AreaNameList.slaveShackSix, list);

        #endregion
        #region Slave Shack 7

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.slaveShackSeven, AreaNameList.campNorthEast, new Vector3Int(5, 7), Facing.SouthEast));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackSeven, list);

        #endregion
        #region Slave Shack 8

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.slaveShackEight, AreaNameList.campManse, new Vector3Int(-1, -5), Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackEight, list);

        #endregion
        #region Slave Shack 9

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.slaveShackNine, AreaNameList.campManse, new Vector3Int(1, 3), Facing.SouthEast));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, AreaNameList.slaveShackNine, AreaNameList.campManse, new Vector3Int(6, -2), Facing.SouthWest));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.slaveShackNine, AreaNameList.campManse, new Vector3Int(0, -7), Facing.NorthWest));

        transitionSpawnInfoDict.Add(AreaNameList.slaveShackNine, list);

        #endregion

        #region MessHall

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.messHall, AreaNameList.campSouthEast, new Vector3Int(6, 14), Facing.SouthEast));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, AreaNameList.messHall, AreaNameList.campSouthEast, new Vector3Int(0, 14), Facing.SouthEast));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.messHall, AreaNameList.campSouthEast, new Vector3Int(-3, 9), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.messHall, list);

        #endregion
        #region Stables

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.stables, AreaNameList.campCenter, new Vector3Int(1, 4), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.stables, AreaNameList.campCenter, new Vector3Int(9, -5), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexFour, AreaNameList.stables, AreaNameList.campCenter, new Vector3Int(14, 4), Facing.SouthWest, sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.stables, list);

        #endregion
        #region Temple

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.temple, AreaNameList.campCenter, new Vector3Int(6, -5), Facing.NorthWest));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, AreaNameList.temple, AreaNameList.campCenter, new Vector3Int(12, 4), Facing.SouthWest, twoSpaceMultiplier, sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.temple, list);

        #endregion
        #region GuardShack

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.guardShack, AreaNameList.campCenter, new Vector3Int(11, 9), Facing.SouthWest));

        transitionSpawnInfoDict.Add(AreaNameList.guardShack, list);

        #endregion
        #region Stockhouse

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.stockhouse, AreaNameList.campMineEntrance, new Vector3Int(9, 6), Facing.SouthEast, sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexThree, AreaNameList.stockhouse, AreaNameList.campMineEntrance, new Vector3Int(3, 2), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.stockhouse, list);

        #endregion

        #region GuardHouseNE

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.guardHouseNorthEast, AreaNameList.campManse, new Vector3Int(7, -2), Facing.SouthWest, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.guardHouseNorthEast, AreaNameList.campManse, new Vector3Int(0, 4), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexFour, AreaNameList.guardHouseNorthEast, AreaNameList.guardHouseSouthWest, new Vector3Int(-3, -1), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.guardHouseNorthEast, list);

        #endregion
        #region GuardHouseSW

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.guardHouseSouthWest, AreaNameList.campMineEntrance, new Vector3Int(-17, -1), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.guardHouseSouthWest, AreaNameList.guardHouseTopFloor, new Vector3Int(-10, 0), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexFour, AreaNameList.guardHouseSouthWest, AreaNameList.guardHouseNorthEast, new Vector3Int(-5, -1), Facing.SouthWest, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.guardHouseSouthWest, list);

        #endregion
        #region GuardHouseTopFloor

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.guardHouseTopFloor, AreaNameList.guardHouseSouthWest, new Vector3Int(-10, 1), Facing.NorthWest, twoSpaceMultiplier, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.guardHouseTopFloor, list);

        #endregion

        #region NECamp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.campNorthEast, AreaNameList.slaveShackTwo, new Vector3Int(11, 3), Facing.SouthWest, twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(AreaNameList.campNorthEast, AreaNameList.slaveShackThree, new Vector3Int(5, 12), Facing.SouthEast, twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(AreaNameList.campNorthEast, AreaNameList.slaveShackSeven, new Vector3Int(3, -5), Facing.NorthWest, twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(AreaNameList.campNorthEast, AreaNameList.campCenter, new Vector3Int(-17, 1), Facing.NorthEast, sizeThree, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.campNorthEast, list);

        #endregion
        #region CenterCamp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.campCenter, AreaNameList.slaveShackOne, new Vector3Int(8, -2), Facing.SouthWest, twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(AreaNameList.campCenter, AreaNameList.guardShack, new Vector3Int(-4, -3), Facing.NorthEast, twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(AreaNameList.campCenter, AreaNameList.temple, new Vector3Int(-9, 9), Facing.SouthEast, twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, AreaNameList.campCenter, AreaNameList.temple, new Vector3Int(-6, 14), Facing.NorthEast, twoSpaceMultiplier, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.campCenter, AreaNameList.stables, new Vector3Int(4, 14), Facing.NorthEast, twoSpaceMultiplier, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.campCenter, AreaNameList.stables, new Vector3Int(8, 9), Facing.NorthWest, twoSpaceMultiplier, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexFour, AreaNameList.campCenter, AreaNameList.stables, new Vector3Int(9, 14), Facing.SouthWest, twoSpaceMultiplier, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.campCenter, AreaNameList.campNorthEast, new Vector3Int(18, 8), Facing.SouthWest, sizeThree, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.campCenter, AreaNameList.campManse, new Vector3Int(1, 24), Facing.SouthEast, sizeTwo, Axis.DescendingX));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.campCenter, AreaNameList.campManse, new Vector3Int(-1, 24), Facing.SouthEast, sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.campCenter, AreaNameList.campSouthEast, new Vector3Int(-17, 3), Facing.NorthEast, sizeThree, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.campCenter, AreaNameList.forest, new Vector3Int(2, -35), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.campCenter, list);

        #endregion
        #region SECamp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.campSouthEast, AreaNameList.slaveShackFour, new Vector3Int(0, 11), Facing.NorthEast, twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(AreaNameList.campSouthEast, AreaNameList.slaveShackFive, new Vector3Int(12, 13), Facing.SouthEast, twoSpaceMultiplier, sizeTwo, Axis.DescendingX));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.campSouthEast, AreaNameList.slaveShackFive, new Vector3Int(10, 16), Facing.SouthWest, twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(AreaNameList.campSouthEast, AreaNameList.slaveShackSix, new Vector3Int(15, 8), Facing.NorthWest, twoSpaceMultiplier, sizeTwo, Axis.DescendingX));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, AreaNameList.campSouthEast, AreaNameList.slaveShackSix, new Vector3Int(10, 6), Facing.SouthWest, twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.campSouthEast, AreaNameList.slaveShackSix, new Vector3Int(16, 4), Facing.SouthEast, twoSpaceMultiplier, sizeTwo, Axis.DescendingX));


        list.Add(new TransitionSpawnInfo(AreaNameList.campSouthEast, AreaNameList.messHall, new Vector3Int(6, 14), Facing.SouthEast));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, AreaNameList.campSouthEast, AreaNameList.messHall, new Vector3Int(0, 14), Facing.SouthEast));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.campSouthEast, AreaNameList.messHall, new Vector3Int(-3, 9), Facing.NorthEast, sizeTwo, Axis.DescendingY));


        list.Add(new TransitionSpawnInfo(AreaNameList.campSouthEast, AreaNameList.campCenter, new Vector3Int(18, 1), Facing.SouthWest, sizeThree, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.campSouthEast, AreaNameList.campMineEntrance, new Vector3Int(6, 22), Facing.SouthEast, sizeThree, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.campSouthEast, list);

        #endregion
        #region MineEntranceCamp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.campMineEntrance, AreaNameList.guardHouseSouthWest, new Vector3Int(12, -14), Facing.SouthWest, twoSpaceMultiplier, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.campMineEntrance, AreaNameList.stockhouse, new Vector3Int(16, 3), Facing.NorthWest, twoSpaceMultiplier, sizeThree, Axis.DescendingX));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexThree, AreaNameList.campMineEntrance, AreaNameList.stockhouse, new Vector3Int(12, 0), Facing.SouthWest, twoSpaceMultiplier, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.campMineEntrance, AreaNameList.campSouthEast, new Vector3Int(9, -19), Facing.NorthWest, sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.campMineEntrance, AreaNameList.mineLvl1 + AreaNameList.section1a, new Vector3Int(11, 16), Facing.SouthEast, sizeFive, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.campMineEntrance, list);

        #endregion
        #region ManseCamp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.campManse, AreaNameList.guardHouseNorthEast, new Vector3Int(-2, -16), Facing.NorthEast, twoSpaceMultiplier, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.campManse, AreaNameList.guardHouseNorthEast, new Vector3Int(-10, -14), Facing.NorthWest, twoSpaceMultiplier, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.campManse, AreaNameList.slaveShackEight, new Vector3Int(8, -14), Facing.SouthWest, twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(AreaNameList.campManse, AreaNameList.slaveShackNine, new Vector3Int(-8, -5), Facing.NorthWest, twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, AreaNameList.campManse, AreaNameList.slaveShackNine, new Vector3Int(-6, -7), Facing.NorthEast, twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.campManse, AreaNameList.slaveShackNine, new Vector3Int(-8, -9), Facing.SouthEast, twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(AreaNameList.campManse, AreaNameList.campCenter, new Vector3Int(5, -22), Facing.NorthWest, sizeFive, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.campManse, AreaNameList.manseFirstFloor + AreaNameList.section1a, new Vector3Int(3, 10), Facing.SouthEast, twoSpaceMultiplier, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.campManse, AreaNameList.manseFirstFloor + AreaNameList.kitchens, new Vector3Int(-6, 6), Facing.SouthEast, twoSpaceMultiplier, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.campManse, list);

        #endregion

        #region MineLvl_1-1a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl1 + AreaNameList.section1a, AreaNameList.campMineEntrance, new Vector3Int(6, 0), Facing.NorthWest, sizeFive, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl1 + AreaNameList.section1a, AreaNameList.mineLvl1 + AreaNameList.section1b, new Vector3Int(5, 14), Facing.SouthEast, sizeThree, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl1 + AreaNameList.section1a, list);

        #endregion
        #region MineLvl_1-1b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl1 + AreaNameList.section1b, AreaNameList.mineLvl1 + AreaNameList.section1a, new Vector3Int(9, -11), Facing.NorthWest, sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl1 + AreaNameList.section1b, AreaNameList.mineLvl1 + AreaNameList.section1c, new Vector3Int(17, 6), Facing.SouthWest, twoSpaceMultiplier, sizeThree, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl1 + AreaNameList.section1b, AreaNameList.mineLvl2 + AreaNameList.section1a, new Vector3Int(9, 11), Facing.SouthEast, sizeThree, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl1 + AreaNameList.section1b, list);

        #endregion
        #region MineLvl_1-1c

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl1 + AreaNameList.section1c, AreaNameList.mineLvl1 + AreaNameList.section1b, new Vector3Int(-10, 2), Facing.NorthEast, sizeThree, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl1 + AreaNameList.section1c, list);

        #endregion

        #region MineLvl_2-1a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section1a, AreaNameList.mineLvl1 + AreaNameList.section1b, new Vector3Int(2, -6), Facing.NorthWest, sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section1a, AreaNameList.mineLvl2 + AreaNameList.section6, new Vector3Int(8, 7), Facing.SouthEast));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section1a, AreaNameList.mineLvl2 + AreaNameList.section1b, new Vector3Int(2, 18), Facing.SouthEast, sizeThree, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section1a, list);

        #endregion
        #region MineLvl_2-1b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section1b, AreaNameList.mineLvl2 + AreaNameList.section1a, new Vector3Int(0, -11), Facing.NorthWest, sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section1b, AreaNameList.mineLvl2 + AreaNameList.section1c, new Vector3Int(-5, -2), Facing.NorthEast, sizeTwo, Axis.DescendingY));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.mineLvl2 + AreaNameList.section1b, AreaNameList.mineLvl2 + AreaNameList.section1c, new Vector3Int(-5, 17), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section1b, AreaNameList.mineLvl2 + AreaNameList.section2a, new Vector3Int(0, 21), Facing.SouthEast, sizeThree, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section1b, list);

        #endregion
        #region MineLvl_2-1c

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section1c, AreaNameList.mineLvl2 + AreaNameList.section1b, new Vector3Int(10, -8), Facing.SouthWest, sizeTwo, Axis.DescendingY));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.mineLvl2 + AreaNameList.section1c, AreaNameList.mineLvl2 + AreaNameList.section1b, new Vector3Int(10, 11), Facing.SouthWest, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section1c, AreaNameList.mineLvl2 + AreaNameList.section2a, new Vector3Int(4, 18), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section1c, list);

        #endregion
        #region MineLvl_2-2a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section2a, AreaNameList.mineLvl2 + AreaNameList.section1b, new Vector3Int(12, -5), Facing.NorthWest, sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section2a, AreaNameList.mineLvl2 + AreaNameList.section1c, new Vector3Int(-2, -5), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section2a, AreaNameList.mineLvl2 + AreaNameList.section2b, new Vector3Int(12, 5), Facing.SouthEast, sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section2a, AreaNameList.mineLvl3 + AreaNameList.section1a, new Vector3Int(-5, 8), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section2a, list);

        #endregion
        #region MineLvl_2-2b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section2b, AreaNameList.mineLvl2 + AreaNameList.section2a, new Vector3Int(-9, 5), Facing.NorthWest, sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section2b, AreaNameList.mineLvl2 + AreaNameList.section3a, new Vector3Int(13, 9), Facing.SouthWest, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section2b, AreaNameList.mineLvl2 + AreaNameList.section7b, new Vector3Int(5, -9), Facing.NorthWest));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section2b, list);

        #endregion
        #region MineLvl_2-3a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section3a, AreaNameList.mineLvl2 + AreaNameList.section2b, new Vector3Int(-4, 7), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section3a, AreaNameList.mineLvl2 + AreaNameList.section3b, new Vector3Int(8, 5), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section3a, AreaNameList.mineLvl2 + AreaNameList.section5a, new Vector3Int(-1, -15), Facing.NorthWest, sizeThree, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section3a, list);

        #endregion
        #region MineLvl_2-3b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section3b, AreaNameList.mineLvl2 + AreaNameList.section3a, new Vector3Int(-11, 5), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section3b, AreaNameList.mineLvl2 + AreaNameList.section4, new Vector3Int(-11, -11), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section3b, AreaNameList.mineLvl2 + AreaNameList.section7a, new Vector3Int(8, -5), Facing.NorthWest));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section3b, list);

        #endregion
        #region MineLvl_2-4

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section4, AreaNameList.mineLvl2 + AreaNameList.section3b, new Vector3Int(3, 13), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section4, AreaNameList.mineLvl2 + AreaNameList.section7a, new Vector3Int(-3, -7), Facing.NorthEast));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section4, list);

        #endregion

        #region MineLvl_2-5a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section5a, AreaNameList.mineLvl2 + AreaNameList.section3a, new Vector3Int(-18, 13), Facing.SouthEast, sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section5a, AreaNameList.mineLvl2 + AreaNameList.section5b, new Vector3Int(10, 7), Facing.SouthEast));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, AreaNameList.mineLvl2 + AreaNameList.section5a, AreaNameList.mineLvl2 + AreaNameList.section5b, new Vector3Int(10, -14), Facing.NorthWest));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.mineLvl2 + AreaNameList.section5a, AreaNameList.mineLvl2 + AreaNameList.section5b, new Vector3Int(-19, -14), Facing.NorthWest));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section5a, AreaNameList.mineLvl2 + AreaNameList.section7a, new Vector3Int(20, -4), Facing.SouthWest));


        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section5a, list);

        #endregion
        #region MineLvl_2-5b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section5b, AreaNameList.mineLvl2 + AreaNameList.section5a, new Vector3Int(17, 11), Facing.SouthWest));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, AreaNameList.mineLvl2 + AreaNameList.section5b, AreaNameList.mineLvl2 + AreaNameList.section5a, new Vector3Int(17, -11), Facing.SouthWest));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, AreaNameList.mineLvl2 + AreaNameList.section5b, AreaNameList.mineLvl2 + AreaNameList.section5a, new Vector3Int(-13, -11), Facing.NorthWest));


        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section5b, list);

        #endregion

        #region MineLvl_2-6

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section6, AreaNameList.mineLvl2 + AreaNameList.section1a, new Vector3Int(-5, 5), Facing.NorthEast));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section6, AreaNameList.mineLvl2 + AreaNameList.section7a, new Vector3Int(13, 5), Facing.SouthEast));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section6, list);

        #endregion
        #region MineLvl_2-7a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section7a, AreaNameList.mineLvl2 + AreaNameList.section3b, new Vector3Int(6, 1), Facing.SouthWest));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section7a, AreaNameList.mineLvl2 + AreaNameList.section4, new Vector3Int(6, -4), Facing.SouthWest));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section7a, AreaNameList.mineLvl2 + AreaNameList.section5a, new Vector3Int(0, 8), Facing.SouthWest));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section7a, AreaNameList.mineLvl2 + AreaNameList.section6, new Vector3Int(0, -10), Facing.SouthWest));


        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section7a, AreaNameList.mineLvl2 + AreaNameList.section7b, new Vector3Int(-6, -1), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section7a, list);

        #endregion
        #region MineLvl_2-7b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section7b, AreaNameList.mineLvl2 + AreaNameList.section7a, new Vector3Int(6, 3), Facing.SouthWest, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl2 + AreaNameList.section7b, AreaNameList.mineLvl2 + AreaNameList.section2b, new Vector3Int(-2, 9), Facing.SouthWest));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl2 + AreaNameList.section7b, list);

        #endregion

        #region MineLvl_3-1a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section1a, AreaNameList.mineLvl2 + AreaNameList.section2a, new Vector3Int(8, 13), Facing.SouthWest, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section1a, AreaNameList.mineLvl3 + AreaNameList.section1b, new Vector3Int(-4, 5), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section1a, AreaNameList.mineLvl3 + AreaNameList.section2a, new Vector3Int(-17, 13), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section1a, AreaNameList.mineLvl3 + AreaNameList.section4a, new Vector3Int(-8, 21), Facing.SouthEast, sizeTwo, Axis.DescendingX));


        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.section1a, list);

        #endregion
        #region MineLvl_3-1b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section1b, AreaNameList.mineLvl3 + AreaNameList.section1a, new Vector3Int(-4, 3), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.section1b, list);

        #endregion
        #region MineLvl_3-2a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section2a, AreaNameList.mineLvl3 + AreaNameList.section1a, new Vector3Int(11, 3), Facing.SouthWest, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section2a, AreaNameList.mineLvl3 + AreaNameList.section2b, new Vector3Int(4, 10), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.section2a, list);

        #endregion
        #region MineLvl_3-2b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section2b, AreaNameList.mineLvl3 + AreaNameList.section2a, new Vector3Int(4, -8), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section2b, AreaNameList.mineLvl3 + AreaNameList.section3a, new Vector3Int(3, 12), Facing.SouthEast, sizeTwo, Axis.DescendingX));
        
        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section2b, AreaNameList.mineLvl3 + AreaNameList.section3b, new Vector3Int(-7, 2), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.section2b, list);

        #endregion
        #region MineLvl_3-3a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section3a, AreaNameList.mineLvl3 + AreaNameList.section6a, new Vector3Int(19, 11), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section3a, AreaNameList.mineLvl3 + AreaNameList.section2b, new Vector3Int(19, -2), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section3a, AreaNameList.mineLvl3 + AreaNameList.section3b, new Vector3Int(3, 1), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section3a, AreaNameList.mineLvl3 + AreaNameList.section7, new Vector3Int(-20, 7), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.section3a, list);

        #endregion
        #region MineLvl_3-3b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section3b, AreaNameList.mineLvl3 + AreaNameList.section2b, new Vector3Int(9, 0), Facing.SouthWest, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section3b, AreaNameList.mineLvl3 + AreaNameList.section3a, new Vector3Int(5, 13), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.section3b, list);

        #endregion
        #region MineLvl_3-4a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section4a, AreaNameList.mineLvl3 + AreaNameList.section1a, new Vector3Int(2, -7), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section4a, AreaNameList.mineLvl3 + AreaNameList.section4b, new Vector3Int(2, 12), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.section4a, list);

        #endregion
        #region MineLvl_3-4b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section4b, AreaNameList.mineLvl3 + AreaNameList.section4a, new Vector3Int(0, -12), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section4b, AreaNameList.mineLvl3 + AreaNameList.section5, new Vector3Int(0, 17), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.section4b, list);

        #endregion
        #region MineLvl_3-5

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section5, AreaNameList.mineLvl3 + AreaNameList.section4b, new Vector3Int(1, -6), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section5, AreaNameList.mineLvl3 + AreaNameList.minerCamp, new Vector3Int(-7, -3), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section5, AreaNameList.mineLvl3 + AreaNameList.section6a, new Vector3Int(-13, 2), Facing.NorthEast, sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.section5, list);

        #endregion
        #region MineLvl_3-Miner Camp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.minerCamp, AreaNameList.mineLvl3 + AreaNameList.section5, new Vector3Int(3, 5), Facing.SouthEast, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.minerCamp, list);

        #endregion
        #region MineLvl_3-6a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section6a, AreaNameList.mineLvl3 + AreaNameList.section5, new Vector3Int(12, 15), Facing.SouthWest, sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section6a, AreaNameList.mineLvl3 + AreaNameList.section3a, new Vector3Int(9, -9), Facing.NorthWest, sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.section6a, list);

        #endregion
        #region MineLvl_3-7

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(AreaNameList.mineLvl3 + AreaNameList.section7, AreaNameList.mineLvl3 + AreaNameList.section3a, new Vector3Int(16, -3), Facing.SouthWest, sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(AreaNameList.mineLvl3 + AreaNameList.section7, list);

        #endregion
    }



}

public enum Axis { DescendingX = 0, DescendingY = 1}

public class TransitionSpawnInfo
{
    private string currentArea;
    private string destinationArea;

    private Vector3Int startCell;

    private Facing playerSpawnDirection;
    private int outputMultiplier;

    private int size;
    private Axis axis;

    public TransitionSpawnInfo(string currentArea, string destinationArea, Vector3Int startCell, Facing playerSpawnDirection)
    {
        this.currentArea = currentArea;
        this.destinationArea = destinationArea;

        this.startCell = startCell;
        this.playerSpawnDirection = playerSpawnDirection;

        this.outputMultiplier = 1;
        this.size = 1;
        this.axis = Axis.DescendingX;
    }

    public TransitionSpawnInfo(string currentArea, string destinationArea, Vector3Int startCell, Facing playerSpawnDirection, int outputMultiplier)
    {
        this.currentArea = currentArea;
        this.destinationArea = destinationArea;

        this.startCell = startCell;
        this.playerSpawnDirection = playerSpawnDirection;
        this.outputMultiplier = outputMultiplier;

        this.size = 1;
        this.axis = Axis.DescendingX;
    }

    public TransitionSpawnInfo(string currentArea, string destinationArea, Vector3Int startCell, Facing playerSpawnDirection, int size, Axis axis)
    {
        this.currentArea = currentArea;
        this.destinationArea = destinationArea;

        this.startCell = startCell;
        this.playerSpawnDirection = playerSpawnDirection;

        this.size = size;
        this.axis = axis;

        this.outputMultiplier = 1;
    }

    public TransitionSpawnInfo(string currentArea, string destinationArea, Vector3Int startCell, Facing playerSpawnDirection, int outputMultiplier, int size, Axis axis)
    {
        this.currentArea = currentArea;
        this.destinationArea = destinationArea;

        this.startCell = startCell;
        this.playerSpawnDirection = playerSpawnDirection;

        this.outputMultiplier = outputMultiplier;
        this.size = size;
        this.axis = axis;
    }

    public virtual int getStartIndex()
    {
        return 0;
    }

    public List<Transition> getTransitions()
    {
        List<Transition> list = new List<Transition>();

        for (int index = 0; index < size; index++)
        {
            Vector3Int currentCell = startCell;

            if (axis == Axis.DescendingX)
            {
                currentCell.x -= index;
            }
            else if (axis == Axis.DescendingY)
            {
                currentCell.y -= index;
            }

            list.Add(new Transition(currentArea, destinationArea, currentCell, index + getStartIndex(), playerSpawnDirection, outputMultiplier));
        }

        return list;
    }

}

public class TransitionSpawnInfoWithCorner : TransitionSpawnInfo
{

    private int startIndex;

    public TransitionSpawnInfoWithCorner(int startIndex, string currentArea, string destinationArea, Vector3Int startCell, Facing playerSpawnDirection):
    base(currentArea, destinationArea, startCell, playerSpawnDirection)
    {
        this.startIndex = startIndex;
    }

    public TransitionSpawnInfoWithCorner(int startIndex, string currentArea, string destinationArea, Vector3Int startCell, Facing playerSpawnDirection, int outputMultiplier):
    base(currentArea, destinationArea, startCell, playerSpawnDirection, outputMultiplier)
    {
        this.startIndex = startIndex;
    }

    public TransitionSpawnInfoWithCorner(int startIndex, string currentArea, string destinationArea, Vector3Int startCell, Facing playerSpawnDirection, int size, Axis axis):
    base(currentArea, destinationArea, startCell, playerSpawnDirection, size, axis)
    {
        this.startIndex = startIndex;
    }

    public TransitionSpawnInfoWithCorner(int startIndex, string currentArea, string destinationArea, Vector3Int startCell, Facing playerSpawnDirection, int outputMultiplier, int size, Axis axis):
    base(currentArea, destinationArea, startCell, playerSpawnDirection, outputMultiplier, size, axis)
    {
        this.startIndex = startIndex;
    }

    public override int getStartIndex()
    {
        return startIndex;
    }

}
