using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public static class GateSpawnInfoList
{
    private const bool useRubbleColor = true;

    private static Dictionary<string, List<GateSpawnInfo>> gateSpawnInfoDict;

    public static List<GateSpawnInfo> getGateSpawnInfo(string areaName)
    {
        if (!gateSpawnInfoDict.ContainsKey(areaName))
        {
            return new List<GateSpawnInfo>();
        }

        return gateSpawnInfoDict[areaName];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateGateSpawnInfoList()
    {
        gateSpawnInfoDict = new Dictionary<string, List<GateSpawnInfo>>();
        List<GateSpawnInfo> list;

        #region 6SlaveShack

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.liftableRubble,
                                    LocationNameList.slaveShackSix,
                                    new Vector3Int(6, -1),
                                    PrefabNames.blockRubble,
                                    Constants.sizeTwo,
                                    Axis.DescendingY,
                                    TutorialSequenceList.interactableRubbleTargetHash,
                                    useRubbleColor));

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.awkwardRubble,
                                    LocationNameList.slaveShackSix,
                                    new Vector3Int(6, -1),
                                    PrefabNames.blockRubble,
                                    Constants.sizeTwo,
                                    Axis.DescendingY,
                                    TutorialSequenceList.fallenBeamTargetHash,
                                    useRubbleColor));

        gateSpawnInfoDict.Add(LocationNameList.slaveShackSix, list);

        #endregion

        #region Guard House NE

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.barracksGate,
                                    LocationNameList.guardHouseNorthEast,
                                    new Vector3Int(6, -1),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));

        gateSpawnInfoDict.Add(LocationNameList.guardHouseNorthEast, list);

        #endregion

        #region Guard House SW

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.barracksGate,
                                    LocationNameList.guardHouseSouthWest,
                                    new Vector3Int(-16, -1),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));

        list.Add(new GateWithKeySpawnInfo(Constants.indexOne,
                                    NPCNameList.barracksArmoryGate,
                                    LocationNameList.guardHouseSouthWest,
                                    PrefabNames.portcullis2x1Path,
                                    new Vector3Int(-6, 0),
                                    Constants.sizeTwo,
                                    Axis.DescendingX,
                                    new GateKeyDetails("*This is the gate to the Barracks' Armory. It is currently locked, and watched closely by the guards.*",
                                                       ItemList.barracksArmoryKeyName,
                                                       HostilityScriptList.openBarracksGateScriptKey,
                                                       "the " + MapDisplayNameList.lovashiCamp)));

        gateSpawnInfoDict.Add(LocationNameList.guardHouseSouthWest, list);

        #endregion

        #region CenterCamp

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.campGate,
                                    LocationNameList.campCenter,
                                    new Vector3Int(2, -17),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));

        gateSpawnInfoDict.Add(LocationNameList.campCenter, list);

        #endregion

        #region Camp Mine Entrance

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.barracksGate,
                                    LocationNameList.campMineEntrance,
                                    new Vector3Int(11, -14),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));

        gateSpawnInfoDict.Add(LocationNameList.campMineEntrance, list);

        #endregion

        #region ManseCamp

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.manseFrontDoor,
                                    LocationNameList.campManse,
                                    new Vector3Int(3, 15),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));


        list.Add(new GateSpawnInfo(Constants.indexOne,
                                    NPCNameList.manseServiceEntrance,
                                    LocationNameList.campManse,
                                    new Vector3Int(-6, 6),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));


        list.Add(new GateSpawnInfo(Constants.indexTwo,
                                    NPCNameList.barracksGate,
                                    LocationNameList.campManse,
                                    new Vector3Int(-1, -16),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));

        gateSpawnInfoDict.Add(LocationNameList.campManse, list);

        #endregion

        #region MineLvl_1-1b

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.awkwardRubble,
                                    ZoneKeyList.mineLvl1 + LocationNameList.section1b,
                                    new Vector3Int(5, 1),
                                    PrefabNames.lowRubble,
                                    Constants.sizeThree,
                                    Axis.DescendingY,
                                    useRubbleColor: useRubbleColor));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_1-1c

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.liftableGate,
                                    ZoneKeyList.mineLvl1 + LocationNameList.section1c,
                                    new Vector3Int(2, 1),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1c, list);

        #endregion

        #region MineLvl_2-2a

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section2a,
                                    new Vector3Int(3, 3),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2a, list);

        #endregion

        #region MineLvl_2-2b

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section2b,
                                    new Vector3Int(6, -4),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));

        list.Add(new GateSpawnInfo(Constants.indexOne,
                                    NPCNameList.mineArmoryGate,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section2b,
                                    new Vector3Int(6, 6),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_2-3a

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.awkwardRubble,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section3a,
                                    new Vector3Int(3, 7),
                                    PrefabNames.blockRubble,
                                    Constants.sizeTwo,
                                    Axis.DescendingY,
                                    useRubbleColor: useRubbleColor));

        list.Add(new GateSpawnInfo(Constants.indexOne,
                                    NPCNameList.awkwardRubble,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section3a,
                                    new Vector3Int(10, 7),
                                    PrefabNames.blockRubble,
                                    Constants.sizeTwo,
                                    Axis.DescendingY,
                                    useRubbleColor: useRubbleColor));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section3a, list);

        #endregion

        #region MineLvl_2-3b

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section3b,
                                    new Vector3Int(5, -1),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));

        list.Add(new GateSpawnInfo(Constants.indexOne,
                                    NPCNameList.liftableGate,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section3b,
                                    new Vector3Int(-11, -10),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section3b, list);

        #endregion

        #region MineLvl_2-6

        list = new List<GateSpawnInfo>();

        list.Add(new TemporaryGateSpawnInfo(Constants.indexZero, //A1 - S1
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(2, 9),
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        list.Add(new TemporaryGateSpawnInfo(Constants.indexOne, //B1 - S2
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(2, 5),
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        list.Add(new TemporaryGateSpawnInfo(Constants.indexTwo, //C1 - S3
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(2, 1),
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        list.Add(new TemporaryGateSpawnInfo(Constants.indexThree, //A1 - B1
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(4, 7),
                                    Constants.sizeOne,
                                    Axis.DescendingX));

        list.Add(new TemporaryGateSpawnInfo(Constants.indexFour, //B1 - C1
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(4, 3),
                                    Constants.sizeOne,
                                    Axis.DescendingX));
        
        list.Add(new TemporaryGateSpawnInfo(Constants.indexFive, //A1 - A2
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(6, 9),
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        list.Add(new TemporaryGateSpawnInfo(Constants.indexSix, //B1 - B2
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(6, 5),
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        list.Add(new TemporaryGateSpawnInfo(Constants.indexSeven, //C1 - C2
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(6, 1),
                                    Constants.sizeOne,
                                    Axis.DescendingY));
        
        list.Add(new TemporaryGateSpawnInfo(Constants.indexEight, //A2 - B2
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(8, 7),
                                    Constants.sizeOne,
                                    Axis.DescendingX));

        list.Add(new TemporaryGateSpawnInfo(Constants.indexNine, //B2 - C2
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(8, 3),
                                    Constants.sizeOne,
                                    Axis.DescendingX));

        list.Add(new TemporaryGateSpawnInfo(Constants.indexTen, //B2 - 7A
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(10, 5),
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        list.Add(new TemporaryGateSpawnInfo(Constants.indexEleven, //C2 - C3
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(10, 1),
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        list.Add(new TemporaryGateSpawnInfo(Constants.indexTwelve, //C3 - 7a
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section6,
                                    new Vector3Int(12, 3),
                                    Constants.sizeOne,
                                    Axis.DescendingX));



        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section6, list);

        #endregion

        #region MineLvl_2-7a

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section7a,
                                    new Vector3Int(-1, 5),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));
        
        list.Add(new GateSpawnInfo(Constants.indexOne,
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section7a,
                                    new Vector3Int(1, 2),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));

        list.Add(new GateSpawnInfo(Constants.indexTwo,
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section7a,
                                    new Vector3Int(1, -4),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));

        list.Add(new GateSpawnInfo(Constants.indexThree,
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section7a,
                                    new Vector3Int(-1, -7),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section7a, list);

        #endregion

        #region MineLvl_2-7b

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.awkwardRubble,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section7b,
                                    new Vector3Int(-9, -7),
                                    PrefabNames.blockRubble,
                                    Constants.sizeThree,
                                    Axis.DescendingX,
                                    useRubbleColor: useRubbleColor));

        list.Add(new GateSpawnInfo(Constants.indexOne,
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section7b,
                                    new Vector3Int(-10, -1),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));

        list.Add(new GateSpawnInfo(Constants.indexTwo,
                                    NPCNameList.liftableGate,
                                    ZoneKeyList.mineLvl2 + LocationNameList.section7b,
                                    new Vector3Int(-6, -4),
                                    PrefabNames.portcullis1x1Path,
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section7b, list);

        #endregion

        #region MineLvl_3

        #region MineLvl_3-2b

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.liftableGate,
                                    ZoneKeyList.mineLvl3 + LocationNameList.section2b,
                                    new Vector3Int(-6, 2),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));

        list.Add(new GateSpawnInfo(Constants.indexOne,
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl3 + LocationNameList.section2b,
                                    new Vector3Int(3, 10),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_3-3b

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.liftableGate,
                                    ZoneKeyList.mineLvl3 + LocationNameList.section3b,
                                    new Vector3Int(5, 2),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section3b, list);

        #endregion

        #region MineLvl_3-4b

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl3 + LocationNameList.section4b,
                                    new Vector3Int(0, 15),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));

        list.Add(new GateSpawnInfo(Constants.indexOne, 
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl3 + LocationNameList.section4b,
                                    new Vector3Int(-6, 2),
                                    PrefabNames.portcullis1x1Path,
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section4b, list);

        #endregion

        #region MineLvl_3-5

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.liftableRubble,
                                    ZoneKeyList.mineLvl3 + LocationNameList.section5,
                                    new Vector3Int(7, 2),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_3-6a

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl3 + LocationNameList.section6a,
                                    new Vector3Int(-4, 7),
                                    PrefabNames.portcullis3x1Path,
                                    Constants.sizeThree,
                                    Axis.DescendingX));

        list.Add(new GateWithHiddenTerrainSpawnInfo(Constants.indexOne, 
                                    NPCNameList.unstablePillar,
                                    ZoneKeyList.mineLvl3 + LocationNameList.section6a,
                                    PrefabNames.unstablePillar,
                                    ColorList.mineLvl3RubbleColor,
                                    new Vector3Int(-6, -1),
                                    SecretDoorKeyList.mineLvl3_6aUnstablePillarHiddenTerrain,
                                    StatDifficultyList.strengthDifficultyThree));

        list.Add(new GateWithHiddenTerrainSpawnInfo(Constants.indexOne, 
                                    NPCNameList.unstablePillar,
                                    ZoneKeyList.mineLvl3 + LocationNameList.section6a,
                                    PrefabNames.unstablePillar,
                                    ColorList.mineLvl3RubbleColor,
                                    new Vector3Int(-6, -5),
                                    SecretDoorKeyList.mineLvl3_6aUnstablePillarHiddenTerrain,
                                    StatDifficultyList.strengthDifficultyThree));

        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section6a, list);

        #endregion

        #region MineLvl_3-7

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.mineLvl3 + LocationNameList.section7,
                                    new Vector3Int(0, 2),
                                    PrefabNames.portcullis3x1Path,
                                    Constants.sizeThree,
                                    Axis.DescendingY));

        list.Add(new GateWithHiddenTerrainSpawnInfo(Constants.indexOne, 
                                    NPCNameList.unstablePillar,
                                    ZoneKeyList.mineLvl3 + LocationNameList.section7,
                                    PrefabNames.unstablePillar,
                                    ColorList.mineLvl3RubbleColor,
                                    new Vector3Int(-9, -2),
                                    SecretDoorKeyList.mineLvl3_7UnstablePillarHiddenTerrain,
                                    StatDifficultyList.strengthDifficultyThree));


        gateSpawnInfoDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section7, list);

        #endregion

        #endregion

        #region Manse

        #region Manse-1f

        #region Manse-1f-Dining Room

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom,
                                    new Vector3Int(-1, 6),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));
                                    
        gateSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.diningRoom, list);

        #endregion

        #region Manse-1f-2a

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.manseFirstFloor + LocationNameList.section2a,
                                    new Vector3Int(2, -4),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));
                                    
        gateSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, list);

        #endregion

        #region Manse-1f-2b

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.manseFirstFloor + LocationNameList.section2b,
                                    new Vector3Int(-5, -2),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));
                                    
        gateSpawnInfoDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2b, list);

        #endregion

        #endregion

        #region Manse-2F

        #region Manse-2F-3a

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.manseSecondFloor + LocationNameList.section3a,
                                     new Vector3Int(-3, -1),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));
                                    
        gateSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3a, list);

        #endregion

        #region Manse-2F-3b

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.liftableGate,
                                    ZoneKeyList.manseSecondFloor + LocationNameList.section3b,
                                     new Vector3Int(0, -3),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY,
                                    statDifficulty: new KeyValuePair<string, int>(InkVariableNameList.strDiffVarName, Constants.difficultyThree)));
                                    
        gateSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.section3b, list);

        #endregion

        #region Manse-2F-Stockroom

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.heavyBarrels,
                                    ZoneKeyList.manseSecondFloor + LocationNameList.stockroom,
                                     new Vector3Int(-3, -3),
                                    PrefabNames.tripleBarrel,
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.heavyBarrels,
                                    ZoneKeyList.manseSecondFloor + LocationNameList.stockroom,
                                     new Vector3Int(-3, -4),
                                    PrefabNames.tripleBarrel,
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        list.Add(new GateSpawnInfo(Constants.indexOne, 
                                    NPCNameList.heavyBarrels,
                                    ZoneKeyList.manseSecondFloor + LocationNameList.stockroom,
                                     new Vector3Int(-3, 2),
                                    PrefabNames.tripleBarrel,
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        list.Add(new GateSpawnInfo(Constants.indexOne, 
                                    NPCNameList.heavyBarrels,
                                    ZoneKeyList.manseSecondFloor + LocationNameList.stockroom,
                                     new Vector3Int(-3, 1),
                                    PrefabNames.tripleBarrel,
                                    Constants.sizeOne,
                                    Axis.DescendingY));

        gateSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.stockroom, list);

        #endregion

        #region Manse-2F-Office

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.officeDoor,
                                    ZoneKeyList.manseSecondFloor + LocationNameList.office,
                                    new Vector3Int(-4, -1),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));

        gateSpawnInfoDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.office, list);

        #endregion

        #endregion

        #region Pit

        #region Pit-2b

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.cellDoor,
                                    ZoneKeyList.pit + LocationNameList.section2b,
                                    new Vector3Int(0, -3),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingY));
                                    
        gateSpawnInfoDict.Add(ZoneKeyList.pit + LocationNameList.section2b, list);

        #endregion

        #region Pit-2c

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero, 
                                    NPCNameList.ancientPortcullis,
                                    ZoneKeyList.pit + LocationNameList.section2c,
                                    new Vector3Int(4, 12),
                                    PrefabNames.portcullis2x1Path,
                                    Constants.sizeTwo,
                                    Axis.DescendingX));
                                    
        gateSpawnInfoDict.Add(ZoneKeyList.pit + LocationNameList.section2c, list);

        #endregion

        #endregion

        #endregion

    }
}

public class GateSpawnInfo : AxisSpawnInfo
{
    protected int gateIndex;
    protected string npcName;
    protected string spriteName;
    protected bool useRubbleColor;

    protected Dictionary<string, int> statDifficulties = new Dictionary<string, int>();

    public GateSpawnInfo(int gateIndex, 
                         string npcName, 
                         string currentArea, 
                         Vector3Int startCell, 
                         string spriteName = null, 
                         int size = 1, 
                         Axis axis = Axis.DescendingX, 
                         string tutorialTargetHash = "",
                         bool useRubbleColor = false,
                         KeyValuePair<string, int> statDifficulty = new KeyValuePair<string, int>()) :
    base(currentArea, startCell, size, axis)
    {
        this.gateIndex = gateIndex;
        this.npcName = npcName;
        this.spriteName = spriteName;
        this.tutorialTargetHash = tutorialTargetHash;
        this.useRubbleColor = useRubbleColor;

        if(statDifficulty.Key != null && statDifficulty.Key.Length > 0)
        {
            this.statDifficulties.Add(statDifficulty.Key, statDifficulty.Value);
        }
    }

    protected virtual string getGateName()
    {
        if (gateIndex == 0)
        {
            return npcName;
        }
        else
        {
            return npcName + gateIndex;
        }
    }

    protected string getSpriteName(Axis axis, int index)
    {
        if (spriteName != null && spriteName.Length > 0)
        {
            return spriteName;
        }

        switch (axis)
        {
            case Axis.DescendingX:
                return PrefabNames.XAxisDoor + (index + 1);
            default:
                return PrefabNames.YAxisDoor + (index + 1);
        }
    }

    protected bool skewed()
    {
        switch(spriteName)
        {
            case PrefabNames.portcullis1x1Path:
            case PrefabNames.portcullis2x1Path:
            case PrefabNames.portcullis3x1Path:
                return false;
            default:
                return true;
        }
    }

    public override bool shouldSpawn()
    {
        return SpawnParamsList.getSpawnParams(currentArea, getGateName()).canSpawn(getGateName());
    }

    public virtual GateSpawnDetails createSpawnDetails(Vector3Int currentCell, int index)
    {
        return new GateSpawnDetails(getGateName(), currentCell, currentArea, getSpriteName(axis, index), tutorialTargetHash, skewed(),  indexHasSprite(spriteName, index), axis, statDifficulties, useRubbleColor);
    }

    public static bool indexHasSprite(string spriteName, int index)
    {
        switch(spriteName)
        {
            case PrefabNames.portcullis1x1Path:
            case PrefabNames.portcullis2x1Path:
            case PrefabNames.portcullis3x1Path:
                return index == 0;
            default:
                return true;
        }
    }

    public override List<OOCSpawnDetails> getSpawnDetails()
    {
        List<OOCSpawnDetails> list = new List<OOCSpawnDetails>();

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

            list.Add(createSpawnDetails(currentCell, index));
        }

        return list;
    }

}

public class TemporaryGateSpawnInfo : GateSpawnInfo
{

    public TemporaryGateSpawnInfo(int gateIndex, string npcName, string currentArea, Vector3Int startCell, int size, Axis axis) :
    base(gateIndex, npcName, currentArea, startCell, PrefabNames.portcullis1x1Path, size, axis)
    {
    }

    protected override string getGateName()
    {
        return npcName + gateIndex;
    }

    public override GateSpawnDetails createSpawnDetails(Vector3Int currentCell, int index)
    {
        return new TemporaryGateSpawnDetails(getGateName(), currentCell, currentArea, getSpriteName(axis, index), tutorialTargetHash, skewed(), axis, statDifficulties);
    }

}

public class GateWithHiddenTerrainSpawnInfo : GateSpawnInfo
{
    private string hiddenTerrainFlag;
    private Color tint = Color.white;

    public GateWithHiddenTerrainSpawnInfo(int gateIndex, string npcName, string currentArea, Vector3Int startCell, string hiddenTerrainFlag) :
    base(gateIndex, npcName, currentArea, startCell)
    {
        this.hiddenTerrainFlag = hiddenTerrainFlag;        
    }

    public GateWithHiddenTerrainSpawnInfo(int gateIndex, string npcName, string currentArea, string spriteName, Color tint, Vector3Int startCell, string hiddenTerrainFlag) :
    base(gateIndex, npcName, currentArea, startCell, spriteName)
    {
        this.hiddenTerrainFlag = hiddenTerrainFlag;        
        this.tint = tint;        
    }

    public GateWithHiddenTerrainSpawnInfo(int gateIndex, string npcName, string currentArea, string spriteName, Color tint, Vector3Int startCell, string hiddenTerrainFlag, KeyValuePair<string, int> statDifficulty) :
    base(gateIndex, npcName, currentArea, startCell, spriteName, statDifficulty: statDifficulty)
    {
        this.hiddenTerrainFlag = hiddenTerrainFlag;        
        this.tint = tint;        
    }

    public GateWithHiddenTerrainSpawnInfo(int gateIndex, string npcName, string currentArea, Vector3Int startCell, int size, Axis axis, string hiddenTerrainFlag) :
    base(gateIndex, npcName, currentArea, startCell, size: size, axis: axis)
    {
        this.hiddenTerrainFlag = hiddenTerrainFlag;        
    }

    public override GateSpawnDetails createSpawnDetails(Vector3Int currentCell, int index)
    {
        return new GateWithHiddenTerrainSpawnDetails(getGateName(), currentCell, currentArea, getSpriteName(axis, index), tutorialTargetHash, skewed(), statDifficulties, hiddenTerrainFlag, tint);
    }

}


public class GateKeyDetails : IStoryVariableSource
{

    public string description = "";
    public string keyName = "";

    public string hostileAreaName = "";
    public string hostilityScriptKey = "";

    public GateKeyDetails(string description, string keyName)
    {
        this.description = description;
        this.keyName = keyName;
    }

    public GateKeyDetails(string description, string keyName, string hostilityScriptKey, string hostileAreaName)
    {
        this.description = description;
        this.keyName = keyName;

        this.hostilityScriptKey = hostilityScriptKey;
        this.hostileAreaName = hostileAreaName;
    }

    public Story addVariables(Story story)
    {
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.description, description);
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.keyName, keyName);

        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.hostileAreaName, hostileAreaName);
        story = InkVariableNameList.setStoryVariable(story, InkVariableNameList.hostilityScriptKey, hostilityScriptKey);
  
        return story;
    }

}

public class GateWithKeySpawnInfo : GateSpawnInfo
{
    private GateKeyDetails gateKeyDetails;

    public GateWithKeySpawnInfo(int gateIndex, string npcName, string currentArea, string spriteName, Vector3Int startCell, int size, Axis axis, GateKeyDetails gateKeyDetails) :
    base(gateIndex, npcName, currentArea, startCell, spriteName, size, axis)
    {
        this.gateKeyDetails = gateKeyDetails;
    }

    public override GateSpawnDetails createSpawnDetails(Vector3Int currentCell, int index)
    {
        return new GateWithKeySpawnDetails(getGateName(), currentCell, currentArea, getSpriteName(axis, index), skewed(),  indexHasSprite(spriteName, index), axis, gateKeyDetails);
    }
}