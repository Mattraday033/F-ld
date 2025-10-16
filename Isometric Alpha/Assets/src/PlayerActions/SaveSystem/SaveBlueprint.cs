using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct InventoryWrapper
{
    public string key;
    public string[] inventory;
}

[System.Serializable]
public struct StatsWrapper
{
    public string key;

    public int strength;
    public int dexterity;
    public int wisdom;
    public int charisma;

    public int level;
    public int xp;
    public int currentHealth;

    public bool canJoinParty;

    public bool placed;

    public string partyMemberPlacedPosition;
    public GridCoords partyMemberFormationCoords;

    public string[] currentEquipment;
    public string[] combatActions;

    public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
    {
        if (!canJoinParty)
        {
            return new List<DescriptionPanelBuildingBlock>();
        }

        List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Name, key));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Level: " + level));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Health: " + currentHealth));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Experience: " + xp));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, ""));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Strength: " + strength));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Dexterity: " + dexterity));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Wisdom: " + wisdom));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Charisma: " + charisma));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, ""));

        return buildingBlocks;
    }

}

public interface IJSONConvertable
{
    public string convertToJson();

    //public static T extractFromJson(string json);
}

[System.Serializable]
public class SaveBlueprint : IDescribable, ISortable, IDescribableInBlocks
{
	private const string dividerCharacter = "~";

	public int saveNumber;

	public string currentLocation;
	public float[] playerPosition;

	public bool terrainHidden;

	public string overworldSpriteSortingLayer;
    public int gold;
	public int affinity;
	public int cunningsRemaining;
	public int intimidatesRemaining;

	public int playerFacing;

	public bool onLeftFoot;

	public string saveName;

	public string currentFlags;
	public string[] currentChoices = new string[ChoiceManager.choices.Count];
	public string[] currentDeathFlags = new string[DeathFlagManager.deadNames.Count];
	public string[] currentMetFlags = new string[MetFlagManager.metNames.Count];
	public string[] currentChestFlags = new string[GateAndChestManager.getKeyCount()];
	public string[] currentActivatedTrapsAndButtons = new string[TrapAndButtonStateManager.allActivatedTrapKeys.Count];

	public string[] currentInventory = new string[State.inventory.Count];
	public string[] currentJunk = new string[State.junkPocket.Count];
	public string[] currentLessons = new string[State.lessonsLearned.Length];
	public string[] currentQuestList = new string[State.questDictionary.Count];
	public string[] currentKnownMapData = new string[State.allKnownMapData.Count];
	public string[] currentAreaHostilities = new string[AreaList.allAreas.Count];

    public StatsWrapper[] partyMemberStats = new StatsWrapper[PartyManager.getNumberOfPartyMembersTotal()];

	public InventoryWrapper[] currentShopkeeperInventories;
	public InventoryWrapper[] currentBuyBackInventories;

	public string[] monsterPackList;
	public string[] currentMonsterDefeatKeys = new string[State.monsterDefeatKeys.Count];

	public static SaveBlueprint build(string saveName, int saveNumber)
	{
		SaveBlueprint saveBlueprint = new SaveBlueprint();

		saveBlueprint.overworldSpriteSortingLayer = PlayerMovement.getInstance().playerSpriteRenderer.sortingLayerName;

		saveBlueprint.onLeftFoot = State.onLeftFoot;
		saveBlueprint.saveNumber = saveNumber;

		saveBlueprint.terrainHidden = State.terrainHidden;

		saveBlueprint.cunningsRemaining = CunningManager.getCunningsRemaining();
		saveBlueprint.intimidatesRemaining = IntimidateManager.getIntimidatesRemaining();

		saveBlueprint.affinity = AffinityManager.getTotalAffinity();
        saveBlueprint.gold = Purse.getCoinsInPurse();

		saveBlueprint.playerPosition = new float[3];
		saveBlueprint.playerPosition[0] = PlayerMovement.getTransform().position.x; //player.transform.position.x;
		saveBlueprint.playerPosition[1] = PlayerMovement.getTransform().position.y;
		saveBlueprint.playerPosition[2] = PlayerMovement.getTransform().position.z;

		saveBlueprint.playerFacing = (int) State.playerFacing.getFacing();

		saveBlueprint.currentFlags = Flags.getFlagsForSave();
		saveBlueprint.currentInventory = SaveBlueprint.convertToJson<Item>(State.inventory);
		saveBlueprint.currentJunk = convertToJson<Item>(State.junkPocket);
		saveBlueprint.currentLessons = State.lessonsLearned;
		saveBlueprint.currentQuestList = convertToJson(State.questDictionary);
		saveBlueprint.currentKnownMapData = SaveBlueprint.convertAllKnownMapDataToJson();
		saveBlueprint.currentAreaHostilities = SaveBlueprint.convertAllAreaHostilitiesToJson();
		saveBlueprint.currentChoices = convertToJson<ChoiceKey>(ChoiceManager.choices);
		saveBlueprint.currentDeathFlags = Helpers.arrayListToStrings(DeathFlagManager.deadNames);
		saveBlueprint.currentMetFlags = Helpers.arrayListToStrings(MetFlagManager.metNames);
		saveBlueprint.currentChestFlags = GateAndChestManager.getSaveData();
		saveBlueprint.currentActivatedTrapsAndButtons = Helpers.arrayListToStrings(TrapAndButtonStateManager.allActivatedTrapKeys);
		saveBlueprint.currentLocation = AreaManager.locationName;
		saveBlueprint.saveName = saveName;

		// saveBlueprint.playerFormationPosition = State.formation.findLocationOfStats(State.playerStats);

		saveBlueprint.setPartyMemberDetails();

		saveBlueprint.currentMonsterDefeatKeys = convertAllMonsterDefeatKeysToJson();

		saveBlueprint.currentShopkeeperInventories = convertShopkeeperInventoriesToJson(ShopkeeperInventoryList.shopkeeperInventories);
		saveBlueprint.currentBuyBackInventories = convertShopkeeperInventoriesToJson(ShopkeeperInventoryList.buyBackInventories);

		if (State.currentMonsterPackList != null)
		{
			saveBlueprint.monsterPackList = convertToJson(Array.ConvertAll(State.currentMonsterPackList.monsterPacks, item => (IJSONConvertable)item));
		}

		return saveBlueprint;
	}

	public static SaveBlueprint build(TextAsset textAsset)
	{
		return new SaveBlueprint(textAsset.ToString());
	}

	public static SaveBlueprint build(string json)
	{
		return new SaveBlueprint(json);
	}

	public SaveBlueprint()
	{

	}

	public SaveBlueprint(string jsonString)
	{

		dynamic jsonDynamic = JsonConvert.DeserializeObject<dynamic>(jsonString);

		this.saveName = GetFromJson.getElementFromJson(SaveDefaultValues.badSaveName, nameof(saveName), jsonDynamic, SaveDefaultValues.badSaveName);
		this.saveNumber = GetFromJson.getElementFromJson(this.saveName, nameof(saveNumber), jsonDynamic, SaveDefaultValues.badSaveNumber);

		this.currentLocation = GetFromJson.getElementFromJson(this.saveName, nameof(currentLocation), jsonDynamic, SaveDefaultValues.defaultSceneName);
		this.playerPosition = GetFromJson.getElementFromJson(this.saveName, nameof(playerPosition), jsonDynamic, SaveDefaultValues.defaultPlayerPosition);
		this.terrainHidden = GetFromJson.getElementFromJson(this.saveName, nameof(terrainHidden), jsonDynamic, SaveDefaultValues.defaultBoolFalse);

		this.overworldSpriteSortingLayer = GetFromJson.getElementFromJson(this.saveName, nameof(overworldSpriteSortingLayer), jsonDynamic, SaveDefaultValues.defaultOverworldSpriteSortingLayer);

		this.gold = GetFromJson.getElementFromJson(this.saveName, nameof(gold), jsonDynamic, SaveDefaultValues.defaultStatZero);
		this.affinity = GetFromJson.getElementFromJson(this.saveName, nameof(affinity), jsonDynamic, SaveDefaultValues.defaultStatZero);

		this.cunningsRemaining = GetFromJson.getElementFromJson(this.saveName, nameof(cunningsRemaining), jsonDynamic, SaveDefaultValues.defaultStatTwo);
		this.intimidatesRemaining = GetFromJson.getElementFromJson(this.saveName, nameof(intimidatesRemaining), jsonDynamic, SaveDefaultValues.defaultStatTwo);
		this.playerFacing = (int) GetFromJson.getElementFromJson(this.saveName, nameof(playerFacing), jsonDynamic, SaveDefaultValues.defaultFacing);
		this.onLeftFoot = GetFromJson.getElementFromJson(this.saveName, nameof(onLeftFoot), jsonDynamic, SaveDefaultValues.defaultBoolFalse);


		this.currentFlags = GetFromJson.getElementFromJson(this.saveName, nameof(currentFlags), jsonDynamic, SaveDefaultValues.defaultCurrentFlags);
		this.currentChoices = GetFromJson.getElementFromJson(this.saveName, nameof(currentChoices), jsonDynamic, SaveDefaultValues.defaultEmptyStringArray);
		this.currentDeathFlags = GetFromJson.getElementFromJson(this.saveName, nameof(currentDeathFlags), jsonDynamic, SaveDefaultValues.defaultEmptyStringArray);
		this.currentMetFlags = GetFromJson.getElementFromJson(this.saveName, nameof(currentMetFlags), jsonDynamic, SaveDefaultValues.defaultEmptyStringArray);
		this.currentChestFlags = GetFromJson.getElementFromJson(this.saveName, nameof(currentChestFlags), jsonDynamic, SaveDefaultValues.defaultEmptyStringArray);

		this.currentInventory = GetFromJson.getElementFromJson(this.saveName, nameof(currentInventory), jsonDynamic, SaveDefaultValues.defaultEmptyStringArray);
		this.currentJunk = GetFromJson.getElementFromJson(this.saveName, nameof(currentJunk), jsonDynamic, SaveDefaultValues.defaultEmptyStringArray);

		this.currentLessons = GetFromJson.getElementFromJson(this.saveName, nameof(currentLessons), jsonDynamic, SaveDefaultValues.defaultEmptyStringArray);
		this.currentQuestList = GetFromJson.getElementFromJson(this.saveName, nameof(currentQuestList), jsonDynamic, SaveDefaultValues.defaultEmptyStringArray);
		this.currentKnownMapData = GetFromJson.getElementFromJson(this.saveName, nameof(currentKnownMapData), jsonDynamic, SaveDefaultValues.defaultEmptyStringArray).ToObject<string[]>();
		this.currentAreaHostilities = GetFromJson.getElementFromJson(this.saveName, nameof(currentAreaHostilities), jsonDynamic, SaveDefaultValues.defaultEmptyStringArray);

        this.partyMemberStats = GetFromJson.getElementFromJson(this.saveName, nameof(partyMemberStats), jsonDynamic, SaveDefaultValues.defaultEmptyStatsWrapperArray);

		this.currentShopkeeperInventories = GetFromJson.getElementFromJson(this.saveName, nameof(currentShopkeeperInventories), jsonDynamic, SaveDefaultValues.defaultEmptyInventoryWrapperArray);
		this.currentBuyBackInventories = GetFromJson.getElementFromJson(this.saveName, nameof(currentBuyBackInventories), jsonDynamic, SaveDefaultValues.defaultEmptyInventoryWrapperArray);


		this.monsterPackList = GetFromJson.getElementFromJson(this.saveName, nameof(monsterPackList), jsonDynamic, null);
		this.currentMonsterDefeatKeys = GetFromJson.getElementFromJson(this.saveName, nameof(currentMonsterDefeatKeys), jsonDynamic, SaveDefaultValues.defaultEmptyStringArray);
	}

	public static string[] convertToJson(IJSONConvertable[] arrayOfJSONConvertableObjects)
	{
		string[] json = new string[arrayOfJSONConvertableObjects.Length];

		for (int index = 0; index < json.Length; index++)
		{
			if (arrayOfJSONConvertableObjects[index] != null)
			{
				json[index] = arrayOfJSONConvertableObjects[index].convertToJson();
			}
			else
			{
				json[index] = null;
			}
		}

		return json;
	}

	public static string[] convertToJson(ArrayList listOfJSONConvertableObjects)
	{
		string[] json = new string[listOfJSONConvertableObjects.Count];

		for (int index = 0; index < json.Length; index++)
		{
			if (listOfJSONConvertableObjects[index] != null)
			{
				json[index] = ((IJSONConvertable)listOfJSONConvertableObjects[index]).convertToJson();
			}
			else
			{
				json[index] = null;
			}
		}

		return json;
	}

	public static string[] convertToJson<T>(Dictionary<string, T> dictOfJSONConvertableObjects)
	{
		string[] json = new string[dictOfJSONConvertableObjects.Count];

		int index = 0;
		foreach (KeyValuePair<string, T> kvp in dictOfJSONConvertableObjects)
		{
			json[index] = ((IJSONConvertable)kvp.Value).convertToJson();
			index++;
		}

		return json;
	}

	public static InventoryWrapper[] convertShopkeeperInventoriesToJson(Dictionary<string, Dictionary<string, Item>> dictOfInventories)
	{
		InventoryWrapper[] wrapperOfShopkeeperInventories = new InventoryWrapper[dictOfInventories.Count];

		int inventoryIndex = 0;
		foreach (KeyValuePair<string, Dictionary<string, Item>> kvp in dictOfInventories)
		{
			Dictionary<string, Item> inventory = kvp.Value;

			wrapperOfShopkeeperInventories[inventoryIndex].key = kvp.Key;
			wrapperOfShopkeeperInventories[inventoryIndex].inventory = convertToJson(inventory);
			inventoryIndex++;
		}

		return wrapperOfShopkeeperInventories;
	}

	public static Dictionary<string, Dictionary<string, Item>> extractShopkeeperInventoriesFromJson(InventoryWrapper[] wrapperOfInventories)
	{
		Dictionary<string, Dictionary<string, Item>> extractedShopkeeperInventories = new Dictionary<string, Dictionary<string, Item>>();

		foreach (InventoryWrapper wrapper in wrapperOfInventories)
		{
			extractedShopkeeperInventories[wrapper.key] = extractInventoryItemsFromJson(wrapper.inventory);
		}

		return extractedShopkeeperInventories;
	}

	public static EquippableItem[] extractEquippedItemsFromJson(string[] equipmentJson)
	{
		EquippableItem[] equipment = new EquippableItem[EquippedItems.totalEquipmentSlots];

		int i = 0;
		foreach (string json in equipmentJson)
		{
			equipment[i] = (EquippableItem) convertJsonToItem(json);
			
			i++;
		}

        return equipment;
	}

	public static Dictionary<string, Item> extractInventoryItemsFromJson(string[] pocket)
	{

		Dictionary<string, Item> newInventory = new Dictionary<string, Item>();

		foreach (string json in pocket)
		{
			Item item = convertJsonToItem(json);

			newInventory.Add(item.getKey(), item);
		}

		return newInventory;
	}

	public static Item convertJsonToItem(string json)
	{
		if (json == null || json.Equals(""))
		{
			return null;
		}

		dynamic jsonDynamic = JsonConvert.DeserializeObject<dynamic>(json);
		ItemListID itemListID = new ItemListID(SaveDefaultValues.defaultIndexNegativeOne, SaveDefaultValues.defaultIndexNegativeOne, SaveDefaultValues.defaultStatOne);

		itemListID.listIndex = GetFromJson.getElementFromJson(ItemListID.listIndexElementName, jsonDynamic, SaveDefaultValues.defaultIndexNegativeOne);
		itemListID.itemIndex = GetFromJson.getElementFromJson(ItemListID.itemIndexElementName, jsonDynamic, SaveDefaultValues.defaultIndexNegativeOne);
		itemListID.quantity = GetFromJson.getElementFromJson(ItemListID.quantityElementName, jsonDynamic, SaveDefaultValues.defaultStatOne);

		if (itemListID.listIndex == SaveDefaultValues.defaultIndexNegativeOne ||
			itemListID.itemIndex == SaveDefaultValues.defaultIndexNegativeOne)
		{
			return null;
		}
		else
		{
			return ItemList.getItem(itemListID);
		}
	}

	public MonsterPackList extractMonsterPackListFromJson()
	{
		if (monsterPackList == null)
		{
			return default(MonsterPackList);
		}

		MonsterPack[] monsterPacks = new MonsterPack[monsterPackList.Length];

		int i = 0;
		foreach (string json in monsterPackList)
		{
			MonsterPack monsterPack = MonsterPack.extractFromJson(json);

			monsterPacks[i] = monsterPack;

			i++;
		}

		MonsterPackList newMonsterPackList = new MonsterPackList(currentLocation, monsterPacks);
		//newMonsterPackList.shouldReset = false;

		return newMonsterPackList;
	}

	public static void resetAndOverwriteQuestDictionary(Dictionary<string, Quest> newQuestDictionary)
	{
		QuestList.buildQuestListFromScratch();

		foreach (KeyValuePair<string, Quest> kvp in newQuestDictionary)
		{
			if (State.questDictionary.ContainsKey(kvp.Key))
			{
				State.questDictionary[kvp.Key] = kvp.Value;
			}
		}
	}

	public Dictionary<string, Quest> extractQuestListFromJson()
	{
		Dictionary<string, Quest> newQuestDictionary = new Dictionary<string, Quest>();

		for (int questIndex = 0; questIndex < currentQuestList.Length; questIndex++)
		{
			Quest currentQuest = Quest.extractFromJson(currentQuestList[questIndex]);

			if (currentQuest != null)
			{
				newQuestDictionary.Add(currentQuest.getName(), currentQuest);
			}
		}

		return newQuestDictionary;
	}

	public Dictionary<string, ChoiceKey> extractChoicesFromJson()
	{
		Dictionary<string, ChoiceKey> newChoices = new Dictionary<string, ChoiceKey>();

		foreach (string choiceString in currentChoices)
		{
			string key = choiceString.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("++", "+").Split(":")[1];

			ChoiceKey choice = new ChoiceKey(key.Split("+")[0], key.Split("+")[1]);

			newChoices.Add(choice.getKey(), choice);
		}

		return newChoices;
	}

	public ArrayList extractArrayListOfStringsFromJson(string[] json)
	{
		ArrayList listOfStrings = new ArrayList();

		foreach (string str in json)
		{
			listOfStrings.Add(str);
		}

		return listOfStrings;
	}

	public static CombatAction[] extractCombatActionsFromJson(string[] combatActionJSON)
	{
		CombatAction[] newCombatActions = new CombatAction[CombatActionArray.maxPlayerCombatActions];

		int index = 0;
		foreach (string actionString in combatActionJSON)
		{
			newCombatActions[index] = CombatAction.extractFromJson(actionString);

			index++;
		}

		return newCombatActions;
	}

	public void setPartyMemberDetails()
	{
		int partyMemberIndex = 0;
		List<PartyMember> allPartyMembers = PartyManager.getAllPartyMembers();
		foreach (PartyMember partyMember in allPartyMembers)
		{
            StatsWrapper partyMemberStats = new StatsWrapper();    
        
			partyMemberStats.key = "" + partyMember.getName();

            partyMemberStats.level = partyMember.stats.getLevel();
            partyMemberStats.currentHealth = partyMember.stats.currentHealth;
            partyMemberStats.canJoinParty = partyMember.canJoinParty;

            partyMemberStats.strength = partyMember.stats.getStrengthWithoutBoosts();
            partyMemberStats.dexterity = partyMember.stats.getDexterityWithoutBoosts();
            partyMemberStats.wisdom = partyMember.stats.getWisdomWithoutBoosts();
            partyMemberStats.charisma = partyMember.stats.getCharismaWithoutBoosts();

            partyMemberStats.placed = partyMember.placed;

			partyMemberStats.partyMemberPlacedPosition = partyMember.placedPosition.x + "_" + partyMember.placedPosition.y + "_" + partyMember.placedPosition.z;
			partyMemberStats.partyMemberFormationCoords = State.formation.findLocationOfStats(partyMember.stats);

            partyMemberStats.currentEquipment = convertToJson(partyMember.stats.getEquippedItems().equippedItems);
            partyMemberStats.combatActions = convertToJson(partyMember.stats.getActionArray().getActions());

            this.partyMemberStats[partyMemberIndex] = partyMemberStats;

            partyMemberIndex++;
		}
	}

    public void extractPartyMemberDetails()
    {
        State.formation = new Formation();

        Dictionary<string, PartyMember> partyMemberDict = new Dictionary<string, PartyMember>();

        for (int partyMemberIndex = 0; partyMemberIndex < this.partyMemberStats.Length; partyMemberIndex++)
        {
            string partyMemberName = this.partyMemberStats[partyMemberIndex].key;
            StatsWrapper partyMemberStatsWrapper = this.partyMemberStats[partyMemberIndex];

            AllyStats partyMemberStats = new AllyStats(partyMemberStatsWrapper);
            PartyMember partyMember = new PartyMember(partyMemberStats);

            partyMember.canJoinParty = this.partyMemberStats[partyMemberIndex].canJoinParty;
            partyMember.placed = this.partyMemberStats[partyMemberIndex].placed;

            string[] placedPosition = this.partyMemberStats[partyMemberIndex].partyMemberPlacedPosition.Split("_");
            partyMember.placedPosition = new Vector3(float.Parse(placedPosition[0]),
                                                     float.Parse(placedPosition[1]),
                                                     float.Parse(placedPosition[2]));

            ((AllyStats) partyMember.stats).equippedItems = new EquippedItems(((AllyStats) partyMember.stats), extractEquippedItemsFromJson(partyMemberStatsWrapper.currentEquipment));

            partyMemberDict.Add(partyMemberName, partyMember);

            State.formation.setCharacterAtCoords(partyMemberStatsWrapper.partyMemberFormationCoords, partyMember.stats);
        }

        PartyManager.setPartyMemberDict(partyMemberDict);
	}

	public static string[] convertAllKnownMapDataToJson()
	{
		string[] allKnownSceneNames = new string[1];
		int sceneNameIndex = 0;

		foreach (KeyValuePair<string, ArrayList> kvp in State.allKnownMapData)
		{
			ArrayList sceneNames = kvp.Value;

			foreach (string sceneName in sceneNames)
			{
				string sceneNamePlusPrefix = kvp.Key + dividerCharacter + sceneName;

				if (sceneNameIndex == allKnownSceneNames.Length)
				{
					allKnownSceneNames = Helpers.appendArray<string>(allKnownSceneNames, sceneNamePlusPrefix);
				}
				else
				{
					allKnownSceneNames[sceneNameIndex] = sceneNamePlusPrefix;
				}

				sceneNameIndex++;
			}
		}

		return allKnownSceneNames;
	}

	public static string[] convertAllMonsterDefeatKeysToJson()
	{
		string[] allMonsterDefeatKeys = new string[State.monsterDefeatKeys.Count];

		int index = 0;
		foreach (KeyValuePair<string, bool> kvp in State.monsterDefeatKeys)
		{
			allMonsterDefeatKeys[index] = kvp.Key + dividerCharacter + kvp.Value;
			index++;
		}

		return allMonsterDefeatKeys;
	}

	public void extractAllMonsterDefeatKeysToJson()
	{
		State.monsterDefeatKeys = new Dictionary<string, bool>();

		foreach (string kvpString in currentMonsterDefeatKeys)
		{
			string monsterKey = kvpString.Split(dividerCharacter)[0];
			bool defeated = bool.Parse(kvpString.Split(dividerCharacter)[1]);

			State.monsterDefeatKeys[monsterKey] = defeated;
		}
	}

	public Dictionary<string, ArrayList> extractAllKnownMapDataFromJson()
	{
		Dictionary<string, ArrayList> extractedMapData = new Dictionary<string, ArrayList>();

		foreach (string kvpString in currentKnownMapData)
		{
			string zoneKey = kvpString.Split(dividerCharacter)[0];
			string sceneName = kvpString.Split(dividerCharacter)[1];

			ArrayList listOfSceneNames;

			if (!extractedMapData.TryGetValue(zoneKey, out listOfSceneNames))
			{
				listOfSceneNames = new ArrayList();
				extractedMapData[zoneKey] = listOfSceneNames;
			}

			listOfSceneNames.Add(sceneName);
		}

		return extractedMapData;
	}

	public static string[] convertAllAreaHostilitiesToJson()
	{
		string[] allAreaHostilities = new string[AreaList.allAreas.Count];

		int areaIndex = 0;
		foreach (KeyValuePair<string, Area> kvp in AreaList.allAreas)
		{
			allAreaHostilities[areaIndex] = kvp.Value.areaKey + "~" + kvp.Value.hostility;
			areaIndex++;
		}

		return allAreaHostilities;
	}

	public void extractAllAreaHostilitiesFromJson()
	{
		AreaList.resetAreaList();

		for (int areaIndex = 0; areaIndex < currentAreaHostilities.Length; areaIndex++)
		{
			string areaKey = currentAreaHostilities[areaIndex].Split("~")[0];
			int hostility = int.Parse(currentAreaHostilities[areaIndex].Split("~")[1]);


			AreaList.allAreas[areaKey].setHostility(hostility);
		}
	}

	public string[] extractAllLessonKeysFromJson()
	{
		string[] lessonKeys = new string[currentLessons.Length];

		for (int keyIndex = 0; keyIndex < lessonKeys.Length; keyIndex++)
		{
			lessonKeys[keyIndex] = Lesson.extractFromJson(currentLessons[keyIndex]);
		}

		return lessonKeys;
	}

	private int getNumberOfPartyMembersInFormation()
	{
		int partyMembersInFormation = 1;

		foreach (StatsWrapper wrapper in partyMemberStats)
		{
			if (!wrapper.partyMemberFormationCoords.Equals(GridCoords.getDefaultCoords()))
			{
				partyMembersInFormation++;
			}
		}

		return partyMembersInFormation;
	}

	private string getPartyMembersInPartyText()
	{
        return getNumberOfPartyMembersInFormation() + "/" + PartyStats.getPartySizeMaximum(this.partyMemberStats);
        // PlayerStats.getMaximumPartyMemberSlots(charisma, level);
	}

	private string getLocationUIDisplayName()
	{
		return MapObjectList.getMapObject(currentLocation).getNotificationDisplayName();
	}

	//public const string autoSave1Name = "Autosave 1";
	//public const string autoSave2Name = "Autosave 2";
	// public const string autoSave3Name = "Autosave 3";

	public bool isAutosave()
	{
		return saveNumber < 0 && SaveHandler.nameMeetsAutosaveCriteria(saveName);
	}

	//IDescribable methods
	public string getName()
	{
		return saveName;
	}

	public bool ineligible()
	{
		return false;
	}

	public GameObject getRowType(RowType rowType)
	{
		return Resources.Load<GameObject>(PrefabNames.saveRow);
	}

	public GameObject getDescriptionPanelFull()
	{
		return Resources.Load<GameObject>(PrefabNames.saveLoadPanelFull);
	}

	public GameObject getDescriptionPanelFull(PanelType type)
	{
		string descriptionPanelType;

		switch (type)
		{
			case PanelType.Notification:
				descriptionPanelType = PrefabNames.areaNameDescriptionPanel;
				break;
			default:
				descriptionPanelType = PrefabNames.saveLoadPanelFull;
				break;
		}

		return Resources.Load<GameObject>(descriptionPanelType);
	}

	public GameObject getDecisionPanel()
	{
		if (SaveHandler.cannotSaveInCurrentState())
		{
			return Resources.Load<GameObject>(PrefabNames.loadDecisionPanel);
		}
		else
		{
			return Resources.Load<GameObject>(PrefabNames.loadOverwriteDeleteDecisionPanel);
		}
	}

	public bool withinFilter(string[] filterParameters)
	{
		return true;
	}

	public void describeSelfFull(DescriptionPanel panel)
	{
        AllyStats playerStats = (AllyStats)PartyManager.getPlayerStats(partyMemberStats);

        playerStats.describeSelfFull(panel);

		panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.secondaryNameText, saveName);

		DescriptionPanel.setText(panel.amountText, getSaveNumberForDisplay());

		DescriptionPanel.setText(panel.worthText, gold);
		DescriptionPanel.setText(panel.affinityText, affinity);
		DescriptionPanel.setText(panel.partyText, getPartyMembersInPartyText());
	}

	public void describeSelfRow(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, saveName);
		DescriptionPanel.setText(panel.amountText, getSaveNumberForDisplay());
	}

	public void setUpDecisionPanel(IDecisionPanel descisionPanel) 
	{
		//empty on purpose
	}

	public ArrayList getRelatedDescribables()
	{
		return new ArrayList();
	}
	
	public bool buildableWithBlocks()
	{
		return true;
	}
	
	public bool buildableWithBlocksRows()
    {
        return false;
    }

	//IDescribableInBlocks methods
	public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Save Name: " + getName()));
		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Save Number: " + getSaveNumberForDisplay()));
		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Location: " + getLocationUIDisplayName()));

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, ""));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, ""));

        foreach (StatsWrapper partyMember in partyMemberStats)
        {
            buildingBlocks.AddRange(partyMember.getDescriptionBuildingBlocks());
        }

		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, ""));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, ""));

        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Gold: " + gold));
		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Affinity: " + affinity));
		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, "Party Size: " + getPartyMembersInPartyText()));

		return buildingBlocks;
	}

	//ISortable Methods
	public int getQuantity()
	{
		throw new NotImplementedException("SaveBlueprints cannot be sorted by Quantity, only name/number");
	}
	public int getWorth()
	{
		throw new NotImplementedException("SaveBlueprints cannot be sorted by Worth, only name/number");
	}
	public string getType()
	{
		throw new NotImplementedException("SaveBlueprints cannot be sorted by Type, only name/number");
	}
	public string getSubtype()
	{
		throw new NotImplementedException("SaveBlueprints cannot be sorted by Subtype, only name/number");
	}
	public int getLevel()
	{
		throw new NotImplementedException("SaveBlueprints cannot be sorted by Level, only name/number");
	}
	public int getNumber()
	{
		return Math.Abs(saveNumber);
	}

	private string getSaveNumberForDisplay()
	{
		switch(saveName)
		{
			case SaveHandler.autoSave1Name:
				return "A1";
			case SaveHandler.autoSave2Name:
				return "A2";
			case SaveHandler.autoSave3Name:
				return "A3";
			default:
				return "" + getNumber();
		}
	}

}