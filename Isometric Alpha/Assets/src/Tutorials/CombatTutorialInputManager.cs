using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTutorialInputManager : MonoBehaviour
{    // Update is called once per frame
    void Update()
    {
        if (TutorialSequence.currentlyInTutorialSequence())
        {
            KeyPressManager.updateKeyBools();

            TutorialSequenceInput.handleCombatTutorialInput();
        }
        else
        {
            this.enabled = false;
        }
    }
}
