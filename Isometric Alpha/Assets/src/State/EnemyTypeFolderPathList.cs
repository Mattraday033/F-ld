using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyTypeFolderPathList
{

    private const string batsFolderPath = PrefabNames.charactersFolder + EnemyCategoryNameList.bats + "/";

    private const string lovashiFolderPath = PrefabNames.charactersFolder + EnemyCategoryNameList.humans + "/" + EnemyCategoryNameList.lovashi + "/";

    private const string partyMembersFolderPath = PrefabNames.charactersFolder + EnemyCategoryNameList.humans + "/PartyMembers/";


    private static Dictionary<string, string> folderPathDict;

    public static string getEnemyTypeFolderPath(string enemyType)
    {
        if(enemyType.Equals(PartyManager.getPlayerStats().getName()) || 
        enemyType.Equals(NPCNameList.carter) || 
        enemyType.Equals(NPCNameList.nandor))
        {
            enemyType = NPCNameList.thatch;
        }

        if(!folderPathDict.ContainsKey(enemyType))
        {
            return folderPathDict[MonsterNameList.executioner];
        }

        return folderPathDict[enemyType];
    }

    [RuntimeInitializeOnLoadMethod]
    public static void initializeEnemyTypeFolderPathList()
    {
        if(folderPathDict != null)
        {
            return;
        }

        folderPathDict = new Dictionary<string, string>();

        #region Bats
        folderPathDict.Add(MonsterNameList.batSwarm, batsFolderPath + MonsterNameList.batSwarm + "/");
        folderPathDict.Add(MonsterNameList.giantBat, batsFolderPath + MonsterNameList.giantBat + "/");
        folderPathDict.Add(MonsterNameList.armoredBat, batsFolderPath + MonsterNameList.armoredBat + "/");
        folderPathDict.Add(MonsterNameList.screecher, batsFolderPath + MonsterNameList.screecher + "/");
        folderPathDict.Add(MonsterNameList.denMother, batsFolderPath + MonsterNameList.denMother + "/");
        folderPathDict.Add(MonsterNameList.caveMatron, batsFolderPath + MonsterNameList.caveMatron + "/");
        #endregion

        #region Lovashi
        folderPathDict.Add(MonsterNameList.axeman, lovashiFolderPath + MonsterNameList.axeman + "/");
        folderPathDict.Add(MonsterNameList.disciplinarian, lovashiFolderPath + MonsterNameList.disciplinarian + "/");
        folderPathDict.Add(MonsterNameList.executioner, lovashiFolderPath + MonsterNameList.executioner + "/");
        folderPathDict.Add(MonsterNameList.javelineer, lovashiFolderPath + MonsterNameList.javelineer + "/");
        folderPathDict.Add(MonsterNameList.lancer, lovashiFolderPath + MonsterNameList.lancer + "/");
        folderPathDict.Add(MonsterNameList.lieutenant, lovashiFolderPath + MonsterNameList.lieutenant + "/");
        folderPathDict.Add(MonsterNameList.lineBreaker, lovashiFolderPath + MonsterNameList.lineBreaker + "/");
        folderPathDict.Add(MonsterNameList.signaleer, lovashiFolderPath + MonsterNameList.signaleer + "/");        
        folderPathDict.Add(MonsterNameList.spearman, lovashiFolderPath + MonsterNameList.spearman + "/");        
        #endregion

        #region Party Members
        
        folderPathDict.Add(NPCNameList.thatch, partyMembersFolderPath + NPCNameList.thatch + "/");        

        #endregion

    }

}
