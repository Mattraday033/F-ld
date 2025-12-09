using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NotificationPopUpButton : PopUpButton
{
    public NotificationPopUpButton() :
    base(PopUpType.Notification)
    {

    }

    public override GameObject getCurrentPopUpGameObject()
    {
        if (NotificationPopUpWindow.getInstance() != null && !(NotificationPopUpWindow.getInstance() is null))
        {
            return NotificationPopUpWindow.getInstance().gameObject;
        }
        else
        {
            return null;
        }
    }

    public override void spawnPopUp()
    {
        StartCoroutine(spawnPopUpWhenParentExists());
    }

    private IEnumerator spawnPopUpWhenParentExists()
    {
        while(OverallUIManager.notificationParent == null)
        {
            yield return null;
        }

        GameObject notificationPopUpGameObject = Instantiate(Resources.Load<GameObject>(getPopUpPrefabName(type)), OverallUIManager.notificationParent);

        if(notificationPopUpGameObject == null || getCurrentPopUpGameObject() == null)
        {
            yield break;
        }

        setPopUpWindow(getCurrentPopUpGameObject().GetComponent<PopUpWindow>());

        getPopUpWindow().setProgenitor(this);

        NotificationManager.OnDeleteAllNotifications.AddListener(destroyPopUp);

        NotificationPopUpWindow window = (NotificationPopUpWindow)getPopUpWindow();

        window.populate();

        NotificationManager.purgeNotifications();

        //EscapeStack.addEscapableObject(getPopUpWindow());
    }

    public override void destroyPopUp()
    {
        if (getCurrentPopUpGameObject() != null && !(getCurrentPopUpGameObject() is null))
        {
            DestroyImmediate(getCurrentPopUpGameObject());
        }

        NotificationManager.purgeNotifications();
	}
}
