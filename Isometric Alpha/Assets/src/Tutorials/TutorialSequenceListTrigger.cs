using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSequenceListTrigger : MonoBehaviour
{

    //[SerializeField]
    public TutorialSequence[] tutorialSequences;

    public TutorialSequence getTutorialSequence(int index)
    {
        if (index >= 0 && index < tutorialSequences.Length)
        {
            return tutorialSequences[index];
        }
        else if (tutorialSequences.Length > 0)
        {
            return tutorialSequences[0];
        }
        else
        {
            return null;
        }
    }
}
