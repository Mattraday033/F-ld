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

        #region NECamp
        list = new List<MultiTutorialColliderSpawnInfo>();

        list.Add(new MultiTutorialColliderSpawnInfo(LocationNameList.campNorthEast, new Vector3Int(-7, 3),
                                                    TutorialSequenceList.hiddenObjectTutorialSequenceKey,
                                                    TutorialSequenceList.hiddenObjectsTutorialSeenFlag,
                                                    Constants.sizeSix, Axis.DescendingY,
                                                    new StartSpawningAllTrueFlagList(new string[] {FlagNameList.givenTaskByBalint})));

        list.Add(new MultiTutorialColliderSpawnInfo(LocationNameList.campNorthEast, new Vector3Int(-2, -1),
                                                    TutorialSequenceList.hiddenObjectTutorialSequenceKey,
                                                    TutorialSequenceList.hiddenObjectsTutorialSeenFlag,
                                                    new StartSpawningAllTrueFlagList(new string[] {FlagNameList.givenTaskByBalint})));

        tutorialSpawnDetailsDict.Add(LocationNameList.campNorthEast, list);
        #endregion

        #region SECamp
        list = new List<MultiTutorialColliderSpawnInfo>();

        list.Add(new MultiTutorialColliderSpawnInfo(LocationNameList.campSouthEast, new Vector3Int(10,26),
                                                    TutorialSequenceList.questCounterTutorialSequenceKey,
                                                    TutorialSequenceList.questCounterTutorialSeenFlag,
                                                    Constants.sizeThree, Axis.DescendingX));

        list.Add(new MultiTutorialColliderSpawnInfo(LocationNameList.campSouthEast, new Vector3Int(24,1),
                                                    TutorialSequenceList.questCounterTutorialSequenceKey,
                                                    TutorialSequenceList.questCounterTutorialSeenFlag,
                                                    Constants.sizeThree, Axis.DescendingY));

        tutorialSpawnDetailsDict.Add(LocationNameList.campSouthEast, list);
        #endregion

        #region MineLvl_2-1a
        list = new List<MultiTutorialColliderSpawnInfo>();

        list.Add(new MultiTutorialColliderSpawnInfo(LocationNameList.mineLvl2 + LocationNameList.section1a, 
                                                    new Vector3Int(4,-4),
                                                    TutorialSequenceList.movableObjectTutorialSequenceKey,
                                                    TutorialSequenceList.movableObjectTutorialSeenFlag,
                                                    Constants.sizeFive, Axis.DescendingX));

        tutorialSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section1a, list);
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
        return startSpawningFlagList.evaluateFlags() && !Flags.getFlag(seenFlagName);
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