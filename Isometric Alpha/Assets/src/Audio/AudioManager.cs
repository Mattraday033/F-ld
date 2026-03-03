using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public const bool fadeOut = true;

    #region Music Volume Values
    public const float musicVolumeMaximum = 1f;
    public const float musicVolumeMinimum = 0f;

    public static float musicVolumePlayerSetting = 1f; 
    #endregion

    private static AudioManager instance;

    public static string previousMusicPath;
    public static string currentMusicPath;

    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource SFXSource;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(LayerAndTagManager.musicTag);

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(mode == LoadSceneMode.Additive || scene.name.Equals(SceneNameList.loadingScreen))
        {
            return;
        }

        DontDestroyOnLoad(gameObject);

        StartCoroutine(waitTwoFramesThenStartMusic());
    }

    private IEnumerator waitTwoFramesThenStartMusic()
    {
        yield return null;
        yield return null;

        instance = this;
        setMusicSourceVolume(musicVolumePlayerSetting);

        if(Flags.isInNewGameMode())
        {
            addMusicFade();
            setCurrentMusicPath(AudioClipList.campOverworld);
            loadAndPlayCurrentMusicClip();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static void setCurrentMusicPath(string newMusicPath)
    {
        previousMusicPath = currentMusicPath;
        currentMusicPath = newMusicPath;
    }

    public static void setMusicSourceVolume(float volumePercent)
    {
        if(volumePercent < 0)
        {
            volumePercent = 0f;
        } else if(volumePercent > 1f)
        {
            volumePercent = 1f;
        }

        instance.musicSource.volume = volumePercent;
    }

    public static void setSFXSourceVolume(float volumePercent)
    {
        if(volumePercent < 0)
        {
            volumePercent = 0f;
        } else if(volumePercent > 1f)
        {
            volumePercent = 1f;
        }

        instance.SFXSource.volume = volumePercent;
    }

    public static void playNextAreaMusic(string newAreaName)
    {
        string newAreaMusicPath = AreaList.getArea(newAreaName).musicPath;

        if(newAreaMusicPath.Equals(currentMusicPath))
        {
            return;
        } else
        {
            setCurrentMusicPath(newAreaMusicPath);
            addMusicFade();
            MusicFade.OnMusicMidFade.AddListener(loadAndPlayCurrentMusicClip);
        }
    }

    public static void playBattleMusic()
    {
        loadAndPlayClip(instance.musicSource, AudioClipList.campBattle);
    }

    private static void loadAndPlayCurrentMusicClip()
    {
        loadAndPlayClip(instance.musicSource, currentMusicPath);
        MusicFade.OnMusicMidFade.RemoveListener(loadAndPlayCurrentMusicClip);
    }

    private static void loadAndPlayClip(AudioSource source, string clipPath)
    {
        source.clip = Resources.Load<AudioClip>(clipPath);
        source.Play();
    }

    public static void addMusicFade()
    {
        BetweenAreaFade fade = new BetweenAreaFade(fadeOut, previousMusicPath, currentMusicPath);

        FadeToBlackManager.createFade(fade);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateAudioManager()
    {
        instance = null;
        previousMusicPath = "";
        currentMusicPath = "";
        TransitionManager.ChangeAreaMusic.AddListener(playNextAreaMusic);
        musicVolumePlayerSetting = musicVolumeMaximum;
    }

}

public static class AudioClipList
{
    public const string audioFolderPath = "Audio/";

    public const string musicFolderPath = audioFolderPath + "Music/";

    public const string campOverworld = musicFolderPath + "Camp Overworld";
    public const string campInterior = musicFolderPath + "Camp Interior";
    public const string campBattle = musicFolderPath + "Camp Battle";
    public const string deathMusic = musicFolderPath + "Dead";
    public const string caveOne = musicFolderPath + "Cave 1";

}