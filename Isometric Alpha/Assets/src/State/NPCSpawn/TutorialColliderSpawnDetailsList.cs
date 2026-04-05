using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialColliderSpawnDetailsList
{

    private static Dictionary<string, List<MultiTutorialColliderSpawnInfo>> tutorialSpawnDetailsDict;

    public static List<MultiTutorialColliderSpawnInfo> getTutorialColliderSpawnDetails(string locationName)
    {
        if (!tutorialSpawnDetailsDict.ContainsKey(locationName))
        {
            return new List<MultiTutorialColliderSpawnInfo>();
        }

        return tutorialSpawnDetailsDict[locationName];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeSpawnDetailsList()
    {
        tutorialSpawnDetailsDict = new Dictionary<string, List<MultiTutorialColliderSpawnInfo>>();
        List<MultiTutorialColliderSpawnInfo> list;

        #region SECamp
        list = new List<MultiTutorialColliderSpawnInfo>();

        list.Add(new MultiTutorialColliderSpawnInfo(LocationNameList.campSouthEast, new Vector3Int(9,26),
                                                    TutorialSequenceList.hiddenObjectsTutorialSequenceKey,
                                                    TutorialSequenceList.hiddenObjectsTutorialSeenFlag,
                                                    Constants.sizeThree, Axis.DescendingX));

        list.Add(new MultiTutorialColliderSpawnInfo(LocationNameList.campSouthEast, new Vector3Int(28,1),
                                                    TutorialSequenceList.hiddenObjectsTutorialSequenceKey,
                                                    TutorialSequenceList.hiddenObjectsTutorialSeenFlag,
                                                    Constants.sizeThree, Axis.DescendingY));

        tutorialSpawnDetailsDict.Add(LocationNameList.campSouthEast, list);
        #endregion

        #region MineLvl_1-1b
        list = new List<MultiTutorialColliderSpawnInfo>();

        list.Add(new MultiTutorialColliderSpawnInfo(ZoneKeyList.mineLvl1 + LocationNameList.section1b, 
                                                    new Vector3Int(9,-3),
                                                    TutorialSequenceList.secondLeadershipTutorialSequenceKey,
                                                    TutorialSequenceList.secondLeadershipTutorialSeenFlag,
                                                    Constants.sizeFour, Axis.DescendingX));

        tutorialSpawnDetailsDict.Add(ZoneKeyList.mineLvl1 + LocationNameList.section1b, list);
        #endregion

        #region MineLvl_2-1a
        list = new List<MultiTutorialColliderSpawnInfo>();

        list.Add(new MultiTutorialColliderSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1a, 
                                                    new Vector3Int(4,-4),
                                                    TutorialSequenceList.movableObjectTutorialSequenceKey,
                                                    TutorialSequenceList.movableObjectTutorialSeenFlag,
                                                    Constants.sizeFive, Axis.DescendingX));

        list.Add(new MultiTutorialColliderSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1a, 
                                                    new Vector3Int(4,7),
                                                    TutorialSequenceList.secondObservationTutorialSequenceKey,
                                                    TutorialSequenceList.secondObservationTutorialSeenFlag,
                                                    Constants.sizeFive, Axis.DescendingX));

        list.Add(new MultiTutorialColliderSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1a, 
                                                    new Vector3Int(4,-2),
                                                    TutorialSequenceList.secondObservationTutorialSequenceKey,
                                                    TutorialSequenceList.secondObservationTutorialSeenFlag,
                                                    Constants.sizeFive, Axis.DescendingX));

        tutorialSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section1a, list);
        #endregion

        #region MineLvl_2-5
        list = new List<MultiTutorialColliderSpawnInfo>();

        list.Add(new MultiTutorialColliderSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section5, 
                                                    new Vector3Int(3, 10),
                                                    TutorialSequenceList.thirdCunningTutorialSequenceKey,
                                                    TutorialSequenceList.thirdCunningTutorialSeenFlag,
                                                    Constants.sizeTwo, Axis.DescendingY));

        tutorialSpawnDetailsDict.Add(ZoneKeyList.mineLvl2 + LocationNameList.section5, list);
        #endregion
    }
}

public class MultiTutorialColliderSpawnInfo : AxisSpawnInfo
{
    private string tutorialKey;
    private string seenFlagName;

    private StartSpawningAllTrueFlagList startSpawningFlagList;

    public MultiTutorialColliderSpawnInfo(string currentLocation, Vector3Int startCell, string tutorialKey, string seenFlagName) :
    base(currentLocation, startCell)
    {
        this.tutorialKey = tutorialKey;
        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = new StartSpawningAllTrueFlagList();
    }

    public MultiTutorialColliderSpawnInfo(string currentLocation, Vector3Int startCell, string tutorialKey, string seenFlagName, StartSpawningAllTrueFlagList startSpawningFlagList) :
    base(currentLocation, startCell)
    {
        this.tutorialKey = tutorialKey;
        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = startSpawningFlagList;
    }

    public MultiTutorialColliderSpawnInfo(string currentLocation, Vector3Int startCell, string tutorialKey, string seenFlagName, int size, Axis axis) :
    base(currentLocation, startCell, size, axis)
    {
        this.tutorialKey = tutorialKey;
        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = new StartSpawningAllTrueFlagList();
    }

    public MultiTutorialColliderSpawnInfo(string currentLocation, Vector3Int startCell, string tutorialKey, string seenFlagName, int size, Axis axis, StartSpawningAllTrueFlagList startSpawningFlagList) :
    base(currentLocation, startCell, size, axis)
    {
        this.tutorialKey = tutorialKey;
        this.seenFlagName = seenFlagName;
        this.startSpawningFlagList = startSpawningFlagList;
    }

    public override bool shouldSpawn()
    {
        return startSpawningFlagList.evaluateFlags() && !TutorialFlags.getFlag(seenFlagName);
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

            list.Add(new TutorialColliderSpawnDetails(currentCell, tutorialKey, seenFlagName, startSpawningFlagList));
        }

        return list;
    }

}