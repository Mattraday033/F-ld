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
/*

	public Sprite backgroundTileSprite;
	public Color backgroundTint;
	
	public int numberOfDiagonals; //should be an odd number, because of how the diagonals are stacked and for consistance when wrapping around
	public int lengthOfDiagonals; //also starts counting at 0, can be whatever number it needs to be
	
	public GameObject backgroundGrid;
	
	private int frameCount = 0;
	
	private Vector3 farthestDiagonalPositionOddIndex;
	private Vector3 farthestDiagonalPositionEvenIndex;
	
	private static int sectionIncrement = 20;
	private static Vector3Int sectionDimensions = new Vector3Int(sectionIncrement,sectionIncrement,0);
	
	private TilemapDiagonal[] tilemapDiagonals;
	private Vector3[] diagonalPositions;
	
    // Start is called before the first frame update
    void Start()
    {
		if(numberOfDiagonals % 2 == 0)
		{
			numberOfDiagonals++;
		}			
		
		Tile tile = new Tile();
        tile.sprite = backgroundTileSprite;
		
		tilemapDiagonals = new TilemapDiagonal[numberOfDiagonals];
		diagonalPositions = new Vector3[numberOfDiagonals];
		
		for(int currentDiagonalIndex = 0; currentDiagonalIndex < tilemapDiagonals.Length; currentDiagonalIndex++)
		{
			tilemapDiagonals[currentDiagonalIndex] = ((GameObject) Instantiate(Resources.Load("TilemapDiagonal"), backgroundGrid.transform)).GetComponent<TilemapDiagonal>();
			Tilemap[] backgroundTilemaps = new Tilemap[lengthOfDiagonals];
			
			
			for(int currentBackgroundTilemapIndex = 0; currentBackgroundTilemapIndex < backgroundTilemaps.Length; currentBackgroundTilemapIndex++)
			{
				backgroundTilemaps[currentBackgroundTilemapIndex] = ((GameObject) Instantiate(Resources.Load("Tilemap"), tilemapDiagonals[currentDiagonalIndex].transform)).GetComponent<Tilemap>();
				
				backgroundTilemaps[currentBackgroundTilemapIndex].color = backgroundTint;
				backgroundTilemaps[currentBackgroundTilemapIndex].size = sectionDimensions;
				
				backgroundTilemaps[currentBackgroundTilemapIndex].origin += originOffsetFromTilemapNumber(currentBackgroundTilemapIndex);
				
				backgroundTilemaps[currentBackgroundTilemapIndex].ResizeBounds();
				backgroundTilemaps[currentBackgroundTilemapIndex].FloodFill(backgroundTilemaps[currentBackgroundTilemapIndex].origin, tile);
				
				
				Helpers.updateGameObjectPosition(backgroundTilemaps[currentBackgroundTilemapIndex].gameObject);
			}
			
			tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps = backgroundTilemaps;	
			
			tilemapDiagonals[currentDiagonalIndex].transform.position += positionOffsetFromDiagonalNumber(currentDiagonalIndex);
			
			diagonalPositions[currentDiagonalIndex] = tilemapDiagonals[currentDiagonalIndex].transform.position;
		}

		farthestDiagonalPositionEvenIndex = diagonalPositions[0] + farthestDriftAllowed(0);
		farthestDiagonalPositionOddIndex = diagonalPositions[1] + farthestDriftAllowed(1);

    }

    // Update is called once per frame
    void Update() //here for Animation
    {
		float delta = Time.deltaTime;

        frameCount++;
		//20
		if(frameCount % 20 == 0)
		{
			for(int currentDiagonalIndex = 0; currentDiagonalIndex < tilemapDiagonals.Length; currentDiagonalIndex++)
			{
				tilemapDiagonals[currentDiagonalIndex].transform.position += new Vector3(.25f * delta, .25f * delta, 0);
				
				Helpers.updateGameObjectPosition(tilemapDiagonals[currentDiagonalIndex].gameObject);
				
				
				if((currentDiagonalIndex % 2) == 0)
				{
					if(tilemapDiagonals[currentDiagonalIndex].transform.position.x >= farthestDiagonalPositionEvenIndex.x ||
							tilemapDiagonals[currentDiagonalIndex].transform.position.y >= farthestDiagonalPositionEvenIndex.y)
					{
						tilemapDiagonals[currentDiagonalIndex].transform.position = diagonalPositions[diagonalPositions.Length-1];
					}
				} else
				{
					if(tilemapDiagonals[currentDiagonalIndex].transform.position.x >= farthestDiagonalPositionOddIndex.x ||
						tilemapDiagonals[currentDiagonalIndex].transform.position.y >= farthestDiagonalPositionOddIndex.y)
					{
						tilemapDiagonals[currentDiagonalIndex].transform.position = diagonalPositions[diagonalPositions.Length-2];
					}
				}
			} 
			
			if(frameCount > 150000)
			{
				frameCount = 1;
			}
		}
    }
	
	private Vector3Int originOffsetFromTilemapNumber(int currentBackgroundTilemapIndex)
	{
		return (new Vector3Int(sectionIncrement*currentBackgroundTilemapIndex,sectionIncrement*(-1*currentBackgroundTilemapIndex),0));
	}
	
	private Vector3Int positionOffsetFromDiagonalNumber(int currentDiagonalIndex)
	{
		return (new Vector3Int(sectionIncrement*(-1*((currentDiagonalIndex)/2)),sectionIncrement*(-1*((currentDiagonalIndex+1)/2)),0));
	}
	
	private Vector3Int farthestDriftAllowed(int currentDiagonalIndex)
	{
		return (new Vector3Int(sectionIncrement*(((currentDiagonalIndex+1)/2)),sectionIncrement*(((currentDiagonalIndex)/2)),0));
	}
				if((currentDiagonalIndex % 2) == 0)
				{
					if(tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[0].origin.x >= farthestDiagonalPositionEvenIndex.x ||
						tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[0].origin.y >= farthestDiagonalPositionEvenIndex.y)
					{
						if(((tilemapDiagonals.Length-1) % 2) == 0)
						{
							for(int currentBackgroundTilemapIndex = 0; currentBackgroundTilemapIndex < tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps.Length; currentBackgroundTilemapIndex++)
							{
								tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[currentBackgroundTilemapIndex].transform.position = 
																			(originOffsetFromTilemapNumber(currentBackgroundTilemapIndex) + 
																			 originOffsetFromDiagonalNumber(tilemapDiagonals.Length-1));
					
								Helpers.updateGameObjectPosition(tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[currentBackgroundTilemapIndex].gameObject);
							}
						} else
						{
							for(int currentBackgroundTilemapIndex = 0; currentBackgroundTilemapIndex < tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps.Length; currentBackgroundTilemapIndex++)
							{
								tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[currentBackgroundTilemapIndex].transform.position = 
																			(originOffsetFromTilemapNumber(currentBackgroundTilemapIndex) + 
																			 originOffsetFromDiagonalNumber(tilemapDiagonals.Length-2));
					
								Helpers.updateGameObjectPosition(tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[currentBackgroundTilemapIndex].gameObject);
							}
						}
					}
				} else 
				{
					if(tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[0].origin.x >= farthestDiagonalPositionOddIndex.x ||
						tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[0].origin.y >= farthestDiagonalPositionOddIndex.y)
					{
						if(((tilemapDiagonals.Length-1) % 2) != 0)
						{
							for(int currentBackgroundTilemapIndex = 0; currentBackgroundTilemapIndex < tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps.Length; currentBackgroundTilemapIndex++)
							{
								tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[currentBackgroundTilemapIndex].transform.position = 
																			(originOffsetFromTilemapNumber(currentBackgroundTilemapIndex) + 
																			 originOffsetFromDiagonalNumber(tilemapDiagonals.Length-1));
					
								Helpers.updateGameObjectPosition(tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[currentBackgroundTilemapIndex].gameObject);
							}
						} else
						{
							for(int currentBackgroundTilemapIndex = 0; currentBackgroundTilemapIndex < tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps.Length; currentBackgroundTilemapIndex++)
							{
								tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[currentBackgroundTilemapIndex].transform.position = 
																			(originOffsetFromTilemapNumber(currentBackgroundTilemapIndex) + 
																			 originOffsetFromDiagonalNumber(tilemapDiagonals.Length-2));
					
								Helpers.updateGameObjectPosition(tilemapDiagonals[currentDiagonalIndex].backgroundTilemaps[currentBackgroundTilemapIndex].gameObject);
							}
						}
					}
				}*/
