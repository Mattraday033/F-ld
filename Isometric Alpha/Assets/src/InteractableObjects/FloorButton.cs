using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFloorButton
{
	
	public void evaluate();
	public void declareButton();
	
}

public class FloorButton : MonoBehaviour, IFloorButton
{

	private CapsuleCollider2D collider;
	
	public bool deactivateTargetOnPress;
	
	public ContactFilter2D filterPlayer;
	public ContactFilter2D filterMovableObject;
	public ContactFilter2D filterNPC;
	
	public GameObject target;
	
	void Start()
	{
		collider = gameObject.GetComponent<CapsuleCollider2D>();
		gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		
		declareButton();
	}

	public void declareButton()
	{
		MovementManager.getInstance().addFloorButton(this);
	}

    public void evaluate()
    {
		if(Helpers.hasCollision(collider, filterPlayer) || Helpers.hasCollision(collider, filterMovableObject) ||
			Helpers.hasCollision(collider, filterNPC))
		{
			gameObject.GetComponent<SpriteRenderer>().color = Color.green;
			
			if(deactivateTargetOnPress)
			{
				target.GetComponent<ButtonTarget>().deactivate();
			}
			
		} else if(gameObject.GetComponent<SpriteRenderer>().color != Color.white)
		{
			gameObject.GetComponent<SpriteRenderer>().color = Color.white;
			
			if(deactivateTargetOnPress)
			{
				target.GetComponent<ButtonTarget>().activate();
			}
		}
    }
	
}
