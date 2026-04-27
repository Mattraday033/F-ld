using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStepActivationScript: PlayerInteractionScript
{
    public override void runScript(GameObject target = null)
    {
        //empty on purpose
    }

}

public class PreventTutorialsAfterBatsKilledScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        Flags.setFlag(TutorialSequenceList.firstHostilityTutorialSeenFlag, true);
        TutorialFlags.setFlag(TutorialSequenceList.firstHostilityTutorialSeenFlag, true);

        if(SecretDoorFlags.secretDoorHasBeenDiscovered(SecretDoorKeyList.wisTutorialSecretDoor))
        {
            Flags.setFlag(TutorialSequenceList.observationTutorialSeenFlag, true);
        }
    }
}

public class TaborIntimidateTutorialScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        Flags.setFlag(FlagNameList.finishedTaborIntimidateTutorial, true);
        TutorialFlags.setFlag(TutorialSequenceList.intimidateTutorialSeenFlag, true);

        QuestList.activateQuestStep(QuestNameList.chiefTaborQuestTitle, QuestNameList.chiefTaborStepTitleFour);
    }
}

public class TaborCunningTutorialScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        if(!Flags.getFlag(FlagNameList.finishedTaborCunningTutorial))
        {
            Flags.setFlag(FlagNameList.finishedTaborCunningTutorial, true);
            TutorialFlags.setFlag(TutorialSequenceList.secondCunningTutorialSeenFlag, true);

            QuestList.activateQuestStep(QuestNameList.chiefTaborQuestTitle, QuestNameList.chiefTaborStepTitleSix);
            NotificationManager.ManualNotificationSpawn.Invoke();
        }
    }
}

public class TaborLeadershipTutorialScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        if(!Flags.getFlag(FlagNameList.finishedTaborLeadershipTutorial))
        {
            Flags.setFlag(FlagNameList.finishedTaborLeadershipTutorial, true);
            TutorialFlags.setFlag(TutorialSequenceList.leadershipTutorialSeenFlag, true);

            NotificationManager.preventSkipNextNotificationSpawn();

            QuestList.activateQuestStep(QuestNameList.chiefTaborQuestTitle, QuestNameList.chiefTaborStepTitleEight);

            NotificationManager.ManualNotificationSpawn.Invoke();
        }
    }
}

public class TaborObservationTutorialScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {

        if(!Flags.getFlag(FlagNameList.finishedTaborObservationTutorial))
        {
            Flags.setFlag(FlagNameList.finishedTaborObservationTutorial, true);
            TutorialFlags.setFlag(TutorialSequenceList.observationTutorialSeenFlag, true);

            NotificationManager.preventSkipNextNotificationSpawn();

            QuestList.activateQuestStep(QuestNameList.chiefTaborQuestTitle, QuestNameList.chiefTaborStepTitleTen);
            NotificationManager.ManualNotificationSpawn.Invoke();
        }
    }
}