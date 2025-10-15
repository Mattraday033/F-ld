using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMultibutton : MonoBehaviour, IFloorButton
{
	private CapsuleCollider2D collider;
	
	public bool pressedDown = false;
	
	void Start()
	{

		collider = gameObject.GetComponent<CapsuleCollider2D>();
		gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		
		declareButton();
	}

	public void evaluate()
    {
		if (Helpers.getCollision(collider, LayerAndTagManager.pressesButtonsLayerMask) != null)
		{
			gameObject.GetComponent<SpriteRenderer>().color = Color.green;
			pressedDown = true;

		}
		else
		{
			if (gameObject.GetComponent<SpriteRenderer>().color != Color.white)
			{
				gameObject.GetComponent<SpriteRenderer>().color = Color.white;
			}

			pressedDown = false;
		}
    }

	public void declareButton()
	{
		MovementManager.getInstance().addFloorButton(this);
	}
}
