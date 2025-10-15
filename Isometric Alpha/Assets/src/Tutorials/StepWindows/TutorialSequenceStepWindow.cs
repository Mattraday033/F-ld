using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ArrowDirection { Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, TopLeft, Center } 
public class TutorialSequenceStepWindow : DescriptionPanel
{
    private static TutorialSequenceStepWindow instance;

    public GameObject skipText;

    public static TutorialSequenceStepWindow getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            GameObject.DestroyImmediate(instance.gameObject);
        }

        instance = this;

        if (skipText != null && TutorialSequence.currentTutorialSequence.isSkippable())
        {
            skipText.SetActive(true);
        } else if(skipText != null)
        {
            skipText.SetActive(false);
        }
    }

    public const float zeroPivot = 0f;
    public const float halfPivot = 0.5f;
    public const float fullPivot = 1f;
    public const float negativeFullPivot = -1f;

    public TutorialSequenceStep tutorialSequenceStep;
    public ITutorialSequenceTarget tutorialSequenceTarget;

    public RectTransform parentRect;

    public RectTransform topRectTransform;
    public LayoutGroup topLayoutGroup;

    public RectTransform secondTopRect;
    public RectTransform thirdTopRect;

    public RectTransform descPanelWindow;

    public RectTransform arrowPointer;
    public LayoutGroup arrowPositioningLayoutGroup;

    public void setTutorialSequenceStepAndTarget(TutorialSequenceStep tutorialSequenceStep, ITutorialSequenceTarget tutorialSequenceTarget, bool disableArrow)
    {
        if (disableArrow)
        {
            arrowPointer.gameObject.SetActive(false);
        }

        this.tutorialSequenceStep = tutorialSequenceStep;
        this.tutorialSequenceTarget = tutorialSequenceTarget; 
    }

    public void setArrowPointerPosition(ArrowDirection direction)
    {
        if (direction == ArrowDirection.Center)
        {
            arrowPointer.gameObject.SetActive(false);
            return;
        }

        arrowPositioningLayoutGroup.childAlignment = getArrowChildAlignment(direction);

        Canvas.ForceUpdateCanvases();
    }

    public void setWindowPosition(ArrowDirection direction)
    {
        topLayoutGroup.childAlignment = getTopRectChildAlignment(direction);

        Canvas.ForceUpdateCanvases();
    }

    public void rotatePointerTowardsTargetPosition(RectTransform targetTransform)
    {
        Vector3 toPosition = targetTransform.TransformPoint(targetTransform.rect.center);
        toPosition.z = 0f;

        Vector3 fromPosition = secondTopRect.TransformPoint(secondTopRect.rect.center);
        fromPosition.z = 0f;


        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = getAngleFromVectorFloat(dir);

        arrowPointer.localEulerAngles = new Vector3(0, 0, angle);
    }

    public static float getAngleFromVectorFloat(Vector3 dir) 
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public virtual void setWorldPositionForDescriptionPanel(RectTransform targetTransform, ArrowDirection directionToPanel)
    {
        if (targetTransform == null)
        {
            Debug.LogError("targetTransform is not assigned!");
            return;
        }

        parentRect = targetTransform;

        setTopRectDimensions();

        setWindowPosition(directionToPanel);
        setArrowPointerPosition(directionToPanel);
        rotatePointerTowardsTargetPosition(targetTransform);

        /*

        Vector2 playerTransformLocalScale = PlayerMovement.getTransform().localScale;

        //Vector2 arrowIndicatorSizeOffset = new Vector2( 100f * descriptionPanelRectTransform.localScale.x * playerTransformLocalScale.x,
        //                                                150f * descriptionPanelRectTransform.localScale.y * playerTransformLocalScale.y);

        //Canvas.ForceUpdateCanvases();

        Vector2 targetDimensions = tutorialSequenceTarget.getDimensions();

        Canvas.ForceUpdateCanvases();

        Vector2 distanceModifiers = new Vector2((secondTopRect.rect.width / 2F * (secondTopRect.localScale.x * playerTransformLocalScale.x)) + targetDimensions.x,
                                                (secondTopRect.rect.height / 2F * (secondTopRect.localScale.y * playerTransformLocalScale.y)) + targetDimensions.y);

        Vector2 directionModifiers = getDirectionModifiers(directionToPanel);
        Vector2 targetPosition = targetTransform.TransformPoint(targetTransform.rect.center);

		//Vector2 screenCenter = RectTransformUtility.WorldToScreenPoint(Camera.main, targetTransform.position);
        //Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenCenter.x, screenCenter.y, Camera.main.nearClipPlane + 1f));

        //RectTransformUtility.ScreenPointToWorldPointInRectangle(targetTransform, Camera.main.WorldToViewportPoint(targetTransform.position), null, out targetPosition);

        secondTopRect.position = new Vector2(targetPosition.x + (distanceModifiers.x * directionModifiers.x),
                                          targetPosition.y + (distanceModifiers.y * directionModifiers.y));
        */
    }

    public virtual void setTopRectDimensions()
    {
        topRectTransform.localPosition = Vector3.zero;

        Canvas.ForceUpdateCanvases();

        // Debug.LogError("getDescPanelWidth()*2.08f = " + getDescPanelWidth()*2.08f);
        // Debug.LogError("parentRect.rect.width = " + parentRect.rect.width);
        // Debug.LogError(" 1/topRectTransform.localScale.x = " +  (1 / topRectTransform.localScale.x));
        // Debug.LogError("getDescPanelHeight()*2 = " + getDescPanelHeight()*2f);
        // Debug.LogError("parentRect.rect.height = " + parentRect.rect.height);
        // Debug.LogError("1/topRectTransform.localScale.y = " + (1 / topRectTransform.localScale.y));

        topRectTransform.sizeDelta = new Vector2(getDescPanelWidth()*2.08f + (parentRect.rect.width * 1/topRectTransform.localScale.x), getDescPanelHeight() * 2.08f + (parentRect.rect.height * 1/topRectTransform.localScale.y));
    }

    public float getDescPanelWidth()
    {
        Canvas.ForceUpdateCanvases();

        return thirdTopRect.rect.width;
    }

    public float getDescPanelHeight()
    {
        Canvas.ForceUpdateCanvases();

        return thirdTopRect.rect.height;
    }

    public static TextAnchor getTopRectChildAlignment(ArrowDirection arrowDirection)
    {

        switch (arrowDirection)
        {
            case ArrowDirection.Top:
                return TextAnchor.UpperCenter;
            case ArrowDirection.TopRight:
                return TextAnchor.UpperRight;
            case ArrowDirection.Right:
                return TextAnchor.MiddleRight;
            case ArrowDirection.BottomRight:
                return TextAnchor.LowerRight;
            case ArrowDirection.Bottom:
                return TextAnchor.LowerCenter;
            case ArrowDirection.BottomLeft:
                return TextAnchor.LowerLeft;
            case ArrowDirection.Left:
                return TextAnchor.MiddleLeft;
            case ArrowDirection.TopLeft:
                return TextAnchor.UpperLeft;
            case ArrowDirection.Center:
                return TextAnchor.MiddleCenter;
        }

        return TextAnchor.MiddleRight;
    }

    public static TextAnchor getArrowChildAlignment(ArrowDirection arrowDirection)
    {

        switch (arrowDirection)
        {
            case ArrowDirection.Top:
                return TextAnchor.LowerCenter;
            case ArrowDirection.TopRight:
                return TextAnchor.LowerLeft;
            case ArrowDirection.Right:
                return TextAnchor.MiddleLeft;
            case ArrowDirection.BottomRight:
                return TextAnchor.UpperLeft;
            case ArrowDirection.Bottom:
                return TextAnchor.UpperCenter;
            case ArrowDirection.BottomLeft:
                return TextAnchor.UpperRight;
            case ArrowDirection.Left:
                return TextAnchor.MiddleRight;
            case ArrowDirection.TopLeft:
                return TextAnchor.LowerRight;
        }

        return TextAnchor.MiddleLeft;
    }

    public static Vector2 getAnchorMinMax(ArrowDirection arrowDirection)
    {

        switch (arrowDirection)
        {
            case ArrowDirection.Top:
                return new Vector2(halfPivot, zeroPivot);
            case ArrowDirection.TopRight:
                return new Vector2(zeroPivot, zeroPivot);
            case ArrowDirection.Right:
                return new Vector2(zeroPivot, halfPivot);
            case ArrowDirection.BottomRight:
                return new Vector2(zeroPivot, fullPivot);
            case ArrowDirection.Bottom:
                return new Vector2(halfPivot, fullPivot);
            case ArrowDirection.BottomLeft:
                return new Vector2(fullPivot, fullPivot);
            case ArrowDirection.Left:
                return new Vector2(fullPivot, halfPivot);
            case ArrowDirection.TopLeft:
                return new Vector2(fullPivot, zeroPivot);
        }

        return new Vector3(fullPivot, fullPivot);
    }

    public static Vector2 getDirectionModifiers(ArrowDirection arrowDirection)
    {

        switch (arrowDirection)
        {
            case ArrowDirection.Top:
                return new Vector2(0, fullPivot);

            case ArrowDirection.TopRight:
                return new Vector2(fullPivot, fullPivot);

            case ArrowDirection.Right:
                return new Vector2(fullPivot, 0);

            case ArrowDirection.BottomRight:
                return new Vector2(fullPivot, negativeFullPivot);

            case ArrowDirection.Bottom:
                return new Vector2(0, negativeFullPivot);

            case ArrowDirection.BottomLeft:
                return new Vector2(negativeFullPivot, negativeFullPivot);

            case ArrowDirection.Left:
                return new Vector2(negativeFullPivot, 0);

            case ArrowDirection.TopLeft:
                return new Vector2(negativeFullPivot, fullPivot);
        }

        return new Vector3(fullPivot, fullPivot, 0);
    }


    private void OnEnable()
    {
        TutorialSequence.DestroyAllTutorialMessageWindows.AddListener(destroyGameObject);
    }

    private void OnDestroy()
    {
        TutorialSequence.DestroyAllTutorialMessageWindows.RemoveListener(destroyGameObject);
    }

    private void destroyGameObject()
    {
        if (tutorialSequenceTarget != null && !(tutorialSequenceTarget is null))
        {
            tutorialSequenceTarget.unhighlight(tutorialSequenceStep.skipUnhighlight); 
        }

        GameObject.DestroyImmediate(gameObject);
    }

}
