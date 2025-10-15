using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface ILessonDisplayInfo
{
	public string getType();
	public string getLessonDescription();
	public string getMechanicalDescription();
}

public class LessonUIManager : MonoBehaviour
{
	private static LessonUIManager instance;
	
	private static int xpPanelIndex;
	private const string lessonWindowPrefab = "New Lesson Window";
	private const string xpTitle = "Personal Perspective";
	private const string xpType = "Bonus XP";
	private const string xpLoreDescription = "You walk away with a more personal lesson.";
	private const string xpUseDescriptionStart = "Gain ";
	private const string xpUseDescriptionEnd = " extra experience points.";

	public Transform scrollableArea; 

	public bool isLearningLesson{get; private set;}
	public static bool waitingToActivate {get; private set;}
	private DescriptionPanel[] lessonDescriptionPanels = new DescriptionPanel[0];
	public GameObject learnLessonScreen;
	public Button acceptButton;
	
	private string[] lessonKeys = new string[0];
	public ArrayList lessonsToDisplay;

	public int currentLessonIndex;
	public static int lastEarnedXPBonus;
	
	public static LessonUIManager getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("instance of LessonUIManager already exists");
		}
		
		instance = this;
	}
		
	public static IEnumerator activateAfterDialogue()
    {
		waitingToActivate = true;
		DialogueManager dialogueManager = DialogueManager.getInstance();
		
		while(PlayerOOCStateManager.currentActivity == OOCActivity.inDialogue)
        {
            yield return null;
        }
		
		instance.activate();
		waitingToActivate = false;
		
		yield break;
    }
	
	private void activate()
	{
		isLearningLesson = true;
		learnLessonScreen.SetActive(true);
		populateDescriptionPanels();
		setAcceptButtonInteractability(false);
	}
	
	public void deactivate()
	{
		isLearningLesson = false;
		learnLessonScreen.SetActive(false);
		setAcceptButtonInteractability(false);
	}
	
	public void populateDescriptionPanels()
	{
		lessonsToDisplay = LessonManager.getLessons(lessonKeys);
		
		int index = 0;
		foreach(Lesson lesson in lessonsToDisplay)
		{
			instantiateNewDescriptionPanel();
			
			lessonDescriptionPanels[index].nameText.text = lesson.getKey();
			lessonDescriptionPanels[index].typeText.text = lesson.getType();
			lessonDescriptionPanels[index].loreDescriptionText.text = lesson.getLessonDescription();
			lessonDescriptionPanels[index].useDescriptionText.text = lesson.getMechanicalDescription();
			
			setLessonButtonOnClick(lessonDescriptionPanels[index].gameObject.GetComponent<Button>(), index);
			
			index++;
		}
		
		populateXPPanel();
	}
	
	private void setLessonButtonOnClick(Button button, int index)
	{
		button.onClick.AddListener(() => selectLesson(index));
	}
	
	private void instantiateNewDescriptionPanel()
	{
		GameObject newDescriptionPanelGO = Instantiate(Resources.Load<GameObject>(lessonWindowPrefab),scrollableArea);
		
		lessonDescriptionPanels = Helpers.appendArray<DescriptionPanel>(lessonDescriptionPanels, 
																		newDescriptionPanelGO.GetComponent<DescriptionPanel>());
	}
	
	private void populateXPPanel()
	{
		instantiateNewDescriptionPanel();
		xpPanelIndex = lessonDescriptionPanels.Length-1;
		DescriptionPanel xpDescriptionPanel = lessonDescriptionPanels[xpPanelIndex];
		
		xpDescriptionPanel.nameText.text = xpTitle;
		xpDescriptionPanel.typeText.text = xpType;
		xpDescriptionPanel.loreDescriptionText.text = xpLoreDescription;
		xpDescriptionPanel.useDescriptionText.text = xpUseDescriptionStart + lastEarnedXPBonus + xpUseDescriptionEnd;
		
		setLessonButtonOnClick(lessonDescriptionPanels[xpPanelIndex].gameObject.GetComponent<Button>(), xpPanelIndex);
	}
	
	public void learnLesson()
	{
		if(currentLessonIndex < lessonsToDisplay.Count)
		{
			LessonManager.addLesson((Lesson) lessonsToDisplay[currentLessonIndex]);
		} else
		{
			PartyManager.addXP(lastEarnedXPBonus);
		}
		
		resetLessons();
		lessonsToDisplay = new ArrayList();
	}
	
	private void setAcceptButtonInteractability(bool interactable)
	{
		acceptButton.interactable = interactable;
	}
	
	public void selectLesson(int panelIndex)
	{
		currentLessonIndex = panelIndex;
		DescriptionPanel panel = lessonDescriptionPanels[panelIndex];
		
		for(int index = 0; index < lessonDescriptionPanels.Length; index++)
		{
			if(index == panelIndex)
			{
				adjustColorsOfButton(lessonDescriptionPanels[index], Color.white);
			} else
			{
				adjustColorsOfButton(lessonDescriptionPanels[index], Color.grey);
			}
		}
		
		setAcceptButtonInteractability(true);
	}
	
	private void adjustColorsOfButton(DescriptionPanel panel, Color color)
	{
		Transform panelTransform = panel.gameObject.transform;
		
		panelTransform.GetChild(0).GetChild(0).GetComponent<Image>().color = color;
		panelTransform.GetChild(0).GetChild(1).GetComponent<Image>().color = color;
		panelTransform.GetChild(0).GetChild(2).GetComponent<Image>().color = color;
		panelTransform.GetChild(0).GetChild(3).GetComponent<Image>().color = color;
	}
	
	private void resetLessons()
	{
		lessonKeys = new string[0];
	}
	
	public void addLessonKey(string lessonKey)
	{
		this.lessonKeys = Helpers.appendArray<string>(this.lessonKeys, lessonKey);
	}

}
