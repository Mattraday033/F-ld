using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindingDisplay : MonoBehaviour
{
    private static string hideKeyBindingListMessage
    {
        get
        {
            return KeyBindingList.showHideKeyBindingsListKey.ToString() + ": Hide Key Bindings";
        }
    }

    private static string showKeyBindingListMessage
    {
        get
        {
            return KeyBindingList.showHideKeyBindingsListKey.ToString() + ": Show Key Bindings";
        }
    }

    private static string spaceBetweenKeyBindings
    {
        get
        {
            return "   ";
        }
    }

    private static string selectKeyBinding
    {
        get
        {
            string select = ": Select";

            if(CombatStateManager.inCombat)
            {
                return KeyBindingList.combatSelectKey.ToString() + select;
            } else
            {
                return KeyBindingList.interactKey.ToString() + select;
            }
        }
    }

    private static string escapeMenuKeyBinding
    {
        get
        {
            return KeyBindingList.combatSettingsScreenKey.ToString() + ": Settings";
        }
    }

    private static string exitKeyBinding
    {
        get
        {
            return KeyBindingList.settingsScreenKey.ToString() + "/" + KeyBindingList.backOutKey.ToString() + ": Exit";
        }
    }

    private static string deselectKeyBinding
    {
        get
        {
            return KeyBindingList.combatDeselectKey.ToString() + ": Deselect";
        }
    }

    private static string backOutOfActionKeyBinding
    {
        get
        {
            return KeyBindingList.backOutKey.ToString() + ": Back";
        }
    }

    private static string backOutOfPopUpKeyBinding
    {
        get
        {
            string back = ": Back";

            if(CombatStateManager.inCombat)
            {
                return KeyBindingList.combatSettingsScreenKey.ToString() + "/" + KeyBindingList.combatDeselectKey.ToString() + back;
            } else
            {
                return KeyBindingList.settingsScreenKey.ToString() + "/" + KeyBindingList.backOutKey.ToString() + back;
            }
        }
    }

    #region In Combat Constants

    private static string removeAbilityKeyBinding
    {
        get
        {
            return KeyBindingList.combatDeselectKey.ToString() + ": Unqueue Action";
        }
    }

    private static string movementKeyBinding
    {
        get
        {
            return KeyBindingList.moveNorthKey.ToString() + "/" + 
                    KeyBindingList.moveWestKey.ToString() + "/" + 
                    KeyBindingList.moveSouthKey.ToString() + "/" + 
                    KeyBindingList.moveEastKey.ToString() + ": Move Selector";
        }
    }

    private static string cycleKeyBinding
    {
        get
        {
            return KeyBindingList.moveCounterClockwiseKey.ToString() + "/" + 
                    KeyBindingList.moveClockwiseKey.ToString() + "/1-8: Cycle Actions";
        }
    }

    private static string selectActionKeyBinding
    {
        get
        {
            return KeyBindingList.combatSelectKey.ToString() + ": Select Action";
        }
    }

    private static string nextTurnButtonPress
    {
        get
        {
            return KeyBindingList.resolveTurnKey.ToString() + ": Resolve Turn";
        }
    }

    private static string moreInfoKeyBinding
    {
        get
        {
            return KeyBindingList.showFormulaKey.ToString() + ": More Info";
        }
    }

    private static string fastForwardKeyBinding
    {
        get
        {
            return KeyBindingList.combatFastForwardAnimationKey.ToString() + ": Fast Foward";
        }
    }

    #endregion

    #region Out Of Combat Constants

    private static string useSkillKeyBinding
    {
        get
        {
            return KeyBindingList.skillKey.ToString() + ": Use Skill";
        }
    }

    private static string changeSkillKeyBinding
    {
        get
        {
            return KeyBindingList.cycleSkillAscendingKey.ToString() + "-" + KeyBindingList.cycleSkillDescendingKey.ToString() + ": Change Skill";
        }
    }

    private static string quicksaveKeyBinding
    {
        get
        {
            return KeyBindingList.quicksaveKey.ToString() + ": Quicksave";
        }
    }

    private static string mapKeyBinding
    {
        get
        {
            return KeyBindingList.mapKey.ToString() + ": Map";
        }
    }

    private static string worldMapKeyBinding
    {
        get
        {
            return KeyBindingList.worldMapKey.ToString() + ": World Map";
        }
    }

    private static string toggleTerrainKeyBinding
    {
        get
        {
            return KeyBindingList.hideTerrainKey.ToString() + ": Toggle Terrain";
        }
    }

    private static string transcriptKeyBinding
    {
        get
        {
            return KeyBindingList.transcriptKey.ToString() + ": Transcript";
        }
    }

    private static string highlightKeyBinding
    {
        get
        {
            return KeyBindingList.revealKey.ToString() + ": Highlight";
        }
    }

    private static string characterScreenKeyBinding
    {
        get
        {
            return KeyBindingList.characterScreenKey.ToString() + ": Character";
        }
    }

    private static string inventoryScreenKeyBinding
    {
        get
        {
            return KeyBindingList.inventoryScreenKey.ToString() + ": Inventory";
        }
    }

    private static string partyScreenKeyBinding
    {
        get
        {
            return KeyBindingList.partyScreenKey.ToString() + ": Party";
        }
    }

    private static string journalScreenKeyBinding
    {
        get
        {
            return KeyBindingList.journalScreenKey.ToString() + ": Journal";
        }
    }

    private static string saveLoadKeyBinding
    {
        get
        {
            return KeyBindingList.loadScreenKey.ToString() + ": Save/Load";
        }
    }

    private static string continueKeyBinding
    {
        get
        {
            string continueMsg = ": Continue";

            if(CombatStateManager.inCombat)
            {
                return KeyBindingList.combatSelectKey.ToString() + "/" + KeyBindingList.resolveTurnKey.ToString() + continueMsg;
            } else
            {
                return KeyBindingList.interactKey.ToString() + "/" + KeyBindingList.acceptKey.ToString() + continueMsg;
            }
        }
    }

    private static string dialogueChoicesKeyBinding
    {
        get
        {
            return "1-8: Select Dialogue";
        }
    }

    private static string leaveMapKeyBinding
    {
        get
        {
            return KeyBindingList.mapKey.ToString() + "/" + KeyBindingList.settingsScreenKey.ToString() + ": Exit";
        }
    }

    private static string leaveWorldMapKeyBinding
    {
        get
        {
            return KeyBindingList.worldMapKey.ToString() + "/" + KeyBindingList.settingsScreenKey.ToString() + ": Exit";
        }
    }

    private static string useActivatedSkillKeyBinding
    {
        get
        {
            return KeyBindingList.interactKey.ToString() + ": Use Skill";
        }
    }

    private static string moveSkillTargetKeyBinding
    {
        get
        {
            return KeyBindingList.moveNorthKey.ToString() + "/" + 
                    KeyBindingList.moveWestKey.ToString() + "/" + 
                    KeyBindingList.moveSouthKey.ToString() + "/" + 
                    KeyBindingList.moveEastKey.ToString() + ": Move Selector";
        }
    }

    private static string leaveSkillKeyBinding
    {
        get
        {
            return KeyBindingList.settingsScreenKey.ToString() + "/" + 
                    KeyBindingList.skillKey.ToString() + ": Exit Skill Mode";
        }
    }

    #endregion

    public static bool hideKeyBindingsInCombat = false;
    public static bool hideKeyBindingsOOC = false;

//E: Select    R: Back    Space: Use Skill    1-2: Change Skill    Q: Quicksave    M: Map    N: World Map    C: Character    I: Inventory    P: Party    L: Save/Load    F: Toggle Terrain    T: Transcript    Shift: Highlight    Backspace: Hide Keys

    public TextMeshProUGUI displayText;

    private void Awake()
    {
        if(Flags.isInNewGameMode() && !LoadSaveFile.midLoad)
        {
            gameObject.SetActive(false);
            return;
        }

        PlayerOOCStateManager.OnStateChange.AddListener(setKeyBindingDisplay);
        CombatStateManager.OnCurrentActivityChange.AddListener(setKeyBindingDisplay);
        CombatInputManager.OnHideKeyBindingsList.AddListener(onHideKeyBindingsButtonPress);

        setKeyBindingsListVisibility();    
    }

    private void OnDestroy()
    {
        PlayerOOCStateManager.OnStateChange.RemoveListener(setKeyBindingDisplay);
        CombatStateManager.OnCurrentActivityChange.RemoveListener(setKeyBindingDisplay);
        CombatInputManager.OnHideKeyBindingsList.RemoveListener(onHideKeyBindingsButtonPress);
    }

    private void setKeyBindingDisplay()
    {
        if(keyBindingsHidden() || 
            (CombatStateManager.inCombat && CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu))
        {
            return;
        }

        clearDisplay();

        if(CombatStateManager.inCombat)
        {
            setKeyBindingsDisplayInCombat();
        } else
        {
            setKeyBindingsDisplayOOC();
        }

        displayText.text += hideKeyBindingListMessage;
    }

    private void setKeyBindingsDisplayInCombat()
    {
        switch(CombatStateManager.currentActivity)
        {
            case CurrentActivity.ChoosingActor:
                displayText.text += movementKeyBinding;
                addSpace();
                displayText.text += selectKeyBinding;
                addSpace();
                displayText.text += removeAbilityKeyBinding;
                addSpace();                
                displayText.text += nextTurnButtonPress;
                addSpace();
                displayText.text += moreInfoKeyBinding;
                addSpace();
                displayText.text += escapeMenuKeyBinding;
                addSpace();
                break;
            case CurrentActivity.ChoosingAbility:
                displayText.text += cycleKeyBinding;
                addSpace();
                displayText.text += selectActionKeyBinding;
                addSpace();
                displayText.text += deselectKeyBinding;
                addSpace();
                displayText.text += moreInfoKeyBinding;
                addSpace();
                break;
            case CurrentActivity.ChoosingLocation:
            case CurrentActivity.ChoosingTertiary:
                displayText.text += movementKeyBinding;
                addSpace();
                displayText.text += selectKeyBinding;
                addSpace();
                displayText.text += deselectKeyBinding;
                addSpace();
                displayText.text += moreInfoKeyBinding;
                addSpace();
                break;
            case CurrentActivity.Finished:
                displayText.text += removeAbilityKeyBinding;
                addSpace();
                displayText.text += moreInfoKeyBinding;
                addSpace();
                displayText.text += nextTurnButtonPress;
                addSpace();
                break;
            case CurrentActivity.ResolveActionWarning:
                displayText.text += continueKeyBinding;
                addSpace();
                displayText.text += backOutOfPopUpKeyBinding;
                addSpace();
                break;
            case CurrentActivity.Waiting:
                displayText.text += fastForwardKeyBinding;
                addSpace();
                break;
        }
    }

    private void setKeyBindingsDisplayOOC()
    {
        switch(PlayerOOCStateManager.currentActivity)
        {
            case OOCActivity.walking:
                displayText.text += selectKeyBinding;
                addSpace();
                displayText.text += backOutOfActionKeyBinding;
                addSpace();
                displayText.text += useSkillKeyBinding;
                addSpace();
                displayText.text += changeSkillKeyBinding;
                addSpace();
                displayText.text += quicksaveKeyBinding;
                addSpace();
                // displayText.text += mapKeyBinding;
                // addSpace();
                // displayText.text += worldMapKeyBinding;
                // addSpace();
                displayText.text += toggleTerrainKeyBinding;
                addSpace();
                displayText.text += transcriptKeyBinding;
                addSpace();
                displayText.text += highlightKeyBinding;
                addSpace();
                break;
            case OOCActivity.inDialogue:
                displayText.text += continueKeyBinding;
                addSpace();
                displayText.text += dialogueChoicesKeyBinding;
                addSpace();
                break;
            case OOCActivity.inUI:
                displayText.text += characterScreenKeyBinding;
                addSpace();
                displayText.text += inventoryScreenKeyBinding;
                addSpace();
                displayText.text += partyScreenKeyBinding;
                addSpace();
                displayText.text += journalScreenKeyBinding;
                addSpace();
                displayText.text += saveLoadKeyBinding;
                addSpace();
                displayText.text += exitKeyBinding;
                addSpace();
                break;
            case OOCActivity.inMap:
                displayText.text += worldMapKeyBinding;
                addSpace();
                displayText.text += leaveMapKeyBinding;
                addSpace();
                break;
            case OOCActivity.intimidating:
                displayText.text += useActivatedSkillKeyBinding;
                addSpace();
                displayText.text += leaveSkillKeyBinding;
                addSpace();
                break;
            case OOCActivity.cunning:
                displayText.text += useActivatedSkillKeyBinding;
                addSpace();
                displayText.text += moveSkillTargetKeyBinding;
                addSpace();
                displayText.text += leaveSkillKeyBinding;
                addSpace();
                break;
            case OOCActivity.observing:
                displayText.text += leaveSkillKeyBinding;
                addSpace();
                break;
            case OOCActivity.inChestUI:
                displayText.text += continueKeyBinding;
                addSpace();
                break;
            case OOCActivity.inBookUI:
                displayText.text += exitKeyBinding;
                addSpace();
                break;
            case OOCActivity.inShopUI:
                displayText.text += exitKeyBinding;
                addSpace();
                break;
            case OOCActivity.inDialoguePopUp:
                displayText.text += exitKeyBinding;
                addSpace();
                break;
            case OOCActivity.inLevelUpPopUp:
                break;
            case OOCActivity.inTutorialPopUp:
                break;
            case OOCActivity.inTutorialSequence:
                break;
            case OOCActivity.inWorldMap:
                displayText.text += mapKeyBinding;
                addSpace();
                displayText.text += leaveWorldMapKeyBinding;
                addSpace();
                break;
        }
    }

    private bool keyBindingsHidden()
    {
        if(CombatStateManager.inCombat)
        {
            return hideKeyBindingsInCombat;
        } else
        {
            return hideKeyBindingsOOC;
        }
    }

    private void addSpace()
    {
        displayText.text += spaceBetweenKeyBindings;
    }

    private void clearDisplay()
    {
        displayText.text = "";
    }

    private void onHideKeyBindingsButtonPress()
    {
        if(SaveHandler.saveNameFieldIsSelected())
        {
            return;
        }

        if(CombatStateManager.inCombat)
        {
            hideKeyBindingsInCombat = !hideKeyBindingsInCombat;
        } else
        {
            hideKeyBindingsOOC = !hideKeyBindingsOOC;
        }


        setKeyBindingsListVisibility();
    }

    private void setKeyBindingsListVisibility()
    {
        bool currentFlag = false;

        if(CombatStateManager.inCombat)
        {
            currentFlag = hideKeyBindingsInCombat;
        } else
        {
            currentFlag = hideKeyBindingsOOC;
        }


        if(currentFlag)
        {
            displayText.text = showKeyBindingListMessage;
        } else
        {
            setKeyBindingDisplay();
        }
    }

}
