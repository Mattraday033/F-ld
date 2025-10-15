using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//One day may contain all EnemyStats, right now just loads EnemyStats from Resources
public static class EnemyStatsList
{

	private const string armoredBat = "BatArmoredSecondRound";
	private const string chargedBat = "BatCharged";
	private const string spawnerBat = "BatSpawner";
	private const string masterBat = "BatMaster";
	private const string explosiveBat = "BatMinionExplosion";

	private const string wormMinionAcid = "WormMinionAcid";
	
	private const string slaveBlocker = "Slave Conscript";
	private const string slaveWarrior = "Slave Warrior";
	
	private const string smallStoneMaterials = "StoneSaintBuildingMaterialsSmall";

	public static EnemyStats[][] pupSpawnCombos =  {new EnemyStats[] {Resources.Load<EnemyStats>(explosiveBat), Resources.Load<EnemyStats>(chargedBat)},
													new EnemyStats[] {Resources.Load<EnemyStats>(armoredBat), Resources.Load<EnemyStats>(masterBat)},
													new EnemyStats[] {Resources.Load<EnemyStats>(spawnerBat), Resources.Load<EnemyStats>(spawnerBat)},
													new EnemyStats[] {Resources.Load<EnemyStats>(explosiveBat), Resources.Load<EnemyStats>(explosiveBat)},
													new EnemyStats[] {Resources.Load<EnemyStats>(chargedBat), Resources.Load<EnemyStats>(masterBat)}};
	
	public static EnemyStats[] wormSplitSpawnCombo = new EnemyStats[] {Resources.Load<EnemyStats>(wormMinionAcid), Resources.Load<EnemyStats>(wormMinionAcid)};

	public static EnemyStats[] wormSplitBossSpawnCombo = new EnemyStats[] {Resources.Load<EnemyStats>(wormMinionAcid), Resources.Load<EnemyStats>(wormMinionAcid), 
																		   Resources.Load<EnemyStats>(wormMinionAcid), Resources.Load<EnemyStats>(wormMinionAcid)};

	public static EnemyStats[] slaveBlockerCombo = new EnemyStats[] {Resources.Load<EnemyStats>(slaveBlocker), Resources.Load<EnemyStats>(slaveBlocker)};
	public static EnemyStats[] slaveWarriorCombo = new EnemyStats[] {Resources.Load<EnemyStats>(slaveWarrior), 
																	 Resources.Load<EnemyStats>(slaveWarrior),
																	 Resources.Load<EnemyStats>(slaveWarrior)};
	
	public static EnemyStats[] smallStonesCombo = new EnemyStats[] {Resources.Load<EnemyStats>(smallStoneMaterials), 
																	Resources.Load<EnemyStats>(smallStoneMaterials), 
																	Resources.Load<EnemyStats>(smallStoneMaterials)};

}
