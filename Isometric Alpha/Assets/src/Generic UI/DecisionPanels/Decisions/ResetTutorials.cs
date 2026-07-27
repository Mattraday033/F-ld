using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class ResetTutorials : IDecision
{
    private const string resetTutorialsMessage = "Are you sure you want to reset all Tutorials? This cannot be undone.";

    public ResetTutorials()
    {
    }

    public string getMessage()
    {
        return resetTutorialsMessage;
    }

    public void execute()
    {
        TutorialFlags.resetFlags();
    }

    public void backOut()
    {

    }
}
