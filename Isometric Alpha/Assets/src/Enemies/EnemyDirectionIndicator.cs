using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDirectionIndicator : MonoBehaviour
{	
	public Sprite northEastArrow;
	public Sprite southEastArrow;
	public Sprite southWestArrow;
	public Sprite northWestArrow;
	
	public SpriteRenderer tileSpriteRenderer;
	public SpriteRenderer arrowIconSpriteRenderer;
	
	public void setArrowDirection(CharacterFacing enemyFacing)
	{
		switch (enemyFacing.getFacing())
		{
			case Facing.NorthEast:
                arrowIconSpriteRenderer.sprite = northEastArrow;
                return;
            case Facing.SouthEast:
                arrowIconSpriteRenderer.sprite = southEastArrow;
                return;
            case Facing.SouthWest:
                arrowIconSpriteRenderer.sprite = southWestArrow;
                return;
            case Facing.NorthWest:
                arrowIconSpriteRenderer.sprite = northWestArrow;
                return;
			default:
				throw new IOException("Unknown facing: " + enemyFacing.getFacing().ToString());
        }
	}
	
	public void setColors(Color newColor)
	{
		tileSpriteRenderer.color = newColor;
		arrowIconSpriteRenderer.color = newColor;
	}

}
