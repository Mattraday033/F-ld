using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiTargetButton : Button
{
    private Image[] additionalTargetGraphics;

    protected override void Awake()
    {
        base.Awake();

        additionalTargetGraphics = GetComponentsInChildren<Image>(true);
    }
    
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        // First, do the normal transition for the main target graphic
        base.DoStateTransition(state, instant);

        // Get the appropriate color based on the state
        Color color;
        switch (state)
        {
            case SelectionState.Normal:
                color = colors.normalColor;
                break;
            case SelectionState.Highlighted:
                color = colors.highlightedColor;
                break;
            case SelectionState.Pressed:
                color = colors.pressedColor;
                break;
            case SelectionState.Selected:
                color = colors.selectedColor;
                break;
            case SelectionState.Disabled:
                color = colors.disabledColor;
                break;
            default:
                color = Color.white;
                break;
        }

        // Apply the color to all additional target graphics
        foreach (Graphic graphic in additionalTargetGraphics)
        {
            if (graphic == null)
            {
                continue;
            }

            graphic.CrossFadeColor(color * colors.colorMultiplier,
                                  instant ? 0f : colors.fadeDuration,
                                  true,
                                  true);
        }
    }

}
