using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
//The project's own Key item class would win over the Input System's Key enum, so the enum is aliased.
using InputKey = UnityEngine.InputSystem.Key;

/// <summary>
/// A virtual keyboard and mouse for headless regression tests. Presses go through the Input System exactly
/// like a player's, so they reach CustomInputActions, InputReader, the any-button hook and the UI's
/// InputSystemUIInputModule.
///
/// Everything here does nothing unless the game runs in batch mode, so it can't touch a player's input.
/// A headless run starts with no input devices at all; ensureDevices adds the virtual pair.
///
/// Typing text is not supported: TMP_InputField reads typed characters from IMGUI events, which the Input
/// System doesn't feed. Tests set a field's text directly.
/// </summary>
public static class VirtualInputDriver
{
    private const string keyboardName = "RegressionKeyboard";
    private const string mouseName = "RegressionMouse";

    //the shortest tap that code polling in Update still sees: one frame down, then released the next
    public const int minimumTapFrames = 2;

    private static Keyboard keyboard = null;
    private static Mouse mouse = null;

    private static readonly HashSet<InputKey> heldKeys = new HashSet<InputKey>();
    private static Vector2 mousePosition = Vector2.zero;

    //Held mouse buttons in legacy Mouse0-Mouse4 order. The Input System numbers its buttons differently
    //(forward before back), so they're turned into MouseState buttons by name.
    private static readonly bool[] heldMouseButtons = new bool[5];
    private static readonly MouseButton[] mouseButtonsInLegacyOrder = new MouseButton[]
    {
        MouseButton.Left, MouseButton.Right, MouseButton.Middle, MouseButton.Back, MouseButton.Forward
    };

    //KeyBinds by their KeyBindingList field name, so a test names a binding rather than a key and keeps
    //working when the binding is changed. Listed by hand rather than found by reflection, which a stripped
    //build could lose.
    private static readonly Dictionary<string, KeyBind> keyBindsByName = new Dictionary<string, KeyBind>()
    {
        { "moveNorthKey", KeyBindingList.moveNorthKey },
        { "moveWestKey", KeyBindingList.moveWestKey },
        { "moveSouthKey", KeyBindingList.moveSouthKey },
        { "moveEastKey", KeyBindingList.moveEastKey },

        { "interactKey", KeyBindingList.interactKey },
        { "backOutKey", KeyBindingList.backOutKey },
        { "hideTerrainKey", KeyBindingList.hideTerrainKey },
        { "revealKey", KeyBindingList.revealKey },
        { "removePlacedCompanionMovableObjectKey", KeyBindingList.removePlacedCompanionMovableObjectKey },
        { "quicksaveKey", KeyBindingList.quicksaveKey },
        { "transcriptKey", KeyBindingList.transcriptKey },
        { "showHideKeyBindingsListKey", KeyBindingList.showHideKeyBindingsListKey },

        { "skillKey", KeyBindingList.skillKey },
        { "cycleSkillAscendingKey", KeyBindingList.cycleSkillAscendingKey },
        { "cycleSkillDescendingKey", KeyBindingList.cycleSkillDescendingKey },

        { "acceptKey", KeyBindingList.acceptKey },
        { "acceptInputKey", KeyBindingList.acceptInputKey },
        { "lastScreenKey", KeyBindingList.lastScreenKey },
        { "characterScreenKey", KeyBindingList.characterScreenKey },
        { "inventoryScreenKey", KeyBindingList.inventoryScreenKey },
        { "partyScreenKey", KeyBindingList.partyScreenKey },
        { "journalScreenKey", KeyBindingList.journalScreenKey },
        { "loadScreenKey", KeyBindingList.loadScreenKey },
        { "settingsScreenKey", KeyBindingList.settingsScreenKey },
        { "moveLeftKey", KeyBindingList.moveLeftKey },
        { "moveRightKey", KeyBindingList.moveRightKey },
        { "inspectKey", KeyBindingList.inspectKey },
        { "mapKey", KeyBindingList.mapKey },
        { "worldMapKey", KeyBindingList.worldMapKey },
        { "showFormulaKey", KeyBindingList.showFormulaKey },
        { "maxAmountKey", KeyBindingList.maxAmountKey },
        { "multiplyByTenAmountKey", KeyBindingList.multiplyByTenAmountKey },

        { "combatSelectKey", KeyBindingList.combatSelectKey },
        { "combatDeselectKey", KeyBindingList.combatDeselectKey },
        { "resolveTurnKey", KeyBindingList.resolveTurnKey },
        { "jumpMoveKey", KeyBindingList.jumpMoveKey },
        { "combatSettingsScreenKey", KeyBindingList.combatSettingsScreenKey },
        { "combatFastForwardAnimationKey", KeyBindingList.combatFastForwardAnimationKey },

        { "moveCounterClockwiseKey", KeyBindingList.moveCounterClockwiseKey },
        { "moveClockwiseKey", KeyBindingList.moveClockwiseKey },
    };

    private static bool canDrive
    {
        get => Application.isBatchMode;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void resetState()
    {
        keyboard = null;
        mouse = null;
        heldKeys.Clear();
        System.Array.Clear(heldMouseButtons, 0, heldMouseButtons.Length);
        mousePosition = Vector2.zero;
    }

    //Adds the virtual keyboard and mouse once, makes them current and turns off any other keyboard or
    //mouse, so nothing but the test drives input. Safe to call before every command.
    public static void ensureDevices()
    {
        if(!canDrive)
        {
            return;
        }

        if(keyboard == null || !keyboard.added)
        {
            keyboard = InputSystem.AddDevice<Keyboard>(keyboardName);
        }

        if(mouse == null || !mouse.added)
        {
            mouse = InputSystem.AddDevice<Mouse>(mouseName);
        }

        keyboard.MakeCurrent();
        mouse.MakeCurrent();

        foreach(InputDevice device in InputSystem.devices.ToArray())
        {
            if(device != keyboard && device != mouse && device.enabled && (device is Keyboard || device is Mouse))
            {
                InputSystem.DisableDevice(device);
            }
        }
    }

    public static bool tryGetKeyBind(string name, out KeyBind keyBind)
    {
        return keyBindsByName.TryGetValue(name, out keyBind);
    }

    #region Keys

    //A KeyBind presses whatever it's bound to right now, which may be a mouse button.
    public static void hold(KeyBind keyBind)
    {
        hold(keyBind.getCurrentKeyCode());
    }

    public static void release(KeyBind keyBind)
    {
        release(keyBind.getCurrentKeyCode());
    }

    public static void hold(KeyCode keyCode)
    {
        setButton(keyCode, true);
    }

    public static void release(KeyCode keyCode)
    {
        setButton(keyCode, false);
    }

    public static void releaseAll()
    {
        if(!canDrive)
        {
            return;
        }

        ensureDevices();

        heldKeys.Clear();
        System.Array.Clear(heldMouseButtons, 0, heldMouseButtons.Length);

        queueKeyboardState();
        queueMouseState(0f);
    }

    //Holds the key for the given number of frames, then releases it. Run it with StartCoroutine or yield it
    //from a coroutine.
    public static IEnumerator tap(KeyCode keyCode, int frames = minimumTapFrames)
    {
        hold(keyCode);

        for(int frame = 0; frame < Mathf.Max(frames, minimumTapFrames) - 1; frame++)
        {
            yield return null;
        }

        release(keyCode);
        yield return null;
    }

    public static IEnumerator tap(KeyBind keyBind, int frames = minimumTapFrames)
    {
        return tap(keyBind.getCurrentKeyCode(), frames);
    }

    private static void setButton(KeyCode keyCode, bool down)
    {
        if(!canDrive)
        {
            return;
        }

        ensureDevices();

        int mouseButton = keyCode.ToMouseButtonIndex();

        if(mouseButton >= 0)
        {
            setMouseButton(mouseButton, down);
            return;
        }

        if(!keyCode.TryGetInputKey(out InputKey key))
        {
            Debug.LogWarning("[VirtualInputDriver] " + keyCode + " has no Input System key and can't be pressed");
            return;
        }

        if(down)
        {
            heldKeys.Add(key);
        }
        else
        {
            heldKeys.Remove(key);
        }

        queueKeyboardState();
    }

    private static void queueKeyboardState()
    {
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(heldKeys.ToArray()));
    }

    #endregion

    #region Mouse

    //0 left, 1 right, 2 middle, 3 back, 4 forward - the legacy Mouse0-Mouse4 order.
    public static void setMouseButton(int button, bool down)
    {
        if(!canDrive || button < 0 || button > 4)
        {
            return;
        }

        ensureDevices();

        heldMouseButtons[button] = down;

        queueMouseState(0f);
    }

    public static void moveMouse(Vector2 screenPosition)
    {
        if(!canDrive)
        {
            return;
        }

        ensureDevices();

        mousePosition = screenPosition;
        queueMouseState(0f);
    }

    //Positive scrolls up. The Input System reports about 120 per notch on Windows.
    public static void scroll(float amount)
    {
        if(!canDrive)
        {
            return;
        }

        ensureDevices();

        queueMouseState(amount);
    }

    //Moves onto the target, presses and releases the left button, a frame apart, so hover, down, up and
    //click all reach it the way a player's click would.
    public static IEnumerator click(GameObject target)
    {
        if(!tryGetScreenPoint(target, out Vector2 point))
        {
            Debug.LogWarning("[VirtualInputDriver] can't find a screen point for " + (target == null ? "null" : target.name));
            yield break;
        }

        moveMouse(point);
        yield return null;

        setMouseButton(0, true);
        yield return null;

        setMouseButton(0, false);
        yield return null;
    }

    //Where the target is on screen: the centre of a UI element's rect, or of a world object's collider or
    //renderer as Camera.main sees it.
    public static bool tryGetScreenPoint(GameObject target, out Vector2 point)
    {
        point = Vector2.zero;

        if(target == null)
        {
            return false;
        }

        if(target.transform is RectTransform rectTransform)
        {
            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            Camera canvasCamera = canvas == null || canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            Vector3 worldCentre = rectTransform.TransformPoint(rectTransform.rect.center);

            point = RectTransformUtility.WorldToScreenPoint(canvasCamera, worldCentre);
            return true;
        }

        Camera camera = Camera.main;

        if(camera == null)
        {
            return false;
        }

        Vector3 centre = target.transform.position;

        Collider2D collider = target.GetComponent<Collider2D>();
        Renderer renderer = target.GetComponent<Renderer>();

        if(collider != null)
        {
            centre = collider.bounds.center;
        }
        else if(renderer != null)
        {
            centre = renderer.bounds.center;
        }

        point = camera.WorldToScreenPoint(centre);
        return true;
    }

    private static void queueMouseState(float scrollY)
    {
        MouseState state = new MouseState
        {
            position = mousePosition,
            scroll = new Vector2(0f, scrollY)
        };

        for(int button = 0; button < heldMouseButtons.Length; button++)
        {
            if(heldMouseButtons[button])
            {
                state = state.WithButton(mouseButtonsInLegacyOrder[button]);
            }
        }

        InputSystem.QueueStateEvent(mouse, state);
    }

    #endregion
}
