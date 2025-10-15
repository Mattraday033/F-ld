using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedMove : MonoBehaviour
{
	public Vector3 destination;
	
	public Collider2D collider;
	
	void Update() //Depricated
	{
		if(Helpers.hasCollision(collider) && (Helpers.getCollision(collider).gameObject.GetComponent<PlayerMovement>() != null ||
												!(Helpers.getCollision(collider).gameObject.GetComponent<PlayerMovement>() is null)))
		{
			
			PlayerMovement.getInstance().gameObject.transform.localPosition = destination;
			
		}
	}
	
}
