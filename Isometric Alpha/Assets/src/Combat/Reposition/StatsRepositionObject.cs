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
		Combatant combatant = combatantToReposition.getCombatant();

		if(combatant == null)
		{
			return;
		}

		//the combatant is drawn in layers, so the body stands in for it on this single renderer
		SpriteRenderer combatantBodyRenderer = combatant.rendererList[SpriteLayer.Body];

		spriteRenderer.sprite = combatantBodyRenderer.sprite;
		spriteRenderer.color = combatantBodyRenderer.color;
	}
	
	public GameObject getGameObject()
	{
		return gameObject;
	}

}
