using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHoverParentDeclarer : MonoBehaviour
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
