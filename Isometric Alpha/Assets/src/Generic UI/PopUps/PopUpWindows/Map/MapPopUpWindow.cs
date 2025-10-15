using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapPopUpWindow : PopUpWindow, IEscapable
{

	public static Dictionary<string, MapTile> sceneTileDictionary;

	private static MapPopUpWindow instance;

	public Transform mapGridParent;

	public ScrollableUIElement journalEntryGrid;
	public GameObject journalEntryDropdown;
	public TextMeshProUGUI journalEntryDescriptionText;
	public TextMeshProUGUI totalQuestCounter;

	public TextMeshProUGUI zoneName;
	public TextMeshProUGUI locationName;

	public string currentZoneKey;
	private MapFormat currentMapFormat;

	public IMapObject fastTravelTarget;
	public BinaryPanelPopUpButton fastTravelPopUpButton;

	public ChangeMapZoneButton[] zoneButtons;

	public void populate(string zoneKey)
	{
		this.currentZoneKey = zoneKey;

		currentMapFormat = MapFormatList.mapFormats[zoneKey];

		destroyAllMapTiles();

		deactivateZoneButtons();

		generateMapTiles();

		populateNamePlates();

		populateJournalEntryGrid();
	}

	private void populateJournalEntryGrid()
	{
		ArrayList relevantJournalEntries = new ArrayList();

		ArrayList activeUnfinishedQuests = QuestList.getActiveUnfinishedQuests();

		foreach (Quest quest in activeUnfinishedQuests)
		{
			QuestStep currentQuestStep = quest.getCurrentQuestStep();

			if (currentQuestStep.hasTargetLocation() && currentQuestStep.mapZone.Equals(currentZoneKey))
			{
				relevantJournalEntries.Add(currentQuestStep);
				MapTile.OnJournalEntryShownOnMap.Invoke(currentQuestStep.mapLocation);
			}
		}

		totalQuestCounter.text = "" + QuestList.getNumberOfActiveUnfinishedQuestsInZone(currentZoneKey);

		journalEntryGrid.populatePanels(relevantJournalEntries);
	}

	public static void highlightQuestStar(string locationName)
	{
		if (locationName != null && sceneTileDictionary.ContainsKey(locationName))
		{
			sceneTileDictionary[locationName].questCounter.highlightStar();
		}
	}

	public static void unhighlightQuestStar(string locationName)
	{
		if (locationName != null && sceneTileDictionary.ContainsKey(locationName))
		{
			sceneTileDictionary[locationName].questCounter.unhighlightStar();
		}
	}

	public static void showJournalEntryDescription(QuestStep questStep)
	{
		if (instance == null)
		{
			return;
		}

		instance.journalEntryDescriptionText.text = questStep.journalDescription;
		instance.journalEntryDropdown.SetActive(true);
	}

	public static void hideJournalEntryDescription()
	{
		if (instance == null)
		{
			return;
		}

		instance.journalEntryDropdown.SetActive(false);
	}

	private void populateNamePlates()
	{
		zoneName.text = MapObjectList.getMapObject(currentZoneKey).getMapUIDisplayName();
		locationName.text = MapObjectList.getMapObject(AreaManager.locationName).getMapUIDisplayNameWithoutZoneName();
	}

	private void destroyAllMapTiles()
	{
		foreach (Transform child in mapGridParent)
		{
			Destroy(child.gameObject);
		}
	}
	private void generateMapTiles()
	{
		sceneTileDictionary = new Dictionary<string, MapTile>();

		foreach (MapTileFormat tileFormat in currentMapFormat.tileFormats)
		{
			MapTile mapTile = Instantiate(Resources.Load<GameObject>(PrefabNames.mapTileName), mapGridParent).GetComponent<MapTile>();

			if (!tileFormat.isVisible())
			{
				mapTile.readInFormat(currentMapFormat.getDefaultMapTileFormat());
			}
			else
			{

				mapTile.readInFormat(tileFormat);
				setUpZoneButtons(tileFormat);

				if (tileFormat.locationName != null && tileFormat.locationName.Length > 0 && tileFormat.hasBeenDiscovered())
				{
					addSceneNameToDictionary(tileFormat.locationName, mapTile);
				}
			}
		}
	}

	public void addSceneNameToDictionary(string locationName, MapTile mapTile)
	{
		sceneTileDictionary.Add(locationName, mapTile);

		//Add Interiors to Dictionary pointing at same mapTile

		IMapObject mapObject = MapObjectList.getMapObject(locationName);

		string[] adjacentLocations = mapObject.getAdjacentMapObjects();

		foreach (string adjacentLocation in adjacentLocations)
		{
			if (MapObjectList.getMapObject(adjacentLocation).isInterior() &&
				!sceneTileDictionary.ContainsKey(adjacentLocation))
			{
				sceneTileDictionary.Add(adjacentLocation, mapTile);
			}
		}
	}

	public void setUpZoneButtons(MapTileFormat tileFormat)
	{
		if (tileFormat.locationName == null || tileFormat.locationName.Equals("") || !MapObjectList.getMapObject(tileFormat.locationName).hasBeenDiscovered())
		{
			return;
		}

		ZoneButtonInfo[] buttonInfos = MapObjectList.getMapObject(tileFormat.locationName).getZoneButtons();

		foreach (ZoneButtonInfo buttonInfo in buttonInfos)
		{
			zoneButtons[buttonInfo.buttonIndex].setZoneKey(buttonInfo.zoneKey);
		}
	}

	public static void setFastTravelTarget(IMapObject target)
	{
		getInstance().fastTravelTarget = target;
	}

	public static bool hasFastTravelTarget()
	{
		return getInstance().fastTravelTarget != null;
	}

	public static void leaveFastTravelMode()
	{
		MapPopUpWindow.getInstance().fastTravelTarget = null;
	}

	public static void fastTravelPanelCloseButtonPress()
	{
		BinaryDescisionPanel fastTravelPanel = (BinaryDescisionPanel)getInstance().fastTravelPopUpButton.getPopUpWindow();
		
		fastTravelPanel.closeButtonPress();
	}

	private void deactivateZoneButtons()
	{
		foreach (ChangeMapZoneButton button in zoneButtons)
		{
			button.deactivate();
		}
	}

	public static Sprite getDefaultFloorImage()
	{
		return Helpers.loadSpriteFromResources(getInstance().currentMapFormat.defaultFloorImage);
	}

	public static Sprite getDefaultMapIcon()
	{
		return Helpers.loadSpriteFromResources(getInstance().currentMapFormat.defaultMapIcon);
	}

	public static MapPopUpWindow getInstance()
	{
		return instance;
	}

	private void Awake()
	{
		if (instance != null)
		{
			throw new IOException("Duplicate instances of MapPopUpWindow exist");
		}

		instance = this;
		NotificationManager.OnDeleteAllNotifications.Invoke();
	}
	public const int northEastButtonIndex = 0;
	public const int northButtonIndex = 1;
	public const int eastNorthButtonIndex = 2;
	public const int eastSouthButtonIndex = 3;
	public const int eastButtonIndex = 4;
	public const int southEastButtonIndex = 5;
	public const int southWestButtonIndex = 6;
	public const int southButtonIndex = 7;
	public const int westSouthButtonIndex = 8;
	public const int westNorthButtonIndex = 9;
	public const int westButtonIndex = 10;
	public const int northWestButtonIndex = 11;
}
