using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : ScriptableObject
{

	public int[] enemyMinimum;
	public int[] enemyMaximum;

    public EnemyStats[] enemyTypes;

	public string dropTableName;
	
	public ItemListID[] guaranteedDrops;
	
	public int numberOfDrops = 1;

    public int determineEnemyCount(int index)
    {
		if(enemyMinimum[index] == enemyMaximum[index])
		{
			return enemyMinimum[index];
		} else
		{
			return Random.Range(enemyMinimum[index], enemyMaximum[index]);
		}
    }

}
