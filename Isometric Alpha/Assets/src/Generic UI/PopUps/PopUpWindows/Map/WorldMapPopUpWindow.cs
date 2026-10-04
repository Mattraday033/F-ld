using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum ZoomLevel { FarthestOut, Middle, FarthestIn}

public class WorldMapPopUpWindow : PopUpWindow, IEscapable
{

    public Grid worldMapLandmarkSpawnGrid;
    public Transform worldMapLandmarkParent;
    private readonly static Vector3 posAdjustment = new Vector3(0.22f, 0.45f);

	private static WorldMapPopUpWindow instance;

    private readonly static Vector3 farthestOutZoomScale = new Vector3(65f, 65f, 1f);
    private readonly static Vector3 middleZoomScale = new Vector3(100f, 100f, 1f);
    private readonly static Vector3 farthestInZoomScale = new Vector3(150f, 150f, 1f);

    public ZoomLevel currentZoomLevel = ZoomLevel.FarthestIn;
    public ThreeRingButton zoomInButton;
    public ThreeRingButton zoomOutButton;
    public RectTransform worldMapGridTransform;

    public Dictionary<string, WorldMapLandmark> landmarkDict = new Dictionary<string, WorldMapLandmark>();

	public static WorldMapPopUpWindow getInstance()
	{
		return instance;
	}

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateWorldMapPopUpWindow()
    {
        instance = null;
    }

	public void populate()
    {
        string zoneKey = MapObjectList.getCurrentZoneKey();

        //the prebuilt window is shown again after the player has moved, and an indicator is otherwise only ever switched on
        foreach(WorldMapLandmark landmark in landmarkDict.Values)
        {
            landmark.hideIndicator();
        }

        landmarkDict[zoneKey].revealIndicator();

        // set world map to be above current landmark button
    }

    #region Prebuilt window

    //set on the copy PrebuiltScreenManager makes, which is shown and hidden. Any other copy is made and destroyed as before
    public bool isPrebuilt { get; private set; } = false;

    //the zoom, map scale and scroll position the prefab opens on, which the prebuilt window is put back to each time it is shown
    private ZoomLevel _StartingZoomLevel;
    private Vector3 _StartingGridScale;
    private RectTransform scrollContent;
    private Vector2 _StartingScrollPosition;

    //only the copy that is on screen answers as the world map, so "there is an instance" keeps meaning "the world map is open"
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
    }

    //does for the prebuilt window what instantiating one does: on screen, answering as the world map, opened where the prefab opens
    public void show(Transform parent)
    {
        transform.SetParent(parent, false);

        //the screen blocker is spawned just before this, and the window has to be drawn over it
        transform.SetAsLastSibling();

        currentZoomLevel = _StartingZoomLevel;
        worldMapGridTransform.localScale = _StartingGridScale;

        if(scrollContent != null)
        {
            scrollContent.anchoredPosition = _StartingScrollPosition;
        }

        setZoomButtonInteractability();

        gameObject.SetActive(true);

        instance = this;
        NotificationManager.OnDeleteAllNotifications.Invoke();
    }

    //leaves the prebuilt window the way destroying it left things, ready to be shown again
    public void hide(Transform holder)
    {
        foreach(WorldMapLandmark landmark in landmarkDict.Values)
        {
            landmark.clearHover();
        }

        if(instance == this)
        {
            instance = null;
        }

        gameObject.SetActive(false);
        transform.SetParent(holder, false);
    }

    #endregion

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(instance.gameObject);
		}

		instance = this;
		NotificationManager.OnDeleteAllNotifications.Invoke();

        _StartingZoomLevel = currentZoomLevel;
        _StartingGridScale = worldMapGridTransform.localScale;

        ScrollRect scrollArea = GetComponentInChildren<ScrollRect>(true);

        if(scrollArea != null && scrollArea.content != null)
        {
            scrollContent = scrollArea.content;
            _StartingScrollPosition = scrollContent.anchoredPosition;
        }

        zoomInButton.lockInRestingPosition();
        zoomOutButton.lockInRestingPosition();

        instantiateLandmarks();
        setZoomButtonInteractability();
	}

	void Update()
	{
        KeyPressManager.updateKeyBools();

        if(KeyBindingList.mouseWheelScrollingUp() && !KeyPressManager.handlingPrimaryKeyPress && canZoomIn())
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            zoomIn();
        }

        if(KeyBindingList.mouseWheelScrollingDown() && !KeyPressManager.handlingPrimaryKeyPress && canZoomOut())
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            zoomOut();
        }

	}

    public void zoomIn()
    {
        if(!canZoomIn())
        {
            return;
        }

        currentZoomLevel++;

        setMapToCurrentZoomScale();

        setZoomButtonInteractability();
    }

    public void zoomOut()
    {
        if(!canZoomOut())
        {
            return;
        }
        
        currentZoomLevel--;

        setMapToCurrentZoomScale();

        setZoomButtonInteractability();
    }

    private bool canZoomOut()
    {
        return currentZoomLevel != ZoomLevel.FarthestOut;
    }

    private bool canZoomIn()
    {
        return currentZoomLevel != ZoomLevel.FarthestIn;
    }

    private void setZoomButtonInteractability()
    {
        switch(currentZoomLevel)
        {
            case ZoomLevel.FarthestOut:
                zoomOutButton.interactable = false;
                zoomInButton.interactable = true;
                return;
            case ZoomLevel.Middle:
                zoomOutButton.interactable = true;
                zoomInButton.interactable = true;
                return;
            case ZoomLevel.FarthestIn:
                zoomOutButton.interactable = true;
                zoomInButton.interactable = false;
                return;
        }
    }

    private void setMapToCurrentZoomScale()
    {
        switch(currentZoomLevel)
        {
            case ZoomLevel.FarthestOut:
                worldMapGridTransform.localScale = farthestOutZoomScale;
                return;
            case ZoomLevel.Middle:
                worldMapGridTransform.localScale = middleZoomScale;
                return;
            case ZoomLevel.FarthestIn:
                worldMapGridTransform.localScale = farthestInZoomScale;
                return;
        }
    }

    private void instantiateLandmarks()
    {
        List<LandmarkSpawnDetails> allLandmarks = WorldMapLandmarkList.getAllLandmarks();

        foreach(LandmarkSpawnDetails landmarkSpawnDetails in allLandmarks)
        {
            Vector3 landmarkSpawnPos = worldMapLandmarkSpawnGrid.GetCellCenterWorld(landmarkSpawnDetails.spawnCoords);

            GameObject landmark = Instantiate(Resources.Load<GameObject>(PrefabNames.worldMapLandmark), worldMapLandmarkParent);

            RectTransform rectTransform = landmark.GetComponent<RectTransform>();

            rectTransform.position = landmarkSpawnPos - posAdjustment;

            WorldMapLandmark landmarkComp = landmark.GetComponent<WorldMapLandmark>();

            landmarkComp.setLandmark(landmarkSpawnDetails);
            landmarkDict[landmarkComp.zoneKey] = landmarkComp;

            foreach(string extraZoneKey in landmarkSpawnDetails.extraZoneKeys)
            {
                landmarkDict[extraZoneKey] = landmarkComp;
            }
        }
    }
    
}

public static class WorldMapLandmarkList
{
    private static List<LandmarkSpawnDetails> allLandmarks;

    public static List<LandmarkSpawnDetails> getAllLandmarks()
    {
        if(allLandmarks == null)
        {
            instantiateWorldMapLandmarkList();
        }

        return allLandmarks;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateWorldMapLandmarkList()
    {
        allLandmarks = new List<LandmarkSpawnDetails>();

        allLandmarks.Add(new LandmarkSpawnDetails(new Vector3Int(5, 4), MapDisplayNameList.lovashiCamp, MapTileSpriteList.campWithManseMapTile, ZoneKeyList.lovashiCamp, 
                                                    new string[]{ZoneKeyList.manseFirstFloor, ZoneKeyList.manseSecondFloor, ZoneKeyList.pit}));
        allLandmarks.Add(new HighSortPriortyLandmarkSpawnDetails(new Vector3Int(5, 5), MapDisplayNameList.lovashiMine, MapTileSpriteList.worldMapMineMapTile, 
                                                                  ZoneKeyList.mineLvl1, new string[]{ZoneKeyList.mineLvl2, ZoneKeyList.mineLvl3}));

    }
}

public class LandmarkSpawnDetails
{
    public string landmarkName;
    public string spriteName;
    public Vector3Int spawnCoords;

    public string zoneKey;    
    public string[] extraZoneKeys;

    public LandmarkSpawnDetails(Vector3Int spawnCoords, string landmarkName, string spriteName, string zoneKey)
    {
        this.landmarkName = landmarkName;
        this.spriteName = spriteName;
        this.spawnCoords = spawnCoords;

        this.zoneKey = zoneKey;
        this.extraZoneKeys = new string[0];
    }

    public LandmarkSpawnDetails(Vector3Int spawnCoords, string landmarkName, string spriteName, string zoneKey, string[] extraZoneKeys)
    {
        this.landmarkName = landmarkName;
        this.spriteName = spriteName;
        this.spawnCoords = spawnCoords;

        this.zoneKey = zoneKey;
        this.extraZoneKeys = extraZoneKeys;
    }

    public Sprite getSprite()
    {
        return SpriteUtil.loadSpriteFromResources(MapTileSpriteList.getSpriteFullPath(spriteName));
    }

    public virtual int getSortPriority()
    {
        return Constants.indexFive;
    }
}

public class HighSortPriortyLandmarkSpawnDetails: LandmarkSpawnDetails
{

    public HighSortPriortyLandmarkSpawnDetails(Vector3Int spawnCoords, string landmarkName, string spriteName, string zoneKey):
    base(spawnCoords, landmarkName, spriteName, zoneKey)
    {
        
    }

    public HighSortPriortyLandmarkSpawnDetails(Vector3Int spawnCoords, string landmarkName, string spriteName, string zoneKey, string[] extraZoneKeys):
    base(spawnCoords, landmarkName, spriteName, zoneKey, extraZoneKeys)
    {

    }

    public override int getSortPriority()
    {
        return Constants.indexSeven;
    }
}