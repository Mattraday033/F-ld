using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SecretDoorSpawnInfoList
{
    private static Dictionary<string, List<SecretDoorSpawnInfo>> secretDoorSpawnDetailsDict;

    public static List<SecretDoorSpawnInfo> getSecretDoorSpawnDetails(string areaName)
    {
        if (!secretDoorSpawnDetailsDict.ContainsKey(areaName))
        {
            return new List<SecretDoorSpawnInfo>();
        }

        return secretDoorSpawnDetailsDict[areaName];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeSpawnDetailsList()
    {
        secretDoorSpawnDetailsDict = new Dictionary<string, List<SecretDoorSpawnInfo>>();
        List<SecretDoorSpawnInfo> list;

        #region 5SlaveShack
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(LocationNameList.slaveShackFive, NPCNameList.wallPatch, PrefabNames.defaultNPCSprite, new Vector3Int(2, -6),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchThree), Constants.sizeTwo, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(LocationNameList.slaveShackFive, list);
        #endregion
        #region 6SlaveShack
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(LocationNameList.slaveShackSix, NPCNameList.wallPatch, PrefabNames.defaultNPCSprite, new Vector3Int(5, 2),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchTwo), Constants.sizeTwo, Axis.DescendingX));

        list.Add(new SecretDoorSpawnInfo(LocationNameList.slaveShackSix, NPCNameList.wallPatch, PrefabNames.defaultNPCSprite, new Vector3Int(8, -6),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchOne), Constants.sizeTwo, Axis.DescendingX));

        list.Add(new SecretDoorSpawnInfo(LocationNameList.slaveShackSix, NPCNameList.wallPatch, PrefabNames.defaultNPCSprite, new Vector3Int(2, -4),
                                            new TutorialSecretDoorInfo(SecretDoorKeyList.wisTutorialSecretDoor,
                                            new StartSpawningAllTrueFlagList(new string[] { FlagNameList.choseWisdomAtStart })),
                                            Constants.sizeTwo, Axis.DescendingY,
                                            TutorialSequenceList.secretDoorTargetHash));

        secretDoorSpawnDetailsDict.Add(LocationNameList.slaveShackSix, list);
        #endregion

        #region Stables
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(LocationNameList.stables, NPCNameList.wallPatch, PrefabNames.defaultNPCSprite, new Vector3Int(9, -4),
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchOne), Constants.sizeTwo, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(LocationNameList.stables, list);
        #endregion
        #region Temple
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(LocationNameList.temple, NPCNameList.wallPatch, PrefabNames.defaultNPCSprite, new Vector3Int(11, 4),
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchTwo), Constants.sizeTwo, Axis.DescendingY));

        secretDoorSpawnDetailsDict.Add(LocationNameList.temple, list);
        #endregion

        #region CenterCamp
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(LocationNameList.campCenter, NPCNameList.wallPatch, PrefabNames.defaultNPCSprite, new Vector3Int(8, 12),
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchOne), Constants.sizeTwo, Axis.DescendingX));

        list.Add(new SecretDoorSpawnInfo(LocationNameList.campCenter, NPCNameList.wallPatch, PrefabNames.defaultNPCSprite, new Vector3Int(-7, 14),
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchTwo), Constants.sizeTwo, Axis.DescendingY));

        secretDoorSpawnDetailsDict.Add(LocationNameList.campCenter, list);
        #endregion
        #region SECamp
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(LocationNameList.campSouthEast, NPCNameList.wallPatch, PrefabNames.defaultNPCSprite, new Vector3Int(18, 15),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchThree), Constants.sizeTwo, Axis.DescendingX));

        list.Add(new SecretDoorSpawnInfo(LocationNameList.campSouthEast, NPCNameList.wallPatch, PrefabNames.defaultNPCSprite, new Vector3Int(19, 8),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchTwo), Constants.sizeTwo, Axis.DescendingX));

        list.Add(new SecretDoorSpawnInfo(LocationNameList.campSouthEast, NPCNameList.wallPatch, PrefabNames.defaultNPCSprite, new Vector3Int(20, 4),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchOne), Constants.sizeTwo, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(LocationNameList.campSouthEast, list);
        #endregion

        #region MineLvl_2

        #region MineLvl_2-1a
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1a, NPCNameList.suspiciousWall, PrefabNames.mineLvl2WallSecretDoor, new Vector3Int(5, 1),
                                            new SecretDoorInfo(SecretDoorKeyList.mineLvl2FirstSecretDoor), Constants.sizeTwo, Axis.DescendingY));

        secretDoorSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1a, list);
        #endregion

        #region MineLvl_2-7b
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section7b, NPCNameList.suspiciousWall, PrefabNames.mineLvl2WallSecretDoor, new Vector3Int(4, 5),
                                            new SecretDoorInfo(SecretDoorKeyList.mineLvl2SecondSecretDoor), Constants.sizeThree, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section7b, list);
        #endregion

        #endregion

        #region MineLvl_3

        #region MineLvl_3-1b
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(ZoneKeyList.mineLvl3 + LocationNameList.section1b, NPCNameList.suspiciousWall, PrefabNames.mineLvl3WallSecretDoor, new Vector3Int(0, -3),
                                            new SecretDoorInfo(SecretDoorKeyList.mineLvl3PuzzleDoor), Constants.sizeThree, Axis.DescendingY));

        secretDoorSpawnDetailsDict.Add(ZoneKeyList.mineLvl3 + LocationNameList.section1b, list);
        #endregion

        #endregion
    }
}

public enum Axis { DescendingX = 0, DescendingY = 1}


public abstract class AxisSpawnInfo
{
    public string currentArea;

    public Vector3Int startCell;
    public int size;
    public Axis axis;

    public string tutorialTargetHash = "";

    public AxisSpawnInfo(string currentArea, Vector3Int startCell)
    {
        this.currentArea = currentArea;

        this.startCell = startCell;

        this.size = 1;
        this.axis = Axis.DescendingX;
    }

    public AxisSpawnInfo(string currentArea, Vector3Int startCell,  int size, Axis axis)
    {
        this.currentArea = currentArea;

        this.startCell = startCell;
        this.size = size;
        this.axis = axis;
    }

    public abstract bool shouldSpawn();

    public abstract List<OOCSpawnDetails> getSpawnDetails();

}


public class SecretDoorSpawnInfo : AxisSpawnInfo
{
    private string secretDoorName;
    private SecretDoorInfo secretDoorInfo;
    private string spritePathName;

    public SecretDoorSpawnInfo(string currentArea, string secretDoorName, string spritePathName, Vector3Int startCell, SecretDoorInfo secretDoorInfo):
    base(currentArea, startCell)
    {
        this.secretDoorName = secretDoorName;
        this.secretDoorInfo = secretDoorInfo;
        this.spritePathName = spritePathName;
    }

    public SecretDoorSpawnInfo(string currentArea, string secretDoorName, string spritePathName, Vector3Int startCell, SecretDoorInfo secretDoorInfo, int size, Axis axis) :
    base(currentArea, startCell, size, axis)
    {
        this.secretDoorName = secretDoorName;
        this.secretDoorInfo = secretDoorInfo;
        this.spritePathName = spritePathName;        
    }
    
    public SecretDoorSpawnInfo(string currentArea, string secretDoorName, string spritePathName, Vector3Int startCell, SecretDoorInfo secretDoorInfo, int size, Axis axis, string tutorialTargetHash) :
    base(currentArea, startCell, size, axis)
    {
        this.secretDoorName = secretDoorName;
        this.secretDoorInfo = secretDoorInfo;
        this.spritePathName = spritePathName;        
        
        this.tutorialTargetHash = tutorialTargetHash;
    }
    

    public override bool shouldSpawn()
    {
        return !secretDoorInfo.hasBeenDiscovered();
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

            list.Add(new SecretDoorSpawnDetails(secretDoorName, currentCell, currentArea, secretDoorInfo, tutorialTargetHash, spritePathName));
        }

        return list;
    }

}
