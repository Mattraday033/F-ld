using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyPressManager
{
	public static bool handlingPrimaryKeyPress = false;
    public static bool handlingSecondaryKeyPress = false;

    public static KeyCode[] WASDKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    public static bool WOnlyKeyPressed()
    {
        return Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D);
    }
	
	public static bool AOnlyKeyPressed()
	{
		return !Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D);
	}
	
	public static bool SOnlyKeyPressed()
	{
		return !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D);
	}
	
	public static bool DOnlyKeyPressed()
	{
		return !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D);
	}

	public static KeyCode getFirstMovementKeyPressedDetectedInWASDOrder()
	{
        foreach(KeyCode keyCode in WASDKeys)
        {
            if(Input.GetKey(keyCode))
            {
                return keyCode;
            }
        }

        return KeyCode.None;
    }

    public static KeyCode getFirstNonBarredMovementKeyPressedDetectedInWASDOrder(ArrayList barredMovementKeyCodes)
    {
        foreach (KeyCode keyCode in WASDKeys)
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
        foreach (KeyCode keyCode in WASDKeys)
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

		foreach(KeyCode keyCode in WASDKeys)
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
            return false;
        }

        switch (PlayerOOCStateManager.currentActivity)
        {
            case OOCActivity.walking:
                return KeyBindingList.revealKeyIsPressed() || Input.GetKey(KeyBindingList.hideTerrainKey);

            case OOCActivity.inDialogue:
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
	
}
