using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class InspectNode : MonoBehaviour
{
    private static bool _Inspecting;
    public static bool inspecting
    {
        get
        {
            return _Inspecting;
        } 
        set
        {
            _Inspecting = value;
            OnInspect.Invoke();
        }
    }

    public static InspectNode instance; 

    public TextMeshProUGUI keybindText;

    public readonly static UnityEvent OnInspect = new UnityEvent();

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        instance = null;
        _Inspecting = false;
        PlayerOOCStateManager.OnStateChangeFromInUI.AddListener(endInspectingOnStateChange);
        PlayerOOCStateManager.OnStateChangeFromInChestUI.AddListener(endInspectingOnStateChange);
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(endInspectingOnStateChange);
    }

    [SerializeField]
    private Transform hover;

    private void Awake()
    {
        if(CombatStateManager.inCombat && AbilityMenuButton.hoveringOverAbilityMenuButton)
        {
            gameObject.SetActive(false);
            return;
        }

        keybindText.text = "[" + KeyBindingList.inspectKey.ToString() + "]";
    }

    void Update()
    {
        KeyPressManager.updateKeyBools();

        if(!inspecting && Input.GetKey(KeyBindingList.inspectKey.getCurrentKeyCode()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            setToInspectingMode();
            return;
        }

        if(inspecting && 
            (Input.GetKey(KeyBindingList.inspectKey.getCurrentKeyCode()) || 
            Input.GetKey(KeyBindingList.settingsScreenKey.getCurrentKeyCode()) || 
            KeyBindingList.continueUIKeyIsPressed()) &&
             !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            exitInspectingMode();
            return;
        }
    }

    //PlayerInput.Update returns early while inspecting, but the CustomInputActions are event driven rather
    //than polled, so they would keep firing underneath the inspect node - one Space would dismiss the node
    //and close the chest behind it. Same pattern as KeybindingButton.listenForKeyPress: pull the whole set
    //and let PlayerOOCStateManager put back whatever the current activity calls for.
    //Combat is left alone - no OOC action is enabled there, and CombatInputManager.Update already gates
    //itself on InspectNode.inspecting.
    private void setToInspectingMode()
    {
        inspecting = true;

        if (!CombatStateManager.inCombat)
        {
            CustomInputAction.DisableAllInputActions.Invoke();
        }

        TutorialSequenceStepTargetUIObject.createCutOutMask(hover);


    }

    private void exitInspectingMode()
    {
        inspecting = false;

        if (!CombatStateManager.inCombat)
        {
            PlayerOOCStateManager.updateEnabledInputActions();
        }

        MouseHoverManager.OnHoverPanelCreation.Invoke();
    }

    private static void endInspectingOnStateChange()
    {
        _Inspecting = false;
    }

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if(instance == this)
        {
            instance = null;
        }
    }

}
