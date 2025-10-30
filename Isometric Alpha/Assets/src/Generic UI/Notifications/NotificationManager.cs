using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NotificationManager : MonoBehaviour  
{
    public static ArrayList notificationQueue;
    private static NotificationManager instance;

    public static UnityEvent OnDeleteAllNotifications;

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
            IDescribable describable = (IDescribable)notificationQueue[index];

            if (AreaManager.getInstance().getAreaDescription().getName().Equals(describable.getName()) ||
                describable.getName().Contains(SaveHandler.quickSaveName))
            {
                if (PlayerOOCStateManager.currentActivity == OOCActivity.walking && !State.hasLoadedDialogueKey())
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
        notificationQueue = new ArrayList();
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
        }

        // if (AreaManager.getInstance() != null)
        // {
        addToNotificationQueue(AreaManager.getInstance().getAreaDescription(), 0);

        Debug.LogError("notificationQueue.Count = " + notificationQueue.Count);

        startSpawningNotifications();
        // }
    }

    [RuntimeInitializeOnLoadMethod]
    private static void initializeNotificationManager()
    {
        OnDeleteAllNotifications = new UnityEvent();

        PlayerOOCStateManager.OnStateChangeToWalking.AddListener(startSpawningNotifications);
        PlayerOOCStateManager.OnLeavingTutorialSequenceState.AddListener(startSpawningNotifications);
        AreaManager.OnAreaSpawn.AddListener(spawnNotificationsOnAreaChange);

        notificationQueue = new ArrayList();
        
        notificationPopUpButton = null;
        instance = null;
        skipNextSpawn = false;
    }

    public void Awake()
    {
        instance = this;
    }

}
