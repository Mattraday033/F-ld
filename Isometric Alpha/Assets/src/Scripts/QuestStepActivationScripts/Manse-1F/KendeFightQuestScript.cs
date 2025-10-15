using UnityEngine;

public class KendeFightQuestScript : QuestStepActivationScript
{
    private const string questName = "Assist the Nonbranded";

    private const bool questSucceeded = true;
    private const int imreBetrayalQuestStepIndex = 5;
    private const int imreLiberatedQuestStepIndex = 4;

    public override void runScript()
    {


        if (Flags.getFlag(FlagNameList.convincedImre))
        {
            QuestList.finishQuest(questName, imreLiberatedQuestStepIndex, questSucceeded);
        }
        else if (!Flags.getFlag(FlagNameList.convincedImre) && Flags.getFlag(FlagNameList.terrifiedImre))
        {
            QuestList.finishQuest(questName, imreBetrayalQuestStepIndex, questSucceeded);
        }
        else
        {
            Debug.LogError("KendeFightQuestScript ran but quest step was activated.");
        }

    }

}
