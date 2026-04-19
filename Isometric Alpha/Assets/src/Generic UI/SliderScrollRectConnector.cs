using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScrollRectConnector : MonoBehaviour
{
    public bool valueChangedByScrollRect = false; //if the scroll rect is being moved with the mouse wheel
    public bool valueChangedBySlider = false; //if the slider is being dragged
    public GameObject handle;
    public GameObject background;

    public Slider slider;
    public ScrollRect scrollRect;

    void Start()
    {
        checkHandleVisibility();
        ScrollableUIElement.PanelsPopulated.AddListener(checkHandleVisibility);
        AbilityGridSideTab.OnSideTabChosen.AddListener(checkHandleVisibility);
        GridRow.OnDescribableToDisplay.AddListener(checkHandleVisibility);
    }

    private void OnDestroy()
    {
        ScrollableUIElement.PanelsPopulated.RemoveListener(checkHandleVisibility);
        AbilityGridSideTab.OnSideTabChosen.RemoveListener(checkHandleVisibility);
        GridRow.OnDescribableToDisplay.RemoveListener(checkHandleVisibility);
    }

    public void checkHandleVisibility(object obj)
    {
        checkHandleVisibility();
    }

    public void checkHandleVisibility()
    {
        if(scrollRect == null || handle == null || background == null)
        {
            OnDestroy();
            return;
        }

        StartCoroutine(waitThenCheckVisibility());
    }
    private IEnumerator waitThenCheckVisibility()
    {	
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        bool show = scrollRect.viewport.rect.height <= scrollRect.content.rect.height;

        handle.SetActive(show);
        background.SetActive(show);

        valueChangedByScrollRect = true;

        slider.value = 1f;

        valueChangedByScrollRect = false;
    }

    public void setScrollRectVerticalPosition()
    {
        if(valueChangedByScrollRect)
        {
            return;
        }

        valueChangedBySlider = true;

        scrollRect.verticalNormalizedPosition = slider.value;

        valueChangedBySlider = false;
    }

    public void setSliderValue()
    {
        if(valueChangedBySlider)
        {
            return;
        }

        valueChangedByScrollRect = true;

        slider.value = scrollRect.verticalNormalizedPosition;

        valueChangedByScrollRect = false;

    }

}
