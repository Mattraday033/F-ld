using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPosition : ScriptableObject
{
    public Vector3 oldPosition = new Vector3(38.38674f,-1.136966f,0.0f);
	
	public TransitionPosition(float x, float y, float z){
		oldPosition = new Vector3(x,y,z);
	}
	
	public TransitionPosition(Vector3 newPosition){
		oldPosition = newPosition;
	}
}
