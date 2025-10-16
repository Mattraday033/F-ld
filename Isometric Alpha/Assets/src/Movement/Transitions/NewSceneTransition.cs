using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionHash : ICloneable
{
	public int index;
	public string currentSceneName;
	public string targetSceneName;

	public TransitionHash(int index, string currentSceneName, string targetSceneName)
	{
		this.index = index;
		this.currentSceneName = currentSceneName;
		this.targetSceneName = targetSceneName;
	}

	public override bool Equals(object obj)
	{
		var other = (TransitionHash)obj;

		if (other.index == this.index &&

			((String.Equals(currentSceneName, other.currentSceneName, StringComparison.OrdinalIgnoreCase) &&
			 String.Equals(targetSceneName, other.targetSceneName, StringComparison.OrdinalIgnoreCase)) ||

			 (String.Equals(currentSceneName, other.targetSceneName, StringComparison.OrdinalIgnoreCase) &&
			 String.Equals(targetSceneName, other.currentSceneName, StringComparison.OrdinalIgnoreCase))))
		{
			return true;
		}

		return false;
	}
	
	public object Clone()
	{
		return this.MemberwiseClone();
	}
	
	public TransitionHash clone()
    {
        return  (TransitionHash) Clone();
    }
}

[Serializable]
public class TransitionInfo : ICloneable
{
	public TransitionHash transitionHash = null;
	public bool flipFacing = false;
	public string hash;
    public string sceneName;
	public bool skipAutoSave;
	public string sortingLayerName;

	public PlayerInteractionScript[] scripts;
	
	public TransitionInfo(string hash, string sceneName)
	{
		this.hash = hash;
		this.sceneName = sceneName;
	}
	
	public TransitionInfo(Scene targetScene, int index)
	{
		this.transitionHash = new TransitionHash(index, SceneManager.GetActiveScene().name, targetScene.name);
	}
	
	public object Clone()
	{
		return this.MemberwiseClone();
	}

	public string getTargetLocationName()
	{
		if (transitionHash != null)
		{
			// Debug.LogError("getTargetLocationName() is returning: " + transitionHash.targetSceneName);
			return transitionHash.targetSceneName;
		}
		else
		{
			// Debug.LogError("sceneName = " + sceneName); 
			return sceneName;
		}
	}

	public TransitionInfo clone()
	{
		TransitionInfo transitionInfoClone = (TransitionInfo) Clone();

		transitionInfoClone.scripts = scripts; //doesn't really clone anything but PlayerInteractionScripts shouldn't contain state so it shouldn't matter

		if (transitionHash != null)
		{
			transitionInfoClone.transitionHash = transitionHash.clone();
		}

		return transitionInfoClone;
	}
}

[Serializable]
public class NewSceneTransition : MonoBehaviour
{

	public int targetSceneBuildOrder = -1;

	public int index = -1;
	public bool fastTravelTarget;

	public Collider2D collider;
	//[SerializeField]
	private TransitionInfo info;

	public GameObject currentSceneDestinationSquare;

	public TransitionInfo getTransitionInfo()
	{

		if (targetSceneBuildOrder < 0)
		{
			return info;
		}
		else
		{
			info.transitionHash = getTransitionHash();
			return info;
		}
	}

	private TransitionHash getTransitionHash()
	{
		return new TransitionHash(index, SceneManager.GetActiveScene().name, extractTargetSceneNameFromScenePath(SceneUtility.GetScenePathByBuildIndex(targetSceneBuildOrder)));
	}

	private static string extractTargetSceneNameFromScenePath(string scenePath)
	{
		return scenePath.Split("/")[scenePath.Split("/").Length - 1].Split(".")[0];
	}

}