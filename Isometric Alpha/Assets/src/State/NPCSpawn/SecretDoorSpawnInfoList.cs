using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SecretDoorSpawnInfoList
{
    private static Dictionary<string, List<SecretDoorSpawnInfo>> secretDoorSpawnDetailsDict;

    public static List<SecretDoorSpawnInfo> getSecretDoorSpawnDetails(string areaName)
    {
        if(secretDoorSpawnDetailsDict == null)
        {
            initializeSpawnDetailsList();
        }

        if (!secretDoorSpawnDetailsDict.ContainsKey(areaName))
        {
            return new List<SecretDoorSpawnInfo>();
        }

        return secretDoorSpawnDetailsDict[areaName];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeSpawnDetailsList()
    {
        if(secretDoorSpawnDetailsDict != null)
        {
            return;
        }

        secretDoorSpawnDetailsDict = new Dictionary<string, List<SecretDoorSpawnInfo>>();
        List<SecretDoorSpawnInfo> list;

        #region 4SlaveShack
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new WallPatchSpawnInfo(LocationNameList.slaveShackFour, new Vector3Int(5, 18),
                                            new SecretDoorInfo(SecretDoorKeyList.wisTutorialSecretDoor),
                                            Constants.sizeTwo, Axis.DescendingY,
                                            TutorialSequenceList.secretDoorTargetHash));

        secretDoorSpawnDetailsDict.Add(LocationNameList.slaveShackFour, list);
        #endregion

        #region 5SlaveShack
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new WallPatchSpawnInfo(LocationNameList.slaveShackFive, new Vector3Int(2, -6),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchThree), Constants.sizeTwo, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(LocationNameList.slaveShackFive, list);
        #endregion
        #region 6SlaveShack
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new WallPatchSpawnInfo(LocationNameList.slaveShackSix, new Vector3Int(5, 2),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchTwo), Constants.sizeTwo, Axis.DescendingX));

        list.Add(new WallPatchSpawnInfo(LocationNameList.slaveShackSix, new Vector3Int(8, -6),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchOne), Constants.sizeTwo, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(LocationNameList.slaveShackSix, list);
        #endregion

        #region Stables
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new WallPatchSpawnInfo(LocationNameList.stables, new Vector3Int(9, -4),
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchOne), Constants.sizeTwo, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(LocationNameList.stables, list);
        #endregion
        #region Temple
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new WallPatchSpawnInfo(LocationNameList.temple, new Vector3Int(11, 4),
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchTwo), Constants.sizeTwo, Axis.DescendingY));

        secretDoorSpawnDetailsDict.Add(LocationNameList.temple, list);
        #endregion

        #region CenterCamp
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new WallPatchSpawnInfo(LocationNameList.campCenter, new Vector3Int(8, 12),
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchOne, addHostilityIfOutside: true), Constants.sizeTwo, Axis.DescendingX, 
                                            terrainSpriteName: PrefabNames.wallPatch, tall: true));

        list.Add(new WallPatchSpawnInfo(LocationNameList.campCenter, new Vector3Int(-7, 14),
                                            new SecretDoorInfo(SecretDoorKeyList.centerCampWallPatchTwo, addHostilityIfOutside: true), Constants.sizeTwo, Axis.DescendingY, 
                                            terrainSpriteName: PrefabNames.wallPatch, tall: true));

        secretDoorSpawnDetailsDict.Add(LocationNameList.campCenter, list);
        #endregion
        #region SECamp
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new WallPatchSpawnInfo(LocationNameList.campSouthEast, new Vector3Int(18, 15),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchThree, addHostilityIfOutside: true), Constants.sizeTwo, Axis.DescendingX,
                                             terrainSpriteName: PrefabNames.wallPatch, tall: true));

        list.Add(new WallPatchSpawnInfo(LocationNameList.campSouthEast, new Vector3Int(19, 8),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchTwo, addHostilityIfOutside: true), Constants.sizeTwo, Axis.DescendingX,
                                             terrainSpriteName: PrefabNames.wallPatch, tall: true));

        list.Add(new WallPatchSpawnInfo(LocationNameList.campSouthEast, new Vector3Int(20, 4),
                                            new SecretDoorInfo(SecretDoorKeyList.southEastCampWallPatchOne, addHostilityIfOutside: true), Constants.sizeTwo, Axis.DescendingX,
                                             terrainSpriteName: PrefabNames.wallPatch, tall: true));

        secretDoorSpawnDetailsDict.Add(LocationNameList.campSouthEast, list);
        #endregion
        #region NWCamp
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new WallPatchSpawnInfo(LocationNameList.campNorthWest, new Vector3Int(4, 1),
                                            new SecretDoorInfo(SecretDoorKeyList.northWestCampWallPatchOne, addHostilityIfOutside: false), Constants.sizeThree, Axis.DescendingY, 
                                            terrainSpriteName: PrefabNames.wallPatch, observable: () => Flags.getFlag(FlagNameList.startedTaborObservationTutorial),
                                            script: new TaborObservationTutorialScript(),
                                            tutorialTargetHash: TutorialSequenceList.secretDoorTargetHash, tall: true));

        secretDoorSpawnDetailsDict.Add(LocationNameList.campNorthWest, list);
        #endregion

        #region Manse-1f

        #region Manse-1f-2a
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, NPCNameList.suspiciousWall, PrefabNames.manseWallSecretDoor, new Vector3Int(-1, 10),
                                            new SecretDoorInfo(SecretDoorKeyList.manseHiddenGardenFlag, difficulty: Constants.difficultyThree), Constants.sizeFive, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section2a, list);
        #endregion

        #region Manse-1f-3a
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3a, NPCNameList.suspiciousWall, PrefabNames.manseWallSecretDoor, new Vector3Int(9, -3),
                                            new SecretDoorInfo(SecretDoorKeyList.meetingRoomSecretEntrance, difficulty: Constants.difficultyThree), Constants.sizeThree, Axis.DescendingY));

        secretDoorSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3a, list);
        #endregion

        #region Manse-1f-3c
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(ZoneKeyList.manseFirstFloor + LocationNameList.section3c, NPCNameList.suspiciousWall, PrefabNames.secretShelfNWSecretDoor, new Vector3Int(3, 0),
                                            new SecretDoorInfo(SecretDoorKeyList.secretBookShelfFlag, difficulty: Constants.difficultyThree, description: "*These bookshelfs look exactly like the others, except for a distinct lack of dust.*"), Constants.sizeTwo, Axis.DescendingX));

        secretDoorSpawnDetailsDict.Add(ZoneKeyList.manseFirstFloor + LocationNameList.section3c, list);
        #endregion

        #endregion

        #region Manse-2f

        #region Manse-2f-Office
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(ZoneKeyList.manseSecondFloor + LocationNameList.office, NPCNameList.suspiciousWall, PrefabNames.manseWallSecretDoor, new Vector3Int(4, 2),
                                            new SecretDoorInfo(SecretDoorKeyList.officeSecretEntranceFlag, difficulty: Constants.difficultyThree, customDialoguePath: DialogueNameList.officeSecretDoorPathName),
                                            Constants.sizeFour, Axis.DescendingY));

        secretDoorSpawnDetailsDict.Add(ZoneKeyList.manseSecondFloor + LocationNameList.office, list);
        #endregion

        #endregion

        #region MineLvl_2

        #region MineLvl_2-1a
        list = new List<SecretDoorSpawnInfo>();

        list.Add(new SecretDoorSpawnInfo(ZoneKeyList.mineLvl2 + LocationNameList.section1a, NPCNameList.suspiciousWall, PrefabNames.mineLvl2WallSecretDoor, new Vector3Int(5, 1),
                                            new SecretDoorInfo(SecretDoorKeyList.mineLvl2FirstSecretDoor, questName: QuestNameList.hiddenAwayQuestTitle, questStepName: QuestNameList.hiddenAwayStepTitleOne, completeQuest: true), Constants.sizeTwo, Axis.DescendingY));

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

    public AxisSpawnInfo(string currentArea, Vector3Int startCell, int size = 1, Axis axis = Axis.DescendingX)
    {
        this.currentArea = currentArea;

        this.startCell = startCell;
        
        this.size = size;
        this.axis = axis;
    }

    public abstract bool shouldSpawn();

    public abstract List<OOCSpawnDetails> getSpawnDetails();

}

public delegate bool ObservableDelegate();

public class SecretDoorSpawnInfo : AxisSpawnInfo
{
    private string secretDoorName;
    protected SecretDoorInfo secretDoorInfo;
    protected string spritePathName;
    private string terrainSpriteName;
    private ObservableDelegate observable;
    private QuestStepActivationScript script;

    public SecretDoorSpawnInfo( string currentArea,
                                string secretDoorName,
                                string spritePathName, 
                                Vector3Int startCell, 
                                SecretDoorInfo secretDoorInfo, 
                                int size = 1, 
                                Axis axis = Axis.DescendingX, 
                                string tutorialTargetHash = "", 
                                string terrainSpriteName = "",
                                ObservableDelegate observable = null,
                                QuestStepActivationScript script = null):
    base(currentArea, startCell, size, axis)
    {
        this.secretDoorName = secretDoorName;
        this.secretDoorInfo = secretDoorInfo;
        this.spritePathName = spritePathName;        

        this.tutorialTargetHash = tutorialTargetHash;
        this.terrainSpriteName = terrainSpriteName;

        this.observable = observable;
        this.script = script;
    }
    

    public override bool shouldSpawn()
    {
        return !secretDoorInfo.hasBeenDiscovered();
    }

    public string getPrimarySecretDoorKey()
    {
        if(secretDoorInfo == null || secretDoorInfo.secretDoorKeys.Count == 0)
        {
            return null;
        }

        return secretDoorInfo.secretDoorKeys[0];
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

            list.Add(new SecretDoorSpawnDetails(secretDoorName, currentCell, currentArea, secretDoorInfo, tutorialTargetHash, spritePathName, terrainSpriteName, observable, script));
        }

        return list;
    }

}

public class WallPatchSpawnInfo : SecretDoorSpawnInfo
{
    public WallPatchSpawnInfo( string currentArea,
                                Vector3Int startCell, 
                                SecretDoorInfo secretDoorInfo, 
                                int size = 1, 
                                Axis axis = Axis.DescendingX, 
                                string tutorialTargetHash = "", 
                                string terrainSpriteName = "",
                                ObservableDelegate observable = null,
                                QuestStepActivationScript script = null,
                                bool tall = false):
    base(   currentArea,
            NPCNameList.wallPatch,
            PrefabNames.wallPatch, 
            startCell, 
            secretDoorInfo, 
            size, 
            axis, 
            tutorialTargetHash, 
            terrainSpriteName,
            observable,
            script)
    {
        secretDoorInfo.description = "*This section of boards holds up the structure's ceiling. Redundancies allow a determined individual to remove some of the boards without collapsing the roof.";
        secretDoorInfo.searchChoice = "Attempt to make a hole in the wall.*";
        secretDoorInfo.successDescription = "*After a moment of planning, you understand the safest order to remove the boards.*";
        secretDoorInfo.successChoice = "Start removing boards.*";
        secretDoorInfo.failureDescription = "*You are unable to safely remove the boards.*";
        secretDoorInfo.openDescription = "*The way is open.*";

        if(tall)
        {
            this.spritePathName = PrefabNames.wallPatchTall;
        }
    }
}
