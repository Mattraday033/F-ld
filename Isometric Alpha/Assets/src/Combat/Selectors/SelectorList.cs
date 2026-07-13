using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectorList
{
    #region Selector Names

    public const string playerCursorName  = "Player Cursor";
    public const string singleName        = "Single";

    public const string horizontalOneName   = "Horizontal 1";
    public const string horizontalTwoName   = "Horizontal 2";
    public const string horizontalThreeName = "Horizontal 3";
    public const string horizontalFourName  = "Horizontal 4";

    public const string verticalOneName   = "Vertical 1";
    public const string verticalTwoName   = "Vertical 2";
    public const string verticalThreeName = "Vertical 3";
    public const string verticalFourName = "Vertical 4";

    public const string boxOneName   = "Box 1";
    public const string boxTwoName   = "Box 2";
    public const string boxThreeName = "Box 3";

    public const string hookOneName        = "Hook 1";
    public const string reverseHookOneName = "Reverse Hook 1";

    public const string L_OneName        = "L 1";
    public const string reverseL_OneName = "Reverse L 1";

    public const string checkeredLeftName  = "Checkered Left";
    public const string checkeredRightName = "Checkered Right";

    public const string crossName = "Cross";

    #endregion

    #region Selectors

    private static readonly Selector _PlayerCursor = new Selector(
        name: playerCursorName,
        width: 1,
        height: 1,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[1, 1] { { true } },
        alwaysWhite: true);
    public static Selector playerCursor { private set{} get{ return _PlayerCursor; } }

    private static readonly Selector _Single = new Selector(
        name: singleName,
        width: 1,
        height: 1,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[1, 1] { { true } });
    public static Selector single { private set{} get{ return _Single; } }

    private static readonly Selector _HorizontalOne = new Selector(
        name: horizontalOneName,
        width: 2,
        height: 1,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[1, 2] { { true, true } });
    public static Selector horizontalOne { private set{} get{ return _HorizontalOne; } }

    private static readonly Selector _HorizontalTwo = new Selector(
        name: horizontalTwoName,
        width: 3,
        height: 1,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[1, 3] { { true, true, true } });
    public static Selector horizontalTwo { private set{} get{ return _HorizontalTwo; } }

    private static readonly Selector _HorizontalThree = new Selector(
        name: horizontalThreeName,
        width: 4,
        height: 1,
        startingCoords: new GridCoords(1, 0),
        spaces: new bool[1, 4] { { true, true, true, true } });
    public static Selector horizontalThree { private set{} get{ return _HorizontalThree; } }

    private static readonly Selector _HorizontalFour = new Selector(
        name: horizontalFourName,
        width: 4,
        height: 2,
        startingCoords: new GridCoords(1, 0),
        spaces: new bool[2, 4]
        {
            { true, true, true, true },
            { true, true, true, true },
        });
    public static Selector horizontalFour { private set{} get{ return _HorizontalFour; } }

    private static readonly Selector _VerticalOne = new Selector(
        name: verticalOneName,
        width: 1,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 1]
        {
            { true },
            { true },
        });
    public static Selector verticalOne { private set{} get{ return _VerticalOne; } }

    private static readonly Selector _VerticalTwo = new Selector(
        name: verticalTwoName,
        width: 1,
        height: 3,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[3, 1]
        {
            { true },
            { true },
            { true },
        });
    public static Selector verticalTwo { private set{} get{ return _VerticalTwo; } }

    private static readonly Selector _VerticalThree = new Selector(
        name: verticalThreeName,
        width: 1,
        height: 4,
        startingCoords: new GridCoords(0, 1),
        spaces: new bool[4, 1]
        {
            { true },
            { true },
            { true },
            { true },
        });
    public static Selector verticalThree { private set{} get{ return _VerticalThree; } }

    private static readonly Selector _VerticalFour = new Selector(
        name: verticalFourName,
        width: 2,
        height: 4,
        startingCoords: new GridCoords(0, 1),
        spaces: new bool[4, 2]
        {
            { true , true },
            { true , true },
            { true , true },
            { true , true },
        });
    public static Selector verticalFour { private set{} get{ return _VerticalFour; } }

    private static readonly Selector _BoxOne = new Selector(
        name: boxOneName,
        width: 2,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 2]
        {
            { true, true },
            { true, true },
        });
    public static Selector boxOne { private set{} get{ return _BoxOne; } }

    private static readonly Selector _BoxTwo = new Selector(
        name: boxTwoName,
        width: 3,
        height: 3,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[3, 3]
        {
            { true, true, true },
            { true, true, true },
            { true, true, true },
        });
    public static Selector boxTwo { private set{} get{ return _BoxTwo; } }

    private static readonly Selector _BoxThree = new Selector(
        name: boxThreeName,
        width: 4,
        height: 4,
        startingCoords: new GridCoords(0, 0),
        spaces: new bool[4, 4]
        {
            { true, true, true, true },
            { true, true, true, true },
            { true, true, true, true },
            { true, true, true, true },
        });
    public static Selector boxThree { private set{} get{ return _BoxThree; } }

    private static readonly Selector _HookOne = new Selector(
        name: hookOneName,
        width: 2,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 2]
        {
            { true, true },
            { false, true },
        });
    public static Selector hookOne { private set{} get{ return _HookOne; } }

    private static readonly Selector _ReverseHookOne = new Selector(
        name: reverseHookOneName,
        width: 2,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 2]
        {
            { true, true },
            { true, false },
        });
    public static Selector reverseHookOne { private set{} get{ return _ReverseHookOne; } }

    private static readonly Selector _L_One = new Selector(
        name: L_OneName,
        width: 2,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 2]
        {
            { true, false },
            { true, true },
        });
    public static Selector L_One { private set{} get{ return _L_One; } }

    private static readonly Selector _ReverseL_One = new Selector(
        name: reverseL_OneName,
        width: 2,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 2]
        {
            { false, true },
            { true, true },
        });
    public static Selector reverseL_One { private set{} get{ return _ReverseL_One; } }

    private static readonly Selector _CheckeredLeft = new Selector(
        name: checkeredLeftName,
        width: 4,
        height: 4,
        startingCoords: new GridCoords(0, 0),
        spaces: new bool[4, 4]
        {
            { true, false, true, false },
            { false, true, false, true },
            { true, false, true, false },
            { false, true, false, true },
        });
    public static Selector checkeredLeft { private set{} get{ return _CheckeredLeft; } }

    private static readonly Selector _CheckeredRight = new Selector(
        name: checkeredRightName,
        width: 4,
        height: 4,
        startingCoords: new GridCoords(0, 0),
        spaces: new bool[4, 4]
        {
            { false, true, false, true },
            { true, false, true, false },
            { false, true, false, true },
            { true, false, true, false },
        });
    public static Selector checkeredRight { private set{} get{ return _CheckeredRight; } }

    private static readonly Selector _Cross = new Selector(
        name: crossName,
        width: 3,
        height: 3,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[3, 3]
        {
            { false, true, false },
            { true, true, true },
            { false, true, false },
        });
    public static Selector cross { private set{} get{ return _Cross; } }

    #endregion

    #region Lookup

    public readonly static Dictionary<string, Selector> selectorDict = new Dictionary<string, Selector>();

    [RuntimeInitializeOnLoadMethod]
    public static void init()
    {
        if(selectorDict.Count > 0)
        {
            return;
        }

        selectorDict[playerCursorName]     = _PlayerCursor;
        selectorDict[singleName]           = _Single;
        selectorDict[horizontalOneName]    = _HorizontalOne;
        selectorDict[horizontalTwoName]    = _HorizontalTwo;
        selectorDict[horizontalThreeName]  = _HorizontalThree;
        selectorDict[horizontalFourName]   = _HorizontalFour;
        selectorDict[verticalOneName]      = _VerticalOne;
        selectorDict[verticalTwoName]      = _VerticalTwo;
        selectorDict[verticalThreeName]    = _VerticalThree;
        selectorDict[verticalFourName]    = _VerticalFour;
        selectorDict[boxOneName]           = _BoxOne;
        selectorDict[boxTwoName]           = _BoxTwo;
        selectorDict[boxThreeName]         = _BoxThree;
        selectorDict[hookOneName]          = _HookOne;
        selectorDict[reverseHookOneName]   = _ReverseHookOne;
        selectorDict[L_OneName]            = _L_One;
        selectorDict[reverseL_OneName]     = _ReverseL_One;
        selectorDict[checkeredLeftName]    = _CheckeredLeft;
        selectorDict[checkeredRightName]   = _CheckeredRight;
        selectorDict[crossName]            = _Cross;
    }

    public static Selector getByName(string selectorName)
    {
        if(selectorDict.ContainsKey(selectorName))
        {
            return selectorDict[selectorName];
        }

        return single;
    }

    public static void resetAllSelectors()
	{
		foreach (Selector selector in selectorDict.Values)
		{
			selector.setToStartLocation();
		}

		playerCursor.setToCurrentSelector();
	}

    #endregion

}
