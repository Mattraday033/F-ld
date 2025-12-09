using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterSpawnDetailsList
{

    public const bool chasesPlayer = true;
    private static Dictionary<string, List<MonsterSpawnDetails>> monsterSpawnDetailsDict;


    public static List<MonsterSpawnDetails> getMonsterSpawnDetails()
    {
        string key = AreaManager.locationName;

        if (!monsterSpawnDetailsDict.ContainsKey(key))
        {
            return new List<MonsterSpawnDetails>();
        }

        return monsterSpawnDetailsDict[key];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateMonsterSpawnDetailsList()
    {
        monsterSpawnDetailsDict = new Dictionary<string, List<MonsterSpawnDetails>>();
        List<MonsterSpawnDetails> list;

        #region Camp

        #region 6SlaveShack
        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-1, -5), Facing.SouthEast, TutorialSequenceList.firstTutorialEnemyTargetHash));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(4, -2), Facing.NorthEast, TutorialSequenceList.secondTutorialEnemyTargetHash));

        monsterSpawnDetailsDict.Add(LocationNameList.slaveShackSix, list);
        #endregion

        #region NECamp
        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(13, -3), Facing.SouthWest));

        monsterSpawnDetailsDict.Add(LocationNameList.campNorthEast, list);
        #endregion

        #endregion

        #region Mine Levels 1-3

        #region MineLvl_1-1b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(8, 7)));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl1 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_2-1a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(2, -1), PrefabNames.pushableCrate, TutorialSequenceList.tutorialCrateTargetHash));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(1, 2), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section1a, list);

        #endregion

        #region MineLvl_2-1b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(1, 6)));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section1b, list);

        #endregion

        #region MineLvl_2-1c

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(0, -1)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(1, 13)));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section1c, list);

        #endregion

        #region MineLvl_2-2b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-1, 9)));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_2-3a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(8, 12)));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section3a, list);

        #endregion

        #region MineLvl_2-3b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(3, 1), Facing.SouthEast));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(0, 0), Facing.SouthEast));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-4, 0), Facing.SouthWest));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(2, -10), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-1, -10), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, -10), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section3b, list);

        #endregion

        #region MineLvl_2-4

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(5, 9)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(4, 2)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-2, -5), Facing.NorthWest));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section4, list);

        #endregion

        #region MineLvl_2-5

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(9, 2), chasesPlayer));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(9, 16), chasesPlayer));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section5, list);

        #endregion
        #region MineLvl_2-7b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(1, -11)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-3, 2)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(3, 9)));

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-3, 9), Facing.SouthWest));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl2 + LocationNameList.section7b, list);

        #endregion

        #region MineLvl_3-1a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-11, 13), chasesPlayer));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(1, 13), chasesPlayer));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl3 + LocationNameList.section1a, list);

        #endregion

        #region MineLvl_3-1b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(4, -2), chasesPlayer, Facing.NorthWest));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl3 + LocationNameList.section1b, list);

        #endregion


        #region MineLvl_3-2a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-1, -2), chasesPlayer));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl3 + LocationNameList.section2a, list);

        #endregion

        #region MineLvl_3-2b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(1, 2)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(5, 3)));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(9, 3), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(2, -2), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl3 + LocationNameList.section2b, list);

        #endregion

        #region MineLvl_3-3a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(1, 6)));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl3 + LocationNameList.section3a, list);

        #endregion

        #region MineLvl_3-4a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-7, 4), Facing.SouthWest));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl3 + LocationNameList.section4a, list);

        #endregion

        #region MineLvl_3-4b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-4, 2), chasesPlayer, Facing.SouthWest));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(2, 13), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl3 + LocationNameList.section4b, list);

        #endregion

        #region MineLvl_3-5

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(1, 1), chasesPlayer));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(16, 3), chasesPlayer));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(15, -1), chasesPlayer));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl3 + LocationNameList.section5, list);

        #endregion

        #region MineLvl_3-6a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-4, 17)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-16, 9)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-14, -4)));
        
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-6, 13), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-7, 12), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, 11), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, 10), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-5, 9), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(-4, 12), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl3 + LocationNameList.section6a, list);

        #endregion

        #region MineLvl_3-7

        list = new List<MonsterSpawnDetails>();

        // list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-7, 2))); //boss

        // list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-8, -2)));
        // list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-4, -6)));
        
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(8, 0), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(6, 2), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(6, -2), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(4, -1), PrefabNames.pushableCrate));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.pushableCrate, new Vector3Int(3, -3), PrefabNames.pushableCrate));

        monsterSpawnDetailsDict.Add(LocationNameList.mineLvl3 + LocationNameList.section7, list);

        #endregion

        #endregion

        #region Manse

        #region Manse-1F

        #endregion

        #region Manse-2F

        #region Manse-2F-3b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.lovashi, new Vector3Int(-8, 5), Facing.NorthEast));

        monsterSpawnDetailsDict.Add(LocationNameList.manseSecondFloor + LocationNameList.section3b, list);

        #endregion

        #endregion

        #endregion

    }


}
