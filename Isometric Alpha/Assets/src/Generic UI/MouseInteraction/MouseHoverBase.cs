using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoverBase : MonoBehaviour
{

    private const int fullyVisable = 1;

    public RectTransform baseRectTransform;

    public CanvasGroup canvasGroup;

    private void Awake()
    {
        StartCoroutine(revealWhenReady(canvasGroup));
    }

    public void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        baseRectTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
    }

    public static IEnumerator revealWhenReady(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;

        yield return null;

        yield return null;

        canvasGroup.alpha = fullyVisable;
    }

    private void OnEnable()
    {
        MouseHoverManager.OnHoverPanelCreation.AddListener(destroyHover);
    }

    private void OnDestroy()
    {
        MouseHoverManager.OnHoverPanelCreation.RemoveListener(destroyHover);
    }

    private void destroyHover()
    {
        DestroyImmediate(gameObject);
    }

}
