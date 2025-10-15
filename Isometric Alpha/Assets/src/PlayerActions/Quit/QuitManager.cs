using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitManager : MonoBehaviour
{

	public void quitButtonPress()  
	{
		Debug.Log("Quitting");


        Application.Quit();
	}
}
