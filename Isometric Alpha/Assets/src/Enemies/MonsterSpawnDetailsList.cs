using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterSpawnDetailsList
{

    private static Dictionary<string, List<MonsterSpawnDetails>> monsterSpawnDetailsDict;
    private static List<MonsterSpawnDetails> list;


    public static List<MonsterSpawnDetails> getMonsterSpawnDetails(string key)
    {
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

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(-1, -5)));
        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(4, -2)));

        monsterSpawnDetailsDict.Add(AreaNameList.slaveShackSix, list);
        #endregion

        #endregion

        #region Mine Levels 1-3

        #region MineLvl_1-1b

        list = new List<MonsterSpawnDetails>();

        list.Add(new MonsterSpawnDetails(EnemyCategoryNameList.bats, new Vector3Int(8, 7)));

        monsterSpawnDetailsDict.Add(AreaNameList.mineLvl1 + AreaNameList.section1b, list);

        #endregion

        #endregion

    }


}
