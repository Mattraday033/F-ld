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
        PlayerOOCStateManager.setCurrentActivity(OOCActivity.Loading);
    }

    private void Start()
    {
        StartCoroutine(beforeBarWait());
    }

    private void OnDestroy()
    {
        if(PlayerOOCStateManager.currentActivity != OOCActivity.inDialogue)
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        }

        HeartBeatManager.MediumHeartBeat.RemoveListener(animateProtagRunSprite);
        runFrontSprites = null;
        loadSaveFile = null;
    }

	void Update()
	{
		if(canChangeScene && (KeyBindingList.continueUIKeyIsPressed() || Input.GetKey(KeyCode.Mouse0)))
		{
            loadSaveFile.performOutro();
            loadSaveFile = null;
        } 
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

        StartCoroutine(endWait());
    }

    private IEnumerator endWait()
    {
        while(elapsedTime < continueMessageWait)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        canChangeScene = true;
		pressAnyKeyMessage.SetActive(true);
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
