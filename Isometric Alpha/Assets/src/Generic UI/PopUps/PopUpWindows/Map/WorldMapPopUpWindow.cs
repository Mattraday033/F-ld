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
    private readonly static Vector3 posAdjustment = new Vector3(0.17f, 0.35f);

	private static WorldMapPopUpWindow instance;

    private readonly static Vector3 farthestOutZoomScale = new Vector3(65f, 65f, 1f);
    private readonly static Vector3 middleZoomScale = new Vector3(100f, 100f, 1f);
    private readonly static Vector3 farthestInZoomScale = new Vector3(150f, 150f, 1f);

    public ZoomLevel currentZoomLevel = ZoomLevel.FarthestIn;
    public Button zoomInButton;
    public Button zoomOutButton;
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

        landmarkDict[zoneKey].revealIndicator();

        // set world map to be above current landmark button
    }

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(instance.gameObject);
		}

		instance = this;
		NotificationManager.OnDeleteAllNotifications.Invoke();

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
                                                    new string[]{ZoneKeyList.manseFirstFloor, ZoneKeyList.manseSecondFloor}));
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
        return Resources.Load<Sprite>(MapTileSpriteList.getSpriteFullPath(spriteName));
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