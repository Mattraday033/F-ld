using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorParentDeclarer : MonoBehaviour
{
	private void Awake()
	{
		CombatUI.selectorParent = transform;
	}
}
