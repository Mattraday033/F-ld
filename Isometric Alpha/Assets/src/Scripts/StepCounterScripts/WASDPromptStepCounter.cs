using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDPromptStepCounter : StepCountScript
{

    public const int standardStepsToWait = 4;

    public WASDPromptStepCounter(int stepsToWait) :
    base(stepsToWait)
    {

    }

    public override void runScript(GameObject target = null)
    {
        PlayerObject.hasCustomPromptMessage = false;
    }

    public static void createStepCounter()
    {
        StepCountScriptManager.addStepCountScript(new WASDPromptStepCounter(standardStepsToWait));
    }

}
