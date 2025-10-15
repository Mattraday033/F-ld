using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMultibuttonTarget : MonoBehaviour, IFloorButton
{
	public GameObject[] buttons;

	void Start()
	{
		declareButton();
	}

	public void evaluate()
    {
        if(allButtonsPressed())
		{
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			gameObject.GetComponent<Collider2D>().enabled = false;
		} else
		{
			gameObject.GetComponent<SpriteRenderer>().enabled = true;
			gameObject.GetComponent<Collider2D>().enabled = true;
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
