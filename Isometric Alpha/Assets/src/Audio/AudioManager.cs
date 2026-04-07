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

    public static void setMusicSourceVolume(float volumePercent)
    {
        if(instance == null || instance.musicSource == null)
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

        instance.musicSource.volume = volumePercent;
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

    public static string previousMusicPath;
    public static string currentMusicPath;

    public Coroutine playingQueuedAudioClips;

    [SerializeField]
    private AudioSource musicSource;    
    [SerializeField]
    private AudioSource footStepSource;

    public static List<KeyValuePair<string, VolumeType>> audioClipPathQueue;
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
        instance.musicSource.volume = getVolumeByType(VolumeType.Music);

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

    private static void loadAndPlayCurrentMusicClip()
    {
        loadAndPlayClip(instance.musicSource, currentMusicPath, VolumeType.Music);
        MusicFade.OnMusicMidFade.RemoveListener(loadAndPlayCurrentMusicClip);
    }

    private static void loadAndPlayClip(AudioSource source, string clipPath, VolumeType type)
    {
        source.clip = Resources.Load<AudioClip>(clipPath);
        source.volume = getVolumeByType(type);
        source.Play();
    }

    public static void addMusicFade()
    {
        BetweenAreaFade fade = new BetweenAreaFade(fadeOut, previousMusicPath, currentMusicPath);

        FadeToBlackManager.createFade(fade);
    }

    public static void playEffectAnimationSFX(string effectType)
    {
        if(effectType.Equals(EffectAnimationType.Default.ToString()) || 
            effectType.Equals(EffectAnimationType.BatSwarm.ToString()))
        {
            return;
        }

        if(effectType.Equals(EffectAnimationType.SmokeBomb.ToString()))
        {
            playSmokebombSFX();
            return;
        }

        if(effectType.Equals(EffectAnimationType.Intimidate.ToString()))
        {
            effectType = EffectAnimationType.Negative.ToString();
        }

        queueAudioClip(AudioClipList.hitSFXFolder + effectType + "/" + effectType, VolumeType.SFX);
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
            footStepSource.volume = getVolumeByType(VolumeType.Footstep);
            footStepSource.Play();
        }
    }

    private static void queueAudioClip(string audioClipPath, VolumeType type = VolumeType.SFX)
    {
        audioClipPathQueue.Add(new KeyValuePair<string, VolumeType>(audioClipPath, type));

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
                AudioClip currentClip = Resources.Load<AudioClip>(audioClipPathQueue[0].Key);

                instance.StartCoroutine(playQueuedAudioClip(currentClip, audioClipPathQueue[0].Value));
                audioClipPathQueue.RemoveAt(0);
                timeWaited = 0f;
            }
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

    public static void playAudioClipAsSingleton(string clipPath, VolumeType type = VolumeType.SFX)
    {
        playAudioClipAsSingleton(Resources.Load<AudioClip>(clipPath), type);
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

    #region Play Specific SFX/Music

    public static void playDefeatMusic()
    {
        playMusicWithoutFade(AudioClipList.deathMusic);
    }


    public static void playBattleMusic()
    {
        playMusicWithoutFade(AudioClipList.campBattle + Random.Range(Constants.indexOne, AudioClipList.campBattleCount + 1));
    }

    public static void playNoMusic()
    {
        playMusicWithoutFade("");
    }

    public static void playMusicWithoutFade(string musicPath)
    {
        FadeToBlackManager.StopFade(FadeType.Music);
        instance.musicSource.volume = getVolumeByType(VolumeType.Music);
        loadAndPlayClip(instance.musicSource, musicPath, VolumeType.Music);
        setCurrentMusicPath(musicPath);
    }

    public static void playCoinSFX()
    {
        playAudioClipAsSingleton(Resources.Load<AudioClip>(AudioClipList.coinSFXPrefix + 
                Random.Range(Constants.indexOne, AudioClipList.coinSFXCount + 1)));
    }

    public static void playWeaponChangeSFX()
    {
        playAudioClipAsSingleton(Resources.Load<AudioClip>(AudioClipList.weaponPrefix + 
                Random.Range(Constants.indexOne, AudioClipList.weaponSFXCount + 1)));
    }

    public static void playSelectorMovedSFX()
    {
        playAudioClipAsSingleton(Resources.Load<AudioClip>(AudioClipList.moveSelectorSFX));
    }

    public static void playChangeSelectedActionFX()
    {
        playAudioClipAsSingleton(Resources.Load<AudioClip>(AudioClipList.changeSelectedActionSFX));
    }

    public static void playChooseActorAbilityLocationSFX()
    {
        playAudioClipAsSingleton(Resources.Load<AudioClip>(AudioClipList.chooseActorAbilityLocationSFX));
    }

    public static void playCannotChooseActorAbilityLocationSFX()
    {
        playAudioClipAsSingleton(Resources.Load<AudioClip>(AudioClipList.cannotChooseActorAbilityLocationSFX));
    }

    public static void playChangeScreenSFX()
    {
        playAudioClipAsSingleton(Resources.Load<AudioClip>(AudioClipList.changeScreenSFXPrefix + 
                                 Random.Range(Constants.indexOne, AudioClipList.changeScreenSFXCount + 1)));
    }

    public static void playOnTransitionSFX()
    {
        playAudioClipAsSingleton(Resources.Load<AudioClip>(AudioClipList.onTransitionSFX));
    }

    public static void playWhipSFX()
    {
        playAudioClipAsSingleton(Resources.Load<AudioClip>(AudioClipList.whipAttackSound));
    }

    public static void playPlacePartyMemberSFX()
    {
        playAudioClipAsSingleton(Resources.Load<AudioClip>(AudioClipList.placePartyMemberSFX));
    }

    public static void playSmokebombSFX()
    {
        queueAudioClip(AudioClipList.smokeBombSFX);
    }

    public static void playTunnelExplosionSFX()
    {
        queueAudioClip(AudioClipList.tunnelExplosionSFX);
    }

    public static void playJellyMisfireSFX()
    {
        queueAudioClip(AudioClipList.jellyMisfireSFX);
    }


    public static void playRestSFX()
    {
        playAudioClipAsSingleton(AudioClipList.snoringDialogueSFX);
        playAudioClipAsSingleton(AudioClipList.restSFX);
    }

    public static void playGateOpenSFX()
    {
        playAudioClipAsSingleton(AudioClipList.gateOpen);
    }

    public static void playGateOpenShortSFX()
    {
        playAudioClipAsSingleton(AudioClipList.gateOpenShort);
    }

    #endregion

    [RuntimeInitializeOnLoadMethod]
    private static void instantiateAudioManager()
    {
        instance = null;
        previousMusicPath = "";
        currentMusicPath = "";
        TransitionManager.ChangeAreaMusic.AddListener(playNextAreaMusic);

        Config.readConfig();

        playLeftFootstep = true;
        playSFXOnNextHeartBeat = true;

        audioClipPathQueue = new List<KeyValuePair<string, VolumeType>>();

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
    public const string deathMusic = musicFolderPath + "Dead";
    public const string caveOne = musicFolderPath + "Cave 1";
    public const string winMusic = musicFolderPath + "Win";

    public const string battleFolderPath = musicFolderPath + "Battle/";
    public const string campBattle = battleFolderPath + "Camp Battle";
    public const int campBattleCount = 3;

    public const string SFXFolderPath = audioFolderPath + "Sound Effects/";

    public const string footstepFolderPath = SFXFolderPath + "Footsteps/";

    public const string footstepSFXFilePrefix = "FS";

    public const string miscSFXFolder = SFXFolderPath + "Misc/";

    public const string chestOpen = miscSFXFolder + "ChestOpen";
    public const string gateOpen = miscSFXFolder + "GateOpen";
    public const string gateOpenShort = miscSFXFolder + "GateOpenShort";
    public const string placeInInventory = miscSFXFolder + "PlaceInInventory";
    public const string restSFX = miscSFXFolder + "Rest";

    public const string tunnelExplosionSFX = miscSFXFolder + "TunnelExplosion";
    public const string jellyMisfireSFX = miscSFXFolder + "JellyMisfire";

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

    #region Attack Sounds

    public const string attackSoundsSFXFolder = SFXFolderPath + "Attack Sounds/";

    public const string miscAttackSoundFolder = attackSoundsSFXFolder + "Misc/";
    public const string biteAttackSound = miscAttackSoundFolder + "Bite";

    public const string weaponAttackSoundFolder = attackSoundsSFXFolder + "Weapons/";
    public const string weaponSwingAttackSound = weaponAttackSoundFolder + "WeaponSwing";
    public const string whipAttackSound = weaponAttackSoundFolder + "Whip";

    public const string batAttackSoundsSFXFolder = attackSoundsSFXFolder + "Bats/";
    public const string batSwarmAttackSound = batAttackSoundsSFXFolder + "Bat Swarm";
    public const string batAttackSound = batAttackSoundsSFXFolder + "Bat Attack";
    public const string batHowlAttackSound = batAttackSoundsSFXFolder + "Bat Howl";

    public const string wormAttackSoundsSFXFolder = attackSoundsSFXFolder + "Worms/";
    public const string wormVomitAttackSound = wormAttackSoundsSFXFolder + "WormAcidVomit";
    public const string wormExplodeOnDeathSound = wormAttackSoundsSFXFolder + "WormExplodeOnDeath";
    public const string wormSummonSound = wormAttackSoundsSFXFolder + "WormSummon";

    public const string horseAttackSoundsSFXFolder = attackSoundsSFXFolder + "Horse/";
    public const string horseAttackSound = horseAttackSoundsSFXFolder + "Horse Whinny";

    public const string placePartyMemberSFX = miscAttackSoundFolder + "PlacePartyMember";
    public const string smokeBombSFX = miscAttackSoundFolder + "Smokebomb";

    #endregion

    #region Death Sounds

    public const string deathSoundsSFXFolder = SFXFolderPath + "Death Sounds/";

    public const string batDeathSoundsFolder = deathSoundsSFXFolder + "Bats/";
    public const string batDeathSFXOne = batDeathSoundsFolder + "Bat Death 1";
    public const string batDeathSFXTwo = batDeathSoundsFolder + "Bat Death 2";

    public const string horseDeathSoundsFolder = deathSoundsSFXFolder + "Horse/";
    public const string horseDeathSFX = horseDeathSoundsFolder + "Death";

    public const string maleHumanDeathSoundsFolder = deathSoundsSFXFolder + "Male Human/";
    public const string maleHumanDeathSound = maleHumanDeathSoundsFolder + "Death";

    public const string femaleHumanDeathSoundsFolder = deathSoundsSFXFolder + "Female Human/";
    public const string femaleHumanDeathSound = femaleHumanDeathSoundsFolder + "Death";

    #endregion

    #region UI

    public const string UISFXFolder = SFXFolderPath + "UI/";

    public const string moveSelectorSFX = UISFXFolder + "MoveSelector";
    public const string changeSelectedActionSFX = UISFXFolder + "ChangeSelectedAction";
    public const string chooseActorAbilityLocationSFX = UISFXFolder + "ChooseActorAbilityLocation";
    public const string cannotChooseActorAbilityLocationSFX = UISFXFolder + "CannotChooseActorAbilityLocation";
    public const string changeScreenSFXFolder = UISFXFolder + "ChangeScreen/";
    public const string changeScreenSFXPrefix = changeScreenSFXFolder + "ChangeScreen";
    public const int changeScreenSFXCount = 3;

    public const string betweeenAreasSFXFolder = SFXFolderPath + "BetweenAreas/";

    public const string onTransitionSFX = betweeenAreasSFXFolder + "OnTransition";

    #endregion

    #region Items

    public const string itemsSFXFolder = SFXFolderPath + "Items/";

    public const string eatingSFXFolder = itemsSFXFolder + "Eating/";
    public const string eatingSFXPrefix = eatingSFXFolder + "Eating";
    public const int eatingSFXCount = 6;
    public readonly static PlaySFXLogic playEatingSFX = () => AudioManager.playAudioClipAsSingleton(Resources.Load<AudioClip>(eatingSFXPrefix + Random.Range(Constants.indexOne, eatingSFXCount + 1)));


    public const string drinkingSFXFolder = itemsSFXFolder + "Drinking/";
    public const string sipSFX = drinkingSFXFolder + "Sip";
    public readonly static PlaySFXLogic playSipSFX = () => AudioManager.playAudioClipAsSingleton(Resources.Load<AudioClip>(sipSFX));

    #endregion

    #region Dialogue

    public const string dialogueSFXFolder = SFXFolderPath + "Dialogue/";

    public const string humanMaleDialogueSFXFolder = dialogueSFXFolder + "Male Human/";
    public const int humanMaleIntroCount = 3;
    public const string humanFemaleDialogueSFXFolder = dialogueSFXFolder + "Female Human/";
    public const int humanFemaleIntroCount = 3;

    public const string dialogueIntroPrefix = "Intro";

    public const string horseDialogueSFXFolder = dialogueSFXFolder + "Horse/";

    public const string horseIntroSFX = horseDialogueSFXFolder + dialogueIntroPrefix;

    public const string objectsDialogueSFXFolder = dialogueSFXFolder + "Objects/";

    public const string crateIntroSFX = objectsDialogueSFXFolder + "Crate" + dialogueIntroPrefix;
    public const string rockIntroSFX = objectsDialogueSFXFolder + "Rock" + dialogueIntroPrefix;
    public const string gateIntroSFX = objectsDialogueSFXFolder + "Gate" + dialogueIntroPrefix;

    public readonly static PlaySFXLogic playEatingRockCakeSFX = () => AudioManager.playAudioClipAsSingleton(Resources.Load<AudioClip>(rockIntroSFX));

    public const string sleepingDialogueSFXFolder = dialogueSFXFolder + "Sleeping/";

    public const string snoringDialogueSFX = sleepingDialogueSFXFolder + "Snoring";

    public static PlaySFXLogic getDialogueIntroSFXLogic(string npcName, bool sleeping = false)
    {
        if(sleeping)
        {
            return () => AudioManager.playAudioClipAsSingleton(Resources.Load<AudioClip>(snoringDialogueSFX), VolumeType.Voice);
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
                return () =>  AudioManager.playAudioClipAsSingleton(Resources.Load<AudioClip>(crateIntroSFX), VolumeType.Voice);
            case NPCNameList.barracksGate:
            case NPCNameList.manseFrontDoor:
            case NPCNameList.manseServiceEntrance:
            case NPCNameList.gate:
            case NPCNameList.liftableGate:
            case NPCNameList.ancientPortcullis:
            case NPCNameList.campGate:
            case NPCNameList.mineArmoryGate:
                return () => AudioManager.playAudioClipAsSingleton(Resources.Load<AudioClip>(gateIntroSFX), VolumeType.Voice);
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
                return () => AudioManager.playAudioClipAsSingleton(Resources.Load<AudioClip>(rockIntroSFX), VolumeType.Voice);
            case NPCNameList.slate:
                return () => { };
            case NPCNameList.csalan:
            case NPCNameList.horse:
                return () => AudioManager.playAudioClipAsSingleton(Resources.Load<AudioClip>(horseIntroSFX), VolumeType.Voice);
            case NPCNameList.controlPanel:  
            case NPCNameList.leafPile:
                return () => AudioManager.playAudioClipAsSingleton(Resources.Load<AudioClip>(onTransitionSFX), VolumeType.Voice);     
            case NPCNameList.guardVirag:
            case NPCNameList.guardReka:
            case NPCNameList.guardMuzsa:
            case NPCNameList.quartermasterEmese:
            case NPCNameList.page:
                return () =>
                {
                    AudioManager.playAudioClipAsSingleton(Resources.Load<AudioClip>(humanFemaleDialogueSFXFolder + dialogueIntroPrefix +
                                 Random.Range(Constants.indexOne, humanFemaleIntroCount + 1)), VolumeType.Voice);
                };
            default:
                return () =>
                {
                    AudioManager.playAudioClipAsSingleton(Resources.Load<AudioClip>(humanMaleDialogueSFXFolder + dialogueIntroPrefix +
                                 Random.Range(Constants.indexOne, humanMaleIntroCount + 1)), VolumeType.Voice);
                };
        }
    }

    #endregion

}