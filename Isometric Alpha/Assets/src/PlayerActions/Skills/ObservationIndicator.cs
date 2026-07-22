using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservationIndicator : SkillIndicator
{
	public void detectObservableObject()
    {
		if(Helpers.hasCollision(collider, LayerAndTagManager.observableLayerMask))
		{
            GameObject observedObj = Helpers.getCollision(collider, LayerAndTagManager.observableLayerMask).gameObject;

			if(observedObj.CompareTag(LayerAndTagManager.observableTag))
			{
				observedObj.GetComponent<ObservableObject>().markAsObserved();
				disableSelf(true);
			} 			
			
		} else
		{
			disableSelf(false);
		}
    }

}
