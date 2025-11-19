using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyTypeFolderPathList
{

    private const string batsFolderPath = PrefabNames.charactersFolder + EnemyCategoryNameList.bats + "/";

    private static Dictionary<string, string> folderPathDict;

    public static string getEnemyTypeFolderPath(string enemyType)
    {
        if(!folderPathDict.ContainsKey(enemyType))
        {
            return null;
        }

        return folderPathDict[enemyType];
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeEnemyTypeFolderPathList()
    {
        folderPathDict = new Dictionary<string, string>();


        #region Bats
        folderPathDict.Add(MonsterNameList.batSwarm, batsFolderPath + MonsterNameList.batSwarm + "/");
        folderPathDict.Add(MonsterNameList.giantBat, batsFolderPath + MonsterNameList.giantBat + "/");
        folderPathDict.Add(MonsterNameList.armoredBat, batsFolderPath + MonsterNameList.armoredBat + "/");
        folderPathDict.Add(MonsterNameList.screecher, batsFolderPath + MonsterNameList.screecher + "/");
        folderPathDict.Add(MonsterNameList.denMother, batsFolderPath + MonsterNameList.denMother + "/");

        #endregion

    }

}
