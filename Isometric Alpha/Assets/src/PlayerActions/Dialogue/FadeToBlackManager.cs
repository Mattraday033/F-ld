using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;
using UnityEngine.Events;

public class FadeToBlackManager : MonoBehaviour
{
	private static FadeToBlackManager instance;
    public readonly static UnityEvent OnFadeToBlack = new UnityEvent();
    public readonly static UnityEvent OnFadeBackInFinished = new UnityEvent();

    public Camera mainCamera;
    public CinemachineVirtualCamera mainCM;

    public Canvas fadeToBlackCanvas;

    public Image fadeToBlackImage;

    public bool fadeBackInOnStart = false;

	private static Coroutine currentCoroutine;
    private static IFadeTransition currentFadeTransition;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeFadeToBlackManager()
    {
        currentCoroutine = null;
        currentFadeTransition = null;

        instance = null;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        currentCoroutine = null;
        currentFadeTransition = null;

        instance = this;
    }

	public static FadeToBlackManager getInstance()
	{
		return instance;
	}

	void Start()
	{
		setCameras();

        if(CombatStateManager.inCombat && CombatStateManager.whoseTurn == WhoseTurn.Lost)
        {
           return;
        }

        if(CombatStateManager.inCombat)
        {
            createFade(new CircleTransitionReduce(mainCamera.transform, OOCActivity.walking));
        } else
        {
            if(PlayerOOCStateManager.currentActivity == OOCActivity.walking)
            {
                PlayerOOCStateManager.setCurrentActivity(OOCActivity.inFade);
            }

            setToMaxOpacity();
            setAndStartFadeBackIn();
        }
	}

	public void setCameras(){
		
		mainCamera = Camera.main;
		mainCM = GameObject.FindWithTag(LayerAndTagManager.mainVirtualCameraTag).GetComponent<CinemachineVirtualCamera>();

		fadeToBlackCanvas.worldCamera = mainCamera;
	}

	public static bool isMidFade()
	{
        return currentFadeTransition != null && !currentFadeTransition.isFinished();
	}

	public void setAndStartFadeToBlack()
	{
		createFade(new FadeToBlackTransition());
	}
	
	public void setAndStartFadeBackIn()
	{
		createFade(new FadeBackInTransition());
	}

    public static void startCombatTransition(Transform monsterTransform)
    {
        if(instance == null)
        {
            return;
        }

        instance.createFade(new CircleTransitionEnlarge(monsterTransform, OOCActivity.preCombat));
    }

    public void createFade(IFadeTransition fadeTransition)
    {
        currentFadeTransition = fadeTransition;	
		currentCoroutine = StartCoroutine(currentFadeTransition.getCoroutine());
    }

    public static void setToMaxOpacity()
    {
        if(instance == null)
        {
            return;
        }
        
        instance.fadeToBlackImage.color = Color.black;
    }
}
