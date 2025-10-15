using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFloorButtonScript
{
	public bool hasBeenActivated();
	
	public void declareHasBeenActivated();
	
	public void activate();
}
