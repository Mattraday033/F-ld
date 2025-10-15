using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParentDeclarer : MonoBehaviour
{
	public GameObject UIParentPanel;

	public Canvas canvas;

	private void Awake()
	{
		declareUICanvas();

		canvas.worldCamera = Camera.main;
		canvas.sortingLayerName = "DialogueBox";
	}
	
	private void OnEnable()
	{
		declareUICanvas();
	}

	private void declareUICanvas()
	{
		OverallUIManager.UIParentPanel = UIParentPanel;
	}
}
