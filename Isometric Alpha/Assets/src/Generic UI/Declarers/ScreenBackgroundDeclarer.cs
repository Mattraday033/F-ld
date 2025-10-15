using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBackgroundDeclarer : MonoBehaviour
{

	private void Awake()
	{
		declareScreenBackground();		
	}
	
	private void OnEnable()
	{
		declareScreenBackground();
	}

	private void declareScreenBackground()
	{
		OverallUIManager.screenBackground = transform;
	}

}
