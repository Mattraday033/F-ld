using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundWinch : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
            QuestList.activateQuestStep(QuestNameList.exploreTheMineQuestTitle, QuestNameList.exploreTheMineStepTitleFour);
    }

}

public class FoundToolBundle : QuestStepActivationScript
{
    public override void runScript(GameObject target = null)
    {
        if(Flags.getFlag(FlagNameList.toldToFindTools) && 
            !(Flags.getFlag(FlagNameList.hasToolBundle) || 
                Flags.getFlag(FlagNameList.gaveKastorToolBundle) || 
                Flags.getFlag(FlagNameList.convincedSlavesToHelpYou)))
        {
            QuestList.activateQuestStep(QuestNameList.thePlanQuestTitle, QuestNameList.thePlanStepTitleNine);
            Flags.setFlag(FlagNameList.hasToolBundle, true);
        }
    }

}
