using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NotificationManager : MonoBehaviour  
{
    public static ArrayList notificationQueue = new ArrayList();
    private static NotificationManager instance;

    public static UnityEvent OnDeleteAllNotifications = new UnityEvent();

    private static bool skipNextSpawn = false;

    private const float timeBetweenNotifications = 1f;
    private float elapsedTime = 0f;

    private NotificationPopUpButton notificationPopUpButton;

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

        instance.startSpawningNotifications();
    }

    private void startSpawningNotifications()
    {
        // Debug.LogError("spawning Notifications");

        if (notificationQueue.Count == 0 || skipNextSpawn)
        {
            skipNextSpawn = false;
            return;
        }

        if (notificationPopUpButton == null || notificationPopUpButton is null)
        {
            notificationPopUpButton = Instantiate(Resources.Load<GameObject>(PrefabNames.notificationPopUpButton), transform.parent).GetComponent<NotificationPopUpButton>();
        }

        for (int index = 0; index < notificationQueue.Count; index++)
        {
            IDescribable describable = (IDescribable)notificationQueue[index];

            if (AreaManager.getInstance().getAreaDescription().getName().Equals(describable.getName()) ||
                describable.getName().Contains(SaveHandler.quickSaveName))
            {
                if (PlayerOOCStateManager.currentActivity == OOCActivity.walking && !State.hasLoadedDialogueKey())
                {
                    StartCoroutine(spawnNotification(describable));
                }

                notificationQueue.RemoveAt(index);
                break;
            }
        }

        if (notificationQueue.Count > 0 && ((PlayerOOCStateManager.currentActivity == OOCActivity.walking || PlayerOOCStateManager.currentActivity == OOCActivity.inUI) && !State.hasLoadedDialogueKey()))
        {
            notificationPopUpButton.spawnPopUp();
            purgeNotifications();
        }
    }

    private IEnumerator spawnNotification(IDescribable describable)
    {
        while (!FadeToBlackManager.isTransparent())
        {
            yield return null;
        }

        describeNotification(describable);
    }
    
    private static void describeNotification(IDescribable notification)
    {
        DescriptionPanel descriptionPanel = GameObject.Instantiate(notification.getDescriptionPanelFull(PanelType.Notification), getInstance().transform).GetComponent<DescriptionPanel>();

        descriptionPanel.transform.SetAsFirstSibling();

        notification.describeSelfFull(descriptionPanel);
    }

    public static void skipWaitForNextNotificationSpawn()
    {
        getInstance().elapsedTime += timeBetweenNotifications;
    }

    private void OnDestroy()
    {
        PlayerOOCStateManager.OnStateChangeToWalking.RemoveListener(startSpawningNotifications);
        PlayerOOCStateManager.OnLeavingTutorialSequenceState.RemoveListener(startSpawningNotifications);
    }

    public static void purgeNotifications()
    {
        // Debug.LogError("purging notifications");
        notificationQueue = new ArrayList();
    }

    public static GameObject getCurrentNotificationPopUpWindowGameObject()
    {
        if (getInstance().notificationPopUpButton == null)
        {
            return null;
        }

        return getInstance().notificationPopUpButton.getCurrentPopUpGameObject();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Duplicate instances of NotificationManager exist erroneously.");
        }

        instance = this;
        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(startSpawningNotifications);
        PlayerOOCStateManager.OnLeavingTutorialSequenceState.AddListener(startSpawningNotifications);

        if (!skipNextSpawn)
        {
            //purgeNotifications();
        }
        else
        {
            skipNextSpawn = false;
        }

        if (AreaManager.getInstance() != null)
        {
            addToNotificationQueue(AreaManager.getInstance().getAreaDescription(), 0);

            startSpawningNotifications();
        }
    }

}
