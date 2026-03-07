using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MusicFade : IFade
{
    public readonly static UnityEvent OnMusicMidFade = new UnityEvent();

    private Coroutine activeCoroutine;

    public abstract bool isFinished();

    public abstract IEnumerator getCoroutineTemplate();

    public void setActiveCoroutine(Coroutine coroutine)
    {
        activeCoroutine = coroutine;
    }

    public Coroutine getActiveCoroutine()
    {
        return activeCoroutine;
    }

    public FadeType getFadeType()
    {
        return FadeType.Music;
    }

    public void stopActiveCoroutine()
    {
        if(activeCoroutine != null)
        {
            FadeToBlackManager.StopFade(getFadeType());
        }
    }
}

public class BetweenAreaFade : MusicFade
{

    public float timeWaited = 0;
    public float fadeTime = 2.5f;

    public string originalClipPath;
    public string newClipPath;

    public bool fadeOut;

    public BetweenAreaFade(bool fadeOut, string originalClipPath, string newClipPath)
    {
        if(originalClipPath.Equals(Constants.emptyString))
        {
            this.fadeOut = false;
        } else
        {
            this.fadeOut = fadeOut;
        }
        
        this.originalClipPath = originalClipPath;
        this.newClipPath = newClipPath;
    }

    public void setTimeWaited(float newTimeWaited, bool incomingDirection)
    {
        if(incomingDirection != fadeOut)
        {
            timeWaited = fadeTime*(1-(newTimeWaited/fadeTime));
        } else
        {
            timeWaited = newTimeWaited;
        }
    }

	protected void updateMusicVolume()
	{
        float fadePercent = getCurrentFadePercent();

		AudioManager.setMusicSourceVolume(fadePercent);
	} 

    public float getCurrentFadePercent()
    {
        if(fadeOut)
        {
            return Mathf.Lerp(AudioManager.musicVolumePlayerSetting, 0f, timeWaited/fadeTime);
        } else
        {
            return Mathf.Lerp(0f, AudioManager.musicVolumePlayerSetting, timeWaited/fadeTime);
        }
    }

    protected void setToFull()
	{
        AudioManager.setMusicSourceVolume(AudioManager.musicVolumePlayerSetting);
        AudioManager.setFootStepSourceVolume(AudioManager.footstepPlayerSetting);
	}
    protected void setToMute()
	{
        AudioManager.setMusicSourceVolume(0f);
        // AudioManager.setFootStepSourceVolume(0f);
	}

    public override bool isFinished()
    {
        return timeWaited >= fadeTime;
    }

    public override IEnumerator getCoroutineTemplate()
    {
        if(Flags.isInNewGameMode())
        {
            fadeOut = false;
            setToMute();
        }

        while (!isFinished())
        {
            yield return null;

            if(fadeOut)
            {
                timeWaited += Time.deltaTime;
            } else
            {
                timeWaited += Time.deltaTime/2f;
            }

            if(fadeOut && timeWaited >= fadeTime)
            {
                fadeOut = false;
                timeWaited = 0;
                setToMute();
                OnMusicMidFade.Invoke();
            } else
            {
                updateMusicVolume();
            }

            if (isFinished())
            {
                break;
            }
        }
		
        setToFull();
        FadeToBlackManager.StopFade(getFadeType());
    }
}