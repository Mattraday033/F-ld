using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum PlayerDirectionFromEnemy { NorthEast, NorthWest, SouthWest, SouthEast }

public static class State
{
	public static bool debugStopMonsterSpawning = false;
	public static bool debugDiscoverAllLocations = false;
	public static bool allLocationsFastTravelAvailable = false;
	public static bool debugRetreatAutoSucceed = false;
	public static bool enableGridDebugger = false;

    public static string playerPortraitName = NPCNameList.thatch;
    public static string _PlayerSpriteName = NPCNameList.thatch;

    public static string playerSpriteName
    {
        get
        {
            return _PlayerSpriteName;
        }
        set
        {
            _PlayerSpriteName = value;
        }
    }

	public static OOCUIManager oocUIManager;

	public static bool terrainHidden;

	public static Dictionary<string, Item> inventory = new Dictionary<string, Item>();
	public static Dictionary<string, Item> junkPocket = new Dictionary<string, Item>();
	// public static EquippableItem[] equippedItems = new EquippableItem[6];
	//public static CombatAction[] CombatActionArray = new CombatAction[CombatActionArray.maxPlayerCombatActions];

	public static string[] lessonsLearned = new string[0];
	public static Formation formation = new Formation();

    public static SkillType currentSkillType;

	public static EnemyPackInfo enemyPackInfo;
    public static AllyPackInfo allyPackInfo;
	public static CharacterFacing playerFacing;
	public static bool onLeftFoot;

	public static Dictionary<string, List<string>> allKnownMapData = new Dictionary<string, List<string>>();

	public static bool enteredCombatFromDialogue = false;
	public static string dialogueUponSceneLoadKey;

	public static bool hasLoadedDialogueKey()
	{
		return dialogueUponSceneLoadKey != null && dialogueUponSceneLoadKey.Length > 0;
	}
}