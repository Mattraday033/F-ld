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
        PlayerStateManager.OnStateChangeFromInUI.AddListener(endInspectingOnStateChange);
        PlayerStateManager.OnStateChangeFromInChestUI.AddListener(endInspectingOnStateChange);
        PlayerStateManager.OnStateChangeToWalking.AddListener(endInspectingOnStateChange);
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

    //Entering stays a held-key read here rather than an action: the node appears inside many activities, and
    //the inspect key is also the Walking transcript key. Leaving is the Inspecting overlay's actions.
    void Update()
    {
        KeyPressManager.updateKeyBools();

        if(!inspecting && InputReader.isHeld(KeyBindingList.inspectKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            setToInspectingMode();
            return;
        }
    }

    //While inspecting, the Inspecting overlay replaces every other action set, in and out of combat, so one
    //Space dismisses the node without also closing the chest behind it. Its actions are the ways out (the
    //inspect, settings and continue keys) plus show-formula.
    private void setToInspectingMode()
    {
        inspecting = true;

        PlayerStateManager.addInputOverlay(InputOverlay.Inspecting);

        TutorialSequenceStepTargetUIObject.createCutOutMask(hover);
    }

    private void exitInspectingMode()
    {
        inspecting = false;

        PlayerStateManager.removeInputOverlay(InputOverlay.Inspecting);

        MouseHoverManager.OnHoverPanelCreation.Invoke();
    }

    //Called by the Inspecting overlay's actions.
    public static void exitInspectingFromKey()
    {
        if(!inspecting || KeyPressManager.handlingPrimaryKeyPress)
        {
            return;
        }

        KeyPressManager.handlingPrimaryKeyPress = true;

        if(instance != null)
        {
            instance.exitInspectingMode();
        } else
        {
            inspecting = false;
            PlayerStateManager.removeInputOverlay(InputOverlay.Inspecting);
        }
    }

    //The state change that fires this re-syncs the input set itself, so the overlay goes without a refresh.
    private static void endInspectingOnStateChange()
    {
        _Inspecting = false;
        PlayerStateManager.removeInputOverlay(InputOverlay.Inspecting, refresh: false);
    }

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;

            //Awake only runs once, and the inspect key can be rebound while a screen that is kept between uses is hidden
            keybindText.text = "[" + KeyBindingList.inspectKey.ToString() + "]";
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
