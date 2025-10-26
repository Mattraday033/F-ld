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

        list.Add(new SecretDoorSpawnInfo(LocationNameList.slaveShackFive, NPCNameList.wallPatch, new Vector3Int(16, 3), 
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchThree), Constants.sizeTwo, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(LocationNameList.slaveShackFive, list);
        #endregion
        #region 6SlaveShack
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(LocationNameList.slaveShackSix, NPCNameList.wallPatch, new Vector3Int(5, 2), 
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchTwo), Constants.sizeTwo, Axis.DescendingX));

        list.Add(new SecretDoorSpawnInfo(LocationNameList.slaveShackSix, NPCNameList.wallPatch, new Vector3Int(8, -6),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchOne), Constants.sizeTwo, Axis.DescendingX));
                                            
        list.Add(new SecretDoorSpawnInfo(LocationNameList.slaveShackSix, NPCNameList.wallPatch, new Vector3Int(2, -4), 
                                            new SecretDoorInfo(SecretDoorKeyList.wisTutorialSecretDoor), Constants.sizeTwo, Axis.DescendingY));

        secretDoorSpawnDetailsDict.Add(LocationNameList.slaveShackSix, list);
        #endregion

        #region Stables
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(LocationNameList.stables, NPCNameList.wallPatch, new Vector3Int(9, -4), 
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchOne), Constants.sizeTwo, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(LocationNameList.stables, list);
        #endregion
        #region Temple
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(LocationNameList.temple, NPCNameList.wallPatch, new Vector3Int(11, 4), 
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchTwo), Constants.sizeTwo, Axis.DescendingY));

        secretDoorSpawnDetailsDict.Add(LocationNameList.temple, list);
        #endregion

        #region CenterCamp
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(LocationNameList.campCenter, NPCNameList.wallPatch, new Vector3Int(8, 8), 
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchOne), Constants.sizeTwo, Axis.DescendingX));

        list.Add(new SecretDoorSpawnInfo(LocationNameList.campCenter, NPCNameList.wallPatch, new Vector3Int(-5, 14), 
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchTwo), Constants.sizeTwo, Axis.DescendingY));

        secretDoorSpawnDetailsDict.Add(LocationNameList.campCenter, list);
        #endregion
        #region SECamp
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(LocationNameList.campSouthEast, NPCNameList.wallPatch, new Vector3Int(12, 12), 
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchThree), Constants.sizeTwo, Axis.DescendingX));

        list.Add(new SecretDoorSpawnInfo(LocationNameList.campSouthEast, NPCNameList.wallPatch, new Vector3Int(15, 9), 
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchTwo), Constants.sizeTwo, Axis.DescendingX));

        list.Add(new SecretDoorSpawnInfo(LocationNameList.campSouthEast, NPCNameList.wallPatch, new Vector3Int(16, 3), 
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchOne), Constants.sizeTwo, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(LocationNameList.campSouthEast, list);
        #endregion
    }
}

public enum Axis { DescendingX = 0, DescendingY = 1}


public class SecretDoorSpawnInfo
{
    private string currentArea;

    private Vector3Int startCell;
    private int size;
    private Axis axis;

    private string secretDoorName;
    private SecretDoorInfo secretDoorInfo;

    public SecretDoorSpawnInfo(string currentArea, string secretDoorName, Vector3Int startCell, SecretDoorInfo secretDoorInfo)
    {
        this.currentArea = currentArea;

        this.secretDoorName = secretDoorName;
        this.secretDoorInfo = secretDoorInfo;

        this.startCell = startCell;

        this.size = 1;
        this.axis = Axis.DescendingX;
    }

    public SecretDoorSpawnInfo(string currentArea, string secretDoorName, Vector3Int startCell, SecretDoorInfo secretDoorInfo,  int size, Axis axis)
    {
        this.currentArea = currentArea;

        this.secretDoorName = secretDoorName;
        this.secretDoorInfo = secretDoorInfo;

        this.startCell = startCell;
        this.size = size;
        this.axis = axis;
    }

    public bool shouldSpawn()
    {
        return !secretDoorInfo.hasBeenDiscovered();
    }

    public List<SecretDoorSpawnDetails> getSecretDoors()
    {
        List<SecretDoorSpawnDetails> list = new List<SecretDoorSpawnDetails>();

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

            list.Add(new SecretDoorSpawnDetails(secretDoorName, currentCell, currentArea, secretDoorInfo));
        }

        return list;
    }

}
