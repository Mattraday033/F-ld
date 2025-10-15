using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manse2F3aButtonEvaluationScript : MonoBehaviour, IButtonEvaluationScript
{
	public GameObject pillarOne;
	public GameObject pillarTwo;
	public GameObject pillarThree;
	public GameObject pillarFour;
	
	public bool pillarThreePermaDown = false;
	
	public const int buttonOne = 0;
	public const int buttonTwo = 1;
	public const int buttonThree = 2;
	public const int buttonFour = 3;
	public const int buttonFive = 4;
	public const int buttonSix = 5;
	
	public void evaluate(FloorButtonTrueFalse[] buttons)
	{
		if(buttons[buttonTwo].isPressed())
		{
			pillarOne.SetActive(false);
			pillarTwo.SetActive(false);
			pillarThree.SetActive(false);
			return;
		} else
		{
			pillarOne.SetActive(true);
			pillarTwo.SetActive(true);
			
			if(!pillarThreePermaDown)
			{
				pillarThree.SetActive(true);
			}
		}
		
		if(buttons[buttonFour].isPressed())
		{
			pillarThreePermaDown = true;
			pillarThree.SetActive(false);
		}
		
		/*
		if(buttons[buttonFive].isPressed())
		{
			pillarOne.SetActive(false);
			pillarTwo.SetActive(false);
		}*/
		
		if(buttons[buttonOne].isPressed())
		{
			pillarOne.SetActive(false);
			pillarTwo.SetActive(true);
		} else
		{
			pillarOne.SetActive(true);
			pillarTwo.SetActive(false);
		}
		
		if(buttons[buttonSix].isPressed())
		{
			pillarFour.SetActive(false);
		} else
		{
			pillarFour.SetActive(true);
		}
		
	}


}
