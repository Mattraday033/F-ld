using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkObservableObject : MonoBehaviour
{
	public ContactFilter2D filterObservable;
	
	public Collider2D collider;

	public void detectObservableObject()
    {
		if(Helpers.hasCollision(collider))
		{
			GameObject observedObj = Helpers.getCollision(collider).gameObject;
			
			if(observedObj.CompareTag("Observable"))
			{
				observedObj.GetComponent<ObservableObject>().markAsObserved();
				disableSelf(true);
			} 			
			
		} else
		{
			disableSelf(false);
		}
			
		
    }
	
	private void disableSelf(bool deactivate)
	{
		if(deactivate)
		{
			gameObject.SetActive(false);
		} else
		{
			gameObject.GetComponent<MarkObservableObject>().enabled = false;
		}
		
	}
	

	
	private void disableSelf()
	{
		
	}
	
}
