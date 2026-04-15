using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterNameList
{
    #region Bats
    public const string batsPackName = "Bats";
    public const string denMother = "Den Mother";
    public const string giantBat = "Giant Bat";
    public const string batSwarm = "Bat Swarm";
    public const string armoredBat = "Armored Bat";
    public const string armoredBatShielded = "Armored Bat (Shielded)";
    public const string screecher = "Screecher";
    public const string caveMatron = "Cave Matron";
    #endregion

    #region Worms
    public const string wormsPackName = "Worms";
    public const string armoredWorm = "Armored Worm";
    public const string broodling = "Broodling";
    public const string direWorm = "Dire Worm";
    public const string direGuardianWorm = "Dire Guardian Worm";
    public const string guardianWorm = "Guardian Worm";
    public const string hiveHerald = "Hive Herald";
    public const string hiveHeraldNest = "Hive Herald Nest";
    public const string martyrWorm = "Martyr Worm";
    public const string martyrWormNest = "Martyr Worm Nest";
    public const string toxicWorm = "Toxic Worm";
    public const string toxicWormNest = "Toxic Worm Nest";
    public const string worm = "Worm";
    public const string wormNest = "Worm Nest";
    #endregion

    #region Lovashi
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
    #endregion

    #region Slaves
    public const string slavePackName = "Slaves";
    public const string noBrandLoyalist = "No-Brand Loyalist";
    public const string noBrandRioter = "No-Brand Rioter";

    public const string brandedConscript = "Branded Conscript";
    public const string angryBranded = "Angry Branded";
    public const string brandedRioter = "Branded Rioter";
    
    public const string pickMarker = " (Pick)";
    public const string shivMarker = " (Shiv)";
    public const string shovelMarker = " (Shovel)";
    #endregion

    #region Horses
    public const string horseCharger = "Horse Charger";
    public const string horseStomper = "Horse Stomper";
    public const string horsesPackName = "Horses";
    #endregion

    #region Saints
    public const string stoneSaint = "Stone Saint";
    public const string lesserStoneSaint = "Lesser Stone Saint";
    public const string smallRock = "Small Rock";
    public const string largeRock = "Large Rock";
    #endregion

    public const string movableObject = "Movable Object";

    public static string getPackName(string enemyType)
    {
        if(enemyType.Equals(NPCNameList.guard) ||
            enemyType.Contains(NPCNameList.guard + " ") ||
            enemyType.Contains(NPCNameList.overseer) ||
            enemyType.Contains(NPCNameList.chief)  )
        {
                return lovashiPackName;
        }

        switch(enemyType)
        {
            // Bats
            case denMother:
            case giantBat:
            case batSwarm:
            case armoredBat:
            case armoredBatShielded:
            case screecher:
            case caveMatron:
                return batsPackName;

            // Worms
            case armoredWorm:
            case broodling:
            case direWorm:
            case direGuardianWorm:
            case guardianWorm:
            case hiveHerald:
            case hiveHeraldNest:
            case martyrWorm:
            case martyrWormNest:
            case toxicWorm:
            case toxicWormNest:
            case worm:
            case wormNest:
                return wormsPackName;

            // Lovashi
            case axeman:
            case disciplinarian:
            case executioner:
            case javelineer:
            case lancer:
            case lieutenant:
            case linebreaker:
            case overseer:
            case signaleer:
            case spearman:
            case NPCNameList.director:
            case NPCNameList.kende:
                return lovashiPackName;

            // Slaves
            case NPCNameList.imre:
            case NPCNameList.beam:
            case noBrandLoyalist:
            case noBrandRioter:
            case brandedConscript:
            case brandedRioter:
                return slavePackName;

            // Horses
            case NPCNameList.csalan:
            case horseCharger:
            case horseStomper:
                return horsesPackName;

            case stoneSaint:
            case smallRock:
            case largeRock:
                return stoneSaint;

            // Movable Object
            case movableObject:
                return movableObject;

            default:
                return "???";
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
