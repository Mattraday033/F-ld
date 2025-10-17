using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyAmountList
{
    private const int oneEnemy = 1;
    private const int twoEnemies = 2;
    private const int threeEnemies = 3;

    public readonly static EnemyAmount oneBatSwarm = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static EnemyAmount twoBatSwarms = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static EnemyAmount threeBatSwarms = new EnemyAmount(threeEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static EnemyAmount fourBatSwarms = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));
    public readonly static EnemyAmount fiveBatSwarms = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.batSwarm));

    public readonly static EnemyAmount oneGiantBat = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.giantBat));
    public readonly static EnemyAmount twoGiantBats = new EnemyAmount(twoEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.giantBat));
    public readonly static EnemyAmount threeGiantBats = new EnemyAmount(threeEnemies, EnemyStatsList.getEnemyStats(MonsterNameList.giantBat));

    public readonly static EnemyAmount oneScreecherBat = new EnemyAmount(oneEnemy, EnemyStatsList.getEnemyStats(MonsterNameList.screecher));


}
