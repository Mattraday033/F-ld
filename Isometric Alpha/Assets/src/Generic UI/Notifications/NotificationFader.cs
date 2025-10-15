using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotificationFader : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static UnityEvent OnMouseEnterNotificationPanel = new UnityEvent();
    public static UnityEvent OnMouseExitNotificationPanel = new UnityEvent();

    private const float waitBeforeFade = 5f;
    private const float fadeIncrement = .6f;

    private const float opaque = 1f;
    private const float transparent = 0f;

    public Image[] allImages;

    private bool fadingIn = true;
    private bool haltFade = false;
    private float elapsedTime = 0f;

    public bool skipFadeIn;

    private void Awake()
    {
        OnMouseEnterNotificationPanel.AddListener(resetOpacityAndHaltFade);
        OnMouseExitNotificationPanel.AddListener(resumeFade);
        FadeToBlackManager.OnFadeToBlack.AddListener(destroyGameObjectAndRemoveListener);
        NotificationManager.OnDeleteAllNotifications.AddListener(destroyGameObjectAndRemoveListener);

        if (skipFadeIn)
        {
            fadingIn = false;
        }
        else
        {
            foreach (Image image in allImages)
            {
                setImageOpacity(image, transparent);
            } 
        }
    }

    void Update()
    {
        if (haltFade)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        if(fadingIn)
        {
            foreach (Image image in allImages)
            {
                incrementImageOpacity(image);

                if (image.color.a >= 1f)
                {
                    fadingIn = false;
                }
            }
        }

        if (elapsedTime < waitBeforeFade)
        {
            return;
        }

        foreach (Image image in allImages)
        {
            decrementImageOpacity(image);

            if(image.color.a <= 0f)
            {
                destroyGameObjectAndRemoveListener();
                return;
            }
        }
    }

    private void destroyGameObjectAndRemoveListener()
    {
        OnMouseEnterNotificationPanel.RemoveListener(resetOpacityAndHaltFade);
        OnMouseExitNotificationPanel.RemoveListener(resumeFade);
        FadeToBlackManager.OnFadeToBlack.RemoveListener(destroyGameObjectAndRemoveListener);
        NotificationManager.OnDeleteAllNotifications.RemoveListener(destroyGameObjectAndRemoveListener);

        NotificationManager.skipWaitForNextNotificationSpawn();
        DestroyImmediate(gameObject);
    }

    private void incrementImageOpacity(Image image)
    {
        Color newColor = image.color;
        newColor.a += fadeIncrement * Time.deltaTime * 3f;

        image.color = newColor;
    }

    private void decrementImageOpacity(Image image)
    {
        Color newColor = image.color;
        newColor.a -= fadeIncrement * Time.deltaTime;

        image.color = newColor;
    }

    private void setImageOpacity(Image image, float newOpacity)
    {
        Color newColor = image.color;
        newColor.a = newOpacity;

        image.color = newColor;
    }

    private void resetOpacityAndHaltFade()
    {
        foreach (Image image in allImages)
        {
            setImageOpacity(image, opaque);
        }

        haltFade = true;
        elapsedTime = waitBeforeFade/2f;
    }

    private void resumeFade()
    {
        haltFade = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterNotificationPanel.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitNotificationPanel.Invoke();
    }
}
