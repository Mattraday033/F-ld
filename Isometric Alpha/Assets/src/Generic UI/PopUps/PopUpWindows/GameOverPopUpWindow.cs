using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverPopUpWindow : PopUpWindow
{
    private static GameOverPopUpWindow instance;

    public static GameOverPopUpWindow getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Duplicate Instances of GameOverPopUpWindow exist erroneously.");
        }

        instance = this;
    }


    public override void destroyWindow()
	{
        //empty on purpose
	}

	public override void acceptButtonPress()
	{
        //empty on purpose
    }

    public override void closeButtonPress()
	{
        //empty on purpose
    }

    public override void handleEscapePress()
	{
        //empty on purpose		
    }
}
