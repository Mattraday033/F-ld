using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationParentDeclarer : MonoBehaviour
{
	private void Awake()
	{
		declareUICanvas();
	}
	
	private void OnEnable()
	{
		declareUICanvas();
	}

	private void declareUICanvas()
	{
		OverallUIManager.notificationParent = transform;
	}
}
