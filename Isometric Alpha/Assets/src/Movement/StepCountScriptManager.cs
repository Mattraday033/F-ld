using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StepCountScriptManager
{

    public static int stepCount;
    public static List<StepCountScript> stepCountScripts;

    public static void addStepCountScript(StepCountScript script)
    {
        script.startStepCounter();
        stepCountScripts.Add(script);
    }

    public static void incrementStepCount()
    {
        if (stepCountScripts.Count <= 0 && stepCount > 1000000)
        {
            resetStepCounter();
        }
        else
        {
            stepCount++;
            activateAllScriptsWaitingOnCurrentStep();
        }
    }

    public static void activateAllScriptsWaitingOnCurrentStep()
    {
        if (stepCountScripts.Count <= 0)
        {
            return;
        }

        for (int scriptIndex = 0; scriptIndex < stepCountScripts.Count; scriptIndex++)
        {
            if (stepCount == stepCountScripts[scriptIndex].getActivationStep())
            {
                stepCountScripts[scriptIndex].runScript();
                stepCountScripts.RemoveAt(scriptIndex);
                scriptIndex--;
            }
        }
    }

    public static void reset()
    {
        purgeStepCountScripts();
        resetStepCounter();
    }

    private static void purgeStepCountScripts()
    {
        stepCountScripts = new List<StepCountScript>();
    }

    private static void resetStepCounter()
    {
        stepCount = 0;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void init()
    {
        stepCount = 0;
        stepCountScripts = new List<StepCountScript>();
        
        LoadSaveFile.OnLoadResetData.AddListener(resetStepCounter);
    }

}
