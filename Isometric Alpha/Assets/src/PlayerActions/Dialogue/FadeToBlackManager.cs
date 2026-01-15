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

    [Header("Cameras")]

    //[SerializeField] 
    public Camera mainCamera;
    //[SerializeField] 
    public CinemachineVirtualCamera mainCM;

    [Header("Canvas")]

    //[SerializeField] 
    public Canvas fadeToBlackCanvas;

    [Header("Black Screen")]

	//[SerializeField] 
    public Image fadeToBlackImage;

	private const float maxOpacity = 255f;
	private static float frameCount;
	
	private static IEnumerator fadingToBlackCoroutine;
	private static IEnumerator fadingBackInCoroutine;

    private static bool waitToFadeIn;

    public bool fadeBackInOnStart = false;

    private const float slowFadeInSpeed = 3.5f;
    public float fadeTime = .5f;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeFadeToBlackManager()
    {
        waitToFadeIn = false;

        fadingBackInCoroutine = null;
        fadingToBlackCoroutine = null;

        frameCount = 0f;

        instance = null;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Fade To Black Manager in the scene.");
        }

        fadingToBlackCoroutine = null;

        fadingBackInCoroutine = null;

        waitToFadeIn = false;

        instance = this;

        if (fadeBackInOnStart || Flags.isInNewGameMode())
        {
            frameCount = maxOpacity;
        }

        if (Flags.isInNewGameMode())
        {
            fadeTime = slowFadeInSpeed;
        }
    }

	public static FadeToBlackManager getInstance()
	{
		return instance;
	}

	void Start()
	{
		setCameras();
		updateFadeToBlackImageOpacity();
		
	}
	
	void Update() //here for Animation
	{
        if (isBlack() && !currentlyFadingToBlack() && !currentlyFadingBackIn() && !waitToFadeIn) 
		{
			setAndStartFadeBackIn();
		}
		
		if(isBlack() && currentlyFadingToBlack())
		{
			
			StopCoroutine(fadingToBlackCoroutine);
			fadingToBlackCoroutine = null;
		}
		
		if(isTransparent() && currentlyFadingBackIn())
		{
			StopCoroutine(fadingBackInCoroutine);
			fadingBackInCoroutine = null;
		}

	}

	public void setCameras(){
		
		mainCamera = Camera.main;
		mainCM = GameObject.FindWithTag("MainVirtualCamera").GetComponent<CinemachineVirtualCamera>();

		fadeToBlackCanvas.worldCamera = mainCamera;
	}
	
	public static bool isBlack()
	{
		return frameCount >= maxOpacity;
	}
	
	public static bool isTransparent()
	{
		return frameCount <= 0;
	}

	public void setAndStartFadeToBlack()
	{
		fadingBackInCoroutine = null;
		fadingToBlackCoroutine = fadeToBlack();
		
		StartCoroutine(fadingToBlackCoroutine);
	}
	
	public void setAndStartFadeBackIn()
	{
		fadingToBlackCoroutine = null;
		fadingBackInCoroutine = fadeBackIn();
		
		StartCoroutine(fadingBackInCoroutine);
	}
	
	public bool currentlyFadingToBlack()
	{
		return fadingToBlackCoroutine != null;
	}
	
	public bool currentlyFadingBackIn()
	{
		return fadingBackInCoroutine != null;
	}

	private IEnumerator fadeToBlack()
    {
		OnFadeToBlack.Invoke();

        float timeWaited = 0f;

        while (!isBlack())
        {
            frameCount = Mathf.Lerp(0f, maxOpacity, timeWaited/fadeTime);
            timeWaited += Time.deltaTime;
            updateFadeToBlackImageOpacity();

            if (isBlack())
            {
                break;
            }

            yield return null;
        }
		
		frameCount = maxOpacity;
		updateFadeToBlackImageOpacity();

        GC.Collect();

        DialogueManager.setCameraToDefaultSpeed();
    }
	
	private IEnumerator fadeBackIn()
    {
        float timeWaited = 0f;

        yield return null;
        yield return null; //2 frames of instant camera speed

        if(PlayerOOCStateManager.currentActivity == OOCActivity.inDialogue)
        {
            DialogueManager.setCameraToDialogueSpeed();
        } else
        {
            if(RevealManager.currentlyRevealed)
            {
                RevealManager.revealAllObjects();
            }

            DialogueManager.setCameraToDefaultSpeed();
        }        

        while (!isTransparent())
        {
            frameCount = Mathf.Lerp(maxOpacity, 0f, timeWaited / fadeTime);

            timeWaited += Time.deltaTime;
            updateFadeToBlackImageOpacity();

            if (isTransparent())
            {
                break;
            }

            yield return null;
        }
		
		
		frameCount = 0;
		updateFadeToBlackImageOpacity();
        
        if(!Flags.isInNewGameMode())
        {
            OnFadeBackInFinished.Invoke();
        }
	}

	public static void delayFadingIn()
	{
		waitToFadeIn = true;
	}
		
	public static void allowFadingIn()
	{
		waitToFadeIn = false;
	}
	

	private void updateFadeToBlackImageOpacity()
	{
		fadeToBlackImage.color = new Color(0f,0f,0f, frameCount/maxOpacity);
	}

	public static void setToMaxOpacity()
	{
		frameCount = maxOpacity;
	}
}
