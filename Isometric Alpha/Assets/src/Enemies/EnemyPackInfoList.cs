using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyPackInfoList
{
	public const string kendeTheCook = "KendeTheCook";
	public const string kendeTheCookWithoutSummon = "KendeTheCookWithoutSummon";
	public const string slaveWarrior = "Slave Warrior";
	public const string kitchenGuards = "GuardJavalineer";
	
	public const string chiefTabor = "ChiefTabor";
	
	public static string[] flagsToCheckForSlaveAllies = new string[]{"convincedSlavesToHelpYou","kastorStartedRevolt"};

    //used in the dialogue started upon entering the Manse kitchens
    public static EnemyPackInfo halfSlavesNoGuardFight = new EnemyPackInfo(new int[]{1,6}, new int[]{1,6}, new EnemyStats[]{loadEnemyStatsFromResources(kendeTheCookWithoutSummon),
																															loadEnemyStatsFromResources(slaveWarrior)
																														   },
																														    flagsToCheckForSlaveAllies,
																														    DropTableList.slaveMineDT1Name,
																															new KendeFightQuestScript());

    //used in the dialogue started upon entering the Manse kitchens
    public static EnemyPackInfo halfSlavesFight = new EnemyPackInfo(new int[]{1,6,2}, new int[]{1,6,2}, new EnemyStats[]{loadEnemyStatsFromResources(kendeTheCookWithoutSummon),
																														 loadEnemyStatsFromResources(slaveWarrior),
																														 loadEnemyStatsFromResources(kitchenGuards)
																														},
																														flagsToCheckForSlaveAllies,
																														DropTableList.slaveMineDT1Name,
                                                                                                                        new KendeFightQuestScript());

    //used in the dialogue started upon entering the Manse kitchens
    public static EnemyPackInfo fullSlavesNoGuardFight = new EnemyPackInfo(new int[]{1,12}, new int[]{1,10}, new EnemyStats[]{loadEnemyStatsFromResources(kendeTheCook),
																															  loadEnemyStatsFromResources(slaveWarrior)
																															 },
																															  flagsToCheckForSlaveAllies,
																															  DropTableList.slaveMineDT1Name,
                                                                                                                              new KendeFightQuestScript());

    //used in the dialogue started upon entering the Manse kitchens
    public static EnemyPackInfo fullSlavesFight = new EnemyPackInfo(new int[]{1,12,2}, new int[]{1,10,2}, new EnemyStats[]{loadEnemyStatsFromResources(kendeTheCook),
																														   loadEnemyStatsFromResources(slaveWarrior),
																														   loadEnemyStatsFromResources(kitchenGuards)
																														  },
																														   flagsToCheckForSlaveAllies,
																														   DropTableList.slaveMineDT1Name,
                                                                                                                           new KendeFightQuestScript());
	
	public static EnemyPackInfo taborFight = new EnemyPackInfo(new int[]{1}, new int[]{1}, new EnemyStats[]{loadEnemyStatsFromResources(chiefTabor)}, flagsToCheckForSlaveAllies, DropTableList.slaveMineDT1Name);
	
	
	private static EnemyStats loadEnemyStatsFromResources(string enemyStatsName)
	{
		EnemyStats loadedStats = Resources.Load<EnemyStats>(enemyStatsName); 

		if(loadedStats == null)
		{
			Debug.LogError("Couldn't find any EnemyStats object named: '" + enemyStatsName + "'");
		}

		return loadedStats;
	}

}
