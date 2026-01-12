using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class ScreenButtonManager : MonoBehaviour
{
	public Button[] screenButtons;
	
	private static ScreenButtonManager instance;

	public void setCurrentScreenType(int screenType)
	{
		OverallUIManager.changeScreen((ScreenType) screenType);
	}

	public static void setCurrentScreenButton(ScreenType screen)
	{
		setCurrentScreenButton(((int) screen) - 1);
	}

	public static void setCurrentScreenButton(int buttonIndex)
	{
		if(instance == null)
		{
			return;
		}
		
		for(int currentIndex = 0; currentIndex < getInstance().screenButtons.Length; currentIndex++)
		{			
			getInstance().screenButtons[currentIndex].interactable = true;
		}
		
		getInstance().screenButtons[buttonIndex].interactable = false;
	}

	public static ScreenButtonManager getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("Duplicate instances of ScreenButtonManager exist");
		}
		
		if(Flags.getFlag("newGame") || CombatStateManager.inCombat)
		{
			gameObject.SetActive(false);
		} else
		{
			instance = this;
		}
	}

}
