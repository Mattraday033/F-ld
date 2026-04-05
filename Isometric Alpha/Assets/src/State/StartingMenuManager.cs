using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.EventSystems;

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

        if (SaveHandler.getInstance() != null) 
        {
            if (!SaveHandler.saveNameFieldIsSelected() && KeyBindingList.settingsScreenOrBackKeyPressed() && !KeyPressManager.handlingPrimaryKeyPress)
            {
                KeyPressManager.handlingPrimaryKeyPress = true;

                handleESCPress();
                return;
            } 
            else if(SaveHandler.saveNameFieldIsSelected() && Input.GetKey(KeyBindingList.settingsScreenKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
            {
                KeyPressManager.handlingPrimaryKeyPress = true;

                EventSystem.current.SetSelectedGameObject(null);
                handleESCPress();
                return;
            }
        } 

        if (CharacterCreationPopUpWindow.getInstanceCC() != null && KeyBindingList.settingsScreenOrBackKeyPressed() && !KeyPressManager.handlingPrimaryKeyPress && !CharacterCreationPopUpWindow.inNameInputField())
		{
            KeyPressManager.handlingPrimaryKeyPress = true;

			handleESCPress();
            return;
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
