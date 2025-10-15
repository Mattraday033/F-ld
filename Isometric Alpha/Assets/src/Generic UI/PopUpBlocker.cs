using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PopUpBlocker
{
	private static Transform popUpParent;
    private static GameObject popUpScreenBlockerGameObject;


	public static void declarePopUpParent(Transform newPopUpParent)
	{
		popUpParent = newPopUpParent;
	}

	public static void spawnPopUpScreenBlocker()
	{
		if (!popUpScreenBlockerGameObjectExists())
		{
			popUpScreenBlockerGameObject = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.popUpScreenBlocker), getPopUpParent());
		}
	}
	
	public static bool popUpScreenBlockerGameObjectExists()
	{
		return popUpScreenBlockerGameObject != null;
	}

	public static Transform getPopUpParent()
	{
		return popUpParent;
	}

	public static Transform getPopUpParent(PopUpType type)
	{
		if(type == PopUpType.LoadOnlyScreen)
		{
			return OverallUIManager.screenBackground;
		}

		return popUpParent;
	}

	public static void destroyPopUpScreenBlocker()
	{
		if (popUpScreenBlockerGameObject && TutorialSequence.canDestroyTutorialMessageWindows())
		{
			GameObject.DestroyImmediate(popUpScreenBlockerGameObject);
		}
	}

}
