using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnemyTypeFolderPathList
{

    private const string batsFolderPath = PrefabNames.charactersFolder + EnemyCategoryNameList.bats + "/";

    private const string brandedFolderPath = PrefabNames.charactersFolder + EnemyCategoryNameList.humans + "/Branded Slaves/";
    private const string nonbrandedFolderPath = PrefabNames.charactersFolder + EnemyCategoryNameList.humans + "/Nonbranded Slaves/";

    private const string lovashiFolderPath = PrefabNames.charactersFolder + EnemyCategoryNameList.humans + "/" + EnemyCategoryNameList.lovashi + "/";

    private const string miscFolderPath = PrefabNames.charactersFolder + "Misc/";
    private const string horsesFolderPath = PrefabNames.charactersFolder + "Horses/";

    private const string partyMembersFolderPath = PrefabNames.charactersFolder + EnemyCategoryNameList.humans + "/PartyMembers/";


    private static Dictionary<string, string> folderPathDict;

    public static string getEnemyTypeFolderPath(string enemyType)
    {
        if(enemyType.Contains(PartyManager.playerMarker))
        {
            enemyType = NPCNameList.thatch;
        }

        if(!folderPathDict.ContainsKey(enemyType))
        {
            enemyType = DialogueList.scrubNameOfEndNumbers(enemyType);
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
        folderPathDict.Add(MonsterNameList.linebreaker, lovashiFolderPath + MonsterNameList.linebreaker + "/");
        folderPathDict.Add(MonsterNameList.overseer, lovashiFolderPath + MonsterNameList.overseer + "/");
        folderPathDict.Add(MonsterNameList.signaleer, lovashiFolderPath + MonsterNameList.signaleer + "/");
        folderPathDict.Add(MonsterNameList.spearman, lovashiFolderPath + MonsterNameList.spearman + "/");

        //Named Lovashi
        folderPathDict.Add(NPCNameList.andras, lovashiFolderPath + NPCNameList.guardAndras + "/");
        folderPathDict.Add(NPCNameList.guardAndras, lovashiFolderPath + NPCNameList.guardAndras + "/");
        folderPathDict.Add(NPCNameList.director, lovashiFolderPath + NPCNameList.director + "/");
        folderPathDict.Add(NPCNameList.quartermasterEmese, lovashiFolderPath + NPCNameList.quartermasterEmese + "/");
        folderPathDict.Add(NPCNameList.guardLaszlo, lovashiFolderPath + NPCNameList.guardLaszlo + "/");
        folderPathDict.Add(NPCNameList.guardMuzsa, lovashiFolderPath + NPCNameList.guardMuzsa + "/");
        folderPathDict.Add(NPCNameList.kende, lovashiFolderPath + NPCNameList.kende + "/");
        folderPathDict.Add(NPCNameList.marcos, lovashiFolderPath + NPCNameList.marcos + "/");
        folderPathDict.Add(NPCNameList.guardMarcos, lovashiFolderPath + NPCNameList.guardMarcos + "/");
        folderPathDict.Add(NPCNameList.pazman, lovashiFolderPath + NPCNameList.guardPazman + "/");
        folderPathDict.Add(NPCNameList.guardPazman, lovashiFolderPath + NPCNameList.guardPazman + "/");
        folderPathDict.Add(NPCNameList.reka, lovashiFolderPath + NPCNameList.guardReka + "/");
        folderPathDict.Add(NPCNameList.guardReka, lovashiFolderPath + NPCNameList.guardReka+ "/");
        folderPathDict.Add(NPCNameList.guardVazul, lovashiFolderPath + MonsterNameList.spearman + "/");
        folderPathDict.Add(NPCNameList.guardVirag, lovashiFolderPath + NPCNameList.guardVirag+ "/");
        #endregion

        #region Slaves

        #region Branded Slaves
        folderPathDict.Add(NPCNameList.broglin, brandedFolderPath + NPCNameList.broglin + "/");
        folderPathDict.Add(NPCNameList.balint, brandedFolderPath + NPCNameList.balint + "/");
        folderPathDict.Add(NPCNameList.clay, brandedFolderPath + NPCNameList.clay + "/");
        folderPathDict.Add(NPCNameList.ervin, brandedFolderPath + NPCNameList.ervin + "/");
        folderPathDict.Add(NPCNameList.feher, brandedFolderPath + NPCNameList.feher + "/");
        folderPathDict.Add(NPCNameList.garcha, brandedFolderPath + NPCNameList.garcha + "/");
        folderPathDict.Add(NPCNameList.janos, brandedFolderPath + NPCNameList.janos + "/");
        folderPathDict.Add(NPCNameList.kastor, brandedFolderPath + NPCNameList.kastor + "/");
        folderPathDict.Add(NPCNameList.temple, brandedFolderPath + NPCNameList.temple + "/");
        folderPathDict.Add(NPCNameList.uros, brandedFolderPath + NPCNameList.uros + "/");

        folderPathDict.Add(NPCNameList.seb, brandedFolderPath + MonsterNameList.brandedConscript + "/");
        folderPathDict.Add(NPCNameList.slate, brandedFolderPath + MonsterNameList.brandedConscript + "/");

        folderPathDict.Add(MonsterNameList.brandedConscript, brandedFolderPath + MonsterNameList.brandedConscript + "/");   

        folderPathDict.Add(MonsterNameList.brandedRioter +  MonsterNameList.pickMarker, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.pickMarker + "/");   
        folderPathDict.Add(MonsterNameList.brandedRioter +  MonsterNameList.shivMarker, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shivMarker + "/");   
        folderPathDict.Add(MonsterNameList.brandedRioter +  MonsterNameList.shovelMarker, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shovelMarker + "/");   

        folderPathDict.Add(NPCNameList.slave, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.pickMarker + "/");   
        folderPathDict.Add(NPCNameList.slave+1, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shivMarker + "/");   
        folderPathDict.Add(NPCNameList.slave+2, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shovelMarker + "/");
        folderPathDict.Add(NPCNameList.slave+3, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.pickMarker + "/");   
        folderPathDict.Add(NPCNameList.slave+4, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shivMarker + "/");   
        folderPathDict.Add(NPCNameList.slave+5, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shovelMarker + "/");
        folderPathDict.Add(NPCNameList.slave+6, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.pickMarker + "/");   
        folderPathDict.Add(NPCNameList.slave+7, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shivMarker + "/");   
        folderPathDict.Add(NPCNameList.slave+8, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shovelMarker + "/");  
        folderPathDict.Add(NPCNameList.slave+9, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.pickMarker + "/");   
        folderPathDict.Add(NPCNameList.slave+10, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shivMarker + "/");   
        folderPathDict.Add(NPCNameList.slave+11, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shovelMarker + "/");  
        folderPathDict.Add(NPCNameList.slave+12, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.pickMarker + "/");

        folderPathDict.Add(NPCNameList.woundedSlave, brandedFolderPath + NPCNameList.ervin + "/");
        folderPathDict.Add(NPCNameList.woundedSlave+1, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shivMarker + "/");
        folderPathDict.Add(NPCNameList.woundedSlave+2, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.shovelMarker + "/");
        
        folderPathDict.Add(NPCNameList.crowd, brandedFolderPath + MonsterNameList.brandedRioter + MonsterNameList.pickMarker + "/");           
        #endregion

        #region Nonbranded Slaves
        folderPathDict.Add(NPCNameList.beam, nonbrandedFolderPath + NPCNameList.beam + "/");
        folderPathDict.Add(NPCNameList.imre, nonbrandedFolderPath + NPCNameList.imre + "/");
        folderPathDict.Add(NPCNameList.page, nonbrandedFolderPath + NPCNameList.page + "/");

        folderPathDict.Add(NPCNameList.noBrand, nonbrandedFolderPath + MonsterNameList.noBrandLoyalist + Constants.maleMarker + "/");
        folderPathDict.Add(NPCNameList.noBrand+1, nonbrandedFolderPath + MonsterNameList.noBrandLoyalist + Constants.femaleMarker + "/");
        folderPathDict.Add(NPCNameList.noBrand+4, nonbrandedFolderPath + MonsterNameList.noBrandLoyalist + Constants.femaleMarker + "/");
        folderPathDict.Add(MonsterNameList.noBrandLoyalist + Constants.maleMarker, nonbrandedFolderPath + MonsterNameList.noBrandLoyalist + Constants.maleMarker + "/");
        folderPathDict.Add(MonsterNameList.noBrandLoyalist + Constants.femaleMarker, nonbrandedFolderPath + MonsterNameList.noBrandLoyalist + Constants.femaleMarker + "/");

        folderPathDict.Add(NPCNameList.pan, nonbrandedFolderPath + MonsterNameList.noBrandRioter + Constants.maleMarker + "/");
        folderPathDict.Add(NPCNameList.noBrand+2, nonbrandedFolderPath + MonsterNameList.noBrandRioter + Constants.maleMarker + "/");
        folderPathDict.Add(NPCNameList.noBrand+3, nonbrandedFolderPath + MonsterNameList.noBrandRioter + Constants.femaleMarker + "/");
        folderPathDict.Add(NPCNameList.noBrand+5, nonbrandedFolderPath + MonsterNameList.noBrandRioter + Constants.maleMarker + "/");
        folderPathDict.Add(MonsterNameList.noBrandRioter + Constants.femaleMarker, nonbrandedFolderPath + MonsterNameList.noBrandRioter + Constants.femaleMarker + "/");
        folderPathDict.Add(MonsterNameList.noBrandRioter + Constants.maleMarker, nonbrandedFolderPath + MonsterNameList.noBrandRioter + Constants.maleMarker + "/");
        #endregion

        #endregion

        #region Misc
    
        folderPathDict.Add(NPCNameList.barricade, miscFolderPath + NPCNameList.barricade + "/");

        #endregion

        #region Horses

        folderPathDict.Add(NPCNameList.horse, horsesFolderPath + MonsterNameList.horseCharger + "/");
        folderPathDict.Add(NPCNameList.horse+1, horsesFolderPath + MonsterNameList.horseCharger + "/");
        folderPathDict.Add(NPCNameList.horse+2, horsesFolderPath + MonsterNameList.horseStomper + "/");

        folderPathDict.Add(NPCNameList.csalan, horsesFolderPath + MonsterNameList.horseStomper + "/");

        #endregion

        #region Party Members
        
        folderPathDict.Add(NPCNameList.thatch, partyMembersFolderPath + NPCNameList.thatch + "/");  
        folderPathDict.Add(NPCNameList.carter, partyMembersFolderPath + NPCNameList.carter + "/");  
        folderPathDict.Add(NPCNameList.nandor, partyMembersFolderPath + NPCNameList.nandor + "/");     

        folderPathDict.Add(NPCNameList.sampson, partyMembersFolderPath + NPCNameList.sampson + "/");     

        folderPathDict.Add(NPCNameList.tabor, partyMembersFolderPath + NPCNameList.tabor + "/");     
        folderPathDict.Add(NPCNameList.chiefTabor, partyMembersFolderPath + NPCNameList.tabor + "/");     

        folderPathDict.Add(NPCNameList.gaspar, partyMembersFolderPath + NPCNameList.gaspar + "/");  
        folderPathDict.Add(NPCNameList.overseerGaspar, partyMembersFolderPath + NPCNameList.gaspar + "/");         

        #endregion

    }

}
