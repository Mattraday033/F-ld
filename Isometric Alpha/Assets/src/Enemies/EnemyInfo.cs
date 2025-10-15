using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //info about a pack of enemies on the overworld, such as how many of them there are and of what type. Stored in State
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
