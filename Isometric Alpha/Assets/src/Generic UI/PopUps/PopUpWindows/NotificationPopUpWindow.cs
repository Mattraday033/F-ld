using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopUpWindow : PageReaderPopUpWindow 
{
    private static NotificationPopUpWindow instance;

    public List<IDescribable> notificationQueue;
    public static bool pagesFadeIn;

    public static NotificationPopUpWindow getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(instance.gameObject);
        }

        instance = this;
        pagesFadeIn = false;
        readInNotifications();
    }

    private void readInNotifications()
    {
        List<ISortable> sortableNotificationQueue = new List<ISortable>();
        List<IDescribable> nonsortableNotificationQueue = new List<IDescribable>();
        notificationQueue = new List<IDescribable>();

        foreach (IDescribable notification in NotificationManager.notificationQueue)
        {
            if(notification as ISortable != null)
            {
                sortableNotificationQueue.Add(notification as ISortable);
            } else
            {
                nonsortableNotificationQueue.Add(notification);
            }
        }

        sortableNotificationQueue.Sort(ComparerList.getComparer(SortBy.Number));

        notificationQueue.AddRange(sortableNotificationQueue);
        notificationQueue.AddRange(nonsortableNotificationQueue);

        NotificationManager.purgeNotifications();
    }

    private void OnDestroy()
    {
        if (popupProgenitor != null)
        {
            NotificationManager.OnDeleteAllNotifications.RemoveListener(popupProgenitor.destroyPopUp);
        }
    }

	public override void closeButtonPress()
	{
		popupProgenitor.destroyPopUp();
	}

    private IDescribable getCurrentNotificationDescribable()
    {
        return notificationQueue[getCurrentPageNum()];
    }

    public override GameObject getCurrentPageObject()
    {
        IDescribable describable = getCurrentNotificationDescribable();

        return describable.getDescriptionPanelFull(PanelType.Notification);
    }

    public override GameObject getNextPageObject()
    {
        if (getCurrentPageNum() + 1 >= notificationQueue.Count)
        {
            return null;
        }

        IDescribable describable = notificationQueue[getCurrentPageNum() + 1];

        return describable.getDescriptionPanelFull(PanelType.Notification);
    }
    
    public override void instantiatePage()
    {
        destroyCurrentPage();

        GameObject pageGameObject = Instantiate(getCurrentPageObject(), pageParent);

        pagesFadeIn = false;

        DescriptionPanel descriptionPanel = pageGameObject.GetComponent<DescriptionPanel>();

        getCurrentNotificationDescribable().describeSelfFull(descriptionPanel);

        setCurrentPageObject(pageGameObject);
    }

}
