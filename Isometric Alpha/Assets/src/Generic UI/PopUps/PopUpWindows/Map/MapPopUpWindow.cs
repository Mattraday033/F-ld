using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
	public GameObject locationNameBackground;

	public string currentZoneKey;
	private MapFormat currentMapFormat;

	public IMapObject fastTravelTarget;
	public BinaryPanelPopUpButton fastTravelPopUpButton;

	public ChangeMapZoneButton[] zoneButtons;

	public static readonly ProfilerMarker showMarker = new ProfilerMarker("MapPopUpWindow.show");

	//How the prebuilt window is hidden: by switching its canvas off (true), or by switching the whole window off and moving it
	//to the prebuilt holder (false). Left switchable so the two can be timed against each other with the marker above.
	//Once one has won, this and the path it turns off can go
	private static readonly bool hideByDisablingCanvas = true;

	//set on the copy PrebuiltScreenManager makes, which is shown and hidden. Any other copy is made and destroyed as before
	public bool isPrebuilt { get; private set; } = false;

	//the tiles in grid order. They are made once and filled in again each time a zone is shown
	private List<MapTile> mapTiles = new List<MapTile>();

	//only on the prebuilt copy
	private Canvas windowCanvas;
	private GraphicRaycaster windowRaycaster;

	//the canvases inside the window that were switched off with it, so that showing it switches the same ones back on
	private List<Canvas> nestedCanvasesSwitchedOff = new List<Canvas>();

	//how the prefab has the dropdown, which is how a window that was just instantiated would have it
	private bool _JournalEntryDropdownActiveByDefault;

	public void populate(string zoneKey)
	{
		this.currentZoneKey = zoneKey;

		currentMapFormat = MapFormatList.mapFormats[zoneKey];

		deactivateZoneButtons();

		refreshMapTiles();

		populateNamePlates();

		populateJournalEntryGrid();
	}

	#region Prebuilt window

	//only the copy that is on screen answers as the map, so "there is an instance" keeps meaning "the map is open"
	public bool isShowing
	{
		get
		{
			return instance == this;
		}
	}

	public void markAsPrebuilt()
	{
		isPrebuilt = true;

		//its own canvas, so it can be drawn or not without switching off everything inside it
		windowCanvas = gameObject.AddComponent<Canvas>();

		//a nested canvas needs its own raycaster, or nothing inside the window receives clicks or hovers
		windowRaycaster = gameObject.AddComponent<GraphicRaycaster>();

		//made behind the loading screen, and not the open map until it is shown
		setRendered(false);
		instance = null;
	}

	//makes up to this many more of the tiles a zone needs, and says whether they have all been made
	public bool createTiles(int howMany)
	{
		int tilesNeeded = MapFormatList.mapDimensions * MapFormatList.mapDimensions;

		for (int made = 0; made < howMany && mapTiles.Count < tilesNeeded; made++)
		{
			getOrCreateTile(mapTiles.Count);
		}

		//the tiles just made each bring a canvas that is switched on, and the window is hidden
		if (isPrebuilt && !windowCanvas.enabled)
		{
			switchOffNestedCanvases();
		}

		return mapTiles.Count >= tilesNeeded;
	}

	//does for the prebuilt window what instantiating one does: on screen, answering as the map, with nothing left from last time
	public void show(Transform parent)
	{
		if (isShowing)
		{
			throw new IOException("Duplicate instances of MapPopUpWindow exist");
		}

		attachTo(parent);

		//the screen blocker is spawned just before this, and the window has to be drawn over it
		transform.SetAsLastSibling();

		gameObject.SetActive(true);
		setRendered(true);

		instance = this;
		NotificationManager.OnDeleteAllNotifications.Invoke();

		fastTravelTarget = null;
		journalEntryDropdown.SetActive(_JournalEntryDropdownActiveByDefault);
	}

	//leaves the prebuilt window the way destroying it left things, ready to be shown again
	public void hide(Transform holder)
	{
		foreach (TutorialSequenceStepTargetUIObject tutorialTarget in GetComponentsInChildren<TutorialSequenceStepTargetUIObject>(true))
		{
			tutorialTarget.clearHighlight();
		}

		foreach (MapTile mapTile in mapTiles)
		{
			mapTile.questCounter.clearStarHighlight();
		}

		MouseHoverManager.destroyHoverIconInside(transform);

		journalEntryGrid.deleteAllPanels();

		fastTravelTarget = null;

		if (instance == this)
		{
			instance = null;
		}

		if (hideByDisablingCanvas)
		{
			setRendered(false);
		}
		else
		{
			gameObject.SetActive(false);
			transform.SetParent(holder, false);
		}
	}

	//for a scene that is about to be unloaded. However the window is hidden, it can't stay under that scene's UI
	public void park(Transform holder)
	{
		if (isShowing)
		{
			hide(holder);
		}

		gameObject.SetActive(false);
		transform.SetParent(holder, false);
	}

	//Puts a window that is hidden by its canvas back under the live UI, switched on and ready. Switching it on is the
	//expensive part, so this is done as the overworld's UI loads rather than when the map is first opened
	public void attachHidden(Transform parent)
	{
		if (!hideByDisablingCanvas || parent == null || isShowing)
		{
			return;
		}

		attachTo(parent);

		setRendered(false);
		switchOffNestedCanvases();

		gameObject.SetActive(true);
	}

	//moved while switched off, because moving it while it is on makes every image and label inside it rebuild
	private void attachTo(Transform parent)
	{
		if (transform.parent == parent)
		{
			return;
		}

		gameObject.SetActive(false);
		transform.SetParent(parent, false);
	}

	private void setRendered(bool rendered)
	{
		if (windowCanvas.enabled == rendered)
		{
			return;
		}

		windowCanvas.enabled = rendered;
		windowRaycaster.enabled = rendered;

		if (rendered)
		{
			foreach (Canvas nestedCanvas in nestedCanvasesSwitchedOff)
			{
				if (nestedCanvas != null)
				{
					nestedCanvas.enabled = true;
				}
			}

			nestedCanvasesSwitchedOff.Clear();
		}
		else
		{
			switchOffNestedCanvases();
		}
	}

	//the quest counters and the journal dropdown have canvases of their own, which are switched off along with the window's
	//in case they would otherwise go on being drawn
	private void switchOffNestedCanvases()
	{
		foreach (Canvas nestedCanvas in GetComponentsInChildren<Canvas>(true))
		{
			if (nestedCanvas != windowCanvas && nestedCanvas.enabled)
			{
				nestedCanvas.enabled = false;
				nestedCanvasesSwitchedOff.Add(nestedCanvas);
			}
		}
	}

	#endregion

	private void populateJournalEntryGrid()
	{
		List<QuestStep> relevantJournalEntries = new List<QuestStep>();

		List<Quest> activeUnfinishedQuests = QuestList.getActiveUnfinishedQuests();

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

        if(currentZoneKey.Equals(MapObjectList.getCurrentZoneKey()))
        {
            locationNameBackground.SetActive(true);
            locationName.text = MapObjectList.getMapObject(AreaManager.locationName).getMapUIDisplayNameWithoutZoneName();
        } else
        {
            locationNameBackground.SetActive(false);
        }
	}

	//the tile for this place in the grid, made the first time it is asked for
	private MapTile getOrCreateTile(int tileIndex)
	{
		//whatever the prefab has sitting in the grid is cleared out once, before the first tile is made
		if (mapTiles.Count <= 0)
		{
			foreach (Transform child in mapGridParent)
			{
				Destroy(child.gameObject);
			}
		}

		while (mapTiles.Count <= tileIndex)
		{
			mapTiles.Add(Instantiate(Resources.Load<GameObject>(PrefabNames.mapTileName), mapGridParent).GetComponent<MapTile>());
		}

		return mapTiles[tileIndex];
	}

	//Every zone's map is the same size, so the tiles made for the first one shown are filled in again for each of the others,
	//where they used to be destroyed and instantiated again
	private void refreshMapTiles()
	{
		sceneTileDictionary = new Dictionary<string, MapTile>();

		int tileIndex = 0;
		foreach (MapTileFormat tileFormat in currentMapFormat.tileFormats)
		{
			MapTile mapTile = getOrCreateTile(tileIndex);

			mapTile.resetToBlank();

			tileIndex++;

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
		return SpriteUtil.loadSpriteFromResources(MapTileSpriteList.getSpriteFullPath(getInstance().currentMapFormat.defaultFloorImage));
	}

	public static Sprite getDefaultMapIcon()
	{
		return SpriteUtil.loadSpriteFromResources(MapTileSpriteList.getSpriteFullPath(getInstance().currentMapFormat.defaultMapIcon));
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

		_JournalEntryDropdownActiveByDefault = journalEntryDropdown.activeSelf;
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
