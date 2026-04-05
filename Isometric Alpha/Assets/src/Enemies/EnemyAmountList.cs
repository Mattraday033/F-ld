using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyAmountList
{
    #region Named NPCs
    public readonly static CreatureAmount guardVazul = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.guardVazul));
    public readonly static CreatureAmount guardAndras = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.guardAndras));
    public readonly static CreatureAmount imre = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.imre));

    public readonly static CreatureAmount muzsa = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.guardMuzsa));

    public readonly static CreatureAmount barricade = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.barricade));

    public readonly static CreatureAmount guardReka = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.guardReka));
    public readonly static CreatureAmount guardPazman = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.guardPazman));
    public readonly static CreatureAmount guardVirag = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.guardVirag));
    public readonly static CreatureAmount overseerGaspar = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.overseerGaspar));

    public readonly static CreatureAmount director = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.director));
    public readonly static CreatureAmount kende = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.kende));

    #endregion

    #region Lovashi Guards
    public readonly static CreatureAmount oneAxeman = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.axeman));
    public readonly static CreatureAmount twoAxemen = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.axeman));
    public readonly static CreatureAmount threeAxemen = new CreatureAmount(Constants.threeCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.axeman));

    public readonly static CreatureAmount oneDisciplinarian = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.disciplinarian));
    public readonly static CreatureAmount twoDisciplinarians = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.disciplinarian));

    public readonly static CreatureAmount oneExecutioner = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.executioner));
    public readonly static CreatureAmount twoExecutioners = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.executioner));

    public readonly static CreatureAmount oneJavelineer = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.javelineer));
    public readonly static CreatureAmount twoJavelineers = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.javelineer));
    public readonly static CreatureAmount threeJavelineers = new CreatureAmount(Constants.threeCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.javelineer));
    public readonly static CreatureAmount fourJavelineers = new CreatureAmount(Constants.fourCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.javelineer));
    public readonly static CreatureAmount fiveJavelineers = new CreatureAmount(Constants.fiveCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.javelineer));
    public readonly static CreatureAmount tenJavelineers = new CreatureAmount(Constants.tenCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.javelineer));

    public readonly static CreatureAmount oneLancer = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.lancer));
    public readonly static CreatureAmount twoLancers = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.lancer));

    public readonly static CreatureAmount oneLieutenant = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.lieutenant));
    public readonly static CreatureAmount twoLieutenants = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.lieutenant));

    public readonly static CreatureAmount oneLinebreaker = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.linebreaker));
    public readonly static CreatureAmount twoLinebreakers = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.linebreaker));

    public readonly static CreatureAmount oneOverseer = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.overseer));

    public readonly static CreatureAmount oneSignaleer = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.signaleer));
    public readonly static CreatureAmount twoSignaleers = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.signaleer));

    public readonly static CreatureAmount oneSpearman = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.spearman));
    public readonly static CreatureAmount twoSpearmen = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.spearman));

    #endregion

    #region NonBranded Slaves
    public readonly static CreatureAmount beam = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.beam));

    public readonly static CreatureAmount fourNonBrandedLoyalists = new CreatureAmount(Constants.fourCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.noBrandLoyalist));
    public readonly static CreatureAmount eightNonBrandedLoyalists = new CreatureAmount(Constants.eightCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.noBrandLoyalist));
    public readonly static CreatureAmount tenNonBrandedLoyalists = new CreatureAmount(Constants.tenCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.noBrandLoyalist));
    #endregion

    #region Branded Slaves
    public readonly static CreatureAmount eightBrandedConscripts = new CreatureAmount(Constants.eightCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.brandedConscript));

    #endregion

    #region Bats
    public readonly static CreatureAmount oneBatSwarm = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static CreatureAmount twoBatSwarms = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static CreatureAmount threeBatSwarms = new CreatureAmount(Constants.threeCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static CreatureAmount fourBatSwarms = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static CreatureAmount fiveBatSwarms = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));

    public readonly static CreatureAmount oneGiantBat = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.giantBat));
    public readonly static CreatureAmount twoGiantBats = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.giantBat));
    public readonly static CreatureAmount threeGiantBats = new CreatureAmount(Constants.threeCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.giantBat));

    public readonly static CreatureAmount oneScreecherBat = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.screecher));

    public readonly static CreatureAmount oneArmoredBat = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.armoredBat));
    public readonly static CreatureAmount twoArmoredBats = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.armoredBat));

    public readonly static CreatureAmount oneArmoredBatShielded = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.armoredBatShielded));

    public readonly static CreatureAmount oneDenMother = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.denMother));
    public readonly static CreatureAmount twoDenMothers = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.denMother));
    public readonly static CreatureAmount threeDenMothers = new CreatureAmount(Constants.threeCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.denMother));

    public readonly static CreatureAmount caveMatron = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.caveMatron));

    #endregion

    #region Worms

    public readonly static CreatureAmount oneArmoredWorm = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.armoredWorm));
    public readonly static CreatureAmount twoArmoredWorms = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.armoredWorm));
    public readonly static CreatureAmount threeArmoredWorms = new CreatureAmount(Constants.threeCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.armoredWorm));

    public readonly static CreatureAmount oneBroodling = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.broodling));
    public readonly static CreatureAmount twoBroodlings = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.broodling));

    public readonly static CreatureAmount oneDireWorm = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.direWorm));
    public readonly static CreatureAmount twoDireWorms = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.direWorm));

    public readonly static CreatureAmount oneDireGuardianWorm = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.direGuardianWorm));

    public readonly static CreatureAmount oneGuardianWorm = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.guardianWorm));
    public readonly static CreatureAmount twoGuardianWorms = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.guardianWorm));

    public readonly static CreatureAmount oneHiveHerald = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.hiveHerald));
    public readonly static CreatureAmount twoHiveHeralds = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.hiveHerald));
    public readonly static CreatureAmount threeHiveHeralds = new CreatureAmount(Constants.threeCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.hiveHerald));

    public readonly static CreatureAmount oneMartyrWorm = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.martyrWorm));
    public readonly static CreatureAmount twoMartyrWorms = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.martyrWorm));

    public readonly static CreatureAmount oneToxicWorm = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.toxicWorm));
    public readonly static CreatureAmount twoToxicWorms = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.toxicWorm));
    public readonly static CreatureAmount threeToxicWorms = new CreatureAmount(Constants.threeCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.toxicWorm));

    public readonly static CreatureAmount oneWorm = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.worm));
    public readonly static CreatureAmount twoWorms = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.worm));
    public readonly static CreatureAmount threeWorms = new CreatureAmount(Constants.threeCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.worm));


    public readonly static CreatureAmount hiveHeraldNest = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.hiveHeraldNest));
    public readonly static CreatureAmount martyrWormNest = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.martyrWormNest));
    public readonly static CreatureAmount toxicWormNest = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.toxicWormNest));
    public readonly static CreatureAmount wormNest = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.wormNest));

    #endregion

    #region Horses

    public readonly static CreatureAmount oneHorseCharger = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.horseCharger));
    public readonly static CreatureAmount twoHorseChargers = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.horseCharger));

    public readonly static CreatureAmount oneHorseStomper = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(MonsterNameList.horseStomper));
    public readonly static CreatureAmount twoHorseStompers = new CreatureAmount(Constants.twoCreatures, EnemyStatsList.getEnemyStats(MonsterNameList.horseStomper));

    public readonly static CreatureAmount csalan = new CreatureAmount(Constants.oneCreature, EnemyStatsList.getEnemyStats(NPCNameList.csalan));

    #endregion
}
