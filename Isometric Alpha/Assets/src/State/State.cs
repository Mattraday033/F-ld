using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum PlayerDirectionFromEnemy { NorthEast, NorthWest, SouthWest, SouthEast }

public static class State
{
	public static bool debugStopMonsterSpawning = false;
	public static bool debugDiscoverAllLocations = false;
	public static bool debugRetreatAutoSucceed = false;

	public static MonsterPackList currentMonsterPackList;
	public static Dictionary<string, bool> monsterDefeatKeys = new Dictionary<string, bool>();

	public static OOCUIManager oocUIManager;

	public static bool terrainHidden;

	public static Dictionary<string, Item> inventory = new Dictionary<string, Item>();
	public static Dictionary<string, Item> junkPocket = new Dictionary<string, Item>();
	// public static EquippableItem[] equippedItems = new EquippableItem[6];
	//public static CombatAction[] CombatActionArray = new CombatAction[CombatActionArray.maxPlayerCombatActions];

	public static string[] lessonsLearned = new string[0];
	public static Formation formation = new Formation();
	public static Dictionary<string, Quest> questDictionary = new Dictionary<string, Quest>();

	public static EnemyPackInfo enemyPackInfo;
	public static CharacterFacing playerFacing;
	public static CharacterFacing enemyFacing;
	public static Vector3 playerPosition;
	public static Vector3 enemyPosition;
	public static bool onLeftFoot;

    public static TransitionInfo currentSourceTransitionInfo;
	public static Dictionary<string, ArrayList> allKnownMapData = new Dictionary<string, ArrayList>();

	public static bool enteredCombatFromDialogue = false;
	public static string dialogueUponSceneLoadKey;

	public static bool hasLoadedDialogueKey()
	{
		return dialogueUponSceneLoadKey != null && dialogueUponSceneLoadKey.Length > 0;
	}
}