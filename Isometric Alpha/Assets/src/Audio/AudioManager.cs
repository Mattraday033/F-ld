using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public const bool fadeOut = true;
    public static bool playLeftFootstep = true;

    #region Music Volume Values
    public const float musicVolumeMaximum = 1f;
    public const float musicVolumeMinimum = 0f;

    public static float musicVolumePlayerSetting = 1f; 
    public static float footstepPlayerSetting = 1f;
    public const float footstepMufflePercent = .6f;

    public static float sfxVolumePlayerSetting = 1f;
    public const float sfxMufflePercent = .6f;

    #endregion

    private static bool playSFXOnNextHeartBeat;
    private static AudioManager instance;

    public static string previousMusicPath;
    public static string currentMusicPath;

    public Coroutine playingQueuedAudioClips;

    [SerializeField]
    private AudioSource musicSource;    
    [SerializeField]
    private AudioSource SFXSource;
    [SerializeField]
    private AudioSource footStepSource;

    public static List<string> audioClipPathQueue;
    public static Dictionary<AudioClip, AudioSource> singletonAudioClips;

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

        destroyAllSingletonAudioSources();

        DontDestroyOnLoad(gameObject);

        StartCoroutine(waitTwoFramesThenStartMusic());
    }

    private static void destroyAllSingletonAudioSources()
    {
        if(singletonAudioClips == null)
        {
            singletonAudioClips = new Dictionary<AudioClip, AudioSource>();
        }

        foreach(AudioSource source in singletonAudioClips.Values)
        {
            if(source != null)
            {
                Destroy(source.gameObject);
            }
        }
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

    public static void setFootStepSourceVolume(float volumePercent)
    {
        if(volumePercent < 0)
        {
            volumePercent = 0f;
        } else if(volumePercent > 1f)
        {
            volumePercent = 1f;
        }

        instance.footStepSource.volume = volumePercent*footstepMufflePercent;
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
        FadeToBlackManager.StopFade(FadeType.Music);
        setMusicSourceVolume(musicVolumePlayerSetting);
        loadAndPlayClip(instance.musicSource, AudioClipList.campBattle);
        setCurrentMusicPath(AudioClipList.campBattle);
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

    public static void playSFX(string SFXClipPath)
    {
        AudioClip SFXClip = Resources.Load<AudioClip>(SFXClipPath);

        if(SFXClip == null)
        {
            return;
        }

        instance.SFXSource.clip = SFXClip;
        instance.SFXSource.Play();
    }

    public static void playCoinSFX()
    {
        playSFX(AudioClipList.coinSFXPrefix + 
                Random.Range(Constants.indexOne, AudioClipList.coinSFXCount + 1));
    }

    public static void playWeaponChangeSFX()
    {
        playSFX(AudioClipList.weaponPrefix + 
                Random.Range(Constants.indexOne, AudioClipList.weaponSFXCount + 1));
    }

    public static void playEffectAnimationSFX(string effectType)
    {
        if(effectType.Equals(EffectAnimationType.Default.ToString()) || 
            effectType.Equals(EffectAnimationType.BatSwarm.ToString()))
        {
            return;
        }

        queueAudioClip(AudioClipList.hitSFXFolder + effectType + "/" + effectType);
    }

    public static void queueSFX(string audioClipPath)
    {
        queueAudioClip(audioClipPath);
    }

    public static void playFootStep(int row)
    {
        if(CombatStateManager.inCombat || (PlayerMovement.getInstance() != null && 
            !PlayerMovement.getInstance().isMoving()))
        {
            return;
        }

        AudioSource footStepSource = instance.footStepSource;

        if(playLeftFootstep)
        {
            footStepSource.pitch = .8f;
        } else
        {
           footStepSource.pitch = 1.2f;
        }

        playLeftFootstep = !playLeftFootstep;

        FootStepType footStepType = AreaList.getAreaFootStepType();
        string footstepSFXFolderPath = AudioClipList.footstepFolderPath + footStepType.ToString() + "/" + AudioClipList.footstepSFXFilePrefix;
        int highestFootStepSFX = 2;

        switch(footStepType)
        {
            case FootStepType.Dirt:
                highestFootStepSFX = 7;
                break;
            case FootStepType.WoodFloor:
                highestFootStepSFX = 5;
                break;
            default:
                highestFootStepSFX = 9;
                break;
        }

        int soundEffectNumber = Random.Range(Constants.indexOne, highestFootStepSFX + 1);

        AudioClip footstepClip = Resources.Load<AudioClip>(footstepSFXFolderPath + soundEffectNumber);

        if(footstepClip != null)
        {
            footStepSource.clip = footstepClip;
            footStepSource.Play();
        }
    }

    private static float getSFXVolume()
    {
        return sfxVolumePlayerSetting * sfxMufflePercent;
    }

    private static void queueAudioClip(string audioClipPath)
    {
        audioClipPathQueue.Add(audioClipPath);

        if(instance != null && 
            instance.playingQueuedAudioClips == null)
        {
            instance.playingQueuedAudioClips = instance.StartCoroutine(playAllQueuedAudioClips());
        }
    }

    private static IEnumerator playAllQueuedAudioClips()
    {
        float timeWaited = 0f;
        
        while(audioClipPathQueue.Count > 0)
        {
            yield return null;

            timeWaited += Time.deltaTime;

            if(timeWaited >= HeartBeatManager.fastBeatLengthSeconds*1.5f)
            {
                AudioClip currentClip = Resources.Load<AudioClip>(audioClipPathQueue[0]);
                audioClipPathQueue.RemoveAt(0);

                instance.StartCoroutine(playQueuedAudioClip(currentClip));
                timeWaited = 0f;
            }
        }

        if(instance != null)
        {
            instance.playingQueuedAudioClips = null;
        }
    }

    private static IEnumerator playQueuedAudioClip(AudioClip currentClip, AudioSource source = null)
    {
        if(currentClip == null || instance == null)
        {
            yield break;
        }

        if(source == null)
        {
            source = createOneOffAudioSource();
        }

        source.clip = currentClip;
        source.volume = getSFXVolume();
        source.Play();

        float timeWaited = 0f;

        while(timeWaited < currentClip.length)
        {
            yield return null;

            timeWaited += Time.deltaTime;
        }

        Destroy(source.gameObject);
    }

    private static AudioSource createOneOffAudioSource()
    {
        GameObject audioPlayer = new GameObject("AudioClip Player");
        audioPlayer.transform.parent = instance.transform;

        return audioPlayer.AddComponent<AudioSource>();
    }

    public static void playAudioClipAsSingleton(AudioClip clip)
    {
        if(clip == null || instance == null)
        {
            return;
        }

        if(!singletonAudioClips.ContainsKey(clip) || singletonAudioClips[clip]== null)
        {
            singletonAudioClips[clip] = createOneOffAudioSource();
        }

        AudioSource source = singletonAudioClips[clip];

        instance.StartCoroutine(playQueuedAudioClip(clip, source: source));
    }

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateAudioManager()
    {
        instance = null;
        previousMusicPath = "";
        currentMusicPath = "";
        TransitionManager.ChangeAreaMusic.AddListener(playNextAreaMusic);
        musicVolumePlayerSetting = musicVolumeMaximum;
        footstepPlayerSetting = musicVolumeMaximum; 

        playLeftFootstep = true;
        playSFXOnNextHeartBeat = true;

        audioClipPathQueue = new List<string>();

        singletonAudioClips = new Dictionary<AudioClip, AudioSource>();

        HeartBeatManager.MediumHeartBeat.AddListener(playFootStep);
    }

}

public enum FootStepType { Dirt, Cave, WoodFloor }

public static class AudioClipList
{
    public const string audioFolderPath = "Audio/";

    public const string musicFolderPath = audioFolderPath + "Music/";

    public const string campOverworld = musicFolderPath + "Camp Overworld";
    public const string campInterior = musicFolderPath + "Camp Interior";
    public const string campBattle = musicFolderPath + "Camp Battle";
    public const string deathMusic = musicFolderPath + "Dead";
    public const string caveOne = musicFolderPath + "Cave 1";

    public const string SFXFolderPath = audioFolderPath + "Sound Effects/";

    public const string footstepFolderPath = SFXFolderPath + "Footsteps/";

    public const string footstepSFXFilePrefix = "FS";

    public const string miscSFXFolder = SFXFolderPath + "Misc/";

    public const string chestOpen = miscSFXFolder + "ChestOpen";
    public const string placeInInventory = miscSFXFolder + "PlaceInInventory";

    public const string coinSFXFolder = SFXFolderPath + "Coin/";

    public const string coinSFXPrefix = coinSFXFolder + "Coin";
    public const int coinSFXCount = 5;

    public const string equipUnequipSFXFolder = SFXFolderPath + "EquipUnequip/";

    public const string actionsEquipSFXFolder = equipUnequipSFXFolder + "Actions/";
    public const string actionEquipSFX = actionsEquipSFXFolder + "ActionEquip";
    public const string actionUnequipSFX = actionsEquipSFXFolder + "ActionUnequip";

    public const string armorEquipSFXFolder = equipUnequipSFXFolder + "Armor/";

    public const string headSlotChangeSFX = armorEquipSFXFolder + "Head";
    public const string bodySlotChangeSFX = armorEquipSFXFolder + "Body";
    public const string handsSlotChangeSFX = armorEquipSFXFolder + "Hands";
    public const string feetSlotChangeSFX = armorEquipSFXFolder + "Feet";
    public const string trinketSlotChangeSFX = armorEquipSFXFolder + "Trinket";

    public const string weaponEquipSFXFolder = equipUnequipSFXFolder + "Weapon/";

    public const string weaponPrefix = weaponEquipSFXFolder + "Weapon";
    public const int weaponSFXCount = 3;

    public const string hitSFXFolder = SFXFolderPath + "Hit/";

    public const string attackSoundsSFXFolder = SFXFolderPath + "Attack Sounds/";

    public const string miscAttackSoundFolder = attackSoundsSFXFolder + "Misc/";
    public const string biteAttackSound = miscAttackSoundFolder + "Bite";

    public const string weaponAttackSoundFolder = attackSoundsSFXFolder + "Weapons/";
    public const string weaponSwingAttackSound = weaponAttackSoundFolder + "WeaponSwing";

    public const string batAttackSoundsSFXFolder = attackSoundsSFXFolder + "Bats/";
    public const string batSwarmAttackSound = batAttackSoundsSFXFolder + "Bat Swarm";
    public const string batAttackSound = batAttackSoundsSFXFolder + "Bat Attack";
    public const string batHowelAttackSound = batAttackSoundsSFXFolder + "Bat Howl";

    public const string wormAttackSoundsSFXFolder = attackSoundsSFXFolder + "Worms/";
    public const string wormVomitAttackSound = wormAttackSoundsSFXFolder + "WormAcidVomit";
    public const string wormExplodeOnDeathSound = wormAttackSoundsSFXFolder + "WormExplodeOnDeath";
    public const string wormSummonSound = wormAttackSoundsSFXFolder + "WormSummon";

    public const string deathSoundsSFXFolder = SFXFolderPath + "Death Sounds/";

    public const string batDeathSoundsFolder = deathSoundsSFXFolder + "Bats/";
    public const string batDeathSFXOne = batDeathSoundsFolder + "Bat Death 1";
    public const string batDeathSFXTwo = batDeathSoundsFolder + "Bat Death 2";


}