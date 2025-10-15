using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manse1F2CButtonEvaluationScript : MonoBehaviour, IButtonEvaluationScript
{
	public GameObject pillarOne;
	
	public const int buttonOne = 0;
	
	public void evaluate(FloorButtonTrueFalse[] buttons)
	{
		if(buttons[buttonOne].isPressed())
		{
			pillarOne.SetActive(false);
		} 
	}

}
