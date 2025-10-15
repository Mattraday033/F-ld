using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverIconDescriptionPanel : TutorialSequenceStepWindow
{
    private static int screenWidthFirstThird = (int) (((double) Screen.width) * (1.0 / 3.0));
    private static int screenWidthSecondThird = (int) (((double) Screen.width) * (2.0 / 3.0));

    private static int screenHeightFirstThird = (int) (((double) Screen.height) * (1.0 / 3.0));
    private static int screenHeightSecondThird = (int) (((double) Screen.height) * (2.0 / 3.0));

    private const int distanceFromHover = 15;

    public bool alwaysTop = false;
    public LayoutGroup thirdLayoutGroup;

    private void Awake()
    {
        parentRect = transform.parent.gameObject.GetComponent<RectTransform>();

        // setTopRectDimensions();
        Vector2 centerWorldPosition = topRectTransform.TransformPoint(topRectTransform.rect.center);

        ArrowDirection direction;

        if (alwaysTop)
        {
            direction = ArrowDirection.Bottom;
        } else
        {
            direction = getDirectionOfHover(RectTransformUtility.WorldToScreenPoint(Camera.main, centerWorldPosition));    
        }

        setAnchorsAndPivot(direction);
        setPadding();
    }

    public void setDescriptionText(string text)
    {
        DescriptionPanel.setText(useDescriptionText, text);
    }

    private static ArrowDirection getDirectionOfHover(Vector2 screenPoint)
    {
        //Vector2Int mousePos = Vector2Int.RoundToInt(Input.mousePosition);

        Vector2Int mousePos = Vector2Int.RoundToInt(screenPoint);

        if (mouseInWidthFirstSection(mousePos.x) && mouseInHeightFirstSection(mousePos.y)) { return ArrowDirection.BottomLeft;}
        if (mouseInWidthFirstSection(mousePos.x) && mouseInHeightSecondSection(mousePos.y)) { return ArrowDirection.Left;}
        if (mouseInWidthFirstSection(mousePos.x) && mouseInHeightThirdSection(mousePos.y)) { return ArrowDirection.TopLeft;}

        if (mouseInWidthSecondSection(mousePos.x) && mouseInHeightFirstSection(mousePos.y)) { return ArrowDirection.Bottom;}
        if (mouseInWidthSecondSection(mousePos.x) && mouseInHeightSecondSection(mousePos.y)) { return ArrowDirection.Bottom;}
        if (mouseInWidthSecondSection(mousePos.x) && mouseInHeightThirdSection(mousePos.y)) { return ArrowDirection.Top;}

        if (mouseInWidthThirdSection(mousePos.x) && mouseInHeightFirstSection(mousePos.y)) { return ArrowDirection.BottomRight;}
        if (mouseInWidthThirdSection(mousePos.x) && mouseInHeightSecondSection(mousePos.y)) { return ArrowDirection.Right;}
        if (mouseInWidthThirdSection(mousePos.x) && mouseInHeightThirdSection(mousePos.y)) { return ArrowDirection.TopRight;}

        return ArrowDirection.Top;
    }

    private void setAnchorsAndPivot(ArrowDirection direction)
    {
        switch (direction)
        {
            case ArrowDirection.Top:
                thirdTopRect.anchorMin = new Vector2(0.5f, 1f);
                thirdTopRect.anchorMax = new Vector2(0.5f, 1f);
                thirdTopRect.pivot = new Vector2(0.5f, 1f);
                break;
            case ArrowDirection.TopRight:
                thirdTopRect.anchorMin = new Vector2(1f, 1f);
                thirdTopRect.anchorMax = new Vector2(1f, 1f);
                thirdTopRect.pivot = new Vector2(1f, 1f);
                break;
            case ArrowDirection.Right:
                thirdTopRect.anchorMin = new Vector2(1f, 0.5f);
                thirdTopRect.anchorMax = new Vector2(1f, 0.5f);
                thirdTopRect.pivot = new Vector2(1f, 0.5f);
                break;
            case ArrowDirection.BottomRight:
                thirdTopRect.anchorMin = new Vector2(1f, 0f);
                thirdTopRect.anchorMax = new Vector2(1f, 0f);
                thirdTopRect.pivot = new Vector2(1f, 0f);
                break;
            case ArrowDirection.Bottom:
                thirdTopRect.anchorMin = new Vector2(0.5f, 0f);
                thirdTopRect.anchorMax = new Vector2(0.5f, 0f);
                thirdTopRect.pivot = new Vector2(0.5f, 0f);
                break;
            case ArrowDirection.BottomLeft:
                thirdTopRect.anchorMin = new Vector2(0f, 0f);
                thirdTopRect.anchorMax = new Vector2(0f, 0f);
                thirdTopRect.pivot = new Vector2(0f, 0f);
                break;
            case ArrowDirection.Left:
                thirdTopRect.anchorMin = new Vector2(0f, 0.5f);
                thirdTopRect.anchorMax = new Vector2(0f, 0.5f);
                thirdTopRect.pivot = new Vector2(0f, 0.5f);
                break;
            case ArrowDirection.TopLeft:
                thirdTopRect.anchorMin = new Vector2(0f, 1f);
                thirdTopRect.anchorMax = new Vector2(0f, 1f);
                thirdTopRect.pivot = new Vector2(0f, 1f);
                break;
            case ArrowDirection.Center:
                thirdTopRect.anchorMin = new Vector2(0.5f, 0.5f);
                thirdTopRect.anchorMax = new Vector2(0.5f, 0.5f);
                thirdTopRect.pivot = new Vector2(0.5f, 0.5f);
                break;
        }

        Helpers.updateGameObjectPosition(gameObject);
    }

    private void setPadding()
    {
        int widthPadding = ((int)topRectTransform.rect.width) + distanceFromHover;
        int heightPadding = ((int)topRectTransform.rect.height) + distanceFromHover;

        thirdLayoutGroup.padding.left = widthPadding;
        thirdLayoutGroup.padding.right = widthPadding;
        
        thirdLayoutGroup.padding.top = heightPadding;
        thirdLayoutGroup.padding.bottom = heightPadding;
    }

    private static bool mouseInWidthFirstSection(int mousePosX)
    {
        return mousePosX <= screenWidthFirstThird;
    }

    private static bool mouseInWidthSecondSection(int mousePosX)
    {
        return mousePosX >= screenWidthFirstThird && mousePosX <= screenWidthSecondThird;
    }

    private static bool mouseInWidthThirdSection(int mousePosX)
    {
        return mousePosX >= screenWidthSecondThird;
    }

    private static bool mouseInHeightFirstSection(int mousePosY)
    {
        return mousePosY <= screenHeightFirstThird;
    }

    private static bool mouseInHeightSecondSection(int mousePosY)
    {
        return mousePosY >= screenHeightFirstThird && mousePosY <= screenHeightSecondThird;
    }

    private static bool mouseInHeightThirdSection(int mousePosY)
    {
        return mousePosY >= screenHeightSecondThird;
    }
}
