using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ScreenType {Character = 1, Inventory = 2, Party = 3, Journal = 4, SaveAndLoad = 5, Settings = 6}

public static class OverallUIManager
{
    public static bool _ShowFormula;
    public static bool showFormula
    {
        get
        {
            return _ShowFormula;
        } 
        set
        {
            _ShowFormula = value;
            DescriptionPanelBuilder.OnFormulaSwap.Invoke();
        }
    }

    public static ScreenType lastScreenType;
    public static ScreenManager currentScreenManager { get; set; }

    public static Transform screenBackground; //parent of everything described in a screen manager
    public static ScrollableUIElement partyMemberSelectionGrid; 
    public static GameObject UIParentPanel;
    public static Transform notificationParent;

    public static LevelUpPopUpButton levelUpPopUpButton { get; private set; }

    public static Stats previousPartyMember;
    public static Dictionary<ScreenType, ScreenState> screenStates;

    static OverallUIManager()
    {
        levelUpPopUpButton = new LevelUpPopUpButton();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        previousPartyMember = null;
        screenStates = new Dictionary<ScreenType, ScreenState>();
        screenBackground = null;
        partyMemberSelectionGrid = null;
        UIParentPanel = null;
        notificationParent = null;
        lastScreenType = ScreenType.Inventory;
        _ShowFormula = false;

        LoadSaveFile.OnLoadResetData.AddListener(removeCurrentScreenType);
        LoadSaveFile.OnLoadResetData.AddListener(resetScreenStates);
    }

    public static void leaveUI()
    {
        closeCurrentScreen();

        UIParentPanel.SetActive(false);
    }

    public static void changeScreen(ScreenType newScreenType)
    {
        if (newScreenType == lastScreenType && PlayerStateManager.currentActivity == CurrentActivity.InUI)
        {
            return;
        }

        AudioManager.playChangeScreenSFX();

        MouseHoverManager.destroyMouseHoverBase();

        UIParentPanel.SetActive(true);

        savePreviousPartyMember();
        closeCurrentScreen();

        lastScreenType = newScreenType;

        currentScreenManager = PrebuiltScreenManager.show(newScreenType, screenBackground);

        //no prebuilt copy of this screen, so it is made the way it always was
        if (currentScreenManager == null)
        {
            currentScreenManager = GameObject.Instantiate(Resources.Load<GameObject>(getScreenPrefabName(newScreenType)), screenBackground).GetComponent<ScreenManager>();
        }

        ScreenButtonManager.setCurrentScreenButton(newScreenType);
        // currentScreenManager.setToScreenState(getScreenState(newScreenType));
    }

    public static string getScreenPrefabName(ScreenType screenType)
    {
        switch (screenType)
        {
            case ScreenType.Character:
                return PrefabNames.characterScreen;
            case ScreenType.Inventory:
                return PrefabNames.inventoryScreen;
            case ScreenType.Party:
                return PrefabNames.partyScreen;
            case ScreenType.Journal:
                return PrefabNames.journalScreen;
            case ScreenType.SaveAndLoad:
                return PrefabNames.saveScreen;
            case ScreenType.Settings:
                return PrefabNames.settingsScreen;
            default:
                throw new IOException("Unexpected Screen Value: " + screenType.ToString());
        }
    }

    public static KeyCode getCurrentScreenExitKey()
    {
        if(currentScreenManager == null)
        {
            return KeyCode.None;
        }

        return currentScreenManager.getExitKeyCode();
    }

    public static void removeCurrentScreenType()
    {
        setCurrentScreenType(null);
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

    //a prebuilt screen is put away to be shown again. Any other copy is destroyed, as every screen used to be
    private static void closeCurrentScreen()
    {
        if (currentScreenManager != null)
        {
            if (currentScreenManager.isPrebuilt)
            {
                PrebuiltScreenManager.hide(currentScreenManager);
            }
            else
            {
                GameObject.DestroyImmediate(currentScreenManager.gameObject);
            }

            currentScreenManager = null;
        }
    }

    private static void savePreviousPartyMember()
    {
        if (ScreenManager.currentPartyMember != null)
        {
            previousPartyMember = ScreenManager.currentPartyMember;
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

        // if (ScreenManager.currentPartyMember != null)
        // {
        //     Debug.LogError("ScreenManager.currentPartyMember = " + ScreenManager.currentPartyMember.uniqueName);
        // }

        return ScreenManager.currentPartyMember;
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
