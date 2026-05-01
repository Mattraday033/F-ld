using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningMonologueManager : MonoBehaviour
{
    public Slider slider;

    public GameObject skipPrompt;
    public TextMeshProUGUI skipPromptText;

    public TextMeshProUGUI continuePromptText;

    public GameObject scrollBackground;
    public TextMeshProUGUI scrollText;

    public Image quoteBackground;
    public TextMeshProUGUI quoteText;

    private bool canExit = false;

    private void Awake()
    {
        continuePromptText.text = "Press <nobr>' " + KeyBindingList.acceptKey.getCurrentKeyCode().ToString() + " '</nobr> to continue...";
        scrollText.text = openingText;

        StartCoroutine(FadeQuote());
    }

    void Update()
    {
        if(!canExit)
        {
            return;
        }

        if(KeyBindingList.continueUIKeyIsPressed() || KeyBindingList.settingsScreenOrBackKeyPressed())
        {
            SceneChange.changeSceneToLoadingScreen();
            return;
        } 

        if(Input.anyKeyDown && !skipPrompt.activeSelf)
        {
            skipPrompt.SetActive(true);
            return;
        } 
    }

    private IEnumerator FadeQuote()
    {
        scrollBackground.gameObject.SetActive(false);
        quoteBackground.gameObject.SetActive(true);
        
        FadeToBlackManager.setToMaxOpacity();

        FadeToBlackManager.StopFade(FadeType.Screen);
        FadeToBlackManager.StopFade(FadeType.Music);

        float fadeInTime = 3f;

        yield return new WaitForSeconds(fadeInTime); // wait before fade in

        FadeBackInTransition firstFadeIn = new();
        firstFadeIn.fadeTime = fadeInTime;
        FadeToBlackManager.createFade(firstFadeIn);
        yield return new WaitForSeconds(fadeInTime); // time to fade in

        yield return new WaitForSeconds(fadeInTime*2f); //time quote is visible

        FadeToBlackTransition fadeToBlack = new(skipFadeIn: true);
        fadeToBlack.fadeTime = fadeInTime;
        FadeToBlackManager.createFade(fadeToBlack);

        yield return new WaitForSeconds(7f); // time to fade out and time for black screen between quote and scroll

        quoteBackground.gameObject.SetActive(false);
        scrollBackground.gameObject.SetActive(true);

        StartCoroutine(ScrollMonologue()); // scroll starts half second before fade starts

        AudioManager.restartMusic();
        AudioManager.addMusicFade();

        yield return new WaitForSeconds(.5f); 

        FadeBackInTransition secondFadeIn = new();
        secondFadeIn.fadeTime = fadeInTime;
        FadeToBlackManager.createFade(secondFadeIn);

        canExit = true;
    }

    private IEnumerator ScrollMonologue()
    {
        float duration = 130f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(slider.maxValue, slider.minValue, elapsed / duration);
            yield return null;
        }
    }

    private const string openingText =  "the shrieks of comrades\n\n" + 
                                        "ringing clash of bronze on bronze\n\n" +
                                        "hooves thundering near\n\n" + 

                                        "These sounds your ancestors knew well, having fought the Lovashi for decades. But, for you, those times are over. " + 
                                        "The <nobr>cousin-kingdoms</nobr> that still resist are distant now: far-flung embers of a conflict your kin no longer have the will to wage. " + 
                                        "The Folk of the Craft, heirs to what was once a proud culture, are now all but a conquered people.\n\n" + 

                                        "Forced to live in squalor in their own lands, most of the Craft Folk serve as serfs to their new Lords, the Counts of the Lovashi Confederation. " + 
                                        "Only three banners still blaze defiant against their oppressors: those of the Kingdoms of the Masons, Smiths, and Jewelers. " + 
                                        "Far away, and a generation ago, these last Craft Kingdoms rallied and won a great victory, putting the Rider Lords to route. " + 
                                        "Ever since, the Confederation has maintained an uneasy truce with the remnants of the free Craft Folk, and have turned their attentions inwards.\n\n" + 

                                        "The current era is one choked with purges and crushed revolts, as the Lovashi make ready to resume their march. " + 
                                        "Any of your fellows that resisted their domitors have been given \"the brand\": a mark inflicted by applying a burning metal collar to the victim’s neck. " + 
                                        "In this way the Confederation designates the branded as a criminal and slave, to be worked until they expire.\n\n" + 

                                        "Whether for crimes real or imagined, you have become one of these branded and are now being transported to the final days of your new life. " + 
                                        "As the wagons move along the forest path, you get what rest you can and escape to sleep, where your harassers cannot follow...";

}
