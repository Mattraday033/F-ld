using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialTriggerCollider : MonoBehaviour
{
    //[SerializeField]
    private string tutorialSequenceKey;
    //[SerializeField]
    private TutorialSequence tutorialSequence;

    public TutorialSequence getTutorialSequence()
    {
        if (tutorialSequenceKey != null && tutorialSequenceKey.Length > 0)
        {
            return TutorialSequenceList.getTutorialSequence(tutorialSequenceKey);
        }
        else
        {
            return tutorialSequence;
        }
    }
}
