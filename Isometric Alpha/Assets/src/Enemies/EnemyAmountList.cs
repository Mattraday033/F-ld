using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyAmountList
{
    private const int oneEnemy = 1;
    private const int twoEnemies = 2;
    private const int threeEnemies = 3;

    #region NPCs
    public readonly static EnemyAmount guardVazul = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(NPCNameList.guardVazul));
    public readonly static EnemyAmount guardAndras = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(NPCNameList.guardAndras));
    public readonly static EnemyAmount imre = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(NPCNameList.imre));

    #endregion

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

}
