using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundManager : MonoBehaviour
{

	public List<Tilemap> backgroundTilemaps = new List<Tilemap>();
    private int currentBGPrefabIndex = 1;
    private const string backgroundAbbreviationCharacter = "-BG-";
    private string backgroundKey = "";

    private const int maxRows = 15;
    private const int maxCols = 15;

    private Vector3Int currentTileCoords = new Vector3Int(-15,-15);
    private const int lengthMinusOne = 6;

    private List<List<Tilemap>> tilemapPrefabs = new List<List<Tilemap>>();

    private void Start()
    {
        buildBackground();
    }

    private void buildBackground()
    {
        if(backgroundKey.Equals(getBackgroundKey()))
        {
            return;
        }

        destroyAllTilemapPrefabs();

        backgroundKey = getBackgroundKey();

        findBackgroundTilePrefabs();

        if(tilemapPrefabs.Count <= 0)
        {
            return;
        }

        createBackgroundTilemap();

        StartCoroutine(populateBackgroundDelayed());
    }

    private IEnumerator populateBackgroundDelayed()
    {
        yield return null; // Wait one frame for tilemaps to initialize
        
        populateBackgroundTilemap();
    }

    private void populateBackgroundTilemap()
    {
        for(int row = -10; row <= maxRows; row++)
        {
            for(int col = -10; col <= maxCols; col++)
            {
                currentTileCoords = new Vector3Int(row, col);
                populateSingleBackgroundTile();
            } 
        }
    }

    private void populateSingleBackgroundTile()
    {
        List<Tilemap> tilemapPrefab = tilemapPrefabs[Random.Range(Constants.indexZero, tilemapPrefabs.Count)];

        for(int index = 0; index < tilemapPrefab.Count && index < backgroundTilemaps.Count; index++)
        {
            copyPrefabIntoBackground(tilemapPrefab[index], index);
        }
    }

    private void copyPrefabIntoBackground(Tilemap prefabTilemap, int layerIndex)
    {
        Tilemap background = backgroundTilemaps[layerIndex];

        for(int row = -3; row <= 3; row++)
        {
            for(int col = -3; col <= 3; col++)
            {
                Vector3Int coords = new Vector3Int(row, col);
                background.SetTile(getBackgroundCoords(coords), prefabTilemap.GetTile(coords));
            }
        }
    }

    private Vector3Int getBackgroundCoords(Vector3Int prefabCoords)
    {
        return new Vector3Int(prefabCoords.x + ((currentTileCoords.x-1)*lengthMinusOne),
                              prefabCoords.y + ((currentTileCoords.y-1)*lengthMinusOne));
    }

    private void createBackgroundTilemap()
    {
        backgroundTilemaps = Instantiate(Resources.Load<GameObject>(PrefabNames.backgroundTilemap), transform)
                             .GetComponent<BackgroundTileLayerList>().tilemapLayers;
    }

    private void findBackgroundTilePrefabs()
    {
        currentBGPrefabIndex = 1;
        GameObject backgroundPrefab = Resources.Load<GameObject>(getCurrentPrefabFolderPath());

        while(backgroundPrefab != null)
        {
            backgroundPrefab = Instantiate(backgroundPrefab, transform);

            backgroundPrefab.SetActive(false);

            tilemapPrefabs.Add(backgroundPrefab.GetComponent<BackgroundTileLayerList>().tilemapLayers);

            //set up for next loop
            currentBGPrefabIndex++;
            backgroundPrefab = Resources.Load<GameObject>(getCurrentPrefabFolderPath());
        } 
    }

    private void destroyAllTilemapPrefabs()
    {
        foreach(List<Tilemap> listOfTilemaps in tilemapPrefabs)
        {
            DestroyImmediate(listOfTilemaps[0].transform.parent.gameObject);
        }

        tilemapPrefabs = new List<List<Tilemap>>();

        if(backgroundTilemaps.Count > 0)
        {
            DestroyImmediate(backgroundTilemaps[0].transform.parent.gameObject);

            backgroundTilemaps = new List<Tilemap>();
        }
    }

    private string getCurrentPrefabFolderPath()
    {
        return PrefabNames.OOCBackgroundFolderPath + 
                                backgroundKey + Constants.seperatorChar +
                                backgroundKey + backgroundAbbreviationCharacter + currentBGPrefabIndex;
    }
	
    private void OnEnable()
    {
        AreaManager.OnAreaSpawn.AddListener(buildBackground);
    }

    private void OnDisable()
    {
        AreaManager.OnAreaSpawn.RemoveListener(buildBackground);
    }

    private static string getBackgroundKey()
    {
        string zoneKey = MapObjectList.getCurrentZoneKey();

        switch(zoneKey)
        {
            case ZoneKeyList.pit:
                return ZoneKeyList.mineLvl3;

            case ZoneKeyList.manseFirstFloor:
            case ZoneKeyList.manseSecondFloor:
                return ZoneKeyList.lovashiCamp;

            default:
                return zoneKey;
        }
    }

}
