using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RepositionPlaceholderGenerator
{
	public const string placeHolderObjectGOName = "RepositionPlaceholder";
	public const float placeHolderSpriteOpaqueness = .4f;

	public static GameObject generatePlaceholderObject(Stats combatantToBeMoved, GridCoords placeHolderPosition)
	{
		GameObject placeHolderObject = GameObject.Instantiate(Resources.Load<GameObject>(placeHolderObjectGOName));
		
		SpriteRenderer placeHolderSprite = placeHolderObject.GetComponent<SpriteRenderer>();
		SpriteRenderer combatantToBeMovedSprite = combatantToBeMoved.combatSprite.GetComponent<SpriteRenderer>();
		
		placeHolderSprite.sprite = combatantToBeMovedSprite.sprite;
		placeHolderSprite.flipX = combatantToBeMovedSprite.flipX;
		placeHolderSprite.flipY = combatantToBeMovedSprite.flipY;
		Color placeHolderColor = combatantToBeMovedSprite.color;
		placeHolderColor.a = placeHolderSpriteOpaqueness;
		placeHolderSprite.color = placeHolderColor;
		
		placeHolderObject.transform.position = CombatGrid.fullCombatGrid[placeHolderPosition.row][placeHolderPosition.col] + combatantToBeMoved.adjustment;
		placeHolderObject.transform.localScale = combatantToBeMoved.combatSprite.transform.localScale;
		Helpers.updateGameObjectPosition(placeHolderObject);
		
		return placeHolderObject;
	}

}
