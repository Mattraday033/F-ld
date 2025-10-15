using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SlidePanel { Default = 0, Large1 = 1 }

// [System.Serializable]
public struct SlideShowCoroutineDetails
{

    public SlidePanel slidePanel;
    public bool hasDetails;

    public float startX;
    public float endX;

    public float waitAtEnd;
}

public class SlideShowManager : MonoBehaviour
{

    private const float waitMultiplier = 1000f;

    public float endWait;
    public List<float> times;

    public List<string> subtitles;
    public TextMeshProUGUI subtitleText;

    private int slideIndex = 0;
    public List<Sprite> slides;

    public List<SlideShowCoroutineDetails> details;

    public Image blackScreen;

    public GameObject largeSlide;
    public Image slideImage;

    private ReturnToMainMenu toMainMenu = new ReturnToMainMenu();
    private Coroutine currentSlideShow;

    void Start()
    {
        currentSlideShow = StartCoroutine(executeSlideShow());
    }

    void Update()
    {
        if ((KeyBindingList.continueUIKeyIsPressed() || KeyBindingList.eitherBackoutKeyIsPressed()) && !KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.handlingPrimaryKeyPress = true;
            toMainMenu.execute();
        }
    }

    private IEnumerator executeSlideShow()
    {

        while (!FadeToBlackManager.isTransparent())
        {
            yield return null;
        }

        subtitleText.text = subtitles[slideIndex];

        float timeWaited = 0f;
        float nextWait = getNextWait(0);

        while (slideIndex < slides.Count)
        {
            while (timeWaited < nextWait)
            {
                timeWaited += Time.deltaTime;
                yield return null;
            }

            nextWait += nextSlide();
        }

        FadeToBlackManager.getInstance().fadeTime = 5.5f;

        FadeToBlackManager.getInstance().setAndStartFadeToBlack();

        while (!FadeToBlackManager.isBlack())
        {
            yield return null;
        }

        blackScreen.gameObject.SetActive(true);

        toMainMenu.execute();
    }

    private float nextSlide()
    {

        slideIndex++;

        if (slideIndex >= slides.Count)
        {
            return 0f;
        }

        setPanel();

        if (details[slideIndex].hasDetails)
        {
            StartCoroutine(movePanel());
        }

        slideImage.sprite = slides[slideIndex];
        subtitleText.text = subtitles[slideIndex];

        return getNextWait(slideIndex);
    }

    private void setPanel()
    {
        switch (details[slideIndex].slidePanel)
        {
            case SlidePanel.Large1:
                largeSlide.SetActive(true);
                slideImage.gameObject.SetActive(false);
                break;
            default:
                largeSlide.SetActive(false);
                slideImage.gameObject.SetActive(true);
                break;
        }
    }

    private IEnumerator movePanel()
    {
        int index = slideIndex;
        SlideShowCoroutineDetails currendDetails = details[index];


        if (!currendDetails.hasDetails)
        {
            yield break;
        }

        RectTransform panelTransform;

        switch (currendDetails.slidePanel)
        {
            case SlidePanel.Large1:
                panelTransform = largeSlide.GetComponent<RectTransform>();
                break;
            default:
                panelTransform = slideImage.gameObject.GetComponent<RectTransform>();
                break;
        }

        float wait = 0f;

        while (index == slideIndex)
        {
            panelTransform.anchoredPosition = new Vector3(Mathf.Lerp(currendDetails.startX, currendDetails.endX, wait / (times[index] - details[index].waitAtEnd)), 0f, 0f);

            wait += Time.deltaTime;

            yield return null;
        }
    }

    private float getNextWait(int index)
    {
        return times[index];
    }

}
