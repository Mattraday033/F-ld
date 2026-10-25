using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
//The project's own Key item class would win over the Input System's Key enum, so the enum is aliased.
using InputKey = UnityEngine.InputSystem.Key;

/// <summary>
/// Reads the current state of the keyboard and mouse through the Input System, in place of the legacy
/// UnityEngine.Input. Used for held keys, modifiers, chords and key-repeat, where code asks "is this down
/// right now?" rather than reacting to a press. Discrete presses go through CustomInputActions instead.
///
/// Reading through the Input System matters for headless tests: a virtual keyboard or mouse added by a
/// test drives these reads, while legacy Input can't be faked at all.
///
/// Every read returns false or zero when there's no keyboard or mouse.
/// </summary>
public static class InputReader
{
    public static bool isHeld(KeyBind keyBind)
    {
        if(keyBind.mouseButtonIndex >= 0)
        {
            return mouseButtonHeld(keyBind.mouseButtonIndex);
        }

        return keyBind.hasInputKey && keyIsHeld(keyBind.inputKey);
    }

    public static bool isHeld(KeyCode keyCode)
    {
        int mouseButton = keyCode.ToMouseButtonIndex();

        if(mouseButton >= 0)
        {
            return mouseButtonHeld(mouseButton);
        }

        return keyCode.TryGetInputKey(out InputKey key) && keyIsHeld(key);
    }

    public static bool wasPressedThisFrame(KeyCode keyCode)
    {
        int mouseButton = keyCode.ToMouseButtonIndex();

        if(mouseButton >= 0)
        {
            ButtonControl button = getMouseButton(mouseButton);
            return button != null && button.wasPressedThisFrame;
        }

        Keyboard keyboard = Keyboard.current;

        return keyboard != null && keyCode.TryGetInputKey(out InputKey key) && keyboard[key].wasPressedThisFrame;
    }

    //Any keyboard key or mouse button, matching legacy Input.anyKey (which counts mouse buttons too).
    public static bool anyButtonHeld()
    {
        Keyboard keyboard = Keyboard.current;

        if(keyboard != null && keyboard.anyKey.isPressed)
        {
            return true;
        }

        Mouse mouse = Mouse.current;

        return mouse != null &&
                (mouse.leftButton.isPressed || mouse.rightButton.isPressed || mouse.middleButton.isPressed ||
                 mouse.backButton.isPressed || mouse.forwardButton.isPressed);
    }

    //0 left, 1 right, 2 middle, 3 back, 4 forward - the legacy Mouse0-Mouse4 order.
    public static bool mouseButtonHeld(int button)
    {
        ButtonControl control = getMouseButton(button);

        return control != null && control.isPressed;
    }

    //Screen position in pixels, as a Vector3 with z = 0 like legacy Input.mousePosition.
    public static Vector3 mousePosition
    {
        get
        {
            Mouse mouse = Mouse.current;

            return mouse == null ? Vector3.zero : (Vector3)mouse.position.ReadValue();
        }
    }

    //This frame's vertical scroll. Only the sign is comparable with legacy Input.mouseScrollDelta: the
    //Input System reports larger values per notch.
    public static float scrollY
    {
        get
        {
            Mouse mouse = Mouse.current;

            return mouse == null ? 0f : mouse.scroll.ReadValue().y;
        }
    }

    private static bool keyIsHeld(InputKey key)
    {
        Keyboard keyboard = Keyboard.current;

        return keyboard != null && keyboard[key].isPressed;
    }

    private static ButtonControl getMouseButton(int button)
    {
        Mouse mouse = Mouse.current;

        if(mouse == null)
        {
            return null;
        }

        switch(button)
        {
            case 0:
                return mouse.leftButton;
            case 1:
                return mouse.rightButton;
            case 2:
                return mouse.middleButton;
            case 3:
                return mouse.backButton;
            case 4:
                return mouse.forwardButton;
            default:
                return null;
        }
    }
}
