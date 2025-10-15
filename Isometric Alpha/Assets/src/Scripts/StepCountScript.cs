using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepCountScript : PlayerInteractionScript
{

    private int stepsToWait;
    private int activationStep;

    public StepCountScript(int stepsToWait)
    {
        this.stepsToWait = stepsToWait;
    }

    public int getActivationStep()
    {
        return activationStep;
    }

    public virtual void startStepCounter()
    {
        activationStep = StepCountScriptManager.stepCount + stepsToWait;

    }

}
