using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsRepositionObject : MonoBehaviour
{

	public Stats combatantToReposition;
	
	public SpriteRenderer spriteRenderer;
	
	public void setCombatantToReposition(Stats stats)
	{
		combatantToReposition = stats;
	}
	
	public Stats getCombatantToReposition()
	{
		return combatantToReposition;
	}
	
	public void setSprite()
	{
		SpriteRenderer combatantSpriteRenderer = Resources.Load<GameObject>(combatantToReposition.combatSpriteName).GetComponent<SpriteRenderer>();
		
		spriteRenderer.sprite = combatantSpriteRenderer.sprite;
		spriteRenderer.color = combatantSpriteRenderer.color;
	}
	
	public GameObject getGameObject()
	{
		return gameObject;
	}

}
