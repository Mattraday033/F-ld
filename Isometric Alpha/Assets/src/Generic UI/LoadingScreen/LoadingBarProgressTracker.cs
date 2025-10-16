using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBarProgressTracker : MonoBehaviour
{
	public RectTransform loadProgressBar;
	public GameObject pressAnyKeyMessage;

	public static LoadSaveFile loadSaveFile;

	private static bool garbageCollectionHasOccured;

	private int currentWaitUntilFrame;

	private static bool canChangeScene = false;

	private float elapsedTime = 0f;

	private const float waitMin = .25f;
    private const float waitMax = 1.5f;
	private float waitInSeconds;

    private float speed;
	private const float speedMin = 800f;
	private const float speedMax = 1000f;

	private const float offsetMinimum = -5f;

	private float endWait = -1f;

	void Start()
	{
		waitInSeconds = getNewWait();

        speed = UnityEngine.Random.Range(speedMin, speedMax);
	}

	void Update()
	{
		if((canChangeScene && KeyBindingList.continueUIKeyIsPressed()) || Application.isEditor)
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
}
