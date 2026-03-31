using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScrollRectConnector : MonoBehaviour
{
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
        scrollRect.verticalNormalizedPosition = slider.value;
    }


}
