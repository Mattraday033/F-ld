using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMultiInvertButton : MonoBehaviour, IFloorButton
{

	private CapsuleCollider2D collider;
	
	public bool shouldUpdate = false;
	public bool steppedOff = true;
	
	public ContactFilter2D filterPlayer;
	public ContactFilter2D filterMovableObject;
	
	public ButtonTarget[] targets;
	
	void Start()
	{
		collider = gameObject.GetComponent<CapsuleCollider2D>();
		gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		
		declareButton();
	}

	public void evaluate()
    {
		if(Helpers.hasCollision(collider, filterPlayer) || Helpers.hasCollision(collider, filterMovableObject))
		{
			gameObject.GetComponent<SpriteRenderer>().color = Color.green;
			if(steppedOff)
			{
				shouldUpdate = true;
			}
		} else
		{ 
			if(gameObject.GetComponent<SpriteRenderer>().color != Color.white)
			{
				gameObject.GetComponent<SpriteRenderer>().color = Color.white;
			}
			steppedOff = true;
		}
		
		if(shouldUpdate && steppedOff)
		{
			flipTargets();
			shouldUpdate = false;
			steppedOff = false;
		}
		
    }
	
	public void flipTargets()
	{
		foreach(ButtonTarget target in targets)
		{
			target.flip();
		}
	}
	
	public void declareButton()
	{
		MovementManager.getInstance().addFloorButton(this);
	}
}
