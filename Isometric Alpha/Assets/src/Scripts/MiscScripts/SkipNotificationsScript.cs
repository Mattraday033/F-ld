using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipNotificationsScript : TutorialSequenceStepScript
{
    public override void runScript(GameObject target = null)
    {
        NotificationManager.skipNextNotificationSpawn();
    }
}
