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
        while (FadeToBlackManager.isMidFade())
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
        addToNotificationQueue(AreaManager.getInstance().getAreaDescription(), 0);

        startSpawningNotifications();
        // }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeNotificationManager()
    {
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(startSpawningNotifications);
        PlayerOOCStateManager.OnLeavingTutorialSequenceState.AddListener(startSpawningNotifications);
        AreaManager.OnAreaSpawn.AddListener(spawnNotificationsOnAreaChange);

        notificationQueue = new List<IDescribable>();
        
        notificationPopUpButton = null;
        instance = null;
        skipNextSpawn = false;
    }

    public void Awake()
    {
        instance = this;
    }

}
