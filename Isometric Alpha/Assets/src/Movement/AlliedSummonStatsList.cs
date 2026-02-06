using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AlliedSummonStatsList
{
    public static Dictionary<string, AlliedSummonStats> allyStatsDict
    {
        private get;
        set;
    }

    public static AlliedSummonStats getSummonStats(string key)
    {
        if(allyStatsDict == null)
        {
            EnemyStatsList.initialize();
        }

        if (!allyStatsDict.ContainsKey(key))
        {
            return null;
        }

        return allyStatsDict[key];
    }


    public static void addEnemyBasedSummons()
    {
        #region Named NPCs
        #region Lovashi Guards
        allyStatsDict.Add(NPCNameList.guardReka, new AlliedSummonStats(EnemyStatsList.getEnemyStats(NPCNameList.guardReka)));
        allyStatsDict.Add(NPCNameList.guardVirag, new AlliedSummonStats(EnemyStatsList.getEnemyStats(NPCNameList.guardVirag)));
        allyStatsDict.Add(NPCNameList.guardPazman, new AlliedSummonStats(EnemyStatsList.getEnemyStats(NPCNameList.guardPazman)));
        allyStatsDict.Add(NPCNameList.overseerGaspar, new AlliedSummonStats(EnemyStatsList.getEnemyStats(NPCNameList.overseerGaspar)));
        #endregion
        #endregion

        #region Branded Slaves
        allyStatsDict.Add(MonsterNameList.brandedRioter, new AlliedSummonStats(EnemyStatsList.getEnemyStats(MonsterNameList.brandedRioter)));
        #endregion

        #region NonBranded Slaves
        allyStatsDict.Add(MonsterNameList.noBrandRioter, new AlliedSummonStats(EnemyStatsList.getEnemyStats(MonsterNameList.noBrandRioter)));
        #endregion
    }

}
