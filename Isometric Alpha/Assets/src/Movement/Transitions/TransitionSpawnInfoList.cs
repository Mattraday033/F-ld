using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransitionSpawnInfoList
{
    private const bool sendingMatrix = false;
    private const bool receivingMatrix = true;

    private const int twoSpaceMultiplier = 2;

    private const int startingIndexOne = 1;
    private const int startingIndexTwo = 2;
    private const int startingIndexThree = 3;
    private const int startingIndexFour = 4;

    private static Dictionary<string, List<TransitionSpawnInfo>> transitionSpawnInfoDict;

    public static List<TransitionSpawnInfo> getTransitionSpawnInfo(string areaName)
    {
        if (!transitionSpawnInfoDict.ContainsKey(areaName))
        {
            return new List<TransitionSpawnInfo>();
        }

        return transitionSpawnInfoDict[areaName];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateTransitionSpawnInfoList()
    {
        transitionSpawnInfoDict = new Dictionary<string, List<TransitionSpawnInfo>>();
        List<TransitionSpawnInfo> list;

        #region Camp Interiors

        #region Slave Shack 

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.slaveShackOne, LocationNameList.campCenter, new Vector3Int(3, 1), Facing.NorthEast));

        transitionSpawnInfoDict.Add(LocationNameList.slaveShackOne, list);

        #endregion
        #region Slave Shack 2

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.slaveShackTwo, LocationNameList.campNorthEast, new Vector3Int(1, 1), Facing.NorthEast));

        transitionSpawnInfoDict.Add(LocationNameList.slaveShackTwo, list);

        #endregion
        #region Slave Shack 3

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.slaveShackThree, LocationNameList.campNorthEast, new Vector3Int(4, 0), Facing.NorthWest));

        transitionSpawnInfoDict.Add(LocationNameList.slaveShackThree, list);

        #endregion
        #region Slave Shack 4

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.slaveShackFour, LocationNameList.campSouthEast, new Vector3Int(12, 11), Facing.SouthWest));

        transitionSpawnInfoDict.Add(LocationNameList.slaveShackFour, list);

        #endregion
        #region Slave Shack 5

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.slaveShackFive, LocationNameList.campSouthEast, new Vector3Int(2, -7), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.slaveShackFive, LocationNameList.campSouthEast, new Vector3Int(-3, -2), Facing.NorthEast));

        transitionSpawnInfoDict.Add(LocationNameList.slaveShackFive, list);

        #endregion
        #region Slave Shack 6

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.slaveShackSix, LocationNameList.campSouthEast, new Vector3Int(5, 3), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.slaveShackSix, LocationNameList.campSouthEast, new Vector3Int(-3, -2), Facing.NorthEast));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexThree, LocationNameList.slaveShackSix, LocationNameList.campSouthEast, new Vector3Int(8, -7), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));

        transitionSpawnInfoDict.Add(LocationNameList.slaveShackSix, list);

        #endregion
        #region Slave Shack 7

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.slaveShackSeven, LocationNameList.campNorthEast, new Vector3Int(2, 6), Facing.NorthEast));

        transitionSpawnInfoDict.Add(LocationNameList.slaveShackSeven, list);

        #endregion
        #region Slave Shack 8

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.slaveShackEight, LocationNameList.campManse, new Vector3Int(-1, -5), Facing.NorthEast));

        transitionSpawnInfoDict.Add(LocationNameList.slaveShackEight, list);

        #endregion
        #region Slave Shack 9

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.slaveShackNine, LocationNameList.campManse, new Vector3Int(1, 3), Facing.SouthEast));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, LocationNameList.slaveShackNine, LocationNameList.campManse, new Vector3Int(6, -2), Facing.SouthWest));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.slaveShackNine, LocationNameList.campManse, new Vector3Int(0, -7), Facing.NorthWest));

        transitionSpawnInfoDict.Add(LocationNameList.slaveShackNine, list);

        #endregion

        #region MessHall

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.messHall, LocationNameList.campSouthEast, new Vector3Int(6, 14), Facing.SouthEast));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, LocationNameList.messHall, LocationNameList.campSouthEast, new Vector3Int(0, 14), Facing.SouthEast));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.messHall, LocationNameList.campSouthEast, new Vector3Int(-3, 9), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY, scriptOnTransition: new GuardPunishmentTransitionScript()));

        transitionSpawnInfoDict.Add(LocationNameList.messHall, list);

        #endregion
        #region Stables

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.stables, LocationNameList.campCenter, new Vector3Int(1, 4), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.stables, LocationNameList.campCenter, new Vector3Int(9, -5), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexFour, LocationNameList.stables, LocationNameList.campCenter, new Vector3Int(14, 4), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(LocationNameList.stables, list);

        #endregion
        #region Temple

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.temple, LocationNameList.campCenter, new Vector3Int(6, -5), Facing.NorthWest));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, LocationNameList.temple, LocationNameList.campCenter, new Vector3Int(12, 4), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY, outputMultiplier: twoSpaceMultiplier));

        transitionSpawnInfoDict.Add(LocationNameList.temple, list);

        #endregion
        #region GuardShack

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.guardShack, LocationNameList.campCenter, new Vector3Int(11, 9), Facing.SouthEast));

        transitionSpawnInfoDict.Add(LocationNameList.guardShack, list);

        #endregion
        #region Stockhouse

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.stockhouse, LocationNameList.campMineEntrance, new Vector3Int(9, 6), Facing.SouthEast, Constants.sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexThree, LocationNameList.stockhouse, LocationNameList.campMineEntrance, new Vector3Int(3, 2), Facing.NorthEast, Constants.sizeThree, Axis.DescendingY));

        transitionSpawnInfoDict.Add(LocationNameList.stockhouse, list);

        #endregion

        #region GuardHouseNE

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.guardHouseNorthEast, LocationNameList.campManse, new Vector3Int(7, -1), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.guardHouseNorthEast, LocationNameList.campManse, new Vector3Int(0, 4), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(LocationNameList.guardHouseNorthEast, LocationNameList.guardHouseSouthWest, new Vector3Int(-3, -1), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(LocationNameList.guardHouseNorthEast, list);

        #endregion
        #region GuardHouseSW

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.guardHouseSouthWest, LocationNameList.campMineEntrance, new Vector3Int(-17, -1), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(LocationNameList.guardHouseSouthWest, LocationNameList.guardHouseTopFloor, new Vector3Int(-10, 0), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(LocationNameList.guardHouseSouthWest, LocationNameList.guardHouseNorthEast, new Vector3Int(-4, -1), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(LocationNameList.guardHouseSouthWest, list);

        #endregion
        #region GuardHouseTopFloor

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.guardHouseTopFloor, LocationNameList.guardHouseSouthWest, new Vector3Int(-10, 1), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));

        transitionSpawnInfoDict.Add(LocationNameList.guardHouseTopFloor, list);

        #endregion

        #endregion

        #region Camp Exteriors

        #region NECamp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.campNorthEast, LocationNameList.slaveShackTwo, new Vector3Int(13, 5), Facing.SouthWest, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campNorthEast, LocationNameList.slaveShackThree, new Vector3Int(3, 12), Facing.SouthEast, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campNorthEast, LocationNameList.slaveShackSeven, new Vector3Int(-1, -12), Facing.SouthWest, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campNorthEast, LocationNameList.campCenter, new Vector3Int(-17, 1), Facing.NorthEast, Constants.sizeThree, Axis.DescendingY));

        list.Add(new FastTravelTransitionSpawnInfo(new Vector3Int(-2, -1), Facing.SouthWest));

        transitionSpawnInfoDict.Add(LocationNameList.campNorthEast, list);

        #endregion
        #region CenterCamp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.campCenter, LocationNameList.slaveShackOne, new Vector3Int(10, -3), Facing.SouthWest, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campCenter, LocationNameList.guardShack, new Vector3Int(-11, -6), Facing.NorthWest, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campCenter, LocationNameList.temple, new Vector3Int(-10, 9), Facing.SouthEast, outputMultiplier: twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, LocationNameList.campCenter, LocationNameList.temple, new Vector3Int(-8, 14), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campCenter, LocationNameList.stables, new Vector3Int(4, 18), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY, outputMultiplier: twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.campCenter, LocationNameList.stables, new Vector3Int(8, 13), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexFour, LocationNameList.campCenter, LocationNameList.stables, new Vector3Int(10, 18), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campCenter, LocationNameList.campNorthEast, new Vector3Int(23, 9), Facing.SouthWest, Constants.sizeThree, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(LocationNameList.campCenter, LocationNameList.campManse, new Vector3Int(0, 24), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.campCenter, LocationNameList.campManse, new Vector3Int(-2, 24), Facing.SouthEast, Constants.sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(LocationNameList.campCenter, LocationNameList.campSouthEast, new Vector3Int(-17, 4), Facing.NorthEast, Constants.sizeThree, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(LocationNameList.campCenter, ZoneKeyList.forest, new Vector3Int(2, -35), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(LocationNameList.campCenter, list);

        #endregion
        #region SECamp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.campSouthEast, LocationNameList.slaveShackFour, new Vector3Int(-1, 4), Facing.NorthEast, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campSouthEast, LocationNameList.slaveShackFive, new Vector3Int(18, 16), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.campSouthEast, LocationNameList.slaveShackFive, new Vector3Int(15, 17), Facing.SouthWest, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campSouthEast, LocationNameList.slaveShackSix, new Vector3Int(19, 7), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.campSouthEast, LocationNameList.slaveShackSix, new Vector3Int(15, 6), Facing.SouthWest, outputMultiplier: twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexThree, LocationNameList.campSouthEast, LocationNameList.slaveShackSix, new Vector3Int(20, 5), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campSouthEast, LocationNameList.messHall, new Vector3Int(20, -5), Facing.NorthWest, outputMultiplier: twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, LocationNameList.campSouthEast, LocationNameList.messHall, new Vector3Int(13, -5), Facing.NorthWest, outputMultiplier: twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.campSouthEast, LocationNameList.messHall, new Vector3Int(12, -7), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campSouthEast, LocationNameList.campCenter, new Vector3Int(29, 1), Facing.SouthWest, Constants.sizeThree, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(LocationNameList.campSouthEast, LocationNameList.campMineEntrance, new Vector3Int(9, 27), Facing.SouthEast, Constants.sizeThree, Axis.DescendingX));

        list.Add(new FastTravelTransitionSpawnInfo(new Vector3Int(12, 2), Facing.SouthWest));

        transitionSpawnInfoDict.Add(LocationNameList.campSouthEast, list);

        #endregion
        #region MineEntranceCamp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.campMineEntrance, LocationNameList.guardHouseSouthWest, new Vector3Int(12, -14), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campMineEntrance, LocationNameList.stockhouse, new Vector3Int(17, 6), Facing.NorthWest, Constants.sizeThree, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexThree, LocationNameList.campMineEntrance, LocationNameList.stockhouse, new Vector3Int(15, 4), Facing.SouthWest, Constants.sizeThree, Axis.DescendingY, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campMineEntrance, LocationNameList.campSouthEast, new Vector3Int(9, -19), Facing.NorthWest, Constants.sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(LocationNameList.campMineEntrance, ZoneKeyList.mineLvl1 + LocationNameList.section1a, new Vector3Int(11, 16), Facing.SouthEast, Constants.sizeFive, Axis.DescendingX));

        transitionSpawnInfoDict.Add(LocationNameList.campMineEntrance, list);

        #endregion
        #region ManseCamp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.campManse, LocationNameList.guardHouseNorthEast, new Vector3Int(-2, -16), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.campManse, LocationNameList.guardHouseNorthEast, new Vector3Int(-10, -14), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campManse, LocationNameList.slaveShackEight, new Vector3Int(8, -14), Facing.SouthWest, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campManse, LocationNameList.slaveShackNine, new Vector3Int(-8, -5), Facing.NorthWest, outputMultiplier: twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexOne, LocationNameList.campManse, LocationNameList.slaveShackNine, new Vector3Int(-5, -6), Facing.NorthEast, outputMultiplier: twoSpaceMultiplier));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, LocationNameList.campManse, LocationNameList.slaveShackNine, new Vector3Int(-8, -7), Facing.SouthEast, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campManse, LocationNameList.campCenter, new Vector3Int(5, -23), Facing.NorthWest, Constants.sizeFive, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(LocationNameList.campManse, ZoneKeyList.manseFirstFloor + LocationNameList.section1a, new Vector3Int(3, 16), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campManse, ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, new Vector3Int(-6, 7), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(LocationNameList.campManse, LocationNameList.campNorthWest, new Vector3Int(18,-6), Facing.SouthWest, Constants.sizeThree, Axis.DescendingY));

        transitionSpawnInfoDict.Add(LocationNameList.campManse, list);

        #endregion

        #region NWCamp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(LocationNameList.campNorthWest, LocationNameList.campManse, new Vector3Int(-17, 1), Facing.NorthEast, Constants.sizeThree, Axis.DescendingY));

        transitionSpawnInfoDict.Add(LocationNameList.campNorthWest, list);

        #endregion

        #endregion

        #region Manse + Pit

        #region Manse-1F

        #region Manse-1F-1a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, LocationNameList.campManse, new Vector3Int(2, -8), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, ZoneKeyList.manseFirstFloor + LocationNameList.section1b, new Vector3Int(-5, 1), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, ZoneKeyList.manseSecondFloor + LocationNameList.section1a, new Vector3Int(0, 5), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, ZoneKeyList.manseFirstFloor + LocationNameList.section1a, ZoneKeyList.manseSecondFloor + LocationNameList.section1a, new Vector3Int(3, 5), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, ZoneKeyList.manseFirstFloor + LocationNameList.section3a, new Vector3Int(8, 1), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section1a, list);

        #endregion
        #region Manse-1F-1b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section1b, ZoneKeyList.manseFirstFloor + LocationNameList.section1a, new Vector3Int(9, 2), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section1b, ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, new Vector3Int(-13, 2), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section1b, ZoneKeyList.manseFirstFloor + LocationNameList.section1c, new Vector3Int(-2, -1), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section1b, list);

        #endregion
        #region Manse-1F-1c

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section1c, ZoneKeyList.manseFirstFloor + LocationNameList.section1b, new Vector3Int(22, 8), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section1c, ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, new Vector3Int(3, -10), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section1c, list);

        #endregion
        #region Manse-1F-Kitchens

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, LocationNameList.campManse, new Vector3Int(-2, -7), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, ZoneKeyList.manseFirstFloor + LocationNameList.section1c, new Vector3Int(2, -1), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, new Vector3Int(-2, 6), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        // list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, new Vector3Int(1, 4), Facing.SouthWest));

        list.Add(new FastTravelTransitionSpawnInfo(new Vector3Int(0, 0), Facing.SouthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, list);

        #endregion
        #region Manse-1F-Dining Room

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, ZoneKeyList.manseFirstFloor + LocationNameList.section2a, new Vector3Int(-1, 9), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, ZoneKeyList.manseFirstFloor + LocationNameList.section1b, new Vector3Int(3, -1), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, new Vector3Int(-1, -9), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, list);

        #endregion
        #region Manse-1F-2a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, new Vector3Int(2, -5), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, ZoneKeyList.manseFirstFloor + LocationNameList.section2b, new Vector3Int(9, 6), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, list);

        #endregion
        #region Manse-1F-2b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, ZoneKeyList.manseFirstFloor + LocationNameList.section2a, new Vector3Int(-9, 2), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, ZoneKeyList.manseFirstFloor + LocationNameList.section3c, new Vector3Int(14, 2), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, new Vector3Int(3, 4), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, ZoneKeyList.manseFirstFloor + LocationNameList.section2c, new Vector3Int(3, -1), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, ZoneKeyList.pit + LocationNameList.section1a, new Vector3Int(-4, -7), Facing.SouthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, list);

        #endregion
        #region Manse-1F-2c

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section2c, ZoneKeyList.manseFirstFloor + LocationNameList.section2b, new Vector3Int(0, 4), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        List<Vector3Int> m2F2bToM1F2cCoords = new List<Vector3Int>() { new Vector3Int(3,3), new Vector3Int(1,3), new Vector3Int(-2,3), new Vector3Int(-3,2), new Vector3Int(3,1),
                                                                        new Vector3Int(1,1), new Vector3Int(-1,1), new Vector3Int(-3,1), new Vector3Int(1,0), new Vector3Int(-1,0),
                                                                        new Vector3Int(-4,0), new Vector3Int(-2,-1), new Vector3Int(-4,-1), new Vector3Int(1,-2), new Vector3Int(-3,-2),
                                                                        new Vector3Int(3,-3), new Vector3Int(1,-3), new Vector3Int(-1,-3), new Vector3Int(2,-4), new Vector3Int(0,-4)};

        list.Add(new TransitionSpawnInfoMatrix(ZoneKeyList.manseFirstFloor + LocationNameList.section2c, ZoneKeyList.manseSecondFloor + LocationNameList.section2b, receivingMatrix, m2F2bToM1F2cCoords));

        List<Vector3Int> m1F2CToS2PCoords = new List<Vector3Int>() { new Vector3Int(-3, 3), new Vector3Int(3, 2), new Vector3Int(2, 2), new Vector3Int(1, 2), new Vector3Int(-4, 2),
                                                                     new Vector3Int(0, 1), new Vector3Int(-2, 1), new Vector3Int(-4, 1), new Vector3Int(0, 0), new Vector3Int(-2, 0), 
                                                                     new Vector3Int(-3, 0), new Vector3Int(1, -1), new Vector3Int(-1,-1), new Vector3Int(3,-2),new Vector3Int(2,-2), 
                                                                     new Vector3Int(0,-2), new Vector3Int(-4,-2), new Vector3Int(0,-3), new Vector3Int(-2,-3), new Vector3Int(-3,-3), 
                                                                     new Vector3Int(-1,-4), new Vector3Int(-2,-4), new Vector3Int(-4,-4)};

        list.Add(new TransitionSpawnInfoMatrix(ZoneKeyList.manseFirstFloor + LocationNameList.section2c, ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, sendingMatrix, m1F2CToS2PCoords));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2c, list);

        #endregion
        #region Manse-1F-StairsToPit

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, ZoneKeyList.manseFirstFloor + LocationNameList.section2b, new Vector3Int(0, 17), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        // list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, ZoneKeyList.pit + LocationNameList.section1b, new Vector3Int(-4, 1), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfoMatrix(ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, ZoneKeyList.manseFirstFloor + LocationNameList.section2c, receivingMatrix, m1F2CToS2PCoords));

        List<Vector3Int> s2PToP1aCoords = new List<Vector3Int>() { new Vector3Int(3,3), new Vector3Int(2,3), new Vector3Int(-4,3), new Vector3Int(0,2), new Vector3Int(-2,2),
                                                                   new Vector3Int(-3,2), new Vector3Int(1,1), new Vector3Int(-1,1), new Vector3Int(-3,1), new Vector3Int(3,0),
                                                                   new Vector3Int(1,0), new Vector3Int(-4,0), new Vector3Int(3,-1), new Vector3Int(2,-1), new Vector3Int(0,-1),
                                                                   new Vector3Int(-4,-1), new Vector3Int(1,-2), new Vector3Int(-1,-2), new Vector3Int(-2,-2), new Vector3Int(-3,-2),
                                                                   new Vector3Int(0,-4), new Vector3Int(-2,-4), new Vector3Int(-3,-4)};

        list.Add(new TransitionSpawnInfoMatrix(ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, ZoneKeyList.pit + LocationNameList.section1a, sendingMatrix, s2PToP1aCoords));

        list.Add(new TransitionSpawnInfoWithCorner(23, ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, ZoneKeyList.pit + LocationNameList.section1a, new Vector3Int(-10, 17), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));
        
        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, list);

        #endregion
        #region Manse-1F-3a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3a, ZoneKeyList.manseFirstFloor + LocationNameList.section1a, new Vector3Int(-5, 0), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3a, ZoneKeyList.manseSecondFloor + LocationNameList.office, new Vector3Int(14, 1), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3a, ZoneKeyList.manseFirstFloor + LocationNameList.section3b, new Vector3Int(6, 5), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3a, list);

        #endregion
        #region Manse-1F-3b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, ZoneKeyList.manseFirstFloor + LocationNameList.section3a, new Vector3Int(5, -5), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, ZoneKeyList.manseFirstFloor + LocationNameList.section3d, new Vector3Int(1, 1), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, ZoneKeyList.manseFirstFloor + LocationNameList.section3c, new Vector3Int(-1, 10), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3b, list);

        #endregion
        #region Manse-1F-3c

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3c, ZoneKeyList.manseFirstFloor + LocationNameList.section2b, new Vector3Int(0, 6), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3c, ZoneKeyList.manseFirstFloor + LocationNameList.section3e, new Vector3Int(3, -2), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3c, ZoneKeyList.manseFirstFloor + LocationNameList.section3b, new Vector3Int(9, 6), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3c, ZoneKeyList.manseSecondFloor + LocationNameList.section2a, new Vector3Int(8, 8), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3c, list);

        #endregion
        #region Manse-1F-3d

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3d, ZoneKeyList.manseFirstFloor + LocationNameList.section3b, new Vector3Int(4, 4), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3d, list);

        #endregion
        #region Manse-1F-3e

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3e, ZoneKeyList.manseFirstFloor + LocationNameList.section3c, new Vector3Int(4, 3), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3e, list);

        #endregion

        #endregion

        #region Manse-2F

        #region Manse-2F-1a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, ZoneKeyList.manseSecondFloor + LocationNameList.section2a, new Vector3Int(3, 9), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, ZoneKeyList.manseSecondFloor + LocationNameList.section1b, new Vector3Int(10, 6), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, ZoneKeyList.manseSecondFloor + LocationNameList.office, new Vector3Int(10, -2), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, ZoneKeyList.manseFirstFloor + LocationNameList.section1a, new Vector3Int(-1, 3), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, ZoneKeyList.manseSecondFloor + LocationNameList.section1a, ZoneKeyList.manseFirstFloor + LocationNameList.section1a, new Vector3Int(6, 3), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, ZoneKeyList.manseSecondFloor + LocationNameList.section1c, new Vector3Int(-5, -2), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, ZoneKeyList.manseSecondFloor + LocationNameList.section3c, new Vector3Int(-6, 6), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section1a, list);

        #endregion
        #region Manse-2F-1b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section1b, ZoneKeyList.manseSecondFloor + LocationNameList.section1a, new Vector3Int(-2, -1), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section1b, list);

        #endregion
        #region Manse-2F-1c

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section1c, ZoneKeyList.manseSecondFloor + LocationNameList.section1a, new Vector3Int(-1, 0), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section1c, list);

        #endregion
        #region Manse-2F-Office

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.office, ZoneKeyList.manseSecondFloor + LocationNameList.section1a, new Vector3Int(-9, -1), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.office, ZoneKeyList.manseFirstFloor + LocationNameList.section3a, new Vector3Int(11, -2), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.office, list);

        #endregion
        #region Manse-2F-2a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section2a, ZoneKeyList.manseFirstFloor + LocationNameList.section3c, new Vector3Int(8, 9), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section2a, ZoneKeyList.manseSecondFloor + LocationNameList.section2c, new Vector3Int(10, 7), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section2a, ZoneKeyList.manseSecondFloor + LocationNameList.section2d, new Vector3Int(10, -2), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section2a, ZoneKeyList.manseSecondFloor + LocationNameList.section2b, new Vector3Int(-6, 7), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section2a, ZoneKeyList.manseSecondFloor + LocationNameList.section1a, new Vector3Int(1, -5), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section2a, list);

        #endregion
        #region Manse-2F-2b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section2b, ZoneKeyList.manseSecondFloor + LocationNameList.section2a, new Vector3Int(4, 8), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section2b, ZoneKeyList.manseSecondFloor + LocationNameList.section3a, new Vector3Int(-5, 8), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfoMatrix(ZoneKeyList.manseSecondFloor + LocationNameList.section2b, ZoneKeyList.manseFirstFloor + LocationNameList.section2c, sendingMatrix, m2F2bToM1F2cCoords));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section2b, list);

        #endregion
        #region Manse-2F-2c

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section2c, ZoneKeyList.manseSecondFloor + LocationNameList.section2a, new Vector3Int(-4, -1), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section2c, list);

        #endregion
        #region Manse-2F-2d

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section2d, ZoneKeyList.manseSecondFloor + LocationNameList.section2a, new Vector3Int(-6, -4), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section2d, list);

        #endregion
        #region Manse-2F-3a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section3a, ZoneKeyList.manseSecondFloor + LocationNameList.section2b, new Vector3Int(9, 10), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section3a, ZoneKeyList.manseSecondFloor + LocationNameList.section3b, new Vector3Int(-12, 10), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section3a, ZoneKeyList.manseSecondFloor + LocationNameList.section3c, new Vector3Int(-8, -5), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3a, list);

        #endregion
        #region Manse-2F-3b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, ZoneKeyList.manseSecondFloor + LocationNameList.section3a, new Vector3Int(1, 4), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, new Vector3Int(-3, -7), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, list);

        #endregion
        #region Manse-2F-3c

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section3c, ZoneKeyList.manseSecondFloor + LocationNameList.section3a, new Vector3Int(-4, 1), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section3c, ZoneKeyList.manseSecondFloor + LocationNameList.section1a, new Vector3Int(10, -1), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.section3c, ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, new Vector3Int(-12, -1), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3c, list);

        #endregion
        #region Manse-2F-Stockroom

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, ZoneKeyList.manseSecondFloor + LocationNameList.section3b, new Vector3Int(-2, 8), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, ZoneKeyList.manseSecondFloor + LocationNameList.section3c, new Vector3Int(2, 4), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        // list.Add(new TransitionSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, ZoneKeyList.manseFirstFloor + LocationNameList.kitchens, new Vector3Int(0, -2), Facing.SouthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, list);

        #endregion
        #endregion

        #region Pit

        #region Pit-1a Old

        // list = new List<TransitionSpawnInfo>();

        // list.Add(new TransitionSpawnInfo(ZoneKeyList.pit + LocationNameList.section1a, ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, new Vector3Int(0, 4), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        // list.Add(new TransitionSpawnInfoMatrix(ZoneKeyList.pit + LocationNameList.section1a, ZoneKeyList.manseFirstFloor + LocationNameList.section1a, receivingMatrix, m1F2CToP1aCoords));

        // List<Vector3Int> p1aToP1bCoords = new List<Vector3Int>() { new Vector3Int(3,3), new Vector3Int(2,3), new Vector3Int(-4,3), new Vector3Int(0,2), new Vector3Int(-2,2),
        //                                                            new Vector3Int(-3,2), new Vector3Int(1,1), new Vector3Int(-1,1), new Vector3Int(-3,1), new Vector3Int(3,0),
        //                                                            new Vector3Int(1,0), new Vector3Int(-4,0), new Vector3Int(3,-1), new Vector3Int(2,-1), new Vector3Int(0,-1),
        //                                                            new Vector3Int(-4,-1), new Vector3Int(1,-2), new Vector3Int(-1,-2), new Vector3Int(-2,-2), new Vector3Int(-3,-2),
        //                                                            new Vector3Int(0,-4), new Vector3Int(-2,-4), new Vector3Int(-3,-4)};

        // list.Add(new TransitionSpawnInfoMatrix(ZoneKeyList.pit + LocationNameList.section1a, ZoneKeyList.pit + LocationNameList.section1b, sendingMatrix, p1aToP1bCoords));

        // transitionSpawnInfoDict.Add(ZoneKeyList.pit + LocationNameList.section1a, list);

        #endregion
        #region Pit-1a

        list = new List<TransitionSpawnInfo>();

        // list.Add(new TransitionSpawnInfo(ZoneKeyList.pit + LocationNameList.section1a, ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, new Vector3Int(0, 4), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.pit + LocationNameList.section1a, ZoneKeyList.pit + LocationNameList.section2a, new Vector3Int(4, -3), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.pit + LocationNameList.section1a, ZoneKeyList.manseFirstFloor + LocationNameList.section2b, new Vector3Int(-9, -2), Facing.SouthEast));

        list.Add(new TransitionSpawnInfoMatrix(ZoneKeyList.pit + LocationNameList.section1a, ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, receivingMatrix, s2PToP1aCoords));

        list.Add(new TransitionSpawnInfoWithCorner(23, ZoneKeyList.pit + LocationNameList.section1a, ZoneKeyList.manseFirstFloor + LocationNameList.stairsToPit, new Vector3Int(-10, 17), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.pit + LocationNameList.section1a, list);

        #endregion
        #region Pit-2a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.pit + LocationNameList.section2a, ZoneKeyList.pit + LocationNameList.section1a, new Vector3Int(-6, -1), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.pit + LocationNameList.section2a, ZoneKeyList.pit + LocationNameList.section2b, new Vector3Int(9, -5), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.pit + LocationNameList.section2a, ZoneKeyList.pit + LocationNameList.section2c, new Vector3Int(6, 6), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.pit + LocationNameList.section2a, list);

        #endregion
        #region Pit-2b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.pit + LocationNameList.section2b, ZoneKeyList.pit + LocationNameList.section2a, new Vector3Int(-8, -3), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.pit + LocationNameList.section2b, ZoneKeyList.pit + LocationNameList.section2d, new Vector3Int(-3, -1), Facing.SouthEast));

        transitionSpawnInfoDict.Add(ZoneKeyList.pit + LocationNameList.section2b, list);

        #endregion
        #region Pit-2c

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.pit + LocationNameList.section2c, ZoneKeyList.pit + LocationNameList.section2a, new Vector3Int(4, -25), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.pit + LocationNameList.section2c, list);

        #endregion
        #region Pit-2d

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.pit + LocationNameList.section2d, ZoneKeyList.pit + LocationNameList.section2b, new Vector3Int(-6, -6), Facing.NorthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.pit + LocationNameList.section2d, list);

        #endregion
        #endregion

        #endregion

        #region Mine Levels 1-3

        #region MineLvl_1-1a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl1 + LocationNameList.section1a, LocationNameList.campMineEntrance, new Vector3Int(6, 0), Facing.NorthWest, Constants.sizeFive, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl1 + LocationNameList.section1a, ZoneKeyList.mineLvl1 + LocationNameList.section1b, new Vector3Int(5, 14), Facing.SouthEast, Constants.sizeThree, Axis.DescendingX));

        list.Add(new FastTravelTransitionSpawnInfo(new Vector3Int(4, 5), Facing.NorthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1a, list);

        #endregion
        #region MineLvl_1-1b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl1 + LocationNameList.section1b, ZoneKeyList.mineLvl1 + LocationNameList.section1a, new Vector3Int(9, -11), Facing.NorthWest, Constants.sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl1 + LocationNameList.section1b, ZoneKeyList.mineLvl1 + LocationNameList.section1c, new Vector3Int(17, 6), Facing.SouthWest, Constants.sizeThree, Axis.DescendingY, outputMultiplier: twoSpaceMultiplier));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl1 + LocationNameList.section1b, ZoneKeyList.mineLvl2 + LocationNameList.section1a, new Vector3Int(9, 11), Facing.SouthEast, Constants.sizeThree, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1b, list);

        #endregion
        #region MineLvl_1-1c

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl1 + LocationNameList.section1c, ZoneKeyList.mineLvl1 + LocationNameList.section1b, new Vector3Int(-10, 2), Facing.NorthEast, Constants.sizeThree, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1c, list);

        #endregion

        #region MineLvl_2-1a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1a, ZoneKeyList.mineLvl1 + LocationNameList.section1b, new Vector3Int(3, -7), Facing.NorthWest, Constants.sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1a, ZoneKeyList.mineLvl2 + LocationNameList.section6, new Vector3Int(19, 1), Facing.SouthEast));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1a, ZoneKeyList.mineLvl2 + LocationNameList.section1b, new Vector3Int(3, 8), Facing.SouthEast, Constants.sizeThree, Axis.DescendingX));

        list.Add(new FastTravelTransitionSpawnInfo(new Vector3Int(2, -5), Facing.NorthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1a, list);

        #endregion
        #region MineLvl_2-1b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1b, ZoneKeyList.mineLvl2 + LocationNameList.section1a, new Vector3Int(0, -11), Facing.NorthWest, Constants.sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1b, ZoneKeyList.mineLvl2 + LocationNameList.section1c, new Vector3Int(-5, -2), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, ZoneKeyList.mineLvl2 + LocationNameList.section1b, ZoneKeyList.mineLvl2 + LocationNameList.section1c, new Vector3Int(-5, 17), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1b, ZoneKeyList.mineLvl2 + LocationNameList.section2a, new Vector3Int(0, 21), Facing.SouthEast, Constants.sizeThree, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1b, list);

        #endregion
        #region MineLvl_2-1c

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1c, ZoneKeyList.mineLvl2 + LocationNameList.section1b, new Vector3Int(10, -8), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));
        list.Add(new TransitionSpawnInfoWithCorner(startingIndexTwo, ZoneKeyList.mineLvl2 + LocationNameList.section1c, ZoneKeyList.mineLvl2 + LocationNameList.section1b, new Vector3Int(10, 11), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1c, ZoneKeyList.mineLvl2 + LocationNameList.section2a, new Vector3Int(4, 18), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1c, list);

        #endregion
        #region MineLvl_2-2a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section2a, ZoneKeyList.mineLvl2 + LocationNameList.section1b, new Vector3Int(12, -5), Facing.NorthWest, Constants.sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section2a, ZoneKeyList.mineLvl2 + LocationNameList.section1c, new Vector3Int(-2, -5), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section2a, ZoneKeyList.mineLvl2 + LocationNameList.section2b, new Vector3Int(12, 5), Facing.SouthEast, Constants.sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section2a, ZoneKeyList.mineLvl3 + LocationNameList.section1a, new Vector3Int(-5, 8), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2a, list);

        #endregion
        #region MineLvl_2-2b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section2b, ZoneKeyList.mineLvl2 + LocationNameList.section2a, new Vector3Int(-9, 5), Facing.NorthWest, Constants.sizeThree, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section2b, ZoneKeyList.mineLvl2 + LocationNameList.section3a, new Vector3Int(13, 9), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section2b, ZoneKeyList.mineLvl2 + LocationNameList.section7b, new Vector3Int(5, -9), Facing.NorthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2b, list);

        #endregion
        #region MineLvl_2-3a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section3a, ZoneKeyList.mineLvl2 + LocationNameList.section2b, new Vector3Int(-4, 7), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section3a, ZoneKeyList.mineLvl2 + LocationNameList.section3b, new Vector3Int(14, 3), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section3a, ZoneKeyList.mineLvl2 + LocationNameList.section5, new Vector3Int(0, -11), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new FastTravelTransitionSpawnInfo(new Vector3Int(-1, 7), Facing.NorthEast));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section3a, list);

        #endregion
        #region MineLvl_2-3b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section3b, ZoneKeyList.mineLvl2 + LocationNameList.section3a, new Vector3Int(-11, 5), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section3b, ZoneKeyList.mineLvl2 + LocationNameList.section4, new Vector3Int(-11, -11), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section3b, ZoneKeyList.mineLvl2 + LocationNameList.section7a, new Vector3Int(8, -5), Facing.NorthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section3b, list);

        #endregion
        #region MineLvl_2-4

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section4, ZoneKeyList.mineLvl2 + LocationNameList.section3b, new Vector3Int(3, 13), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section4, ZoneKeyList.mineLvl2 + LocationNameList.section7a, new Vector3Int(-1, -7), Facing.SouthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section4, list);

        #endregion

        #region MineLvl_2-5

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section5, ZoneKeyList.mineLvl2 + LocationNameList.section3a, new Vector3Int(-18, 13), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section5, ZoneKeyList.mineLvl2 + LocationNameList.section7a, new Vector3Int(40, 11), Facing.SouthWest));


        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_2-6

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section6, ZoneKeyList.mineLvl2 + LocationNameList.section1a, new Vector3Int(-5, 5), Facing.NorthEast));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section6, ZoneKeyList.mineLvl2 + LocationNameList.section7a, new Vector3Int(17, 5), Facing.SouthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section6, list);

        #endregion
        #region MineLvl_2-7a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section7a, ZoneKeyList.mineLvl2 + LocationNameList.section3b, new Vector3Int(6, 1), Facing.SouthWest));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section7a, ZoneKeyList.mineLvl2 + LocationNameList.section4, new Vector3Int(6, -4), Facing.SouthWest));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section7a, ZoneKeyList.mineLvl2 + LocationNameList.section5, new Vector3Int(0, 8), Facing.SouthWest));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section7a, ZoneKeyList.mineLvl2 + LocationNameList.section6, new Vector3Int(0, -10), Facing.SouthWest));


        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section7a, ZoneKeyList.mineLvl2 + LocationNameList.section7b, new Vector3Int(-6, 0), Facing.NorthEast, Constants.sizeThree, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section7a, list);

        #endregion
        #region MineLvl_2-7b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section7b, ZoneKeyList.mineLvl2 + LocationNameList.section7a, new Vector3Int(6, 3), Facing.SouthWest, Constants.sizeThree, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section7b, ZoneKeyList.mineLvl2 + LocationNameList.section2b, new Vector3Int(-2, 11), Facing.SouthEast));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section7b, list);

        #endregion

        #region MineLvl_3-1a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section1a, ZoneKeyList.mineLvl2 + LocationNameList.section2a, new Vector3Int(8, 13), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section1a, ZoneKeyList.mineLvl3 + LocationNameList.section1b, new Vector3Int(-4, 5), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section1a, ZoneKeyList.mineLvl3 + LocationNameList.section2a, new Vector3Int(-17, 13), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section1a, ZoneKeyList.mineLvl3 + LocationNameList.section4a, new Vector3Int(-8, 21), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new FastTravelTransitionSpawnInfo(new Vector3Int(5, 12), Facing.SouthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1a, list);

        #endregion
        #region MineLvl_3-1b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section1b, ZoneKeyList.mineLvl3 + LocationNameList.section1a, new Vector3Int(-4, 3), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1b, list);

        #endregion
        #region MineLvl_3-2a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section2a, ZoneKeyList.mineLvl3 + LocationNameList.section1a, new Vector3Int(11, 3), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section2a, ZoneKeyList.mineLvl3 + LocationNameList.section2b, new Vector3Int(4, 10), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section2a, list);

        #endregion
        #region MineLvl_3-2b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section2b, ZoneKeyList.mineLvl3 + LocationNameList.section2a, new Vector3Int(5, -8), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section2b, ZoneKeyList.mineLvl3 + LocationNameList.section3a, new Vector3Int(3, 14), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section2b, ZoneKeyList.mineLvl3 + LocationNameList.section3b, new Vector3Int(-8, 2), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section2b, list);

        #endregion
        #region MineLvl_3-3a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section3a, ZoneKeyList.mineLvl3 + LocationNameList.section6a, new Vector3Int(19, 14), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section3a, ZoneKeyList.mineLvl3 + LocationNameList.section2b, new Vector3Int(19, -2), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section3a, ZoneKeyList.mineLvl3 + LocationNameList.section3b, new Vector3Int(3, -3), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section3a, ZoneKeyList.mineLvl3 + LocationNameList.section7, new Vector3Int(-20, 7), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new FastTravelTransitionSpawnInfo(new Vector3Int(3, 6), Facing.SouthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section3a, list);

        #endregion
        #region MineLvl_3-3b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section3b, ZoneKeyList.mineLvl3 + LocationNameList.section2b, new Vector3Int(9, 0), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section3b, ZoneKeyList.mineLvl3 + LocationNameList.section3a, new Vector3Int(5, 13), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section3b, list);

        #endregion
        #region MineLvl_3-4a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section4a, ZoneKeyList.mineLvl3 + LocationNameList.section1a, new Vector3Int(2, -7), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section4a, ZoneKeyList.mineLvl3 + LocationNameList.section4b, new Vector3Int(2, 14), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section4a, list);

        #endregion
        #region MineLvl_3-4b

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section4b, ZoneKeyList.mineLvl3 + LocationNameList.section4a, new Vector3Int(0, -12), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section4b, ZoneKeyList.mineLvl3 + LocationNameList.section5, new Vector3Int(0, 18), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section4b, list);

        #endregion
        #region MineLvl_3-5

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section5, ZoneKeyList.mineLvl3 + LocationNameList.section4b, new Vector3Int(1, -6), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section5, ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, new Vector3Int(-7, -3), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section5, ZoneKeyList.mineLvl3 + LocationNameList.section6a, new Vector3Int(-13, 2), Facing.NorthEast, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new FastTravelTransitionSpawnInfo(new Vector3Int(-7, 2), Facing.SouthWest));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section5, list);

        #endregion
        #region MineLvl_3-Miner Camp

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, ZoneKeyList.mineLvl3 + LocationNameList.section5, new Vector3Int(3, 5), Facing.SouthEast, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.minerCamp, list);

        #endregion
        #region MineLvl_3-6a

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section6a, ZoneKeyList.mineLvl3 + LocationNameList.section5, new Vector3Int(12, 15), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section6a, ZoneKeyList.mineLvl3 + LocationNameList.section3a, new Vector3Int(9, -9), Facing.NorthWest, Constants.sizeTwo, Axis.DescendingX));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section6a, list);

        #endregion
        #region MineLvl_3-7

        list = new List<TransitionSpawnInfo>();

        list.Add(new TransitionSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section7, ZoneKeyList.mineLvl3 + LocationNameList.section3a, new Vector3Int(16, -3), Facing.SouthWest, Constants.sizeTwo, Axis.DescendingY));

        transitionSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section7, list);

        #endregion

        #endregion


    }



}

public class TransitionSpawnInfo
{
    protected string currentArea;
    protected string destinationLocation;

    private Vector3Int startCell;

    protected Facing playerSpawnDirection;
    protected int outputMultiplier;

    private int size;
    private Axis axis;
    protected bool destinationOnly;

    protected PlayerInteractionScript scriptOnTransition;
    

    public TransitionSpawnInfo(string currentArea, string destinationLocation, Vector3Int startCell, Facing playerSpawnDirection, int size = 1, Axis axis = Axis.DescendingX, int outputMultiplier = 1, bool destinationOnly = false, PlayerInteractionScript scriptOnTransition = null)
    {
        this.currentArea = currentArea;
        this.destinationLocation = destinationLocation;

        this.startCell = startCell;
        this.playerSpawnDirection = playerSpawnDirection;

        this.outputMultiplier = outputMultiplier;
        this.size = size;
        this.axis = axis;
        this.destinationOnly = destinationOnly;

        if(destinationOnly)
        {
            this.outputMultiplier = 0;
        }

        this.scriptOnTransition = scriptOnTransition;
    }

    public virtual int getStartIndex()
    {
        return 0;
    }

    public virtual bool fastTravelCapable()
    {
        return false;
    }

    public virtual int getOutputMultiplier()
    {
        return outputMultiplier;
    }

    public virtual List<Transition> getTransitions()
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

            list.Add(new Transition(currentArea, destinationLocation, currentCell, index + getStartIndex(), playerSpawnDirection, fastTravelCapable(), getOutputMultiplier(), scriptOnTransition, destinationOnly));
        }

        return list;
    }

}

public class TransitionSpawnInfoWithCorner : TransitionSpawnInfo
{

    private int startIndex;

    public TransitionSpawnInfoWithCorner(int startIndex, string currentArea, string destinationLocation, Vector3Int startCell, Facing playerSpawnDirection, int size = 1, Axis axis = Axis.DescendingX, int outputMultiplier = 1, bool destinationOnly = false, PlayerInteractionScript scriptOnTransition = null):
    base(currentArea, destinationLocation, startCell, playerSpawnDirection, size, axis, outputMultiplier, destinationOnly, scriptOnTransition)
    {
        this.startIndex = startIndex;
    }

    public override int getStartIndex()
    {
        return startIndex;
    }

}

public class FastTravelTransitionSpawnInfo : TransitionSpawnInfo
{

    public FastTravelTransitionSpawnInfo(Vector3Int startCell, Facing playerSpawnDirection) :
    base(null, null, startCell, playerSpawnDirection)
    {

    }

    // public FastTravelTransitionSpawnInfo(string destinationLocation) :
    // base(AreaManager.locationName, destinationLocation, MovementManager.getPlayerGridCell(), State.playerFacing.getFacing())
    // {

    // }

    public override bool fastTravelCapable()
    {
        return true;
    }

    public override int getOutputMultiplier()
    {
        return 0;
    }

}

public class TransitionSpawnInfoMatrix : TransitionSpawnInfo
{

    private List<Vector3Int> listOfCoords;

    public TransitionSpawnInfoMatrix(string currentArea,
                               string destinationLocation,
                               bool destinationOnly, 
                               List<Vector3Int> listOfCoords,
                               PlayerInteractionScript scriptOnTransition = null):
    base(currentArea,
         destinationLocation,
         Vector3Int.zero, 
         Facing.SouthWest,
         outputMultiplier: Constants.sizeZero,
         destinationOnly: destinationOnly)
    {
        this.listOfCoords = listOfCoords;
    }


    public override List<Transition> getTransitions()
    {
        List<Transition> list = new List<Transition>();

        int index = 0;
        foreach(Vector3Int coords in listOfCoords)
        {
            list.Add(new Transition(currentArea, destinationLocation, coords, index + getStartIndex(), playerSpawnDirection, fastTravelCapable(), getOutputMultiplier(), scriptOnTransition, destinationOnly, allowAutosave: false));
            index++;
        }

        return list;
    }
}