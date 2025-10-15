using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialSequenceInput
{

	public static void handleCombatTutorialInput()
	{
		if (TutorialSequence.shouldAdvanceCurrentTutorialSequence() && !KeyPressManager.handlingPrimaryKeyPress)
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			TutorialSequence.advanceCurrentTutorialSequence();
			return;
		}

		if (TutorialSequence.additionalScriptButtonPressed() && !KeyPressManager.handlingPrimaryKeyPress)
		{
			KeyPressManager.handlingPrimaryKeyPress = true;
			TutorialSequence.runAdditionalScripts();
			return;
		}

		if (KeyBindingList.skipTutorialKeysArePressed())
		{
			KeyPressManager.handlingPrimaryKeyPress = true;

			if (TutorialSequence.currentTutorialSequence.isSkippable())
			{
				TutorialSequence.currentTutorialSequence.skipTutorial();
			}
		}
    }

}
