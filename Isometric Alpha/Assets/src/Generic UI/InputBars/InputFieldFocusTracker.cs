using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

//Puts PlayerStateManager into InInputField while the attached field has focus, driven by the field's own
//events. TMP has no event for regaining focus, and focus can only come back to a field that is selected but not
//focused. So this never lets that case exist: when editing ends (Escape, Enter) the selection is cleared as
//well, which keeps selected and focused changing together. onSelect and onDeselect then cover entering and
//leaving.
[RequireComponent(typeof(TMP_InputField))]
public class InputFieldFocusTracker : MonoBehaviour
{
    private TMP_InputField inputField;

    private bool enteredInputFieldState = false;
    private CurrentActivity activityToReturnTo;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    private void OnEnable()
    {
        inputField.onSelect.AddListener(onFieldSelected);
        inputField.onDeselect.AddListener(onFieldDeselected);
        inputField.onEndEdit.AddListener(onFieldEndEdit);
    }

    private void OnDisable()
    {
        inputField.onSelect.RemoveListener(onFieldSelected);
        inputField.onDeselect.RemoveListener(onFieldDeselected);
        inputField.onEndEdit.RemoveListener(onFieldEndEdit);

        //Disabling or destroying a selected field fires no onDeselect, so the state is handed back here.
        leaveInputFieldState();
    }

    private void onFieldSelected(string text)
    {
        enterInputFieldState();
    }

    private void onFieldDeselected(string text)
    {
        leaveInputFieldState();
    }

    //Escape and Enter end editing but leave the field selected. Clearing the selection here fires onDeselect,
    //which is what leaves the state.
    //onEndEdit also fires from inside TMP's own deselect. At that point the selection is already changing, so
    //SetSelectedGameObject would be refused with an error, and there is nothing left to clear anyway.
    private void onFieldEndEdit(string text)
    {
        EventSystem eventSystem = EventSystem.current;

        if (eventSystem == null ||
            eventSystem.alreadySelecting ||
            eventSystem.currentSelectedGameObject != gameObject)
        {
            return;
        }

        eventSystem.SetSelectedGameObject(null);
    }

    private void enterInputFieldState()
    {
        if (enteredInputFieldState)
        {
            return;
        }

        activityToReturnTo = PlayerStateManager.currentActivity;

        PlayerStateManager.setCurrentActivity(CurrentActivity.InInputField);

        //setCurrentActivity ignores the change during combat and tutorial sequences, so this only owns the
        //state if the change actually happened.
        enteredInputFieldState = PlayerStateManager.currentActivity == CurrentActivity.InInputField;
    }

    private void leaveInputFieldState()
    {
        if (!enteredInputFieldState)
        {
            return;
        }

        enteredInputFieldState = false;

        //If something else changed the state while the field had focus (a load, a scene change), that state
        //wins and this does not overwrite it.
        if (PlayerStateManager.currentActivity == CurrentActivity.InInputField)
        {
            PlayerStateManager.setCurrentActivity(activityToReturnTo);
        }
    }
}
