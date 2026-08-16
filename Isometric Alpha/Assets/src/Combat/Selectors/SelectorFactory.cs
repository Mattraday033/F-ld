using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectorTemplate
{
    Generated,
    PlayerCursor,
    Single,

    HorizontalOne,
    HorizontalTwo,
    HorizontalThree,
    HorizontalFour,

    VerticalOne,
    VerticalTwo,
    VerticalThree,
    VerticalFour,

    BoxOne,
    BoxTwo,
    BoxThree,

    HookOne,
    ReverseHookOne,

    L_One,
    ReverseL_One,

    CheckeredLeft,
    CheckeredRight,

    Cross
}

public static class SelectorFactory
{
    #region Selectors
    private static readonly Selector _PlayerCursor = new Selector(
        template: SelectorTemplate.PlayerCursor,
        width: 1,
        height: 1,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[1, 1] { { true } },
        alwaysWhite: true);
    public static Selector playerCursor { private set{} get{ return _PlayerCursor; } }

    private static readonly Selector _Single = new Selector(
        template: SelectorTemplate.Single,
        width: 1,
        height: 1,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[1, 1] { { true } });

    private static readonly Selector _HorizontalOne = new Selector(
        template: SelectorTemplate.HorizontalOne,
        width: 2,
        height: 1,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[1, 2] { { true, true } });

    private static readonly Selector _HorizontalTwo = new Selector(
        template: SelectorTemplate.HorizontalTwo,
        width: 3,
        height: 1,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[1, 3] { { true, true, true } });

    private static readonly Selector _HorizontalThree = new Selector(
        template: SelectorTemplate.HorizontalThree,
        width: 4,
        height: 1,
        startingCoords: new GridCoords(1, 0),
        spaces: new bool[1, 4] { { true, true, true, true } });

    private static readonly Selector _HorizontalFour = new Selector(
        template: SelectorTemplate.HorizontalFour,
        width: 4,
        height: 2,
        startingCoords: new GridCoords(1, 0),
        spaces: new bool[2, 4]
        {
            { true, true, true, true },
            { true, true, true, true },
        });

    private static readonly Selector _VerticalOne = new Selector(
        template: SelectorTemplate.VerticalOne,
        width: 1,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 1]
        {
            { true },
            { true },
        });

    private static readonly Selector _VerticalTwo = new Selector(
        template: SelectorTemplate.VerticalTwo,
        width: 1,
        height: 3,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[3, 1]
        {
            { true },
            { true },
            { true },
        });

    private static readonly Selector _VerticalThree = new Selector(
        template: SelectorTemplate.VerticalThree,
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

    private static readonly Selector _VerticalFour = new Selector(
        template: SelectorTemplate.VerticalFour,
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

    private static readonly Selector _BoxOne = new Selector(
        template: SelectorTemplate.BoxOne,
        width: 2,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 2]
        {
            { true, true },
            { true, true },
        });

    private static readonly Selector _BoxTwo = new Selector(
        template: SelectorTemplate.BoxTwo,
        width: 3,
        height: 3,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[3, 3]
        {
            { true, true, true },
            { true, true, true },
            { true, true, true },
        });

    private static readonly Selector _BoxThree = new Selector(
        template: SelectorTemplate.BoxThree,
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

    private static readonly Selector _HookOne = new Selector(
        template: SelectorTemplate.HookOne,
        width: 2,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 2]
        {
            { true, true },
            { false, true },
        });

    private static readonly Selector _ReverseHookOne = new Selector(
        template: SelectorTemplate.ReverseHookOne,
        width: 2,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 2]
        {
            { true, true },
            { true, false },
        });

    private static readonly Selector _L_One = new Selector(
        template: SelectorTemplate.L_One,
        width: 2,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 2]
        {
            { true, false },
            { true, true },
        });

    private static readonly Selector _ReverseL_One = new Selector(
        template: SelectorTemplate.ReverseL_One,
        width: 2,
        height: 2,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[2, 2]
        {
            { false, true },
            { true, true },
        });

    private static readonly Selector _CheckeredLeft = new Selector(
        template: SelectorTemplate.CheckeredLeft,
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

    private static readonly Selector _CheckeredRight = new Selector(
        template: SelectorTemplate.CheckeredRight,
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

    private static readonly Selector _Cross = new Selector(
        template: SelectorTemplate.Cross,
        width: 3,
        height: 3,
        startingCoords: new GridCoords(1, 1),
        spaces: new bool[3, 3]
        {
            { false, true, false },
            { true, true, true },
            { false, true, false },
        });

    #endregion

    #region Lookup

    public readonly static Dictionary<SelectorTemplate, Selector> selectorDict = new Dictionary<SelectorTemplate, Selector>();

    [RuntimeInitializeOnLoadMethod]
    public static void init()
    {
        if(selectorDict.Count > 0)
        {
            return;
        }

        selectorDict[SelectorTemplate.PlayerCursor]     = _PlayerCursor;
        selectorDict[SelectorTemplate.Single]           = _Single;
        selectorDict[SelectorTemplate.HorizontalOne]    = _HorizontalOne;
        selectorDict[SelectorTemplate.HorizontalTwo]    = _HorizontalTwo;
        selectorDict[SelectorTemplate.HorizontalThree]  = _HorizontalThree;
        selectorDict[SelectorTemplate.HorizontalFour]   = _HorizontalFour;
        selectorDict[SelectorTemplate.VerticalOne]      = _VerticalOne;
        selectorDict[SelectorTemplate.VerticalTwo]      = _VerticalTwo;
        selectorDict[SelectorTemplate.VerticalThree]    = _VerticalThree;
        selectorDict[SelectorTemplate.VerticalFour]    = _VerticalFour;
        selectorDict[SelectorTemplate.BoxOne]           = _BoxOne;
        selectorDict[SelectorTemplate.BoxTwo]           = _BoxTwo;
        selectorDict[SelectorTemplate.BoxThree]         = _BoxThree;
        selectorDict[SelectorTemplate.HookOne]          = _HookOne;
        selectorDict[SelectorTemplate.ReverseHookOne]   = _ReverseHookOne;
        selectorDict[SelectorTemplate.L_One]            = _L_One;
        selectorDict[SelectorTemplate.ReverseL_One]     = _ReverseL_One;
        selectorDict[SelectorTemplate.CheckeredLeft]    = _CheckeredLeft;
        selectorDict[SelectorTemplate.CheckeredRight]   = _CheckeredRight;
        selectorDict[SelectorTemplate.Cross]            = _Cross;
    }

    public static Selector buildByTemplate(SelectorTemplate selectorTemplate)
    {
        if(selectorDict.Count <= 0)
        {
            init();
        }

        if(selectorDict.ContainsKey(selectorTemplate))
        {
            return selectorDict[selectorTemplate].clone();
        }

        return selectorDict[SelectorTemplate.Single].clone();
    }

    #endregion

}
