using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WorldMapPopUpWindow : PopUpWindow, IEscapable
{

    public Grid worldMapLandmarkSpawnGrid;
    public Transform worldMapLandmarkParent;
    private readonly static Vector3 posAdjustment = new Vector3(0.17f, 0.35f);

	private static WorldMapPopUpWindow instance;

	public static WorldMapPopUpWindow getInstance()
	{
		return instance;
	}

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateWorldMapPopUpWindow()
    {
        instance = null;
    }

	// public void populate(string zoneKey)
    // {
        
    // }

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(instance.gameObject);
		}

		instance = this;
		NotificationManager.OnDeleteAllNotifications.Invoke();

        instantiateLandmarks();
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

        allLandmarks.Add(new LandmarkSpawnDetails(new Vector3Int(1, -12), MapDisplayNameList.lovashiCamp, ZoneKeyList.lovashiCamp, PrefabNames.delverCampMapTile));
        allLandmarks.Add(new HighSortPriortyLandmarkSpawnDetails(new Vector3Int(1, -11), MapDisplayNameList.lovashiMine, ZoneKeyList.mineLvl1, PrefabNames.mineMapTile));

    }
}

public class LandmarkSpawnDetails
{

    public string landmarkName;
    public string zoneKey;
    public string spriteName;
    public Vector3Int spawnCoords;

    public LandmarkSpawnDetails(Vector3Int spawnCoords, string landmarkName, string zoneKey, string spriteName)
    {
        this.landmarkName = landmarkName;
        this.zoneKey = zoneKey;
        this.spriteName = spriteName;
        this.spawnCoords = spawnCoords;
    }

    public Sprite getSprite()
    {
        Debug.LogError("spriteName = " + spriteName);
        return Resources.Load<Sprite>(spriteName);
    }

    public virtual int getSortPriority()
    {
        return Constants.indexOne;
    }
}

public class HighSortPriortyLandmarkSpawnDetails: LandmarkSpawnDetails
{

    public HighSortPriortyLandmarkSpawnDetails(Vector3Int spawnCoords, string landmarkName, string zoneKey, string spriteName):
    base(spawnCoords, landmarkName, zoneKey, spriteName)
    {
        
    }

    public override int getSortPriority()
    {
        return Constants.indexThree;
    }
}