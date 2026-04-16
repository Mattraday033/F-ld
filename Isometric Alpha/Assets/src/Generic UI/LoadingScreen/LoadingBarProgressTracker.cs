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

	private static bool garbageCollectionHasOccured;

	private static bool canChangeScene = false;

	private float elapsedTime = 0f;

	private const float waitMin = .25f;
    private const float waitMax = 1.5f;
	private float waitInSeconds;

    private float speed;
	private const float speedMin = 800f;
	private const float speedMax = 1000f;

	private const float offsetMinimum = 0f;

	private float endWait = -1f;

    private static Sprite[] runFrontSprites;

    private static bool skipNextHeartBeat;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeLoadingBarProgressTracker()
    {
        instance = null;
        _LoadSaveFile = null;
        loadSaveFile = null;
        garbageCollectionHasOccured = false;
        canChangeScene = false;
        runFrontSprites = null;
        skipNextHeartBeat = false;
    }

    private void Awake()
    {
        instance = this;
    }

	void Start()
	{
		waitInSeconds = getNewWait();

        speed = UnityEngine.Random.Range(speedMin, speedMax);

        if(CombatStateManager.inCombat)
        {
            CombatStateManager.resetCombat();
            CombatStateManager.inCombat = false;
        }
	}

    private void OnDestroy()
    {
        HeartBeatManager.MediumHeartBeat.RemoveListener(animateProtagRunSprite);
        runFrontSprites = null;
        loadSaveFile = null;
    }

	void Update()
	{
		// if((canChangeScene && (KeyBindingList.continueUIKeyIsPressed() || Input.GetKey(KeyCode.Mouse0))) || Application.isEditor)
		if(canChangeScene && (KeyBindingList.continueUIKeyIsPressed() || Input.GetKey(KeyCode.Mouse0)))
		{
            garbageCollectionHasOccured = false;
            canChangeScene = false;

            loadSaveFile.execute();
            loadSaveFile = null;
			return;
        } else if(canChangeScene)
		{
			return;
		}

		elapsedTime += Time.deltaTime;

		if (elapsedTime <= waitInSeconds)
		{
			return;
		}

		if(!garbageCollectionHasOccured)
		{
            GC.Collect();
			garbageCollectionHasOccured = true;
        }

        if(loadProgressBar.offsetMax.x < offsetMinimum)
		{
            if ((loadProgressBar.offsetMax.x + (speed * Time.deltaTime)) > offsetMinimum)
            {
                loadProgressBar.offsetMax = new Vector2(offsetMinimum, loadProgressBar.offsetMax.y);
            }
            else
            {
                loadProgressBar.offsetMax = new Vector2(loadProgressBar.offsetMax.x + speed * Time.deltaTime, loadProgressBar.offsetMax.y);
            }

            return;
        } else if(endWait < 0)
		{
			endWait = elapsedTime + getNewWait();

        }

		if (elapsedTime < endWait)
		{
			return;
		}

        canChangeScene = true;
		pressAnyKeyMessage.SetActive(true);
    }
    private float getNewWait()
	{
		return UnityEngine.Random.Range(waitMin, waitMax);
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
}
