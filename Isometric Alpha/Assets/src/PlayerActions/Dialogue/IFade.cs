using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFade
{
    public bool isFinished();

    public IEnumerator getCoroutineTemplate();
    public void stopActiveCoroutine();

    public void setActiveCoroutine(Coroutine coroutine);
    public Coroutine getActiveCoroutine();

    public FadeType getFadeType();
}

public abstract class ScreenFade : IFade
{
    private Coroutine activeCoroutine;

    public abstract bool isFinished();

    public abstract IEnumerator getCoroutineTemplate();

    public void setActiveCoroutine(Coroutine coroutine)
    {
        activeCoroutine = coroutine;
    }

    public void stopActiveCoroutine()
    {
        if(activeCoroutine != null)
        {
            FadeToBlackManager.StopFade(getFadeType());
        }
    }

    public Coroutine getActiveCoroutine()
    {
        return activeCoroutine;
    }

    public FadeType getFadeType()
    {
        return FadeType.Screen;
    }
}

public abstract class FullScreenTransition : ScreenFade
{
	protected float frameCount = 0;
    protected const float slowFadeInSpeed = 3.5f;
    public float fadeTime = .5f;

	protected void updateFadeToBlackImageOpacity()
	{
		FadeToBlackManager.getInstance().fadeToBlackImage.color = new Color(0f,0f,0f, frameCount/Constants.maxOpacity);
	}

    protected void setToClear()
	{
		FadeToBlackManager.getInstance().fadeToBlackImage.color = Color.clear;
	}
    protected void setToOpaque()
	{
		FadeToBlackManager.getInstance().fadeToBlackImage.color = Color.black;
	}

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

    public override IEnumerator getCoroutineTemplate()
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
		
        setToOpaque();

        GC.Collect();

        DialogueManager.setCameraToDefaultSpeed();

        if(PlayerOOCStateManager.currentActivity == OOCActivity.inDialogue)
        {
            yield break;
        }

        yield return null;
        yield return null;
        yield return null;
        yield return null;

        fadeBackIn = new FadeBackInTransition();
        fadeBackIn.setFrameCountAtStart();

        while(!fadeBackIn.isFinished())
        {
            yield return fadeBackIn.getCoroutineTemplate();
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

    public override IEnumerator getCoroutineTemplate()
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
		
        setToClear();
        
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

public abstract class CircleTransition : ScreenFade
{
    protected const float minimumScale = 0.01f;
    protected const float maximumScale = 22.5f;
    protected const float transitionTimeSeconds = 1.25f;
    protected float currentScale = 0f;

    protected Transform parent;
    protected OOCActivity endingState;

    protected abstract float getStartingScale();
    protected abstract float getEndingScale();

    protected Transform createCircleTransition()
    {
        Transform circleTransition =  GameObject.Instantiate(Resources.Load<GameObject>(PrefabNames.circleTransitionObject), parent).transform;

        circleTransition.position = new Vector3(circleTransition.position.x,
                                                circleTransition.position.y,
                                                -10f);

        Helpers.updateColliderPosition(circleTransition);

        return circleTransition;
    }

    protected Vector3 getCurrentScale(float timeWaited)
    {
        currentScale = Mathf.Lerp(getStartingScale(), getEndingScale(), timeWaited/transitionTimeSeconds);

        return new Vector3(currentScale, currentScale, 1f);
    }

    public override IEnumerator getCoroutineTemplate()
    {
        Transform circleTransitionObject = createCircleTransition();

        float timeWaited = 0f;

        do
        {
            timeWaited += Time.deltaTime;

            circleTransitionObject.localScale = getCurrentScale(timeWaited);

            yield return null;
        }while(!isFinished());

        PlayerOOCStateManager.setCurrentActivity(endingState);

        if(CombatStateManager.inCombat)
        {
            GameObject.DestroyImmediate(circleTransitionObject.gameObject);
        }
    }
}

public class CircleTransitionEnlarge : CircleTransition
{

    public CircleTransitionEnlarge(Transform parent, OOCActivity endingState)
    {
        this.parent = parent;
        this.endingState = endingState;
    }

    protected override float getStartingScale()
    {
        return minimumScale;
    }
    protected override float getEndingScale()
    {
        return maximumScale;
    }

    public override bool isFinished()
    {
        return currentScale >= getEndingScale();
    }
}

public class CircleTransitionReduce : CircleTransition
{

    public CircleTransitionReduce(Transform parent, OOCActivity endingState)
    {
        this.parent = parent;
        this.endingState = endingState;
    }

    protected override float getStartingScale()
    {
        return maximumScale;
    }
    protected override float getEndingScale()
    {
        return minimumScale;
    }

    public override bool isFinished()
    {
        return currentScale <= getEndingScale();
    }
}