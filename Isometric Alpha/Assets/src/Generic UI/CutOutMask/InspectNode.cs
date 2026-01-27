using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InspectNode : MonoBehaviour
{
    public static bool inspecting;

    public readonly static UnityEvent OnInspect = new UnityEvent();

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateInspectNode()
    {
        inspecting = false;
        PlayerOOCStateManager.OnStateChangeFromInUI.AddListener(endInspectingOnStateChange);
    }

    [SerializeField]
    private Transform hover;

    private void OnEnable()
    {
        gameObject.SetActive(!CombatStateManager.inCombat);
    }


    void Update()
    {
        KeyPressManager.updateKeyBools();

        if(!inspecting && Input.GetKey(KeyBindingList.inspectKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            setToInspectingMode();
            return;
        }

        if(inspecting && Input.GetKey(KeyBindingList.inspectKey) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            exitInspectingMode();
            return;
        }
    }

    private void setToInspectingMode()
    {
        inspecting = true;

        OnInspect.Invoke();
        TutorialSequenceStepTargetUIObject.createCutOutMask(hover);
    }

    private void exitInspectingMode()
    {
        inspecting = false;

        OnInspect.Invoke();

        MouseHoverManager.OnHoverPanelCreation.Invoke();
    }

    private static void endInspectingOnStateChange()
    {
        inspecting = false;
    }
}
