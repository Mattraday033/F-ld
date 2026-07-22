using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningMonologueManager : MonoBehaviour
{
    // public Slider slider;

    // public GameObject skipPrompt;
    // public TextMeshProUGUI skipPromptText;

    public TextMeshProUGUI continuePromptText;

    public Image artPanel;

    public GameObject scrollBackground;
    public TextMeshProUGUI scrollText;
    public TextMeshProUGUI centeredText;

    public Image quoteBackground;
    public TextMeshProUGUI quoteText;

    private bool canExit = false;

    private void Awake()
    {
        // continuePromptText.text = "Press <nobr>' " + KeyBindingList.acceptKey.getCurrentKeyCode().ToString() + " '</nobr> to continue...";
        // scrollText.text = openingText;

        StartCoroutine(FadeQuote());
    }

    // void Update()
    // {
    //     if(!canExit)
    //     {
    //         return;
    //     }

    //     if(KeyBindingList.continueUIKeyIsPressed() || KeyBindingList.settingsScreenOrBackKeyPressed())
    //     {
    //         SceneChange.changeSceneToOverworldWithLoadingScreen();
    //         return;
    //     } 

    //     // if(Input.anyKeyDown && !skipPrompt.activeSelf)
    //     // {
    //     //     skipPrompt.SetActive(true);
    //     //     return;
    //     // } 
    // }

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

        // StartCoroutine(ScrollMonologue()); // scroll starts half second before fade starts

        AudioManager.restartMusic();
        AudioManager.addMusicFade();

        yield return new WaitForSeconds(.5f); 

        FadeBackInTransition secondFadeIn = new();
        secondFadeIn.fadeTime = fadeInTime;
        FadeToBlackManager.createFade(secondFadeIn);

        yield return new WaitForSeconds(fadeInTime); // wait for the second fade in to finish

        canExit = true;

        StartCoroutine(FeedPages());
    }

    private const float letterInterval = .03f;
    private const float newlineInterval = .68f;

    // Feeds each page of the opening monologue to the scroll text box in turn.
    // Each page reveals one letter at a time; a spacebar press advances to the
    // next page once the current page has been fully shown.
    private IEnumerator FeedPages()
    {
        yield return StartCoroutine(FeedCenteredThenScroll(openingTextFirstPageCentered, openingTextFirstPageLeftAligned));
        yield return StartCoroutine(WaitForSpacePress());

        centeredText.gameObject.SetActive(false);

        yield return StartCoroutine(FeedPage(openingTextSecondPage));
        yield return StartCoroutine(WaitForSpacePress());

        yield return StartCoroutine(FeedPage(openingTextThirdPage));
        yield return StartCoroutine(WaitForSpacePress());

        yield return StartCoroutine(FeedPage(openingTextFourthPage));
        yield return StartCoroutine(WaitForSpacePress());

        yield return StartCoroutine(FeedPage(openingTextFifthPage));
        yield return StartCoroutine(WaitForSpacePress());        
        
        yield return StartCoroutine(FeedPage(openingTextSixthPage));
        yield return StartCoroutine(WaitForSpacePress());

        yield return StartCoroutine(FeedPage(openingTextSeventhPage));
        yield return StartCoroutine(WaitForSpacePress());

        SceneChange.changeSceneToOverworldWithLoadingScreen();
    }

    // Reveals a single page one letter at a time at letterInterval seconds per
    // letter. A spacebar press at any point dumps the remaining letters instantly.
    private IEnumerator FeedPage(string page)
    {
        scrollText.maxVisibleCharacters = 0;
        scrollText.text = page;
        scrollText.ForceMeshUpdate();

        int totalVisible = scrollText.textInfo.characterCount;
        int visible = 0;

        while (visible < totalVisible)
        {
            KeyPressManager.updateKeyBools();

            visible++;
            scrollText.maxVisibleCharacters = visible;

            // a revealed newline pauses longer before the next character is revealed
            float interval = scrollText.textInfo.characterInfo[visible - 1].character == '\n'
                ? newlineInterval
                : letterInterval;

            float elapsed = 0f;
            while (elapsed < interval)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !KeyPressManager.handlingPrimaryKeyPress)
                {
                    scrollText.maxVisibleCharacters = totalVisible;
                    KeyPressManager.handlingPrimaryKeyPress = true;
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    // Reveals centeredString into centeredText one letter at a time, then once it
    // is finished reveals nonCenteredString into scrollText the same way. A spacebar
    // press reveals all of both strings in their respective boxes and ends.
    private IEnumerator FeedCenteredThenScroll(string centeredString, string nonCenteredString)
    {
        centeredText.maxVisibleCharacters = 0;
        centeredText.text = centeredString;
        centeredText.ForceMeshUpdate();

        scrollText.maxVisibleCharacters = 0;
        scrollText.text = nonCenteredString;
        scrollText.ForceMeshUpdate();

        int centeredTotal = centeredText.textInfo.characterCount;
        int scrollTotal = scrollText.textInfo.characterCount;

        int visible = 0;
        while (visible < centeredTotal)
        {
            KeyPressManager.updateKeyBools();

            visible++;
            centeredText.maxVisibleCharacters = visible;

            // a revealed newline pauses longer before the next character is revealed
            float interval = centeredText.textInfo.characterInfo[visible - 1].character == '\n'
                ? newlineInterval
                : letterInterval;

            float elapsed = 0f;
            while (elapsed < interval)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !KeyPressManager.handlingPrimaryKeyPress)
                {
                    centeredText.maxVisibleCharacters = centeredTotal;
                    scrollText.maxVisibleCharacters = scrollTotal;
                    KeyPressManager.handlingPrimaryKeyPress = true;
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        visible = 0;
        while (visible < scrollTotal)
        {
            KeyPressManager.updateKeyBools();

            visible++;
            scrollText.maxVisibleCharacters = visible;

            // a revealed newline pauses longer before the next character is revealed
            float interval = scrollText.textInfo.characterInfo[visible - 1].character == '\n'
                ? newlineInterval
                : letterInterval;

            float elapsed = 0f;
            while (elapsed < interval)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !KeyPressManager.handlingPrimaryKeyPress)
                {
                    centeredText.maxVisibleCharacters = centeredTotal;
                    scrollText.maxVisibleCharacters = scrollTotal;
                    KeyPressManager.handlingPrimaryKeyPress = true;
                    yield break;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    // Waits for a fresh spacebar press, first ignoring any press still being
    // held from skipping the previous page's letter feed (so a single tap does
    // not both finish a page and advance past it).
    private IEnumerator WaitForSpacePress()
    {
        Color promptColor = continuePromptText.color;
        Coroutine pulse = StartCoroutine(PulseContinuePrompt(promptColor));

        while (!Input.GetKeyDown(KeyCode.Space) || KeyPressManager.handlingPrimaryKeyPress)
        {
            KeyPressManager.updateKeyBools();

            yield return null;
        }

        KeyPressManager.handlingPrimaryKeyPress = true;

        StopCoroutine(pulse);
        continuePromptText.color = promptColor;
    }

    // Pulses continuePromptText from its current color up to the scroll text's
    // color and back over a three second interval, looping until stopped.
    private IEnumerator PulseContinuePrompt(Color baseColor)
    {
        float elapsed = 0f;

        while (true)
        {
            float t = Mathf.PingPong(elapsed * (2f / 2.5f), 1f);
            continuePromptText.color = Color.Lerp(baseColor, Color.black, t);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private const string openingTextFirstPageCentered =  "the shrieks of comrades\n\n" + 
                                        "ringing clash of bronze on bronze\n\n" +
                                        "hooves thundering near\n\n\n";

    private const string openingTextFirstPageLeftAligned = "These sounds your ancestors knew well, having fought the Lovashi for decades.\n\nBut, for you, those times are over.";


    private const string openingTextSecondPage = "The <nobr>cousin-kingdoms</nobr> that still resist are distant now: far-flung embers of a conflict your kin no longer have the will to wage.\n\n" + 
                                        "The Folk of the Craft, heirs to what was once a proud culture, are all but a conquered people.\n\n" + 
                                        "Forced to live in squalor in their own lands, most of the Craft Folk now serve as serfs to their new Lords, the Counts of the Lovashi Confederation.";

    private const string openingTextThirdPage =  "The rest shelter beneath the last banners that still dare combat their oppressors: those of the Masons, Smiths, and Jewelers.\n\n" + 
                                        "Far away, and a generation ago, these last Craft Kingdoms rallied and won a great victory, putting the Rider Lords to route.\n\n" + 
                                        "Ever since, the Confederation has maintained an uneasy truce with the remnants of the free Craft Folk, and have turned their attentions inwards.";
    private const string openingTextFourthPage = "The current era is one choked with purges and crushed revolts, as the Lovashi make ready to resume their march.\n\n" + 
                                            "Great fields have been sewn. Grain reaped and stockpiled. Weapons battered into form. Monuments raised to their Beast god.\n\n" + 
                                            "All by the industry of your people's unwilling hands.";

    private const string openingTextFifthPage = "Any of your fellows that resisted their domitors have been given <nobr>\"the brand\":</nobr> a mark inflicted by applying a burning metal collar to the victim’s neck.\n\n" + 
                                        "In this way the Confederation designates the branded as a criminal and slave, to be worked until they expire.";

    private const string openingTextSixthPage = "Whether for crimes real or imagined, you have become one of these branded and are now being transported to the final days of your new life.";
    private const string openingTextSeventhPage = "As the wagons move along the forest path, you get what rest you can and escape to sleep, where your harassers cannot follow...";
                                                

}
