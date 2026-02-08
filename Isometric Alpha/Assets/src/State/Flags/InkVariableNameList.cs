using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public static class InkVariableNameList
{
    public const string playerName = "playerName";

    public const string strengthVarName = "strength";
    public const string dexterityVarName = "dexterity";
    public const string wisdomVarName = "wisdom";
    public const string charismaVarName = "charisma";

    public const string strDiffVarName = "strDifficulty";
    public const string dexDiffVarName = "dexDifficulty";
    public const string wisDiffVarName = "wisDifficulty";
    public const string chaDiffVarName = "chaDifficulty";

    public const string secretDoorKey = "secretDoorKey";

    public const string facingNE = "facingNE";
    public const string facingNW = "facingNW";
    public const string facingSW = "facingSW";
    public const string facingSE = "facingSE";

    public const string explanation = "explanation";
    public const string gateKey = "gateKey";

    public const string description = "description";
    public const string destinationName = "destinationName";

    public const string keyName = "keyName";

    public const string hostileAreaName = "hostileAreaName";
    public const string hostilityScriptKey = "hostilityScriptKey";

    public const string plural = "plural";
    public const string objectName = "objectName";
    public const string size = "size";

    public const string attitude = "attitude";

    public const string defeatFlag = "defeatFlag";

    public static Story setStoryVariable(Story story, string variableName, int value)
    {
        if (story.variablesState[variableName] != null)
        {
            story.variablesState[variableName] = value;
        }
        
        return story;
    }

    public static Story setStoryVariable(Story story, string variableName, bool value)
    {
        if (story.variablesState[variableName] != null)
        {
            story.variablesState[variableName] = value;
        }
        
        return story;
    }

    public static Story setStoryVariable(Story story, string variableName, string value)
    {
        if (story.variablesState[variableName] != null)
        {
            story.variablesState[variableName] = value;
        }
        
        return story;
    }
}
