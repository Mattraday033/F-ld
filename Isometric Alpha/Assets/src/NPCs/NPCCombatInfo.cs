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
	//[SerializeField]
	private bool fightable;
	//[SerializeField]
	public bool ignoreDeathFlags = false;
	
	//[SerializeField]
	private EnemyPackInfo[] enemyPackInfo;
	
	//[SerializeField]
	public DeadNameList[] deadNameList;
	
	//[SerializeField]
	public bool isRestNPC;
	//[SerializeField]
	public bool isCompanion;
	
	public int hostility;
	
	public NPCCombatInfo(EnemyPackInfo[] enemyPackInfo)
	{
		this.enemyPackInfo = enemyPackInfo;
	}
	
	public NPCCombatInfo(EnemyPackInfo[] enemyPackInfo, DeadNameList[] deadNameList)
	{
		this.enemyPackInfo = enemyPackInfo;
		this.deadNameList = deadNameList;
	}
	
	public bool canBeFought()
	{
		return fightable;
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
