using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStepActivationScript : PlayerInteractionScript
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

#region Tabor Tutorial Scripts

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

#endregion

#region Kastor Tutorial Scripts

public class KastorIntimidateTutorialScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        Flags.setFlag(FlagNameList.finishedKastorIntimidateTutorial, true);
        TutorialFlags.setFlag(TutorialSequenceList.intimidateTutorialSeenFlag, true);

        PartyStats.resetAllSkills();

        QuestList.activateQuestStep(QuestNameList.saveDibberQuestTitle, QuestNameList.saveDibberStepTitleTwo);
    }
}

public class KastorCunningTutorialScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        if(!Flags.getFlag(FlagNameList.finishedKastorCunningTutorial))
        {
            Flags.setFlag(FlagNameList.finishedKastorCunningTutorial, true);
            TutorialFlags.setFlag(TutorialSequenceList.secondCunningTutorialSeenFlag, true);

            QuestList.activateQuestStep(QuestNameList.saveDibberQuestTitle, QuestNameList.saveDibberStepTitleFour);
            NotificationManager.ManualNotificationSpawn.Invoke();
        }
    }
}

public class KastorLeadershipTutorialScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        if(!Flags.getFlag(FlagNameList.finishedKastorLeadershipTutorial))
        {
            Flags.setFlag(FlagNameList.finishedKastorLeadershipTutorial, true);
            TutorialFlags.setFlag(TutorialSequenceList.leadershipTutorialSeenFlag, true);

            NotificationManager.preventSkipNextNotificationSpawn();

            QuestList.activateQuestStep(QuestNameList.saveDibberQuestTitle, QuestNameList.saveDibberStepTitleSix);

            NotificationManager.ManualNotificationSpawn.Invoke();
        }
    }
}

public class KastorObservationTutorialScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        if(!Flags.getFlag(FlagNameList.finishedKastorObservationTutorial))
        {
            Flags.setFlag(FlagNameList.finishedKastorObservationTutorial, true);
            TutorialFlags.setFlag(TutorialSequenceList.observationTutorialSeenFlag, true);

            NotificationManager.preventSkipNextNotificationSpawn();

            QuestList.activateQuestStep(QuestNameList.saveDibberQuestTitle, QuestNameList.saveDibberStepTitleEight);
            NotificationManager.ManualNotificationSpawn.Invoke();
        }
    }
}

#endregion


public class ThiefsBodyScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        PlayerObject.getInstance().StartCoroutine(waitForWalkingThenStartDialogue());
    }

    private IEnumerator waitForWalkingThenStartDialogue()
    {

        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;

        while (PlayerOOCStateManager.currentActivity != OOCActivity.walking)
        {
            yield return null;
        }

        PlayerOOCStateManager.setCurrentActivity(OOCActivity.inDialogue);
        DialogueManager.getInstance().startDialogue(DialogueList.getDialogue(LocationNameList.bodyPile, NPCNameList.body + 1));
    }
}

public class KilledMineLvlThreeGuardsB4BreachScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {

        if(Flags.getFlag(FlagNameList.sentIntoMineByDirector))
        {
            if(!Flags.getFlag(FlagNameList.trainedByEmeseToUseBlasingJelly))
            {
                QuestList.activateQuestStep(QuestNameList.noGoodDeedQuestTitle, QuestNameList.noGoodDeedStepTitleSeven);

                if(Flags.getFlag(FlagNameList.mineLvl3ToldAboutJelly))
                {
                    QuestList.activateQuestStep(QuestNameList.sealingTheBreachQuestTitle, QuestNameList.sealingTheBreachStepTitleTwo);
                } 
            } else
            {
                QuestList.activateQuestStep(QuestNameList.noGoodDeedQuestTitle, QuestNameList.noGoodDeedStepTitleSix);

                if(Flags.getFlag(FlagNameList.mineLvl3ToldAboutJelly))
                {
                    QuestList.activateQuestStep(QuestNameList.sealingTheBreachQuestTitle, QuestNameList.sealingTheBreachStepTitleFive);
                }
            }
        } else if(Flags.getFlag(FlagNameList.mineLvl3ToldAboutJelly))
        {
            QuestList.activateQuestStep(QuestNameList.sealingTheBreachQuestTitle, QuestNameList.sealingTheBreachStepTitleTwo);
        }
    }
}

public abstract class AfterBreachScript : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        if(Flags.getFlag(FlagNameList.playerSealedBreachThemself))
        {
            QuestList.finishQuest(QuestNameList.sealingTheBreachQuestTitle, QuestNameList.sealingTheBreachStepTitleSix, true);

        } else if(Flags.getFlag(FlagNameList.mineLvl3MarcosDiedSealingBreach))
        {
            QuestList.finishQuest(QuestNameList.sealingTheBreachQuestTitle, QuestNameList.sealingTheBreachStepTitleSeven, true);
            
        } else if(Flags.getFlag(FlagNameList.guardsSealedBreach))
        {
            QuestList.finishQuest(QuestNameList.sealingTheBreachQuestTitle, QuestNameList.sealingTheBreachStepTitleEight, true);
        }

        if(Flags.getFlag(FlagNameList.sentIntoMineByDirector))
        {
            QuestList.activateQuestStep(QuestNameList.noGoodDeedQuestTitle, QuestNameList.noGoodDeedStepTitleEight);
        } 
    }
}

public class KilledMineLvlThreeGuardsAfterBreachScript : AfterBreachScript
{
    public override void runScript(GameObject target = null)
    {
        base.runScript(target);
    }
}

public class KilledNandorCarterAfterBreachScript : AfterBreachScript
{
    public override void runScript(GameObject target = null)
    {
        base.runScript(target);

    }
}