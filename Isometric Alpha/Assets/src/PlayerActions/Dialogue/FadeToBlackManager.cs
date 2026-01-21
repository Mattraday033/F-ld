using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;
using UnityEngine.Events;


public interface IFadeTransition
{
    public bool isFinished();

    public IEnumerator getCoroutine();
}

public abstract class FullScreenTransition : IFadeTransition
{

	protected float frameCount = 0;
    protected const float slowFadeInSpeed = 3.5f;
    public float fadeTime = .5f;

	protected void updateFadeToBlackImageOpacity()
	{
		FadeToBlackManager.getInstance().fadeToBlackImage.color = new Color(0f,0f,0f, frameCount/Constants.maxOpacity);
	}

	public abstract bool isFinished();

    public abstract IEnumerator getCoroutine();

    public abstract void setFrameCountAtStart();
}

public class FadeToBlackTransition : FullScreenTransition
{

    private FadeBackInTransition fadeBackIn;

	public override bool isFinished()
	{
        if(fadeBackIn == null)
        {
		    return frameCount >= Constants.maxOpacity;
        } else
        {
            return fadeBackIn.isFinished();
        }
	}

    public override void setFrameCountAtStart()
    {
        frameCount = 0f;
        updateFadeToBlackImageOpacity();
    }

    public override IEnumerator getCoroutine()
    {
        if(PlayerOOCStateManager.currentActivity == OOCActivity.walking ||
            PlayerOOCStateManager.currentActivity == OOCActivity.inMap )
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.inFade);
        }

		FadeToBlackManager.OnFadeToBlack.Invoke();

        if(Flags.isInNewGameMode())
        {
            fadeTime = slowFadeInSpeed;
        }

        float timeWaited = 0f;

        while (!isFinished())
        {
            frameCount = Mathf.Lerp(0f, Constants.maxOpacity, timeWaited/fadeTime);
            timeWaited += Time.deltaTime;
            updateFadeToBlackImageOpacity();

            if (isFinished())
            {
                break;
            }

            yield return null;
        }
		
        GC.Collect();

        DialogueManager.setCameraToDefaultSpeed();

        yield return null;
        yield return null;
        yield return null;
        yield return null;

        fadeBackIn = new FadeBackInTransition();
        fadeBackIn.setFrameCountAtStart();

        while(!fadeBackIn.isFinished())
        {
            yield return fadeBackIn.getCoroutine();
        }
    }
}

public class FadeBackInTransition : FullScreenTransition
{
	public override bool isFinished()
	{
		return frameCount <= 0;
	}

    public override void setFrameCountAtStart()
    {
        frameCount = Constants.maxOpacity;
        updateFadeToBlackImageOpacity();
    }

    public override IEnumerator getCoroutine()
    {
        setFrameCountAtStart();
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

        while (!isFinished())
        {
            frameCount = Mathf.Lerp(Constants.maxOpacity, 0f, timeWaited / fadeTime);

            timeWaited += Time.deltaTime;
            updateFadeToBlackImageOpacity();

            if (isFinished())
            {
                break;
            }

            yield return null;
        }
		
		
		frameCount = 0;
		updateFadeToBlackImageOpacity();
        
        if(!Flags.isInNewGameMode())
        {
            FadeToBlackManager.OnFadeBackInFinished.Invoke();
        }

        if(PlayerOOCStateManager.currentActivity == OOCActivity.inFade)
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.walking);
        }
	}
}

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

    private static bool waitToFadeIn;

    public bool fadeBackInOnStart = false;

	private static Coroutine currentCoroutine;
    private static IFadeTransition currentFadeTransition;

    [RuntimeInitializeOnLoadMethod]
    private static void initializeFadeToBlackManager()
    {
        waitToFadeIn = false;

        currentCoroutine = null;
        currentFadeTransition = null;

        instance = null;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Fade To Black Manager in the scene.");
        }

        currentCoroutine = null;
        currentFadeTransition = null;
        waitToFadeIn = false;

        instance = this;


    }

	public static FadeToBlackManager getInstance()
	{
		return instance;
	}

	void Start()
	{
		setCameras();

        if(PlayerOOCStateManager.currentActivity == OOCActivity.walking)
        {
            PlayerOOCStateManager.setCurrentActivity(OOCActivity.inFade);
        }

        setToMaxOpacity();
        setAndStartFadeBackIn();
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

    public void createFade(IFadeTransition fadeTransition)
    {
        currentFadeTransition = fadeTransition;	
		currentCoroutine = StartCoroutine(currentFadeTransition.getCoroutine());
    }

	public static void delayFadingIn()
	{
		waitToFadeIn = true;
	}
		
	public static void allowFadingIn()
	{
		waitToFadeIn = false;
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
