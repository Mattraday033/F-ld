using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonTrueFalse : MonoBehaviour, IFloorButton
{

	private CapsuleCollider2D collider;
	
	private SpriteRenderer spriteRenderer;
	
	public bool deactivateTargetOnPress;
	
	public ContactFilter2D filterPlayer;
	public ContactFilter2D filterMovableObject;
	public ContactFilter2D filterNPC;
	
	private bool pressed;
	
	void Start()
	{
		collider = gameObject.GetComponent<CapsuleCollider2D>();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.color = Color.white;
		
		declareButton();
	}

    public void evaluate()
    {
		if(Helpers.hasCollision(collider, filterPlayer) || Helpers.hasCollision(collider, filterMovableObject) ||
			Helpers.hasCollision(collider, filterNPC))
		{
			spriteRenderer.color = Color.green;
			pressed = true;
			
		} else
		{
			spriteRenderer.color = Color.white;
			pressed = false;
		}
    }
	
	public void declareButton()
	{
		MovementManager.getInstance().addFloorButton(this);
	}
	
	public bool isPressed()
	{
		return pressed;
	}
	
}
