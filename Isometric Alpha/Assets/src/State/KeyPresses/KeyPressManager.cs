using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyPressManager
{
    private static bool _HandlingPrimaryKeyPress = false;
    public static bool handlingPrimaryKeyPress
    {
        get => _HandlingPrimaryKeyPress;
        set
        {
           _HandlingPrimaryKeyPress = value;
        } 
    }

    public static bool _HandlingSecondaryKeyPress = false;
    public static bool handlingSecondaryKeyPress
    {
        get => _HandlingSecondaryKeyPress;
        set => _HandlingSecondaryKeyPress = value;
    }


    public static KeyCode[] movementKeys;


	public static KeyCode getFirstMovementKeyPressedDetectedInWASDOrder()
	{
        foreach(KeyCode keyCode in movementKeys)
        {
            if(Input.GetKey(keyCode))
            {
                return keyCode;
            }
        }

        return KeyCode.None;
    }

    public static KeyCode getFirstNonBarredMovementKeyPressedDetectedInWASDOrder(List<KeyCode> barredMovementKeyCodes)
    {
        foreach (KeyCode keyCode in movementKeys)
        {
            if (Input.GetKey(keyCode) && !barredMovementKeyCodes.Contains(keyCode))
            {
                return keyCode;
            }
        }

        return KeyCode.Z;
    }

    public static KeyCode getFirstMovementKeyPressedDetectedInWASDOrderSkippingGivenKey(KeyCode givenKey)
    {
        foreach (KeyCode keyCode in movementKeys)
        {
            if (Input.GetKey(keyCode) && givenKey != keyCode)
            {
                return keyCode;
            }
        }

        return KeyCode.None;
    }

    public static int numberOfMovementKeysPressed()
    {
		int movementKeysPressed = 0;

		foreach(KeyCode keyCode in movementKeys)
		{
			if(Input.GetKey(keyCode))
			{
				movementKeysPressed++;
            }
		}

		return movementKeysPressed; 
    }

    public static bool secondaryKeyPressed()
    {
        if (CombatStateManager.inCombat)
        {
            return KeyBindingList.movementKeyPressed();
        }

        switch (PlayerOOCStateManager.currentActivity)
        {
            case OOCActivity.walking:
            case OOCActivity.inDialogue:
                return Input.GetKey(KeyBindingList.revealKey.getCurrentKeyCode()) || Input.GetKey(KeyBindingList.hideTerrainKey.getCurrentKeyCode());
            case OOCActivity.inUI:
            case OOCActivity.inMap:
            case OOCActivity.cunning:
            case OOCActivity.observing:
            case OOCActivity.intimidating:
            case OOCActivity.inChestUI:
            case OOCActivity.inBookUI:
            case OOCActivity.inShopUI:
            case OOCActivity.inDialoguePopUp:
            case OOCActivity.inLevelUpPopUp:
            case OOCActivity.inTutorialPopUp:
            case OOCActivity.inTutorialSequence:
                return false;

            default:
                return false;
        }
    }


    public static void updateKeyBools()
    {
        if (!Input.anyKey)
        {
            handlingPrimaryKeyPress = false;
            handlingSecondaryKeyPress = false;
        }
        else if (!secondaryKeyPressed())
        {
            handlingSecondaryKeyPress = false;
        }
    }
	
    [RuntimeInitializeOnLoadMethod]
    private static void initializeKeyPressManager()
    {
        Config.initializeSettingsFromConfigFile();

        movementKeys = new KeyCode[] { 
                                        KeyBindingList.moveNorthKey.getCurrentKeyCode(), 
                                        KeyBindingList.moveWestKey.getCurrentKeyCode(), 
                                        KeyBindingList.moveSouthKey.getCurrentKeyCode(), 
                                        KeyBindingList.moveEastKey.getCurrentKeyCode() 
                                    };      

        KeyBindingSettingsManager.EnableAllKeyBindButtons.AddListener(initializeKeyPressManager);  
    }

}
