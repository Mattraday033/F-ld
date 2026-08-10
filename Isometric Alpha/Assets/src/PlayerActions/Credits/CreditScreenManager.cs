using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreditScreenManager : MonoBehaviour
{
    public GameObject thankYouMessage;

    public Slider slider;

    private void Awake()
    {
        if(!Flags.isInNewGameMode() && thankYouMessage != null)
        {
            thankYouMessage.SetActive(true);
            StartCoroutine(playCreditsMusic());
        }

        StartCoroutine(ScrollCredits());
    }

    private IEnumerator playCreditsMusic()
    {
        yield return null;
        yield return null;
        yield return null;

        AudioManager.playNoMusic();
        AudioManager.playAudioClipAsSingleton(AudioClipList.winMusic);

        AudioClip winClip = AudioClipList.getAudioClip(AudioClipList.winMusic);

        if(winClip != null)
        {
            yield return new WaitForSeconds(winClip.length);
        }

        AudioManager.previousMusicPath = AudioClipList.winMusic;
        AudioManager.currentMusicPath = AudioClipList.campOverworld;
        AudioManager.addMusicFade();
        MusicFade.OnMusicMidFade.AddListener(LoadCampOverworld);
    }

    private static void LoadCampOverworld()
    {
        AudioManager.playMusicWithoutFade(AudioClipList.campOverworld);
        MusicFade.OnMusicMidFade.RemoveListener(LoadCampOverworld);
    }
 
    private IEnumerator ScrollCredits()
    {
        yield return new WaitForSeconds(9f);

        float duration = 15f;
        float elapsed = 0f;
        float currentValue = slider.value;

        if(slider.value != slider.maxValue)
        {
            slider.gameObject.SetActive(true);
            yield break;
        }

        while (elapsed < duration)
        {
            if(EventSystem.current.IsPointerOverGameObject() && Input.GetKey(KeyCode.Mouse0))
            {
                slider.gameObject.SetActive(true);
                yield break;
            }

            elapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(slider.maxValue, slider.minValue, elapsed / duration);
            currentValue = slider.value;
            yield return null;
        }

        slider.value = slider.minValue;
        slider.gameObject.SetActive(true);
    }

}
