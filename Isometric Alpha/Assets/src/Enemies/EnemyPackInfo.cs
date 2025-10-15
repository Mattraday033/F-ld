using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//info about a pack of enemies on the overworld, such as how many of them there are and of what type. Stored in State
public class EnemyPackInfo : MonoBehaviour, IDescribableInBlocks
{

	public int[] enemyMinimum;
	public int[] enemyMaximum;

	//[SerializeField]
	private string[] flagsToCheckForAllies;

	public string tutorialSequenceKey;

	public EnemyStats[] enemyTypes;

	public string dropTableName;

	public string killFlagKey;

	public string dialogueUponSceneLoadKey;

	//[SerializeField]
	public ItemListID[] guaranteedDrops;

	public int numberOfDrops = 1;
	public int xpDrop = -1;

	public bool isBossMonster = false;

	public string questName;
	public int questStep;

	public QuestStepActivationScript script;

	public EnemyPackInfo(int[] enemyMinimum, int[] enemyMaximum, EnemyStats[] enemyTypes, string dropTableName)
	{
		this.enemyMinimum = enemyMinimum;
		this.enemyMaximum = enemyMaximum;

		this.enemyTypes = enemyTypes;

		this.dropTableName = dropTableName;
	}

	public EnemyPackInfo(int[] enemyMinimum, int[] enemyMaximum, EnemyStats[] enemyTypes, string[] flagsToCheckForAllies, string dropTableName)
	{
		this.enemyMinimum = enemyMinimum;
		this.enemyMaximum = enemyMaximum;

		this.enemyTypes = enemyTypes;

		this.flagsToCheckForAllies = flagsToCheckForAllies;

		this.dropTableName = dropTableName;
	}

	public EnemyPackInfo(int[] enemyMinimum, int[] enemyMaximum, EnemyStats[] enemyTypes, string[] flagsToCheckForAllies, string dropTableName, QuestStepActivationScript script)
	{
		this.enemyMinimum = enemyMinimum;
		this.enemyMaximum = enemyMaximum;

		this.enemyTypes = enemyTypes;

		this.flagsToCheckForAllies = flagsToCheckForAllies;

		this.dropTableName = dropTableName;

		this.script = script;
	}

	public void markBossAsKilled()
	{
		if (isBossMonster)
		{
			Flags.setFlag(killFlagKey, true);
		}
	}

	public int determineEnemyCount(int index)
	{
		if (enemyMinimum[index] == enemyMaximum[index])
		{
			return enemyMinimum[index];
		}
		else
		{
			return UnityEngine.Random.Range(enemyMinimum[index], enemyMaximum[index]);
		}
	}

	public bool hasSummonsToSpawn()
	{

		if (flagsToCheckForAllies == null || flagsToCheckForAllies is null || flagsToCheckForAllies.Length == 0)
		{
			return false;
		}

		foreach (string flag in flagsToCheckForAllies)
		{
			if (flag == null || flag is null || flag.Length == 0)
			{
				continue;
			}
			else
			{
				if (Flags.getFlag(flag))
				{
					return true;
				}
			}
		}

		return false;
	}

	public string getAllyGroupingKey()
	{
		for (int keyIndex = 0; keyIndex < flagsToCheckForAllies.Length; keyIndex++)
		{
			if (Flags.getFlag(flagsToCheckForAllies[keyIndex]))
			{
				return flagsToCheckForAllies[keyIndex];
			}
		}

		throw new IOException("No key to use");
	}


	//IDescribableInBlocks methods
	public string getName()
	{
		return "";
	}

	public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> blocks = new List<DescriptionPanelBuildingBlock>();

		for (int enemyIndex = 0; enemyIndex < enemyTypes.Length &&
								  enemyIndex < enemyMinimum.Length &&
								  enemyIndex < enemyMaximum.Length;
			enemyIndex++)
		{
			string enemyNumber = "";

			if (enemyMinimum[enemyIndex] == enemyMaximum[enemyIndex])
			{
				enemyNumber = enemyMaximum[enemyIndex].ToString();
			}
			else
			{
				enemyNumber = enemyMinimum[enemyIndex] + " - " + enemyMaximum[enemyIndex];
			}

			blocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, enemyNumber + "   " + enemyTypes[enemyIndex].getName()));
		}

		return blocks;
	}
}
