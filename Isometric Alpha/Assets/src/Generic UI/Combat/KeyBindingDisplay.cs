using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindingDisplay : MonoBehaviour
{
    private const string hideKeyBindingListMessage = "Backspace: Hide Key Bindings";
    private const string showKeyBindingListMessage = "Backspace: Show Key Bindings";    
    private const string spaceBetweenKeyBindings = "   ";
    private const string selectKeyBinding = "E: Select";
    private const string escapeMenuKeyBinding = "Esc: Settings";
    private const string exitKeyBinding = "Esc/R: Exit";
    private const string backOutOfActionKeyBinding = "R: Back";
    private const string backOutOfPopUpKeyBinding = "Esc/R: Back";

    #region In Combat Constants

    private const string removeAbilityKeyBinding = "R: Unqueue Action";
    private const string movementKeyBinding = "WASD: Move Selector";

    private const string cycleKeyBinding = "A/D/1-8: Cycle Actions";
    private const string deselectAllyKeyBinding = "R: Deselect Ally";
    private const string selectActionKeyBinding = "E: Select Action";

    private const string nextTurnButtonPress = "Space: Resolve Turn";

    #endregion

    #region Out Of Combat Constants

    private const string useSkillKeyBinding = "Space: Use Skill";
    private const string changeSkillKeyBinding = "1-2: Change Skill";
    private const string quicksaveKeyBinding = "Q: Quicksave";
    private const string mapKeyBinding = "M: Map";
    private const string worldMapKeyBinding = "N: World Map";
    private const string toggleTerrainKeyBinding = "F: Toggle Terrain"; 
    private const string transcriptKeyBinding = "T: Transcript"; 
    private const string highlightKeyBinding = "Shift: Highlight"; 

    private const string characterScreenKeyBinding = "C: Character"; 
    private const string inventoryScreenKeyBinding = "I: Inventory"; 
    private const string partyScreenKeyBinding = "P: Party"; 
    private const string saveLoadKeyBinding = "L: Save/Load"; 

    private const string continueKeyBinding = "E/Space: Continue";
    private const string dialogueChoicesKeyBinding = "1-8: Select Dialogue"; 

    private const string leaveMapKeyBinding = "M/Esc: Exit";
    private const string leaveWorldMapKeyBinding = "N/Esc: Exit";

    private const string useActivatedSkillKeyBinding = "E: Use Skill";

    private const string moveSkillTargetKeyBinding = "WASD: Move Target";

    private const string leaveSkillKeyBinding = "Esc/Space: Exit Skill Mode";

    #endregion

    public static bool hideKeyBindingsInCombat = false;
    public static bool hideKeyBindingsOOC = false;

//E: Select    R: Back    Space: Use Skill    1-2: Change Skill    Q: Quicksave    M: Map    N: World Map    C: Character    I: Inventory    P: Party    L: Save/Load    F: Toggle Terrain    T: Transcript    Shift: Highlight    Backspace: Hide Keys

    public TextMeshProUGUI displayText;

    private void Awake()
    {
        if(Flags.isInNewGameMode())
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
                displayText.text += escapeMenuKeyBinding;
                addSpace();
                break;
            case CurrentActivity.ChoosingAbility:
                displayText.text += cycleKeyBinding;
                addSpace();
                displayText.text += selectActionKeyBinding;
                addSpace();
                displayText.text += deselectAllyKeyBinding;
                addSpace();
                break;
            case CurrentActivity.ChoosingLocation:
            case CurrentActivity.ChoosingTertiary:
                displayText.text += movementKeyBinding;
                addSpace();
                displayText.text += selectKeyBinding;
                addSpace();
                displayText.text += backOutOfActionKeyBinding;
                addSpace();
                break;
            case CurrentActivity.Finished:
                displayText.text += removeAbilityKeyBinding;
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
                displayText.text += mapKeyBinding;
                addSpace();
                displayText.text += worldMapKeyBinding;
                addSpace();
                displayText.text += characterScreenKeyBinding;
                addSpace();
                displayText.text += inventoryScreenKeyBinding;
                addSpace();
                displayText.text += partyScreenKeyBinding;
                addSpace();
                displayText.text += saveLoadKeyBinding;
                addSpace();
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
