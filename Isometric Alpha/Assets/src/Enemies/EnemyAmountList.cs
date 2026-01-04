using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyAmountList
{
    private const int oneEnemy = 1;
    private const int twoEnemies = 2;
    private const int threeEnemies = 3;
    private const int fourEnemies = 4;

    #region Named NPCs
    public readonly static EnemyAmount guardVazul = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(NPCNameList.guardVazul));
    public readonly static EnemyAmount guardAndras = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(NPCNameList.guardAndras));
    public readonly static EnemyAmount imre = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(NPCNameList.imre));

    public readonly static EnemyAmount barricade = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(NPCNameList.barricade));

    public readonly static EnemyAmount guardReka = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(NPCNameList.guardReka));
    public readonly static EnemyAmount guardPazman = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(NPCNameList.guardPazman));
    public readonly static EnemyAmount guardVirag = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(NPCNameList.guardVirag));
    public readonly static EnemyAmount overseerGaspar = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(NPCNameList.overseerGaspar));

    #endregion

    #region Lovashi Guards
    public readonly static EnemyAmount oneAxeman = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.axeman));
    public readonly static EnemyAmount twoAxemen = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.axeman));

    public readonly static EnemyAmount oneDisciplinarian = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.disciplinarian));
    public readonly static EnemyAmount twoDisciplinarians = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.disciplinarian));

    public readonly static EnemyAmount oneExecutioner = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.executioner));
    public readonly static EnemyAmount twoExecutioners = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.executioner));

    public readonly static EnemyAmount oneJavelineer = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.javelineer));
    public readonly static EnemyAmount twoJavelineers = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.javelineer));
    public readonly static EnemyAmount threeJavelineers = new EnemyAmount(threeEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.javelineer));

    public readonly static EnemyAmount oneLancer = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.lancer));
    public readonly static EnemyAmount twoLancers = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.lancer));

    public readonly static EnemyAmount oneLieutenant = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.lieutenant));
    public readonly static EnemyAmount twoLieutenants = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.lieutenant));

    public readonly static EnemyAmount oneLineBreaker = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.lineBreaker));
    public readonly static EnemyAmount twoLineBreakers = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.lineBreaker));


    public readonly static EnemyAmount oneSignaleer = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.signaleer));
    public readonly static EnemyAmount twoSignaleers = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.signaleer));

    public readonly static EnemyAmount oneSpearman = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.spearman));
    public readonly static EnemyAmount twoSpearmen = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.spearman));

    #endregion

    #region Bats
    public readonly static EnemyAmount oneBatSwarm = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static EnemyAmount twoBatSwarms = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static EnemyAmount threeBatSwarms = new EnemyAmount(threeEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static EnemyAmount fourBatSwarms = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static EnemyAmount fiveBatSwarms = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));

    public readonly static EnemyAmount oneGiantBat = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.giantBat));
    public readonly static EnemyAmount twoGiantBats = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.giantBat));
    public readonly static EnemyAmount threeGiantBats = new EnemyAmount(threeEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.giantBat));

    public readonly static EnemyAmount oneScreecherBat = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.screecher));

    public readonly static EnemyAmount oneArmoredBat = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.armoredBat));
    public readonly static EnemyAmount twoArmoredBats = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.armoredBat));

    public readonly static EnemyAmount oneDenMother = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.denMother));
    public readonly static EnemyAmount twoDenMothers = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.denMother));
    public readonly static EnemyAmount threeDenMothers = new EnemyAmount(threeEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.denMother));

    public readonly static EnemyAmount caveMatron = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.caveMatron));

    #endregion
}
