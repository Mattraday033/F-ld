using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum FadeType { Screen, Music };

public class FadeToBlackManager : MonoBehaviour
{
	private static FadeToBlackManager instance;
    public readonly static UnityEvent OnFadeToBlack = new UnityEvent();
    public readonly static UnityEvent OnFadeBackInFinished = new UnityEvent();

    public Camera mainCamera;
    public CinemachineVirtualCamera mainCM;

    public Canvas fadeToBlackCanvas;

    public Image fadeToBlackImage;

    private readonly static Dictionary<FadeType, IFade> fadeDictionary = new Dictionary<FadeType, IFade>();

    [RuntimeInitializeOnLoadMethod]
    private static void initializeFadeToBlackManager()
    {
        instance = null;
    }

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(LayerAndTagManager.fadeToBlackTag);

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }    
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(mode == LoadSceneMode.Additive)
        {
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;

        setCameras();

        if(CombatStateManager.inCombat)
        {
            
            StartCoroutine(waitTwoFramesThenStartFadeBackIn(new CircleTransitionReduce(mainCamera.transform, OOCActivity.walking)));
        } else
        {
            if(PlayerOOCStateManager.currentActivity == OOCActivity.walking && !Flags.isInNewGameMode())
            {
                PlayerOOCStateManager.setCurrentActivity(OOCActivity.inFade);
            }

            setToMaxOpacity();
            StartCoroutine(waitTwoFramesThenStartFadeBackIn(new FadeBackInTransition()));
        }        
    }

    private IEnumerator waitTwoFramesThenStartFadeBackIn(ScreenFade fade)
    {
        yield return null;
        yield return null;

        instance = this;
        createFade(fade);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

	public static FadeToBlackManager getInstance()
	{
		return instance;
	}

	public void setCameras()
    {
		mainCamera = Camera.main;

        GameObject mainCMObject = GameObject.FindWithTag(LayerAndTagManager.mainVirtualCameraTag);

        if(mainCMObject != null)
        {
		    mainCM = mainCMObject.GetComponent<CinemachineVirtualCamera>(); //no mainCM in Loading Screen Scene
        }

		fadeToBlackCanvas.worldCamera = mainCamera;
	}

	public static bool isMidScreenFade()
	{
        return fadeDictionary.ContainsKey(FadeType.Screen) && 
                fadeDictionary[FadeType.Screen] != null && 
                !fadeDictionary[FadeType.Screen].isFinished();
	}

	public void setAndStartFadeBackIn()
	{
		createFade(new FadeBackInTransition());
	}

	public void setAndStartFadeToBlack()
	{
		createFade(new FadeToBlackTransition());
	}
	
    public static void startCombatTransition(Transform monsterTransform)
    {
        if(instance == null)
        {
            return;
        }

        createFade(new CircleTransitionEnlarge(monsterTransform, OOCActivity.preCombat));
    }

    public static void createFade(ScreenFade fade)
    {
        if(fade == null)
        {
            return;
        }

        fadeDictionary[fade.getFadeType()] = fade;
        fadeDictionary[fade.getFadeType()].setActiveCoroutine(instance.StartCoroutine(fade.getCoroutineTemplate()));
    }

    public static void createFade(BetweenAreaFade newFade)
    {
        if(newFade == null || 
            instance == null)
        {
            return;
        } else if(fadeDictionary.ContainsKey(newFade.getFadeType()) &&
             (fadeDictionary[newFade.getFadeType()] as BetweenAreaFade) != null)
        {
            BetweenAreaFade oldFade = fadeDictionary[newFade.getFadeType()] as BetweenAreaFade;

            if(oldFade.newClipPath.Equals(newFade.newClipPath))
            {
                return;
            } else if(oldFade.originalClipPath.Equals(newFade.newClipPath) && oldFade.fadeOut)
            {
                newFade.fadeOut = false;
            } 

            newFade.setTimeWaited(oldFade.timeWaited, oldFade.fadeOut);            
            StopFade(newFade.getFadeType());
        }

        fadeDictionary[newFade.getFadeType()] = newFade;
        fadeDictionary[newFade.getFadeType()].setActiveCoroutine(instance.StartCoroutine(newFade.getCoroutineTemplate()));
    }

    public static void setToMaxOpacity()
    {
        if(instance == null)
        {
            return;
        }
        
        instance.fadeToBlackImage.color = Color.black;
    }

    public static void StopFade(FadeType type)
    {
        if(instance != null && 
            fadeDictionary != null && 
            fadeDictionary.ContainsKey(type) &&
            fadeDictionary[type] != null)
        {
            if(fadeDictionary[type].getActiveCoroutine() != null)
            {
                instance.StopCoroutine(fadeDictionary[type].getActiveCoroutine());
            }

            fadeDictionary.Remove(type);
        }
    }
}
