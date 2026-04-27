using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SecretDoorKeyList
{
    private const string puzzledFinishedSuffix = "PuzzleFinished";

    #region Camp

    #region 6SlaveShack

    public const string wisTutorialSecretDoor = LocationNameList.slaveShackSix + NPCNameList.wallPatch;

    #endregion

    #region CenterCamp

    public const string centerCampWallPatchOne = LocationNameList.campCenter + NPCNameList.wallPatch + "1";
    public const string centerCampWallPatchTwo = LocationNameList.campCenter + NPCNameList.wallPatch + "2";

    #endregion
    #region SECamp

    public const string southEastCampWallPatchOne = LocationNameList.campSouthEast + NPCNameList.wallPatch + "1";
    public const string southEastCampWallPatchTwo = LocationNameList.campSouthEast + NPCNameList.wallPatch + "2";
    public const string southEastCampWallPatchThree = LocationNameList.campSouthEast + NPCNameList.wallPatch + "3";

    #endregion

    #region NWCamp

    public const string northWestCampWallPatchOne = LocationNameList.campNorthWest + NPCNameList.wallPatch + "1";

    #endregion

    #endregion

    #region Mine

    #region MineLvl_2-1a

    public const string mineLvl2FirstSecretDoor = ZoneKeyList.mineLvl2 + LocationNameList.section1a + NPCNameList.mineLvl2Wall;

    #endregion

    #region MineLvl_2-7b

    public const string mineLvl2SecondSecretDoor = ZoneKeyList.mineLvl2 + LocationNameList.section7b + NPCNameList.mineLvl2Wall;

    #endregion

    #region MineLvl_3-1b

    public const string mineLvl3PuzzleDoor = ZoneKeyList.mineLvl3 + LocationNameList.section1b + NPCNameList.mineLvl3Wall;
    public const string mineLvl3PuzzleFinished = ZoneKeyList.mineLvl3 + LocationNameList.section1b + puzzledFinishedSuffix;

    #endregion

    #region MineLvl_3-6a

    public const string mineLvl3_6aUnstablePillarHiddenTerrain = ZoneKeyList.mineLvl3 + LocationNameList.section6a + NPCNameList.unstablePillar;

    #endregion


    #region MineLvl_3-7

    public const string mineLvl3_7UnstablePillarHiddenTerrain = ZoneKeyList.mineLvl3 + LocationNameList.section7 + NPCNameList.unstablePillar;
    public const string mineLvl3_7PocketSealedRubble = ZoneKeyList.mineLvl3 + LocationNameList.section7 + NPCNameList.rubble;

    #endregion

    #endregion

    #region Manse

    #region Manse-1f-2a

    public const string manseHiddenGardenFlag = ZoneKeyList.manseFirstFloor + LocationNameList.section2a + NPCNameList.suspiciousWall;

    #endregion

    #region Manse-1f-3a

    public const string meetingRoomSecretEntrance = ZoneKeyList.manseFirstFloor + LocationNameList.section3a + NPCNameList.suspiciousWall;

    #endregion

    #region Manse-1f-3c

    public const string secretBookShelfFlag = ZoneKeyList.manseFirstFloor + LocationNameList.section3c + NPCNameList.suspiciousShelf;

    #endregion

    #region Manse-2f-Office

    public const string officeSecretEntranceFlag = ZoneKeyList.manseSecondFloor + LocationNameList.office + NPCNameList.suspiciousWall;

    #endregion

    #endregion

}

