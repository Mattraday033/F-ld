using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manse2FStockroomButtonEvaluationScript : MonoBehaviour, IButtonEvaluationScript
{
	public GameObject pillarOne;
	public GameObject pillarTwo;

	public bool buttonThreeWasPressed = false;
	
	public const int buttonOne = 0;
	public const int buttonTwo = 1;
	public const int buttonThree = 2;

	public void evaluate(FloorButtonTrueFalse[] buttons)
	{
		if (buttons[buttonThree].isPressed() || buttonThreeWasPressed)
		{
			if (!buttonThreeWasPressed)
			{
				buttonThreeWasPressed = true;
				pillarTwo.SetActive(false);
				pillarOne.SetActive(false);
			}
		}
		else
		{
			if (buttons[buttonOne].isPressed())
			{
				pillarOne.SetActive(false);
			}
			else
			{
				pillarOne.SetActive(true);
			}

			if (buttons[buttonTwo].isPressed())
			{
				pillarTwo.SetActive(false);
			}
			else
			{
				pillarTwo.SetActive(true);
			}
		}
	}


}
