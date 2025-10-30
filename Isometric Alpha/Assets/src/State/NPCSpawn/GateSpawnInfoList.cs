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

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.liftableRubble,
                                    LocationNameList.slaveShackSix,
                                    PrefabNames.blockRubble,
                                    new Vector3Int(6, -1),
                                    Constants.sizeTwo,
                                    Axis.DescendingY,
                                    TutorialSequenceList.interactableRubbleTargetHash));


        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.fallenBeam,
                                    LocationNameList.slaveShackSix,
                                    PrefabNames.blockRubble,
                                    new Vector3Int(6, -1),
                                    Constants.sizeTwo,
                                    Axis.DescendingY,
                                    TutorialSequenceList.fallenBeamTargetHash));

        gateSpawnInfoDict.Add(LocationNameList.slaveShackSix, list);

        #endregion

        #region ManseCamp

        list = new List<GateSpawnInfo>();

        list.Add(new GateSpawnInfo(Constants.indexZero,
                                    NPCNameList.manseFrontDoor,
                                    LocationNameList.campManse,
                                    new Vector3Int(3, 9),
                                    Constants.sizeTwo,
                                    Axis.DescendingX));
        

        list.Add(new GateSpawnInfo( Constants.indexOne,
                                    NPCNameList.manseServiceEntrance,
                                    LocationNameList.campManse,
                                    new Vector3Int(-6, 5),
                                    Constants.sizeTwo,
                                    Axis.DescendingX));

        gateSpawnInfoDict.Add(LocationNameList.campManse, list);

        #endregion

    }
}

public class GateSpawnInfo : AxisSpawnInfo
{
    private int gateIndex;
    private string npcName;
    private string spriteName;

    public GateSpawnInfo(int gateIndex, string npcName, string currentArea, Vector3Int startCell):
    base(currentArea, startCell)
    {
        this.gateIndex = gateIndex;
        this.npcName = npcName;
        this.tutorialTargetHash = "";
    }

    public GateSpawnInfo(int gateIndex, string npcName, string currentArea, Vector3Int startCell, int size, Axis axis):
    base(currentArea, startCell, size, axis) 
    {
        this.gateIndex = gateIndex;
        this.npcName = npcName;
        this.tutorialTargetHash = "";
    }

    public GateSpawnInfo(int gateIndex, string npcName, string currentArea, string spriteName, Vector3Int startCell) :
    base(currentArea, startCell)
    {
        this.gateIndex = gateIndex;
        this.npcName = npcName;
        this.spriteName = spriteName;
        this.tutorialTargetHash = "";
    }

    public GateSpawnInfo(int gateIndex, string npcName, string currentArea, string spriteName, Vector3Int startCell, int size, Axis axis):
    base(currentArea, startCell, size, axis) 
    {
        this.gateIndex = gateIndex;
        this.npcName = npcName;
        this.spriteName = spriteName;
        this.tutorialTargetHash = "";
    }

    public GateSpawnInfo(int gateIndex, string npcName, string currentArea, string spriteName, Vector3Int startCell, int size, Axis axis, string tutorialTargetHash):
    base(currentArea, startCell, size, axis) 
    {
        this.gateIndex = gateIndex;
        this.npcName = npcName;
        this.spriteName = spriteName;
        this.tutorialTargetHash = tutorialTargetHash;
    }

    private string getGateName()
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

    private string getSpriteName(Axis axis, int index)
    {
        if(spriteName != null && spriteName.Length > 0)
        {
            return spriteName;
        }

        switch (axis)
        {
            case Axis.DescendingX:
                return PrefabNames.XAxisDoor + (index+1);
            default:
                return PrefabNames.YAxisDoor + (index+1);
        }
    }

    private bool skewed()
    {
        return spriteName != null && spriteName.Length > 0;
    }

    public override bool shouldSpawn()
    {
        return NPCSpawnParamList.getNPCSpawnParams(currentArea, getGateName()).canSpawn(getGateName());
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

            list.Add(new GateSpawnDetails(getGateName(), currentCell, currentArea, getSpriteName(axis,index), tutorialTargetHash, skewed()));
        }

        return list;
    }

}
