using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ScreenType {Character = 1, Inventory = 2, Party = 3, Journal = 4, SaveAndLoad = 5, Settings = 6}

public static class OverallUIManager
{
    private static GameObject popUpScreenBlockerGameObject;

    public static bool showFormula = false;

    public static ScreenType lastScreenType = ScreenType.Inventory;
    public static ScreenManager currentScreenManager { get; set; }

    public static Transform screenBackground; //parent of everything described in a screen manager
    public static GameObject UIParentPanel;

    public static LevelUpPopUpButton levelUpPopUpButton { get; private set; }

    public static Stats previousPartyMember;
    public static Dictionary<ScreenType, ScreenState> screenStates = new Dictionary<ScreenType, ScreenState>();

    static OverallUIManager()
    {
        levelUpPopUpButton = new LevelUpPopUpButton();
    }

    public static void leaveUI()
    {
        saveCurrentScreenType();
        destroyCurrentScreenType();

        UIParentPanel.SetActive(false);
    }

    public static void changeScreen(ScreenType newScreenType)
    {
        if (newScreenType == lastScreenType && PlayerOOCStateManager.currentActivity == OOCActivity.inUI)
        {
            return;
        }

        MouseHoverManager.destroyMouseHoverBase();

        UIParentPanel.SetActive(true);

        saveCurrentScreenType();
        savePreviousPartyMember();
        destroyCurrentScreenType();

        lastScreenType = newScreenType;

        switch (newScreenType)
        {
            case ScreenType.Character:
                currentScreenManager = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.characterScreen), screenBackground).GetComponent<ScreenManager>();
                break;
            case ScreenType.Inventory:
                currentScreenManager = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.inventoryScreen), screenBackground).GetComponent<ScreenManager>();
                break;
            case ScreenType.Party:
                currentScreenManager = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.partyScreen), screenBackground).GetComponent<ScreenManager>();
                break;
            case ScreenType.Journal:
                currentScreenManager = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.journalScreen), screenBackground).GetComponent<ScreenManager>();
                break;
            case ScreenType.SaveAndLoad:
                currentScreenManager = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.saveScreen), screenBackground).GetComponent<ScreenManager>();
                break;
            case ScreenType.Settings:
                currentScreenManager = GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.settingsScreen), screenBackground).GetComponent<ScreenManager>();
                break;
            default:
                throw new IOException("Unexpected Screen Value: " + newScreenType.ToString());
        }

        ScreenButtonManager.setCurrentScreenButton(newScreenType);
        currentScreenManager.setToScreenState(getScreenState(newScreenType));
    }

    public static void setCurrentScreenType(ScreenManager screenManager)
    {
        currentScreenManager = screenManager;
    }


    public static ScreenState getScreenState(ScreenType screenType)
    {
        if (!screenStates.ContainsKey(screenType))
        {
            return null;
        }
        else
        {
            return screenStates[screenType];
        }
    }

    private static void destroyCurrentScreenType()
    {
        if (currentScreenManager != null)
        {
            GameObject.Destroy(currentScreenManager.gameObject);
            currentScreenManager = null;
        }
    }

    private static void savePreviousPartyMember()
    {
        if (currentScreenManager != null && currentScreenManager.getCurrentPartyMember() != null)
        {
            previousPartyMember = currentScreenManager.getCurrentPartyMember();
        }
    }

    private static void saveCurrentScreenType()
    {
        if (currentScreenManager != null)
        {
            screenStates[currentScreenManager.getScreenType()] = new ScreenState(currentScreenManager);
        }
    }

    public static void resetScreenStates()
    {
        screenStates = new Dictionary<ScreenType, ScreenState>();
    }



    public static bool onScreen(ScreenType screen)
    {
        switch (screen)
        {
            case ScreenType.Character:
                return onCharacterScreen();
            case ScreenType.Inventory:
                return onInventoryScreen();
            case ScreenType.Party:
                return onPartyScreen();
            case ScreenType.Journal:
                return onJournalScreen();
            case ScreenType.SaveAndLoad:
                return onSaveAndLoadScreen();
            case ScreenType.Settings:
                return onSettingsScreen();
            default:
                return false;
        }
    }

    public static void moveToScreenToTheLeft()
    {
        changeScreen(getScreenToTheLeft());
    }

    public static void moveToScreenToTheRight()
    {
        changeScreen(getScreenToTheRight());
    }

    private static ScreenType getScreenToTheLeft()
    {
        switch (lastScreenType)
        {
            case ScreenType.Character:
                return ScreenType.Settings;
            case ScreenType.Inventory:
                return ScreenType.Character;
            case ScreenType.Party:
                return ScreenType.Inventory;
            case ScreenType.Journal:
                return ScreenType.Party;
            case ScreenType.SaveAndLoad:
                return ScreenType.Journal;
            case ScreenType.Settings:
                return ScreenType.SaveAndLoad;
            default:
                return ScreenType.Inventory;
        }
    }

    private static ScreenType getScreenToTheRight()
    {
        switch (lastScreenType)
        {
            case ScreenType.Character:
                return ScreenType.Inventory;
            case ScreenType.Inventory:
                return ScreenType.Party;
            case ScreenType.Party:
                return ScreenType.Journal;
            case ScreenType.Journal:
                return ScreenType.SaveAndLoad;
            case ScreenType.SaveAndLoad:
                return ScreenType.Settings;
            case ScreenType.Settings:
                return ScreenType.Character;
            default:
                return ScreenType.Inventory;
        }
    }

    public static bool onCharacterScreen()
    {
        return lastScreenType == ScreenType.Character;
    }

    public static bool onInventoryScreen()
    {
        return lastScreenType == ScreenType.Inventory;
    }

    public static bool onPartyScreen()
    {
        return lastScreenType == ScreenType.Party;
    }

    public static bool onJournalScreen()
    {
        return lastScreenType == ScreenType.Journal;
    }
    public static bool onSaveAndLoadScreen()
    {
        return lastScreenType == ScreenType.SaveAndLoad;
    }

    public static bool onSettingsScreen()
    {
        return lastScreenType == ScreenType.Settings;
    }

    public static AllyStats getCurrentPartyMember()
    {
        if (currentScreenManager == null)
        {
            return null;
        }

        // if (currentScreenManager.getCurrentPartyMember() != null)
        // {
        //     Debug.LogError("currentScreenManager.getCurrentPartyMember() = " + currentScreenManager.getCurrentPartyMember().getName());
        // }

        return currentScreenManager.getCurrentPartyMember();
    }

    public static CombatActionArray getCurrentActionArray()
    {
        Stats currentPartyMember = getCurrentPartyMember();

        if (currentPartyMember == null)
        {
            return null;
        }

        return currentPartyMember.getActionArray();
    }

    public static EquippedItems getCurrentEquippedItems()
    {
        Stats currentPartyMember = getCurrentPartyMember();

        if (currentPartyMember == null)
        {
            return null;
        }

        return currentPartyMember.getEquippedItems();
    }
}
