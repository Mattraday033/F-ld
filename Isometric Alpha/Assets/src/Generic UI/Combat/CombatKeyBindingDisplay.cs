using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatKeyBindingDisplay : MonoBehaviour
{

    private const string spaceBetweenKeyBindings = "    ";
    private const string selectKeyBinding = "E: Select";
    private const string removeAbilityKeyBinding = "R: Unqueue Action";
    private const string escapeMenuKeyBinding = "Esc: Settings";
    private const string movementKeyBinding = "WASD: Move Selector";

    private const string cycleKeyBinding = "A/D/1-8: Cycle Actions";
    private const string deselectAllyKeyBinding = "R: Deselect Ally";
    private const string selectActionKeyBinding = "E: Select Action";

    private const string backOutOfActionKeyBinding = "R: Back";

    private const string hideKeyBindingListMessage = "Tab: Hide Key Bindings";
    private const string showKeyBindingListMessage = "Tab: Show Key Bindings";    

    private const string nextTurnButtonPress = "Space: Resolve Turn";

    public static bool hideKeyBindings = false;

    public TextMeshProUGUI displayText;

    private void Awake()
    {
        CombatStateManager.OnCurrentActivityChange.AddListener(setKeyBindingDisplay);
        CombatInputManager.OnHideKeyBindingsList.AddListener(onHideKeyBindingsButtonPress);

        setKeyBindingsListVisibility();    
    }

    private void OnDestroy()
    {
        CombatStateManager.OnCurrentActivityChange.RemoveListener(setKeyBindingDisplay);
        CombatInputManager.OnHideKeyBindingsList.RemoveListener(onHideKeyBindingsButtonPress);
    }

    private void setKeyBindingDisplay()
    {
        if(hideKeyBindings || CombatStateManager.currentActivity == CurrentActivity.InEscapeMenu)
        {
            return;
        }

        clearDisplay();

        switch(CombatStateManager.currentActivity)
        {
            case CurrentActivity.ChoosingActor:
                displayText.text += movementKeyBinding;
                addSpace();
                displayText.text += selectKeyBinding;
                addSpace();
                displayText.text += removeAbilityKeyBinding;
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
        }

        displayText.text += hideKeyBindingListMessage;
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
        hideKeyBindings = !hideKeyBindings;
        setKeyBindingsListVisibility();
    }

    private void setKeyBindingsListVisibility()
    {
        if(hideKeyBindings)
        {
            displayText.text = showKeyBindingListMessage;
        } else
        {
            setKeyBindingDisplay();
        }
    }

}
