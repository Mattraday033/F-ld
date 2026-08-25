using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum VolumeType { Music, SFX, Voice, Footstep}

public class AudioManager : MonoBehaviour
{
    public const bool fadeOut = true;
    public static bool playLeftFootstep = true;

    #region Volume Values
    public const float volumeMaximum = 1f;
    public const float volumeMinimum = 0f;

    public static float masterVolumePlayerSetting;

    public static float _MusicVolumePlayerSetting;
    public static float musicVolumePlayerSetting
    {
        get => _MusicVolumePlayerSetting * masterVolumePlayerSetting;
        set
        {
            if(value > volumeMaximum)
            {
                value = volumeMaximum;
            } else if(value < volumeMinimum)
            {
                value = volumeMinimum;
            }

            _MusicVolumePlayerSetting = value;

            setMusicSourceVolume(musicVolumePlayerSetting);
        }
    }

    public const float dialogueMusicDuckMultiplier = .5f;
    public const float dialogueMusicDuckDurationSeconds = 1f;
    public static bool isMusicDuckedForDialogue;
    private static Coroutine musicDuckCoroutine;

    public static void setMusicSourceVolume(float volumePercent)
    {
        if(instance == null || instance.musicSource == null)
        {
            return;
        }

        if(isMusicDuckedForDialogue)
        {
            volumePercent *= dialogueMusicDuckMultiplier;
        }

        if(volumePercent < 0)
        {
            volumePercent = 0f;
        } else if(volumePercent > 1f)
        {
            volumePercent = 1f;
        }

        instance.musicSource.volume = volumePercent;
    }

    public static void duckMusicForDialogue()
    {
        if(isMusicDuckedForDialogue)
        {
            return;
        }

        isMusicDuckedForDialogue = true;
        startMusicSourceVolumeFade(musicVolumePlayerSetting * dialogueMusicDuckMultiplier, dialogueMusicDuckDurationSeconds);
    }

    public static void unduckMusicAfterDialogue()
    {
        if(!isMusicDuckedForDialogue)
        {
            return;
        }

        isMusicDuckedForDialogue = false;
        startMusicSourceVolumeFade(musicVolumePlayerSetting, dialogueMusicDuckDurationSeconds);
    }

    private static void startMusicSourceVolumeFade(float targetVolume, float duration)
    {
        if(instance == null || instance.musicSource == null || FadeToBlackManager.isMidScreenFade())
        {
            return;
        }

        if(musicDuckCoroutine != null)
        {
            instance.StopCoroutine(musicDuckCoroutine);
        }

        musicDuckCoroutine = instance.StartCoroutine(fadeMusicSourceVolumeCoroutine(targetVolume, duration));
    }

    private static IEnumerator fadeMusicSourceVolumeCoroutine(float targetVolume, float duration)
    {
        AudioSource source = instance.musicSource;
        float startVolume = source.volume;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }

        source.volume = targetVolume;
        musicDuckCoroutine = null;
    }

    public static float _SFXVolumePlayerSetting;
    public static float sfxVolumePlayerSetting
    {
        get => _SFXVolumePlayerSetting * masterVolumePlayerSetting * sfxMufflePercent;
        set
        {
            if(value > volumeMaximum)
            {
                value = volumeMaximum;
            } else if(value < volumeMinimum)
            {
                value = volumeMinimum;
            }

            _SFXVolumePlayerSetting = value;
        }
    }
    public const float sfxMufflePercent = .7f;

    public static float _VoiceVolumePlayerSetting;
    public static float voiceVolumePlayerSetting
    {
        get => _VoiceVolumePlayerSetting * masterVolumePlayerSetting * voiceMufflePercent;
        set
        {
            if(value > volumeMaximum)
            {
                value = volumeMaximum;
            } else if(value < volumeMinimum)
            {
                value = volumeMinimum;
            }

            _VoiceVolumePlayerSetting = value;
        }
    }
    public const float voiceMufflePercent = .7f;

    public static float _FootstepVolumePlayerSetting;
    public static float footstepVolumePlayerSetting
    {
        get => _FootstepVolumePlayerSetting * masterVolumePlayerSetting * footstepMufflePercent;
        set
        {
            if(value > volumeMaximum)
            {
                value = volumeMaximum;
            } else if(value < volumeMinimum)
            {
                value = volumeMinimum;
            }

            _FootstepVolumePlayerSetting = value;            
            setFootstepSourceVolume(footstepVolumePlayerSetting);
        }
    }
    public const float footstepMufflePercent = .6f;

    public static void setFootstepSourceVolume(float volumePercent)
    {
        if(instance == null || instance.footStepSource == null)
        {
            return;
        }

        if(volumePercent < 0)
        {
            volumePercent = 0f;
        } else if(volumePercent > 1f)
        {
            volumePercent = 1f;
        }

        instance.footStepSource.volume = volumePercent;
    }

    public static float getVolumeByType(VolumeType type)
    {
        switch(type)
        {
            case VolumeType.Music:
                return musicVolumePlayerSetting;
            case VolumeType.Voice:
                return voiceVolumePlayerSetting;
            case VolumeType.Footstep:
                return footstepVolumePlayerSetting;
            default:
                return sfxVolumePlayerSetting;
        }
    }

    #endregion
    private static bool playSFXOnNextHeartBeat;
    private static AudioManager instance;

    public static SFXType previousMusic;
    public static SFXType currentMusic;
    public static SFXType currentAmbience;

    public Coroutine playingQueuedAudioClips;

    [SerializeField]
    private AudioSource musicSource;    
    [SerializeField]
    private AudioSource footStepSource;    
    [SerializeField]
    private AudioSource ambienceSource;

    public static List<KeyValuePair<SFXType, VolumeType>> audioClipPathQueue;
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
        if(mode == LoadSceneMode.Additive ||
            LoadSaveFile.midLoad)
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
        instance.musicSource.volume = getVolumeByType(VolumeType.Music);

        if(Flags.isInNewGameMode())
        {
            addMusicFade();
            setCurrentMusic(SFXType.CampOverworld);
            loadAndPlayCurrentMusicClip();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private static void setCurrentMusic(SFXType newMusic)
    {
        previousMusic = currentMusic;
        currentMusic = newMusic;
    }

    public static void playNextAreaMusic(string locationName)
    {
        SFXType newAreaMusic = AreaList.getAreaMusic(locationName);

        if(newAreaMusic.Equals(currentMusic))
        {
            return;
        } else
        {
            setCurrentMusic(newAreaMusic);
            addMusicFade();
            MusicFade.OnMusicMidFade.AddListener(loadAndPlayCurrentMusicClip);
        }
    }

    public static void restartMusic()
    {
        if(instance != null && instance.musicSource != null)
        {
            instance.musicSource.Play();
        }
    }

    private static void loadAndPlayCurrentMusicClip()
    {
        loadAndPlayClip(instance.musicSource, currentMusic, VolumeType.Music);
        MusicFade.OnMusicMidFade.RemoveListener(loadAndPlayCurrentMusicClip);
    }

    private static void loadAndPlayClip(AudioSource source, SFXType sfxType, VolumeType type)
    {
        source.clip = AudioClipList.getAudioClip(sfxType);
        source.volume = getVolumeByType(type);
        source.Play();
    }

    public static void addMusicFade()
    {
        BetweenAreaFade fade = new BetweenAreaFade(fadeOut, previousMusic, currentMusic);

        FadeToBlackManager.createFade(fade);
    }

    public static void playEffectAnimationSFX(EffectAnimationType effectType)
    {
        switch(effectType)
        {
            case EffectAnimationType.Default:
            case EffectAnimationType.BatSwarm:
                return;
            case EffectAnimationType.SmokeBomb:
                playSmokebombSFX();
                return;
            case EffectAnimationType.Intimidate:
                playEffectAnimationSFX(EffectAnimationType.Negative);
                return;
            default:
                queueAudioClip(effectType.convertEffectTypeToSFXType(), VolumeType.SFX);
                return;
        }
    }

    public static void playFootStep(int row)
    {
        if(LoadSaveFile.midLoad || 
            AreaManager.locationName == null || 
            AreaManager.locationName.Length <= 0 || 
            LoadingBarProgressTracker.loadingInProgress() ||
             CombatStateManager.inCombat || 
             (PlayerMovement.getInstance() != null && !PlayerMovement.getInstance().isMoving()))
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
        SFXType footStepSFX = SFXType.NoSFX;

        switch(footStepType)
        {
            case FootStepType.Dirt:
                footStepSFX = AudioClipList.getRandomSFXInRange(SFXType.Dirt_FS1, SFXType.Dirt_FS7);
                break;
            case FootStepType.WoodFloor:
                footStepSFX = AudioClipList.getRandomSFXInRange(SFXType.WoodFloor_FS1, SFXType.WoodFloor_FS5);
                break;
            default:
                footStepSFX = AudioClipList.getRandomSFXInRange(SFXType.Cave_FS1, SFXType.FS9);
                break;
        }

        AudioClip footstepClip = AudioClipList.getAudioClip(footStepSFX);

        if(footstepClip != null)
        {
            footStepSource.clip = footstepClip;
            footStepSource.volume = getVolumeByType(VolumeType.Footstep);
            footStepSource.Play();
        }
    }

    private static void queueAudioClip(SFXType sfxType, VolumeType type = VolumeType.SFX)
    {
        audioClipPathQueue.Add(new KeyValuePair<SFXType, VolumeType>(sfxType, type));

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
            timeWaited += Time.deltaTime;

            if(timeWaited >= HeartBeatManager.fastBeatLengthSeconds*1.5f)
            {
                AudioClip currentClip = AudioClipList.getAudioClip(audioClipPathQueue[0].Key);

                instance.StartCoroutine(playQueuedAudioClip(currentClip, audioClipPathQueue[0].Value));
                audioClipPathQueue.RemoveAt(0);
                timeWaited = 0f;
            }
            yield return null;
        }

        if(instance != null)
        {
            instance.playingQueuedAudioClips = null;
        }
    }

    private static IEnumerator playQueuedAudioClip(AudioClip currentClip, VolumeType type, AudioSource source = null, bool destroy = true)
    {
        if(currentClip == null || instance == null)
        {
            yield break;
        }

        if(source == null)
        {
            source = createOneOffAudioSource();
        }

        if(!destroy)
        {
            source.Stop();
        }

        source.clip = currentClip;
        source.volume = getVolumeByType(type);
        source.Play();

        float timeWaited = 0f;

        while(timeWaited < currentClip.length)
        {
            yield return null;

            timeWaited += Time.deltaTime;
        }

        if(source != null && destroy)
        {
            Destroy(source.gameObject);
        }
    }

    private static AudioSource createOneOffAudioSource()
    {
        GameObject audioPlayer = new GameObject("AudioClip Player");
        audioPlayer.transform.parent = instance.transform;

        return audioPlayer.AddComponent<AudioSource>();
    }

    public static void playAudioClipAsSingleton(SFXType sfxType, VolumeType type = VolumeType.SFX)
    {
        playAudioClipAsSingleton(AudioClipList.getAudioClip(sfxType), type);
    }

    public static void playAudioClipAsSingleton(AudioClip clip, VolumeType type = VolumeType.SFX)
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

        source.priority = 256;

        instance.StartCoroutine(playQueuedAudioClip(clip, type, source: source, destroy: false));
    }

    public static void playSFXAsAmbience(SFXType sfxType)
    {
        if(instance == null || 
            instance.ambienceSource == null || 
            currentAmbience.Equals(sfxType))
        {
            return;
        }

        currentAmbience = sfxType;

        if(currentAmbience == SFXType.NoSFX)
        {
            return;
        }

        instance.ambienceSource.volume = sfxVolumePlayerSetting * .6f;
        instance.ambienceSource.clip = AudioClipList.getAudioClip(currentAmbience);
        instance.ambienceSource.Play();
    }

    public static void endAmbience()
    {
        if(instance == null || instance.ambienceSource == null)
        {
            return;
        }

        instance.ambienceSource.Stop();
        instance.ambienceSource.clip = null;
        currentAmbience = SFXType.NoSFX;
    }


    #region Play Specific SFX/Music

    public static void playCrowdAmbience()
    {
        playSFXAsAmbience(SFXType.Crowd);
    }

    public static void playBirdsAmbience()
    {
        playSFXAsAmbience(SFXType.Birds);
    }

    public static void playDefeatMusic()
    {
        playMusicWithoutFade(SFXType.Dead);
    }

    public static void playBattleMusic()
    {
        playMusicWithoutFade(AudioClipList.getRandomSFXInRange(SFXType.CampBattle1, SFXType.CampBattle3));
    }

    public static void playNoMusic()
    {
        playMusicWithoutFade(SFXType.NoSFX);
    }

    public static void playMusicWithoutFade(SFXType sfxType)
    {
        FadeToBlackManager.StopFade(FadeType.Music);
        instance.musicSource.volume = getVolumeByType(VolumeType.Music);
        loadAndPlayClip(instance.musicSource, sfxType, VolumeType.Music);
        setCurrentMusic(sfxType);
    }

    public static void playExecutionSFX()
    {
        playAudioClipAsSingleton(SFXType.Execution);
    }

    public static void playCoinSFX()
    {
        playAudioClipAsSingleton(AudioClipList.getRandomSFXInRange(SFXType.Coin1, SFXType.Coin5));
    }

    public static void playWeaponChangeSFX()
    {
        playAudioClipAsSingleton(AudioClipList.getRandomSFXInRange(SFXType.Weapon1, SFXType.Weapon3));
    }

    public static void playSelectorMovedSFX()
    {
        playAudioClipAsSingleton(SFXType.MoveSelector);
    }

    public static void playChangeSelectedActionFX()
    {
        playAudioClipAsSingleton(SFXType.ChangeSelectedAction);
    }

    public static void playChooseActorAbilityLocationSFX()
    {
        playAudioClipAsSingleton(SFXType.ChooseActorAbilityLocation);
    }

    public static void playCannotChooseActorAbilityLocationSFX()
    {
        playAudioClipAsSingleton(SFXType.CannotChooseActorAbilityLocation);
    }

    public static void playChangeScreenSFX()
    {
        playAudioClipAsSingleton(AudioClipList.getRandomSFXInRange(SFXType.ChangeScreen1, SFXType.ChangeScreen3));
    }

    public static void playOnTransitionSFX()
    {
        playAudioClipAsSingleton(SFXType.OnTransition);
    }

    public static void playWhipSFX()
    {
        playAudioClipAsSingleton(SFXType.Whip);
    }

    public static void playPlacePartyMemberSFX()
    {
        playAudioClipAsSingleton(SFXType.PlacePartyMember);
    }

    public static void playSmokebombSFX()
    {
        queueAudioClip(SFXType.Smokebomb);
    }

    public static void playTunnelExplosionSFX()
    {
        queueAudioClip(SFXType.TunnelExplosion);
    }

    public static void playJellyMisfireSFX()
    {
        queueAudioClip(SFXType.JellyMisfire);
    }


    public static void playRestSFX()
    {
        playAudioClipAsSingleton(SFXType.Snoring);
        playAudioClipAsSingleton(SFXType.Rest);
    }

    public static void playGateOpenSFX()
    {
        playAudioClipAsSingleton(SFXType.GateOpen);
    }

    public static void playGateOpenShortSFX()
    {
        playAudioClipAsSingleton(SFXType.GateOpenShort);
    }

    public static void playLvlUpSFX()
    {
        playAudioClipAsSingleton(SFXType.LvlUp);
    }

    public static void playGongSFX()
    {
        playAudioClipAsSingleton(SFXType.Gong);
    }

    public static void playButtonOnSFX()
    {
        playAudioClipAsSingleton(SFXType.ButtonOn);
    }

    public static void playButtonOffSFX()
    {
        playAudioClipAsSingleton(SFXType.ButtonOff);
    }

    #endregion

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        instance = null;
        previousMusic = SFXType.NoSFX;
        currentMusic = SFXType.NoSFX;
        currentAmbience = SFXType.NoSFX;
        TransitionManager.ChangeAreaMusic.AddListener(playNextAreaMusic);

        Config.readConfig();

        playLeftFootstep = true;
        playSFXOnNextHeartBeat = true;

        audioClipPathQueue = new List<KeyValuePair<SFXType, VolumeType>>();

        singletonAudioClips = new Dictionary<AudioClip, AudioSource>();

        HeartBeatManager.MediumHeartBeat.AddListener(playFootStep);

        AudioClipList.init();

        AreaManager.OnAreaSpawn.AddListener(AudioClipList.playLocationAmbience);
    }

}

public enum FootStepType { Dirt, Cave, WoodFloor }

public static class AudioClipList
{

    private const int reservedSFXTypeCount = 1;
    public const string audioClipFilePathsFileName = "AudioClipFilePaths";

    // Every clip under Resources/Audio, keyed by SFXType. Filled from the generated manifest;
    // see SFXTypeGenerator.
    private readonly static ResourceList<SFXType, AudioClip> audioClips =
        new ResourceList<SFXType, AudioClip>(audioClipFilePathsFileName,
                                             reservedSFXTypeCount,
                                             "[AudioClipList]",
                                             "Tools > Audio > Regenerate SFXType");

    public readonly static PlaySFXLogic playEatingSFX = () => AudioManager.playAudioClipAsSingleton(getRandomSFXInRange(SFXType.Eating1, SFXType.Eating6));
    public readonly static PlaySFXLogic playSipSFX = () => AudioManager.playAudioClipAsSingleton(SFXType.Sip);
    public readonly static PlaySFXLogic playEatingRockCakeSFX = () => AudioManager.playAudioClipAsSingleton(SFXType.RockIntro);
    public static void init()
    {
        audioClips.init();
    }

    /// <summary>
    /// The clip for an SFXType, or null for SFXType.NoSFX - a legitimate "play nothing" value
    /// rather than a lookup failure, so it does not log.
    /// </summary>
    public static AudioClip getAudioClip(SFXType sfxType)
    {
        return audioClips.getAsset(sfxType);
    }

    public static PlaySFXLogic getDialogueIntroSFXLogic(string npcName, bool sleeping = false)
    {
        if(sleeping)
        {
            return () => AudioManager.playAudioClipAsSingleton(getAudioClip(SFXType.Snoring), VolumeType.Voice);
        }

        switch(DialogueList.scrubNameOfEndNumbers(npcName))
        {
            case NPCNameList.barrels:
            case NPCNameList.crates:
            case NPCNameList.crate:
            case NPCNameList.barricade:
            case NPCNameList.wallPatch:
            case NPCNameList.ladder:
            case NPCNameList.vaultableBarrels:
            case NPCNameList.hastilyBuiltBarricade:
            case NPCNameList.suspiciousShelf:
                return () =>  AudioManager.playAudioClipAsSingleton(getAudioClip(SFXType.CrateIntro), VolumeType.Voice);
            case NPCNameList.barracksGate:
            case NPCNameList.manseFrontDoor:
            case NPCNameList.manseServiceEntrance:
            case NPCNameList.gate:
            case NPCNameList.liftableGate:
            case NPCNameList.ancientPortcullis:
            case NPCNameList.campGate:
            case NPCNameList.mineArmoryGate:
                return () => AudioManager.playAudioClipAsSingleton(getAudioClip(SFXType.GateIntro), VolumeType.Voice);
            case NPCNameList.rubble:
            case NPCNameList.awkwardRubble:
            case NPCNameList.liftableRubble:
            case NPCNameList.vaultableRocks:
            case NPCNameList.suspiciousWall:
            case NPCNameList.statue:
            case NPCNameList.unstablePillar:
            case NPCNameList.toppledStatue:
            case NPCNameList.vaultableGap:
            case ItemSpriteList.rockCakeSprite:
                return () => AudioManager.playAudioClipAsSingleton(getAudioClip(SFXType.RockIntro), VolumeType.Voice);
            case NPCNameList.slate:
                return () => { };
            case NPCNameList.csalan:
            case NPCNameList.horse:
                return () => AudioManager.playAudioClipAsSingleton(SFXType.HorseIntro, VolumeType.Voice);
            case NPCNameList.controlPanel:  
            case NPCNameList.leafPile:
                return () => AudioManager.playAudioClipAsSingleton(getAudioClip(SFXType.OnTransition), VolumeType.Voice);     
            case NPCNameList.captainAdela:
            case NPCNameList.guardVirag:
            case NPCNameList.guardReka:
            case NPCNameList.guardMuzsa:
            case NPCNameList.quartermasterEmese:
            case NPCNameList.page:
                return () =>
                {
                    AudioManager.playAudioClipAsSingleton(getAudioClip(getRandomSFXInRange(SFXType.FemaleHuman_Intro1, SFXType.FemaleHuman_Intro3)), VolumeType.Voice);
                };
            case NPCNameList.brush:
                return () =>
                {
                    if(AreaManager.locationName.Equals(LocationNameList.slaveShackTwo))
                    {
                        return;
                    } else
                    {
                        AudioManager.playAudioClipAsSingleton(getAudioClip(getRandomSFXInRange(SFXType.MaleHuman_Intro1, SFXType.MaleHuman_Intro3)), VolumeType.Voice);
                    }
                };
            case NPCNameList.director:
                return () =>
                {
                    if(Flags.getFlag(FlagNameList.summonedToDirectorsOffice) && !Flags.getFlag(FlagNameList.revoltStarted) && !AreaList.currentAreaIsHostile())
                    {
                        return;
                    } 
                        
                    AudioManager.playAudioClipAsSingleton(getAudioClip(getRandomSFXInRange(SFXType.MaleHuman_Intro1, SFXType.MaleHuman_Intro3)), VolumeType.Voice);
                };
            default:
                return () =>
                {
                    AudioManager.playAudioClipAsSingleton(getAudioClip(getRandomSFXInRange(SFXType.MaleHuman_Intro1, SFXType.MaleHuman_Intro3)), VolumeType.Voice);
                };
        }
    }

    public static SFXType getRandomSFXInRange(SFXType rangeStart, SFXType rangeEnd)
    {
        int start = (int) rangeStart;
        int end = (int) rangeEnd;

        return (SFXType) Random.Range(start, end + 1);
    }

    public static void playLocationAmbience()
    {
        switch(MapObjectList.getCurrentZoneKey())
        {
            case ZoneKeyList.lovashiCamp:
                switch(AreaManager.locationName)
                {
                    case LocationNameList.campSouthEast:
                        if(SpawnParamsList.guardPunishmentCrowdSpawnParams.canSpawn(NPCNameList.slave))
                        {
                            AudioManager.playCrowdAmbience();
                        } else
                        {
                            AudioManager.playBirdsAmbience(); 
                        }
                        return;
                    default:
                        AudioManager.playBirdsAmbience(); 
                        return;   
                }
            default:
                AudioManager.endAmbience();
                return;
        }
    }

}
