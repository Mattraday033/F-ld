using UnityEngine;

public class KendeFightQuestScript : QuestStepActivationScript
{
    private const bool questSucceeded = true;

    public override void runScript()
    {
        if (Flags.getFlag(FlagNameList.convincedImre))
        {
            QuestList.finishQuest(QuestNameList.assistTheNonbrandedQuestTitle, QuestNameList.assistTheNonbrandedStepTitleFour, questSucceeded);
        }
        else if (!Flags.getFlag(FlagNameList.convincedImre) && Flags.getFlag(FlagNameList.terrifiedImre))
        {
            QuestList.finishQuest(QuestNameList.assistTheNonbrandedQuestTitle, QuestNameList.assistTheNonbrandedStepTitleFive, questSucceeded);
        }
        else
        {
            Debug.LogError("KendeFightQuestScript ran but no quest step was activated.");
        }
    }

}
