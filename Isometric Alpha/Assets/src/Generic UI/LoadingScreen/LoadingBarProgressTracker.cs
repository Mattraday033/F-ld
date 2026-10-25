using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBarProgressTracker : MonoBehaviour
{
	public RectTransform loadProgressBar;
	public GameObject pressAnyKeyMessage;

    public Image protagRunSprite;

    private static LoadingBarProgressTracker instance;

	private static LoadSaveFile _LoadSaveFile;

	public static LoadSaveFile loadSaveFile
    {
        get
        {
            return _LoadSaveFile;
        }
        set
        {
            _LoadSaveFile = value;

            if(_LoadSaveFile != null)
            {
                string folderPath = EnemyTypeFolderPathList.getEnemyTypeFolderPath(_LoadSaveFile.getPlayerSpriteNameInSave());
                runFrontSprites = Resources.LoadAll<Sprite>(folderPath + CharacterAnimationType.Run_Front.ToString());

                HeartBeatManager.MediumHeartBeat.AddListener(animateProtagRunSprite);
                animateProtagRunSprite(0);
            }
        }
    }

	private bool canChangeScene = false;

	private float elapsedTime = 0f;

	private float waitBeforeBarMovement = .25f;
	private float waitFirstLoadStage = .75f;
	private float waitSecondLoadStage = 1.25f;
	private float waitThirdLoadStage = 1.75f;
	private float continueMessageWait = 2.25f;

	private const float offsetMinimum = -1575f;
    private const float offsetMaximum = 500f;

    private static Sprite[] runFrontSprites;

    private static bool skipNextHeartBeat;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeLoadingBarProgressTracker()
    {
        instance = null;
        _LoadSaveFile = null;
        loadSaveFile = null;
        runFrontSprites = null;
        skipNextHeartBeat = false;
    }

    private void Awake()
    {
        instance = this;
        PlayerStateManager.setCurrentActivity(CurrentActivity.Loading);

        HeadlessLogger.LogEvent("LoadingScreen.Opened");
    }

    private void Start()
    {
        StartCoroutine(beforeBarWait());
    }

    private void OnDestroy()
    {
        HeadlessLogger.LogEvent("LoadingScreen.Closed");

        //before the activity change below, which re-syncs the input set itself only if the activity changes
        PlayerStateManager.removeInputOverlay(InputOverlay.LoadingScreenContinue);

        if(PlayerStateManager.currentActivity != CurrentActivity.InDialogue &&
           PlayerStateManager.currentActivity != CurrentActivity.InTutorialSequence)
        {
            PlayerStateManager.setCurrentActivity(CurrentActivity.Walking);
        }

        HeartBeatManager.MediumHeartBeat.RemoveListener(animateProtagRunSprite);
        runFrontSprites = null;
        loadSaveFile = null;
    }

    //Called by the LoadingScreenContinue overlay's actions (the continue keys and a left click). The overlay
    //is added once the "press any key" prompt shows; its actions check the initial state, so a key that is
    //already held then continues straight away, as the old polling did.
    public static void continueFromKey()
    {
        if(instance == null || !instance.canChangeScene || loadSaveFile == null)
        {
            return;
        }

        instance.canChangeScene = false;

        HeadlessLogger.LogEvent("LoadingScreen.Continued");

        loadSaveFile.performOutro();
        loadSaveFile = null;
    }

    private IEnumerator progressLoadBar()
    {
        while(elapsedTime < continueMessageWait)
        {
            yield return null;
            
            loadProgressBar.offsetMax = new Vector2(Mathf.Lerp(offsetMinimum, offsetMaximum, (elapsedTime-waitBeforeBarMovement)/continueMessageWait), loadProgressBar.offsetMax.y);
        }
    }

    private IEnumerator beforeBarWait()
    {
        while(elapsedTime < waitBeforeBarMovement)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        StartCoroutine(progressLoadBar());
        StartCoroutine(firstWait());
    }

    private IEnumerator firstWait()
    {
        while(elapsedTime < waitFirstLoadStage)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        GC.Collect();
        loadSaveFile.resetData();
        loadSaveFile.readFromSaveBlueprint();

        StartCoroutine(secondWait());
    }

    private IEnumerator secondWait()
    {
        while(elapsedTime < waitSecondLoadStage)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        LoadSaveFile.beforeSecondStageLoad = false;

        AreaManager.getInstance().Awake();
        BackgroundManager.getInstance().Start();

        StartCoroutine(thirdWait());
    }

    private IEnumerator thirdWait()
    {
        while(elapsedTime < waitThirdLoadStage)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        GC.Collect();
        OOCUIManager.getInstance().enableOOCUI();
        OOCUIManager.updateOOCUI();

        //built here, behind the loading screen, rather than the first time each screen is opened. endWait holds the prompt until it is done
        StartCoroutine(PrebuiltScreenManager.build());

        StartCoroutine(endWait());
    }

    private IEnumerator endWait()
    {
        while(elapsedTime < continueMessageWait || !PrebuiltScreenManager.buildFinished)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        canChangeScene = true;
		pressAnyKeyMessage.SetActive(true);

        PlayerStateManager.addInputOverlay(InputOverlay.LoadingScreenContinue);
    }

    private static void animateProtagRunSprite(int row)
    {
        if(instance == null || runFrontSprites == null || runFrontSprites.Length < 2)
        {
            return;
        } else if(skipNextHeartBeat)
        {
            skipNextHeartBeat = false;
            return;
        }

        if(instance.protagRunSprite.sprite == runFrontSprites[0])
        {
            instance.protagRunSprite.sprite = runFrontSprites[1];
        }
        else
        {
            instance.protagRunSprite.sprite = runFrontSprites[0];
        }

        skipNextHeartBeat = true;
    }

    public static bool loadingInProgress()
    {
        return SceneManager.GetActiveScene().name.Equals(SceneNameList.loadingScreen);
    }

    public static string getZoneToLoadInto()
    {
        if(loadSaveFile == null || 
            loadSaveFile.saveBlueprint == null || 
            loadSaveFile.saveBlueprint.currentZone == null)
        {
            return SaveDefaultValues.defaultZoneName;
        }

        return loadSaveFile.saveBlueprint.currentZone;
    }
}
