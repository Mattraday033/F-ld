using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DeadNameList
{
	public string[] names;
	
	public DeadNameList(string[] names)
	{
		this.names = names;
	}
}

public class NPCCombatInfo : MonoBehaviour
{
	public bool ignoreDeathFlags = false;
	
	private EnemyPackInfo[] enemyPackInfo;
	
	public DeadNameList[] deadNameList;
	
	public bool isRestNPC;
	public bool isCompanion;
	
	public NPCCombatInfo(EnemyPackInfo[] enemyPackInfo)
	{
		this.enemyPackInfo = enemyPackInfo;
	}
	
	public NPCCombatInfo(EnemyPackInfo[] enemyPackInfo, DeadNameList[] deadNameList)
	{
		this.enemyPackInfo = enemyPackInfo;
		this.deadNameList = deadNameList;
	}
	
	public bool hasDeadNames()
	{
		return deadNameList != null && deadNameList.Length > 0;
	}
	
	public void addAllDeadNames(int index)
	{
		foreach(string deadName in deadNameList[index].names)
		{
			DeathFlagManager.addName(deadName);
		}
	}

	public EnemyPackInfo getEnemyInfo(int index)
	{
		return enemyPackInfo[index];
	}
}
