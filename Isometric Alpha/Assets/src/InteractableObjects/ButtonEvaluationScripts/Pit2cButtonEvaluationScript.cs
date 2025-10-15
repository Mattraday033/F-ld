using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit2cButtonEvaluationScript : MonoBehaviour, IButtonEvaluationScript
{
	public GameObject pillarOne;
	public GameObject pillarTwo;
	public GameObject pillarThree;
	public GameObject pillarFour;
	
	public const int buttonOne = 0;
	public const int buttonTwo = 1;
	public const int buttonThree = 2;
	public const int buttonFour = 3;
	
	public void evaluate(FloorButtonTrueFalse[] buttons)
	{
		if(buttons[buttonOne].isPressed())
		{
			pillarOne.SetActive(false);
		} else
		{
			pillarOne.SetActive(true);
		}
		
		if(buttons[buttonTwo].isPressed())
		{
			pillarTwo.SetActive(false);
		} else
		{
			pillarTwo.SetActive(true);
		}
		
		if(buttons[buttonThree].isPressed())
		{
			pillarThree.SetActive(false);
		} else
		{
			pillarThree.SetActive(true);
		}
		
		if(buttons[buttonFour].isPressed())
		{
			pillarFour.SetActive(false);
		} else
		{
			pillarFour.SetActive(true);
		}
	}


}
