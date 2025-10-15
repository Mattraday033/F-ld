using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupParentDeclarer : MonoBehaviour
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
		PopUpBlocker.declarePopUpParent(transform);
	}
}
