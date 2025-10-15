using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int finalEnemyCount;
	public Canvas combatantInfoCanvas;
	public CombatStateManager combatStateManager;
	public EnemyCombatActionManager enemyCombatActionManager;
	
	public static ArrayList frontLine;
	public static ArrayList specificSpawns;
	public static ArrayList randomSpawns;
	public static ArrayList backLine;
	
	public static Stats[][] enemyFormation = new Stats[][] {new Stats[]{null,null,null,null},
															new Stats[]{null,null,null,null},
															new Stats[]{null,null,null,null},
															new Stats[]{null,null,null,null}};
	
	public static EnemySpawner instance;
	
	public static EnemySpawner getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("Instance of EnemySpawner already exists");
		}
		
		instance = this;
	}
	
    public void spawn()
    {
		int foundCreatureCollisionCount = 0;
		
		CombatGrid.cleanCombatGrid();
		createInitialSpawnQueue(); // populates frontLine/backLine/randomSpawns
		
		foreach(EnemyStats enemy in specificSpawns)
		{
			spawnLargeEnemy(enemy);
		}	
		
		setupEnemyFrontLine();
		
		setupEnemyBackLine();
		
		spawnFormation();
		
		foreach(EnemyStats enemy in randomSpawns)
		{
			while(CombatGrid.howManyEmptyEnemySpaces() > 0 && foundCreatureCollisionCount < Int32.MaxValue/10000)
			{
				int row = UnityEngine.Random.Range(0, 4);
				int col = UnityEngine.Random.Range(0, 4);
				
				if(CombatGrid.getCombatantAtCoords(row,col) == null)
				{
					spawnEnemy(enemy, new GridCoords(row,col));
					
					break;
				} else
				{
					foundCreatureCollisionCount++;
				}
			} 
			
			foundCreatureCollisionCount = 0;
			
			if(CombatGrid.howManyEmptyEnemySpaces() == 0)
			{
				break;
			}
		}	
		
	} 

	private void createInitialSpawnQueue()
	{
		ArrayList spawnQueue = new ArrayList();
		
		for(int enemyTypeIndex = 0; enemyTypeIndex < State.enemyPackInfo.enemyTypes.Length; enemyTypeIndex++)
		{	
			int enemyCount = State.enemyPackInfo.determineEnemyCount(enemyTypeIndex);
			
			for(int currentEnemyNumber = 1; currentEnemyNumber <= enemyCount; currentEnemyNumber++)
			{
				EnemyStats currentEnemyStats = (EnemyStats) State.enemyPackInfo.enemyTypes[enemyTypeIndex].Clone();
				currentEnemyStats.spawningCombatAction();//added to be able to tell traits when sorting spawn queue
												   //should be both here and in spawnEnemy()
				
				spawnQueue.Add(currentEnemyStats);
			}
		}
			
		sortSpawnQueue(spawnQueue);
	}
	
	private void sortSpawnQueue(ArrayList unsortedSpawnQueue)
	{
		frontLine = new ArrayList();
		randomSpawns = new ArrayList();
		specificSpawns = new ArrayList();
		backLine = new ArrayList();
		
		foreach(EnemyStats enemy in unsortedSpawnQueue)
		{	
			if(Helpers.hasQuality<Trait>(enemy.traits, t => t.stackInFront()))
			{
				frontLine.Add(enemy);
			} else if(Helpers.hasQuality<Trait>(enemy.traits, t => t.stackInBack()))
			{
				backLine.Add(enemy);
			} else if(enemy.spawnDetails.hasSpawnDetails)
			{
				specificSpawns.Add(enemy);
			} else
			{
				randomSpawns.Add(enemy);
			}
		}
	}

	public void spawnEnemyRandomLocation(EnemyStats enemyTypeToSpawn)
	{
		if(CombatGrid.howManyEmptyEnemySpaces() > 0)
		{
			GridCoords enemySpawnPoint = CombatGrid.findRandomOpenSpace(CombatGrid.getAllEmptySpacesInEnemyZone());
			
			if(!enemySpawnPoint.Equals(GridCoords.getDefaultCoords()))
			{
				spawnEnemy(enemyTypeToSpawn, enemySpawnPoint);
			}
		} 
	}

	public void setupEnemyFrontLine()
	{
		int enemyCountAtStart = frontLine.Count;
		
		for(int pullCount = 0; pullCount < enemyCountAtStart; pullCount++)
		{
			int enemyIndex = UnityEngine.Random.Range(0,frontLine.Count);
			Stats enemy = (Stats) frontLine[enemyIndex];
			frontLine.RemoveAt(enemyIndex);
			
			GridCoords spawnPoint = findOpenFrontlineSpace();
			
			enemyFormation[spawnPoint.row][spawnPoint.col] = enemy;
		}
	}
	
	public void setupEnemyBackLine()
	{
		int enemyCountAtStart = backLine.Count;
		
		for(int pullCount = 0; pullCount < enemyCountAtStart; pullCount++)
		{
			int enemyIndex = UnityEngine.Random.Range(0,backLine.Count);
			Stats enemy = (Stats) backLine[enemyIndex];
			backLine.RemoveAt(enemyIndex);
			
			GridCoords spawnPoint = findOpenBacklineSpace();
			
			enemyFormation[spawnPoint.row][spawnPoint.col] = enemy;
		}
	}
	
	private GridCoords findOpenFrontlineSpace()
	{
		for(int rowIndex = enemyFormation.Length-1; rowIndex >= 0; rowIndex--)
		{
			int colIndex = findOpenSpaceInRow(enemyFormation[rowIndex], rowIndex);
			
			if(colIndex < 0)
			{
				continue;
			}
			
			return new GridCoords(rowIndex, colIndex);
		}			
		
		return GridCoords.getDefaultCoords();
	}
	
	private GridCoords findOpenBacklineSpace()
	{
		for(int rowIndex = 0; rowIndex < enemyFormation.Length; rowIndex++)
		{
			int colIndex = findOpenSpaceInRow(enemyFormation[rowIndex], rowIndex);
			
			if(colIndex < 0)
			{
				continue;
			}
			
			return new GridCoords(rowIndex, colIndex);
		}			
		
		return GridCoords.getDefaultCoords();
	}
	
	private int findOpenSpaceInRow(Stats[] row, int rowIndex)
	{
		if(!row.Contains(null) && !CombatGrid.combatantStatsGrid[rowIndex].row.Contains(null))
		{
			Debug.Log("row does not contain null");
			return -1;
		}
		
		ArrayList acceptableCoordsInRow = new ArrayList();
		
		for(int colIndex = 0; colIndex < row.Length; colIndex++)
		{
			if(row[colIndex] == null && CombatGrid.getCombatantAtCoords(rowIndex, colIndex) == null)
			{
				acceptableCoordsInRow.Add(new GridCoords(rowIndex, colIndex));
			}
		}
		
		if(acceptableCoordsInRow.Count == 0)
		{
			return -1;
		} else
		{
			GridCoords chosenCoords = (GridCoords) acceptableCoordsInRow[UnityEngine.Random.Range(0,acceptableCoordsInRow.Count)];
			return chosenCoords.col;
		}
	}
	
	//wipes enemyFormation after running
	public void spawnFormation()
	{
		for(int rowIndex = 0; rowIndex < enemyFormation.Length; rowIndex++)
		{
			for(int colIndex = 0; colIndex < enemyFormation[rowIndex].Length; colIndex++)
			{
				if(enemyFormation[rowIndex][colIndex] != null)
				{
					spawnEnemy((EnemyStats) enemyFormation[rowIndex][colIndex], rowIndex, colIndex);
				}
			}
		}
		
		resetEnemyFormation();
	}
	
	private void resetEnemyFormation()
	{
		enemyFormation = enemyFormation = new Stats[][] {new Stats[]{null,null,null,null},
														 new Stats[]{null,null,null,null},
														 new Stats[]{null,null,null,null},
														 new Stats[]{null,null,null,null}};
	}
	
	public void spawnEnemy(EnemyStats enemyTypeToSpawn, int row, int col)
	{
		spawnEnemy(enemyTypeToSpawn, new GridCoords(row,col));
	}
	
	public void spawnEnemy(EnemyStats enemyTypeToSpawn, GridCoords enemySpawnPoint)
	{
		int row = enemySpawnPoint.row;
		int col = enemySpawnPoint.col;
		
		EnemyStats cloneOfEnemyType = (EnemyStats) enemyTypeToSpawn.Clone();
		
		cloneOfEnemyType.spawningCombatAction();
		
		if(CombatGrid.howManyEmptyEnemySpaces() > 0)
		{
			cloneOfEnemyType.position = new GridCoords(row,col);
			
			CombatGrid.combatantStatsGrid[row].setCol(col, cloneOfEnemyType);
			
			cloneOfEnemyType.instantiateCombatSprite();

			cloneOfEnemyType.combatSprite.transform.position = CombatGrid.fullCombatGrid[row][col] + cloneOfEnemyType.adjustment;
			
			((EnemyStats)CombatGrid.getCombatantAtCoords(row, col)).setUpHealthBar
			(
				Instantiate(CombatGrid.getCombatantAtCoords(row,col).healthBar,
							CombatGrid.fullCombatGrid[row][col] + Stats.healthBarAdjustment,
							Quaternion.identity, 
							combatantInfoCanvas.gameObject.transform
							)
			);
			
			if(CombatStateManager.whoseTurn == WhoseTurn.Start)
			{
				cloneOfEnemyType.instateEnvironmentalCombatAction();
			}
		}
	}
	
	public void spawnLargeEnemy(EnemyStats enemyTypeToSpawn)
	{
		EnemyStats cloneOfEnemyType = (EnemyStats) enemyTypeToSpawn.Clone();
		SpawnDetails spawnDetails = cloneOfEnemyType.spawnDetails;
		
		if(spawnDetails.dontSpawnWhenSurprised && CombatStateManager.whoIsSurprised == SurpriseState.EnemySurprised)
		{
			return;
		}
		
		int spriteRow = spawnDetails.spritePosition.row;
		int spriteCol = spawnDetails.spritePosition.col;
		
		cloneOfEnemyType.spawningCombatAction();
		
		if(CombatGrid.howManyEmptyEnemySpaces() >= spawnDetails.allSpawnPositions.Length)
		{
			cloneOfEnemyType.position = spawnDetails.baseStatsPosition;
			
			CombatGrid.combatantStatsGrid[cloneOfEnemyType.position.row].setCol(cloneOfEnemyType.position.col, cloneOfEnemyType);
			
			cloneOfEnemyType.combatSprite = Instantiate(cloneOfEnemyType.combatSprite, CombatGrid.fullCombatGrid[spriteRow][spriteCol] + cloneOfEnemyType.adjustment, Quaternion.identity);
			
			((EnemyStats) CombatGrid.getCombatantAtCoords(cloneOfEnemyType.position.row,cloneOfEnemyType.position.col)).setUpHealthBar
			(
				Instantiate(cloneOfEnemyType.healthBar,
							findAverageOfSpawnPositions(spawnDetails.allSpawnPositions) + Stats.healthBarAdjustment,
							Quaternion.identity,
							combatantInfoCanvas.gameObject.transform
							)
			);
			
			foreach(GridCoords coords in spawnDetails.allSpawnPositions)
			{
				CombatGrid.combatantStatsGrid[coords.row].setCol(coords.col, cloneOfEnemyType);
			}
		}
	}
	
	private Vector3 findAverageOfSpawnPositions(GridCoords[] allSpawnPositions)
	{
		Vector3 sumOfAllSpawnPositions = Vector3.zero;
		
		foreach(GridCoords coords in allSpawnPositions)
		{
			sumOfAllSpawnPositions += CombatGrid.fullCombatGrid[coords.row][coords.col];
		}
		
		Vector3 averageOfAllSpawnPositions = new Vector3(sumOfAllSpawnPositions.x / (float) allSpawnPositions.Length,
														 sumOfAllSpawnPositions.y / (float) allSpawnPositions.Length,
														 sumOfAllSpawnPositions.z / (float) allSpawnPositions.Length);
		
		return averageOfAllSpawnPositions;
	}
}
