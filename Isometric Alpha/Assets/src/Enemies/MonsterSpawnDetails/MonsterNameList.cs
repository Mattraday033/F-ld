using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterNameList
{
    public const string batsPackName = "Bats";
    public const string denMother = "Den Mother";
    public const string giantBat = "Giant Bat";
    public const string batSwarm = "Bat Swarm";
    public const string armoredBat = "Armored Bat";
    public const string armoredBatShielded = "Armored Bat (Shielded)";
    public const string screecher = "Screecher";
    public const string caveMatron = "Cave Matron";

    public const string lovashiPackName = "Lovashi";
    public const string axeman = "Axeman";
    public const string disciplinarian = "Disciplinarian";
    public const string executioner = "Executioner";
    public const string javelineer = "Javelineer";
    public const string lancer = "Lancer";
    public const string lieutenant = "Lieutenant";
    public const string linebreaker = "Linebreaker";
    public const string overseer = "Overseer";
    public const string signaleer = "Signaleer";
    public const string spearman = "Spearman";

    public const string slavePackName = "Slaves";
    public const string noBrandLoyalist = "No-Brand Loyalist";
    public const string noBrandRioter = "No-Brand Rioter";

    public const string brandedConscript = "Branded Conscript";
    public const string brandedRioter = "Branded Rioter";
    
    public const string pickMarker = " (Pick)";
    public const string shivMarker = " (Shiv)";
    public const string shovelMarker = " (Shovel)";

    public const string horseCharger = "Horse Charger";
    public const string horseStomper = "Horse Stomper";

    public const string movableObject = "Movable Object";

    public static string getPackName(string enemyType)
    {
        if(enemyType.Contains(NPCNameList.guard) ||
            enemyType.Contains(NPCNameList.overseer) ||
            enemyType.Contains(NPCNameList.chief)  )
        {
                return lovashiPackName;
        }

        switch(enemyType)
        {
            case NPCNameList.imre:
                return slavePackName;
            case axeman:            
            case disciplinarian:
            case executioner:
            case javelineer:
            case lancer:
            case lieutenant:
            case linebreaker:
            case signaleer:
            case spearman:
                return lovashiPackName;
            default:
                return batsPackName;
        }
    }

    public static bool packNameNeverAddsHostility(EnemyPackInfo packInfo)
    {
        return packNameNeverAddsHostility(packInfo.getPackName());
    }

    public static bool packNameNeverAddsHostility(string packName)
    {
        switch(packName)
        {
            case batsPackName:
            case movableObject:
                return true;
            default:
                return false;
        }
    }

}
