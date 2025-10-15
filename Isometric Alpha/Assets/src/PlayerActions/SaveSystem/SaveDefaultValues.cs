using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveDefaultValues
{

    public const string defaultStatRequirementType = "None";

    public const string badSaveName = "Bad Save";
    public const string badMPName = "Bad MonsterPack";
    public const int badSaveNumber = 0;

    public const string missingSpriteName = "MissingSprite";
    //    public const string defaultSceneName = "2SlaveShack";
    public const string defaultSceneName = AreaNameList.slaveShackTwo;
    public const bool defaultBoolFalse = false;
    public const bool defaultBoolTrue = true;

    public const string defaultCombatSpriteName = "PlayerSprite";
    public const string defaultOverworldSpriteSortingLayer = "First";
    public const string defaultPlayerName = "Brandon";

    public const int defaultIndexNegativeOne = -1;
    public const int defaultStatZero = 0;
    public const int defaultStatOne = 1;
    public const int defaultStatTwo = 2;

    public const float defaultMonsterPackStartingPosition = 999999f;

    public const Facing defaultFacing = Facing.SouthWest;

    public const string defaultCurrentFlags = "{\"newGame\": true}";


    public static float[] defaultPlayerPosition = new float[] { 3.46f, -1.48f, 0.5f };
    public static string[] defaultEmptyStringArray = new string[0];
    public static int[] defaultEmptyIntArray = new int[0];
    public static bool[] defaultEmptyBoolArray = new bool[0];
    public static GridCoords[] defaultEmptyGridCoordsArray = new GridCoords[0];
    public static GridCoords defaultPlayerFormationPosition = new GridCoords(0,1);
    public static InventoryWrapper[] defaultEmptyInventoryWrapperArray = new InventoryWrapper[0];
    public static StatsWrapper[] defaultEmptyStatsWrapperArray = new StatsWrapper[0];
    public static string[] defaultEmptyMonsterPackList = new string[0];

    public static string[] defaultEmptyEquimentArray = new string[6] {
                                                                        null,
                                                                        null,
                                                                        "{\"listIndex\":\"2\",\"itemIndex\":\"0\",\"quantity\":\"1\"}",
                                                                        null,
                                                                        null,
                                                                        null
                                                                     };
    public static string[] defaultEmptyCombatActionArray = new string[8] {
                                                                            null,
                                                                            null,
                                                                            null,
                                                                            null,
                                                                            null,
                                                                            null,
                                                                            null,
                                                                            null
                                                                         };

}
