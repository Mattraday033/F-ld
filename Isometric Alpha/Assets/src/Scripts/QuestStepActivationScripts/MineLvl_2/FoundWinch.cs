using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FoundWinch : QuestStepActivationScript
{
    private const string questName = "Explore the Mine";

    private const int nextQuestStepIndex = 4;

    public override void runScript()
    {
        QuestList.activateQuestStep(questName, nextQuestStepIndex);
    }

}
