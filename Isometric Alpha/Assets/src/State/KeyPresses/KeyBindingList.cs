using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class KeyBindingList
{
    //Movement KeyBinds
    public const KeyCode moveNorthKey = KeyCode.W;
    public const KeyCode moveEastKey = KeyCode.D;
    public const KeyCode moveSouthKey = KeyCode.S;
    public const KeyCode moveWestKey = KeyCode.A;
    public static bool movementKeyPressed()
    {
        return Input.GetKey(moveNorthKey) ||
                Input.GetKey(moveWestKey) ||
                Input.GetKey(moveSouthKey) ||
                Input.GetKey(moveEastKey);
    }
    public static bool noMovementKeyPressed()
    {
        return !Input.GetKey(moveNorthKey) &&
                !Input.GetKey(moveWestKey) &&
                !Input.GetKey(moveSouthKey) &&
                !Input.GetKey(moveEastKey);
    }

    //General Walking Keys
    public const KeyCode interactKey = KeyCode.E;
    public const KeyCode hideTerrainKey = KeyCode.F;
    public const KeyCode removePlacedCompanionMoveableObjectKey = KeyCode.Z;
    public const KeyCode mapKey = KeyCode.M;
    public const KeyCode transcriptKey = KeyCode.T;
    public const KeyCode showHideKeyBindingsListKey = KeyCode.Space;
    public const KeyCode quicksaveKey = KeyCode.Q;

    public static bool revealKeyIsPressed()
    {
        return eitherShiftKeyIsPressed();
    }

    //UI Navigation Keys
    public const KeyCode backOutKey1 = KeyCode.Escape;
    public const KeyCode backOutKey2 = KeyCode.R;
    public const KeyCode moveLeftKey = KeyCode.A;
    public const KeyCode moveCounterClockwiseKey = KeyCode.A;
    public const KeyCode moveRightKey = KeyCode.D;
    public const KeyCode moveClockwiseKey = KeyCode.D;
    public const KeyCode acceptInputKey = KeyCode.Return;

    public static bool eitherShiftKeyIsPressed()
    {
        return Input.GetKey(KeyCode.LeftShift) ||
                Input.GetKey(KeyCode.RightShift);
    }

    public static bool eitherControlKeyIsPressed()
    {
        return Input.GetKey(KeyCode.LeftControl) ||
                Input.GetKey(KeyCode.RightControl);
    }

    public static bool continueUIKeyIsPressed()
    {
        return Input.GetKey(KeyCode.E) ||
                Input.GetKey(KeyCode.Space) ||
                Input.GetKey(KeyCode.Return);
    }

    public static bool eitherAltKeyIsPressed()
    {
        return Input.GetKey(KeyCode.LeftAlt) ||
                Input.GetKey(KeyCode.RightAlt);
    }

    public static bool eitherBackoutKeyIsPressed()
    {
        return Input.GetKey(backOutKey1) ||
                (Input.GetKey(backOutKey2) && !SaveHandler.saveNameFieldIsSelected());
    }


    //Combat Keys
    public const KeyCode resolveTurnKey = KeyCode.Space;
    public const KeyCode combatAcceptChoiceKey = KeyCode.E;

    public static bool jumpMoveKeyIsPressed()
    {
        return eitherShiftKeyIsPressed();
    }

    public static bool skipTutorialKeysArePressed()
    {
        return (Input.GetKey(KeyCode.LeftShift) ||
                Input.GetKey(KeyCode.RightShift)) &&
                Input.GetKey(KeyCode.Escape);
    }


    //Skill KeyBinds
    public const KeyCode intimidateKey = KeyCode.Alpha1;
    public const KeyCode cunningKey = KeyCode.Alpha2;
    public const KeyCode observationKey = KeyCode.Alpha3;
    public const KeyCode placeCompanionKey = KeyCode.Alpha4;

    //Screen KeyBinds
    public const KeyCode lastScreenKey = KeyCode.Tab;
    public const KeyCode characterScreenKey = KeyCode.C;
    public const KeyCode inventoryScreenKey = KeyCode.I;
    public const KeyCode partyScreenKey = KeyCode.P;
    public const KeyCode journalScreenKey = KeyCode.J;
    public const KeyCode saveScreenKey = KeyCode.V;
    public const KeyCode loadScreenKey = KeyCode.L;
    public const KeyCode settingsScreenKey1 = KeyCode.Escape;
    public const KeyCode settingsScreenKey2 = KeyCode.R;

    public static Dictionary<KeyCode, ScreenType> keyToScreenDictionary;

    public static bool saveLoadScreenKeyIsPressed()
    {
        return Input.GetKey(saveScreenKey) ||
                Input.GetKey(loadScreenKey);
    }

    public static bool settingsScreenKeyKeyIsPressed()
    {
        return Input.GetKey(settingsScreenKey1) ||
                Input.GetKey(settingsScreenKey2);
    }

    //Dialogue KeyBinds
    public static bool continueStoryKeyIsPressed()
    {
        return Input.GetKey(KeyCode.E) ||
                Input.GetKey(KeyCode.Alpha1) ||
                Input.GetKey(KeyCode.Space) ||
                Input.GetKey(KeyCode.Return);
    }

    static KeyBindingList()
    {
        buildKeyToScreenDictionary();
    }

    public static ScreenType getScreenType(KeyCode keyCode)
    {
        if (keyToScreenDictionary.ContainsKey(keyCode))
        {
            return keyToScreenDictionary[keyCode];
        }

        return ScreenType.Inventory;
    }

    public static void buildKeyToScreenDictionary()
    {
        keyToScreenDictionary = new Dictionary<KeyCode, ScreenType>()
        {
            { characterScreenKey, ScreenType.Character },
            { inventoryScreenKey, ScreenType.Inventory },
            { partyScreenKey, ScreenType.Party },
            { journalScreenKey, ScreenType.Journal },
            { saveScreenKey, ScreenType.SaveAndLoad },
            { loadScreenKey, ScreenType.SaveAndLoad },
            { settingsScreenKey1, ScreenType.Settings },
            { settingsScreenKey2, ScreenType.Settings }
        };
    }

    //Misc Keys
    public static bool quickLoadKeysPressed()
    {
        return (Application.isEditor &&
                Input.GetKey(KeyCode.Q) &&
                Input.GetKey(KeyCode.LeftControl));
    }

}
