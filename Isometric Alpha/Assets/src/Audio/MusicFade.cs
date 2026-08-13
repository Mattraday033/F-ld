using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MusicFade : IFade
{
    public readonly static UnityEvent OnMusicMidFade = new UnityEvent();

    private Coroutine activeCoroutine;
    public float timeWaited = 0;
    public float fadeTime = 2.5f;

    public abstract bool isFinished();

    public abstract IEnumerator getCoroutineTemplate();

    public void setActiveCoroutine(Coroutine coroutine)
    {
        activeCoroutine = coroutine;
    }

    public virtual void setTimeWaited(float newTimeWaited, bool incomingDirection = false)
    {
        timeWaited = newTimeWaited;
    }

    public virtual float getCurrentFadePercent()
    {
        return Mathf.Lerp(AudioManager.musicVolumePlayerSetting, 0f, timeWaited/fadeTime);
    }

    protected void setToFull()
	{
        AudioManager.setMusicSourceVolume(AudioManager.musicVolumePlayerSetting);
	}
    protected void setToMute()
	{
        AudioManager.setMusicSourceVolume(0f);
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

    public SFXType originalClip;
    public SFXType newClip;

    public bool fadeOut;

    public BetweenAreaFade(bool fadeOut, SFXType originalClip, SFXType newClip)
    {
        if(originalClip == SFXType.NoSFX)
        {
            this.fadeOut = false;
        } else
        {
            this.fadeOut = fadeOut;
        }
        
        this.originalClip = originalClip;
        this.newClip = newClip;
    }

    public override void setTimeWaited(float newTimeWaited, bool incomingDirection = false)
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

    public override float getCurrentFadePercent()
    {
        if(fadeOut)
        {
            return Mathf.Lerp(AudioManager.musicVolumePlayerSetting, 0f, timeWaited/fadeTime);
        } else
        {
            return Mathf.Lerp(0f, AudioManager.musicVolumePlayerSetting, timeWaited/fadeTime);
        }
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

public class FadeToSilence : MusicFade
{

    public FadeToSilence(float fadeTime = 2.5f)
    {
        this.fadeTime = fadeTime;
    }

	protected void updateMusicVolume()
	{
        float fadePercent = getCurrentFadePercent();

		AudioManager.setMusicSourceVolume(fadePercent);
	} 

    public override bool isFinished()
    {
        return timeWaited >= fadeTime;
    }

    public override IEnumerator getCoroutineTemplate()
    {
        while (!isFinished())
        {
            yield return null;

            timeWaited += Time.deltaTime;

            updateMusicVolume();
        }
		
        setToMute();
        FadeToBlackManager.StopFade(getFadeType());
    }
}