using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSequenceStepTargetObject : MonoBehaviour, ITutorialSequenceTarget 
{
	public static Dictionary<string, List<ITutorialSequenceTarget>> hashDictionary = new Dictionary<string, List<ITutorialSequenceTarget>>(); 

	public bool useUltraWideTutorialWindow = false;
	public RectTransform rectTransform;
	public string tutorialHash;
	public bool useUIScale = false;
	public bool disableArrow = false;

	private void Awake()
	{
		if (rectTransform == null)
		{
			setRectTransform(gameObject.GetComponent<RectTransform>());
		}
	}

	public static void addToHashDictionary(ITutorialSequenceTarget targetObject)
	{
		if (targetObject == null ||
			targetObject.getTutorialHash() == null)
		{
			return;
		}

		if (hashDictionary.ContainsKey(targetObject.getTutorialHash()) &&
				!hashDictionary[targetObject.getTutorialHash()].Contains(targetObject))
			{
				hashDictionary[targetObject.getTutorialHash()].Add(targetObject);
			}
			else if (!hashDictionary.ContainsKey(targetObject.getTutorialHash()))
			{
				hashDictionary.Add(targetObject.getTutorialHash(), new List<ITutorialSequenceTarget>() { targetObject });
			}
	}

	public static void removeFromHashDictionary(ITutorialSequenceTarget targetObject)
	{
		if (hashDictionary.ContainsKey(targetObject.getTutorialHash()))
		{
			hashDictionary[targetObject.getTutorialHash()].Remove(targetObject);
		}
	}

	public static ITutorialSequenceTarget getLatestTutorialTarget(string tutorialHash)
	{
		if (hashDictionary.ContainsKey(tutorialHash) && hashDictionary[tutorialHash].Count >= 1)
		{
			return hashDictionary[tutorialHash][hashDictionary[tutorialHash].Count - 1];
		}
		else
		{
			Debug.LogError("No Target Object with Hash Exists in Dictionary: " + tutorialHash);
			return null;
		}
	}

	private void OnEnable()
	{
		createListeners();
	}

	private void OnDisable()
	{
		destroyListeners();
		removeFromHashDictionary(this);
    }

	//ITutorialSequenceTarget interface methods

	public virtual bool isUI()
	{
		return useUIScale;
	}

	public virtual void createListeners()
	{
		TutorialSequence.TutorialSequenceTargetFinder.AddListener(assignToTutorialSequence);
	}

	public virtual void destroyListeners()
	{
		TutorialSequence.TutorialSequenceTargetFinder.RemoveListener(assignToTutorialSequence);
	}

	public virtual Vector2 getDimensions()
	{
		return (new Vector2(getRectTransform().rect.width/4f, getRectTransform().rect.height/4f) * PlayerMovement.getTransform().localScale);
	}

	public virtual Vector2 getPosition()
	{
		return getRectTransform().TransformPoint(getRectTransform().rect.center);
	}

	public virtual string getTutorialHash()
	{
		return tutorialHash;
	}

	public virtual void assignToTutorialSequence(TutorialSequenceStep tutorialSequenceStep)
	{
		if (tutorialSequenceStep.isTutorialTarget(getTutorialHash()))
		{
			addToHashDictionary(this); 
			PopUpBlocker.spawnPopUpScreenBlocker();
			tutorialSequenceStep.createMessageWindowAndRunScript(getTutorialHash(), useUltraWideTutorialWindow, disableArrow);
		}
	}

	public GameObject getGameObject()
	{
		return gameObject;
	}

	public void setRectTransform(RectTransform rectTransform)
	{
		this.rectTransform = rectTransform;
	}

	public RectTransform getRectTransform()
	{
		if (rectTransform == null)
		{
			rectTransform = gameObject.GetComponent<RectTransform>();
		}
		
		return rectTransform;
	}
	
	public Transform getTransform()
	{
		return transform;
	}

	public virtual void highlight(bool skip)
	{
		if(skip)
		{
			return;
		}

		RevealManager.manuallyRevealGameObject(gameObject, RevealManager.tutorialDefault);
	}
	
    public virtual void unhighlight(bool skip)
	{
		if(skip)
		{
			return;
		}
		
		RevealManager.manuallyUnrevealGameObject(gameObject);	
	}
}
