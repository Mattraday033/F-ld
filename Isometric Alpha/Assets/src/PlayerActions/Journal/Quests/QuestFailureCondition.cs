using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestFailureCondition
{
    public abstract bool causesFailure(object o);
    public abstract string getFailureQuestStepName();
}

public class AreaHostilityFailureCondition : QuestFailureCondition
{
    private const string areaHostileQuestStepName = "Area Hostile";
    private string areaName;

    public AreaHostilityFailureCondition(string areaName)
    {
        this.areaName = areaName;
    }

    public override bool causesFailure(object o)
    {
        Area area = o as Area;

        if(area != null && area.areaKey.Equals(this.areaName))
        {
            return true;
        }

        return false;
    }

    public override string getFailureQuestStepName()
    {
        return areaHostileQuestStepName;
    }
}