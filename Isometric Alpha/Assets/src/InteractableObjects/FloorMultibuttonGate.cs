using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMultibuttonGate: MonoBehaviour, IFloorButton
{
	public GameObject[] buttons;
	public GateSpawnChecker gateSpawnChecker;

	void Start()
	{
		declareButton();
	}

	public void evaluate()
    {
        if(allButtonsPressed())
		{
			gateSpawnChecker.setToOpenedPermanently();
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			gameObject.GetComponent<Collider2D>().enabled = false;
		} 
    }
	
	public bool allButtonsPressed()
	{
		foreach(GameObject button in buttons)
		{
			if(!button.GetComponent<FloorMultibutton>().pressedDown)
			{
				return false;
			}
		}
		return true;
	}
	
	public void declareButton()
	{
		MovementManager.getInstance().addFloorButton(this);
	}
}
