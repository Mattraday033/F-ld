using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NotificationManager : MonoBehaviour  
{
    public static List<IDescribable> notificationQueue;
    private static NotificationManager instance;

    public readonly static UnityEvent OnDeleteAllNotifications = new UnityEvent();

    private static bool skipNextSpawn;

    private static string previousLocation = "";

    private const float timeBetweenNotifications = 1f;
    private float elapsedTime = 0f;

    private static NotificationPopUpButton notificationPopUpButton;

    public static NotificationManager getInstance()
    {
        return instance;
    }

    public static void skipNextNotificationSpawn() 
    { 
        skipNextSpawn = true;
    }

    public static void addHostilityAlertToNotificationQueue()
    {
        notificationQueue.Insert(Constants.indexZero, new Notification("The current Area is now Hostile.", "This may affect the outcome of certain quests."));
    }

    public static void addToNotificationQueue(IDescribable notification)
    {
        notificationQueue.Add(notification);
    }

    public static void addToNotificationQueue(IDescribable notification, int index)
    {
        notificationQueue.Insert(index, notification);
    }

    public static void spawnQuickSaveNotification(IDescribable notification)
    {
        addToNotificationQueue(notification);

        startSpawningNotifications();
    }

    private static void startSpawningNotifications()
    {
        if (notificationQueue.Count == 0 || skipNextSpawn)
        {
            skipNextSpawn = false;
            return;
        }

        if (notificationPopUpButton == null || notificationPopUpButton is null)
        {
            notificationPopUpButton = Instantiate(Resources.Load<GameObject>(PrefabNames.notificationPopUpButton), instance.transform).GetComponent<NotificationPopUpButton>();
        }

        for (int index = 0; index < notificationQueue.Count; index++)
        {
            IDescribable describable = notificationQueue[index];

            if (AreaManager.getInstance().getAreaDescription().getName().Equals(describable.getName()) ||
                describable.getName().Contains(SaveHandler.quickSaveName))
            {
                if ((PlayerOOCStateManager.currentActivity == OOCActivity.walking || PlayerOOCStateManager.currentActivity == OOCActivity.inFade) 
                    && !State.hasLoadedDialogueKey())
                {
                    instance.StartCoroutine(instance.spawnNotification(describable));
                }

                notificationQueue.RemoveAt(index);
                break;
            }
        }

        if (notificationQueue.Count > 0 && ((PlayerOOCStateManager.currentActivity == OOCActivity.walking || PlayerOOCStateManager.currentActivity == OOCActivity.inUI) && !State.hasLoadedDialogueKey()))
        {
            notificationPopUpButton.spawnPopUp();
        }
    }

    private IEnumerator spawnNotification(IDescribable describable)
    {
        while (FadeToBlackManager.isMidScreenFade())
        {
            yield return null;
        }

        describeNotification(describable);
    }
    
    private static void describeNotification(IDescribable notification)
    {
        DescriptionPanel descriptionPanel = GameObject.Instantiate(notification.getDescriptionPanelFull(PanelType.Notification), OverallUIManager.notificationParent).GetComponent<DescriptionPanel>();

        descriptionPanel.transform.SetAsFirstSibling();

        notification.describeSelfFull(descriptionPanel);
    }

    public static void skipWaitForNextNotificationSpawn()
    {
        getInstance().elapsedTime += timeBetweenNotifications;
    }

    public static void purgeNotifications()
    {
        notificationQueue = new List<IDescribable>();
    }

    public static GameObject getCurrentNotificationPopUpWindowGameObject()
    {
        if (notificationPopUpButton == null)
        {
            return null;
        }

        return notificationPopUpButton.getCurrentPopUpGameObject();
    }

    private static void spawnNotificationsOnAreaChange()
    {
        if (!skipNextSpawn)
        {
            //purgeNotifications();
        }
        else
        {
            skipNextSpawn = false;
            // return;
        }

        // if (AreaManager.getInstance() != null)
        // {
        if(!AreaManager.locationName.Equals(previousLocation))
        {
            addToNotificationQueue(AreaManager.getInstance().getAreaDescription(), 0);
            previousLocation = AreaManager.locationName;
        }

        startSpawningNotifications();
        // }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeNotificationManager()
    {
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(startSpawningNotifications);
        PlayerOOCStateManager.OnLeavingTutorialSequenceState.AddListener(startSpawningNotifications);
        AreaManager.OnAreaSpawn.AddListener(spawnNotificationsOnAreaChange);

        LoadSaveFile.OnLoad.AddListener(resetPreviousLocation);

        notificationQueue = new List<IDescribable>();
        
        notificationPopUpButton = null;
        instance = null;
        skipNextSpawn = false;
        previousLocation = "";
    }

    private static void resetPreviousLocation()
    {
        previousLocation = "";
    }

    public void Awake()
    {
        instance = this;
    }

}

public enum GenericNotificationType { Alert, Update }

public class Notification : IDescribable
{
    private GenericNotificationType type;
    private string notificationName;
    private string notificationDescription;

    public Notification(string notificationName, string notificationDescription, GenericNotificationType type = GenericNotificationType.Alert)
    {
        this.type = type;
        this.notificationName = notificationName;
        this.notificationDescription = notificationDescription;
    }

	public string getName()
	{
		return notificationName;
	}

    public bool ineligible()
    {
        return false;
    }

	public GameObject getRowType(RowType rowType)
	{
		switch (rowType)
		{
			case RowType.Map:
				return Resources.Load<GameObject>(PrefabNames.mapQuestObjectiveRow);
			case RowType.MapWithoutHover:
				return Resources.Load<GameObject>(PrefabNames.mapQuestObjectiveRowWithoutHover);
			default:
				return Resources.Load<GameObject>(PrefabNames.glossaryCategoryRow);
		}
	}

	public GameObject getDescriptionPanelFull()
	{
		return getDescriptionPanelFull(PanelType.Standard);
	}

	public GameObject getDescriptionPanelFull(PanelType type)
	{
		string panelTypeName = "";

		switch (type)
		{
			case PanelType.Notification:
                panelTypeName = PrefabNames.questStepNotificationDescriptionPanel;
				break;
			default:
				panelTypeName = PrefabNames.writtenGlossaryEntryFull;
				break;
		}

		return DescriptionPanel.getDescriptionPanel(panelTypeName);
	}

	public GameObject getDecisionPanel()
	{
		return null;
	}

	public bool withinFilter(string[] filterParameters)
	{
		return false;
	}

	public void describeSelfFull(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

        DescriptionPanel.setText(panel.notificationNameText, type.ToString() + "!");

		DescriptionPanel.setText(panel.secondaryNameText, getName());
		DescriptionPanel.setText(panel.loreDescriptionText, notificationDescription);
	}

	public void describeSelfRow(DescriptionPanel panel)
	{
		panel.setObjectBeingDescribed(this);

		DescriptionPanel.setText(panel.nameText, DialogueList.scrubNameOfEndNumbers(getName()));
		DescriptionPanel.setText(panel.secondaryNameText, notificationDescription);
	}

	public void setUpDecisionPanel(IDecisionPanel descisionPanel)
	{

	}

	public List<IDescribable> getRelatedDescribables()
	{
		return new List<IDescribable>();
	}

	public bool buildableWithBlocks()
	{
		return true;
	}
	
	public bool buildableWithBlocksRows()
    {
        return true;
    }
	public List<DescriptionPanelBuildingBlock> getDescriptionBuildingBlocks()
	{
		List<DescriptionPanelBuildingBlock> buildingBlocks = new List<DescriptionPanelBuildingBlock>();

		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock("Update"));
		buildingBlocks.Add(DescriptionPanelBuildingBlock.getNameBlock(DialogueList.scrubNameOfEndNumbers(getName())));
        buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, ""));
		buildingBlocks.Add(new DescriptionPanelBuildingBlock(DescriptionPanelBuildingBlockType.Text, notificationDescription));

		return buildingBlocks;
	}

    public bool requiresInspectNode()
    {
        return false;
    }
}