using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SideScreenButtonManager : MonoBehaviour
{
	private static SideScreenButtonManager instance;

	public void setCurrentScreenType(int screenType)
	{
		setCurrentScreenType((ScreenType)screenType);
	}

    public void setCurrentScreenType(ScreenType screenType)
    {
        OverallUIManager.changeScreen(screenType);
        
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inUI);
	}

	public static SideScreenButtonManager getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("Duplicate instances of SideScreenButtonManager exist");
		}
		
		instance = this;
	}
}
