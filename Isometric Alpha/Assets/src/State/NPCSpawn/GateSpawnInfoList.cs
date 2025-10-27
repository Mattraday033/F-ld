using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GateSpawnInfoList
{
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

        list.Add(new GateSpawnInfo(Constants.indexZero, NPCNameList.liftableRubble, LocationNameList.slaveShackSix, new Vector3Int(6, -1), Constants.sizeTwo, Axis.DescendingY));

        gateSpawnInfoDict.Add(LocationNameList.slaveShackSix, list);

        #endregion

    }
}

public class GateSpawnInfo : AxisSpawnInfo
{
    private int gateIndex;
    private string npcName;

    public GateSpawnInfo(int gateIndex, string npcName, string currentArea, Vector3Int startCell):
    base(currentArea, startCell)
    {
        this.gateIndex = gateIndex;
        this.npcName = npcName;
    }


    public GateSpawnInfo(int gateIndex, string npcName, string currentArea, Vector3Int startCell, int size, Axis axis):
    base(currentArea, startCell, size, axis)
    {
        this.gateIndex = gateIndex;
        this.npcName = npcName;
    }

    private string getGateName()
    {
        if(gateIndex == 0)
        {
            return npcName;
        } else
        {
            return npcName + gateIndex;
        }
    }

    public override bool shouldSpawn()
    {
        return NPCSpawnParamList.getNPCSpawnParams(currentArea, getGateName()).canSpawn(getGateName());
    }

    public override List<OOCSpawnDetails> getSpawnDetails()
    {
        List<OOCSpawnDetails> list = new List<OOCSpawnDetails>();
        List<Vector3Int> extraSpaces = new List<Vector3Int>();

        for (int index = 1; index < size; index++)
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

            extraSpaces.Add(currentCell);
        }

        NPCSpawnDetails gateSpawnDetails = new NPCSpawnDetails(getGateName(), startCell, currentArea, extraSpaces.ToArray());



        list.Add(gateSpawnDetails);

        return list;
    }

}
