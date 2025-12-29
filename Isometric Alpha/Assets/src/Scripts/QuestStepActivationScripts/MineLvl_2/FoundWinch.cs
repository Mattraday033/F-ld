using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundWinch : QuestStepActivationScript
{
    public override void runScript()
    {
            QuestList.activateQuestStep(QuestNameList.exploreTheMineQuestTitle, QuestNameList.exploreTheMineStepTitleFour);
    }

}
