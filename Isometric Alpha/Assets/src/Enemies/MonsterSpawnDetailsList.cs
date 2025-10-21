using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterSpawnDetailsList
{

    private static Dictionary<string, List<MonsterSpawnDetails>> monsterSpawnDetailsDict;
    private static List<MonsterSpawnDetails> list;


    public static List<MonsterSpawnDetails> getMonsterSpawnDetails()
    {
        string key = AreaManager.locationName;

        if (!monsterSpawnDetailsDict.ContainsKey(key))
        {
            return new List<MonsterSpawnDetails>();
        }

        return monsterSpawnDetailsDict[key];
    }

    static MonsterSpawnDetailsList()
    {
        monsterSpawnDetailsDict = new Dictionary<string, List<MonsterSpawnDetails>>();

        #region Camp

        #region 6SlaveShack
        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-1, -5), Facing.SouthEast));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(4, -2), Facing.NorthEast));

        monsterSpawnDetailsDict.Add(AreaNameList.slaveShackSix, list);
        #endregion

        #endregion

        #region Mine Levels 1-3

        #region MineLvl_1-1b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(8, 7)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl1 + AreaNameList.section1b, list);

        #endregion

        #region MineLvl_2-1a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.movableCrate, new Vector3Int(0, 7)));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.movableCrate, new Vector3Int(1, 10)));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.movableCrate, new Vector3Int(1, 13)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl2 + AreaNameList.section1a, list);

        #endregion

        #region MineLvl_2-1b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-2, 4)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl2 + AreaNameList.section1b, list);

        #endregion

        #region MineLvl_2-1c

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(2, -1)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(1, 12)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl2 + AreaNameList.section1c, list);

        #endregion

        #region MineLvl_2-2a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(2, -1)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(1, 12)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl2 + AreaNameList.section2a, list);

        #endregion

        #region MineLvl_2-3a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(8, 12)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl2 + AreaNameList.section3a, list);

        #endregion

        #region MineLvl_2-3b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.movableCrate, new Vector3Int(2, -10)));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.movableCrate, new Vector3Int(-1, -10)));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.movableCrate, new Vector3Int(-5, -10)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl2 + AreaNameList.section3b, list);

        #endregion

        #region MineLvl_2-4

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(3, 8)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(2, 3)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-1, -6)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl2 + AreaNameList.section4, list);

        #endregion

        #region MineLvl_2-7b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-4, 8)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl2 + AreaNameList.section7b, list);

        #endregion

        #region MineLvl_3-1a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-11, 13)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(1, 13)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl3 + AreaNameList.section1a, list);

        #endregion

        #region MineLvl_3-4a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(6, 3)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-3, 3)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl3 + AreaNameList.section4a, list);

        #endregion

        #region MineLvl_3-4b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-7, 3)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl3 + AreaNameList.section4b, list);

        #endregion

        #region MineLvl_3-6a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(3, -3)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl3 + AreaNameList.section6a, list);

        #endregion

        #region MineLvl_3-2a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(0, 0)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl3 + AreaNameList.section2a, list);

        #endregion

        #region MineLvl_3-2b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(7, 6)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-2, -1)));

        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.movableCrate, new Vector3Int(4, 0)));
        list.Add(new MovableObjectSpawnDetails(EnemyCategoryNameList.movableCrate, new Vector3Int(0, 6)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl3 + AreaNameList.section2b, list);

        #endregion

        #region MineLvl_3-3a

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(1, 6)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl3 + AreaNameList.section3a, list);

        #endregion

        #region MineLvl_3-7

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-7, 2))); //boss

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-8, -2)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.worms, new Vector3Int(-4, -6)));
        

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl3 + AreaNameList.section7, list);

        #endregion

        #endregion

    }


}
