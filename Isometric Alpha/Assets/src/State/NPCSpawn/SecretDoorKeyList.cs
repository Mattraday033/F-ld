using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SecretDoorKeyList
{
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

    #endregion

    #region Mine

    #region MineLvl_2-1a

    public const string mineLvl2FirstSecretDoor = LocationNameList.mineLvl2 + LocationNameList.section1a + NPCNameList.mineLvl2Wall;
    public const string mineLvl2SecondSecretDoor = LocationNameList.mineLvl2 + LocationNameList.section7b + NPCNameList.mineLvl2Wall;

    #endregion

    #endregion
}

