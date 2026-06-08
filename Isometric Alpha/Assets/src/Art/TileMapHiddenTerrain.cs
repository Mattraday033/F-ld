using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct TileMapEdits
{
    public Vector3Int cellCoords;
    public string secretDoorFlag;
    public string spriteName;

    public void applyEdit(Tilemap tilemap)
    {
        if(tilemap == null)
        {
            return;
        }

        if(spriteName == null || spriteName.Length <= 0)
        {
            tilemap.SetTile(cellCoords, null);
        } else
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();

            tile.sprite = Helpers.loadSpriteFromResources(spriteName);
            tilemap.SetTile(cellCoords, tile);
        }
    }
}

public class TileMapHiddenTerrain : MonoBehaviour
{

    public Tilemap tilemap;

    public List<TileMapEdits> tileMapEdits;

    private void OnEnable()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.AddListener(applyEditsForSecretDoor);
    }

    private void OnDisable()
    {
        SecretDoorFlags.OnSecretDoorDiscovery.RemoveListener(applyEditsForSecretDoor);
    }

    private void Start()
    {
        List<string> secretDoorFlags = new List<string>();

        foreach(TileMapEdits edit in tileMapEdits)
        {
            if(!secretDoorFlags.Contains(edit.secretDoorFlag))
            {
                applyEditsForSecretDoor(edit.secretDoorFlag);
                secretDoorFlags.Add(edit.secretDoorFlag);
            }
        }
    }

    private void applyEditsForSecretDoor(string secretDoorFlag)
    {
        foreach(TileMapEdits tileMapEdit in tileMapEdits)
        {
            if(SecretDoorFlags.secretDoorHasBeenDiscovered(tileMapEdit.secretDoorFlag))
            {
                tileMapEdit.applyEdit(tilemap);
            }
        }
    }

}
