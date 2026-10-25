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

    //In WASD order. The binds themselves rather than copies of their keys, so a rebind is picked up at once.
    private static readonly KeyBind[] movementKeyBinds = new KeyBind[]
    {
        KeyBindingList.moveNorthKey,
        KeyBindingList.moveWestKey,
        KeyBindingList.moveSouthKey,
        KeyBindingList.moveEastKey
    };

	public static KeyCode getFirstMovementKeyPressedDetectedInWASDOrder()
	{
        foreach(KeyBind keyBind in movementKeyBinds)
        {
            if(InputReader.isHeld(keyBind))
            {
                return keyBind.getCurrentKeyCode();
            }
        }

        return KeyCode.None;
    }

    public static KeyCode getFirstNonBarredMovementKeyPressedDetectedInWASDOrder(List<KeyCode> barredMovementKeyCodes)
    {
        foreach (KeyBind keyBind in movementKeyBinds)
        {
            KeyCode keyCode = keyBind.getCurrentKeyCode();

            if (InputReader.isHeld(keyBind) && !barredMovementKeyCodes.Contains(keyCode))
            {
                return keyCode;
            }
        }

        //None, which handleWASDMovement treats as "don't move". This used to return KeyCode.Z, which
        //slipped past that check.
        return KeyCode.None;
    }

    public static KeyCode getFirstMovementKeyPressedDetectedInWASDOrderSkippingGivenKey(KeyCode givenKey)
    {
        foreach (KeyBind keyBind in movementKeyBinds)
        {
            KeyCode keyCode = keyBind.getCurrentKeyCode();

            if (InputReader.isHeld(keyBind) && givenKey != keyCode)
            {
                return keyCode;
            }
        }

        return KeyCode.None;
    }

    public static int numberOfMovementKeysPressed()
    {
		int movementKeysPressed = 0;

		foreach(KeyBind keyBind in movementKeyBinds)
		{
			if(InputReader.isHeld(keyBind))
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

        switch (PlayerStateManager.currentActivity)
        {
            case CurrentActivity.Walking:
            case CurrentActivity.InDialogue:
                return InputReader.isHeld(KeyBindingList.revealKey) || InputReader.isHeld(KeyBindingList.hideTerrainKey);
            case CurrentActivity.InUI:
            case CurrentActivity.InMap:
            case CurrentActivity.Cunning:
            case CurrentActivity.Observing:
            case CurrentActivity.Intimidating:
            case CurrentActivity.InChestUI:
            case CurrentActivity.InBookUI:
            case CurrentActivity.InShopUI:
            case CurrentActivity.InDialoguePopUp:
            case CurrentActivity.InLevelUpPopUp:
            case CurrentActivity.InTutorialPopUp:
            case CurrentActivity.InTutorialSequence:
                return false;

            default:
                return false;
        }
    }


    //Polled from the Update methods of the screens that take input, so the debounce flags clear at the same
    //point in the frame they always have.
    public static void updateKeyBools()
    {
        if (!InputReader.anyButtonHeld())
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
        //Loading Config sets each KeyBind's key, and the KeyBinds rebind their actions, so it no longer
        //matters whether this runs before or after PlayerInputList is first touched.
        Config.initializeSettingsFromConfigFile();
    }

}
