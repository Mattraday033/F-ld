using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public enum StartingMenuState { OnMainMenu = 1, Loading = 2, CharacterCreation = 3 }

public class StartingMenuManager : MonoBehaviour 
{
	private StartingMenuState currentState;

	public GameObject mainMenuBackground; 

	private static StartingMenuManager instance; 

    public static StartingMenuManager getInstance()
    {
        return instance;
    }

    private void Awake()
    {
		if (!Application.isEditor)
		{
            GarbageCollector.GCMode = GarbageCollector.Mode.Manual;
        }

		if(instance != null)
		{
			Debug.LogError("Duplicate instances of StartingMenuManager exist erroneously");
		}

        instance = this;
    }

    void Update() //here for Key Input
	{
		KeyPressManager.updateKeyBools();

        if (KeyBindingList.eitherBackoutKeyIsPressed() && !KeyPressManager.handlingPrimaryKeyPress && !CharacterCreationPopUpWindow.inNameInputField())
		{
            KeyPressManager.handlingPrimaryKeyPress = true;

			handleESCPress();
		}
	}

	public void handleESCPress()
	{
        EscapeStack.escapeAll();

		revertToMainMenu();
    }

    public void revertToMainMenu() 
	{
		switch (currentState)
		{
			case StartingMenuState.OnMainMenu:
				return;
			case StartingMenuState.Loading:

                mainMenuBackground.SetActive(true);

                break;
            case StartingMenuState.CharacterCreation:

                mainMenuBackground.SetActive(true);
                break;
        }

		currentState = StartingMenuState.OnMainMenu;
	}
	public void setCurrentState(int newState)
    {
        if (newState <= 0 || newState > 3) 
        { 
            return;
        }

        currentState = (StartingMenuState) newState;
    }
} 
