using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDiagonal : MonoBehaviour
{

	public Tilemap[] backgroundTilemaps;
	
	public TilemapDiagonal(Tilemap[] backgroundTilemaps)
	{
		this.backgroundTilemaps = backgroundTilemaps;
	}
	
}
