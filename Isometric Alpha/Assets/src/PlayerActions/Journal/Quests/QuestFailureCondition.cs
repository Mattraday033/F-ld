using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestFailureCondition
{
    public abstract bool causesFailure(object o);
    public abstract string getFailureQuestStepName();
}

public abstract class QuestFailureNameCheckCondition : QuestFailureCondition
{
    protected string nameToCheck;

    public QuestFailureNameCheckCondition(string nameToCheck)
    {
        this.nameToCheck = nameToCheck;
    }

    public override bool causesFailure(object o)
    {
        return nameToCheck.Equals(getNameOfCause(o));
    }

    protected abstract string getNameOfCause(object o);

}

public class AreaHostilityFailureCondition : QuestFailureNameCheckCondition
{
    public const string areaHostileQuestStepName = "Area Hostile";
    public AreaHostilityFailureCondition(string areaName):
    base(areaName)
    {
        
    }

    protected override string getNameOfCause(object o)
    {
        if(o as Area == null)
        {
            return "";
        }

        return (o as Area).areaKey;
    }

    public override string getFailureQuestStepName()
    {
        return areaHostileQuestStepName;
    }
}

public class CharacterDeathFailureCondition : QuestFailureNameCheckCondition
{
    public const string questStepNameSuffix = " has died.";

    public CharacterDeathFailureCondition(string characterName):
    base(characterName)
    {
        
    }

    protected override string getNameOfCause(object o)
    {
        if(o as string == null)
        {
            return "";
        }

        return o as string;
    }

    public override string getFailureQuestStepName()
    {
        return nameToCheck + questStepNameSuffix;
    }
}