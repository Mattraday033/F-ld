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

    private void Awake()
    {
        ScrollableUIElement.PanelsPopulated.AddListener(checkHandleVisibility);
        AbilityGridSideTab.OnSideTabChosen.AddListener(checkHandleVisibility);
    }

    private void OnDestroy()
    {
        ScrollableUIElement.PanelsPopulated.RemoveListener(checkHandleVisibility);
        AbilityGridSideTab.OnSideTabChosen.RemoveListener(checkHandleVisibility);
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
		// LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

		// Canvas.ForceUpdateCanvases();

        yield return new WaitForEndOfFrame();

        bool show = scrollRect.viewport.rect.height <= scrollRect.content.rect.height;

        handle.SetActive(show);
        background.SetActive(show);
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
